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
        private Player Player { get; set; }
        private IShip Ship { get; set; }
        private int StartX {  get; set; }
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

            // Przejrzenie komórek, na których znajduje się statek na podstawie współrzędnych początkowych i końcowych
            for (int i = 0; i < Ship.Size; i++)
            {
                int x = StartX;
                int y = StartY;

                if (StartX == EndX)
                {
                    y += i; // Położenie pionowe
                }
                else
                {
                    x += i; // Położenie poziome
                }

                // Sprawdzenie czy komórka jest już zajęta
                var cell = Board.GetCell(x, y);
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
            IShip decoratedShip = player.PlayerId == 1 ? new Player1ShipDecorator(ship, player.PlayerId) : new Player2ShipDecorator(ship, player.PlayerId);

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
