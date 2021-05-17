using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    class EnemySpawner
    {
        public List<Enemy> Enemies = new List<Enemy>();
        private Random random = new Random();
        private TimeSpan lastEnemySpawn;
        public TimeSpan SpawnInterval = TimeSpan.FromSeconds(3);
        private double wandererChance = 0.3;

        public EnemySpawner() { }

        private Vector2 GetRandomLocation()
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

        public bool CanSpawn(GameTime gameTime)
        {
            bool canSpawn = gameTime.TotalGameTime - (TimeSpan)lastEnemySpawn >= SpawnInterval;
            // Increase difficulty TODO: Player Score or GameTime?
            if (SpawnInterval.TotalMilliseconds > 500)
            {
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
            EnemyType type;
            if (wandererChance > random.NextDouble())
                type = EnemyType.Wanderer;
            else
                type = EnemyType.Seeker;

            Vector2 position = GetRandomLocation();
            Enemy enemy = new Enemy(position, type);
            Enemies.Add(enemy);
        }

    }
}
