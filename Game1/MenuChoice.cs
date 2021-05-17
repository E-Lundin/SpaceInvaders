using System;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    class MenuChoice
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Vector2 Position
        {
            get { return new Vector2(X, Y); }
        }

        public string Text { get; set; }
        public bool Selected { get; set; }

        public MenuState State { get; set; }


    }
}
