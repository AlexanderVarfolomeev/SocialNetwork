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

        var data = JsonSerializer.Deserialize<IEnumerable<AccountModel>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<AccountModel>();

        return data;
    }

    public Task<AccountModel> GetAccount(Guid accountId)
    {
        throw new NotImplementedException();
    }
}