public class UpdateActivityRequest
{
    public DateOnly Date { get; set; }
    public string Title { get; set; }
    public string? Notes { get; set; }
    public string? Sport { get; set; }
    public int? DurationMinutes { get; set; }
    public decimal? DistanceKilometers { get; set; }
}
