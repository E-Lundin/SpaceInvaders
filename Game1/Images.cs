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
        public static SpriteFont Font { get; private set; }

        public static void Load(ContentManager content)
        {
            Ship = content.Load<Texture2D>("ship");
            Shot = content.Load<Texture2D>("bullet");
            Enemy = content.Load<Texture2D>("enemy1");
            Font = content.Load<SpriteFont>("font");
        }
    }
}
