using Microsoft.AspNetCore.Mvc;

namespace Miraishop.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloWorldController : ControllerBase
    {
        private readonly ILogger<HelloWorldController> _logger;

        public HelloWorldController(ILogger<HelloWorldController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetHelloWorld")]
        public IActionResult Get()
        {
            _logger.LogInformation("Hello World API 被呼叫了");
            return Ok(new { message = "Hello World from Server!", timestamp = DateTime.Now });
        }

        [HttpPost(Name = "PostHelloWorld")]
        public IActionResult Post([FromBody] HelloRequest request)
        {
            _logger.LogInformation($"收到來自 {request.Name} 的問候");
            return Ok(new { message = $"你好，{request.Name}！伺服器時間：{DateTime.Now}" });
        }
    }

    public class HelloRequest
    {
        public string Name { get; set; } = string.Empty;
    }
}
