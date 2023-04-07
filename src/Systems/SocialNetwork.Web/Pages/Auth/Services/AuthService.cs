using System.Net.Http.Headers;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SocialNetwork.Web.Pages.Auth.Models;
using SocialNetwork.Web.Pages.Users.Services;
using SocialNetwork.Web.Providers;

namespace SocialNetwork.Web.Pages.Auth.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ILocalStorageService _localStorage;
    private readonly IAccountService _accountService;

    public AuthService(HttpClient httpClient,
                       AuthenticationStateProvider authenticationStateProvider,
                       ILocalStorageService localStorage, IAccountService accountService)
    {
        _httpClient = httpClient;
        _authenticationStateProvider = authenticationStateProvider;
        _localStorage = localStorage;
        _accountService = accountService;
    }

    public async Task<LoginResult> Login(LoginModel loginModel)
    {
        var url = $"{Settings.IdentityRoot}/connect/token";
        
        var requestBody = new[] 
        {
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("client_id", Settings.ClientId),
            new KeyValuePair<string, string>("client_secret", Settings.ClientSecret),
            new KeyValuePair<string, string>("username", loginModel.Username!),
            new KeyValuePair<string, string>("password", loginModel.Password!)
        };

        
        var requestContent = new FormUrlEncodedContent(requestBody);

        var response = await _httpClient.PostAsync(url, requestContent);

        var content = await response.Content.ReadAsStringAsync();
        
        var loginResult = JsonSerializer.Deserialize<LoginResult>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new LoginResult();
        loginResult.Successful = response.IsSuccessStatusCode;

        if (!response.IsSuccessStatusCode)
        {
            return loginResult;
        }

        var userId = await _accountService.GetAccountByUsername(loginModel.Username);
        
        await _localStorage.SetItemAsync("authToken", loginResult.AccessToken);
        await _localStorage.SetItemAsync("refreshToken", loginResult.RefreshToken);
        Settings.AccessToken = loginResult.AccessToken;
        await _localStorage.SetItemAsync("currentUserId", userId.Id);
        ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.Username!);

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.AccessToken);

        return loginResult;
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        await _localStorage.RemoveItemAsync("refreshToken");

        ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();

        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}