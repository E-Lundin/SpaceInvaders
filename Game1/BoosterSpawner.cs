using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    class BoosterSpawner
    {
        public List<Booster> Boosters = new List<Booster>();
        private Random random = new Random();
        private TimeSpan lastBoosterSpawn;
        private double spawnChance = 0.2;
        private TimeSpan spawnInterval = TimeSpan.FromSeconds(7);

        public BoosterSpawner() { }

        /// <summary>
        /// Returns a random spawn location
        /// </summary>
        private Vector2 GetRandomLocation()
        {
            int X = random.Next(Game.self.gameWidth - 15);
            int Y = random.Next(Game.self.gameHeight - 15);
            Vector2 pos = new Vector2(X, Y);
            return pos;
        }

        /// <summary>
        /// Resets the state of the BoosterSpawner
        /// </summary>
        public void Reset()
        {
            spawnInterval = TimeSpan.FromSeconds(3);
            lastBoosterSpawn = TimeSpan.Zero;
            spawnChance = 0.2;
            Boosters.Clear();
        }

        /// <summary>
        /// Determines whether a booster can spawn
        /// </summary>
        /// <param name="gameTime"></param>
        public bool CanSpawn(GameTime gameTime)
        {
            // Spawn interval, with a increasing probability to spawn a Booster.
            bool canSpawn = gameTime.TotalGameTime - (TimeSpan)lastBoosterSpawn >= spawnInterval
                && (spawnChance > random.NextDouble());

            if (spawnChance < 0.9)
            {
                //spawnInterval = spawnInterval.Subtract(TimeSpan.FromMilliseconds(0.1));
                spawnChance += 0.00001;
            }

            if (canSpawn)
            {
                lastBoosterSpawn = gameTime.TotalGameTime;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Spawns a Booster of random type at a random location
        /// </summary>
        public void Spawn()
        {
            int type = random.Next(4);
            Vector2 position = GetRandomLocation();
            Booster booster = new Booster(position, (BoosterType)type);
            Boosters.Add(booster);
        }

    }
}
