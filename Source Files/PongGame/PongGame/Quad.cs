using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PongGame
{
    interface Quad : GameObjects
    {
        int Height { get; set; }
        int Width { get; set; }
    }
}
