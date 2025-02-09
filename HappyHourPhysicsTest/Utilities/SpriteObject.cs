using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Dynamics;
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
        public int radius;
        public int density;
        public Vector2 velocity;
        public bool isVisible;

        public World world;
        public Body objectBody;

        public SpriteObject(Game game, Texture2D spriteTexture, Rectangle collisionBox) : base(game)
        {
            this.spriteTexture = spriteTexture;
            this.collisionBox = collisionBox;
            this.spawnLocation = new Rectangle(0, 0, 0, 0);
            this.position = new Vector2(480, 20);
            this.objectBody = null;
        }

        public SpriteObject(Game game, Texture2D spriteTexture, Rectangle collisionBox, Rectangle spawnLocation, World world) : base(game)
        { 
            this.spriteTexture = spriteTexture;
            this.collisionBox = collisionBox;
            this.spawnLocation = spawnLocation;
            this.position = GenerateObjectSpawnPosition();
            this.radius = 4;
            this.density = 1;
            this.velocity = new Vector2(0, 500);
            this.world = world;
            this.objectBody = GenerateObjectBody();
        }

        public void DrawSpriteObject(SpriteBatch spriteBatch) 
        { 
            if (!this.isVisible)
            {
                return;
            }

            spriteBatch.Draw(this.spriteTexture, this.objectBody.Position, Color.White);
        }

        private Vector2 GenerateObjectSpawnPosition() 
        { 
            Random random = new Random();
            int spawnX = random.Next(spawnLocation.X, spawnLocation.X + spawnLocation.Width - 32);
            int spawnY = random.Next(spawnLocation.Y, spawnLocation.Y + spawnLocation.Height);

            return new Vector2(spawnX, spawnY);
        }

        private Body GenerateObjectBody() 
        {
            var body = world.CreateCircle(radius, density, position, BodyType.Dynamic);
            body.LinearVelocity = velocity;

            return body;
        }
    }
}
