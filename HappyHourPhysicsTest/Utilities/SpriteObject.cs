using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyHourPhysicsTest.Utilities
{
    public class SpriteObject
    {
        public Texture2D spriteTexture;
        public Rectangle collisionBox;
        public Vector2 position;
        public bool isVisible;

        public SpriteObject(Texture2D spriteTexture, Rectangle collisionBox) 
        { 
            this.spriteTexture = spriteTexture;
            this.collisionBox = collisionBox;
            this.position = new Vector2(480, 20);
        }

        public void DrawSpriteObject(SpriteBatch spriteBatch) 
        { 
            if (!this.isVisible)
            {
                return;
            }

            spriteBatch.Draw(this.spriteTexture, this.position, Color.White);
        }
    }
}
