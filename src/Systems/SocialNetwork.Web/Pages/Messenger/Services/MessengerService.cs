using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Forms;
using SocialNetwork.Web.Pages.Messenger.Models;
using SocialNetwork.Web.Pages.Posts.Models;

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

    public async Task<IEnumerable<MessageModel>> GetMessages(Guid chatId, int offset = 0, int limit = 10000)
    {
        string url = $"{Settings.ApiRoot}/messenger/chats/{chatId}?offset={offset}&limit={limit}";

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

    public async Task<MessageModel> SendMessage(Guid receiverId, string text )
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

    public async Task<IEnumerable<AttachmentModel>> GetAttachments(Guid messageId)
    {
        string url = $"{Settings.ApiRoot}/messenger/{messageId}";
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<IEnumerable<AttachmentModel>>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<AttachmentModel>();

        return data;
    }

    public async Task AddAttachments(Guid messageId, IEnumerable<IBrowserFile> files)
    {
        using var content = new MultipartFormDataContent();
        foreach (var browserFile in files)
        {
            var fileContent = new StreamContent(browserFile.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(browserFile.ContentType);
            content.Add(content: fileContent, name: "\"files\"", fileName: browserFile.Name);
        }
        
        string url = $"{Settings.ApiRoot}/messenger/{messageId}/upload";

        //var body = JsonSerializer.Serialize(content);
        //var request = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);

        var cont = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(cont);
        }
        else
        {
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }
    }
}