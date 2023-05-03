using Ecommerce.Infrastructure.Domains;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ecommerce.Infrastructure
{
    public class EcommerceDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options)
        {
            try
            {
                var databaseCreater = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (databaseCreater != null)
                {
                    if (!databaseCreater.CanConnect())
                    {
                        databaseCreater.Create();
                    }
                    if (!databaseCreater.HasTables())
                    {
                        databaseCreater.CreateTables();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
    }
}