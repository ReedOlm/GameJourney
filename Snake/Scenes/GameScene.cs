using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Snake.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snake.Scenes
{
    class GameScene : Component
    {
        private Texture2D background;
        private Texture2D headTex;
        private Texture2D tailTex;
        private Texture2D fruitTex;
        private List<Texture2D> bodyList;

        private Rectangle bgPos;
        private Rectangle headPos;
        private Rectangle tailPos;

        private int score = 0;

        internal override void LoadContent(ContentManager Content)
        {
            background = Content.Load<Texture2D>("background");
            headTex = Content.Load<Texture2D>("snakeTile");
            tailTex = Content.Load<Texture2D>("snakeTile");
            fruitTex = Content.Load<Texture2D>("fruit");

            bgPos = new Rectangle(Data.ScreenW / 2 - background.Width / 2, Data.ScreenH / 2 - background.Height / 2, background.Width, background.Height);
        }

        internal override void Update(GameTime gameTime)
        {
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, bgPos, Color.White);
        }
    }
}
