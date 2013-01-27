using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PongGame
{
    class Player : Paddle, Score
    {
        public Player(bool AI)
        {
            this.isAI = AI;
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
        public bool isAI { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public double PositionX { get; set; }
        public int PositionY { get; set; }
        public int Score { get; set; }
        public double SpeedX { get; set; }
        public int Speed { get; set; }
        public Vector2 ScorePosition { get; set; }
        private int aiDesiredXPosition;
        public string ScoreAsString { get; set; }

        public void AddScore()
        {
            Score++;
            ScoreAsString = Convert.ToString(Score);
        }

        public void Movement(KeyboardState state, int screenX)
        {
            if (this.isAI == false)
            {
                if (state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.Up))
                {
                    this.SpeedX = -this.Speed;
                }
                else if (state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.Down))
                {
                    this.SpeedX = this.Speed;
                }
                else 
                {
                    this.SpeedX = 0;
                }
            }
            if (isAI == true)
            {

                if (aiDesiredXPosition < this.PositionX)
                {
                    double positionDiff = (PositionX - aiDesiredXPosition);
                    SpeedX = -(positionDiff / 10);
                    if (SpeedX < -Speed)
                    {
                        SpeedX = -Speed;
                    }
                }
                else if (aiDesiredXPosition > this.PositionX)
                {
                    double positionDiff = (aiDesiredXPosition - PositionX);
                    SpeedX = (positionDiff/10);
                    if (SpeedX > Speed)
                    {
                        SpeedX = Speed;
                    }
                }
                else
                {
                    this.SpeedX = 0;
                }
            }

            this.PositionX += this.SpeedX;

            PlayerOutOfBounds(screenX);
        }

        public void AIMovementCalculate(Random random, Ball ball, int screenX)
        {
            int aiRange = ((this.Height*4) / 4);
            int aiRandom = random.Next(aiRange) + 1;
            int signRandom = random.Next(1);
            switch (signRandom)
            {
                case 0:
                    aiDesiredXPosition = (ball.PositionX + aiRandom + ((int)ball.xSpeed*15) + (ball.Height / 2) - (this.Height / 2));
                    break;
                case 1:
                    aiDesiredXPosition = (ball.PositionX + ((int)ball.xSpeed * 15) + (ball.Height / 2) - ((this.Height / 2) + aiRandom));
                    break;
            }
            
        }

        private void PlayerOutOfBounds(int screenX)
        {
            if (this.PositionX < 0)
            {
                this.PositionX = 0;
            }
            if (this.PositionX + (this.Height) > screenX)
            {
                this.PositionX = screenX - (this.Height);
            }
        }
    }
}
