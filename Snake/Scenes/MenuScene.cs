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
    class MenuScene : Component
    {
        private const int MAX_BTNS = 3;
        private const int BUFFER_VALUE = 10; // Setting a 10 pixel vertical buffer between buttons
        private Texture2D[] btns = new Texture2D[MAX_BTNS]; // Array of buttons
        private Texture2D mouseTex;
        private Rectangle[] btnRects = new Rectangle[MAX_BTNS]; // Targeting rectangles
        private SoundEffectInstance theme;
        private SoundEffect welcome;
        private Texture2D title;
        private Rectangle titlePos;

        // Mouse detection
        private MouseState oldMs;
        private MouseState ms;
        private Rectangle msRect;

        internal override void LoadContent(ContentManager Content)
        {
            title = Content.Load<Texture2D>("title");
            titlePos = new Rectangle(Data.ScreenW / 2 - title.Width / 2, 0, title.Width, title.Height);
            mouseTex = Content.Load<Texture2D>("mouse");
            // Loading all buttons iteraively.
            for (int i = 0; i < btns.Length; i++)
            {
                btns[i] = Content.Load<Texture2D>($"menuBtn{i}480");

                // Center screen, and offset by increment value, Leaves space for title at 0
                btnRects[i] = new Rectangle((Data.ScreenW / 2 - btns[i].Width / 2), btns[i].Height + BUFFER_VALUE + ((btns[i].Height + BUFFER_VALUE) * i), btns[i].Width, btns[i].Height);
            }
            theme = Content.Load<SoundEffect>("snakeTheme").CreateInstance();
            theme.IsLooped = true;
            theme.Play();
            welcome = Content.Load<SoundEffect>("welcome");
            welcome.Play();
        }

        internal override void Update(GameTime gameTime, ContentManager Content)
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
            spriteBatch.Draw(title, titlePos, Color.White);
            // Draw buttons and check if mouse is on top of them
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
