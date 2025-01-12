using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Models.Cells
{
    public class Cell
    {
        public ICellState state { get; set; }
        public Cell() { state = new UnattackedEmptyState(); }
        public Cell(ICellState state) { this.state = state; }

        public void MarkHit()
        {
            state.MarkHit(this);
        }

        public void MarkOccupied()
        {
            state.MarkOccupied(this);
        }
    }
}
