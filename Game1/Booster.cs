using Microsoft.Xna.Framework;

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
        private readonly BoosterType _type;
        public BoosterType Type
        {
            get { return _type; }
        }

        public Booster(Vector2 pos, BoosterType type)
        {
            _type = type;
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
