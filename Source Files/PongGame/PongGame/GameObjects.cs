using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PongGame
{
    interface GameObjects
    {
        bool CanCollide { get; }
        bool IsDrawn { get; }
    }
}
