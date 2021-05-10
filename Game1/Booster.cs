using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{

    //TODO add more BoosterTypes
    public enum BoosterType
    {
        AdditionalHeart = 0,
        FasterCannon,
        ShotSpread
    }
    class Booster : Entity
    {
        public BoosterType type;
        public Booster(Vector2 pos, BoosterType _type)
        {
            type = _type;
            switch (type)
            {
                case (BoosterType.AdditionalHeart):
                    image = Images.ActiveHeart;
                    break;
                case (BoosterType.FasterCannon):
                    //TODO
                    image = Images.ActiveHeart;
                    break;
                case (BoosterType.ShotSpread):
                    //TODO
                    image = Images.ActiveHeart;
                    break;
            }
            Position = pos;
        }

        private bool Collision()
        {
            bool collides = false;

            // Check that enemy is still within border range
            if (!Game.gameSize.Bounds.Contains(Position.ToPoint()))
            {
                collides = true;
            }

            return collides;
        }

        public void handleEnemyCollision(Enemy otherEnemy)
        {
            Vector2 distance = Position - otherEnemy.Position;
            Velocity += 10 * distance / (distance.LengthSquared() + 1);
        }
    }
}
