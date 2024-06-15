using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Primal.Business;
using Primal.Models;
using System.Threading.Tasks;
using Primal.Infrastructure;

namespace Primal.Controllers
{
    [EncounterIdRequired]
    public class GameController : BaseController
    {
        private IGame _game;
        public GameController(IGame game)
        {
            _game = game;
        }

        [HttpPost]
        [Route("move")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(GroupName = "Game")]
        public async Task<IActionResult> Move([FromBody] MoveModel model)
        {
            try
            {
                await _game.Move(model);
                return Ok();
            }
            catch (ErrorMessageException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("spend-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(GroupName = "Game")]
        public async Task<IActionResult> SpendToken([FromBody] SpendTokenModel model)
        {
            try
            {
                await _game.SpendToken(model);
                return Ok();
            }
            catch (ErrorMessageException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("start-attack")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(GroupName = "Game")]
        public IActionResult StartAttack([FromBody] AttackModel model)
        {
            try
            {
                return Ok(_game.StartAttack(model));
            }
            catch (ErrorMessageException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("reroll-attack")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(GroupName = "Game")]
        public IActionResult RerollAttack([FromBody] RerollModel model)
        {
            try
            {
                return Ok(_game.RerollAttack(model));
            }
            catch (ErrorMessageException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("complete-attack")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(GroupName = "Game")]
        public async Task<IActionResult> CompleteAttack([FromQuery] int attackId)
        {
            try
            {
                await _game.CompleteAttack(attackId);
                return Ok();
            }
            catch (ErrorMessageException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("end-turn")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(GroupName = "Game")]
        public async Task<IActionResult> EndTurn()
        {
            try
            {
                await _game.EndTurn();
                return Ok();
            }
            catch (ErrorMessageException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("continue-enemy-action")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(GroupName = "Game")]
        public async Task<IActionResult> ContinueEnemyAction()
        {
            try
            {
                await _game.ContinueEnemyAction();
                return Ok();
            }
            catch (ErrorMessageException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}