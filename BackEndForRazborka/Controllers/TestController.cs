using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEndForRazborka.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class TestController : ControllerBase
    {
        [Authorize]
        [HttpPost]
        public IActionResult TestPost([FromBody] string x)
        {

            return Ok("Защищённые даные");
        }
    }
}
