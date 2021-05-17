using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    class EntityManager
    {
        public Player player;
        public static EnemySpawner enemySpawner;
        public static BoosterSpawner boosterSpawner;
        static Random rand = new Random();
        public static EntityManager self;


        public EntityManager()
        {
            self = this;
            player = new Player();
            enemySpawner = new EnemySpawner();
            boosterSpawner = new BoosterSpawner();
        }
        public void Reset()
        {
            // Reset hearts, score & Shots
            player.Reset();
            // Reset enemies & Timespans
            enemySpawner.Reset();
            // Reset boosters
            boosterSpawner.Reset();
        }
        public void SetPlayerImage(Texture2D skin)
        {
            player.image = skin;
        }

        private bool isColliding(Entity e1, Entity e2)
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
                foreach (Shot shot in player.Shots)
                {
                    if (isColliding(shot, enemy))
                    {
                        enemy.shouldRemove = true;
                        shot.shouldRemove = true;
                        player.AddPoints(5);
                    }
                }

                // Check for collisions with the player
                if (isColliding(enemy, player) && !enemy.isDisabled && !player.InvincibleBoosterActive)
                {
                    enemy.shouldRemove = true;
                    player.RemoveHeart();
                    if (player.AvailableHearts == 0)
                    {
                        Game.CurrentGameState = GameState.GameOver;
                    }
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
            foreach (Booster booster in boosterSpawner.Boosters)
            {
                if (isColliding(booster, player))
                {
                    player.consumeBooster(booster);
                    booster.shouldRemove = true;
                }
            }
        }

        public void Update(KeyboardState keyboardState, MouseState mouseState, GameTime gameTime)
        {
            player.Update(keyboardState, mouseState, gameTime);

            if (enemySpawner.canSpawn(gameTime))
            {
                int maxWaveSize;
                if (player.Score < 150)
                    maxWaveSize = 10;
                else if (player.Score < 350)
                    maxWaveSize = 15;
                else if (player.Score < 450)
                    maxWaveSize = 20;
                else
                    maxWaveSize = 25;

                int waveSize = rand.Next(maxWaveSize);
                for (int i = 0; i <= waveSize; i++)
                {
                    enemySpawner.Spawn();
                }
            }

            if (boosterSpawner.canSpawn(gameTime))
            {
                boosterSpawner.Spawn();
            }

            foreach (Shot shot in player.Shots)
            {
                shot.Update();
            }

            foreach (Enemy enemy in enemySpawner.Enemies)
            {
                enemy.GetPath(player.Position);
                enemy.Update(gameTime);
            }
            CheckCollisions();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            player.Draw(spriteBatch);

            if (player.AvailableHearts != 0)
            {
                foreach (Heart heart in player.Hearts)
                {
                    heart.Draw(spriteBatch);
                }
            }

            if (player.Shots.Count != 0)
            {
                HashSet<Shot> toRemove = new HashSet<Shot>();
                foreach (Shot shot in player.Shots)
                {
                    if (shot.shouldRemove)
                        toRemove.Add(shot);
                    else
                        shot.Draw(spriteBatch);
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

            string score = "Score:" + player.Score;
            float textWidth = Images.Font.MeasureString(score).X;
            Vector2 pos = new Vector2(Game.self.gameWidth - textWidth - 5, 5);
            spriteBatch.DrawString(Images.Font, score, pos, Color.White);
        }
    }
}
