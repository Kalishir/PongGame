using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PongGame
{
    interface Paddle : Quad, GameObjects
    {
        double PositionX { get; set; }
        int PositionY { get; set; }
    }
}
