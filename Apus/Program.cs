using Apus.Models;
using System;

namespace Apus
{
    class Program
    {

        static void Main(string[] args)
        {

            BoardGame board = new BoardGame(4, 4, 8);
            board.PrintHeader(" Minesweeper - Console Application v. 1.0");
            board.GenerateGrid();
            //do
            //{
            for (int i = 0; i < 10; i++)
            {
                board.PrintGrid(false);
                board.ReadQuadrant("Please enter a column and row(e.g.A8): ");
                Console.Clear();
            }
                
            //} while (board.GameOver == false);

            Console.ReadLine();
        }
    }
}
