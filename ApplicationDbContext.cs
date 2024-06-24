using Microsoft.EntityFrameworkCore;
using OrderApplication.Models;

namespace OrderApplication
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public virtual DbSet<OrderModel> Orders { get; set; }
        public virtual DbSet<ProductOrderModel> ProductOrders { get; set; }
    }
}
