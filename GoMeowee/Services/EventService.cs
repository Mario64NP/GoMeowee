using GoMeowee.Models;
using GoMeowee.Models.Events;
using GoMeowee.Models.Interests;

namespace GoMeowee.Services;

public class EventService(ApiClient apiClient)
{
    public async Task<List<EventListItemDto>?> GetEventsAsync()
    {
        var result = await apiClient.GetAsync<List<EventListItemDto>>("api/Events");

        if (!result.IsSuccess)
            return null;

        return result.Response;
    }

    public async Task<EventDetailsDto?> GetEventByIdAsync(Guid eventId)
    {
        var result = await apiClient.GetAsync<EventDetailsDto>($"api/Events/{eventId}");

        if (!result.IsSuccess)
            return null;

        return result.Response;
    }

    public async Task<bool> GetEventInterestByIdAsync(Guid eventId)
    {
        var result = await apiClient.GetAsync<bool>($"api/Events/{eventId}/interest");

        if (!result.IsSuccess)
            return false;

        return result.Response;
    }

    public async Task<IEnumerable<EventInterestDto>?> GetInterestedPeopleAsync(Guid eventId)
    {
        var result = await apiClient.GetAsync<IEnumerable<EventInterestDto>>($"api/Events/{eventId}/interests");

        if (!result.IsSuccess)
            return null;

        return result.Response;
    }

    public async Task<bool> SignalInterestAsync(Guid eventId, string? message)
    {
        var request = new SignalEventInterestRequest()
        {
            Message = message
        };

        var result = await apiClient.PostAsync<object>($"api/Events/{eventId}/interest", request);

        return result.IsSuccess;
    }

    public async Task<bool> RemoveInterestAsync(Guid eventId)
    {
        var result = await apiClient.DeleteAsync($"api/Events/{eventId}/interest");

        return result.IsSuccess;
    }
}
