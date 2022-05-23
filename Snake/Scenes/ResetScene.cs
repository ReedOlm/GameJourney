using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Snake.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snake.Scenes
{
    class ResetScene : Component
    {
        private Texture2D mouseTex;
        private Texture2D title;
        private Texture2D reset;
        private Rectangle titlePos;
        private Rectangle resetRect;
        private SpriteFont scoreDisplay;
        private Vector2 scorePos;
        private Vector2 scoreNPos;
        private Vector2 hScorePos;

        // Mouse detection
        private MouseState oldMs;
        private MouseState ms;
        private Rectangle msRect;

        internal override void LoadContent(ContentManager Content)
        {

            scoreDisplay = Content.Load<SpriteFont>("galleryFont");
            title = Content.Load<Texture2D>("title");
            reset = Content.Load<Texture2D>("reset");
            resetRect = new Rectangle(Data.ScreenW / 2 - reset.Width / 2, Data.ScreenH / 2 - reset.Height / 2, reset.Width, reset.Height);
            titlePos = new Rectangle(Data.ScreenW / 2 - title.Width / 2, 0, title.Width, title.Height);
            mouseTex = Content.Load<Texture2D>("mouse");
            scorePos = new Vector2(Data.ScreenW / 2 - scoreDisplay.MeasureString("Your Score").X / 2, Data.ScreenH / 4);
            scoreNPos = new Vector2(Data.ScreenW / 2 - scoreDisplay.MeasureString(Data.SessionScore.ToString()).X / 2, Data.ScreenH / 4 + 32);
            hScorePos = new Vector2(Data.ScreenW / 2 - scoreDisplay.MeasureString("Congratulations, New High Score!").X / 2, title.Height + 5);
        }

        internal override void Update(GameTime gameTime, ContentManager Content)
        {
            oldMs = ms;
            ms = Mouse.GetState();
            msRect = new Rectangle(ms.X, ms.Y, 1, 1); // Rectangle at tip of pointer 1x1 pixel for collision detection

            // Clicking effects
            if (ms.LeftButton == ButtonState.Pressed && msRect.Intersects(resetRect))
            {
                Data.SessionScore = 0;
                Data.CurrentState = Data.Scenes.Game;
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(title, titlePos, Color.White);
            // Draw buttons and check if mouse is on top of them
            spriteBatch.Draw(reset, resetRect, Color.White);
            if (msRect.Intersects(resetRect))
            {
                spriteBatch.Draw(reset, resetRect, Color.Gray);
            }

            spriteBatch.Draw(mouseTex, new Vector2(msRect.X, msRect.Y), Color.White);

            spriteBatch.DrawString(scoreDisplay, "Your Score", scorePos, Color.White);
            spriteBatch.DrawString(scoreDisplay, "Congratulations, New High Score!", hScorePos, Color.White);
            spriteBatch.DrawString(scoreDisplay, Data.SessionScore.ToString(), scoreNPos, Color.White);
        }
    }
}
