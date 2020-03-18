using System;
using System.Collections.Generic;
using System.Text;

namespace Apus.Models
{
    public class Quadrant
    {
        public int Value { get; set; }
        public bool Selected { get; set; }
        public bool IsMine { get; set; }

        public Quadrant(bool isMine)
        {
            IsMine = isMine;
        }
        public Quadrant(int value, bool isMine)
        {
            Value = value;
            IsMine = isMine;
        }
    }
}
