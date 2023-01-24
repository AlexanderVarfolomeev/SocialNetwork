using Microsoft.AspNetCore.Mvc;
using SocialNetwork.AccountServices;
using SocialNetwork.AccountServices.Models;

namespace SocialNetwork.WebAPI.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class HomeController : ControllerBase
{
    private IProfileService _service;

    public HomeController(IProfileService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<AppAccountModel> C()
    {
        return await _service.RegisterUser(new AppAccountModelRequest()
        {
            Birthday = DateTimeOffset.Now,
            Email = "dsf",
            Name = "sdf",
            Password = "1234",
            PhoneNumber = "234123",
            Status = "dg",
            Surname = "gdfg",
            UserName = "123"
        });
    }
}