using ECSTemplate.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECSTemplate.Scenes
{
    class MenuScene : Component
    {
        private const int MAX_BTNS = 3;
        private const int INCREMENT_VALUE = 248 + 10; // Buttons are 248, with 10 pixel buffer between
        private Texture2D[] btns = new Texture2D[MAX_BTNS]; // Array of buttons
        private Rectangle[] btnRects = new Rectangle[MAX_BTNS]; // Targeting rectangles

        // Mouse detection
        private MouseState oldMs; 
        private MouseState ms;
        private Rectangle msRect;

        internal override void LoadContent(ContentManager Content)
        {
            // Loading all buttons iteraively.
            for(int i = 0; i < btns.Length; i++)
            {
                btns[i] = Content.Load<Texture2D>($"menuBtn{i}");

                // Center screen, and offset by increment value, Leaves space for title at 0
                btnRects[i] = new Rectangle((Data.ScreenW / 2 - 200), INCREMENT_VALUE + (INCREMENT_VALUE * i), btns[i].Width, btns[i].Height);
            }
        }

        internal override void Update(GameTime gameTime)
        {
            oldMs = ms;
            ms = Mouse.GetState();
            msRect = new Rectangle(ms.X, ms.Y, 1, 1); // Rectangle at tip of pointer 1x1 pixel for collision detection

            // Clicking effects
            if (ms.LeftButton == ButtonState.Pressed && msRect.Intersects(btnRects[0]))
                Data.CurrentState = Data.Scenes.Game;
            else if (ms.LeftButton == ButtonState.Pressed && msRect.Intersects(btnRects[1]))
                Data.CurrentState = Data.Scenes.Settings;
            else if (ms.LeftButton == ButtonState.Pressed && msRect.Intersects(btnRects[2]))
                Data.Exit = true;
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            // Draw buttons and check if mouse is on top of them
            for(int i = 0; i < btns.Length; i++)
            {
                spriteBatch.Draw(btns[i], btnRects[i], Color.White);
                if(msRect.Intersects(btnRects[i]))
                {
                    spriteBatch.Draw(btns[i], btnRects[i], Color.Gray);
                }
            }
        }
    }
}
