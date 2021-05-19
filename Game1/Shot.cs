using System;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    class Shot : Entity
    {
        public Shot(Vector2 pos, Vector2 velocity)
        {
            Velocity = velocity;
            Position = pos;
            image = Images.Shot;
            Orientation = (float)Math.Atan2(velocity.Y, velocity.X);
            Radius = 8;
        }

        /// <summary>
        /// Determines if the bullet has traveled outside
        /// of the game's Viewport.
        /// </summary>
        public bool Collision()
        {
            bool collides = false;
            if (!Game.gameSize.Bounds.Contains(Position.ToPoint()))
            {
                collides = true;
            }
            return collides;
        }

        public void Update()
        {
            if (!Collision())
            {
                if (Velocity.LengthSquared() > 0)
                    Orientation = (float)Math.Atan2(Velocity.Y, Velocity.X);
                Position += Velocity;
            }
            else
            {
                shouldRemove = true;
            }

        }
    }
}
