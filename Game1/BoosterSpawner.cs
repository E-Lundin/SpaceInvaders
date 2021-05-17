using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace SpaceInvaders
{
    //PowerUp p = new PowerUp((PowerUpType)type, this);
    class BoosterSpawner
    {
        public List<Booster> Boosters = new List<Booster>();
        private Random random = new Random();
        private TimeSpan lastBoosterSpawn;
        public TimeSpan SpawnInterval = TimeSpan.FromSeconds(7);
        private double SpawnChance = 0.2;

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
            SpawnInterval = TimeSpan.FromSeconds(3);
            lastBoosterSpawn = TimeSpan.Zero;
            SpawnChance = 0.2;
            Boosters.Clear();
        }

        public bool canSpawn(GameTime gameTime)
        {
            // Spawn interval, with a increasing probability to spawn a Booster.
            bool canSpawn = gameTime.TotalGameTime - (TimeSpan)lastBoosterSpawn >= SpawnInterval
                && (SpawnChance > random.NextDouble());

            if (SpawnChance < 0.9)
            {
                //SpawnInterval = SpawnInterval.Subtract(TimeSpan.FromMilliseconds(0.1));
                SpawnChance += 0.00001;
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
