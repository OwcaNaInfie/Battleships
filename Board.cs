using Battleships.Ships;
using Battleships.Cells;
using System.IO;

namespace Battleships
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
        public string GetBoardString(bool showShipPlacement = true, char shipSymbol = '.')
        {
            StringWriter sw = new StringWriter();  

            for (int y = 0; y < Size; y++)
            {
                sw.Write(y + " |"); // Rows

                for (int x = 0; x < Size; x++)
                {
                    Cell cell = Grid[x][y];

                    switch (cell.State)
                    {
                        case UnattackedOccupiedState:
                            if (showShipPlacement)
                            {
                                sw.Write($" {shipSymbol} ");
                            }
                            else
                            {
                                sw.Write(" . ");
                            }
                            break;
                        case HitOccupiedState:
                            sw.Write(" X ");
                            break;
                        case UnattackedEmptyState:
                            sw.Write(" . ");
                            break;
                        case HitEmptyState:
                            sw.Write(" O ");
                            break;
                    }
                }
                sw.WriteLine();
            }

            sw.Write("   ");
            for (int i = 0; i < Size; i++)
            {
                sw.Write("---");
            }
            sw.WriteLine();

            sw.Write("   "); // Kolumny
            for (int i = 0; i < Size; i++)
            {
                sw.Write(" " + i + " ");
            }
            sw.Write("\n\n");

           
            return sw.ToString();
        }

        // Metoda wyświetlająca planszę
        public void DisplayBoard(bool showShips, char shipSymbol = '.')
        {
            Console.WriteLine(GetBoardString(showShips, shipSymbol)); 
        }

        // Klon planszy potrzebny do poprawnego działania historii
        public Board DeepClone()
        {
            Board clone = new Board(this.Size);

            clone.Grid = new List<List<Cell>>();
            for (int x = 0; x < this.Size; x++)
            {
                var newRow = new List<Cell>();
                for (int y = 0; y < this.Size; y++)
                {
                    newRow.Add(this.Grid[x][y].CloneCell());
                }
                clone.Grid.Add(newRow);
            }

            clone.Ships = new List<IShip>();
            foreach (var ship in this.Ships)
            {
                clone.Ships.Add(ship.Clone()); 
            }

            return clone;
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