using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Collections.Generic;
using System;

namespace SpaceInvaders
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D shipImg;
        private Player player;
        private Enemy enemy;
        private EnemySpawner enemySpawner;
        public static Viewport gameSize;
        static Random rand = new Random();
        //private static readonly TimeSpan ShootInterval = TimeSpan.FromMilliseconds(150);
        //private TimeSpan lastPlayerShot;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //_graphics.PreferredBackBufferHeight
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            gameSize = GraphicsDevice.Viewport;
            Images.Load(Content);
            player = new Player(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            //enemy = new Enemy(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            enemySpawner = new EnemySpawner(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            // TODO: use this.Content to load your game content here
        }

        public static float NextFloat(float minValue, float maxValue)
        {
            return (float)rand.NextDouble() * (maxValue - minValue) + minValue;
        }

        public static Vector2 FromPolar(float angle, float magnitude)
        {
            return magnitude * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }

        private bool isColliding(Entity e1, Entity e2)
        {
            float radius = e1.Radius + e2.Radius;
            bool overlapping = Vector2.DistanceSquared(e1.Position, e2.Position) < radius * radius;
            bool activeEntities = !e1.shouldRemove && !e2.shouldRemove;
            return activeEntities && overlapping;
        }

        private void checkCollisions()
        {
            // Check for collisions between all currently active entitites
s            foreach (Enemy enemy in enemySpawner.Enemies)
            {   
                // Check for collision with Bullets
                foreach (Shot shot in player.Shots)
                {
                    if (isColliding(shot, enemy))
                    {
                        enemy.shouldRemove = true;
                        shot.shouldRemove = true;
                    }
                }

                // Check for collisions with the player
                if (isColliding(enemy, player))
                {
                    enemy.shouldRemove = true;
                    // TODO: Implement punishment for Player & Enemy collision
                }

                // Check for collision between enemies
                foreach (Enemy otherEnemy in enemySpawner.Enemies)
                {
                    if (isColliding(enemy, otherEnemy))
                    {
                        enemy.handleEnemyCollision(otherEnemy);
                        otherEnemy.handleEnemyCollision(enemy);
                    }
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            player.Move(keyboardState);
            Vector2 aim = player.GetAim(mouseState);

            if (aim.LengthSquared() > 0 && player.canShoot(gameTime))
            {
                float aimAngle = (float)Math.Atan2(aim.Y, aim.X);
                Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);

                float randomSpread = NextFloat(-0.04f, 0.04f) + NextFloat(-0.04f, 0.04f);
                Vector2 vel = FromPolar(aimAngle + randomSpread, 11f);

                Vector2 offset = Vector2.Transform(new Vector2(25, -8), aimQuat);
                player.Shoot(player.Position + offset, vel);

                offset = Vector2.Transform(new Vector2(25, 8), aimQuat);
                player.Shoot(player.Position + offset, vel);
            }

            if (enemySpawner.canSpawn(gameTime))
            {
                enemySpawner.Spawn();
            }
            //enemy.GetPath(player.Position);
            //enemy.Update();

            /*
            if (keyboardState.IsKeyDown(Keys.Space) && (player.canShoot(gameTime)))
            {
                MouseState mouseState = Mouse.GetState();
                Vector2 Target = new Vector2(mouseState.X, mouseState.Y);

                player.Shoot(Target);
            }
            */
            foreach (Shot shot in player.Shots)
            {
                shot.Update();
            }

            foreach (Enemy enemy in enemySpawner.Enemies)
            {
                enemy.GetPath(player.Position);
                enemy.Update();
            }
            checkCollisions();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            player.Draw(_spriteBatch);

            if (player.Shots.Count != 0)
            {
                HashSet<Shot> toRemove = new HashSet<Shot>();
                foreach (Shot shot in player.Shots)
                {
                    if (shot.shouldRemove)
                        toRemove.Add(shot);
                    else
                        shot.Draw(_spriteBatch);
                }
                player.Shots.RemoveAll(toRemove.Contains);
            }

            if (enemySpawner.Enemies.Count != 0)
            {
                HashSet<Enemy> toRemove = new HashSet<Enemy>();
                foreach (Enemy enemy in enemySpawner.Enemies)
                {
                    if (enemy.shouldRemove)
                        toRemove.Add(enemy);
                    else
                        enemy.Draw(_spriteBatch);
                }
                enemySpawner.Enemies.RemoveAll(toRemove.Contains);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
