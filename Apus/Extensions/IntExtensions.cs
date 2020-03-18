using System;
using System.Collections.Generic;
using System.Text;

namespace Apus.Extensions
{
    public static class IntExtensions
    {
        /// <summary>
        /// Convert an int to the a string based on the position on the aplhabeth of each charchter
        /// </summary>
        /// <param name="s">String to convert</param>
        /// <returns></returns>
        public static string ToAlphabethLetters(this int value)
        {
            // TODO: To check
            string result = string.Empty;
            while (--value >= 0)
            {
                result = (char)('A' + value % 26) + result;
                value /= 26;
            }
            return result;
        }

    }
}
