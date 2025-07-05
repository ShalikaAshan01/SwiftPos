using Microsoft.EntityFrameworkCore;
using PointOfSales.Core.Entities.Infrastructure;
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
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupPermission> GroupPermissions { get; set; }
    public DbSet<UserGroup> UserGroups { get; set; }
    public DbSet<UserPermission> UserPermissions { get; set; }
    public DbSet<ActivityLog> ActivityLogs { get; set; }
    public DbSet<Device> Devices { get; set; }
}