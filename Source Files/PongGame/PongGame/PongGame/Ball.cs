using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

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
        public int StartPositionX { get; set; }
        public int StartPositionY { get; set; }
        public double xSpeed { get; set; }
        public double ySpeed { get; set; }
        public Texture2D Graphic { get; set; }

        public void Movement(double speed)
        {
            Random random = new Random();
            double xSpeed = 0;
            double ySpeed = 0;
            if (this.IsMoving == false)
            {
                this.PositionX = this.StartPositionX;
                this.PositionY = this.StartPositionY;
                while (((xSpeed <= 1 && xSpeed >= -1) || (ySpeed <= 1 && ySpeed >= -1)) ||
                    (xSpeed == ySpeed))
                {
                    this.IsMoving = true;
                    int directionDeg = random.Next(60);
                    int rand = random.Next(3);
                    switch (rand)
                    {
                        case 0:
                            break;
                        case 1:
                            directionDeg += 135;
                            break;
                        case 2:
                            directionDeg += 180;
                            break;
                        case 3:
                            directionDeg += 305;
                            break;
                    }

                    float directionDegRads = MathHelper.ToRadians(directionDeg);
                    xSpeed = Math.Sin(directionDegRads) * ((speed*4)/5);
                    ySpeed = Math.Cos(directionDegRads) * ((speed*4)/5);
                    this.xSpeed = xSpeed;
                    this.ySpeed = ySpeed;
                }
            }
            this.PositionX += (int)this.xSpeed;
            this.PositionY += (int)this.ySpeed;
        }

        public int OutOfBounds(int screenX, int screenY)
        {
            if ((this.PositionX <= 0) || (this.PositionX >= (screenX - this.Height)))
            {
                double xSpeed = this.xSpeed;
                this.xSpeed = -xSpeed;
                return 0;
            }
            else if (this.PositionY <= 0)
            {
                this.IsMoving = false;
                return 1;
            }
            else if (this.PositionY >= screenY)
            {
                this.IsMoving = false;
                return 2;
            }
            else
            {
                return 0;
            }
        }

        public void Collisions(Player player, int speed)
        {
            double batTop = player.PositionX;
            double batLeft = player.PositionY;
            double batBot = player.PositionX + player.Height;
            double batRight = player.PositionY + player.Width;
            double batTopBoundary = player.PositionX + this.xSpeed + player.SpeedX;
            double batLeftBoundary = player.PositionY + this.ySpeed;
            double batBotBoundary = player.PositionX + player.Height + (player.SpeedX + this.xSpeed);
            double batRightBoundary = player.PositionY + player.Width + this.ySpeed;
            double ballTop = this.PositionX;
            double ballLeft = this.PositionY;
            double ballBot = this.PositionX + this.Height;
            double ballRight = this.PositionY + this.Width;

            if ((this.ySpeed < 0) && // Right Bat Collision
                (((ballTop <= batBot) && (ballTop >= batTop)) || 
                ((ballBot <= batBot) && (ballBot >= batTop))) &&
                (ballLeft < batRight) && (ballLeft > batRightBoundary))
            {
                double newSpeed = (this.ySpeed-1);
                this.ySpeed = -newSpeed;
                this.PositionY = (player.PositionY + player.Width);
            }
            if ((this.ySpeed > 0) && // Left Bat Collision
                ((ballTop <= batBot && ballTop >= batTop) || 
                (ballBot <= batBot && ballBot >= batTop)) &&
                ballRight > batLeft && ballRight < batLeftBoundary)
            {
                double newSpeed = (this.ySpeed+1);
                this.ySpeed = -newSpeed;
                this.PositionY = (player.PositionY-this.Width);
            }
            if ((this.xSpeed < 0) && // Bot Bat Collision
                (((ballLeft <= batRight - (this.ySpeed/2)) && ballLeft >= batLeft) ||
                (ballRight <= batLeft + (this.ySpeed/2) && ballRight >= batRight)) &&
                ballTop > batBot && ballTop < batBotBoundary)
            {
                double newSpeed = this.xSpeed;
                this.xSpeed = -newSpeed;
                this.PositionX = ((int)player.PositionX+player.Height);
            }
            if ((this.xSpeed > 0) && // Top Bat Collision
                (((ballLeft <= batRight - (this.ySpeed/2)) && ballLeft >= batLeft) ||
                (ballRight <= batLeft + (this.ySpeed/2) && ballRight >= batRight)) &&
                ballBot > batTop && ballBot < batTopBoundary)
            {
                double newSpeed = this.xSpeed;
                this.xSpeed = -newSpeed;
                this.PositionX = ((int)player.PositionX - this.Height);
            }
        }
    }
}
