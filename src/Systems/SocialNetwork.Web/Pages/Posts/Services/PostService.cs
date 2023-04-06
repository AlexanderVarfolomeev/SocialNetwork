using System.Net.Mail;
using System.Text.Json;
using SocialNetwork.Web.Pages.Posts.Models;
using SocialNetwork.Web.Pages.Users.Models;

namespace SocialNetwork.Web.Pages.Posts.Services;

public class PostService : IPostService
{
    private readonly HttpClient _httpClient;

    public PostService(HttpClient client)
    {
        _httpClient = client;
    }
    
    public async Task<IEnumerable<PostModel>> GetPostsByUser(Guid userId, int offset = 0, int limit = 10)
    {
        string url = $"{Settings.ApiRoot}/accounts/{userId}/posts?offset={offset}&limit={limit}";

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<IEnumerable<PostModel>>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<PostModel>();

        return data;
    }
    
    public async Task<IEnumerable<AttachmentModel>> GetAttachments(Guid postId)
    {
        string url = $"{Settings.ApiRoot}/posts/{postId}/attachments";
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
}