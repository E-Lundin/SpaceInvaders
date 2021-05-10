using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    class Player: Entity
    {
        private int borderWidth;
        private int borderHeight;
        private int availableHearts;
        public List<Heart> Hearts = new List<Heart>();
        public List<Shot> Shots = new List<Shot>();
        private TimeSpan lastPlayerShot;
        public TimeSpan ShootInterval = TimeSpan.FromMilliseconds(100);

        public Player(int gameWidth, int gameHeight)
        {
            borderWidth = gameWidth;
            borderHeight = gameHeight;
            image = Images.Ship;

            for (int idx = 0; idx <= 3; idx++)
            {
                Hearts.Add(new Heart(idx));
            }
            availableHearts = Hearts.Count;
        }

        public int AvailableHearts
        {
            get {
                int available = 0;
                foreach (Heart heart in Hearts)
                {
                    if (!heart.IsConsumed)
                    {
                        available += 1;
                    }
                }
                availableHearts = available;
                return available;
            }
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

        public void RemoveHeart()
        {
            if (!Hearts.Any())
            {
                return;
            }

            int idx = AvailableHearts - 1;
            Heart heart = Hearts[idx];
            if (availableHearts > 3)
            {
                Hearts.Remove(heart);
            }
            else
            {
                heart.IsConsumed = true;
            }
        }

        public void AddHeart()
        {
            // Make the latest removed heart consumeable again:
            if (availableHearts < 3)
            {
                int idx = availableHearts - 1;
                Heart heart = Hearts[idx];
                heart.IsConsumed = false;
            }
            else
            {
                // Add a new heart
                int idx = availableHearts + 1;
                Hearts.Add(new Heart(idx));

            }
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
