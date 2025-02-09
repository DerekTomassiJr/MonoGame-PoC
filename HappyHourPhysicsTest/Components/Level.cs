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
using nkast.Aether.Physics2D.Dynamics;
using System.CodeDom.Compiler;

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
        public World World { get; set; }

        public Level(string levelName, string filePath, TextureAtlas backgroundTextures, GraphicsDevice graphicsDevice, World world)
        { 
            this.levelName = levelName;
            this.backgroundTextures = backgroundTextures;
            this.levelTiles = DeserializeLevelFile(filePath);
            this.levelRects = new List<Rectangle>();
            this.objectSpawnRectangle = CreateObjectSpawnRectangle();
            this.GraphicsDevice = graphicsDevice;
            this.World = world;

            GenerateLevelEdges();
        }

        public void DrawLevel(SpriteBatch spriteBatch)
        {
            // Clear levelRects upon new draw
            this.levelRects.Clear();

            //Drawing spawn rectangle
            Texture2D spawnBoxTexture = new Texture2D(GraphicsDevice, objectSpawnRectangle.Width, objectSpawnRectangle.Height);
            Game1.CreateBorder(spawnBoxTexture, 5, Color.Red);
            spriteBatch.Draw(spawnBoxTexture, new Vector2(objectSpawnRectangle.X, objectSpawnRectangle.Y), Color.White);

            foreach (SpriteTile tile in this.levelTiles)
            {
                Vector2 tilePos = this.backgroundTextures.GetSpritePositionInAtlas(tile.ID);

                int width = this.backgroundTextures.SpriteWidth;
                int height = this.backgroundTextures.SpriteHeight;

                // The source and destination rectangles are used to set the part of the image we want to draw and where we want to draw it.
                Rectangle source = new Rectangle((int)tilePos.X, (int)tilePos.Y, width, height);
                Rectangle destination = new Rectangle((int)tile.Position.X * width, (int)tile.Position.Y * height, width, height);
                this.levelRects.Add(destination);

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
                            Rectangle spriteTileBox = new Rectangle(positionX * this.backgroundTextures.SpriteWidth - 16, positionY * this.backgroundTextures.SpriteHeight - 16, this.backgroundTextures.SpriteWidth, this.backgroundTextures.SpriteHeight);
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
            
            int spawnX = topLeftTile.TileCollisionBox.X + topLeftTile.TileCollisionBox.Width + 16;
            int spawnY = 30;
            int spawnWidth = topRightTile.TileCollisionBox.X - spawnX + 16;
            int spawnHeight = 20;
            
            Rectangle spawnRectangle = new Rectangle(spawnX, spawnY, spawnWidth, spawnHeight);
            return spawnRectangle;
        }

        private void GenerateLevelEdges()
        {
            foreach(SpriteTile tile in levelTiles)
            {
                Rectangle rect = tile.TileCollisionBox;
                
                Body[] edges = new Body[] {
                    World.CreateEdge(new Vector2(rect.Left, rect.Top), new Vector2(rect.Right, rect.Top)),
                    World.CreateEdge(new Vector2(rect.Left, rect.Top), new Vector2(rect.Left, rect.Bottom)),
                    World.CreateEdge(new Vector2(rect.Left, rect.Bottom), new Vector2(rect.Right, rect.Bottom)),
                    World.CreateEdge(new Vector2(rect.Right, rect.Top), new Vector2(rect.Right, rect.Bottom))
                };

                foreach (var edge in edges)
                {
                    edge.BodyType = BodyType.Static;
                }
            }
        }
    }
}
