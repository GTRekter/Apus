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
        #endregion

        #region Properties
        private int Height { get; set; }
        private int Width { get; set; }
        private int Mines { get; set; }
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
        public void PrintHeader(string prompt)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = PromptColor;
                Console.WriteLine(new string('-', 42));
                Console.WriteLine(prompt);
                Console.WriteLine(new string('-', 42));
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
                        Grid[i, j] = new Quadrant(5, false);
                    }               
                }
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
                        if(c == 0)
                        {
                            if (r == 0)
                            {
                                Console.Write("  ");
                            }
                            else
                            {
                                Console.ForegroundColor = PromptColor;
                                Console.Write("{0} ", r-1);
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
                                Console.Write("{0} ", !Grid[r-1, c-1].IsMine ? Grid[r-1, c-1].Value.ToString() : "m");
                                continue;
                            }
                            Console.Write("{0} ", Grid[r-1, c-1].Selected ? Grid[r-1, c-1].Value.ToString() : "*");
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
                    if (input.Length > 0 && column < Width && row < Height)
                    {
                        Grid[row, column].Selected = true;
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
        #endregion



        /// <summary>
        /// Calculate number of mines around the current quadrant
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>Mines count</returns>
        //private int GetMinesAround(int x, int y)
        //{

        //    // pre-process first row
        //    for (int j = 1; j < 3; j++) 
        //    { 
        //        Grid[0, j] = Grid[0, j] + Grid[0, j - 1];
        //    }
        //    // pre-process first column
        //    for (int i = 1; i < 3; i++)
        //        Grid[i][0] = Grid[i][0] + Grid[i - 1][0];

        //    int mines = 0;
        //    for (int i = 0; i < 3; i++)
        //    {
        //        if(Grid[x, y + i].IsMine)
        //        {
        //            mines++;
        //        }

        //    }
        //}

    }
}
