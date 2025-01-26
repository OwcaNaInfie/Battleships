using Battleships.Models.Ships;
using Battleships.Models.Cells;

namespace Battleships.Models
{
    public class Board
    {
        public int Size { get; set; }
        public List<List<Cell>> Grid { get; set; }
        public List<IShip> Ships { get; set; }  // Add this line

        public Board(int size = 10)
        {
            this.Size = size;
            Ships = new List<IShip>();  // Initialize the Ships list
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

        public bool PlaceShip(IShip ship, int startX, int startY, int endX, int endY)
        {
            // Ensure the ship is within bounds
            if (startX < 0 || startX >= Size || startY < 0 || startY >= Size || endX < 0 || endX >= Size || endY < 0 || endY >= Size)
            {
                return false;
            }

            // Calculate the length of the ship based on the coordinates
            int length = 0;
            if (startX == endX)
            {
                // Vertical ship placement
                length = Math.Abs(endY - startY) + 1;
            }
            else if (startY == endY)
            {
                // Horizontal ship placement
                length = Math.Abs(endX - startX) + 1;
            }

            // Check if the length matches the ship size
            if (length != ship.Size)
            {
                Console.WriteLine($"The length of the ship doesn't match the required size. The ship size is {ship.Size}.");
                return false;
            }

            // Loop through the cells the ship occupies based on start and end coordinates
            for (int i = 0; i < ship.Size; i++)
            {
                int x = startX;
                int y = startY;

                // Determine if we are going horizontally or vertically
                if (startX == endX)
                {
                    y += i; // Vertical placement
                }
                else
                {
                    x += i; // Horizontal placement
                }

                // Check if the cell is already occupied
                var cell = Grid[x][y];
                if (cell.State is UnattackedOccupiedState)
                {
                    Console.WriteLine($"Error: Cell at ({x},{y}) is already occupied.");
                    return false; // Invalid placement if the cell is occupied
                }

                // Mark the cell as occupied by the ship
                cell.State = new UnattackedOccupiedState();
            }

            // Add the ship to the list after placement
            Ships.Add(ship);
            return true;
        }



        // Add the DisplayBoard method to visualize the board (as you already have)
        public void DisplayBoard()
        {
            for (int y = 0; y < Size; y++)
            {
                Console.Write(y + " |"); // Writing out row numbers

                for (int x = 0; x < Size; x++)
                {
                    Cell cell = Grid[x][y];

                    // If the cell is occupied by a ship (UnattackedOccupiedState), display a ship symbol (#)
                    // If it's hit, show a ship symbol (#) as well
                    if (cell.State is UnattackedOccupiedState || cell.State is HitOccupiedState)
                    {
                        Console.Write(" # ");  // Ship present
                    }
                    else
                    {
                        Console.Write(" . ");  // Empty or missed cell
                    }
                }
                Console.WriteLine();  // Move to the next row
            }

            Console.Write("   "); // Bottom separation line
            for (int i = 0; i < Size; i++)
            {
                Console.Write("---");
            }
            Console.WriteLine();

            Console.Write("   "); // Writing out column numbers
            for (int i = 0; i < Size; i++)
            {
                Console.Write(" " + i + " ");
            }
            Console.Write("\n\n");
        }
    }
}