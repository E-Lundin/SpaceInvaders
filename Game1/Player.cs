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
        private int vel = 5;
        private int score = 0;
        public List<Heart> Hearts = new List<Heart>();
        public List<Shot> Shots = new List<Shot>();
        private TimeSpan lastPlayerShot;
        public TimeSpan ShootInterval = TimeSpan.FromMilliseconds(100);
        static Random rand = new Random();
           

        // Boosters
        private float CanonSpeedBoosterTimer;
        private bool CanonSpeedBoosterActive = false;
        private float ShipSpeedBoosterTimer;
        private bool ShipSpeedBoosterActive = false;
        private float InvincibleBoosterTimer;
        public bool InvincibleBoosterActive = false;


        public Player(int gameWidth, int gameHeight)
        {
            borderWidth = gameWidth;
            borderHeight = gameHeight;
            image = Images.Ship;

            for (int idx = 0; idx < 3; idx++)
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

        public int Score
        {
            get { return score; }
            set { score = value; }
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
            Shot s = new Shot(position, velocity);
            Shots.Add(s);
        }

        public void RemoveHeart()
        {
            if (!Hearts.Any())
            {
                return;
            }

            int idx = availableHearts - 1;
            Heart heart = Hearts[idx];
            if (availableHearts > 3)
            {
                Hearts.Remove(heart);
            }
            else
            {
                heart.IsConsumed = true;
            }
            // Set invincible for 0.3s
            InvincibleBooster(0.5f);
        }

        public void AddHeart()
        {
            // Make the latest removed heart consumeable again:
            if (availableHearts < 3)
            {
                int idx = availableHearts;
                Heart heart = Hearts[idx];
                heart.IsConsumed = false;
            }
            else
            {
                // Add a new heart
                int idx = availableHearts;
                Hearts.Add(new Heart(idx));

            }
        }

        public void CanonSpeedBooster()
        {
            CanonSpeedBoosterTimer = 4f;
            ShootInterval = TimeSpan.FromMilliseconds(50);
            CanonSpeedBoosterActive = true;
        }

        public void ShipSpeedBooster()
        {
            ShipSpeedBoosterTimer = 6f;
            vel = 8;
            ShipSpeedBoosterActive = true;
        }

        public void InvincibleBooster(float duration, bool fromBooster = false)
        {
            InvincibleBoosterTimer = duration;
            InvincibleBoosterActive = true;

            if (fromBooster)
            {
                color = Color.CornflowerBlue;
            }
        }

        public void consumeBooster(Booster booster)
        {
            switch (booster.type)
            {
                case (BoosterType.AdditionalHeart):
                    AddHeart();
                    break;
                case (BoosterType.FasterCannon):
                    CanonSpeedBooster();
                    break;
                case (BoosterType.FasterShip):
                    ShipSpeedBooster();
                    break;
                case (BoosterType.Invincible):
                    InvincibleBooster(2f, fromBooster: true);
                    break;
            }
            return;
        }
        
        public void AddPoints(int points)
        {
            Score += points;
            // TODO implement multiplier;
        }

        public void Reset()
        {
            // Reset hearts
            Hearts.Clear();
            for (int idx = 0; idx < 3; idx++)
            {
                Hearts.Add(new Heart(idx));
            }
            // Reset score
            score = 0;
            // Reset Shots
            Shots.Clear();
            lastPlayerShot = TimeSpan.Zero;

            // Reset Boosters
            vel = 5;
            CanonSpeedBoosterTimer = 0f;
            CanonSpeedBoosterActive = false;
            ShipSpeedBoosterTimer = 0f;
            ShipSpeedBoosterActive = false;
            InvincibleBoosterTimer = 0f;
            InvincibleBoosterActive = false;

    }
        private void enforceBorder()
        {
            // Assert position
            if (Position.X >= (borderWidth - image.Width))
                Position.X = vel;
            if (Position.X <= 0)
                Position.X = (borderWidth - image.Width - 5);
            if (Position.Y >= (borderHeight - image.Height))
                Position.Y -= vel;
            if (Position.Y <= 0)
                Position.Y += vel;
        }
        public void Move(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
                Position.Y -= vel;

            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
                Position.X -= vel;

            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
                Position.Y += vel;

            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
                Position.X += vel;
            enforceBorder();
        }

        public void HandleBoosters(GameTime gameTime)
        {
            if (CanonSpeedBoosterActive)
            {
                CanonSpeedBoosterTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (CanonSpeedBoosterTimer <= 0f)
                {
                    CanonSpeedBoosterActive = false;
                    CanonSpeedBoosterTimer = 4f;
                    ShootInterval = TimeSpan.FromMilliseconds(100);
                }

            }

            if (ShipSpeedBoosterActive)
            {
                ShipSpeedBoosterTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (ShipSpeedBoosterTimer <= 0f)
                {
                    ShipSpeedBoosterActive = false;
                    ShipSpeedBoosterTimer = 6f;
                    vel = 5;

                }

            }

            if (InvincibleBoosterActive)
            {
                InvincibleBoosterTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (InvincibleBoosterTimer <= 0f)
                {
                    InvincibleBoosterActive = false;
                    color = Color.White;
                }

            }
        }

        public void Update(KeyboardState keyboardState, MouseState mouseState, GameTime gameTime)
        {
            Move(keyboardState);
            HandleBoosters(gameTime);

            Vector2 aim = GetAim(mouseState);
            if (aim.LengthSquared() > 0 && canShoot(gameTime))
            {
                float aimAngle = (float)Math.Atan2(aim.Y, aim.X);
                Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);

                float randomSpread = NextFloat(-0.04f, 0.04f) + NextFloat(-0.04f, 0.04f);
                Vector2 vel = FromPolar(aimAngle + randomSpread, 11f);

                Vector2 offset = Vector2.Transform(new Vector2(25, -8), aimQuat);
                Shoot(Position + offset, vel);

                offset = Vector2.Transform(new Vector2(25, 8), aimQuat);
                Shoot(Position + offset, vel);
            }

        }

        public static float NextFloat(float minValue, float maxValue)
        {
            return (float)rand.NextDouble() * (maxValue - minValue) + minValue;
        }

        public static Vector2 FromPolar(float angle, float magnitude)
        {
            return magnitude * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }
    }

}

