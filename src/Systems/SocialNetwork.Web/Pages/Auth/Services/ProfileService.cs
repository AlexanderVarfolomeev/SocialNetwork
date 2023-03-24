using System.Text;
using System.Text.Json;
using SocialNetwork.Web.Pages.Auth.Models;
using SocialNetwork.Web.Pages.Users.Models;

namespace SocialNetwork.Web.Pages.Auth.Services;

public class ProfileService : IProfileService
{
    private readonly HttpClient _httpClient;

    public ProfileService(HttpClient httpClient)
    {
        _httpClient = httpClient;
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
}