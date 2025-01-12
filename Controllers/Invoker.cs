using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleships.Models.Commands;

namespace Battleships.Controllers
{
    public class Invoker
    {
        private Stack<ICommand> undoStack;

        private Stack<ICommand> redoStack;

        public Invoker()
        {
            undoStack = new();
            redoStack = new();
        }

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            undoStack.Push(command);
            redoStack.Clear();
        }

        public void Undo() 
        { 
            if (undoStack.Count > 0) 
            {
                ICommand command = undoStack.Pop(); 
                command.Undo();
                redoStack.Push(command);
            }
        }

        public void Redo() 
        {
            if (redoStack.Count > 0) 
            {
                ICommand command = redoStack.Pop();
                command.Execute();
                undoStack.Push(command);
            }
        }
    }
}
