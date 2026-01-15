namespace GoMeowee.Models.Users;

public class UserDetailsDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Username { get; set; } = string.Empty;
    public string DisplayName {  get; set; } = string.Empty;
    public string? Bio {  get; set; }
    public IEnumerable<string> Tags { get; set; } = [];
    public string? AvatarUrl { get; set; }
}
