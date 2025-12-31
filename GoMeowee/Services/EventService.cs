using GoMeowee.Models;

namespace GoMeowee.Services;

public class EventsService(ApiClient apiClient)
{
    private readonly ApiClient _apiClient = apiClient;

    public async Task<List<EventListItemDto>?> GetEventsAsync()
    {
        var result = await _apiClient.GetAsync<List<EventListItemDto>>("api/Events");

        if (!result.IsSuccess)
            return null;

        return result.Response;
    }

    public async Task<EventDetailsDto?> GetEventByIdAsync(Guid id)
    {
        var result = await _apiClient.GetAsync<EventDetailsDto>($"api/Events/{id}");

        if (!result.IsSuccess)
            return null;

        return result.Response;
    }

    public async Task<bool> GetEventInterestByIdAsync(Guid id)
    {
        var result = await _apiClient.GetAsync<bool>($"api/Events/{id}/interest");

        if (!result.IsSuccess)
            return false;

        return result.Response;
    }

    public async Task<IEnumerable<UserInterestDto>?> GetInterestedPeopleAsync(Guid id)
    {
        var result = await _apiClient.GetAsync<IEnumerable<UserInterestDto>>($"api/Events/{id}/interests");

        if (!result.IsSuccess)
            return null;

        return result.Response;
    }

    public async Task<bool> SignalInterestAsync(Guid id, string? message)
    {
        var request = new SignalEventInterestRequest()
        {
            Message = message
        };

        var result = await _apiClient.PostAsync<object>($"api/Events/{id}/interest", request);

        return result.IsSuccess;
    }

    public async Task<bool> RemoveInterestAsync(Guid id)
    {
        var result = await _apiClient.DeleteAsync($"api/Events/{id}/interest");

        return result.IsSuccess;
    }
}
