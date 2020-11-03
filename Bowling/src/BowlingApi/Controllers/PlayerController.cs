using BowlingApi.Models;
using BowlingApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BowlingApi.Controllers
{
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IBowlingService _service;

        public PlayerController(IBowlingService service)
        {
            _service = service;
        }

        [Route("api/player")]
        [HttpGet]
        public ActionResult<List<Player>> GetPlayers()
        {
            return Ok(_service.GetPlayers());
        }

        [Route("api/player/{name}")]
        [HttpPost]
        public ActionResult<Player> InsertPlayer([FromRoute] string name)
        {
            return Ok(_service.InsertPlayer(name));
        }

        [Route("api/player/{name}")]
        [HttpGet]
        public ActionResult<Player> GetPlayersByName([FromRoute] string name)
        {
            return Ok(_service.GetPlayersByName(name));
        }

    }
}
