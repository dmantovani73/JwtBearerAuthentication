using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtBearerAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EchoController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get(string name)
        {
            return $"Hello {name}";
        }
    }
}
