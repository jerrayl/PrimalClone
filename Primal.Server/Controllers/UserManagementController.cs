using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Primal.Business;
using Primal.Models;
using Primal.Infrastructure;

namespace Primal.Controllers
{
    public class UserManagementController : BaseController
    {
        private IUserManagement _userManagement;
        private IGame _game;
        public UserManagementController(IUserManagement userManagement, IGame game)
        {
            _userManagement = userManagement;
            _game = game;
        }

        [HttpPost]
        [Route("create-player")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(GroupName = "UserManagment")]
        public IActionResult CreatePlayer([FromBody] CreatePlayerModel model)
        {
            try
            {
                _userManagement.CreatePlayer(model);
                return Ok();
            }
            catch (ErrorMessageException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("create-free-company")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(GroupName = "UserManagment")]
        public IActionResult CreateFreeCompany([FromBody] CreateFreeCompanyModel model)
        {
            try
            {
                _userManagement.CreateFreeCompany(model);
                return Ok();
            }
            catch (ErrorMessageException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("join-free-company")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(GroupName = "UserManagment")]
        public IActionResult JoinFreeCompany([FromBody] JoinFreeCompanyModel model)
        {
            try
            {
                _userManagement.JoinFreeCompany(model);
                return Ok();
            }
            catch (ErrorMessageException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("players")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(GroupName = "UserManagment")]
        public IActionResult GetPlayers()
        {
            return Ok(_userManagement.GetPlayers());
        }

        [HttpGet]
        [Route("free-companies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(GroupName = "UserManagment")]
        public IActionResult GetFreeCompanies()
        {
            return Ok(_userManagement.GetFreeCompanies());
        }

        [HttpGet]
        [Route("encounters")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(GroupName = "UserManagment")]
        public IActionResult GetEncounters()
        {
           return Ok(_userManagement.GetEncounters());
        }

        [HttpPost]
        [Route("start-encounter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(GroupName = "Game")]
        public IActionResult StartEncounter([FromBody] StartEncounterModel model)
        {
            try
            {
                return Ok(_game.StartEncounter(model));
            }
            catch (ErrorMessageException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}