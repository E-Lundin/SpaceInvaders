using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    static class GameOverScreen
    {

        public static void Update(KeyboardState keyboardState, int playerScore)
        {
            if (!Game.HasLoggedScore)
            {
                HighScoreScreen.self.LogScore(playerScore);
                Game.HasLoggedScore = true;
            }
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                Game.CurrentGameState = GameState.Menu;
            }
        }
        public static void Draw(SpriteBatch spriteBatch, int playerScore)
        {
            int highscore;

            if (HighScoreScreen.BiggestHighScore.HasValue)
            {
                highscore = (int)HighScoreScreen.BiggestHighScore;
                if (playerScore > highscore)
                {
                    highscore = playerScore;
                }
            }
            else
            {
                highscore = playerScore;
            }
            string message = String.Format("Game Over\nScore: {0}\nHigh Score: {1}\nPress Space to return to Menu.", playerScore, highscore);
            Vector2 textSize = Images.Font.MeasureString(message);
            Vector2 screenSize = new Vector2(Game.self.gameWidth, Game.self.gameHeight);
            Vector2 position = (screenSize / 2 - textSize / 2);
            spriteBatch.DrawString(Images.Font, message, position, Color.White);
        }
    }
}
