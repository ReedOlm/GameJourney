using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Snake.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snake.Models
{
    public class Fruit
    {
        public Texture2D fruitTex { get; set; }
        public Rectangle fruitPos { get; set; }
        public Rectangle topLeft { get; set; }
        public Color color { get; set; }

        public Fruit(int tileSize, Rectangle topLeft)
        {
            this.fruitPos = new Rectangle(topLeft.X + (MainGame.r.Next(23) * tileSize), topLeft.Y + (MainGame.r.Next(23) * tileSize), tileSize, tileSize);
            this.topLeft = topLeft;
            this.color = Color.White;
        }

        public void Respawn(int tileSize)
        {
                // Options are red blue white orange yellow
                int fc = MainGame.r.Next(1, 6);
                this.fruitPos = new Rectangle(topLeft.X + (MainGame.r.Next(23) * tileSize), topLeft.Y + (MainGame.r.Next(23) * tileSize), tileSize, tileSize);
                switch (fc)
                {
                    case 1:
                        this.color = Color.Red;
                        break;
                    case 2:
                        this.color = Color.Blue;
                        break;
                    case 3:
                        this.color = Color.White;
                        break;
                    case 4:
                        this.color = Color.Orange;
                        break;
                    case 5:
                        this.color = Color.Yellow;
                        break;
                }
        }
    }
}
