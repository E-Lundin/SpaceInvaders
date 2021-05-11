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
        private Player player;
        private EnemySpawner enemySpawner;
        private BoosterSpawner boosterSpawner;
        public static Viewport gameSize;
        static Random rand = new Random();

        enum GameState
        {
            Menu = 0,
            Playing = 1,
            GameOver = 2,
        }
        GameState currentGameState;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            currentGameState = GameState.Playing;
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
            boosterSpawner = new BoosterSpawner(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            // TODO: use this.Content to load your game content here
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
                        currentGameState = GameState.GameOver;
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

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState keyboardState = Keyboard.GetState();
            switch (currentGameState)
            {
                case GameState.Playing:
                    MouseState mouseState = Mouse.GetState();
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
                    checkCollisions();
                    break;

                case GameState.GameOver:
                    if (keyboardState.IsKeyDown(Keys.Enter))
                    {
                        //TODO: Return to menu:
                        currentGameState = GameState.Menu;
                    }
                    break;

                case GameState.Menu:
                    break;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            switch (currentGameState)
            {

                case GameState.Playing:
                    player.Draw(_spriteBatch);

                    if (player.AvailableHearts != 0)
                    {
                        foreach (Heart heart in player.Hearts)
                        {
                            heart.Draw(_spriteBatch);
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

                    if (boosterSpawner.Boosters.Count != 0)
                    {
                        HashSet<Booster> toRemove = new HashSet<Booster>();
                        foreach (Booster booster in boosterSpawner.Boosters)
                        {
                            if (booster.shouldRemove)
                                toRemove.Add(booster);
                            else
                                booster.Draw(_spriteBatch);
                        }
                        boosterSpawner.Boosters.RemoveAll(toRemove.Contains);
                    }

                    string score = "Score:" + player.Score;
                    float textWidth = Images.Font.MeasureString(score).X;
                    Vector2 pos = new Vector2(gameSize.Width - textWidth - 5, 5);
                    _spriteBatch.DrawString(Images.Font, score, pos, Color.White);
                    break;

                case GameState.GameOver:
                    string message = String.Format("Game Over\nScore: {0}\nHigh Score: TODO\nPress Enter to return to Menu.", player.Score);
                    Vector2 textSize = Images.Font.MeasureString(message);
                    Vector2 screenSize = new Vector2(gameSize.Width, gameSize.Height);
                    Vector2 position = (screenSize / 2 - textSize / 2);
                    _spriteBatch.DrawString(Images.Font, message, position, Color.White);
                    break;
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
