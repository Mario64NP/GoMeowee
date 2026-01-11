namespace GoMeowee.Models;

public class EventDetailsDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime StartsAt { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Category {  get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int InterestedCount { get; set; }
    public string Description {  get; set; } = string.Empty;
}
