using Microsoft.EntityFrameworkCore;

namespace MerchShop.Models.Data;

public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Collection> Collections { get; set; }
}