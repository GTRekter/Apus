using System;
using System.Collections.Generic;
using System.Text;

namespace Apus.Models
{
    public class Quadrant
    {
        #region Properties
        /// <summary>
        /// Numer of mines arount the quadrant
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// Show if the user has selected the current wuadrant
        /// </summary>
        public bool Selected { get; set; }
        /// <summary>
        /// Show if the quadrant is a mine
        /// </summary>
        public bool IsMine { get; set; }
        #endregion
        #region Constructor
        public Quadrant(bool isMine)
        {
            IsMine = isMine;
        }
        public Quadrant(int value, bool isMine)
        {
            Value = value;
            IsMine = isMine;
        }
        #endregion
    }
}
