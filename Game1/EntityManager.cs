using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    class EntityManager
    {
        public Player Player;
        private EnemySpawner enemySpawner;
        private BoosterSpawner boosterSpawner;
        static Random rand = new Random();
        public static EntityManager self;


        public EntityManager()
        {
            self = this;
            Player = new Player();
            enemySpawner = new EnemySpawner();
            boosterSpawner = new BoosterSpawner();
        }
        public void Reset()
        {
            // Reset hearts, score & Shots
            Player.Reset();
            // Reset enemies & Timespans
            enemySpawner.Reset();
            // Reset boosters
            boosterSpawner.Reset();
        }
        public void SetPlayerImage(Texture2D skin)
        {
            Player.image = skin;
        }

        private bool IsColliding(Entity e1, Entity e2)
        {
            float radius = e1.Radius + e2.Radius;
            bool overlapping = Vector2.DistanceSquared(e1.Position, e2.Position) < radius * radius;
            bool activeEntities = !e1.shouldRemove && !e2.shouldRemove;
            return activeEntities && overlapping;
        }

        private void CheckCollisions()
        {
            // Check for collisions between all currently active entitites
            foreach (Enemy enemy in enemySpawner.Enemies)
            {
                // Check for collision with Bullets
                foreach (Shot shot in Player.Shots)
                {
                    if (IsColliding(shot, enemy))
                    {
                        enemy.shouldRemove = true;
                        shot.shouldRemove = true;
                        Player.AddPoints(5);
                    }
                }

                // Check for collisions with the Player
                if (IsColliding(enemy, Player) && !enemy.isDisabled && !Player.InvincibleBoosterActive)
                {
                    enemy.shouldRemove = true;
                    Player.RemoveHeart();
                    if (Player.AvailableHearts == 0)
                    {
                        Game.CurrentGameState = GameState.GameOver;
                    }
                }

                // Check for collision between enemies
                foreach (Enemy otherEnemy in enemySpawner.Enemies)
                {
                    if (IsColliding(enemy, otherEnemy))
                    {
                        enemy.HandleEnemyCollision(otherEnemy);
                        otherEnemy.HandleEnemyCollision(enemy);
                    }
                }
            }
            foreach (Booster booster in boosterSpawner.Boosters)
            {
                if (IsColliding(booster, Player))
                {
                    Player.ConsumeBooster(booster);
                    booster.shouldRemove = true;
                }
            }
        }

        public void Update(KeyboardState keyboardState, MouseState mouseState, GameTime gameTime)
        {
            Player.Update(keyboardState, mouseState, gameTime);

            if (enemySpawner.CanSpawn(gameTime))
            {
                int maxWaveSize;
                if (Player.Score < 150)
                    maxWaveSize = 7;
                else if (Player.Score < 350)
                    maxWaveSize = 10;
                else if (Player.Score < 450)
                    maxWaveSize = 15;
                else
                    maxWaveSize = 20;

                int waveSize = rand.Next(maxWaveSize);
                for (int i = 0; i <= waveSize; i++)
                {
                    enemySpawner.Spawn();
                }
            }

            if (boosterSpawner.CanSpawn(gameTime))
            {
                boosterSpawner.Spawn();
            }

            foreach (Shot shot in Player.Shots)
            {
                shot.Update();
            }

            foreach (Enemy enemy in enemySpawner.Enemies)
            {
                enemy.GetPath(Player.Position);
                enemy.Update(gameTime);
            }
            CheckCollisions();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Player.Draw(spriteBatch);

            if (Player.AvailableHearts != 0)
            {
                foreach (Heart heart in Player.Hearts)
                {
                    heart.Draw(spriteBatch);
                }
            }

            if (Player.Shots.Count != 0)
            {
                HashSet<Shot> toRemove = new HashSet<Shot>();
                foreach (Shot shot in Player.Shots)
                {
                    if (shot.shouldRemove)
                        toRemove.Add(shot);
                    else
                        shot.Draw(spriteBatch);
                }
                Player.Shots.RemoveAll(toRemove.Contains);
            }

            if (enemySpawner.Enemies.Count != 0)
            {
                HashSet<Enemy> toRemove = new HashSet<Enemy>();
                foreach (Enemy enemy in enemySpawner.Enemies)
                {
                    if (enemy.shouldRemove)
                        toRemove.Add(enemy);
                    else
                        enemy.Draw(spriteBatch);
                }
                enemySpawner.Enemies.RemoveAll(toRemove.Contains);
            }

            if (boosterSpawner.Boosters.Count != 0)
            {
                HashSet<Booster> toRemove = new HashSet<Booster>();
                foreach (Booster booster in boosterSpawner.Boosters)
                {
                    if (booster.shouldRemove)
                        toRemove.Add(booster);
                    else
                        booster.Draw(spriteBatch);
                }
                boosterSpawner.Boosters.RemoveAll(toRemove.Contains);
            }

            string score = "Score:" + Player.Score;
            float textWidth = Images.Font.MeasureString(score).X;
            Vector2 pos = new Vector2(Game.self.gameWidth - textWidth - 5, 5);
            spriteBatch.DrawString(Images.Font, score, pos, Color.White);
        }
    }
}
