using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    class Button
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Rectangle Rectangle
        {
            get { return new Rectangle(X, Y, 85, 40); }
        }
        public Texture2D Image { get; set; }
        public bool IsSaveBtn { get; set; }
        public bool IsClickable { get; set; } = true;
        public Action ClickAction { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Rectangle, Color.White);
        }
    }
}
