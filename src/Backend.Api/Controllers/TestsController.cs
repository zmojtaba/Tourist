namespace Backend.Api.Controllers
{
    [Route("api/test/")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        [HttpPost("register/")]
        public async Task<IActionResult> CreateAccountAsync([FromBody] CreateAccountDto dto)
        {
            return Ok("tourLeader");
        }

    }
}
