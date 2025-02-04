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
        public Vector2 position;
        public bool isVisible;

        public SpriteObject(Game game, Texture2D spriteTexture, Rectangle collisionBox) : base(game)
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

        public override void Update(GameTime gameTime) 
        {
            if (!this.collisionBox.Intersects(new Rectangle(0,640,960,640)))
            {
                this.position.Y += 5;
            }
            
            base.Update(gameTime);
        }
    }
}
