using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TaskManager.Models;

namespace TaskManager.Data;

public class AppDBContext: DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkspaceModel>()
            .HasMany(w => w.Users)
            .WithMany(w => w.Workspaces)
            .UsingEntity("Workspaces_Users",
                l => l.HasOne(typeof(UserModel)).WithMany().HasForeignKey("EmailAddresses"),
                r => r.HasOne(typeof(WorkspaceModel)).WithMany().HasForeignKey("WorkspaceId"));
        modelBuilder.Entity<NotificationModel>()
            .HasMany(e => e.Receivers)
            .WithMany(e => e.Notifications);
        modelBuilder.Entity<UserModel>()
            .HasOne(e => e.Task)
            .WithMany(e => e.AppointedUsers)
            .HasForeignKey(e => e.TaskId)
            .IsRequired(false);

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<CategoryModel> Categories { get; set; }
    public DbSet<StatusModel> Statuses { get; set; }
    public DbSet<TaskModel> Tasks { get; set; }
    public DbSet<UserModel> Users { get; set; }
    public DbSet<WorkspaceModel> Workspaces { get; set; }
    public DbSet<NotificationModel> Notifications { get; set; }
}
