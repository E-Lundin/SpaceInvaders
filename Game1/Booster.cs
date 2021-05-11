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
        FasterShip,
        Invincible
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
                    image = Images.CanonSpeedBoost;
                    break;
                case (BoosterType.FasterShip):
                    image = Images.ShipSpeedBoost;
                    break;
                case (BoosterType.Invincible):
                    image = Images.InvincibleBoost;
                    break;
            }
            Position = pos;
        }
    }
}
