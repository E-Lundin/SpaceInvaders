using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    class Menu
    {
        // Inspiration från https://csharpskolan.se/article/monogame-meny/
        public List<MenuChoice> MenuChoices = new List<MenuChoice>();
        public string currentSelected = "NONE";
        public KeyboardState CurrentState { get; set; }
        public KeyboardState LastState { get; set; }
        private HighScoreScreen Highscores = new HighScoreScreen();
        public static Menu self;

        public Menu(int gameWidth, int gameHeight)
        {
            self = this;
            MenuChoices.Add(new MenuChoice() { Text = "START", Selected = true });
            MenuChoices.Add(new MenuChoice() { Text = "HIGHSCORES" });
            MenuChoices.Add(new MenuChoice() { Text = "CHARACTER" });
            MenuChoices.Add(new MenuChoice() { Text = "QUIT" });
            float startY = 0.2f * gameHeight;

            foreach (MenuChoice choice in MenuChoices)
            {
                Vector2 size = Images.Font.MeasureString(choice.Text);
                choice.Y = startY;
                choice.X = gameWidth / 2.0f - size.X / 2;
                startY += 70;
            }
        }
        public bool KeyPressed(Keys key)
        {
            return CurrentState.IsKeyDown(key) && !LastState.IsKeyDown(key);
        }

        public void Update(KeyboardState keyboardState, GameTime gameTime)
        {
            LastState = CurrentState;
            CurrentState = keyboardState;

            switch (currentSelected)
            {
                case "NONE":
                    if (KeyPressed(Keys.Down))
                        NextMenuChoice();
                    if (KeyPressed(Keys.Up))
                        PreviousMenuChoice();
                    if (KeyPressed(Keys.Enter))
                    {
                        var selectedChoice = MenuChoices.First(c => c.Selected);
                        currentSelected = selectedChoice.Text;
                    }
                    break;

                case "HIGHSCORES":
                    if (KeyPressed(Keys.Enter))
                    {
                        currentSelected = "NONE";
                    }
                    break;
            }
        }

        private void PreviousMenuChoice()
        {
            int selectedIndex = MenuChoices.IndexOf(MenuChoices.First(c => c.Selected));
            MenuChoices[selectedIndex].Selected = false;
            selectedIndex--;
            if (selectedIndex < 0)
                selectedIndex = MenuChoices.Count - 1;
            MenuChoices[selectedIndex].Selected = true;
        }

        private void NextMenuChoice()
        {
            int selectedIndex = MenuChoices.IndexOf(MenuChoices.First(c => c.Selected));
            MenuChoices[selectedIndex].Selected = false;
            selectedIndex++;
            if (selectedIndex >= MenuChoices.Count)
                selectedIndex = 0;
            MenuChoices[selectedIndex].Selected = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (currentSelected) {
                case "NONE":
                    foreach (MenuChoice choice in MenuChoices)
                    {

                        Color color = choice.Selected ? Color.Yellow: Color.White;
                        spriteBatch.DrawString(Images.MenuFont, choice.Text, choice.Position, color);
                    }
                    break;
                case "START":
                    Game.self.Reset();
                    Game.currentGameState = GameState.Playing;
                    currentSelected = "NONE";
                    break;

                case "HIGHSCORES":
                    KeyboardState keyboardState = Keyboard.GetState();
                    LastState = CurrentState;
                    CurrentState = keyboardState;
                    Highscores.Draw(spriteBatch);
                    break;

                case "QUIT":
                    Game.self.Exit();
                    break;
            }
        }
    }
}
