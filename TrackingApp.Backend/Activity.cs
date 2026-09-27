public class Activity
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public string Title { get; set; }
    public string? Notes { get; set; }
    public ActivityType Sport { get; set; }
    public int? DurationMinutes { get; set; }
    public decimal? DistanceKilometers { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
