using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    class CharacterCustomization
    {
        private Player ship = new Player();
        private List<Button> buttons = new List<Button>();
        private List<Skin> skins = new List<Skin>();
        private Button saveButton;
        private MouseState previousMouse;
        private MouseState currentMouse;
        private string pointsReq;
        public CharacterCustomization self;

        public CharacterCustomization()
        {
            self = this;
            // Add buttons
            buttons.Add(new Button() { Image = Images.PrevBtn, X = 160, Y = 400, ClickAction = PrevChoice });
            buttons.Add(new Button() { Image = Images.NextBtn, X = 550, Y = 400, ClickAction = NextChoice });
            saveButton = new Button() { Image = Images.SelectBtn, IsSaveBtn = true, X = 360, Y = 400, ClickAction = SaveSkin };
            buttons.Add(saveButton);

            // Add skins
            pointsReq = "0";
            skins.Add(new Skin() { Image = Images.Ship, NeededPoints = 0, Name = "default", Selected = true });
            skins.Add(new Skin() { Image = Images.ShipRed, NeededPoints = 500, Name = "shipred" });
            skins.Add(new Skin() { Image = Images.Ship1, NeededPoints = 1000, Name = "ship1" });
            skins.Add(new Skin() { Image = Images.Ship2, NeededPoints = 1500, Name = "ship2" });
        }
        public void Update(KeyboardState keyboardState, MouseState mouseState, GameTime gameTime)
        {
            ship.Update(keyboardState, mouseState, gameTime);
            previousMouse = currentMouse;
            currentMouse = mouseState;
            var mouseRectangle = new Rectangle(mouseState.X, mouseState.Y, 1, 1);
            foreach (Button button in buttons)
            {
                Debug.WriteLine(mouseRectangle.Intersects(button.Rectangle) && button.IsClickable);
                if (mouseRectangle.Intersects(button.Rectangle) && button.IsClickable)
                {
                    if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
                    {
                        button.ClickAction.Invoke();
                    }
                }
            }
        }

        /// <summary>
        /// Determiens if the user has a sufficient highscore for the selected skin 
        /// </summary>
        /// <param name="neededPoints">The points needed to unlock the skin</param>

        private bool SufficientPoints(int neededPoints)
        {
            int? highscore = HighScoreScreen.BiggestHighScore;
            if (!highscore.HasValue)
                return false;
            else
                return highscore >= neededPoints;
        }

        /// <summary>
        /// Changes the image of the `Select` button.
        /// </summary>
        /// <param name="neededPoints">The points needed to unlock the skin</param>
        private void UpdateButton(int neededPoints)
        {
            pointsReq = neededPoints.ToString();
            if (SufficientPoints(neededPoints))
            {
                saveButton.Image = Images.SelectBtn;
                saveButton.IsClickable = true;
            }
            else
            {
                saveButton.Image = Images.DisabledBtn;
                saveButton.IsClickable = false;
            }
        }

        /// <summary>
        /// Toggles the previous choice 
        /// </summary>
        private void PrevChoice()
        {
            int selectedIndex = skins.IndexOf(skins.First(s => s.Selected));
            skins[selectedIndex].Selected = false;
            selectedIndex--;
            if (selectedIndex < 0)
                selectedIndex = skins.Count - 1;

            Skin skin = skins[selectedIndex];
            skin.Selected = true;
            UpdateButton(skin.NeededPoints);
        }

        /// <summary>
        /// Toggles the next choice 
        /// </summary>
        private void NextChoice()
        {
            int selectedIndex = skins.IndexOf(skins.First(s => s.Selected));
            skins[selectedIndex].Selected = false;
            selectedIndex++;
            if (selectedIndex >= skins.Count)
                selectedIndex = 0;
            Skin skin = skins[selectedIndex];
            skin.Selected = true;
            UpdateButton(skin.NeededPoints);
        }

        /// <summary>
        /// Saves the selected skin to `skin.txt` 
        /// </summary>
        private void SaveSkin()
        {
            Skin selectedSkin = skins.First(s => s.Selected);
            string skinName = selectedSkin.Name;
            EntityManager.self.SetPlayerImage(selectedSkin.Image);
            File.WriteAllText("skin.txt", skinName);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float titleSize = Images.MenuFont.MeasureString("CHARACTER CUSTOMIZATION").X;
            float goBackSize = Images.Font.MeasureString("Press Enter to go back").Y;
            Vector2 pos = new Vector2(Game.gameSize.Width / 2 - titleSize / 2, 5);
            Vector2 goBackPos = new Vector2(5, Game.gameSize.Height - goBackSize);
            var image = skins.First(c => c.Selected).Image;
            ship.image = image;

            ship.Draw(spriteBatch);
            spriteBatch.DrawString(Images.MenuFont, "CHARACTER CUSTOMIZATION", pos, Color.Lime);
            spriteBatch.DrawString(Images.MenuFont, String.Format("POINTS TO UNLOCK: {0}", pointsReq), pos + new Vector2(0, 40), Color.White);
            spriteBatch.DrawString(Images.Font, "Press Enter to go back", goBackPos, Color.White);
            foreach (Button button in buttons)
            {
                button.Draw(spriteBatch);
            }

        }
    }
}
