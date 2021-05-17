using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public enum EnemyType
    {
        Seeker = 0,
        Wanderer,
    }
    class Enemy: Entity
    {
        private float DisabledTimer = 0.3f;
        private Random rand = new Random();
        public bool isDisabled = true;
        public EnemyType type;
        private float newPathTimer;
        private Vector2 oldPath;

        public Enemy(Vector2 pos, EnemyType _type)
        {
            type = _type;
            switch (type)
            {
                case (EnemyType.Seeker):
                    image = Images.Enemy;
                    break;
                case (EnemyType.Wanderer):
                    image = Images.Enemy2;
                    break;
            }
            Position = pos;
        }

        private Vector2 getRandomLocation()
        {
            int X = rand.Next(Game.self.gameWidth);
            int Y = rand.Next(Game.self.gameHeight);
            Vector2 pos = new Vector2(X, Y);
            return pos;
        }

        private bool shouldGetNewPath(GameTime gameTime)
        {
            newPathTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (newPathTimer <= 0f)
            {
                newPathTimer = 2f;
                return true;
            }
            return false;
        }

        private void RandomMovement(GameTime gameTime)
        {
            Vector2 path;
            if (oldPath == null || shouldGetNewPath(gameTime))
            {
                path = getRandomLocation() - Position;
                if (path != Vector2.Zero)
                    path.Normalize();
                oldPath = path;
            }
            else
            {
                path = oldPath;
            }
            Velocity += path;
        }

        // Get path to the PlayerShip
        public void GetPath(Vector2 playerPosition)
        {
            if (type is EnemyType.Wanderer)
            {
                return;
            }
            Vector2 path = playerPosition - Position;
            if (path != Vector2.Zero)
                path.Normalize();

            Velocity += path;
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

            switch (type)
            {
                case (EnemyType.Seeker):
                    if (!Collision())
                    {
                        Position += Velocity;
                        Velocity *= 0.4f;
                    }
                    else
                    {
                        shouldRemove = true;
                    }
                    break;

                case (EnemyType.Wanderer):
                    RandomMovement(gameTime);
                    Position += Velocity;
                    Velocity *= 0.4f;
                    break;
            }
        }
    }
}
