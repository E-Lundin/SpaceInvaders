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
        public List<Enemy> Enemies = new List<Enemy>();
        private Random random = new Random();
        private TimeSpan lastEnemySpawn;
        public TimeSpan SpawnInterval = TimeSpan.FromSeconds(3);

        public EnemySpawner() { }

        private Vector2 getRandomLocation()
        {
            int X = random.Next(Game.self.gameWidth);
            int Y = random.Next(Game.self.gameHeight);
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
                SpawnInterval = SpawnInterval.Subtract(TimeSpan.FromMilliseconds(0.5));
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
            int type = random.Next(2);
            Vector2 position = getRandomLocation();
            Enemy enemy = new Enemy(position, (EnemyType)type);
            Enemies.Add(enemy);
        }

    }
}
