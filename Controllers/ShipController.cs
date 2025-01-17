using Microsoft.AspNetCore.Mvc;
using Battleships.Models;
using Battleships.Models.Cells;
using Battleships.Models.Ships;

[ApiController]
[Route("api/[controller]")]
public class ShipController : ControllerBase
{
    [HttpGet("create/player1/{type}")]
    public IActionResult CreateShipForPlayer1(string type)
    {
        ShipFactory factory = type.ToLower() switch
        {
            "one-mast" => new OneMastFactory(),
            "two-mast" => new TwoMastFactory(),
            "three-mast" => new ThreeMastFactory(),
            "four-mast" => new FourMastFactory(),

        };

        if (factory == null)
            return BadRequest("Nie ma takiego statku");

        var ship = factory.CreateShip(1);
        return Ok(ship);
    }

    [HttpGet("create/player2/{type}")]
    public IActionResult CreateShipForPlayer2(string type)
    {
        ShipFactory factory = type.ToLower() switch
        {
            "one-mast" => new OneMastFactory(),
            "two-mast" => new TwoMastFactory(),
            "three-mast" => new ThreeMastFactory(),
            "four-mast" => new FourMastFactory(),

        };

        if (factory == null)
            return BadRequest("Nie ma takiego statku");

        var ship = factory.CreateShip(2);
        return Ok(ship);
    }

    [HttpGet("board")]
    public IActionResult GetBoard()
    {
        var board = new Board();
        return Ok(board);
    }
}
