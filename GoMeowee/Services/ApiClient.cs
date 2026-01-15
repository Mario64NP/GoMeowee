using GoMeowee.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace GoMeowee.Services;

public class ApiClient(HttpClient httpClient)
{
    private static readonly JsonSerializerOptions _jsonOptions =
        new() { PropertyNameCaseInsensitive = true };

    public void SetToken(string? token)
    {
        httpClient.DefaultRequestHeaders.Authorization =
            string.IsNullOrWhiteSpace(token)
                ? null
                : new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<ApiResult<T>> PostAsync<T>(string url, object body)
    {
        var json = JsonSerializer.Serialize(body);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(url, content);
        var responseString = await response.Content.ReadAsStringAsync();

        var result = new ApiResult<T> { IsSuccess = response.IsSuccessStatusCode };

        if (response.IsSuccessStatusCode && response.StatusCode != System.Net.HttpStatusCode.NoContent)
            result.Response = JsonSerializer.Deserialize<T>(responseString, _jsonOptions);
        else
            result.ApiError = string.IsNullOrWhiteSpace(responseString) ? response.ReasonPhrase : responseString;

        return result;
    }

    public async Task<ApiResult<T>> GetAsync<T>(string url)
    {
        var response = await httpClient.GetAsync(url);
        var responseString = await response.Content.ReadAsStringAsync();

        var result = new ApiResult<T> { IsSuccess = response.IsSuccessStatusCode };

        if (response.IsSuccessStatusCode)
            result.Response = JsonSerializer.Deserialize<T>(responseString, _jsonOptions);
        else
            result.ApiError = responseString;

        return result;
    }

    public async Task<ApiResult<object>> DeleteAsync(string url)
    {
        var response = await httpClient.DeleteAsync(url);

        return new ApiResult<object> 
        { 
            IsSuccess = response.IsSuccessStatusCode, 
            ApiError = response.IsSuccessStatusCode ? null : await response.Content.ReadAsStringAsync()
        };
    }
}
