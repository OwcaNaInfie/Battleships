using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZespolowyStateCommand.Commands
{
    public interface ICommand
    {
        public void Execute();
        public void Undo();
    }
}
