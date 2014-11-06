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
        public static bool collisionOn = true;

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

        float moveScale = 1.5f;
        float rotateScale = MathHelper.PiOver2;

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
                glassWallTexture, pebbleWallTexture, metalWallTexture });
            camera = new Camera(GraphicsDevice.Viewport.AspectRatio);
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
            //floorEffect = Content.Load<Effect>("Shader");
            //brickWallEffect = Content.Load<Effect>("Shader");
            //glassWallEffect = Content.Load<Effect>("Shader");
            //metalWallEffect = Content.Load<Effect>("Shader");
            //pebbleWallEffect = Content.Load<Effect>("Shader");

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
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float moveAmount = 0;

            oldKeyState = newKeyState;
            oldPadState = newPadState;
            newKeyState = Keyboard.GetState();
            newPadState = GamePad.GetState(PlayerIndex.One);

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || newKeyState.IsKeyDown(Keys.Escape))
                this.Exit();

            if(newKeyState.IsKeyDown(Keys.Up) || newPadState.ThumbSticks.Left.Y > 0)
            {
                moveAmount = moveScale * elapsed;
            }

            if(newKeyState.IsKeyDown(Keys.Down) || newPadState.ThumbSticks.Left.Y < 0)
            {
                moveAmount = -moveScale * elapsed;
            }

            if(newKeyState.IsKeyDown(Keys.Right) || newPadState.ThumbSticks.Right.X > 0)
            {
                camera.LeftRightRotation = MathHelper.WrapAngle(camera.LeftRightRotation - (rotateScale * elapsed));
            }

            if(newKeyState.IsKeyDown(Keys.Left) || newPadState.ThumbSticks.Right.X < 0)
            {
                camera.LeftRightRotation = MathHelper.WrapAngle(camera.LeftRightRotation + (rotateScale * elapsed));
            }

            if(newKeyState.IsKeyDown(Keys.X) || newPadState.ThumbSticks.Right.Y > 0)
            {
                if(camera.UpDownRotation > -1.5)
                {
                    camera.UpDownRotation = MathHelper.WrapAngle(camera.UpDownRotation - (rotateScale * elapsed));
                }
            }

            if(newKeyState.IsKeyDown(Keys.C) || newPadState.ThumbSticks.Right.Y < 0)
            {
                if(camera.UpDownRotation < 1.5)
                {
                    camera.UpDownRotation = MathHelper.WrapAngle(camera.UpDownRotation + (rotateScale * elapsed));
                }
            }

            if((newKeyState.IsKeyDown(Keys.W) && !oldKeyState.IsKeyDown(Keys.W))
                || (newPadState.Buttons.Y == ButtonState.Pressed && oldPadState.Buttons.Y != ButtonState.Pressed))
            {
                collisionOn = !collisionOn;
            }

            if((newKeyState.IsKeyDown(Keys.LeftShift) && newKeyState.IsKeyDown(Keys.Z) && !oldKeyState.IsKeyDown(Keys.Z))
                || (newPadState.Buttons.A == ButtonState.Pressed && oldPadState.Buttons.A != ButtonState.Pressed))
            {
                if(camera.currentFOVLevel < 2)
                {
                    camera.currentFOVLevel++;
                    camera.UpdateProjection();
                }
            }

            if((!newKeyState.IsKeyDown(Keys.LeftShift) && newKeyState.IsKeyDown(Keys.Z) && !oldKeyState.IsKeyDown(Keys.Z))
                || (newPadState.Buttons.B == ButtonState.Pressed && oldPadState.Buttons.B != ButtonState.Pressed))
            {
                if(camera.currentFOVLevel > 0)
                {
                    camera.currentFOVLevel--;
                    camera.UpdateProjection();
                }
            }

            if(newKeyState.IsKeyDown(Keys.Home) || newPadState.Buttons.Start == ButtonState.Pressed)
            {
                camera.currentFOVLevel = 0;
                camera.Position = camera.startingPosition;
                camera.LeftRightRotation = 0;
                camera.UpDownRotation = 0;
                camera.UpdateProjection();
            }

            if(moveAmount != 0)
            {
                Vector3 newLocation = camera.PreviewMove(moveAmount);
                bool moveOK = true;

                if(newLocation.X < 0 || newLocation.X > Maze.mazeWidth)
                {
                    moveOK = false;
                }
                if(newLocation.Z < 0 || newLocation.Z > Maze.mazeHeight)
                {
                    moveOK = false;
                }

                if(collisionOn)
                {
                    foreach (BoundingBox box in maze.GetBoundsForCell((int)newLocation.X, (int)newLocation.Z))
                    {
                        if (box.Contains(newLocation) == ContainmentType.Contains)
                        {
                            moveOK = false;
                        }
                    }
                }

                if(moveOK)
                {
                    camera.MoveForward(moveAmount);
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                maze.ambientColor = new Vector3(.3f, .3f, .3f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.O))
            {
                maze.ambientColor = Color.White.ToVector3();
            }

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
            //maze.Draw(camera, new Effect[5] { ambient, ambient, ambient, ambient, ambient});
            spriteBatch.Begin();
            if(collisionOn)
            {
                spriteBatch.DrawString(font, "Collision Detection: Yes", new Vector2(0, 0), Color.Black);
            }
            else
            {
                spriteBatch.DrawString(font, "Collision Detection: No", new Vector2(0, 0), Color.Black);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
