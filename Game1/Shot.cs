using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    class Shot
    {
        private int borderWidth;
        private int borderHeight;
        private Rectangle shot;
        public bool shouldRemove = false;

        public Shot(int gameWidth, int gameHeight, int x, int y)
        {
            borderWidth = gameWidth;
            borderHeight = gameHeight;
            shot = new Rectangle(x, y, 20, 20);
        }
        public void Draw(SpriteBatch spriteBatch, Texture2D image)
        {
            spriteBatch.Draw(image, shot, Color.Yellow);
        }

        private bool Collision()
        {
            bool collides = false;
            if (shot.X >= (borderWidth - shot.Width))
                collides = true;
            if (shot.X <= 0)
                collides = true;
            if (shot.Y >= (borderHeight - shot.Height))
                collides = true;
            if (shot.Y <= 0)
                collides = true;

            return collides;
        }

        public void Update()
        {
            if (!Collision()){
                shot.Y -= 6;
            }
            else
            {
                shouldRemove = true;
            }

        }
    }
}
