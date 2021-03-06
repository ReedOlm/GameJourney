using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pong.Core;
using Pong.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Managers
{
    internal partial class GameStateManager : Component
    {
        // Scenes stored here
        private MenuScene ms = new MenuScene();
        private GameScene gs = new GameScene();
        private SettingsScene ss = new SettingsScene();
        private PauseMenuScene pms = new PauseMenuScene();

        // Loads all scenes
        internal override void LoadContent(ContentManager Content)
        {
            ms.LoadContent(Content);
            gs.LoadContent(Content);
            ss.LoadContent(Content);
            pms.LoadContent(Content);
        }

        // Update in current scene
        internal override void Update(GameTime gameTime)
        {
            switch (Data.CurrentState)
            {
                case Data.Scenes.Menu:
                    ms.Update(gameTime);
                    break;
                case Data.Scenes.Game:
                    gs.Update(gameTime);
                    break;
                case Data.Scenes.PauseMenu:
                    pms.Update(gameTime);
                    break;
                case Data.Scenes.Settings:
                    ss.Update(gameTime);
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
                case Data.Scenes.PauseMenu:
                    pms.Draw(spriteBatch);
                    break;
                case Data.Scenes.Settings:
                    ss.Draw(spriteBatch);
                    break;
            }
        }
    }
}
