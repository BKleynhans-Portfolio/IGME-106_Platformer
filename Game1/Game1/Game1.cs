﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Game1 - Platformer for Learning
/// Class Description   : Main Game1 class
/// Author              : Benjamin Kleynhans
/// Modified By         : Benjamin Kleynhans
/// Date                : March 13, 2018
/// Filename            : Game1.cs
/// </summary>

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        // Define graphics devices
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D playerSprites;                                                            // Create variable to contain player spritesheet
        Texture2D enemySprites;
        Texture2D platformSprites;

        // Define input devices
        public static KeyboardState currentKeyboardState;
        public static KeyboardState previousKeyboardState;

        // Define gametime parameter for gravity implementation
        public static GameTime oldGameTime = new GameTime();

        // List of all GameObjects
        List<GameObject> gameObject = new List<GameObject>();

        // Create player object and parameters
        Player player;

        // Create enemy object and parameters
        List<Enemy> enemy = new List<Enemy>();

        // Create platform object and parameters
        List<Platform> platform = new List<Platform>();

        // List of objects currently being intersected
        public List<GameObject> intersectedBy = new List<GameObject>();

        // Animation variables        
        public static double fps;
        public static double secondsPerFrame;
        public static double timeCounter;

        // Window properties
        public const int screenWidth = 1600;
        public const int screenHeight = 900;

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
            graphics.PreferredBackBufferWidth = screenWidth;                                // Set desired width of window
            graphics.PreferredBackBufferHeight = screenHeight;                              // Set desired height of window            
            graphics.ApplyChanges();

            Window.Position = new Point(                                        // Center the game view on the screen
                (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) -
                    (graphics.PreferredBackBufferWidth / 2),
                (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2) -
                    (graphics.PreferredBackBufferHeight / 2)
            );

            this.IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {                                                                                            
            spriteBatch = new SpriteBatch(GraphicsDevice);                                  // Create a new SpriteBatch, which can be used to draw textures.
                        
            playerSprites = Content.Load<Texture2D>("TestImage");                           // Load player sprite
            player = new Player(playerSprites, 550, 50, 50, 50);
            player.ApplyGravity = true;
            //player.gravityDirection = GravityDirection.Right;
                        
            platformSprites = Content.Load<Texture2D>("TestPlatform");                      // Load a platform
            platform.Add(new Platform(platformSprites, 0, 200, 500, 50));
            platform.Add(new Platform(platformSprites, 400, 400, 500, 50));
            platform.Add(new Platform(platformSprites, 0, 600, 500, 50));

            //platform[0].ApplyGravity = true;
            //platform[0].gravityDirection = GravityDirection.Right;
            //platform[0].gravityOnProximityFrom = GravityOnProximityFrom.Top;

            platform[1].ApplyGravity = false;
            //platform[1].gravityDirection = GravityDirection.Down;
            platform[1].gravityDirection = GravityDirection.Right;
            platform[1].gravityOnProximityFrom = GravityOnProximityFrom.Top;
            platform[1].platformMovement = PlatformMovement.ToAndFroRightFirst;
            //platform[1].platformMovement = PlatformMovement.ToAndFroLeftFirst;
            //platform[1].platformMovement = PlatformMovement.ToAndFroDownFirst;

            //platform[2].ApplyGravity = true;
            //platform[2].gravityDirection = GravityDirection.Right;
            //platform[2].gravityOnProximityFrom = GravityOnProximityFrom.Top;

            enemy.Add(new Enemy(playerSprites, 100, 50, 50, 50));                           // Load enemy sprite
            enemy[0].ApplyGravity = true;

            gameObject.Add(player);
            gameObject.Add(enemy[0]);
            gameObject.Add(platform[0]);
            gameObject.Add(platform[1]);
            gameObject.Add(platform[2]);

            fps = 10.0f;                                                                    // Set up animation variables;
            secondsPerFrame = 1.0f / fps;
            timeCounter = 0;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            for (int i = 0; i < (gameObject.Count - 1); i++)
            {
                for (int j = i + 1; j < gameObject.Count; j++)
                {
                    if (gameObject[i].Intersects(gameObject[j]))
                    {
                        if (!gameObject[i].intersectedBy.Contains(gameObject[j]))
                        {
                            gameObject[i].intersectedBy.Add(gameObject[j]);
                        }

                        if (!gameObject[j].intersectedBy.Contains(gameObject[i]))
                        {
                            gameObject[j].intersectedBy.Add(gameObject[i]);
                        }
                    }
                }
            }

            for (int i = 0; i < gameObject.Count; i++)
            {                
                gameObject[i].Update(gameTime);
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

            spriteBatch.Begin();

            for (int i = 0; i < gameObject.Count; i++)
            {
                gameObject[i].Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
