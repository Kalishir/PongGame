using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PongGame
{
    class Ball : Circle, Quad
    {
        public bool CanCollide
        {
            get
            {
                return true;
            }
        }
        public bool IsDrawn
        {
            get
            {
                return true;
            }
        }
        public int Height { get; set; }
        public int Width { get; set; }
        public float Radius { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public bool IsMoving { get; set; }
        public int DrawPositionX { get; set; }
        public int DrawPositionY { get; set; }
        public double xSpeed { get; set; }
        public double ySpeed { get; set; }
    }
}
