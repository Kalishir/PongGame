using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PongGame
{
    interface Score : GameObjects
    {
        int Score { get; set; }
    }
}
