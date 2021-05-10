using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SpaceInvaders
{
    class EnemySpawner
    {
        private int borderWidth;
        private int borderHeight;
        public List<Enemy> Enemies = new List<Enemy>();
        private Random random = new Random();
        private TimeSpan lastEnemySpawn;
        public TimeSpan SpawnInterval = TimeSpan.FromSeconds(3);

        public EnemySpawner(int gameWidth, int gameHeight)
        {
            borderWidth = gameWidth;
            borderHeight = gameHeight;
        }
        private Vector2 getRandomLocation()
        {
            int X = random.Next(borderWidth);
            int Y = random.Next(borderHeight);
            Vector2 pos = new Vector2(X, Y);
            return pos;
        }
        public void Reset()
        {
            SpawnInterval = TimeSpan.FromSeconds(3);
            lastEnemySpawn = TimeSpan.Zero;
            Enemies.Clear();
        }

        public bool canSpawn(GameTime gameTime)
        {
            bool canSpawn = gameTime.TotalGameTime - (TimeSpan)lastEnemySpawn >= SpawnInterval;
            // Increase difficulty TODO: Player Score or GameTime?
            if (SpawnInterval.TotalMilliseconds > 500) {
                SpawnInterval = SpawnInterval.Subtract(TimeSpan.FromMilliseconds(0.1));
            }

            if (canSpawn)
            {
                lastEnemySpawn = gameTime.TotalGameTime;
                return true;
            }
            return false;
        }

        public void Spawn()
        {
            Vector2 position = getRandomLocation();
            Enemy enemy = new Enemy(borderWidth, borderHeight, position);
            Enemies.Add(enemy);
        }

    }
}
