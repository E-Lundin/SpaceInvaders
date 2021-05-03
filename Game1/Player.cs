using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    class Player: Entity
    {
        private int borderWidth;
        private int borderHeight;
        public List<Shot> Shots = new List<Shot>();
        private TimeSpan lastPlayerShot;
        public TimeSpan ShootInterval = TimeSpan.FromMilliseconds(100);

        public Player(int gameWidth, int gameHeight)
        {
            borderWidth = gameWidth;
            borderHeight = gameHeight;
            image = Images.Ship;
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

        public Vector2 GetAim(MouseState mouseState)
        {
            Vector2 Mouse = new Vector2(mouseState.X, mouseState.Y);
            Vector2 aim = Mouse - Position;
            if (aim != Vector2.Zero)
                aim.Normalize();

            return aim;
        }

        public void Shoot(Vector2 position, Vector2 velocity)
        {
            //Vector2 Direction = Target - Position;
            //if (Direction != Vector2.Zero)
               //Direction.Normalize();
            //Shot s = new Shot(borderWidth, borderHeight, ship.X + 1, ship.Y + 1);
            Shot s = new Shot(position, velocity);
            Shots.Add(s);
        }

        private void enforceBorder()
        {
            // Assert position
            if (Position.X >= (borderWidth - image.Width))
                Position.X = 5;
            if (Position.X <= 0)
                Position.X = (borderWidth - image.Width - 5);
            if (Position.Y >= (borderHeight - image.Height))
                Position.Y -= 5;
            if (Position.Y <= 0)
                Position.Y += 5;
        }
        public void Move(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
                Position.Y -= 5;

            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
                Position.X -= 5;

            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
                Position.Y += 5;

            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
                Position.X += 5;
            enforceBorder();
        }
    }
}
