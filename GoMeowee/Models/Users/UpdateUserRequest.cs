namespace GoMeowee.Models.Users;

public class UpdateUserRequest
{
    public string? DisplayName { get; set; }
    public string? Bio {  get; set; }
    public IEnumerable<string>? Tags { get; set; }
}
