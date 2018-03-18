using Microsoft.EntityFrameworkCore;
using Paari.Infrastructure.Model;

namespace Paari.DataAccess
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
            
        }

        public DbSet<Product> ProductItems { get; set; }
    }
}
