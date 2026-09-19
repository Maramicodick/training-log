using Microsoft.EntityFrameworkCore;

public class TrackingDbContext(DbContextOptions<TrackingDbContext> options) : DbContext(options)
{
    public DbSet<Activity> Activities => Set<Activity>();
}
