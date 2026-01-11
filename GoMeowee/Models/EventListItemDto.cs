namespace GoMeowee.Models;

public class EventListItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime StartsAt { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Category {  get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? FullImageUrl { get; set; }
    public bool HasImage => ImageUrl != null;
    public int InterestedCount { get; set; }
}
