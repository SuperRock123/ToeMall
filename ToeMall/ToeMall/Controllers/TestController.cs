using Microsoft.AspNetCore.Mvc;
using ToeMall.Utils;
namespace ToeMall.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            await RequestLogger.LogRequestAsync(HttpContext);
            Console.WriteLine(HashEncoding.ComputeHash("abc"));
            Console.WriteLine(HashEncoding.ComputeHash("abc"));
            Console.WriteLine(HashEncoding.ComputeHash("abcd"));
            Console.WriteLine(HashEncoding.ComputeHash("abcxx"));
            return Ok(new {data="test"});
        }
    }
}
