using Apus.Models;
using System;

namespace Apus
{
    class Program
    {
        static void Main()
        {
            do
            {
                BoardGame board = new BoardGame(4, 4, 8);
                board.GenerateGrid();
                do
                {
                    Console.Clear();
                    board.PrintHeader(" Minesweeper - Console Application v. 1.0");
                    board.PrintGrid(false);
                    board.ReadQuadrant("Please enter a column and row(e.g.A8): ");
                } while (board.GameOver == false);
                Console.Clear();
                board.PrintGrid(true);
                board.PrintGameOverMessage();
            } while (Console.ReadLine() != "QUIT");

            Console.ReadLine();
        }
    }
}
