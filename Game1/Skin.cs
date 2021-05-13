using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    class Skin
    {
        public Texture2D Image { get; set; }
        public string Name { get; set; }
        public int NeededPoints { get; set; }
        public bool Selected { get; set; }
    }
}
