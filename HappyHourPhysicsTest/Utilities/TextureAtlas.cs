namespace HappyHourPhysicsTest.Utilities
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Vector2 = System.Numerics.Vector2;

    /// <summary>
    /// A data class to store and manage multiple sprites from a single texture.
    /// </summary>
    public class TextureAtlas
    {
        public Game game;
        
        /// <summary>
        /// Gets the texture that we are using as an Atlas to pull sprites from.
        /// </summary>
        public Texture2D Texture { get; private set; }

        /// <summary>
        /// Gets the number of rows of sprites that make up the atlas.
        /// </summary>
        public int Rows
        {
            get
            {
                return Texture.Height / SpriteHeight;
            }
        }

        /// <summary>
        /// Gets the number of columns of sprites that make up the atlas.
        /// </summary>
        public int Columns
        {
            get
            {
                return Texture.Width / SpriteWidth;
            }
        }

        /// <summary>
        /// Gets or sets the width of each sprite on the atlas.
        /// </summary>
        public ushort SpriteWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of each sprite on the atlas.
        /// </summary>
        public ushort SpriteHeight { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureAtlas"/> class.
        /// </summary>
        /// <param name="content">The content manager that we will use to load the texture in.</param>
        /// <param name="texturePath">The path to the texture that we plan to load in.</param>
        /// <param name="spriteWidth">The width of the sprites that are found within the atlas.</param>
        /// <param name="spriteHeight">The height of the sprites that are found within the atlas.</param>
        public TextureAtlas(ContentManager content, string texturePath, ushort spriteWidth, ushort spriteHeight)
        {
            ArgumentNullException.ThrowIfNull(content);

            if (string.IsNullOrWhiteSpace(texturePath))
            {
                throw new ArgumentException("The path provided cannot be null or empty.");
            }

            if (spriteWidth == 0 || spriteHeight == 0)
            {
                throw new ArgumentException("The spriteWidth and spriteHeight arguments must be a valid integer greater than 0.");
            }

            
            Texture = content.Load<Texture2D>(texturePath);
            SpriteWidth = spriteWidth;
            SpriteHeight = spriteHeight;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureAtlas"/> class.
        /// </summary>
        /// <param name="texture">The texture that was loaded in.</param>
        /// <param name="spriteWidth">The width of the sprites that are found within the atlas.</param>
        /// <param name="spriteHeight">The height of the sprites that are found within the atlas.</param>
        public TextureAtlas(Game game, Texture2D texture, ushort spriteWidth, ushort spriteHeight)
        {
            ArgumentNullException.ThrowIfNull(texture);

            if (spriteWidth == 0 || spriteHeight == 0)
            {
                throw new ArgumentException("The spriteWidth and spriteHeight arguments must be a valid integer greater than 0.");
            }

            this.game = game;
            Texture = texture;
            SpriteWidth = spriteWidth;
            SpriteHeight = spriteHeight;
        }

        /// <summary>
        /// This method will get a sprite position from the texture atlas via an ID that belongs to the sprite.
        /// </summary>
        /// <param name="id">The ID of the sprite that is within the atlas.</param>
        /// <returns>The position of the sprite within the Texture Atlas.</returns>
        public Vector2 GetSpritePositionInAtlas(int id)
        {
            int spriteColumn = id % Columns;
            int spriteRow = id / Columns;
            int spriteXPosition = spriteColumn * SpriteWidth;
            int spriteYPosition = spriteRow * SpriteHeight;

            return new Vector2(spriteXPosition, spriteYPosition);
        }

        /// <summary>
        /// This method will get a sprite position from the texture atlas via the column and row that it resides in.
        /// </summary>
        /// <param name="column">The column that the sprite is located in.</param>
        /// <param name="row">The row that the sprite is located in.</param>
        /// <returns>The position of the sprite within the Texture Atlas.</returns>
        public Vector2 GetSpritePositionInAtlas(int column, int row)
        {
            int spriteXPosition = column * SpriteWidth;
            int spriteYPosition = row * SpriteHeight;

            return new Vector2(spriteXPosition, spriteYPosition);
        }

        public SpriteObject GenerateSpriteObjectFromAtlas(int id, GraphicsDevice graphicsDevice)
        {
            Vector2 spritePosition = GetSpritePositionInAtlas(id);
            Rectangle spriteBox = new Rectangle((int)spritePosition.X, (int)spritePosition.Y, 32, 32);

            Color[] data = new Color[spriteBox.Width * spriteBox.Height];
            this.Texture.GetData(0, spriteBox, data, 0, data.Length);

            Texture2D spriteTexture = new Texture2D(graphicsDevice, spriteBox.Width, spriteBox.Height);
            spriteTexture.SetData(data);

            return new SpriteObject(this.game, spriteTexture, spriteBox);
        }
    }
}
