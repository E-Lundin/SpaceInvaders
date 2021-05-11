using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    class Enemy: Entity
    {
        private int borderWidth;
        private int borderHeight;
        private float DisabledTimer = 0.3f;
        public bool isDisabled = true;
        public Rectangle ship = new Rectangle(100, 100, 20, 20);

        public Enemy(int gameWidth, int gameHeight, Vector2 pos)
        {
            borderWidth = gameWidth;
            borderHeight = gameHeight;
            image = Images.Enemy;
            Position = pos;
        }

        // Get path to the PlayerShip
        public void GetPath(Vector2 playerPosition)
        {
            Vector2 path = playerPosition - Position;
            if (path != Vector2.Zero)
                path.Normalize();

            Velocity += path;
            //return path;
        }

        private bool Collision()
        {
            bool collides = false;

            // Check that enemy is still within border range
            if (!Game.gameSize.Bounds.Contains(Position.ToPoint()))
            {
                collides = true;
            }

            return collides;
        }

        public void handleEnemyCollision(Enemy otherEnemy)
        {
            Vector2 distance = Position - otherEnemy.Position;
            Velocity += 10 * distance / (distance.LengthSquared() + 1);
        }

        private void DisabledCooldown(GameTime gameTime)
        {
            if (isDisabled)
            {
                DisabledTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (DisabledTimer <= 0f)
                {
                    isDisabled = false;
                }

            }

        }
        public void Update(GameTime gameTime)
        {
            DisabledCooldown(gameTime);

            if (!Collision())
            {
                Position += Velocity;
                Velocity *= 0.4f;
            }
            else
            {
                shouldRemove = true;
            }

        }
    }
}
