using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleships.Cells;
using Battleships.Ships;

namespace Battleships.Commands
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

            if (!Board.IsInBounds(StartX, StartY) && !Board.IsInBounds(EndX, EndY))
            {
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
        }
    }
}
