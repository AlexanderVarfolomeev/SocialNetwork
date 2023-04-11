using System.Text;
using System.Text.Json;
using SocialNetwork.Web.Pages.Auth.Models;
using SocialNetwork.Web.Pages.Profile.Model;
using SocialNetwork.Web.Pages.Users.Models;
using SocialNetwork.Web.Pages.Users.Services;

namespace SocialNetwork.Web.Pages.Auth.Services;

public class ProfileService : IProfileService
{
    private readonly HttpClient _httpClient;
    private readonly IAccountService _accountService;

    public ProfileService(HttpClient httpClient, IAccountService accountService)
    {
        _httpClient = httpClient;
        _accountService = accountService;
    }
    
    public async Task Register(RegisterAccountForm registerAccountModel)
    {
        var url = $"{Settings.ApiRoot}/profile/register";
        var body = JsonSerializer.Serialize(registerAccountModel);
        var request = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, request);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
    }

    public async Task UpdateAccount(Guid accountId, UpdateAccount model)
    {
        string url = $"{Settings.ApiRoot}/accounts/{accountId}";

        var body = JsonSerializer.Serialize(model);
        var request = new StringContent(body, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync(url, request);

        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
    }
}