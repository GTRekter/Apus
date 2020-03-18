using System;
using System.Collections.Generic;
using System.Text;

namespace Apus.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Convert a string to the sum of ASCII number of each charachter
        /// </summary>
        /// <param name="s">String to convert</param>
        /// <returns></returns>
        public static int ToNumber(this string s)
        {
            int result = 0;
            for (int i = 0; i < s.Length; i++)
            {
                result += (int)s[i];
            }
            return result;
        }

        /// <summary>
        /// Convert a string to the a number based on the position on the aplhabeth of each charchter
        /// </summary>
        /// <param name="s">String to convert</param>
        /// <returns></returns>
        public static int ToAlphabethNumber(this string s)
        {
            int result = 0;
            for (int i = 0; i < s.Length; i++)
            {
                result += (int)s[i]-(char)'A';
            }
            return result ;
        }
    }
}
