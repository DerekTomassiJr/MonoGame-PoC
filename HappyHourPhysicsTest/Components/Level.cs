using HappyHourPhysicsTest.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace HappyHourPhysicsTest.Components
{
    internal class Level
    {
        private string levelName;
        private TextureAtlas backgroundTextures;
        private SpriteTile[] levelTiles;

        public Level(string levelName, string filePath, TextureAtlas backgroundTextures)
        { 
            this.levelName = levelName;
            this.backgroundTextures = backgroundTextures;
            this.levelTiles = DeserializeLevelFile(filePath);
        }

        public void DrawLevel(SpriteBatch spriteBatch)
        {
            foreach (SpriteTile tile in this.levelTiles)
            {
                System.Numerics.Vector2 tilePos = this.backgroundTextures.GetSpritePositionInAtlas(tile.ID);

                int width = this.backgroundTextures.SpriteWidth;
                int height = this.backgroundTextures.SpriteHeight;

                // The source and destination rectangles are used to set the part of the image we want to draw and where we want to draw it.
                Rectangle source = new Rectangle((int)tilePos.X, (int)tilePos.Y, width, height);
                Rectangle destination = new Rectangle((int)tile.Position.X * width, (int)tile.Position.Y * height, width, height);

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
                            spriteTiles.Add(new SpriteTile(new System.Numerics.Vector2(positionX, positionY), spriteID));
                        }
                    }
                }

                positionY++;
            }

            return spriteTiles.ToArray();
        }
    }
}
