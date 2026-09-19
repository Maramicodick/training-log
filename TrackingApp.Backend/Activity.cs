public class Activity
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public string Title { get; set; }
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
