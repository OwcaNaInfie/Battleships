using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleships.Models.Cells;
using Battleships.Models.Ships;

    namespace Battleships.Models
    {
        public class Board
        {
        
            public int Size { get; set; }
            public List<List<Cell>> Grid { get; set; }

            public Board(int size = 10)
            {
                this.Size = size;

                Grid = new List<List<Cell>>();

                for (int i = 0; i < size; i++)
                {
                    Grid.Add(new List<Cell>());

                    for (int j = 0; j < size; j++)
                    {
                        Grid[i].Add(new Cell());
                    }
                }
            }

            public bool PlaceShip(IShip ship, int startX, int startY, bool isHorizontal)
            {
                // Check if the coordinates and direction are valid
                if (!IsValidPlacement(ship, startX, startY, isHorizontal))
                {
                    return false; // Invalid placement
                }

                // Place the ship
                for (int i = 0; i < ship.Size; i++)
                {
                    int x = isHorizontal ? startX + i : startX;
                    int y = isHorizontal ? startY : startY + i;

                    Grid[x][y].MarkOccupied(); // Mark the cell as occupied by the ship
                }

                return true; // Ship placed successfully
            }

            // Check if a ship can be placed at the given coordinates and direction
            public bool IsValidPlacement(IShip ship, int startX, int startY, bool isHorizontal)
            {
                // Check if the starting coordinates are within bounds
                if (startX < 0 || startX >= Size || startY < 0 || startY >= Size)
                {
                    return false; // Out of bounds
                }

                // Check if the ship fits in the given direction
                for (int i = 0; i < ship.Size; i++)
                {
                    int x = isHorizontal ? startX + i : startX;
                    int y = isHorizontal ? startY : startY + i;

                    // Check if the cell is within bounds and already occupied
                    if (x >= Size || y >= Size || Grid[x][y].StateName == "Occupied")
                    {
                        return false; // Invalid placement
                    }
                }

                return true;
            }

             public bool AreAllShipsPlaced()
            {
                // Check if all ships have been placed on the board
                // For example, you could check if the board has enough occupied cells for each type of ship
                int totalShipsPlaced = 0;

                foreach (var row in Grid)
                {
                    foreach (var cell in row)
                    {
                        if (cell.StateName == "Occupied")
                        {
                            totalShipsPlaced++;
                        }
                    }
                }

                return totalShipsPlaced == 10; // Assuming you want 10 ships placed in total
            }
        }
    }
 
