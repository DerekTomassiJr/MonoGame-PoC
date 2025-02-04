using HappyHourPhysicsTest.Components;
using HappyHourPhysicsTest.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace HappyHourPhysicsTest
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private TextureAtlas backgroundTextures;
        private TextureAtlas itemTextures;
        private SpriteObject ballSprite;
        private List<SpriteObject> ballSprites;
        private Level activeLevel;

        private Rectangle testBox;
        private Texture2D testBoxTexture;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 960;
            _graphics.PreferredBackBufferHeight = 640;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D itemTiles = Content.Load<Texture2D>(Constants.ITEM_ASSETS_FILE);
            Texture2D blockTiles = Content.Load<Texture2D>(Constants.BLOCK_ASSETS_FILE);

            backgroundTextures = new TextureAtlas(this, blockTiles, 32, 32);
            itemTextures = new TextureAtlas(this, itemTiles, 32, 32);

            ballSprite = this.itemTextures.GenerateSpriteObjectFromAtlas(7, GraphicsDevice);
            ballSprites = new List<SpriteObject>();
            activeLevel = new Level("Drink Pour", "../../../Content/Level1.csv", this.backgroundTextures);

            testBox = new Rectangle(0, 500, 960, 30);
            testBoxTexture = new Texture2D(GraphicsDevice, 960, 30);
            CreateBorder(testBoxTexture, 5, Color.Red);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                SpriteObject newBall = new SpriteObject(this, ballSprite.spriteTexture, ballSprite.collisionBox); // this needs to be reoptimized
                newBall.isVisible = true;
                ballSprites.Add(newBall); 
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            this.activeLevel.DrawLevel(_spriteBatch);
            this.drawSpriteObjects();
            //_spriteBatch.Draw(this.testBoxTexture, new Vector2(this.testBox.X, this.testBox.Y), Color.White); // Test code to show rectangle bounds
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void drawSpriteObjects() 
        {
            foreach (SpriteObject spriteObject in ballSprites)
            {
                if (!spriteObject.collisionBox.Intersects(this.testBox))
                {
                    spriteObject.position.Y += 5;
                    spriteObject.collisionBox.Y += 5;
                }

                spriteObject.DrawSpriteObject(_spriteBatch);
            }
        }

        private static void CreateBorder(Texture2D texture, int borderWidth, Color borderColor)
        {
            Color[] colors = new Color[texture.Width * texture.Height];

            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    bool colored = false;
                    for (int i = 0; i <= borderWidth; i++)
                    {
                        if (x == i || y == i || x == texture.Width - 1 - i || y == texture.Height - 1 - i)
                        {
                            colors[x + y * texture.Width] = borderColor;
                            colored = true;
                            break;
                        }
                    }

                    if (colored == false)
                        colors[x + y * texture.Width] = Color.Transparent;
                }
            }

            texture.SetData(colors);
        }
    }
}
