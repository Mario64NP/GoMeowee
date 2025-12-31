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
}
