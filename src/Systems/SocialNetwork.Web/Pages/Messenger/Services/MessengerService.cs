using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using SocialNetwork.Web.Pages.Messenger.Models;

namespace SocialNetwork.Web.Pages.Messenger.Services;

public class MessengerService : IMessengerService
{
    private readonly HttpClient _httpClient;

    public MessengerService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<ChatModel>> GetChats()
    {
        string url = $"{Settings.ApiRoot}/messenger/chats";

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<IEnumerable<ChatModel>>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ChatModel>();

        return data;
    }

    public async Task<IEnumerable<MessageModel>> GetMessages(Guid chatId)
    {
        string url = $"{Settings.ApiRoot}/messenger/chats/{chatId}";

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<IEnumerable<MessageModel>>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<MessageModel>();

        return data.OrderByDescending(x => x.CreationDateTime);
    }

    public async Task<MessageModel> SendMessage(Guid receiverId, string text)
    {
        string url = $"{Settings.ApiRoot}/messenger/{receiverId}";

        var body = JsonSerializer.Serialize(text);
        var request = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, request);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<MessageModel>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        return data;
    }

    public async Task<IEnumerable<UserInChatModel>> GetUsersInChat(Guid chatId)
    {
        string url = $"{Settings.ApiRoot}/messenger/chats/{chatId}/users";

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<IEnumerable<UserInChatModel>>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<UserInChatModel>();

        return data;
    }
}