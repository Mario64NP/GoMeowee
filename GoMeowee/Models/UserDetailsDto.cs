namespace GoMeowee.Models;

public class UserDetailsDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Username { get; set; } = string.Empty;
    public string DisplayName {  get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
}
