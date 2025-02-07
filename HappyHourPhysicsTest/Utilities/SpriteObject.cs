using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyHourPhysicsTest.Utilities
{
    public class SpriteObject : GameComponent
    {
        public Texture2D spriteTexture;
        public Rectangle collisionBox;
        public Rectangle spawnLocation;
        public Vector2 position;
        public bool isVisible;

        public SpriteObject(Game game, Texture2D spriteTexture, Rectangle collisionBox) : base(game)
        {
            this.spriteTexture = spriteTexture;
            this.collisionBox = collisionBox;
            this.spawnLocation = new Rectangle(0, 0, 0, 0);
            this.position = new Vector2(480, 20);
        }

        public SpriteObject(Game game, Texture2D spriteTexture, Rectangle collisionBox, Rectangle spawnLocation) : base(game)
        { 
            this.spriteTexture = spriteTexture;
            this.collisionBox = collisionBox;
            this.spawnLocation = spawnLocation;
            this.position = GenerateObjectSpawnPosition();
        }

        public void DrawSpriteObject(SpriteBatch spriteBatch) 
        { 
            if (!this.isVisible)
            {
                return;
            }

            spriteBatch.Draw(this.spriteTexture, this.position, Color.White);
        }

        private Vector2 GenerateObjectSpawnPosition() 
        { 
            Random random = new Random();
            int spawnX = random.Next(spawnLocation.X, spawnLocation.X + spawnLocation.Width - 32);
            int spawnY = random.Next(spawnLocation.Y, spawnLocation.Y + spawnLocation.Height);

            return new Vector2(spawnX, spawnY);
        }
    }
}
