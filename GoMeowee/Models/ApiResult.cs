namespace GoMeowee.Models;

public sealed class ApiResult<T>
{
    public bool IsSuccess { get; set; } = true;
    public string? ApiError { get; set; } = null;
    public T? Response { get; set; } = default;
}
