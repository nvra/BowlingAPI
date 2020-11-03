using BowlingApi.Models;
using BowlingApi.Models.Requests;
using BowlingApi.Models.Response;
using BowlingApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BowlingApi.Controllers
{
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IBowlingService _service;

        public GameController(IBowlingService service)
        {
            _service = service;
        }

        [Route("api/startgame/{playerId}")]
        [HttpPost]
        public ActionResult<Game> StartGame([FromRoute] int playerId)
        {
            return Ok(_service.InsertGame(playerId));
        }

        [Route("api/FrameThrowScore")]
        [HttpPost]
        public ActionResult<BowlingResponse> FrameThrowScore([FromBody] FramethrowRequest framethrowRequest)
        {
            if (!ModelState.IsValid || framethrowRequest == null)
            {
                return BadRequest();
            }

            var response = _service.InsertFrameScore(framethrowRequest);

            return Ok(response);
        }

        [Route("api/gameScores/{gameId}")]
        [HttpGet]
        public ActionResult<BowlingResponse> GetGameScores(int gameId)
        {
            return Ok(_service.GetScoresByGameId(gameId));
        }

        [Route("api/deleteGameScores/{gameId}")]
        [HttpDelete]
        public ActionResult<BowlingResponse> DeleteAllScores(int gameId)
        {
            return Ok(_service.DeleteGameScores(gameId));
        }

        [Route("api/getGameId/{playerId}")]
        [HttpGet]
        public ActionResult<int> GetGameByPlayer(int playerId)
        {
            return Ok(_service.GetGameByPlayer(playerId));
        }

    }
}
