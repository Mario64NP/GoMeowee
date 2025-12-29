namespace GoMeowee.Models;

public class EventListItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime StartsAt { get; set; }
    public string Location { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int InterestedCount { get; set; }
}
