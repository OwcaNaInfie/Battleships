using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleships.Models.Cells;

namespace Battleships.Models.Commands
{
    public class MarkHitCommand : ICommand
    {
        private Cell Target;
        private ICellState LastState;
        public MarkHitCommand(Cell Target)
        {
            this.Target = Target;
            LastState = Target.State;
        }
        public void Execute()
        {
            Target.MarkHit();
        }

        public void Undo()
        {
            Target.State = LastState;
        }
    }
}
