using Microsoft.EntityFrameworkCore;

public class TrackingDbContext(DbContextOptions<TrackingDbContext> options) : DbContext(options)
{
    public DbSet<Activity> Activities => Set<Activity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activity>()
            .Property(activity => activity.Sport)
            .HasConversion<string>()
            .HasMaxLength(32);

        modelBuilder.Entity<Activity>()
            .Property(activity => activity.DistanceKilometers)
            .HasPrecision(18, 2);
    }
}
