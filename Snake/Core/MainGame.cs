using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Snake.Managers;
using System;
using System.IO;
using System.Text.Json;

namespace Snake.Core
{
    // This class calls the Game State Manager and sets up initial program
    public class MainGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        public static readonly Random r = new Random();

        // Testing
        private Matrix scaler;

        // Targeting a resolution and framerate
        RenderTarget2D renderTarget;
        public float scale = 0.44444f;
        public float elapsed;

        // Scene switching
        private GameStateManager gsm;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false; // Toggle mouse visibility
        }

        // Initialize stuff on startup
        protected override void Initialize()
        {
            // Targeting frame rate
            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 144.0f);
            IsFixedTimeStep = true; // Needs testing to figure out which value is correct

            // Setting starting user resolution
            graphics.PreferredBackBufferWidth = Data.TargetW;
            graphics.PreferredBackBufferHeight = Data.TargetH;
            graphics.ApplyChanges();

            // Setting target resolution
            renderTarget = new RenderTarget2D(GraphicsDevice, Data.ScreenW, Data.ScreenH);

            // Game state manager initialization
            gsm = new GameStateManager();

            // Initialize stuff

            base.Initialize();
        }

        // Use to load assets to variables
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gsm.LoadContent(Content);
        }

        // Runs at refresh rate
        protected override void Update(GameTime gameTime)
        {
            gsm.Update(gameTime); // Use game state manager to choose which scene to call update on
            // Check to end game
            if (Data.Exit)
                Exit();

            elapsed = (float)gameTime.ElapsedGameTime.TotalMilliseconds; // Simple time tracker to ensure speed etc
            float movementSpeed = elapsed / 2.38f; // Good general constant move speed to avoid collision issues, likely uneeded.

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Targeting resolution
            //scale = 1f / ((float)Data.targetH / graphics.GraphicsDevice.Viewport.Height);
            //scale = 1f / ((float)graphics.PreferredBackBufferHeight / (float)Data.targetH);
            scale = 1f / ((float)Data.ScreenH / (float)Data.TargetH); // This works
            scaler = Matrix.CreateScale(scale);
            
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Calls game State Manager draw
            spriteBatch.Begin();
            gsm.Draw(spriteBatch); // Use game state manager to choose which scene to call draw on
            spriteBatch.End();

            // Targeting resolution
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // Draw render target
            spriteBatch.Begin(transformMatrix: scaler);
            spriteBatch.Draw(renderTarget, Vector2.Zero, Color.White);
            //spriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        // This method will save a passed list of data to ensure persistence
        private void Save(SaveData saveData)
        {
            string serializedText = JsonSerializer.Serialize<SaveData>(saveData);
            File.WriteAllText(Data.PATH, serializedText);
        }

        // Loads from path the save data and returns the Save Data
        private SaveData Load()
        {
            var fileContents = File.ReadAllText(Data.PATH);
            return JsonSerializer.Deserialize<SaveData>(fileContents);
        }
    }
}
