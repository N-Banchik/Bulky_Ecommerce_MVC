using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess.Repository;
using Bulky.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;

namespace BulkyWeb.Startup
{
    public static class AddAppServices
    {
        public static IServiceCollection AddCustomizedServices(this IServiceCollection services, IConfiguration _config)
        {
            services.AddDbContext<DataContext>();
            services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            services.AddScoped<IOrderDetailsRepository, OrderDetailsRepository>();
            services.AddScoped<IOrderHeaderRepository, OrderHeaderRepository>();
            services.AddScoped<IAppUSerRepository,AppUserRepository>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.Configure<StripeSettings>(_config.GetSection("Stripe"));

            services.AddDistributedMemoryCache();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(100);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            return services;
        }
    }
}
