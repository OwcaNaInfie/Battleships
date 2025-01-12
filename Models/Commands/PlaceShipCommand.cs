using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleships.Models.Cells;

namespace Battleships.Models.Commands
{
    public class PlaceShipCommand
    {
        private Cell target;
        private ICellState lastState;
        public PlaceShipCommand(Cell target)
        {
            this.target = target;
            lastState = target.state;
        }
        public void Execute()
        {
            target.MarkOccupied();
        }

        public void Undo()
        {
            target.state = lastState;
        }
    }
}
