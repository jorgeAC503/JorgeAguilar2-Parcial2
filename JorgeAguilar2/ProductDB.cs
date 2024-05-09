using Microsoft.EntityFrameworkCore;

namespace JorgeAguilar2
{
    public class ProductDB : DbContext
    {
        public ProductDB(DbContextOptions<ProductDB> options) : base(options) { }
        public DbSet<Product> Products => Set<Product>();
    }

}
