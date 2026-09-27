using Microsoft.EntityFrameworkCore;

public class ActivityService : IActivityService
{
    private readonly TrackingDbContext _dbContext;

    public ActivityService(TrackingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<Activity>> GetAllActivitiesAsync() =>
        _dbContext.Activities
            .OrderByDescending(a => a.Date)
            .ThenByDescending(a => a.CreatedAt)
            .ToListAsync();

    public async Task<Activity> GetActivityAsync(Guid id)
    {
        var activity = await _dbContext.Activities.FindAsync(id);
        if (activity is null)
        {
            throw new KeyNotFoundException("Activity was not found.");
        }

        return activity;
    }

    public async Task<Activity> CreateAsync(CreateActivityRequest request)
    {
        var title = request.Title?.Trim();
        if (string.IsNullOrEmpty(title))
        {
            throw new ArgumentException("A title is required.");
        }

        if (request.Date == default)
        {
            throw new ArgumentException("A date is required.");
        }

        var sport = ParseSport(request.Sport);
        if (request.DurationMinutes is <= 0)
        {
            throw new ArgumentException("Duration must be a positive number of minutes.");
        }

        if (request.DistanceKilometers is <= 0)
        {
            throw new ArgumentException("Distance must be a positive number of kilometers.");
        }

        var now = DateTime.UtcNow;
        var activity = new Activity
        {
            Id = Guid.NewGuid(),
            Date = request.Date,
            Title = title,
            Notes = string.IsNullOrWhiteSpace(request.Notes) ? null : request.Notes.Trim(),
            Sport = sport,
            DurationMinutes = request.DurationMinutes,
            DistanceKilometers = request.DistanceKilometers,
            CreatedAt = now,
            UpdatedAt = now
        };

        _dbContext.Activities.Add(activity);
        await _dbContext.SaveChangesAsync();
        return activity;
    }

    public async Task UpdateAsync(Guid id, UpdateActivityRequest request)
    {
        var title = request.Title?.Trim();
        if (string.IsNullOrEmpty(title))
        {
            throw new ArgumentException("A title is required.");
        }

        if (request.Date == default)
        {
            throw new ArgumentException("A date is required.");
        }

        var sport = ParseSport(request.Sport);
        if (request.DurationMinutes is <= 0)
        {
            throw new ArgumentException("Duration must be a positive number of minutes.");
        }

        if (request.DistanceKilometers is <= 0)
        {
            throw new ArgumentException("Distance must be a positive number of kilometers.");
        }

        var activity = await GetActivityAsync(id);

        activity.Date = request.Date;
        activity.Title = title;
        activity.Notes = string.IsNullOrWhiteSpace(request.Notes) ? null : request.Notes.Trim();
        activity.Sport = sport;
        activity.DurationMinutes = request.DurationMinutes;
        activity.DistanceKilometers = request.DistanceKilometers;
        activity.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
    }

    private static ActivityType ParseSport(string? sport)
    {
        if (string.IsNullOrWhiteSpace(sport))
        {
            throw new ArgumentException("A sport is required.");
        }

        var name = sport.Trim();
        if (!Enum.TryParse(name, ignoreCase: false, out ActivityType parsed) || parsed.ToString() != name)
        {
            throw new ArgumentException("Sport must be Run, RoadRide, Swim, Strength, or Other.");
        }

        return parsed;
    }

    public async Task DeleteAsync(Guid id)
    {
        var activity = await GetActivityAsync(id);
        _dbContext.Activities.Remove(activity);
        await _dbContext.SaveChangesAsync();
    }
}
