namespace HappyHourPhysicsTest.Utilities
{
    using Microsoft.Xna.Framework;
    using Vector2 = System.Numerics.Vector2;

    /// <summary>
    /// This class represents a sprite that may be gound in a tile set.
    /// </summary>
    public class SpriteTile
    {
        /// <summary>
        /// Gets or sets the position of the sprite tile.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the ID of the sprite tile.
        /// </summary>
        public int ID { get; set; }

        public Rectangle TileCollisionBox { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteTile"/> class.
        /// </summary>
        /// <param name="position">The position of the SpriteTile.</param>
        /// <param name="id">The unique ID of the sprite from a Texture Atlas.</param>
        public SpriteTile(Vector2 position, int id, Rectangle collisionBox)
        {
            Position = position;
            ID = id;
            TileCollisionBox = collisionBox;
        }
    }
}
