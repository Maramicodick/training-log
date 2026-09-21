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

    [Fact]
    public async Task CreateAsync_EmptyTitle_Throws()
    {
        var service = CreateService();
        var request = new CreateActivityRequest { Date = new DateOnly(2026, 1, 1), Title = "   " };

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(request));
    }

    [Fact]
    public async Task CreateAsync_MissingDate_Throws()
    {
        var service = CreateService();
        var request = new CreateActivityRequest { Title = "Run" };

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
            Notes = "  z2  "
        });

        var loaded = await service.GetActivityAsync(created.Id);
        Assert.Equal("Easy run", loaded.Title);
        Assert.Equal("z2", loaded.Notes);
        Assert.Equal(new DateOnly(2026, 1, 2), loaded.Date);
    }

    [Fact]
    public async Task GetAllActivitiesAsync_OrdersByDateThenCreatedAt()
    {
        var service = CreateService();
        var day = new DateOnly(2026, 3, 1);

        await service.CreateAsync(new CreateActivityRequest { Date = day, Title = "first-same-day" });
        await service.CreateAsync(new CreateActivityRequest { Date = day.AddDays(-1), Title = "older-day" });
        await service.CreateAsync(new CreateActivityRequest { Date = day, Title = "second-same-day" });

        var list = await service.GetAllActivitiesAsync();

        Assert.Equal(new[] { "second-same-day", "first-same-day", "older-day" }, list.Select(a => a.Title));
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
            Title = "Old"
        });

        await service.UpdateAsync(created.Id, new UpdateActivityRequest
        {
            Date = new DateOnly(2026, 1, 5),
            Title = "New",
            Notes = "note"
        });

        var loaded = await service.GetActivityAsync(created.Id);
        Assert.Equal("New", loaded.Title);
        Assert.Equal("note", loaded.Notes);
        Assert.Equal(new DateOnly(2026, 1, 5), loaded.Date);
    }

    [Fact]
    public async Task DeleteAsync_RemovesActivity()
    {
        var service = CreateService();
        var created = await service.CreateAsync(new CreateActivityRequest
        {
            Date = new DateOnly(2026, 1, 1),
            Title = "Gone"
        });

        await service.DeleteAsync(created.Id);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetActivityAsync(created.Id));
    }
}
