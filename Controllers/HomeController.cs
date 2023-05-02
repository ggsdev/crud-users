using Microsoft.AspNetCore.Mvc;

namespace ANPCentral.Controllers
{

    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet()]
        public IActionResult Get()
        {
            return Ok(new
            {
                message = "Api online"
            });
        }

    }
}