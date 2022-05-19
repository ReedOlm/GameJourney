using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Pong.Scenes
{
    class GameScene : Component
    {
        private const float initBXSpeed = 3f;
        private const float initBYSpeed = 0f;
        private int scoreL = 0;
        private int scoreR = 0;
        private int pSpeed = 3;
        private float speedMulti = 1f;
        private float bSpeedX = initBXSpeed;
        private float bSpeedY = initBYSpeed;
        private bool lHasTouched = false;
        private bool rHasTouched = false;

        // Textures
        private Texture2D ball;
        private Texture2D paddleL;
        private Texture2D paddleR;
        private Texture2D net;
        private Texture2D side;
        private Texture2D goal;
        private Texture2D background;
        private int scoreWidth = 48; // Pixel width of my drawn scores
        private Texture2D scoreLTex;
        private Texture2D scoreRTex;

        // Collision Rectangles
        private Vector2 ballPos;
        private Rectangle scoreLPos;
        private Rectangle scoreRPos;
        private Rectangle ballRect;
        private Rectangle paddleLRect;
        private Rectangle paddleRRect;
        private Rectangle netRect;
        private Rectangle tlRect;
        private Rectangle trRect;
        private Rectangle tmRect;
        private Rectangle blRect;
        private Rectangle brRect;
        private Rectangle bmRect;
        private Rectangle lGoalRect;
        private Rectangle rGoalRect;

        /*// Positions
        private Vector2 ballPos;
        private Vector2 paddleLPos;
        private Vector2 paddleRPos;
        private Vector2 netPos;*/

        // Keyboard detection
        private KeyboardState oldKs;
        private KeyboardState ks;

        internal override void LoadContent(ContentManager Content)
        {
            background = Content.Load<Texture2D>("Background");
            ball = Content.Load<Texture2D>("Ball");
            paddleL = Content.Load<Texture2D>("Paddle");
            paddleR = Content.Load<Texture2D>("Paddle");
            net = Content.Load<Texture2D>("VerticalLine");
            goal = Content.Load<Texture2D>("VerticalLine");
            side = Content.Load<Texture2D>("HorizontalLine");
            scoreLTex = Content.Load<Texture2D>("Score");
            scoreRTex = Content.Load<Texture2D>("Score");

            scoreLPos = new Rectangle(Data.ScreenW / 3, Data.ScreenH / 5, scoreWidth, scoreLTex.Height);
            scoreRPos = new Rectangle(2 * Data.ScreenW / 3, Data.ScreenH / 5, scoreWidth, scoreLTex.Height);

            ballPos = new Vector2(Data.ScreenW / 3 - ball.Width/2, Data.ScreenH / 2 - ball.Height / 2);
            ballRect = new Rectangle(Data.ScreenW / 3 - ball.Width / 2, Data.ScreenH / 2 - ball.Height / 2, ball.Width, ball.Height);
            netRect = new Rectangle(Data.ScreenW / 2 - net.Width / 2, Data.ScreenH / 2 - net.Height / 2, net.Width, net.Height);

            tlRect = new Rectangle((Data.ScreenW / 4 - goal.Width / 2) - (5 * goal.Width), Data.ScreenH / 2 - goal.Height / 2, side.Width, side.Height);
            trRect = new Rectangle(Data.ScreenW - ((Data.ScreenW / 4 - goal.Width / 2) - (5 * goal.Width) - goal.Width) - side.Width, Data.ScreenH / 2 - goal.Height / 2, side.Width, side.Height);
            tmRect = new Rectangle(Data.ScreenW / 2 - side.Width / 2, Data.ScreenH / 2 - goal.Height / 2, side.Width, side.Height);
            blRect = new Rectangle((Data.ScreenW / 4 - goal.Width / 2) - (5 * goal.Width), Data.ScreenH / 2 + goal.Height / 2, side.Width, side.Height);
            brRect = new Rectangle(Data.ScreenW - ((Data.ScreenW / 4 - goal.Width / 2) - (5 * goal.Width) - goal.Width) - side.Width, Data.ScreenH / 2 + goal.Height / 2, side.Width, side.Height);
            bmRect = new Rectangle(Data.ScreenW / 2 - side.Width / 2, Data.ScreenH / 2 + goal.Height / 2, side.Width, side.Height);

            lGoalRect = new Rectangle((Data.ScreenW / 4 - goal.Width / 2) - (5 * goal.Width), Data.ScreenH / 2 - goal.Height / 2, goal.Width, goal.Height);
            rGoalRect = new Rectangle((Data.ScreenW - (Data.ScreenW / 4 - goal.Width / 2)) + (5 * goal.Width), Data.ScreenH / 2 - goal.Height / 2, goal.Width, goal.Height);
            paddleLRect = new Rectangle(Data.ScreenW / 4 - paddleL.Width / 2, Data.ScreenH / 2 - paddleL.Height / 2, paddleL.Width, paddleL.Height);
            paddleRRect = new Rectangle(Data.ScreenW - (Data.ScreenW / 4 - paddleL.Width / 2), Data.ScreenH / 2 - paddleL.Height / 2, paddleL.Width, paddleL.Height);
        }

        internal override void Update(GameTime gameTime)
        {
            oldKs = ks;
            ks = Keyboard.GetState();

            // Pause
            if (ks.IsKeyDown(Keys.Escape) && !oldKs.IsKeyDown(Keys.Escape))
                Data.CurrentState = Data.Scenes.PauseMenu;

            // Move left paddle
            if (ks.IsKeyDown(Keys.W))
            {
                paddleLRect.Y -= pSpeed;
            }
            else if (ks.IsKeyDown(Keys.S))
            { 
                paddleLRect.Y += pSpeed;
            }
            // Move right paddle
            if (ks.IsKeyDown(Keys.I))
            {
                paddleRRect.Y -= pSpeed;
            }
            else if (ks.IsKeyDown(Keys.K))
            {
                paddleRRect.Y += pSpeed;
            }
            // Clamping
            if(paddleLRect.Y > Data.ScreenH / 2 + goal.Height / 2 - paddleL.Height)
            {
                paddleLRect.Y = Data.ScreenH / 2 + goal.Height / 2 - paddleL.Height;
            }
            else if(paddleLRect.Y < Data.ScreenH / 2 - goal.Height / 2 + side.Height)
            {
                paddleLRect.Y = Data.ScreenH / 2 - goal.Height / 2 + side.Height;
            }
            if (paddleRRect.Y > Data.ScreenH / 2 + goal.Height / 2 - paddleR.Height)
            {
                paddleRRect.Y = Data.ScreenH / 2 + goal.Height / 2 - paddleR.Height;
            }
            else if (paddleRRect.Y < Data.ScreenH / 2 - goal.Height / 2 + side.Height)
            {
                paddleRRect.Y = Data.ScreenH / 2 - goal.Height / 2 + side.Height;
            }

            ballRect = new Rectangle((int)ballPos.X, (int)ballPos.Y, ball.Width, ball.Height);


            // Ball touches left
            if (ballRect.Intersects(paddleLRect) && !lHasTouched)
            {
                lHasTouched = true;
                rHasTouched = false;
                bSpeedX *= -1;
                // Check direction paddle moving
                if(ks.IsKeyDown(Keys.W))
                {
                    double val = (MainGame.r.NextDouble() * (0 - 1) - 1);
                    bSpeedY = (float)val;
                }
                else if (ks.IsKeyDown(Keys.S))
                {
                    double val = (MainGame.r.NextDouble() * 1);
                    bSpeedY = (float)val;
                }
                else
                {
                    double val = (MainGame.r.NextDouble() * (.666 - -.666) + -.666);
                    bSpeedY = (float)val;
                }
            }
            // Ball touches right
            else if(ballRect.Intersects(paddleRRect) && !rHasTouched)
            {
                lHasTouched = false;
                rHasTouched = true;
                bSpeedX *= -1;
                // Check direction paddle moving
                if (ks.IsKeyDown(Keys.I))
                {
                    double val = (MainGame.r.NextDouble() * (0 - 1.666) - 1.666);
                    bSpeedY = (float)val;
                }
                else if (ks.IsKeyDown(Keys.K))
                {
                    double val = (MainGame.r.NextDouble() * 1.666);
                    bSpeedY = (float)val;
                }
                else
                {
                    double val = (MainGame.r.NextDouble() * (.666 - -.666) + -.666);
                    bSpeedY = (float)val;
                }
            }

            // Ball hits cieling
            if(ballRect.Intersects(tlRect) || ballRect.Intersects(tmRect) || ballRect.Intersects(trRect))
            {
                bSpeedY *= -1;
                speedMulti += (float).01;
            }
            // Ball hits floor
            else if(ballRect.Intersects(blRect) || ballRect.Intersects(bmRect) || ballRect.Intersects(brRect))
            {
                bSpeedY *= -1;
                speedMulti += (float).01;
            }

            // Ball scores on right side
            if (ballRect.Intersects(rGoalRect))
            {
                scoreL++;
                ballPos = new Vector2(Data.ScreenW - (Data.ScreenW / 3 - ball.Width / 2), Data.ScreenH / 2 - ball.Height / 2);
                bSpeedX = initBXSpeed * -1;
                bSpeedY = initBYSpeed;
                lHasTouched = false;
                rHasTouched = false;
                speedMulti = 1f;
            }
            // Ball scores on left side
            else if (ballRect.Intersects(lGoalRect))
            {
                scoreR++;
                ballPos = new Vector2(Data.ScreenW / 3 - ball.Width / 2, Data.ScreenH / 2 - ball.Height / 2);
                bSpeedX = initBXSpeed;
                bSpeedY = initBYSpeed;
                lHasTouched = false;
                rHasTouched = false;
                speedMulti = 1f;
            }

            ballPos.X += bSpeedX * speedMulti;
            ballPos.Y += bSpeedY * speedMulti;
            if (scoreL == 7 || scoreR == 7)
            {
                scoreL = 0;
                scoreR = 0;
            }

        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            spriteBatch.Draw(net, netRect, Color.White);
            spriteBatch.Draw(goal, lGoalRect, Color.White);
            spriteBatch.Draw(goal, rGoalRect, Color.White);
            spriteBatch.Draw(side, tlRect, Color.White);
            spriteBatch.Draw(side, trRect, Color.White);
            spriteBatch.Draw(side, blRect, Color.White);
            spriteBatch.Draw(side, brRect, Color.White);
            spriteBatch.Draw(side, tmRect, Color.White);
            spriteBatch.Draw(side, bmRect, Color.White);
            spriteBatch.Draw(ball, ballPos, Color.White);
            spriteBatch.Draw(scoreLTex, scoreLPos, new Rectangle(scoreL * scoreWidth, 0, scoreWidth, scoreLTex.Height), Color.White);
            spriteBatch.Draw(scoreRTex, scoreRPos, new Rectangle(scoreR * scoreWidth, 0, scoreWidth, scoreLTex.Height), Color.White);
            spriteBatch.Draw(paddleL, paddleLRect, Color.White);
            spriteBatch.Draw(paddleR, paddleRRect, Color.White);
        }
    }
}
