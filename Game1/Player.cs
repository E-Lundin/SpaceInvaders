using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    class Player: Entity
    {
        private int _availableHearts;
        private int _vel = 5;
        private int _score = 0;
        public List<Heart> Hearts = new List<Heart>();
        public List<Shot> Shots = new List<Shot>();
        private TimeSpan lastPlayerShot;
        public TimeSpan ShootInterval = TimeSpan.FromMilliseconds(100);
        static Random rand = new Random();
           

        // Boosters
        private float canonSpeedBoosterTimer;
        private bool canonSpeedBoosterActive = false;
        private float shipSpeedBoosterTimer;
        private bool shipSpeedBoosterActive = false;
        private float invincibleBoosterTimer;
        public bool InvincibleBoosterActive = false;


        public Player()
        {
            image = LoadImage();

            for (int idx = 0; idx < 3; idx++)
            {
                Hearts.Add(new Heart(idx));
            }
            _availableHearts = Hearts.Count;
        }

        static Texture2D LoadImage()
        {
            string imageName;

            if (File.Exists("skin.txt"))
            {
                imageName = File.ReadAllText("skin.txt");
            }
            else
            {
                imageName =  "default";
            }

            return imageName switch
            {
                "default" => Images.Ship,
                "shipred" => Images.ShipRed,
                "ship1" => Images.Ship1,
                "ship2" => Images.Ship2,
                _ => Images.Ship,
            };
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
                _availableHearts = available;
                return available;
            }
        }

        public int Score
        {
            get { return _score; }
            set { _score = value; }
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

            int idx = _availableHearts - 1;
            Heart heart = Hearts[idx];
            if (_availableHearts > 3)
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
            if (_availableHearts < 3)
            {
                int idx = _availableHearts;
                Heart heart = Hearts[idx];
                heart.IsConsumed = false;
            }
            else
            {
                // Add a new heart
                int idx = _availableHearts;
                Hearts.Add(new Heart(idx));

            }
        }

        public void CanonSpeedBooster()
        {
            canonSpeedBoosterTimer = 4f;
            ShootInterval = TimeSpan.FromMilliseconds(50);
            canonSpeedBoosterActive = true;
        }

        public void ShipSpeedBooster()
        {
            shipSpeedBoosterTimer = 6f;
            _vel = 8;
            shipSpeedBoosterActive = true;
        }

        public void InvincibleBooster(float duration, bool fromBooster = false)
        {
            invincibleBoosterTimer = duration;
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
            // Reset _score
            _score = 0;
            // Reset Shots
            Shots.Clear();
            lastPlayerShot = TimeSpan.Zero;

            // Reset Boosters
            _vel = 5;
            canonSpeedBoosterTimer = 0f;
            canonSpeedBoosterActive = false;
            shipSpeedBoosterTimer = 0f;
            shipSpeedBoosterActive = false;
            invincibleBoosterTimer = 0f;
            InvincibleBoosterActive = false;

    }
        private void enforceBorder()
        {
            // Assert position
            if (Position.X >= (Game.self.gameWidth - image.Width))
                Position.X = _vel;
            if (Position.X <= 0)
                Position.X = (Game.self.gameWidth - image.Width - 5);
            if (Position.Y >= (Game.self.gameHeight - image.Height))
                Position.Y -= _vel;
            if (Position.Y <= 0)
                Position.Y += _vel;
        }
        public void Move(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
                Position.Y -= _vel;

            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
                Position.X -= _vel;

            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
                Position.Y += _vel;

            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
                Position.X += _vel;
            enforceBorder();
        }

        public void HandleBoosters(GameTime gameTime)
        {
            if (canonSpeedBoosterActive)
            {
                canonSpeedBoosterTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (canonSpeedBoosterTimer <= 0f)
                {
                    canonSpeedBoosterActive = false;
                    canonSpeedBoosterTimer = 4f;
                    ShootInterval = TimeSpan.FromMilliseconds(100);
                }

            }

            if (shipSpeedBoosterActive)
            {
                shipSpeedBoosterTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (shipSpeedBoosterTimer <= 0f)
                {
                    shipSpeedBoosterActive = false;
                    shipSpeedBoosterTimer = 6f;
                    _vel = 5;

                }

            }

            if (InvincibleBoosterActive)
            {
                invincibleBoosterTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (invincibleBoosterTimer <= 0f)
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

