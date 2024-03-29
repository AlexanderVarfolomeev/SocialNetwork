﻿using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Forms;
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
            model.Attachments = await GetPostAttachments(model.Id);
            model.Likes = await GetLikes(model.Id);
            model.Comments = await GetCommentsByPost(model.Id);
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
            model.Attachments = await GetPostAttachments(model.Id);
            model.Likes = await GetLikes(model.Id);
            model.Comments = await GetCommentsByPost(model.Id);
        }

        return data.OrderByDescending(x => x.CreationDateTime);
    }

    public async Task<IEnumerable<AttachmentModel>> GetPostAttachments(Guid postId)
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

    public async Task<bool> IsUserLikedPost(Guid postId)
    {
        string url = $"{Settings.ApiRoot}/posts/{postId}/likes/isLiked";
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
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(response.IsSuccessStatusCode);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
    }

    private async Task<IEnumerable<AttachmentModel>> GetCommentAttachments(Guid commentId)
    {
        string url = $"{Settings.ApiRoot}/comments/{commentId}/attachments";
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

    public async Task<IEnumerable<CommentModel>> GetCommentsByPost(Guid postId, int offset = 0, int limit = 1000)
    {
        string url = $"{Settings.ApiRoot}/posts/{postId}/comments?offset={offset}&limit={limit}";
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<IEnumerable<CommentModel>>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<CommentModel>();

        data = data.ToList();

        foreach (var model in data)
        {
            model.Creator = await _accountService.GetAccount(model.CreatorId);
            model.Attachments = await GetCommentAttachments(model.Id);
        }

        return data;
    }

    public async Task<PostModel> AddPost(PostAddModel post)
    {
        string url = $"{Settings.ApiRoot}/posts";

        var body = JsonSerializer.Serialize(post);
        var request = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, request);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
        
        var model = JsonSerializer.Deserialize<PostModel>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        model.Creator = await _accountService.GetAccount(model.CreatorId);
        model.Attachments = await GetPostAttachments(model.Id);
        model.Likes = await GetLikes(model.Id);
        model.Comments = await GetCommentsByPost(model.Id);
        return model;
    }
    
    public async Task AddAttachments(Guid postId, List<IBrowserFile> files)
    {
        // TODO при отправке более 1 файла запрос даже не выполняется
        using var content = new MultipartFormDataContent();
        string url = $"{Settings.ApiRoot}/posts/{postId}/attachments";
        foreach (var browserFile in files)
        {
            var fileContent = new StreamContent(browserFile.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(browserFile.ContentType);
            content.Add(content: fileContent, name: $"\"attachments_{Guid.NewGuid()}\"", fileName: browserFile.Name);
        }
        var response = await _httpClient.PostAsync(url, content);
        var cont = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine(cont);
            throw new Exception(cont);
        }
    }
}