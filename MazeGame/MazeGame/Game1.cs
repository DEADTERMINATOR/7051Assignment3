using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MazeGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static bool clippingOn = true;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Camera camera;
        Maze maze;

        BasicEffect floorEffect;
        BasicEffect brickWallEffect;
        BasicEffect glassWallEffect;
        BasicEffect metalWallEffect;
        BasicEffect pebbleWallEffect;

        Texture2D floorTexture;
        Texture2D brickWallTexture;
        Texture2D glassWallTexture;
        Texture2D metalWallTexture;
        Texture2D pebbleWallTexture;

        KeyboardState oldKeyState, newKeyState;
        GamePadState oldPadState, newPadState;
        SpriteFont font;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            LoadContent();
            maze = new Maze(GraphicsDevice, new Texture2D[5] { floorTexture, brickWallTexture, 
                glassWallTexture, metalWallTexture, pebbleWallTexture });
            camera = new Camera(GraphicsDevice, maze);
            floorEffect = new BasicEffect(GraphicsDevice);
            brickWallEffect = new BasicEffect(GraphicsDevice);
            glassWallEffect = new BasicEffect(GraphicsDevice);
            metalWallEffect = new BasicEffect(GraphicsDevice);
            pebbleWallEffect = new BasicEffect(GraphicsDevice);
            oldKeyState = Keyboard.GetState();
            newKeyState = Keyboard.GetState();
            oldPadState = GamePad.GetState(PlayerIndex.One);
            newPadState = GamePad.GetState(PlayerIndex.One);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            font = Content.Load<SpriteFont>("SpriteFont1");
            floorTexture = Content.Load<Texture2D>("stampedconcrete");
            brickWallTexture = Content.Load<Texture2D>("brickwall");
            glassWallTexture = Content.Load<Texture2D>("hammeredglasswall");
            metalWallTexture = Content.Load<Texture2D>("metalwall");
            pebbleWallTexture = Content.Load<Texture2D>("pebblewall");

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            oldKeyState = newKeyState;
            oldPadState = newPadState;
            newKeyState = Keyboard.GetState();
            newPadState = GamePad.GetState(PlayerIndex.One);

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || newKeyState.IsKeyDown(Keys.Escape))
                this.Exit();

            if((newKeyState.IsKeyDown(Keys.W) && !oldKeyState.IsKeyDown(Keys.W))
                || (newPadState.Buttons.Y == ButtonState.Pressed && oldPadState.Buttons.Y != ButtonState.Pressed))
            {
                if(clippingOn)
                {
                    clippingOn = false;
                }
                else
                {
                    clippingOn = true;
                }
            }

            camera.UpdatePlayerPosition();
            camera.UpdateCamera();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            maze.Draw(camera, new BasicEffect[5] { floorEffect, brickWallEffect, glassWallEffect, metalWallEffect, pebbleWallEffect });

            spriteBatch.Begin();
            spriteBatch.DrawString(font, camera.playerPosition.X + " " + camera.playerPosition.Z, new Vector2(0, 0), Color.Black);
            spriteBatch.DrawString(font, "\nClipping On: " + clippingOn.ToString(), new Vector2(0, 0), Color.Black);
            spriteBatch.DrawString(font, "\n\n" + camera.playerPitch, new Vector2(0, 0), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
