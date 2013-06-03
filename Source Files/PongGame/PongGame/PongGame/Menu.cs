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
    class Menu
    {
        public Texture2D StartButton;
        public Texture2D ResumeButton;
        public Texture2D ControlsButton;
        public Texture2D ExitButton;
        public Texture2D StartButtonSelected;
        public Texture2D ResumeButtonSelected;
        public Texture2D ControlsButtonSelected;
        public Texture2D ExitButtonSelected;
        public Texture2D ControlsDiagram;

        public Texture2D Start;
        public Texture2D Resume;
        public Texture2D Controls;
        public Texture2D Exit;

        public Vector2 StartButtonPosition { get; set; }
        public Vector2 ResumeButtonPosition { get; set; }
        public Vector2 ControlsButtonPosition { get; set; }
        public Vector2 ExitButtonPosition { get; set; }

        public int CursorPosition { get; set; }

        public KeyboardState oldstate { get; set; }

        public void SetPositions(GameInfo gameInfo)
        {
            StartButtonPosition = new Vector2( ( (gameInfo.screenY/2) - (StartButton.Width / 2) ), ( (gameInfo.screenX/15) *2) );
            ResumeButtonPosition = new Vector2(((gameInfo.screenY / 2) - (ResumeButton.Width / 2)), ((gameInfo.screenX / 15) * 2));
            ControlsButtonPosition = new Vector2(((gameInfo.screenY / 2) - (ControlsButton.Width / 2) ) , ((gameInfo.screenX / 15) * 6));
            ExitButtonPosition = new Vector2( ( (gameInfo.screenY/2) - (ExitButton.Width / 2)), ( (gameInfo.screenX / 15) * 10) );
        }

        public void LoadButtons(ContentManager Content)
        {
            StartButton = Content.Load<Texture2D>("buttonStart");
            ResumeButton = Content.Load<Texture2D>("buttonResume");
            ControlsButton = Content.Load<Texture2D>("buttonControls");
            ExitButton = Content.Load<Texture2D>("buttonExit");
            StartButtonSelected = Content.Load<Texture2D>("buttonStartSelected");
            ResumeButtonSelected = Content.Load<Texture2D>("buttonResumeSelected");
            ControlsButtonSelected = Content.Load<Texture2D>("buttonControlsSelected");
            ExitButtonSelected = Content.Load<Texture2D>("buttonExitSelected");
            ControlsDiagram = Content.Load<Texture2D>("ControlsDiagram");
        }

        public void RunMenu(KeyboardState state, GameInfo gameInfo)
        {
            SetPositions(gameInfo);
            Start = StartButton;
            Resume = ResumeButton;
            Controls = ControlsButton;
            Exit = ExitButton;
            //Do stuff with cursor here then work out what button is lit up.
            CursorMovement(state);
            ButtonSelect(state, gameInfo);
            switch (CursorPosition)
            {
                case 0:
                    Start = StartButtonSelected;
                    Resume = ResumeButtonSelected;
                    break;
                case 1:
                    Controls = ControlsButtonSelected;
                    break;
                case 2:
                    Exit = ExitButtonSelected;
                    break;
            }
            oldstate = state;
        }

        public void DrawMainMenu(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(Start, StartButtonPosition, Color.White);
            spriteBatch.Draw(Controls, ControlsButtonPosition, Color.White);
            spriteBatch.Draw(Exit, ExitButtonPosition, Color.White);

            spriteBatch.End();
        }

        public void DrawPausedMenu(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(Resume, ResumeButtonPosition, Color.White);
            spriteBatch.Draw(Controls, ControlsButtonPosition, Color.White);
            spriteBatch.Draw(Exit, ExitButtonPosition, Color.White);

            spriteBatch.End();
        }

        private void CursorMovement(KeyboardState state)
        {
            if ((state.IsKeyDown(Keys.W) == true) && (oldstate.IsKeyUp(Keys.W) == true) ||
    (state.IsKeyDown(Keys.Up) == true) && (oldstate.IsKeyUp(Keys.Up) == true))
            {
                CursorPosition--;
                if (CursorPosition < 0)
                {
                    CursorPosition = 0;
                }

            }
            else if ((state.IsKeyDown(Keys.S) == true) && (oldstate.IsKeyUp(Keys.S) == true) ||
                (state.IsKeyDown(Keys.Down) == true) && (oldstate.IsKeyUp(Keys.Down) == true))
            {
                CursorPosition++;
                if (CursorPosition > 2)
                {
                    CursorPosition = 2;
                }
            }
        }

        private void ButtonSelect(KeyboardState state, GameInfo gameInfo)
        {
            if (((state.IsKeyDown(Keys.Enter) == true) && (oldstate.IsKeyUp(Keys.Enter)==true)) ||
                ((state.IsKeyDown(Keys.Space) == true) && (oldstate.IsKeyUp(Keys.Space)==true)))
            {
                switch (CursorPosition)
                {
                    case 0:
                        gameInfo.GameIsRunning = true;
                        gameInfo.GameIsPaused = false;
                        if ((gameInfo.Player1.Score >= 20) || (gameInfo.Player2.Score >= 20))
                        {
                            gameInfo.Player1.Score = 0;
                            gameInfo.Player1.ScoreAsString = "0";
                            gameInfo.Player2.Score = 0;
                            gameInfo.Player2.ScoreAsString = "0";
                        }
                        break;
                    case 1:
                        gameInfo.FullScreenToggle = true;
                        gameInfo.DrawControlsToggle = true;
                        break;
                    case 2:
                        gameInfo.EndGame = true;
                        break;
                }
            }
        }

        public void PauseGame(KeyboardState state, GameInfo gameInfo)
        {
            if ((state.IsKeyDown(Keys.Escape) == true) && (oldstate.IsKeyUp(Keys.Escape) == true) ||
                ((state.IsKeyDown(Keys.Space) == true) && (oldstate.IsKeyUp(Keys.Space) == true)) ||
                ((state.IsKeyDown(Keys.P) == true) && (oldstate.IsKeyUp(Keys.P) == true)))
            {
                if (gameInfo.GameIsPaused == false)
                {
                    gameInfo.GameIsPaused = true;
                }
            }
        }

        public void DrawControls(SpriteBatch spriteBatch, GameInfo gameInfo)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(ControlsDiagram, new Rectangle(0, 0, gameInfo.screenY, gameInfo.screenX), Color.White);
            spriteBatch.End();
        }

    }
}
