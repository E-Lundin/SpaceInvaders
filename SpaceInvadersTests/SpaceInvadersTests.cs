using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceInvaders;
using Game = SpaceInvaders.Game;

namespace SpaceInvadersTests
{
    [TestClass]
    public class SpaceInvadersTests
    {
        [TestMethod]
        public void ProperEntityMovement()
        {
            // Arrange
            _ = new Game();
            Game.gameSize = new Viewport(0, 0, 800, 480);
            Vector2 position = new Vector2(414, 215);
            Vector2 velocity = new Vector2(-10, 3);
            Vector2 expected = position + velocity;
            Shot bullet = new Shot(position, velocity);

            // Update the position of the entity
            bullet.Update();

            // Assert
            Assert.IsTrue(bullet.Position.X == expected.X);
            Assert.IsTrue(bullet.Position.Y == expected.Y);

        }

        [TestMethod]
        public void RemovesEntityOutOfBounds()
        {
            // Set the entity's position out of bounds
            _ = new Game();
            Game.gameSize = new Viewport(0, 0, 800, 480);
            Vector2 position = new Vector2(900, 500);
            Shot bullet = new Shot(position, position);

            // Check if it is detected as ouf of bounds
            bool shouldRemove = bullet.Collision();

            // Assert
            Assert.IsTrue(shouldRemove);
        }
    }
}
