using System.Text.Json;
using SocialNetwork.Web.Pages.Friends.Models;

namespace SocialNetwork.Web.Pages.Friends.Services;

public class FriendService : IFriendService
{
    private readonly HttpClient _httpClient;

    public FriendService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task SendFriendshipRequest(Guid userId)
    {
        var url = $"{Settings.ApiRoot}/Relationships?recipientId={userId}";
        var response = await _httpClient.PostAsync(url, null);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
    }

    public async Task AcceptFriendshipRequest(Guid requestId)
    {
        var url = $"{Settings.ApiRoot}/Relationships/accept-friendship/{requestId}";
        var response = await _httpClient.PutAsync(url, null);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
    }

    public async Task RejectFriendshipRequest(Guid requestId)
    {
        var url = $"{Settings.ApiRoot}/Relationships/reject-friendship/{requestId}";
        var response = await _httpClient.PutAsync(url, null);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
    }


    public async Task<IEnumerable<FriendShipRequest>> GetFriendshipRequests()
    {
        string url = $"{Settings.ApiRoot}/relationships?offset=0&limit=1000";

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<IEnumerable<FriendShipRequest>>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<FriendShipRequest>();

        return data.OrderByDescending(x => x.CreationDateTime);
    }
}