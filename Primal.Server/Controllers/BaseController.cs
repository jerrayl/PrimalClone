using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Primal.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class BaseController : Controller
    {
    }
}