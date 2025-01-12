using Microsoft.AspNetCore.Mvc;
using Battleships.Models;
using Battleships.Models.Cells;
using Battleships.Models.Ships;

[ApiController]
[Route("api/[controller]")]
public class ShipController : ControllerBase
{
    [HttpGet("create/{type}")]
    public IActionResult CreateShip(string type)
    {
        ShipFactory factory = type.ToLower() switch
        {
            "one-mast" => new OneMastFactory(),
            "two-mast" => new TwoMastFactory(),
            "three-mast" => new ThreeMastFactory(),
           
        };

        if (factory == null)
            return BadRequest("Nie ma takiego statku");

        var ship = factory.CreateShip();
        return Ok(ship);
    }
    [HttpPost("theme/color")]
    public IActionResult ApplyColorTheme([FromBody] ShipWithColor request)
    {
        var ship = new Ship
        {
            Name = request.Name,
            Size = request.Size,
            Hits = request.Hits,
            Positions = request.Positions
        };

        var coloredShip = new ColoredShip(ship, request.Color);
        //Zmienia kolor na niebieski
       coloredShip.ChangeTheme();

        return Ok(coloredShip);
    }

    [HttpPost("theme/pattern")]
    public IActionResult ApplyPatternTheme([FromBody] ShipWithPattern request)
    {
        var ship = new Ship
        {
            Name = request.Name,
            Size = request.Size,
            Hits = request.Hits,
            Positions = request.Positions
        };

        var patternedShip = new PatternedShip(ship, request.Pattern);
        //Zmienia wz�r na blocky
        patternedShip.ChangeTheme();

        return Ok(patternedShip);
    }
    public class ShipWithColor
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public int Hits { get; set; }
        public List<Cell> Positions { get; set; }
        public string Color { get; set; }
    }

    public class ShipWithPattern
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public int Hits { get; set; }
        public List<Cell> Positions { get; set; }
        public string Pattern { get; set; }
    }

}
