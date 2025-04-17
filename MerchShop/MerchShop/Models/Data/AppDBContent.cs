using Microsoft.EntityFrameworkCore;

namespace MerchShop.Models.Data;

public class AppDBContent : DbContext
{
    public AppDBContent(DbContextOptions<AppDBContent> options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
}