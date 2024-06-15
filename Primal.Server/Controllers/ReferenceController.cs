using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Primal.Models;

namespace Primal.Controllers
{
    public class ReferenceController : Controller
    {
        // Fake endpoints to get models into NSwag
        
        [HttpPost]
        [Route("gamestate")]
        [ProducesResponseType(StatusCodes.Status418ImATeapot)]
        [ApiExplorerSettings(GroupName = "Reference")]
        public IActionResult GameState([FromBody] GameStateModel model)
        {
            return new StatusCodeResult(418);
        }

        [HttpPost]
        [Route("encounter")]
        [ProducesResponseType(StatusCodes.Status418ImATeapot)]
        [ApiExplorerSettings(GroupName = "Reference")]
        public IActionResult Encounter([FromBody] EncounterModel model)
        {
            return new StatusCodeResult(418);
        }

        [HttpPost]
        [Route("free-company")]
        [ProducesResponseType(StatusCodes.Status418ImATeapot)]
        [ApiExplorerSettings(GroupName = "Reference")]
        public IActionResult FreeCompany([FromBody] FreeCompanyModel model)
        {
            return new StatusCodeResult(418);
        }
    }
}