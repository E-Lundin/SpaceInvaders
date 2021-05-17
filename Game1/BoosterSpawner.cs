using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    //PowerUp p = new PowerUp((PowerUpType)type, this);
    class BoosterSpawner
    {
        public List<Booster> Boosters = new List<Booster>();
        private Random random = new Random();
        private TimeSpan lastBoosterSpawn;
        private double spawnChance = 0.2;
        private TimeSpan spawnInterval = TimeSpan.FromSeconds(7);

        public BoosterSpawner() { }
        private Vector2 getRandomLocation()
        {
            int X = random.Next(Game.self.gameWidth - 15);
            int Y = random.Next(Game.self.gameHeight - 15);
            Vector2 pos = new Vector2(X, Y);
            return pos;
        }
        public void Reset()
        {
            spawnInterval = TimeSpan.FromSeconds(3);
            lastBoosterSpawn = TimeSpan.Zero;
            spawnChance = 0.2;
            Boosters.Clear();
        }

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

        public void Spawn()
        {
            int type = random.Next(4);
            Vector2 position = getRandomLocation();
            Booster booster = new Booster(position, (BoosterType)type);
            Boosters.Add(booster);
        }

    }
}
