using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Snake.Core;
using Snake.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Snake.Scenes
{
    class GameScene : Component
    {
        private Texture2D background;
        private Texture2D snakeSheet;
        private Texture2D fruitTex;
        private Texture2D title;
        private SpriteFont scoreDisplay;
        private Vector2 scorePos;
        private Vector2 scoreNPos;
        private Vector2 hScorePos;
        private Vector2 hScoreNPos;

        private Rectangle titlePos;
        private Rectangle bgPos;
        private Rectangle topLeft;

        private Rectangle topWall;
        private Rectangle leftWall;
        private Rectangle botWall;
        private Rectangle rightWall;

        private Rectangle hStartingLoc;
        private Rectangle tStartingLoc;

        private SaveData saveData;
        private int highScore = 0;

        private bool endGame;
        private int score = 0;
        static Models.Snake snake;
        private Fruit fruit;
        private const int tileSize = 16;
        private const double moveSpeed = 0.2; // Time between snake moves, in seconds

        private double elapsed = 0f;

        private List<SoundEffect> sounds;

        private KeyboardState ks;
        private KeyboardState lastKs;

        internal override void LoadContent(ContentManager Content)
        {
            endGame = false;
            background = Content.Load<Texture2D>("background");
            snakeSheet = Content.Load<Texture2D>("snakeTile");
            fruitTex = Content.Load<Texture2D>("fruit");
            title = Content.Load<Texture2D>("title");
            scoreDisplay = Content.Load<SpriteFont>("galleryFont");

            sounds = new List<SoundEffect>();
            sounds.Add(Content.Load<SoundEffect>("owie"));
            sounds.Add(Content.Load<SoundEffect>("munch"));

            saveData = Load();
            highScore = saveData.highScore;

            scorePos = new Vector2(Data.ScreenW - Data.ScreenW / 4 - 5, Data.ScreenH / 4);
            scoreNPos = new Vector2(Data.ScreenW - 121, Data.ScreenH / 4 + 32);
            hScorePos = new Vector2(60, Data.ScreenH / 4);
            hScoreNPos = new Vector2(121, Data.ScreenH / 4 + 32);

            int bgTopLeftW = Data.ScreenW / 2 - background.Width / 2; // Centered game
            int bgTopLeftH = Data.ScreenH - background.Height; // Game at bottom
            titlePos = new Rectangle(Data.ScreenW / 2 - title.Width / 2, 0, title.Width, title.Height);
            bgPos = new Rectangle(bgTopLeftW, bgTopLeftH, background.Width, background.Height);
            topLeft = new Rectangle(bgTopLeftW + tileSize, bgTopLeftH + tileSize, background.Width - (2 * tileSize), background.Height - (2 * tileSize));

            topWall = new Rectangle(bgTopLeftW, bgTopLeftH, tileSize, background.Width);
            leftWall = new Rectangle(bgTopLeftW, bgTopLeftH, background.Height, tileSize);
            botWall = new Rectangle(bgTopLeftW, bgTopLeftH + (background.Height - tileSize), background.Height, tileSize);
            rightWall = new Rectangle(bgTopLeftW + (background.Width - tileSize), bgTopLeftH, tileSize, background.Width);

           /* SaveData highScores = new SaveData();
            //highScores = new List<String>();
            highScores.highScores.Add("Swag: 15");
            highScores.highScores.Add("Jimmy: 10");
            highScores.highScores.Add("Kayla: 4");
            highScores.highScores.Add("Jeff: 20");
            highScores.highScores.Sort();
            MainGame.Save(highScores);*/

            fruit = new Fruit(tileSize, topLeft);

            hStartingLoc = new Rectangle(topLeft.X + (11 * tileSize), topLeft.Y + (11 * tileSize), tileSize, tileSize);
            tStartingLoc = new Rectangle(hStartingLoc.X, hStartingLoc.Y + tileSize, tileSize, tileSize);
            snake = new Models.Snake(new Segment(null, null, hStartingLoc), new Segment(null, null, tStartingLoc), score, Dir.Up);
            snake.head.next = snake.tail;
            snake.tail.prev = snake.head;
        }
        
        internal override void Update(GameTime gameTime, ContentManager Content)
        {
            elapsed += gameTime.ElapsedGameTime.TotalSeconds;

            // Detect movement
            ks = Keyboard.GetState();
            if(ks.GetPressedKeyCount() == 1 && !ks.Equals(lastKs))
            {
                // Snake starts moving when button is pressed
                if(ks.IsKeyDown(Keys.W) || ks.IsKeyDown(Keys.S) || ks.IsKeyDown(Keys.A) || ks.IsKeyDown(Keys.D))
                {
                    lastKs = ks;
                }
            }

            if (elapsed >= moveSpeed)
            {
                elapsed = 0;
                bool ate = movePlayer(snake, lastKs, fruit);
                if (ate)
                {
                    sounds[1].Play();
                    score++;
                    fruit.Respawn(tileSize);
                }
            }
            if(endGame)
            {
                sounds[0].Play();
                if (score > highScore)
                {
                    Data.NewHighScore = true;
                    saveData.highScore = score;
                    Save(saveData);
                }
                Data.SessionScore = score;
                Data.CurrentState = Data.Scenes.Reset; // todo change what happens
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, bgPos, Color.White);
            spriteBatch.Draw(fruitTex, fruit.fruitPos, fruit.color);
            spriteBatch.Draw(title, titlePos, Color.White);
            spriteBatch.DrawString(scoreDisplay, "Current Score", scorePos, Color.White);
            spriteBatch.DrawString(scoreDisplay, "High Score", hScorePos, Color.White);
            spriteBatch.DrawString(scoreDisplay, score.ToString(), scoreNPos, Color.White);
            spriteBatch.DrawString(scoreDisplay, highScore.ToString(), hScoreNPos, Color.White);
            drawPlayer(spriteBatch, snake);
        }

        private bool movePlayer(Models.Snake snake, KeyboardState ks, Fruit fruit)
        {
            bool ate = false;
            bool started = false;
            
            Rectangle goalPos = snake.head.position;
            Rectangle curPos = goalPos;

            // Move head
            if (ks.IsKeyDown(Keys.W))
            {
                if (snake.headDir != Dir.Down)
                {
                    started = true;
                    snake.headDir = Dir.Up;
                    snake.head.position.Y -= tileSize;
                }else
                {
                    started = true;
                    snake.headDir = Dir.Down;
                    snake.head.position.Y += tileSize;
                }
            }
            else if (ks.IsKeyDown(Keys.S))
            {
                if (snake.headDir != Dir.Up)
                {
                    started = true;
                    snake.headDir = Dir.Down;
                    snake.head.position.Y += tileSize;
                }else
                {
                    started = true;
                    snake.headDir = Dir.Up;
                    snake.head.position.Y -= tileSize;
                }
            }
            else if (ks.IsKeyDown(Keys.D))
            {
                if (snake.headDir != Dir.Left)
                {
                    started = true;
                    snake.headDir = Dir.Right;
                    snake.head.position.X += tileSize;
                }else
                {
                    started = true;
                    snake.headDir = Dir.Left;
                    snake.head.position.X -= tileSize;
                }
            }
            else if (ks.IsKeyDown(Keys.A))
            {
                if (snake.headDir != Dir.Right)
                {
                    started = true;
                    snake.headDir = Dir.Left;
                    snake.head.position.X -= tileSize;
                }
                else
                {
                    started = true;
                    snake.headDir = Dir.Right;
                    snake.head.position.X += tileSize;
                }
            }

            if (snake.head.position.Intersects(fruit.fruitPos))
                ate = true;

            // Check for wall collisions 
            if (snake.DetectCollisions() || snake.head.position.Intersects(topWall) || snake.head.position.Intersects(botWall) || snake.head.position.Intersects(leftWall) || snake.head.position.Intersects(rightWall))
            {
                endGame = true;
            }

            // Create snake method to check for body collisions
            

            // Body
            goalPos = snake.ShiftSegments(goalPos);
            // Tail
            curPos = snake.tail.position;
            if(started)
                snake.tail.position = goalPos;
            // Add node to end of snake if it ate
            if(ate)
            {
                snake.AddNode(new Segment(null, null, curPos), fruit.color);
            }

            return ate;
        }

        private void drawPlayer(SpriteBatch spriteBatch, Models.Snake snake)
        {
            // Head
            switch (snake.headDir)
            {
                case Dir.Up:
                    spriteBatch.Draw(snakeSheet, snake.head.position, new Rectangle(0, 0, tileSize, tileSize), Color.Orange);
                    break;
                case Dir.Right:
                    spriteBatch.Draw(snakeSheet, snake.head.position, new Rectangle(16, 0, tileSize, tileSize), Color.Orange);
                    break;
                case Dir.Down:
                    spriteBatch.Draw(snakeSheet, snake.head.position, new Rectangle(32, 0, tileSize, tileSize), Color.Orange);
                    break;
                case Dir.Left:
                    spriteBatch.Draw(snakeSheet, snake.head.position, new Rectangle(48, 0, tileSize, tileSize), Color.Orange);
                    break;
            }
            if (snake.segments > 0)
            {
                Segment segment = snake.head.next;

                // Determine which sprite to draw
                while(segment.next != null)
                {
                    // Either horiz/UpRight/DownRight
                    if (segment.prev.position.X > segment.position.X)
                    {
                        if(segment.next.position.Y > segment.position.Y)
                            spriteBatch.Draw(snakeSheet, segment.position, new Rectangle(32, 48, tileSize, tileSize), segment.color);
                        else if(segment.next.position.Y < segment.position.Y)
                            spriteBatch.Draw(snakeSheet, segment.position, new Rectangle(0, 48, tileSize, tileSize), segment.color);
                        else
                            spriteBatch.Draw(snakeSheet, segment.position, new Rectangle(16, 32, tileSize, tileSize), segment.color);
                    }
                    // Either horiz/UpLeft/DownLeft
                    else if (segment.prev.position.X < segment.position.X)
                    {
                        if (segment.next.position.Y > segment.position.Y)
                            spriteBatch.Draw(snakeSheet, segment.position, new Rectangle(48, 48, tileSize, tileSize), segment.color);
                        else if (segment.next.position.Y < segment.position.Y)
                            spriteBatch.Draw(snakeSheet, segment.position, new Rectangle(16, 48, tileSize, tileSize), segment.color);
                        else
                            spriteBatch.Draw(snakeSheet, segment.position, new Rectangle(16, 32, tileSize, tileSize), segment.color);
                    }
                    // Either vertical/DownRight/DownLeft
                    else if (segment.prev.position.Y > segment.position.Y)
                    {
                        if (segment.next.position.X > segment.position.X)
                            spriteBatch.Draw(snakeSheet, segment.position, new Rectangle(32, 48, tileSize, tileSize), segment.color);
                        else if (segment.next.position.X < segment.position.X)
                            spriteBatch.Draw(snakeSheet, segment.position, new Rectangle(48, 48, tileSize, tileSize), segment.color);
                        else
                            spriteBatch.Draw(snakeSheet, segment.position, new Rectangle(0, 32, tileSize, tileSize), segment.color);
                    }
                    // Either vertical/Upright/UpLeft
                    else if (segment.prev.position.Y < segment.position.Y)
                    {
                        if (segment.next.position.X > segment.position.X)
                            spriteBatch.Draw(snakeSheet, segment.position, new Rectangle(0, 48, tileSize, tileSize), segment.color);
                        else if (segment.next.position.X < segment.position.X)
                            spriteBatch.Draw(snakeSheet, segment.position, new Rectangle(16, 48, tileSize, tileSize), segment.color);
                        else
                            spriteBatch.Draw(snakeSheet, segment.position, new Rectangle(0, 32, tileSize, tileSize), segment.color);
                    }

                    segment = segment.next;
                }
                // Tail w/segments
                if (snake.tail.prev.position.X > snake.tail.position.X)
                    spriteBatch.Draw(snakeSheet, snake.tail.position, new Rectangle(16, 16, tileSize, tileSize), Color.Orange);
                // move left
                else if (snake.tail.prev.position.X < snake.tail.position.X)
                    spriteBatch.Draw(snakeSheet, snake.tail.position, new Rectangle(48, 16, tileSize, tileSize), Color.Orange);
                // Move down
                else if (snake.tail.prev.position.Y > snake.tail.position.Y)
                    spriteBatch.Draw(snakeSheet, snake.tail.position, new Rectangle(32, 16, tileSize, tileSize), Color.Orange);
                // move up
                else if (snake.tail.prev.position.Y < snake.tail.position.Y)
                    spriteBatch.Draw(snakeSheet, snake.tail.position, new Rectangle(0, 16, tileSize, tileSize), Color.Orange);
            }
            // Tail no segments
            else
            {
                // move right
                if (snake.head.position.X > snake.tail.position.X)
                    spriteBatch.Draw(snakeSheet, snake.tail.position, new Rectangle(16, 16, tileSize, tileSize), Color.Orange);
                // move left
                else if (snake.head.position.X < snake.tail.position.X)
                    spriteBatch.Draw(snakeSheet, snake.tail.position, new Rectangle(48, 16, tileSize, tileSize), Color.Orange);
                // Move down
                else if (snake.head.position.Y > snake.tail.position.Y)
                    spriteBatch.Draw(snakeSheet, snake.tail.position, new Rectangle(32, 16, tileSize, tileSize), Color.Orange);
                // move up
                else if (snake.head.position.Y < snake.tail.position.Y)
                    spriteBatch.Draw(snakeSheet, snake.tail.position, new Rectangle(0, 16, tileSize, tileSize), Color.Orange);
            }
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
            if (!File.Exists(Data.PATH))
            {
                SaveData fixer = new SaveData
                {
                    highScore = 0
                };
                Save(fixer);
            }
            var fileContents = File.ReadAllText(Data.PATH);
            return JsonSerializer.Deserialize<SaveData>(fileContents);
        }
    }
}
