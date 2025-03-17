using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Quizz.App.Web.Controllers
{
    [Authorize] // Требуется JWT-токен
    [ApiController]
    [Route("api/protected")]
    public class ProtectedController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetSecretData()
        {
            return Ok(new { Message = "Вы получили доступ к защищённым данным!" });
        }
    }
}