using System;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public enum EnemyType
    {
        Seeker = 0,
        Wanderer,
    }
    class Enemy : Entity
    {
        private float disabledTimer = 0.3f;
        private Random rand = new Random();
        public bool isDisabled = true;
        private float newPathTimer;
        private Vector2 oldPath;
        private readonly EnemyType _type;
        public EnemyType Type
        {
            get { return _type; }
        }

        public Enemy(Vector2 pos, EnemyType type)
        {
            _type = type; ;
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

        private Vector2 GetRandomLocation()
        {
            int X = rand.Next(Game.self.gameWidth);
            int Y = rand.Next(Game.self.gameHeight);
            Vector2 pos = new Vector2(X, Y);
            return pos;
        }

        private bool ShouldGetNewPath(GameTime gameTime)
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
            if (oldPath == null || ShouldGetNewPath(gameTime))
            {
                path = GetRandomLocation() - Position;
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
            if (_type is EnemyType.Wanderer)
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

        public void HandleEnemyCollision(Enemy otherEnemy)
        {
            Vector2 distance = Position - otherEnemy.Position;
            Velocity += 10 * distance / (distance.LengthSquared() + 1);
        }

        private void DisabledCooldown(GameTime gameTime)
        {
            if (isDisabled)
            {
                disabledTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (disabledTimer <= 0f)
                {
                    isDisabled = false;
                }
            }

        }
        public void Update(GameTime gameTime)
        {
            DisabledCooldown(gameTime);

            switch (_type)
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
