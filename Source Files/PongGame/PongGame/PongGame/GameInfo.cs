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
    class GameInfo
    {

        public Random random { get; set; }

        public Player Player1 { get; set; }
        public Player Player2 { get; set; }

        public Ball ball { get; set; }

        public Menu menu { get; set; }

        public SpriteFont font { get; set; }

        public int screenX { get; set; }
        public int screenY { get; set; }
        public int speed { get; set; }
        public int AIMovementCount { get; set; }

        public Texture2D background { get; set; }

        public bool GameIsRunning { get; set; }
        public bool GameIsPaused { get; set; }
        public bool DrawControlsToggle { get; set; }
        public bool FullScreenToggle { get; set; }
        public bool EndGame { get; set; }
        public bool Win { get; set; }
        public bool Lose { get; set; }

        public GameInfo()
        {
            random = new Random();
            Player1 = new Player(false);
            Player2 = new Player(true);
            ball = new Ball();
            menu = new Menu();
            GameIsRunning = false;
            GameIsPaused = false;
        }

        public void ScreenSetup()
        {
            screenX = 480;
            screenY = (screenX * 16) / 9;
        }
        public void Setup()
        {
            speed = screenX / 60;
            Player1.Height = screenX / 8;
            Player1.Width = Player1.Height / 5;
            Player1.PositionX = ((screenX / 2) - (Player1.Height / 2));
            Player1.PositionY = ((screenY / 10) - (Player1.Width / 2));
            Player1.Speed = ((speed * 5) / 4);
            Player2.Height = screenX / 8;
            Player2.Width = Player2.Height / 5;
            Player2.PositionX = ((screenX / 2) - (Player2.Height / 2));
            Player2.PositionY = (screenY - ((screenY / 10) + (Player2.Width / 2)));
            Player2.Speed = ((speed * 5) / 4);
            ball.Height = Player1.Height / 5;
            ball.Width = ball.Height;
            ball.Radius = ball.Height / 2;
            ball.StartPositionX = ((screenX / 2) - (ball.Height / 2));
            ball.StartPositionY = ((screenY / 2) - (ball.Width / 2));
            ball.PositionX = ball.StartPositionX;
            ball.PositionY = ball.StartPositionY;
            Player1.ScorePosition = new Vector2((screenY / 4), screenX / 10);
            Player2.ScorePosition = new Vector2(((screenY / 4) + (screenY / 2)), screenX / 10);
            Player1.ScoreAsString = "0";
            Player2.ScoreAsString = "0";
        }

        public void LoadGraphics(ContentManager Content)
        {
            menu.LoadButtons(Content);
            ball.Graphic = Content.Load<Texture2D>("BallNew");
            Player1.Graphic = Content.Load<Texture2D>("PaddleNew");
            Player2.Graphic = Content.Load<Texture2D>("PaddleNew");
            background = Content.Load<Texture2D>("Background");
        }

        public void LoadFonts(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("Font");
        }

        public void Play(KeyboardState state)
        {
            if (FullScreenToggle == false)
            {
                if (GameIsRunning == false)
                {
                    menu.RunMenu(state, this);
                }
                else if ((GameIsRunning == true) && (GameIsPaused == false))
                {
                    Run(state);
                }
                else if ((GameIsRunning == true) && (GameIsPaused == true))
                {
                    menu.RunMenu(state, this);
                }
            }
            else
            {
                CheckExit(state, menu);
            }
        }

        private void Run(KeyboardState state)
        {
            if ((Player1.Score >= 20) || (Player2.Score >= 20))
            {
                CheckWin();
            }
            else
            {
                // Calls Methods to determine movement & update positions.
                ball.Movement(speed);
                Player1.Movement(state, screenX);
                if (AIMovementCount == 15)
                {
                    Player1.AIMovementCalculate(random, ball, screenX);
                    Player2.AIMovementCalculate(random, ball, screenX);
                    AIMovementCount = 0;
                }
                Player2.Movement(state, screenX);

                // Calls methods to check for collisions and correct for them.
                ball.Collisions(Player1, speed);
                ball.Collisions(Player2, speed);
                int ballStatus = ball.OutOfBounds(screenX, screenY);
                switch (ballStatus)
                {
                    case 0:
                        break;
                    case 1:
                        Player2.AddScore();
                        break;
                    case 2:
                        Player1.AddScore();
                        break;
                }
                // TODO: Add your update logic here

                AIMovementCount++;
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (DrawControlsToggle == true)
            {
                menu.DrawControls(spriteBatch, this);
            }
            else if (Win == true)
            {
                DrawPlayerWin(spriteBatch);
            }
            else if (Lose == true)
            {
                DrawPlayerLose(spriteBatch);
            }
            else
            {
                DrawBackground(spriteBatch);

                if (GameIsRunning == true)
                {
                    DrawGame(spriteBatch);
                }
                if (GameIsRunning == false)
                {
                    menu.DrawMainMenu(spriteBatch);
                }
                else if (GameIsPaused == true)
                {
                    menu.DrawPausedMenu(spriteBatch);
                }
            }
        }

        private void DrawGame(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();


            spriteBatch.DrawString(font, Player1.ScoreAsString, Player1.ScorePosition, Color.DarkGray);
            spriteBatch.DrawString(font, Player2.ScoreAsString, Player2.ScorePosition, Color.DarkGray);
            spriteBatch.Draw(Player1.Graphic, new Rectangle(Player1.PositionY, (int)Player1.PositionX, Player1.Width, Player1.Height), Color.White);
            spriteBatch.Draw(Player2.Graphic, new Rectangle(Player2.PositionY, (int)Player2.PositionX, Player2.Width, Player2.Height), Color.White);
            spriteBatch.Draw(ball.Graphic, new Rectangle(ball.PositionY, ball.PositionX, ball.Width, ball.Height), Color.White);

            spriteBatch.End();
        }

        private void DrawBackground(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, screenY, screenX), Color.White);
            spriteBatch.End();
        }

        private void CheckExit(KeyboardState state, Menu menu)
        {
            if ((state.IsKeyDown(Keys.Escape) == true) && (menu.oldstate.IsKeyUp(Keys.Escape) == true))
            {
                FullScreenToggle = false;
                DrawControlsToggle = false;
                Win = false;
                Lose = false;
            }
        }

        private void CheckWin()
        {
            if (Player1.Score >= 20)
            {
                FullScreenToggle = true;
                GameIsRunning = false;
                GameIsPaused = false;
                Player1.Score = 0;
                Player1.ScoreAsString = "0";
                Player2.Score = 0;
                Player2.ScoreAsString = "0";
                Win = true;
            }
            if (Player2.Score >= 20)
            {
                FullScreenToggle = true;
                GameIsRunning = false;
                GameIsPaused = false;
                Player1.Score = 0;
                Player1.ScoreAsString = "0";
                Player2.Score = 0;
                Player2.ScoreAsString = "0";
                Lose = true;
            }
        }

        private void DrawPlayerWin(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, screenY, screenX), Color.Cyan);
            spriteBatch.DrawString(font, "Congratulations, You WIN!", new Vector2 ((screenY/3), ball.StartPositionX), Color.Cyan);
            spriteBatch.End();
        }

        private void DrawPlayerLose(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, screenY, screenX), Color.Red);
            spriteBatch.DrawString(font, "Sorry Sucker, You Lose!", new Vector2((screenY/3), ball.StartPositionX), Color.Red);
            spriteBatch.End();
        }




    }
}
