using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PongGame
{
    class Player : Paddle, Score
    {
        private string name;

        public Player(string name)
        {
            this.name = name;
        }
        public bool IsDrawn
        {
            get
            {
                return true;
            }
        }
        public bool CanCollide
        {
            get
            {
                return true;
            }
        }
        public int Height { get; set; }
        public int Width { get; set; }
        public int PositionX { get; set; }
        public int DrawPositionX { get; set; }
        public int DrawPositionY { get; set; }
        public int PositionY { get; set; }
        public int Score { get; set; }
    }
}
