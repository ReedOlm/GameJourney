using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Scenes
{
    class SettingsScene : Component
    {
        private Texture2D back;
        private Rectangle backRect;

        // Mouse detection
        private MouseState oldMs;
        private MouseState ms;
        private Rectangle msRect;

        internal override void LoadContent(ContentManager Content)
        {
            back = Content.Load<Texture2D>("Back");
            backRect = new Rectangle(Data.ScreenW / 2 - 200, 24, back.Width, back.Height);
        }

        internal override void Update(GameTime gameTime)
        {
            oldMs = ms;
            ms = Mouse.GetState();
            msRect = new Rectangle(ms.X, ms.Y, 1, 1); // Rectangle at tip of pointer 1x1 pixel for collision detection

            // Clicking effects
            if (ms.LeftButton == ButtonState.Pressed && msRect.Intersects(backRect))
            {
                if (Data.HasStarted)
                    Data.CurrentState = Data.Scenes.PauseMenu;
                else
                    Data.CurrentState = Data.Scenes.Menu;
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            // Draw buttons and check if mouse is on top of them
            spriteBatch.Draw(back, backRect, Color.White);
            if (msRect.Intersects(backRect))
            {
                spriteBatch.Draw(back, backRect, Color.Gray);
            }
        }
    }
}
