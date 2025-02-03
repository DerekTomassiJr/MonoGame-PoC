﻿using HappyHourPhysicsTest.Components;
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
        private Level activeLevel;


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

            backgroundTextures = new TextureAtlas(blockTiles, 32, 32);
            itemTextures = new TextureAtlas(itemTiles, 32, 32);

            activeLevel = new Level("Drink Pour", "../../../Content/Level1.csv", this.backgroundTextures);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            this.activeLevel.DrawLevel(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
