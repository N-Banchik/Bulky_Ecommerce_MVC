using Bulky.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Bulky.DataAccess.Data
{

    public class DtatContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DtatContext(IConfiguration configuration)
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
            modelBuilder.Entity<Category>().HasKey(c => c.Id);
            modelBuilder.Entity<Category>().Property(n => n.Name).IsRequired();
            modelBuilder.Entity<Category>().HasData(new[]
            {
            new Category() { Id=1,Name = "Action", DisplayOrder = 1 },
            new Category() { Id=2,Name = "Fiction", DisplayOrder = 2 },
            new Category() {Id = 3,Name = "Non-Fiction", DisplayOrder = 3 },
            new Category() {Id = 4,Name = "Science fiction", DisplayOrder = 4 }
        });


        }

        public DbSet<Category> Categories { get; set; }
    }

}