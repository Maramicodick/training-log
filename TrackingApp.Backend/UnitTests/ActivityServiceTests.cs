using Microsoft.EntityFrameworkCore;

public class ActivityServiceTests
{
    private static ActivityService CreateService()
    {
        var options = new DbContextOptionsBuilder<TrackingDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ActivityService(new TrackingDbContext(options));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task CreateAsync_EmptyOrWhitespaceTitle_Throws(string title)
    {
        var service = CreateService();
        var request = new CreateActivityRequest { Date = new DateOnly(2026, 1, 1), Title = title, Sport = "Run" };

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(request));
    }

    [Fact]
    public async Task CreateAsync_MissingDate_Throws()
    {
        var service = CreateService();
        var request = new CreateActivityRequest { Title = "Run", Sport = "Run" };

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(request));
    }

    [Fact]
    public async Task CreateAsync_StoresActivity()
    {
        var service = CreateService();
        var created = await service.CreateAsync(new CreateActivityRequest
        {
            Date = new DateOnly(2026, 1, 2),
            Title = "  Easy run  ",
            Notes = "  z2  ",
            Sport = "Run"
        });

        var loaded = await service.GetActivityAsync(created.Id);
        Assert.Equal("Easy run", loaded.Title);
        Assert.Equal("z2", loaded.Notes);
        Assert.Equal(new DateOnly(2026, 1, 2), loaded.Date);
        Assert.Equal(ActivityType.Run, loaded.Sport);
        Assert.Null(loaded.DurationMinutes);
        Assert.Null(loaded.DistanceKilometers);
    }

    [Theory]
    [InlineData("Run", ActivityType.Run)]
    [InlineData("RoadRide", ActivityType.RoadRide)]
    public async Task CreateAsync_StoresSportDurationAndDistance(string sport, ActivityType expected)
    {
        var service = CreateService();
        var created = await service.CreateAsync(new CreateActivityRequest
        {
            Date = new DateOnly(2026, 1, 2),
            Title = "Session",
            Sport = sport,
            DurationMinutes = 45,
            DistanceKilometers = 8
        });

        var loaded = await service.GetActivityAsync(created.Id);
        Assert.Equal(expected, loaded.Sport);
        Assert.Equal(45, loaded.DurationMinutes);
        Assert.Equal(8, loaded.DistanceKilometers);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("Ride")]
    [InlineData("1")]
    public async Task CreateAsync_MissingOrUnknownSport_ThrowsAndDoesNotPersist(string? sport)
    {
        var service = CreateService();

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(new CreateActivityRequest
        {
            Date = new DateOnly(2026, 1, 2),
            Title = "Road",
            Sport = sport
        }));

        Assert.Empty(await service.GetAllActivitiesAsync());
    }

    [Theory]
    [InlineData(0, null)]
    [InlineData(null, 0)]
    public async Task CreateAsync_NonPositiveDurationOrDistance_ThrowsAndDoesNotPersist(int? duration, int? distance)
    {
        var service = CreateService();

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(new CreateActivityRequest
        {
            Date = new DateOnly(2026, 1, 2),
            Title = "Road",
            Sport = "Run",
            DurationMinutes = duration,
            DistanceKilometers = distance
        }));

        Assert.Empty(await service.GetAllActivitiesAsync());
    }

    [Fact]
    public async Task GetAllActivitiesAsync_OrdersByDateThenCreatedAt()
    {
        var service = CreateService();
        var day = new DateOnly(2026, 3, 1);

        await service.CreateAsync(new CreateActivityRequest { Date = day, Title = "first-same-day", Sport = "Run" });
        await service.CreateAsync(new CreateActivityRequest { Date = day.AddDays(-1), Title = "older-day", Sport = "Run" });
        await service.CreateAsync(new CreateActivityRequest { Date = day, Title = "second-same-day", Sport = "Run" });

        var list = await service.GetAllActivitiesAsync();

        Assert.Equal(new[] { "second-same-day", "first-same-day", "older-day" }, list.Select(a => a.Title));
    }

    [Fact]
    public async Task GetAllActivitiesAsync_KeepsDetailsAndDateOrder()
    {
        var service = CreateService();
        var newer = new DateOnly(2026, 4, 2);
        var older = newer.AddDays(-1);

        await service.CreateAsync(new CreateActivityRequest
        {
            Date = newer,
            Title = "with-details",
            Sport = "RoadRide",
            DurationMinutes = 90,
            DistanceKilometers = 40
        });
        await service.CreateAsync(new CreateActivityRequest
        {
            Date = older,
            Title = "sport-only",
            Sport = "Strength"
        });

        var list = await service.GetAllActivitiesAsync();

        Assert.Equal(new[] { "with-details", "sport-only" }, list.Select(a => a.Title));
        Assert.Equal(ActivityType.RoadRide, list[0].Sport);
        Assert.Equal(90, list[0].DurationMinutes);
        Assert.Equal(40, list[0].DistanceKilometers);
        Assert.Equal(ActivityType.Strength, list[1].Sport);
        Assert.Null(list[1].DurationMinutes);
        Assert.Null(list[1].DistanceKilometers);
    }

    [Fact]
    public async Task GetActivityAsync_UnknownId_Throws()
    {
        var service = CreateService();
        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetActivityAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task UpdateAsync_ChangesFields()
    {
        var service = CreateService();
        var created = await service.CreateAsync(new CreateActivityRequest
        {
            Date = new DateOnly(2026, 1, 1),
            Title = "Old",
            Sport = "Run"
        });

        await service.UpdateAsync(created.Id, new UpdateActivityRequest
        {
            Date = new DateOnly(2026, 1, 5),
            Title = "New",
            Notes = "note",
            Sport = "RoadRide"
        });

        var loaded = await service.GetActivityAsync(created.Id);
        Assert.Equal("New", loaded.Title);
        Assert.Equal("note", loaded.Notes);
        Assert.Equal(new DateOnly(2026, 1, 5), loaded.Date);
        Assert.Equal(ActivityType.RoadRide, loaded.Sport);
    }

    [Fact]
    public async Task DeleteAsync_RemovesActivity()
    {
        var service = CreateService();
        var created = await service.CreateAsync(new CreateActivityRequest
        {
            Date = new DateOnly(2026, 1, 1),
            Title = "Gone",
            Sport = "Run"
        });

        await service.DeleteAsync(created.Id);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetActivityAsync(created.Id));
    }
}
