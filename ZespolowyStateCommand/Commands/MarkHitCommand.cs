using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZespolowyStateCommand.Cells;

namespace ZespolowyStateCommand.Commands
{
    public class MarkHitCommand : ICommand
    {
        private Cell target;
        private ICellState lastState;
        public MarkHitCommand(Cell target)
        {
            this.target = target;
            lastState = target.state;
        }
        public void Execute()
        {
            target.MarkHit();
        }

        public void Undo()
        {
            target.state = lastState;
        }
    }
}
