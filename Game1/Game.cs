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
        Texture2D shipImg;
        private Player player;
        //private static readonly TimeSpan ShootInterval = TimeSpan.FromMilliseconds(150);
        //private TimeSpan lastPlayerShot;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            player = new Player(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
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
            shipImg = Content.Load<Texture2D>("ship");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState keyboardState = Keyboard.GetState();
            player.Move(keyboardState);

            if (keyboardState.IsKeyDown(Keys.Space) && (player.canShoot(gameTime)))
            {
                player.Shoot();
            }

            foreach (Shot shot in player.Shots)
            {
                shot.Update();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            player.Draw(_spriteBatch, shipImg);

            if (player.Shots.Count != 0)
            {
                HashSet<Shot> toRemove = new HashSet<Shot>();
                foreach (Shot shot in player.Shots)
                {
                    if (shot.shouldRemove)
                        toRemove.Add(shot);
                    else
                        shot.Draw(_spriteBatch, shipImg);
                }
                player.Shots.RemoveAll(toRemove.Contains);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
