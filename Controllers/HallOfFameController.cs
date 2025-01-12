// Controllers/HallOfFameController.cs
using Battleship.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Battleship.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HallOfFameController : ControllerBase
    {
        private readonly HallOfFame _hallOfFame;

        public HallOfFameController()
        {
            _hallOfFame = HallOfFame.GetInstance();
        }

        // GET: api/HallOfFame/TopThree
        [HttpGet("TopThree")]
        public ActionResult<List<Statistics>> GetTopThreePlayers()
        {
            var topPlayers = _hallOfFame.TopThree();
            return Ok(topPlayers);
        }

        // GET: api/HallOfFame/All
        [HttpGet("All")]
        public ActionResult<List<Statistics>> GetAllPlayerStatistics()
        {
            var allStatistics = _hallOfFame.GetPlayerStatistics();
            return Ok(allStatistics);
        }

        // POST: api/HallOfFame/AddOrUpdate
        [HttpPost("AddOrUpdate")]
        public IActionResult AddOrUpdatePlayerStatistics([FromBody] UpdateStatisticsRequest request)
        {
            if (request == null || request.PlayerID <= 0)
            {
                return BadRequest("Invalid player data.");
            }

            _hallOfFame.AddOrUpdatePlayerStatistics(request.PlayerID, request.IsWin);
            return Ok("Player statistics updated successfully.");
        }
    }

    // Request model for updating statistics
    public class UpdateStatisticsRequest
    {
        public int PlayerID { get; set; }
        public bool IsWin { get; set; }
    }
}
