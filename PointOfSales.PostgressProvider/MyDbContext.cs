using Microsoft.EntityFrameworkCore;
using PointOfSales.Core.Entities.Security;

namespace PointOfSales.PostgressProvider;

public class MyDbContext: DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<User> Users { get; set; }
}