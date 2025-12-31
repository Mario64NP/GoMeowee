namespace GoMeowee.Models.Auth;

public sealed class LoginResult
{
    public Guid UserId { get; init; }
    public string Username { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public string? AvatarUrl {  get; init; }
    public string Token { get; init; } = string.Empty;
}
