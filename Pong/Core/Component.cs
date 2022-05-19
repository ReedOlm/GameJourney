using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Core
{
    // Abstract class for components to inherit and do work on
    internal abstract class Component
    {
        internal abstract void LoadContent(ContentManager Content);

        internal abstract void Update(GameTime gameTime);

        internal abstract void Draw(SpriteBatch spriteBatch);
    }
}
