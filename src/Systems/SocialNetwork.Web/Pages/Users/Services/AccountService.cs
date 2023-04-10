using System.Text.Json;
using SocialNetwork.Web.Pages.Users.Models;

namespace SocialNetwork.Web.Pages.Users.Services;

public class AccountService : IAccountService
{
    private readonly HttpClient _httpClient;

    public AccountService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<AccountModel>> GetAccounts(int offset = 0, int limit = 100)
    {
        string url = $"{Settings.ApiRoot}/accounts?offset={offset}&limit={limit}";

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<IEnumerable<AccountModel>>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<AccountModel>();

        var accountModels = data.ToList();
        foreach (var accountModel in accountModels)
        {
            await AddAvatarsToModel(accountModel);
        }
        
        return accountModels;
    }

    public async Task<IEnumerable<AccountModel>> GetFriends(int offset = 0, int limit = 100)
    {
        string url = $"{Settings.ApiRoot}/Relationships/friends?offset={offset}&limit={limit}";

        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<IEnumerable<AccountModel>>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<AccountModel>();

        var accountModels = data.ToList();
        foreach (var accountModel in accountModels)
        {
            await AddAvatarsToModel(accountModel);
        }
        
        return accountModels;
    }

    public async Task<AccountModel> GetAccount(Guid accountId)
    {
        string url = $"{Settings.ApiRoot}/accounts/{accountId}";
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<AccountModel>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new AccountModel();

        return await AddAvatarsToModel(data);;
    }

    public async Task<AccountModel> GetAccountByUsername(string username)
    {
        string url = $"{Settings.ApiRoot}/accounts/{username}";
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<AccountModel>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new AccountModel();
        
        return await AddAvatarsToModel(data);
    }

    private async Task<AccountModel> AddAvatarsToModel(AccountModel model)
    {
        model.Avatars = (await GetAvatars(model.Id)).ToList();
        if (!model.Avatars.Any())
        {
            model.CurAvatar = new AvatarModel()
            {
                Content = Settings.StandardAvatar
            };
        }
        else
        {
            model.CurAvatar = model.Avatars.First(x => x.IsCurrentAvatar);
        }

        return model;
    }
    public async Task<IEnumerable<AvatarModel>> GetAvatars(Guid accountId)
    {
        string url = $"{Settings.ApiRoot}/accounts/{accountId}/avatars";
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var data = JsonSerializer.Deserialize<IEnumerable<AvatarModel>>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<AvatarModel>();

        return data;
    }
}