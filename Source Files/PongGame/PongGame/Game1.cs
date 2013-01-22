using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PongGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Random random = new Random();
        Player humanPlayer = new Player("Human");
        Player aiPlayer = new Player("AI");
        Ball ball = new Ball();



        // Grab the screen size from the system and use it for the game
        int screenX = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height; 
        int screenY = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        int speed;
        int AIMovementCount;
        int aiDesiredXPosition;

        Texture2D ballGraphic;
        Texture2D paddleGraphic;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // All these values are designed to set the game so it scales based on window size
            speed = screenX / 60;
            int screenTenPercentX = screenX / 10;
            int screenTenPercentY = screenY / 10;
            humanPlayer.Height = screenTenPercentX;
            humanPlayer.Width = humanPlayer.Height / 5;
            humanPlayer.PositionX = screenX / 2;
            humanPlayer.PositionY = screenTenPercentY;
            aiPlayer.Height = screenTenPercentX;
            aiPlayer.Width = aiPlayer.Height / 5;
            aiPlayer.PositionX = screenX / 2;
            aiPlayer.PositionY = screenY - (screenTenPercentY+(aiPlayer.Width/2));
            ball.Height = humanPlayer.Height / 5;
            ball.Width = ball.Height;
            ball.Radius = ball.Height / 2;
            ball.PositionX = screenX / 2;
            ball.PositionY = screenY / 2;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ballGraphic = Content.Load<Texture2D>("Ball");
            paddleGraphic = Content.Load<Texture2D>("Paddle");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            PlayerMovement();
            BallMovement();
            if (AIMovementCount == 15)
            {
                AIMovementCalculate();
                AIMovementCount = 0;
            }
            AIMovement();
            DetectCollisions();
            BallOutOfBounds();
            // TODO: Add your update logic here

            AIMovementCount++;
            DrawPositionUpdate();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            ball.DrawPositionX = ball.PositionX - (ball.Height / 2);
            ball.DrawPositionY = ball.PositionY - (ball.Width / 2);

            spriteBatch.Begin();

            spriteBatch.Draw(ballGraphic, new Rectangle(ball.DrawPositionY, ball.DrawPositionX, ball.Width, ball.Height), Color.White);
            spriteBatch.Draw(paddleGraphic, new Rectangle(humanPlayer.DrawPositionY, humanPlayer.DrawPositionX, humanPlayer.Width, humanPlayer.Height), Color.White);
            spriteBatch.Draw(paddleGraphic, new Rectangle(aiPlayer.DrawPositionY, aiPlayer.DrawPositionX, aiPlayer.Width, aiPlayer.Height), Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public void AIMovementCalculate()
        {
            int aiRandom = random.Next(9) + 1;
            int signRandom = random.Next(1);
            if (signRandom == 0)
            {
                float newXPosition = ((ball.PositionX * aiRandom)/100);
                aiDesiredXPosition = (int)newXPosition+ball.PositionX;
            }
            else if (signRandom == 1)
            {
                float newXPosition = ((ball.PositionX * aiRandom) / 100);
                aiDesiredXPosition = (int)newXPosition - ball.PositionX;
            }
        }

        public void AIMovement()
        {
            if (aiDesiredXPosition < aiPlayer.PositionX)
            {
                aiPlayer.PositionX -= ((speed / 5) * 4);
                if (aiPlayer.PositionX + (aiPlayer.Height / 2) > screenX)
                {
                    aiPlayer.PositionX = screenX - (aiPlayer.Height / 2);
                }
            }
            if (aiDesiredXPosition > aiPlayer.PositionX)
            {
                aiPlayer.PositionX += ((speed / 5) * 4);
                if (aiPlayer.PositionX - (humanPlayer.Height / 2) < 0)
                {
                    aiPlayer.PositionX = 0 + (humanPlayer.Height / 2);
                }
            }
        }

        public void BallOutOfBounds()
        {
            if ((ball.PositionX <= 0) || (ball.PositionX >= screenX))
            {
                double xSpeed = ball.xSpeed;
                ball.xSpeed = -xSpeed;
            }
            else if (ball.PositionY <=0)
            {
                aiPlayer.Score += 1;
                ball.PositionX = screenX / 2;
                ball.PositionY = screenY / 2;
                ball.IsMoving = false;
            }
            else if (ball.PositionY >= screenY)
            {
                humanPlayer.Score += 1;
                ball.PositionX = screenX / 2;
                ball.PositionY = screenY / 2;
                ball.IsMoving = false;
            }
        }
        public void BallMovement()
        {
            double xSpeed;
            double ySpeed;
            if (ball.IsMoving == false)
            {
                ball.IsMoving = true;
                int directionDeg = random.Next(45)+35;
                xSpeed = Math.Sin(directionDeg) * ((speed/3)*2);
                ySpeed = Math.Cos(directionDeg) * ((speed/3)*2);

                if (xSpeed < 1 && xSpeed > 0)
                {
                    xSpeed = 1;
                }
                else if (xSpeed > -1 && xSpeed < 0)
                {
                    xSpeed = -1;
                }
                if (ySpeed < 1 && ySpeed > 0)
                {
                    ySpeed = 1;
                }
                else if (ySpeed > -1 && ySpeed < 0)
                {
                    ySpeed = -1;
                }
                int randNum = random.Next(3);
                if (randNum == 0)
                {
                    ball.xSpeed = xSpeed;
                    ball.ySpeed = ySpeed;
                }
                else if (randNum == 1)
                {
                    ball.xSpeed = -xSpeed;
                    ball.ySpeed = ySpeed;
                }
                else if (randNum == 2)
                {
                    ball.xSpeed = xSpeed;
                    ball.ySpeed = -ySpeed;
                }
                else
                {
                    ball.xSpeed = -xSpeed;
                    ball.ySpeed = -ySpeed;
                }
            }
            ball.PositionX += (int)ball.xSpeed;
            ball.PositionY += (int)ball.ySpeed;
        }

        public void DetectCollisions()
        {
            if (ball.ySpeed < 0)
            {
                if (ball.PositionY - (ball.Width / 2) < humanPlayer.PositionY + (humanPlayer.Width / 2) && ball.PositionY - (ball.Width / 2) > humanPlayer.PositionY - (humanPlayer.Width / 2))
                {
                    if (ball.PositionX + (ball.Height / 2) < humanPlayer.PositionX + (humanPlayer.Height / 2) && ball.PositionX + (ball.Height / 2) > humanPlayer.PositionX - (humanPlayer.Height / 2))
                    {
                        KeyboardState state = Keyboard.GetState();
                        if (state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.Up))
                        {
                            ball.xSpeed -= (speed / 10);
                            ball.ySpeed -= (speed / 10);
                        }
                        if (state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.Down))
                        {
                            ball.xSpeed += (speed / 10);
                            ball.ySpeed -= (speed / 10);
                        }
                        int newSpeed = (int)ball.ySpeed;
                        ball.ySpeed = -newSpeed;
                    }
                }
            }

            if (ball.ySpeed > 0)
            {
                if (ball.PositionY + (ball.Width / 2) > aiPlayer.PositionY - (aiPlayer.Width / 2) && ball.PositionY + (ball.Width / 2) < aiPlayer.PositionY + (aiPlayer.Width / 2))
                {
                    if (ball.PositionX - (ball.Height / 2) > aiPlayer.PositionX - (aiPlayer.Height / 2) && ball.PositionX - (ball.Height / 2) < aiPlayer.PositionX + (aiPlayer.Height / 2))
                    {
                        int newSpeed = (int)ball.ySpeed;
                        ball.ySpeed = -newSpeed;
                    }
                }
            }
        } 
        public void DrawPositionUpdate()
        {
            humanPlayer.DrawPositionX = humanPlayer.PositionX - (humanPlayer.Height / 2);
            humanPlayer.DrawPositionY = humanPlayer.PositionY - (humanPlayer.Width / 2);
            aiPlayer.DrawPositionX = aiPlayer.PositionX - (aiPlayer.Height / 2);
            aiPlayer.DrawPositionY = aiPlayer.PositionY - (aiPlayer.Width / 2);
            ball.DrawPositionX = ball.PositionX - (ball.Height / 2);
            ball.DrawPositionY = ball.PositionY - (ball.Width / 2);
        }

        public void PlayerMovement()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.Up))
            {
                humanPlayer.PositionX -= ((speed/5)*4);
                if ((humanPlayer.PositionX-(humanPlayer.Height/2)) < 0)
                {
                    humanPlayer.PositionX = (0+(humanPlayer.Height/2));
                }
            }
            if (state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.Down))
            {
                humanPlayer.PositionX += ((speed/5)*4);
                if (humanPlayer.PositionX + (humanPlayer.Height/2) > screenX)
                {
                    humanPlayer.PositionX = screenX - (humanPlayer.Height/2);
                }
            }
        }
    }
}
