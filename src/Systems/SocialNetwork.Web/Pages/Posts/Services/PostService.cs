using System.Text.Json;
using SocialNetwork.Web.Pages.Posts.Models;
using SocialNetwork.Web.Pages.Users.Services;

namespace SocialNetwork.Web.Pages.Posts.Services;

public class PostService : IPostService
{
    private readonly HttpClient _httpClient;
    private readonly IAccountService _accountService;

    public PostService(HttpClient client, IAccountService accountService)
    {
        _httpClient = client;
        _accountService = accountService;
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

        data = data.ToList();

        var user = await _accountService.GetAccount(userId);
        foreach (var model in data)
        {
            model.Creator = user;
            model.Attachments = await GetAttachments(model.Id);
            model.Likes = await GetLikes(model.Id);
        }

        return data.OrderByDescending(x => x.CreationDateTime);
    }

    public async Task<IEnumerable<PostModel>> GetAllPosts(int offset = 0, int limit = 10)
    {
        string url = $"{Settings.ApiRoot}/posts?offset={offset}&limit={limit}";

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<IEnumerable<PostModel>>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<PostModel>();

        data = data.ToList();
        foreach (var model in data)
        {
            model.Creator = await _accountService.GetAccount(model.CreatorId);
            model.Attachments = await GetAttachments(model.Id);
            model.Likes = await GetLikes(model.Id);
        }

        return data.OrderByDescending(x => x.CreationDateTime);
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

    private async Task<int> GetLikes(Guid postId)
    {
        string url = $"{Settings.ApiRoot}/posts/{postId}/likes";
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<int>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return data;
    }
    
    public async Task<bool> IsUserLikedPost(Guid postId, Guid userId)
    {
        string url = $"{Settings.ApiRoot}/posts/{postId}/likes/{userId}";
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<bool>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return data;
    }

    public async Task LikePost(Guid postId)
    {
        string url = $"{Settings.ApiRoot}/posts/{postId}/likes";
        var response = await _httpClient.PostAsync(url, null);
        Console.WriteLine("like");
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(response.IsSuccessStatusCode);
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("Err");
            throw new Exception(content);
        }
    }
}