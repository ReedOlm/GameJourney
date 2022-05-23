using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Snake.Core;
using Snake.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snake.Managers
{
    internal partial class GameStateManager : Component
    {
        // Scenes stored here
        private MenuScene ms = new MenuScene();
        private GameScene gs = new GameScene();
        private SettingsScene ss = new SettingsScene();
        private ResetScene rs = new ResetScene();

        // Loads all scenes
        internal override void LoadContent(ContentManager Content)
        {
            ms.LoadContent(Content);
            gs.LoadContent(Content);
            ss.LoadContent(Content);
            rs.LoadContent(Content);
        }

        // Update in current scene
        internal override void Update(GameTime gameTime, ContentManager Content)
        {
            switch (Data.CurrentState)
            {
                case Data.Scenes.Menu:
                    ms.Update(gameTime, Content);
                    break;
                case Data.Scenes.Game:
                    gs.Update(gameTime, Content);
                    break;
                case Data.Scenes.Settings:
                    ss.Update(gameTime, Content);
                    break;
                case Data.Scenes.Reset:
                    gs = new GameScene();
                    gs.LoadContent(Content);
                    rs.Update(gameTime, Content);
                    break;
            }
        }

        // Draw in current scene
        internal override void Draw(SpriteBatch spriteBatch)
        {
            switch (Data.CurrentState)
            {
                case Data.Scenes.Menu:
                    ms.Draw(spriteBatch);
                    break;
                case Data.Scenes.Game:
                    gs.Draw(spriteBatch);
                    break;
                case Data.Scenes.Settings:
                    ss.Draw(spriteBatch);
                    break;
                case Data.Scenes.Reset:
                    rs.Draw(spriteBatch);
                    break;
            }
        }
    }
}
