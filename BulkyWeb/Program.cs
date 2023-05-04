using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess.Seed;
using Bulky.Utility;
using BulkyWeb.Startup;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Stripe;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
});
builder.Services.AddCustomizedServices(builder.Configuration);
builder.Services.AddRazorPages();

var app = builder.Build();
using IServiceScope scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    DataContext context = services.GetRequiredService<DataContext>();
    RoleManager<IdentityRole> roleManager = services.GetService<RoleManager<IdentityRole>>()!;
    UserManager<IdentityUser> userManager = services.GetService<UserManager<IdentityUser>>()!;

    await context.Database.MigrateAsync();
    SeedData sd = new SeedData(context, roleManager,userManager);
    await sd.SeedDataOnStartup();
}
catch (System.Exception ex)
{
    ILogger logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migrations");
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
