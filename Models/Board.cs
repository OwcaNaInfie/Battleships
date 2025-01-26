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

        public Cell? GetCell(int x, int y)
        {
            if ( !IsInBounds(x,y) )
            {
                return null;
            }

            return Grid[x][y];
        }

        public bool IsInBounds(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Size || y >= Size)
            {
                return false;
            }
            return true;
        }


        // Add the DisplayBoard method to visualize the board (as you already have)
        public void DisplayBoard(bool showShipPlacement = true)
        {
            for (int y = 0; y < Size; y++)
            {
                Console.Write(y + " |"); // Writing out row numbers

                for (int x = 0; x < Size; x++)
                {
                    Cell cell = Grid[x][y];

                    switch (cell.State)
                    {
                        case UnattackedOccupiedState:
                            // A player can see their own ship placement, but they can't see the other player's
                            if (showShipPlacement) { Console.Write(" # "); }
                            else { Console.Write(" . "); }
                            break;
                        case HitOccupiedState:
                            Console.Write(" X ");
                            break;
                        case UnattackedEmptyState:
                            Console.Write(" . ");
                            break;
                        case HitEmptyState:
                            Console.Write(" O ");
                            break;
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