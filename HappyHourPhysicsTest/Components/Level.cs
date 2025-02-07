using HappyHourPhysicsTest.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Vector2 = System.Numerics.Vector2;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace HappyHourPhysicsTest.Components
{
    internal class Level
    {
        public string levelName { get; set; }
        private TextureAtlas backgroundTextures;
        public SpriteTile[] levelTiles {  get; set; }
        public List<Rectangle> levelRects { get; set; }
        public Rectangle objectSpawnRectangle { get; set; }
        private GraphicsDevice GraphicsDevice;

        public Level(string levelName, string filePath, TextureAtlas backgroundTextures, GraphicsDevice graphicsDevice)
        { 
            this.levelName = levelName;
            this.backgroundTextures = backgroundTextures;
            this.levelTiles = DeserializeLevelFile(filePath);
            this.levelRects = new List<Rectangle>();
            this.objectSpawnRectangle = CreateObjectSpawnRectangle();
            this.GraphicsDevice = graphicsDevice;
        }

        public void DrawLevel(SpriteBatch spriteBatch)
        {
            //Drawing spawn rectangle
            Texture2D spawnBoxTexture = new Texture2D(GraphicsDevice, objectSpawnRectangle.Width, objectSpawnRectangle.Height);
            Game1.CreateBorder(spawnBoxTexture, 5, Color.Red);
            spriteBatch.Draw(spawnBoxTexture, new Vector2(objectSpawnRectangle.X, objectSpawnRectangle.Y), Color.White);

            foreach (SpriteTile tile in this.levelTiles)
            {
                System.Numerics.Vector2 tilePos = this.backgroundTextures.GetSpritePositionInAtlas(tile.ID);

                int width = this.backgroundTextures.SpriteWidth;
                int height = this.backgroundTextures.SpriteHeight;

                // The source and destination rectangles are used to set the part of the image we want to draw and where we want to draw it.
                Rectangle source = new Rectangle((int)tilePos.X, (int)tilePos.Y, width, height);
                Rectangle destination = new Rectangle((int)tile.Position.X * width, (int)tile.Position.Y * height, width, height);
                this.levelRects.Add(destination);

                Texture2D spriteTileBoxTexture = new Texture2D(GraphicsDevice, tile.TileCollisionBox.Width, tile.TileCollisionBox.Height);
                Game1.CreateBorder(spriteTileBoxTexture, 5, Color.Red);
                spriteBatch.Draw(spriteTileBoxTexture, new Vector2(tile.TileCollisionBox.X, tile.TileCollisionBox.Y), Color.White);

                spriteBatch.Draw(this.backgroundTextures.Texture, destination, source, Color.White);
            }
        }

        private SpriteTile[] DeserializeLevelFile(string filePath)
        {
            int minimumSpriteID = 0;

            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("The path provided cannot be null or whitespace.");
            }

            //if (Uri.IsWellFormedUriString(filePath, UriKind.RelativeOrAbsolute))
            //{
            //    throw new ArgumentException("The path provided is invalid. Check the path provided and try again.");
            //}

            List<SpriteTile> spriteTiles = new List<SpriteTile>();

            StreamReader reader = new StreamReader(filePath);

            int positionY = 0;

            string line;

            // We iterate over each line
            while ((line = reader.ReadLine()) != null)
            {
                string[] sprites = line.Split(',');

                for (int positionX = 0; positionX < sprites.Length; positionX++)
                {
                    if (int.TryParse(sprites[positionX], out int spriteID))
                    {
                        if (spriteID >= minimumSpriteID)
                        {
                            Rectangle spriteTileBox = new Rectangle(positionX * this.backgroundTextures.SpriteWidth, positionY * this.backgroundTextures.SpriteHeight, this.backgroundTextures.SpriteWidth, this.backgroundTextures.SpriteHeight * 3);
                            spriteTiles.Add(new SpriteTile(new System.Numerics.Vector2(positionX, positionY), spriteID, spriteTileBox));
                        }
                    }
                }

                positionY++;
            }

            return spriteTiles.ToArray();
        }

        private Rectangle CreateObjectSpawnRectangle()
        {
            var topLeftTile = levelTiles.GetValue(0) as SpriteTile;
            var topRightTile = levelTiles.GetValue(1) as SpriteTile;
            
            int spawnX = topLeftTile.TileCollisionBox.X + topLeftTile.TileCollisionBox.Width;
            int spawnY = 30;
            int spawnWidth = topRightTile.TileCollisionBox.X - spawnX;
            int spawnHeight = 20;
            
            Rectangle spawnRectangle = new Rectangle(spawnX, spawnY, spawnWidth, spawnHeight);
            return spawnRectangle;
        }
    }
}
