using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleships.Cells;
using Battleships.Ships;
using Battleships.Players;


namespace Battleships.Commands
{
    public class PlaceShipCommand : ICommand
    {
        private Board Board { get; set; }
        private Player Player { get; set; }
        private IShip Ship { get; set; }
        private int StartX { get; set; }
        private int StartY { get; set; }
        private int EndX { get; set; }
        private int EndY { get; set; }
        public PlaceShipCommand(Player player, int shipSize, int startX, int startY, int endX, int endY)
        {
            Board = player.Board;
            Player = player;
            Ship = CreateShip(player, shipSize);
            StartX = startX;
            StartY = startY;
            EndX = endX;
            EndY = endY;
        }
        public bool Execute()
        {

            if (!Board.IsInBounds(StartX, StartY) && !Board.IsInBounds(EndX, EndY))
            {
                CancelShipPlacement(Player, Ship.Size);
                Console.WriteLine("No ship can be placed outside the board.");
                return false;
            }

            // Określenie długości statku na podstawie współrzędnych
            int length = 0;
            if (StartX == EndX)
            {
                // Położenie pionowe statku
                length = Math.Abs(EndY - StartY) + 1;
            }
            else if (StartY == EndY)
            {
                // Położenie poziome statku
                length = Math.Abs(EndX - StartX) + 1;
            }

            // Sprawdzenie czy długość równa się długości statku
            if (length != Ship.Size)
            {
                Console.WriteLine($"The length of the ship doesn't match the required size. The ship size is {Ship.Size}.");
                CancelShipPlacement(Player, Ship.Size);
                return false;
            }

            int x = StartX;
            int y = StartY;

            // W pętli poniżej komórki są zmieniane od lewej do prawej lub od góry do dołu
            // Jeśli użytkownik poda koordynaty "na opak", tzn. od prawej do lewej lub z dołu na górę
            // to trzeba "podmienic" koordynaty poczatkowe z koncowymi
            if ((StartX == EndX && StartY >= EndY) || (StartY == EndY && StartX >= EndX))
            {

                x = EndX;
                y = EndY;

            }

            // Przejrzenie komórek, na których znajduje się statek na podstawie współrzędnych początkowych i końcowych
            for (int i = 0; i < Ship.Size; i++)
            {
                //int x = StartX;
                //int y = StartY;


                if (StartX == EndX && i != 0)
                {
                    y++; // Położenie pionowe
                }
                else if (i != 0)
                {
                    x++; // Położenie poziome
                }

                // Sprawdzenie czy komórka jest już zajęta
                var cell = Board.GetCell(x, y);
                if (cell is null)
                {
                    Console.WriteLine("This cell is not on the board!");
                    return false;
                }
                if (cell.State is UnattackedOccupiedState)
                {
                    Console.WriteLine($"Error: Cell at ({x},{y}) is already occupied.");
                    CancelShipPlacement(Player, Ship.Size);
                    return false; // Jeśli komórka jest już zajęta - nieodpowiednie ułożenie statku
                }

                // Zaznaczenie komórki jako zajętej
                cell.MarkOccupied();
            }

            // Dodanie statku do listy
            Board.Ships.Add(Ship);
            // Wyświetlenie planszy
            Board.DisplayBoard(true, Ship.Symbol);

            return true;
        }

        public void Undo()
        {
            for (int i = 0; i < Ship.Size; i++)
            {
                int x = StartX;
                int y = StartY;

                if (StartX == EndX)
                {
                    y += i;
                }
                else
                {
                    x += i;
                }

                var cell = Board.GetCell(x, y);
                // Zaznaczenie komórki jako pustej
                cell.MarkUnattackedEmpty();
            }

            // Usunięcie statku z listy postawionych statków
            Board.Ships.Remove(Ship);
            // Zmniejszenie liczby statków danego typu w liczniku w fabryce
            CancelShipPlacement(Player, Ship.Size);
            // Wyświetlenie planszy
            Board.DisplayBoard(true, Ship.Symbol);
        }

        static IShip CreateShip(Player player, int shipSize)
        {
            ShipFactory shipFactory = shipSize switch
            {
                1 => OneMastFactory.Instance,
                2 => TwoMastFactory.Instance,
                3 => ThreeMastFactory.Instance,
                4 => FourMastFactory.Instance,
                _ => throw new ArgumentException("Invalid ship size")
            };

            // Stworzenie statku
            IShip ship = shipFactory.CreateShip(player.PlayerId);
            IShip decoratedShip = player.PlayerId == 1 ? new Player1ShipDecorator(ship, Program.player1SymbolOption) : new Player2ShipDecorator(ship, Program.player2SymbolOption);
            return decoratedShip;
        }

        static void CancelShipPlacement(Player player, int shipSize)
        {
            ShipFactory shipFactory = shipSize switch
            {
                1 => OneMastFactory.Instance,
                2 => TwoMastFactory.Instance,
                3 => ThreeMastFactory.Instance,
                4 => FourMastFactory.Instance,
                _ => throw new ArgumentException("Invalid ship size")
            };
            shipFactory.CancelShipPlacement(player.PlayerId);
        }
    }


}
