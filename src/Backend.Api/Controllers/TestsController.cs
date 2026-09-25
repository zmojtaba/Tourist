using Microsoft.Extensions.Caching.Memory;

namespace Backend.Api.Controllers
{
    [Route("api/test/")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly IMemoryCache _cache;

        public TestsController(IMemoryCache cache)
        {
            _cache = cache;
        }

        [HttpPost("register/")]
        public async Task<IActionResult> CreateAccountAsync([FromBody] CreateAccountDto dto)
        {
            return Ok("tourLeader");
        }

        [HttpGet("/test-imemory/{key}")]
        public async Task<IActionResult> TestIMemoryCache([FromRoute] string key)
        {
            _cache.Set("test", key);
            return Ok("ok");
        }

        [HttpGet("gets-imemory/")]
        public async Task<IActionResult> GetsImemory()
        {
            var idn = _cache.TryGetValue("test", out List<string>? value);
            Console.WriteLine("---------------" + idn);
            return Ok(value);
        }



    }
}
