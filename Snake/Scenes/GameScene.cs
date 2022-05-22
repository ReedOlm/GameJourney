using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Snake.Core;
using Snake.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Snake.Scenes
{
    class GameScene : Component
    {
        private Texture2D background;
        private Texture2D snakeSheet;
        private Texture2D fruitTex;

        private Rectangle bgPos;
        private Rectangle topLeft;

        private Rectangle topWall;
        private Rectangle leftWall;
        private Rectangle botWall;
        private Rectangle rightWall;

        private Rectangle hStartingLoc;
        private Rectangle tStartingLoc;

        private int score = 0;
        static LinkedList snake;
        private Fruit fruit;
        private const int tileSize = 16;
        private const double moveSpeed = 0.2; // Time between snake moves, in seconds

        private double elapsed = 0f;

        private KeyboardState ks;
        private KeyboardState lastKs;

        internal override void LoadContent(ContentManager Content)
        {
            background = Content.Load<Texture2D>("background");
            snakeSheet = Content.Load<Texture2D>("snakeTile");
            fruitTex = Content.Load<Texture2D>("fruit");

            int bgTopLeftW = Data.ScreenW / 2 - background.Width / 2;
            int bgTopLeftH = Data.ScreenH / 2 - background.Height / 2;
            bgPos = new Rectangle(bgTopLeftW, bgTopLeftH, background.Width, background.Height);
            topLeft = new Rectangle(bgTopLeftW + tileSize, bgTopLeftH + tileSize, background.Width - (2 * tileSize), background.Height - (2 * tileSize));

            topWall = new Rectangle(bgTopLeftW, bgTopLeftH, tileSize, background.Width);
            leftWall = new Rectangle(bgTopLeftW, bgTopLeftH, background.Height, tileSize);
            botWall = new Rectangle(bgTopLeftW, bgTopLeftH + (background.Height - tileSize), background.Height, tileSize);
            rightWall = new Rectangle(bgTopLeftW + (background.Width - tileSize), bgTopLeftH, tileSize, background.Width);

            fruit = new Fruit(tileSize, topLeft);

            hStartingLoc = new Rectangle(topLeft.X + (11 * tileSize), topLeft.Y + (11 * tileSize), tileSize, tileSize);
            tStartingLoc = new Rectangle(hStartingLoc.X, hStartingLoc.Y + tileSize, tileSize, tileSize);
            snake = new LinkedList(new Node(null, null, hStartingLoc), new Node(null, null, tStartingLoc), score, Dir.Up);
            snake.head.next = snake.tail;
            snake.tail.prev = snake.head;
        }
        
        internal override void Update(GameTime gameTime)
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
                    fruit.Respawn(tileSize);
                }
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, bgPos, Color.White);
            spriteBatch.Draw(fruitTex, fruit.fruitPos, fruit.color);
            drawPlayer(spriteBatch, snake);
        }

        private bool movePlayer(LinkedList snake, KeyboardState ks, Fruit fruit)
        {
            bool ate = false;
            bool started = false;
            
            Rectangle goalPos = snake.head.position;
            Rectangle curPos = goalPos;

            // Move head
            if (ks.IsKeyDown(Keys.W))
            {
                started = true;
                snake.headDir = Dir.Up;
                snake.head.position.Y -= tileSize;
            }
            else if (ks.IsKeyDown(Keys.S))
            {
                started = true;
                snake.headDir = Dir.Down;
                snake.head.position.Y += tileSize;
            }
            else if (ks.IsKeyDown(Keys.D))
            {
                started = true;
                snake.headDir = Dir.Right;
                snake.head.position.X += tileSize;
            }
            else if (ks.IsKeyDown(Keys.A))
            {
                started = true;
                snake.headDir = Dir.Left;
                snake.head.position.X -= tileSize;
            }

            if (snake.head.position.Intersects(fruit.fruitPos))
                ate = true;

            // Check for wall collisions 
            if (snake.head.position.Intersects(topWall) || snake.head.position.Intersects(botWall) || snake.head.position.Intersects(leftWall) || snake.head.position.Intersects(rightWall))
                Data.CurrentState = Data.Scenes.Menu; // todo change what happens

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
                snake.AddNode(new Node(null, null, curPos), fruit.color);
            }

            return ate;
        }

        private void drawPlayer(SpriteBatch spriteBatch, LinkedList snake)
        {
            // Head
            switch (snake.headDir)
            {
                case Dir.Up:
                    spriteBatch.Draw(snakeSheet, snake.head.position, new Rectangle(0, 0, 16, 16), Color.Orange);
                    break;
                case Dir.Right:
                    spriteBatch.Draw(snakeSheet, snake.head.position, new Rectangle(16, 0, 16, 16), Color.Orange);
                    break;
                case Dir.Down:
                    spriteBatch.Draw(snakeSheet, snake.head.position, new Rectangle(32, 0, 16, 16), Color.Orange);
                    break;
                case Dir.Left:
                    spriteBatch.Draw(snakeSheet, snake.head.position, new Rectangle(48, 0, 16, 16), Color.Orange);
                    break;
            }
            if (snake.segments > 0)
            {
                Node segment = snake.head.next;

                // Determine which sprite to draw
                while(segment.next != null)
                {
                    // Either horiz/UpRight/DownRight
                    if (segment.prev.position.X > segment.position.X)
                    {
                        if(segment.next.position.Y > segment.position.Y)
                            spriteBatch.Draw(snakeSheet, segment.position, new Rectangle(32, 48, 16, 16), segment.color);
                        else if(segment.next.position.Y < segment.position.Y)
                            spriteBatch.Draw(snakeSheet, segment.position, new Rectangle(0, 48, 16, 16), segment.color);
                        else
                            spriteBatch.Draw(snakeSheet, segment.position, new Rectangle(16, 32, 16, 16), segment.color);
                    }
                    // Either horiz/UpLeft/DownLeft
                    else if (segment.prev.position.X < segment.position.X)
                    {
                        if (segment.next.position.Y > segment.position.Y)
                            spriteBatch.Draw(snakeSheet, segment.position, new Rectangle(48, 48, 16, 16), segment.color);
                        else if (segment.next.position.Y < segment.position.Y)
                            spriteBatch.Draw(snakeSheet, segment.position, new Rectangle(16, 48, 16, 16), segment.color);
                        else
                            spriteBatch.Draw(snakeSheet, segment.position, new Rectangle(16, 32, 16, 16), segment.color);
                    }
                    // Either vertical/DownRight/DownLeft
                    else if (segment.prev.position.Y > segment.position.Y)
                    {
                        if (segment.next.position.X > segment.position.X)
                            spriteBatch.Draw(snakeSheet, segment.position, new Rectangle(32, 48, 16, 16), segment.color);
                        else if (segment.next.position.X < segment.position.X)
                            spriteBatch.Draw(snakeSheet, segment.position, new Rectangle(48, 48, 16, 16), segment.color);
                        else
                            spriteBatch.Draw(snakeSheet, segment.position, new Rectangle(0, 32, 16, 16), segment.color);
                    }
                    // Either vertical/Upright/UpLeft
                    else if (segment.prev.position.Y < segment.position.Y)
                    {
                        if (segment.next.position.X > segment.position.X)
                            spriteBatch.Draw(snakeSheet, segment.position, new Rectangle(0, 48, 16, 16), segment.color);
                        else if (segment.next.position.X < segment.position.X)
                            spriteBatch.Draw(snakeSheet, segment.position, new Rectangle(16, 48, 16, 16), segment.color);
                        else
                            spriteBatch.Draw(snakeSheet, segment.position, new Rectangle(0, 32, 16, 16), segment.color);
                    }

                    segment = segment.next;
                }
                // Tail w/segments
                if (snake.tail.prev.position.X > snake.tail.position.X)
                    spriteBatch.Draw(snakeSheet, snake.tail.position, new Rectangle(16, 16, 16, 16), Color.Orange);
                // move left
                else if (snake.tail.prev.position.X < snake.tail.position.X)
                    spriteBatch.Draw(snakeSheet, snake.tail.position, new Rectangle(48, 16, 16, 16), Color.Orange);
                // Move down
                else if (snake.tail.prev.position.Y > snake.tail.position.Y)
                    spriteBatch.Draw(snakeSheet, snake.tail.position, new Rectangle(32, 16, 16, 16), Color.Orange);
                // move up
                else if (snake.tail.prev.position.Y < snake.tail.position.Y)
                    spriteBatch.Draw(snakeSheet, snake.tail.position, new Rectangle(0, 16, 16, 16), Color.Orange);
            }
            // Tail no segments
            else
            {
                // move right
                if (snake.head.position.X > snake.tail.position.X)
                    spriteBatch.Draw(snakeSheet, snake.tail.position, new Rectangle(16, 16, 16, 16), Color.Orange);
                // move left
                else if (snake.head.position.X < snake.tail.position.X)
                    spriteBatch.Draw(snakeSheet, snake.tail.position, new Rectangle(48, 16, 16, 16), Color.Orange);
                // Move down
                else if (snake.head.position.Y > snake.tail.position.Y)
                    spriteBatch.Draw(snakeSheet, snake.tail.position, new Rectangle(32, 16, 16, 16), Color.Orange);
                // move up
                else if (snake.head.position.Y < snake.tail.position.Y)
                    spriteBatch.Draw(snakeSheet, snake.tail.position, new Rectangle(0, 16, 16, 16), Color.Orange);
            }
        }
    }
}
