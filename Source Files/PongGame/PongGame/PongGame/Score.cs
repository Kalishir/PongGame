using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PongGame
{
    interface Score : GameObjects
    {
        int Score { get; set; }
        Vector2 ScorePosition { get; set; }
    }
}
