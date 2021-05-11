using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    static class Images
    {
        public static Texture2D Ship { get; private set; }
        public static Texture2D Shot { get; private set; }
        public static Texture2D Enemy { get; private set; }
        public static Texture2D ActiveHeart { get; private set; }
        public static Texture2D InactiveHeart { get; private set; }
        public static Texture2D ShipSpeedBoost { get; private set; }
        public static Texture2D CanonSpeedBoost { get; private set; }
        public static Texture2D InvincibleBoost { get; private set; }
        public static SpriteFont Font { get; private set; }

        public static void Load(ContentManager content)
        {
            Ship = content.Load<Texture2D>("ship");
            Shot = content.Load<Texture2D>("bullet");
            Enemy = content.Load<Texture2D>("enemy1");
            Font = content.Load<SpriteFont>("font");
            ActiveHeart = content.Load<Texture2D>("heart_active");
            InactiveHeart = content.Load<Texture2D>("heart_inactive");
            ShipSpeedBoost = content.Load<Texture2D>("speedup");
            CanonSpeedBoost = content.Load<Texture2D>("fireball");
            InvincibleBoost = content.Load<Texture2D>("star");
        }
    }
}
