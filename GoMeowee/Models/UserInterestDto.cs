namespace GoMeowee.Models;

public class UserInterestDto
{
    public string? AvatarUrl { get; set; }
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Message { get; set; }
    public DateTime InterestedAt { get; set; }
}
