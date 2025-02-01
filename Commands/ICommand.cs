using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Commands
{
    public interface ICommand
    {
        public bool Execute();
        public void Undo();
    }
}
