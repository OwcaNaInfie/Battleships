using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleships.Models.Commands;

namespace Battleships.Controllers
{
    public class CommandManager
    {
        private Stack<ICommand> UndoStack;

        private Stack<ICommand> RedoStack;

        public CommandManager()
        {
            UndoStack = new();
            RedoStack = new();
        }

        public bool ExecuteCommand(ICommand command)
        {
            bool success = command.Execute();
            if (success)
            {
                UndoStack.Push(command);
                RedoStack.Clear();
            }
            return success;
        }

        public void Undo()
        {
            if (UndoStack.Count > 0)
            {
                ICommand command = UndoStack.Pop();
                command.Undo();
                RedoStack.Push(command);
            }
        }

        public void Redo()
        {
            if (RedoStack.Count > 0)
            {
                ICommand command = RedoStack.Pop();
                command.Execute();
                UndoStack.Push(command);
            }
        }
    }
}
