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
        SpriteFont font;

        Random random = new Random();
        Player humanPlayer = new Player(false);
        Player aiPlayer = new Player(true);
        Ball ball = new Ball();
        int screenX = 480;
        int screenY;
        int speed;
        int AIMovementCount;

        Texture2D ballGraphic;
        Texture2D paddleGraphic;
        Texture2D background;

        public Game1()
        {
            screenY = (screenX * 16) / 9;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = screenX;
            graphics.PreferredBackBufferWidth = screenY;
            graphics.IsFullScreen = false;
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

            speed = screenX / 60;
            humanPlayer.Height = screenX/8;
            humanPlayer.Width = humanPlayer.Height / 5;
            humanPlayer.PositionX = ((screenX/2)-(humanPlayer.Height/2));
            humanPlayer.PositionY = ((screenY/10)-(humanPlayer.Width/2));
            humanPlayer.Speed = ((speed*5)/4);
            aiPlayer.Height = screenX/8;
            aiPlayer.Width = aiPlayer.Height / 5;
            aiPlayer.PositionX = ((screenX/2) - (aiPlayer.Height / 2));
            aiPlayer.PositionY = (screenY-((screenY/10)+(aiPlayer.Width/2)));
            aiPlayer.Speed = ((speed*5)/4);
            ball.Height = humanPlayer.Height / 5;
            ball.Width = ball.Height;
            ball.Radius = ball.Height / 2;
            ball.StartPositionX = ((screenX / 2)-(ball.Height/2));
            ball.StartPositionY = ((screenY / 2)-(ball.Width/2));
            ball.PositionX = ball.StartPositionX;
            ball.PositionY = ball.StartPositionY;
            humanPlayer.ScorePosition = new Vector2((screenY/4), screenX/10);
            aiPlayer.ScorePosition = new Vector2(((screenY / 4) + (screenY / 2)), screenX / 10);
            humanPlayer.ScoreAsString = "0";
            aiPlayer.ScoreAsString = "0";

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
            ballGraphic = Content.Load<Texture2D>("BallNew");
            paddleGraphic = Content.Load<Texture2D>("PaddleNew");
            background = Content.Load<Texture2D>("Background");
            font = Content.Load<SpriteFont>("Font");
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
            // Allows the game to exit if back button on Gamepad is pushed or ESC key is pushed
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            // Calls Methods to determine movement & update positions.
            ball.Movement(speed);
            humanPlayer.Movement(state, screenX);
            if (AIMovementCount == 15)
            {
                humanPlayer.AIMovementCalculate(random, ball, screenX);
                aiPlayer.AIMovementCalculate(random, ball, screenX);
                AIMovementCount = 0;
            }
            aiPlayer.Movement(state, screenX);

            // Calls methods to check for collisions and correct for them.
            ball.Collisions(humanPlayer, speed);
            ball.Collisions(aiPlayer, speed);
            int ballStatus = ball.OutOfBounds(screenX, screenY);
            switch (ballStatus)
            {
                case 0:
                    break;
                case 1:
                    aiPlayer.AddScore();
                    break;
                case 2:
                    humanPlayer.AddScore();
                    break;
            }
            // TODO: Add your update logic here

            AIMovementCount++;
            // CheckWin();
            // DrawPositionUpdate();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, screenY, screenX), Color.White);
            spriteBatch.DrawString(font, humanPlayer.ScoreAsString, humanPlayer.ScorePosition, Color.DarkGray);
            spriteBatch.DrawString(font, aiPlayer.ScoreAsString, aiPlayer.ScorePosition, Color.DarkGray);
            spriteBatch.Draw(paddleGraphic, new Rectangle(humanPlayer.PositionY, (int)humanPlayer.PositionX, humanPlayer.Width, humanPlayer.Height), Color.White);
            spriteBatch.Draw(paddleGraphic, new Rectangle(aiPlayer.PositionY, (int)aiPlayer.PositionX, aiPlayer.Width, aiPlayer.Height), Color.White);
            spriteBatch.Draw(ballGraphic, new Rectangle(ball.PositionY, ball.PositionX, ball.Width, ball.Height), Color.White);

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
