using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    class Enemy
    {
        private int borderWidth;
        private int borderHeight;
        public Rectangle ship = new Rectangle(100, 100, 20, 20);
        public List<Shot> Shots = new List<Shot>();
        private TimeSpan lastPlayerShot;
        private static readonly TimeSpan ShootInterval = TimeSpan.FromMilliseconds(150);

        public Enemy(int gameWidth, int gameHeight)
        {
            borderWidth = gameWidth;
            borderHeight = gameHeight;
        }
    }
}
