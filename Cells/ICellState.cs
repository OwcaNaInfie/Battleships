using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Cells
{
    public interface ICellState
    {
        public bool MarkHit(Cell context);
        public void MarkOccupied(Cell context);
    }
}
