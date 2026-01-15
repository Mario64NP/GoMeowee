namespace GoMeowee.Models.Interests;

public class EventInterestDto
{
    public string? AvatarUrl { get; set; }
    public string? FullAvatarUrl { get; set; }
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Message { get; set; }
    public DateTime InterestedAt { get; set; }
    public string InterestedAtRelative { get; set; } = string.Empty;
}
