using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    static class Images
    {
        public static Texture2D Ship { get; private set; }
        public static Texture2D ShipRed { get; private set; }
        public static Texture2D Ship1 { get; private set; }
        public static Texture2D Ship2 { get; private set; }
        public static Texture2D Shot { get; private set; }
        public static Texture2D Enemy { get; private set; }
        public static Texture2D Enemy2 { get; private set; }
        public static Texture2D ActiveHeart { get; private set; }
        public static Texture2D InactiveHeart { get; private set; }
        public static Texture2D ShipSpeedBoost { get; private set; }
        public static Texture2D CanonSpeedBoost { get; private set; }
        public static Texture2D InvincibleBoost { get; private set; }
        public static Texture2D PrevBtn { get; private set; }
        public static Texture2D NextBtn { get; private set; }
        public static Texture2D SelectBtn { get; private set; }
        public static Texture2D DisabledBtn { get; private set; }
        public static SpriteFont Font { get; private set; }
        public static SpriteFont MenuFont { get; private set; }


        /// <summary>
        /// Loads all assets for the game
        /// </summary>
        /// <param name="content"></param>
        public static void Load(ContentManager content)
        {
            Ship = content.Load<Texture2D>("ship");
            ShipRed = content.Load<Texture2D>("shipred");
            Ship1 = content.Load<Texture2D>("ship1");
            Ship2 = content.Load<Texture2D>("ship2");
            Shot = content.Load<Texture2D>("bullet");
            Enemy = content.Load<Texture2D>("enemy1");
            Enemy2 = content.Load<Texture2D>("enemy2");
            Font = content.Load<SpriteFont>("font");
            MenuFont = content.Load<SpriteFont>("menufont");
            ActiveHeart = content.Load<Texture2D>("heart_active");
            InactiveHeart = content.Load<Texture2D>("heart_inactive");
            ShipSpeedBoost = content.Load<Texture2D>("speedup");
            CanonSpeedBoost = content.Load<Texture2D>("fireball");
            InvincibleBoost = content.Load<Texture2D>("star");
            PrevBtn = content.Load<Texture2D>("leftarrow");
            NextBtn = content.Load<Texture2D>("rightarrow");
            SelectBtn = content.Load<Texture2D>("select");
            DisabledBtn = content.Load<Texture2D>("disabledselect");
        }
    }
}
