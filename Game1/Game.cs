using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public enum GameState
    {
        Menu,
        Playing,
        GameOver
    }
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private EntityManager entityManager;
        private Menu GameMenu;
        public static Viewport gameSize;
        static Random rand = new Random();
        private static GameState currentGameState;
        public static Game self;
        private static bool hasLoggedScore = false;
        public int gameWidth;
        public int gameHeight;

        public Game()
        {
            self = this;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            currentGameState = GameState.Menu;
        }

        public static GameState CurrentGameState
        {
            get { return currentGameState; }
            set { currentGameState = value; }
        }

        public static bool HasLoggedScore
        {
            get { return hasLoggedScore; }
            set { hasLoggedScore = value; }
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
            gameWidth = _graphics.PreferredBackBufferWidth;
            gameHeight = _graphics.PreferredBackBufferHeight;
            Images.Load(Content);

            entityManager = new EntityManager();
            GameMenu = new Menu();
        }

        public void Reset()
        {
            // Update Highscores
            hasLoggedScore = false;
            // Update Entities
            entityManager.Reset();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            switch (currentGameState)
            {
                case GameState.Playing:
                    entityManager.Update(keyboardState, mouseState, gameTime);
                    break;
                case GameState.GameOver:
                    GameOverScreen.Update(keyboardState, entityManager.Player.Score);
                    break;
                case GameState.Menu:
                    GameMenu.Update(keyboardState, mouseState, gameTime);
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
                    entityManager.Draw(_spriteBatch);
                    break;
                case GameState.GameOver:
                    GameOverScreen.Draw(_spriteBatch, entityManager.Player.Score);
                    break;
                case GameState.Menu:
                    GameMenu.Draw(_spriteBatch);
                    break;
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
