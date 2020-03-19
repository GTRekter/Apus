using System;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Apus.Extensions;

namespace Apus.Models
{
    public class BoardGame
    {
        #region Constants
        private const ConsoleColor PromptColor = ConsoleColor.White;
		private const ConsoleColor InputColor = ConsoleColor.Yellow;
        private const ConsoleColor ErrorColor = ConsoleColor.Red;
        private const ConsoleColor SuccessColor = ConsoleColor.Green;
        #endregion

        #region Properties
        public bool GameOver { get; set; }
        public bool HasWin { get; set; }
        private int Height { get; set; }
        private int Width { get; set; }
        private int Mines { get; set; }
        private int AvailableQuadrants { get; set; }
        private Quadrant[,] Grid { get; set; }
        #endregion

        #region Constructor
        public BoardGame(int height, int width, int mines)
        {
            Height = height;
            Width = width;
            Mines = mines;
        }
        #endregion

        #region Public Methods
        public void GenerateGrid()
        {
            int minesCount = 0;
            var randomNumber = new Random();
            
            // Initialize a new Matrix of quadrants
            Grid = new Quadrant[Width, Height];

            // Add mines to the grid
            do
            {
                int randomWidth = randomNumber.Next(0, Width);
                int randomHeight = randomNumber.Next(0, Height);
                if (Grid[randomWidth, randomHeight] == null)
                {
                    Grid[randomWidth, randomHeight] = new Quadrant(true);
                    minesCount++;
                }
            } while (minesCount != Mines);

            // Popolate the rest of the grid and add values
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if(Grid[i, j] == null)
                    {
                        int minues = GetMinesAround(i, j);
                        Grid[i, j] = new Quadrant(minues, false);
                    }               
                }
            }
        }
        public void ReadQuadrant(string prompt)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            try
            {
                bool done = false;
                string input = string.Empty;
                while (!done)
                {
                    Console.ForegroundColor = PromptColor;
                    Console.Write(prompt);
                    Console.ForegroundColor = InputColor;                  
                    input = Console.ReadLine();

                    // I slipt the input string into two groups. The first is will contains the chars and the second the digits.
                    var match = Regex.Match(input, @"([|A-Z|a-z|]+)([\d]+)");
                    var column = match.Groups[1].Value.ToAlphabethNumber();
                    var row = int.Parse(match.Groups[2].Value);
                    if (input.Length > 0 && column < Width && row < Height && Grid[row, column].Selected == false)
                    {
                        Grid[row, column].Selected = true;
                        ValidateSelection(row, column);
                        done = true;          
                    }                     
                }      
            }
            catch (Exception ex)
            {
                string location = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "." + System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
                Console.ForegroundColor = ErrorColor;
                Console.WriteLine("An unexpected error occurred in {0}: {1}", location, ex.Message);
                throw ex;
            }
            finally
            {
                Console.ForegroundColor = originalColor;
            }
        }
        public void PrintGrid(bool visible)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            try
            {
                for (int r = 0; r <= Width; r++)
                {
                    for (int c = 0; c <= Height; c++)
                    {
                        // Print headers
                        if (c == 0)
                        {
                            if (r == 0)
                            {
                                Console.Write("  ");
                            }
                            else
                            {
                                Console.ForegroundColor = PromptColor;
                                Console.Write("{0} ", r - 1);
                            }
                        }
                        else if (r == 0)
                        {
                            Console.ForegroundColor = PromptColor;
                            // ASCII alphabeth characters start 65 to 90
                            Console.Write("{0} ", c.ToAlphabethLetters());
                        }

                        // Print values and mines
                        if (r > 0 && c > 0)
                        {
                            Console.ForegroundColor = InputColor;
                            if (visible)
                            {
                                if (Grid[r - 1, c - 1].IsMine)
                                {
                                    Console.ForegroundColor = ErrorColor;
                                    Console.Write("m ");
                                }
                                else
                                {
                                    Console.Write("{0} ", Grid[r - 1, c - 1].Value.ToString());
                                }
                                continue;
                            }
                            Console.Write("{0} ", Grid[r - 1, c - 1].Selected ? Grid[r - 1, c - 1].Value.ToString() : "*");
                        }
                    }
                    Console.Write(Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                string location = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "." + System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
                Console.ForegroundColor = ErrorColor;
                Console.WriteLine("An unexpected error occurred in {0}: {1}", location, ex.Message);
                throw ex;
            }
            finally
            {
                Console.ForegroundColor = originalColor;
            }
        }
        public void PrintHeader(string prompt)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = PromptColor;
            Console.WriteLine(new string('-', 42));
            Console.WriteLine(prompt);
            Console.WriteLine(new string('-', 42));
            Console.ForegroundColor = originalColor;
        }
        public void PrintGameOverMessage()
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            if (!HasWin)
            {
                Console.ForegroundColor = ErrorColor;
                Console.WriteLine("** Sorry you hit a mine **");
                Console.WriteLine("-- Press any key to play again --");
            }
            else
            {
                Console.ForegroundColor = SuccessColor;
                Console.WriteLine("** Congratulations you won! **");
            }
            Console.ForegroundColor = PromptColor;
            Console.Write("Enter");
            Console.ForegroundColor = InputColor;
            Console.Write(" <QUIT> ");
            Console.ForegroundColor = PromptColor;
            Console.WriteLine("to Exit");
            Console.ForegroundColor = originalColor;
        }
        #endregion

        #region Private Methods
        private void ValidateSelection(int row, int column)
        {
            if (Grid[row, column].IsMine)
            {
                GameOver = true;
            }
            else
            {
                if(AvailableQuadrants-- == 0)
                {
                    GameOver = false;
                    HasWin = true;
                }
            }
        }
        /// <summary>
        /// Calculate number of mines around the current quadrant
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>Mines count</returns>
        private int GetMinesAround(int row, int column)
        {
            int mines = 0;
            int topLeftRow = row - 1;
            int topLeftColumn = column - 1;
            int bottomRightRow = row + 1;
            int bottomRightColumn = column + 1;

            if (row-1 < 0)
            {
                topLeftRow = row;
            }
            if (row + 1 >= Width)
            {
                bottomRightRow = row;
            }
            if (column - 1 < 0)
            {
                topLeftColumn = column;
            }
            if (column + 1 >= Height)
            {
                bottomRightColumn = column;
            }
            for (int i = topLeftRow; i <= bottomRightRow; i++)
            {
                for (int j = topLeftColumn; j <= bottomRightColumn; j++)
                {
                    if (Grid[i, j] != null && Grid[i, j].IsMine)
                    {
                        mines++;
                    }
                }
            }
            return mines;
        }
        #endregion
    }
}