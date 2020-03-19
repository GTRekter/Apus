using System;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Apus.Extensions;

namespace Apus.Models
{
    public class BoardGame
    {
        #region Properties
        /// <summary>
        /// Show if the game is over
        /// </summary>
        public bool GameOver { get; set; }
        /// <summary>
        /// Show if the user has won the game
        /// </summary>
        public bool HasWin { get; set; }
        /// <summary>
        /// Grid's height
        /// </summary>
        private int Height { get; set; }
        /// <summary>
        /// Grid's width
        /// </summary>
        private int Width { get; set; }
        /// <summary>
        /// Numer of mines
        /// </summary>
        private int Mines { get; set; }
        /// <summary>
        /// Number of available quadrants
        /// </summary>
        private int AvailableQuadrants { get; set; }
        /// <summary>
        /// Color for promps
        /// </summary>
        private readonly ConsoleColor PromptColor = ConsoleColor.White;
        /// <summary>
        /// Color for user inputs
        /// </summary>
        private readonly ConsoleColor InputColor = ConsoleColor.Yellow;
        /// <summary>
        /// Color for errors
        /// </summary>
        private readonly ConsoleColor ErrorColor = ConsoleColor.Red;
        /// <summary>
        /// Color for success
        /// </summary>
        private readonly ConsoleColor SuccessColor = ConsoleColor.Green;
        /// <summary>
        /// Matrix of quadrants with mines, values, etc.
        /// </summary>
        private Quadrant[,] Grid { get; set; }
        #endregion

        #region Constructor
        public BoardGame(int height, int width, int mines)
        {
            Height = height;
            Width = width;
            Mines = mines;
            AvailableQuadrants = (height * width) - mines;
        }
        public BoardGame(int height, int width, int mines, ConsoleColor promptColor, ConsoleColor inputColor, ConsoleColor errorColor, ConsoleColor successColor)
        {
            Height = height;
            Width = width;
            Mines = mines;
            PromptColor = promptColor;
            InputColor = inputColor;
            ErrorColor = errorColor;
            SuccessColor = successColor;
            AvailableQuadrants = (height * width) - mines;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Generate a new grid. This method also place a N numer of mines in random positions and set the value of the other quadrant with the numer of sorrounding mines.
        /// </summary>
        public void GenerateGrid()
        {
            int minesCount = 0;
            var randomNumber = new Random();

            // Initialize a new Matrix of quadrants and reset variables
            GameOver = false;
            HasWin = false;
            Grid = new Quadrant[Width, Height];

            // Add mines to the grid
            while(minesCount != Mines)
            {
                int randomWidth = randomNumber.Next(0, Width);
                int randomHeight = randomNumber.Next(0, Height);
                if (Grid[randomWidth, randomHeight] == null)
                {
                    Grid[randomWidth, randomHeight] = new Quadrant(true);
                    minesCount++;
                }
            } 

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
        /// <summary>
        /// Ask the user to insert the coordinates of a point, check if the value is valid and then check if the user has won or not.
        /// </summary>
        /// <param name="prompt">Message to pront to the user</param>
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

                    var row = 0;
                    var resultConversion = int.TryParse(match.Groups[2].Value, out row);
                    if (row < Width && column < Height && Grid[row, column].Selected == false && resultConversion)
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
        /// <summary>
        /// Print the grid with the headers
        /// </summary>
        /// <param name="visible">Show or hide the mines</param>
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
                                Console.Write("{0,18}","");
                            }
                            else
                            {
                                Console.ForegroundColor = PromptColor;
                                Console.Write("{0,17} ", r - 1);
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
        /// <summary>
        /// Print the winning or loosing message
        /// </summary>
        public void PrintGameOverMessage()
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            if (!HasWin)
            {
                Console.ForegroundColor = ErrorColor;
                Console.WriteLine(new string('-', 42));
                PrintSkull();
                Console.WriteLine(new string('-', 42));
                Console.WriteLine("{0,24}"," YOU DIED");
                Console.WriteLine(new string('-', 42));           
            }
            else
            {
                Console.ForegroundColor = SuccessColor;
                Console.WriteLine(new string('-', 42));
                PrintShuttle();
                Console.WriteLine(new string('-', 42));
                Console.WriteLine("** CONGRATULATIONS YOU SURVIVED! **");
                Console.WriteLine(new string('-', 42));
            }
            Console.ForegroundColor = PromptColor;
            Console.WriteLine("Press <ANY> key to play again");
            Console.WriteLine("Enter <QUIT> to Exit");
            Console.ForegroundColor = originalColor;
        }
        /// <summary>
        /// Print several utility messages like the number of available uadrant, time, etc..
        /// </summary>
        public void PrintUtilitiesMessages()
        {
            Console.WriteLine(new string('-', 42));
            Console.WriteLine("Available quadrants: {0}", AvailableQuadrants);
        }
        #endregion
        #region Private Methods
        /// <summary>
        /// Print a skull using ASCII
        /// </summary>
        private void PrintSkull()
        {
            Console.WriteLine("{0,27}", "#############");
            Console.WriteLine("{0,29}", "##############*##");
            Console.WriteLine("{0,30}", "################**#");
            Console.WriteLine("{0,31}", "#################***#");
            Console.WriteLine("{0,32}", "##################****#");
            Console.WriteLine("{0,33}", "###################*****#");
            Console.WriteLine("{0,33}", "####   ###########   ***#");
            Console.WriteLine("{0,33}", "###      #######      **#");
            Console.WriteLine("{0,33}", "###   X   #####   X   **#");
            Console.WriteLine("{0,33}", "####     ## # ##     ***#");
            Console.WriteLine("{0,33}", "########## ### ##*******#");
            Console.WriteLine("{0,32}", "### ############**# ###");
            Console.WriteLine("{0,28}", "##-#-#-#-#-#-##");
            Console.WriteLine("{0,27}", "| | | | | | |");
        }

        /// <summary>
        /// Print a shuttle using ASCII
        /// </summary>
        private void PrintShuttle() 
        { 
            Console.WriteLine("{0,18}","^");
            Console.WriteLine("{0,19}", "/ \\");
            Console.WriteLine("{0,20}", "/   \\");
            Console.WriteLine("{0,21}", "/     \\");
            Console.WriteLine("{0,22}", "|       |");
            Console.WriteLine("{0,25}", "^ |    ^   | ^ ");
            Console.WriteLine("{0,25}", "| ||  ( )  || |");
            Console.WriteLine("{0,25}", "|_|| /'_'\\ ||_|");
            Console.WriteLine("{0,25}", "| ||/ '_' \\|| |");
            Console.WriteLine("{0,25}", "|_|/  '_'  \\|_|");
            Console.WriteLine("{0,25}", "| /   '_'   \\ |");
            Console.WriteLine("{0,25}", "|<___' | '___>|");
            Console.WriteLine("{0,25}", "| |   ^^^   | |");
            Console.WriteLine("{0,25}", "/_\\         /_\\");
        }
        /// <summary>
        /// Check if the user has hit a mine or has won the game
        /// </summary>
        /// <param name="x">X-axis coordinates</param>
        /// <param name="y">Y-axis coordinates</param>
        private void ValidateSelection(int x, int y)
        {
            if (Grid[x, y].IsMine)
            {
                GameOver = true;
            }
            else
            {
                if(--AvailableQuadrants == 0)
                {
                    GameOver = true;
                    HasWin = true;
                }
            }
        }
        /// <summary>
        /// Calculate the number of mines around the current quadrant
        /// </summary>
        /// <param name="x">X-axis coordinates</param>
        /// <param name="y">Y-axis coordinates</param>
        /// <returns>Mines count</returns>
        private int GetMinesAround(int x, int y)
        {
            int mines = 0;
            int minimumX = x - 1;
            int minimumY = y - 1;
            int maximumX = x + 1;
            int maximumY = y + 1;

            if (x-1 < 0)
            {
                minimumX = x;
            }
            if (x + 1 >= Width)
            {
                maximumX = x;
            }
            if (y - 1 < 0)
            {
                minimumY = y;
            }
            if (y + 1 >= Height)
            {
                maximumY = y;
            }
            for (int i = minimumX; i <= maximumX; i++)
            {
                for (int j = minimumY; j <= maximumY; j++)
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