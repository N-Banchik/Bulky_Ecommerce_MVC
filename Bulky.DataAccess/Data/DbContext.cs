using Bulky.DataAccess.EntityConfigs;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Bulky.DataAccess.Data
{

    public class DataContext : IdentityDbContext<IdentityUser>
    {
        private readonly IConfiguration _configuration;

        public DataContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CategoryConfig());
            modelBuilder.ApplyConfiguration(new ProductConfig());
            modelBuilder.ApplyConfiguration(new CompanyConfig());
            modelBuilder.ApplyConfiguration(new ShoppingCartConfig());
            modelBuilder.ApplyConfiguration(new OrderHeadersConfig());
            modelBuilder.ApplyConfiguration(new OrderDetailsConfig());


        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Company> Companis { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }

    }

}