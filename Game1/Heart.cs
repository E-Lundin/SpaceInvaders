using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    class Heart
    {
        private bool _isConsumed;
        private readonly int _identifier;

        public Heart(int identifier)
        {
            _identifier = identifier;
            _isConsumed = false;
        }
        public bool IsConsumed
        {
            get { return _isConsumed; }
            set { _isConsumed = value; }
        }

        public int Identifier
        {
            get { return _identifier; }
        }

        public Vector2 Position
        {
            get
            {
                int x = 10 + (32 * _identifier);
                int y = 10;
                return new Vector2(x, y);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!_isConsumed)
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
