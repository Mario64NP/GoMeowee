using GoMeowee.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

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

    public async Task<ApiResult<T>> PatchAsync<T>(string url, object body)
    {
        var json = JsonSerializer.Serialize(body);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await httpClient.PatchAsync(url, content);
        var responseString = await response.Content.ReadAsStringAsync();

        var result = new ApiResult<T> { IsSuccess = response.IsSuccessStatusCode };

        if (response.IsSuccessStatusCode)
            result.Response = JsonSerializer.Deserialize<T>(responseString, _jsonOptions);
        else
            result.ApiError = string.IsNullOrWhiteSpace(responseString )? response.ReasonPhrase : responseString;

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

    public async Task<ApiResult<string>> PostPhotoAsync(string url, FileResult photo)
    {
        using Stream stream = await photo.OpenReadAsync();
        var streamContent = new StreamContent(stream);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue(photo.ContentType);

        var req = new MultipartFormDataContent();
        req.Add(streamContent, "file", photo.FileName);

        var response = await httpClient.PostAsync(url, req);
        var responseString = await response.Content.ReadAsStringAsync();

        var result = new ApiResult<string> { IsSuccess = response.IsSuccessStatusCode };

        if (response.IsSuccessStatusCode)
            result.Response = JsonNode.Parse(responseString)?["fileName"]?.ToString();
        else
            result.ApiError = string.IsNullOrWhiteSpace(responseString) ? response.ReasonPhrase : responseString;

        return result;
    }
}
