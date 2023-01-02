using Microsoft.AspNetCore.Mvc;

namespace SocialNetwork.WebAPI.Controllers;
[ApiVersion("2.1")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class HomeController : ControllerBase
{

    [HttpGet]
    public int Get()
    {
        return 4;
    }
}