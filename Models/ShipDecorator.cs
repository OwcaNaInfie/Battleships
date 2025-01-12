namespace Statki.Models
{
    public abstract class ShipDecorator : IShip
    {
        protected IShip ShipModel;

        public ShipDecorator(IShip shipModel)
        {
            ShipModel = shipModel;
        }

        public virtual string Name { get => ShipModel.Name; set => ShipModel.Name = value; }
        public virtual int Size { get => ShipModel.Size; set => ShipModel.Size = value; }
        public virtual List<Cell> Positions { get => ShipModel.Positions; set => ShipModel.Positions = value; }
        public virtual int Hits { get => ShipModel.Hits; set => ShipModel.Hits = value; }

        public virtual bool IsSunk() => ShipModel.IsSunk();

        public virtual void ChangeTheme()
        {
            ShipModel.ChangeTheme();
        }
    }

    public class ColoredShip : ShipDecorator
    {
        public string Color { get; set; }

        public ColoredShip(IShip shipModel, string color = "red") : base(shipModel)
        {
          
            Color = color;
        }

        public override void ChangeTheme()
        {
            base.ChangeTheme();
            Color = "blue";
        }

    }

    public class PatternedShip : ShipDecorator
    {
        public string Pattern { get; set; }

        public PatternedShip(IShip shipModel, string pattern = "round") : base(shipModel)
        {
            Pattern = pattern;
        }

        public override void ChangeTheme()
        {
            base.ChangeTheme();
            Pattern = "blocky";
        }

  
    }
}
