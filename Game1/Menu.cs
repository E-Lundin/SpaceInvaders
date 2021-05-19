using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public enum MenuState
    {
        Menu,
        Play,
        Highscore,
        Character,
        Quit
    }
    class Menu
    {
        // Inspiration från https://csharpskolan.se/article/monogame-meny/
        public List<MenuChoice> MenuChoices = new List<MenuChoice>();
        private MenuState currentMenuState = MenuState.Menu;
        public KeyboardState CurrentState { get; set; }
        public KeyboardState LastState { get; set; }
        private HighScoreScreen highscores = new HighScoreScreen();
        private CharacterCustomization characterScreen = new CharacterCustomization();
        public static Menu self;

        public Menu()
        {
            self = this;
            MenuChoices.Add(new MenuChoice() { Text = "START", State = MenuState.Play, Selected = true });
            MenuChoices.Add(new MenuChoice() { Text = "HIGHSCORES", State = MenuState.Highscore });
            MenuChoices.Add(new MenuChoice() { Text = "CHARACTER", State = MenuState.Character });
            MenuChoices.Add(new MenuChoice() { Text = "QUIT", State = MenuState.Quit });
            float startY = 0.2f * Game.self.gameHeight;

            foreach (MenuChoice choice in MenuChoices)
            {
                Vector2 size = Images.Font.MeasureString(choice.Text);
                choice.Y = startY;
                choice.X = Game.self.gameWidth / 2.0f - size.X / 2;
                startY += 70;
            }
        }

        /// <summary>
        /// Determines if a key was pressed
        /// </summary>
        public bool KeyPressed(Keys key)
        {
            return CurrentState.IsKeyDown(key) && !LastState.IsKeyDown(key);
        }

        public void Update(KeyboardState keyboardState, MouseState mouseState, GameTime gameTime)
        {
            LastState = CurrentState;
            CurrentState = keyboardState;

            switch (currentMenuState)
            {
                case MenuState.Menu:
                    if (KeyPressed(Keys.Down))
                        NextMenuChoice();
                    if (KeyPressed(Keys.Up))
                        PreviousMenuChoice();
                    if (KeyPressed(Keys.Enter))
                    {
                        var selectedChoice = MenuChoices.First(c => c.Selected);
                        currentMenuState = selectedChoice.State;
                    }
                    break;

                case MenuState.Highscore:
                    if (KeyPressed(Keys.Enter))
                    {
                        currentMenuState = MenuState.Menu;
                    }
                    break;

                case MenuState.Character:
                    if (KeyPressed(Keys.Enter))
                    {
                        currentMenuState = MenuState.Menu;
                    }
                    characterScreen.Update(keyboardState, mouseState, gameTime);
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
            switch (currentMenuState)
            {
                case MenuState.Menu:
                    foreach (MenuChoice choice in MenuChoices)
                    {
                        Color color = choice.Selected ? Color.Yellow : Color.White;
                        spriteBatch.DrawString(Images.MenuFont, choice.Text, choice.Position, color);
                    }
                    break;
                case MenuState.Play:
                    Game.self.Reset();
                    Game.CurrentGameState = GameState.Playing;
                    currentMenuState = MenuState.Menu;
                    break;

                case MenuState.Highscore:
                    KeyboardState keyboardState = Keyboard.GetState();
                    LastState = CurrentState;
                    CurrentState = keyboardState;
                    highscores.Draw(spriteBatch);
                    break;

                case MenuState.Character:
                    characterScreen.Draw(spriteBatch);
                    break;

                case MenuState.Quit:
                    Game.self.Exit();
                    break;
            }
        }
    }
}
