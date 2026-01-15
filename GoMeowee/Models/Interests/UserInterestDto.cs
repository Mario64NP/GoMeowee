namespace GoMeowee.Models.Interests;

public class UserInterestDto
{
    public string? AvatarUrl { get; set; }
    public string? FullAvatarUrl { get; set; }
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Message { get; set; }
    public DateTime InterestedAt { get; set; }
    public string InterestedAtRelative { get; set; } = string.Empty;
    public Guid EventId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime StartsAt { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? FullImageUrl { get; set; }
    public int InterestedCount { get; set; }
    public bool HasImage => !string.IsNullOrEmpty(ImageUrl);
}
