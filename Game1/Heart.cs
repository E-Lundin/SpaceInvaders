using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    class Heart
    {
        private bool isConsumed;
        private int identifier;

        public Heart(int idx)
        {
            identifier = idx;
            isConsumed = false;
            
        }
        public bool IsConsumed
        {
            get { return isConsumed; }
            set { isConsumed = value; }
        }

        public int Identifier
        {
            get { return identifier; }
        }

        public Vector2 Position
        {
            get {
                int x = 10 + (32 * identifier);
                int y = 10;
                return new Vector2(x, y); 
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!isConsumed)
            {
                spriteBatch.Draw(Images.ActiveHeart, Position, Color.White);
            }
            else
            {
                spriteBatch.Draw(Images.InactiveHeart, Position, Color.White);
            }

        }
    }
}
