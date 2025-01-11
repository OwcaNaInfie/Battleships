using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZespolowyStateCommand.Cells
{
    public interface ICellState
    {
        public void MarkHit(Cell context);
        public void MarkOccupied(Cell context);
    }
}
