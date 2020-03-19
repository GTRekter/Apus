using Apus.Models;
using System;
using System.Globalization;

namespace Apus
{
    class Program
    {
        #region Constants
        private const string HeaderMessage = "Minesweeper - Console Application v. 1.0";
        private const ConsoleColor PromptColor = ConsoleColor.White;
        private const ConsoleColor InputColor = ConsoleColor.Yellow;
        private const ConsoleColor ErrorColor = ConsoleColor.Red;
        #endregion

        static void Main()
        {
            do
            {
                PrintHeader(HeaderMessage);
                BoardGame board = ConfigureGrid();
				board.GenerateGrid();
                do
                {
                    Console.Clear();
                    PrintHeader(HeaderMessage);
                    board.PrintGrid(false);
                    board.PrintUtilitiesMessages();
                    board.ReadQuadrant("Please enter a column and row(e.g.A8): ");
                } while (board.GameOver == false);
                Console.Clear();
                PrintHeader(HeaderMessage);
                board.PrintGrid(true);
                board.PrintGameOverMessage();
            } while (!Console.ReadLine().Equals("QUIT",StringComparison.InvariantCultureIgnoreCase));

            Console.ReadLine();
        }
        /// <summary>
        /// Print the header of the game
        /// </summary>
        /// <param name="prompt">Message to promp to the user</param>
        public static void PrintHeader(string prompt)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = PromptColor;
            Console.WriteLine(new string('-', 42));
            Console.WriteLine(prompt);
            Console.WriteLine(new string('-', 42));
            Console.ForegroundColor = originalColor;
        }
        /// <summary>
        /// Ask the user to insert the information required to generate a new grid
        /// </summary>
        /// <returns></returns>
        private static BoardGame ConfigureGrid()
        {
            int height = ReadInt("Insert the grid's height: ");
            int width = ReadInt("Insert the grid's width: ");
            int mines = ReadInt("Insert the numer of mines: ");
            return new BoardGame(height, width, mines);
        }
        /// <summary>
        /// Gets an int from the user
        /// </summary>
        /// <param name="prompt">Prompt to display to the user</param>
        /// /// <param name="lowValue">Input value must be greater than or equal to this</param>
        /// <param name="highValue">Input value must be less than or equal to this</param>
        private static int ReadInt(string prompt, int lowValue = int.MinValue, int highValue = int.MaxValue)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            bool done = false;
            int value = 0;
            while (!done)
            {
                // Prompt and get user input
                Console.ForegroundColor = PromptColor;
                Console.Write(prompt);

                Console.ForegroundColor = InputColor;
                string input = Console.ReadLine();

                // Check for valid type
                if (int.TryParse(input, NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands, NumberFormatInfo.CurrentInfo, out value))
                {
                    // check range
                    if ((value >= lowValue) && (value <= highValue))
                    {
                        // This will allow the loop to exit.
                        done = true;
                    }
                }
                else
                {
                    Console.ForegroundColor = ErrorColor;
                    Console.WriteLine("The value entered was not an integer.  Please try again.");
                }
            }
            return value;
        }
    }
}
