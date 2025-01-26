using Battleships.Models.Ships;
using Battleships.Models.Cells;

namespace Battleships.Models
{
    public class Board
    {
        public int Size { get; set; }
        public List<List<Cell>> Grid { get; set; }
        public List<IShip> Ships { get; set; }

        // Inicjalizacja planszy
        public Board(int size = 10)
        {
            this.Size = size;
            Ships = new List<IShip>();
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
            if (!IsInBounds(x, y))
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


        // Wizualizacja planszy
        public void DisplayBoard(bool showShipPlacement = true, char shipSymbol = '#')
        {
            for (int y = 0; y < Size; y++)
            {
                Console.Write(y + " |"); // Wiersze

                for (int x = 0; x < Size; x++)
                {
                    Cell cell = Grid[x][y];

                    switch (cell.State)
                    {
                        case UnattackedOccupiedState:
                            if (showShipPlacement) { Console.Write($" {shipSymbol} "); }
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
                Console.WriteLine();
            }

            Console.Write("   ");
            for (int i = 0; i < Size; i++)
            {
                Console.Write("---");
            }
            Console.WriteLine();

            Console.Write("   "); // Kolumny
            for (int i = 0; i < Size; i++)
            {
                Console.Write(" " + i + " ");
            }
            Console.Write("\n\n");
        }

        // Metoda sprawdzająca czy wszystkie statki zostały zatopione
        public bool AreAllShipsSunk()
        {
            foreach (var row in Grid)
            {
                foreach (var cell in row)
                {
                    if (cell.State is UnattackedOccupiedState)
                    {
                        return false; // Nie wszystkie zajęte komórki są trafione
                    }
                }
            }
            return true; // Wszystkie komórki są trafione
        }
    }

}