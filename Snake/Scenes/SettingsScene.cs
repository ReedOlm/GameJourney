using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Snake.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snake.Scenes
{
    class SettingsScene : Component
    {
        private Texture2D back;
        private Rectangle backRect;
        private Texture2D apply;
        private Rectangle applyRect;
        private const int MAX_BTNS = 3;
        private const int BUFFER_VALUE = 10; // Setting a 10 pixel vertical buffer between buttons
        private Texture2D[] btns = new Texture2D[MAX_BTNS]; // Array of buttons
        private Rectangle[] btnRects = new Rectangle[MAX_BTNS]; // Targeting rectangles

        int selectedH;
        int selectedW;

        // Mouse detection
        private MouseState oldMs;
        private MouseState ms;
        private Rectangle msRect;
        private Texture2D mouseTex;

        internal override void LoadContent(ContentManager Content)
        {
            back = Content.Load<Texture2D>($"menuBtn3480");
            backRect = new Rectangle(Data.ScreenW / 2 - back.Width / 2,0, back.Width, back.Height);
            apply = Content.Load<Texture2D>("apply");
            applyRect = new Rectangle(Data.ScreenW - Data.ScreenW / 4 ,Data.ScreenH / 2 - apply.Height / 2, apply.Width, apply.Height);
            mouseTex = Content.Load<Texture2D>("mouse");
            // Loading all buttons iteraively.
            for (int i = 0; i < btns.Length; i++)
            {
                btns[i] = Content.Load<Texture2D>($"settingsBtn{i}");

                // Center screen, and offset by increment value, Leaves space for title at 0
                btnRects[i] = new Rectangle((Data.ScreenW / 2 - btns[i].Width / 2), btns[i].Height + BUFFER_VALUE + ((btns[i].Height + BUFFER_VALUE) * i), btns[i].Width, btns[i].Height);
            }
            selectedW = Data.TargetH;
            selectedH = Data.TargetW;
        }

        internal override void Update(GameTime gameTime, ContentManager Content)
        {
            oldMs = ms;
            ms = Mouse.GetState();
            msRect = new Rectangle(ms.X, ms.Y, 1, 1); // Rectangle at tip of pointer 1x1 pixel for collision detection

            // Clicking effects
            if (ms.LeftButton == ButtonState.Pressed && msRect.Intersects(backRect))
            {
                // THIS IS FOR IF YOU HAVE MULTIPLE WAYS TO ENTER MENU, THIS Data BOOL
                // WILL SEND YOU TO CORRECT SCENE
                /*if (Data.HasStarted)
                    Data.CurrentState = Data.Scenes.PauseMenu;
                else*/
                selectedW = Data.TargetH;
                selectedH = Data.TargetW;
                Data.CurrentState = Data.Scenes.Menu;
            }
            else if (ms.LeftButton == ButtonState.Pressed && msRect.Intersects(btnRects[0]))
            {
                selectedW = 854;
                selectedH = 480;
            }
            else if (ms.LeftButton == ButtonState.Pressed && msRect.Intersects(btnRects[1]))
            {
                selectedW = 1280;
                selectedH = 720;
            }
            else if (ms.LeftButton == ButtonState.Pressed && msRect.Intersects(btnRects[2]))
            {
                selectedW = 1920;
                selectedH = 1080;
            }
            else if (ms.LeftButton == ButtonState.Pressed && msRect.Intersects(applyRect))
            {
                Data.TargetH = selectedH;
                Data.TargetW = selectedW;
                Data.UpdateResolution = true;
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
            spriteBatch.Draw(apply, applyRect, Color.White);
            if (msRect.Intersects(applyRect))
            {
                spriteBatch.Draw(apply, applyRect, Color.Gray);
            }
            for (int i = 0; i < btns.Length; i++)
            {
                spriteBatch.Draw(btns[i], btnRects[i], Color.White);
                if (msRect.Intersects(btnRects[i]))
                {
                    spriteBatch.Draw(btns[i], btnRects[i], Color.Gray);
                }
            }
            spriteBatch.Draw(mouseTex, new Vector2(msRect.X, msRect.Y), Color.White);
        }
    }
}
