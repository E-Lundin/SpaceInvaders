using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    class Player
    {
        private int borderWidth;
        private int borderHeight;
        public Rectangle ship = new Rectangle(100, 100, 20, 20);
        public List<Shot> Shots = new List<Shot>();
        private TimeSpan lastPlayerShot;
        private static readonly TimeSpan ShootInterval = TimeSpan.FromMilliseconds(150);

        public Player(int gameWidth, int gameHeight)
        {
            borderWidth = gameWidth;
            borderHeight = gameHeight;
        }

        public bool canShoot(GameTime gameTime)
        {
            bool canShoot = gameTime.TotalGameTime - (TimeSpan)lastPlayerShot >= ShootInterval;
            if (canShoot)
            {
                lastPlayerShot = gameTime.TotalGameTime;
                return true;
            }
            return false;
        }

        public void Shoot()
        {
            Shot s = new Shot(borderWidth, borderHeight, ship.X + 1, ship.Y + 1);
            Shots.Add(s);
        }

        private void enforceBorder()
        {
            // Assert position
            if (ship.X >= (borderWidth - ship.Width))
                ship.X = 5;
            if (ship.X <= 0)
                ship.X = (borderWidth - ship.Width - 5);
            if (ship.Y >= (borderHeight - ship.Height))
                ship.Y -= 5;
            if (ship.Y <= 0)
                ship.Y += 5;
        }
        public void Move(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
                ship.Y -= 5;
            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
                ship.X -= 5;
            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
                ship.Y += 5;
            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
                ship.X += 5;
            enforceBorder();
        }
        public void Draw(SpriteBatch spriteBatch, Texture2D image)
        {
            spriteBatch.Draw(image, ship, Color.Green);
        }
    }
}
