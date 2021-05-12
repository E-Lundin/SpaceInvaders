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
        // https://csharpskolan.se/article/monogame-meny/
        public List<MenuChoice> MenuChoices = new List<MenuChoice>();
        public string currentSelected = "NONE";


        public Menu(int gameWidth, int gameHeight)
        {
            MenuChoices.Add(new MenuChoice() { Text = "START", Selected = true });
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

        public void Update(KeyboardState keyboardState, GameTime gameTime)
        {
            if (keyboardState.IsKeyDown(Keys.Down))
                NextMenuChoice();
            if (keyboardState.IsKeyDown(Keys.Up))
                PreviousMenuChoice();
            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                var selectedChoice = MenuChoices.First(c => c.Selected);
                currentSelected = selectedChoice.Text;
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
                        spriteBatch.DrawString(Images.Font, choice.Text, choice.Position, Color.White);
                    }
                    break;
                case "START":
                    Game.self.Reset();
                    Game.currentGameState = GameState.Playing;
                    currentSelected = "NONE";
                    break;

                case "QUIT":
                    Game.self.Exit();
                    break;
            }
        }
    }
}
