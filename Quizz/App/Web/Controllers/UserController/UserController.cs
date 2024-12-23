using Microsoft.AspNetCore.Mvc;

namespace Quizz.App.Web.Controllers.UserController;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    [HttpGet]
    public int healthCheck()
    {
        return 200;
    }
}