using Blazored.LocalStorage;

namespace SocialNetwork.Web.Services;

public class ConfigurationService : IConfigurationService
{
    private readonly ILocalStorageService _localStorageService;

    public ConfigurationService(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }
    
    public async Task<bool> GetDarkMode()
    {
        return await _localStorageService.GetItemAsync<bool>("darkMode");
    }

    public async Task SetDarkMode(bool value)
    {
        await _localStorageService.SetItemAsync("darkMode", value);
    }

    public async Task<bool> GetNavigationMenuVisible()
    {
        return await _localStorageService.GetItemAsync<bool>("navigationMenuVisible");
    }

    public async Task SetNavigationMenuVisible(bool value)
    {
        await _localStorageService.SetItemAsync("navigationMenuVisible", value);
    }
}