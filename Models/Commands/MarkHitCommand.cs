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
        private Board TargetBoard { get; set; }
        private ICellState? LastState { get; set; }
        private Cell? TargetCell { get; set; }
        public MarkHitCommand(Board board, int x, int y)
        {
            TargetBoard = board;
            TargetCell = TargetBoard.GetCell(x, y);

            if (TargetCell != null)
            {
                LastState = TargetCell.State;
            }
            else
            {
                Console.WriteLine("These coordinates are not on the board.");
            }
        }
        public bool Execute()
        {
            if (TargetCell != null)
            {
                return TargetCell.MarkHit();
            }
            return false;
        }

        public void Undo()
        {
            if (TargetCell != null)
            {
                TargetCell.State = LastState;
            }
        }
    }
}
