using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusHostelAllocation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("profile")]
        [Authorize]
        public IActionResult Profile()
        {
            return Ok(new
            {
                message = "You are authenticated!",
                user = User.Identity?.Name
            });
        }
    }
}