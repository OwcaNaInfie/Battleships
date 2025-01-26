using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleships.Models.Cells;
using Battleships.Models.Ships;

namespace Battleships.Models.Commands
{
    public class PlaceShipCommand : ICommand
    {
        private Board Board { get; set; }
        private IShip Ship { get; set; }
        private int StartX {  get; set; }
        private int StartY { get; set; }
        private int EndX { get; set; }
        private int EndY { get; set; }
        public PlaceShipCommand(Board board, IShip ship, int startX, int startY, int endX, int endY)
        {
            Board = board;
            Ship = ship;
            StartX = startX;
            StartY = startY;
            EndX = endX;
            EndY = endY;
        }
        public bool Execute()
        {

            // Ensure the ship is within bounds
            if (!Board.IsInBounds(StartX, StartY) && !Board.IsInBounds(EndX, EndY))
            {
                return false;
            }

            // Calculate the length of the ship based on the coordinates
            int length = 0;
            if (StartX == EndX)
            {
                // Vertical ship placement
                length = Math.Abs(EndY - StartY) + 1;
            }
            else if (StartY == EndY)
            {
                // Horizontal ship placement
                length = Math.Abs(EndX - StartX) + 1;
            }

            // Check if the length matches the ship size
            if (length != Ship.Size)
            {
                Console.WriteLine($"The length of the ship doesn't match the required size. The ship size is {Ship.Size}.");
                return false;
            }

            // Loop through the cells the ship occupies based on start and end coordinates
            for (int i = 0; i < Ship.Size; i++)
            {
                int x = StartX;
                int y = StartY;

                // Determine if we are going horizontally or vertically
                if (StartX == EndX)
                {
                    y += i; // Vertical placement
                }
                else
                {
                    x += i; // Horizontal placement
                }

                // Check if the cell is already occupied
                var cell = Board.GetCell(x, y);
                if (cell.State is UnattackedOccupiedState)
                {
                    Console.WriteLine($"Error: Cell at ({x},{y}) is already occupied.");
                    return false; // Invalid placement if the cell is occupied
                }

                // Mark the cell as occupied by the ship
                cell.MarkOccupied();
            }

            // Add the ship to the list after placement
            Board.Ships.Add(Ship);
            return true;
        }

        public void Undo()
        {
            
        }
    }
}
