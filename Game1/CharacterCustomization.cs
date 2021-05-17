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
        private Player Ship = new Player();
        private List<Button> Buttons = new List<Button>();
        private List<Skin> Skins = new List<Skin>();
        private Button SaveButton;
        private MouseState previousMouse;
        private MouseState currentMouse;
        private string pointsReq;
        public CharacterCustomization self;

        public CharacterCustomization()
        {
            self = this;
            // Add buttons
            Buttons.Add(new Button() { Image = Images.PrevBtn, X = 160, Y = 400, ClickAction = PrevChoice });
            Buttons.Add(new Button() { Image = Images.NextBtn, X = 550, Y = 400, ClickAction = NextChoice });
            SaveButton = new Button() { Image = Images.SelectBtn, IsSaveBtn = true, X = 360, Y = 400, ClickAction = SaveSkin };
            Buttons.Add(SaveButton);

            // Add Skins
            pointsReq = "0";
            Skins.Add(new Skin() { Image = Images.Ship, NeededPoints = 0, Name = "default", Selected = true });
            Skins.Add(new Skin() { Image = Images.ShipRed, NeededPoints = 500, Name = "shipred" });
            Skins.Add(new Skin() { Image = Images.Ship1, NeededPoints = 1000, Name = "ship1" });
            Skins.Add(new Skin() { Image = Images.Ship2, NeededPoints = 1500, Name = "ship2" });
        }
        public void Update(KeyboardState keyboardState, MouseState mouseState, GameTime gameTime)
        {
            Ship.Update(keyboardState, mouseState, gameTime);
            previousMouse = currentMouse;
            currentMouse = mouseState;
            var mouseRectangle = new Rectangle(mouseState.X, mouseState.Y, 1, 1);
            foreach(Button button in Buttons)
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

        private bool SufficientPoints(int neededPoints)
        {
            int? highscore = HighScoreScreen.BiggestHighScore;
            if (!highscore.HasValue)
                return false;
            else
                return highscore >= neededPoints;
        }

        private void UpdateButton(int neededPoints)
        {
            pointsReq = neededPoints.ToString();
            if (SufficientPoints(neededPoints))
            {
                SaveButton.Image = Images.SelectBtn;
                SaveButton.IsClickable = true;
            }
            else
            {
                SaveButton.Image = Images.DisabledBtn;
                SaveButton.IsClickable = false;
            }
        }

        private void PrevChoice()
        {
            int selectedIndex = Skins.IndexOf(Skins.First(s => s.Selected));
            Skins[selectedIndex].Selected = false;
            selectedIndex--;
            if (selectedIndex < 0)
                selectedIndex = Skins.Count - 1;

            Skin skin = Skins[selectedIndex];
            skin.Selected = true;
            UpdateButton(skin.NeededPoints);
        }

        private void NextChoice()
        {
            int selectedIndex = Skins.IndexOf(Skins.First(s => s.Selected));
            Skins[selectedIndex].Selected = false;
            selectedIndex++;
            if (selectedIndex >= Skins.Count)
                selectedIndex = 0;
            Skin skin = Skins[selectedIndex];
            skin.Selected = true;
            UpdateButton(skin.NeededPoints);
        }

        private void SaveSkin()
        {
            Skin selectedSkin = Skins.First(s => s.Selected);
            string skinName = selectedSkin.Name;
            EntityManager.self.SetPlayerImage(selectedSkin.Image);
            File.WriteAllText("skin.txt", skinName);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var image = Skins.First(c => c.Selected).Image;
            Ship.image = image;
            Ship.Draw(spriteBatch);

            Vector2 titleSize = Images.MenuFont.MeasureString("CHARACTER CUSTOMIZATION");
            Vector2 pos = new Vector2(Game.gameSize.Width / 2 - titleSize.X / 2, 5);
            spriteBatch.DrawString(Images.MenuFont, "CHARACTER CUSTOMIZATION", pos, Color.Lime);
            spriteBatch.DrawString(Images.MenuFont, String.Format("POINTS TO UNLOCK: {0}", pointsReq), pos + new Vector2(0, 40), Color.White);
            foreach (Button button in Buttons)
            {
                button.Draw(spriteBatch);
            }

        }
    }
}
