using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

/// <summary>
/// IGME-106 - Game Development and Algorithmic Problem Solving
/// Group Project
/// Class Description   : 
/// Created By          : Benjamin Kleynhans
/// Creation Date       : March 22, 2018
/// Authors             : Benjamin Kleynhans
///                       
///                       
///                       
/// Last Modified By    : Benjamin Kleynhans
/// Last Modified Date  : March 22, 2018
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
        List<Enemy> enemies = new List<Enemy>();

        // Create platform object and parameters
        List<Platform> platforms = new List<Platform>();

        // List of objects currently being intersected
        public List<GameObject> intersectedBy = new List<GameObject>();

        // Animation variables        
        public static double fps;
        public static double secondsPerFrame;
        public static double timeCounter;

        // Window properties
        public const int SCREENWIDTH = 1600;
        public const int SCREENHEIGHT = 900;

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
            graphics.PreferredBackBufferWidth = SCREENWIDTH;                                // Set desired width of window
            graphics.PreferredBackBufferHeight = SCREENHEIGHT;                              // Set desired height of window            
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
            platformSprites = Content.Load<Texture2D>("Platform");                      // Load a platform

            player = new Player(playerSprites, 50, 50, 50, 50);
            player.ApplyGravity = true;

            LoadFloors();
            LoadEnemies();

            gameObject.Add(player);                                                         // All objects need to be added to the gameObject list

            foreach (Enemy enemy in enemies)
            {
                gameObject.Add(enemy);
            }

            foreach (Platform platform in platforms)
            {
                gameObject.Add(platform);
            }

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

            previousKeyboardState = currentKeyboardState;                                   // Get comparitive keyboard states
            currentKeyboardState = Keyboard.GetState();

            for (int i = 0; i < (gameObject.Count - 1); i++)                                // Cycle through gameObject list and test
            {                                                                               // all created objects for intersection
                for (int j = i + 1; j < gameObject.Count; j++)
                {
                    if (gameObject[i].Intersects(gameObject[j]))
                    {
                        if (!gameObject[i].intersectedBy.Contains(gameObject[j]))           // If there is an intersection, create
                        {                                                                   // references in the objects to each other
                            gameObject[i].intersectedBy.Add(gameObject[j]);
                        }

                        if (!gameObject[j].intersectedBy.Contains(gameObject[i]))
                        {
                            gameObject[j].intersectedBy.Add(gameObject[i]);
                        }
                    }
                }
            }

            for (int i = 0; i < gameObject.Count; i++)                                      // Update all the objects in the game
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

            for (int i = 0; i < gameObject.Count; i++)                                      // Draw all the objects in the game
            {
                gameObject[i].Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void LoadFloors()
        {
            platforms.Add(new Platform(platformSprites, 0, -20, SCREENWIDTH, 50));          // Ceiling platform
            //platform[0].ApplyGravity = true;                                              // MOST OF THESE ARE FOR TESTING PURPOSES
            //platform[0].gravityDirection = GravityDirection.Right;                        // YOU CAN UNCOMMENT TO SEE WHAT HAPPENS
            //platform[0].gravityOnProximityFrom = GravityOnProximityFrom.Top;

            platforms.Add(new Platform(platformSprites, 0, 100, 100, 100));
            //platforms[1].ApplyGravity = false;                                               // State that the second platform should not have gravity
            //platform[1].gravityDirection = GravityDirection.Down;                         // during instantiation because gravity will be implemented
            //platforms[1].gravityDirection = GravityDirection.Right;                          // to the right if the object receives a proximity warning
            //platforms[1].gravityOnProximityFrom = GravityOnProximityFrom.Top;                // from above and then moves back and forth
            //platforms[1].platformMovement = PlatformMovement.ToAndFroRightFirst;
            //platform[1].platformMovement = PlatformMovement.ToAndFroLeftFirst;
            //platform[1].platformMovement = PlatformMovement.ToAndFroDownFirst;

            platforms.Add(new Platform(platformSprites, 100, 200, 200, 50));
            
            platforms.Add(new Platform(platformSprites, 400, 200, 100, 50));
            platforms[3].ApplyGravity = true;
            platforms[3].platformMovement = PlatformMovement.ToAndFroUpFirst;
            platforms[3].ObjectYMoveDistance = 50;

            platforms.Add(new Platform(platformSprites, 500, 100, 200, 50));
            platforms[4].ApplyGravity = false;
            platforms[4].gravityOnProximityFrom = GravityOnProximityFrom.Top;
            platforms[4].platformMovement = PlatformMovement.ToAndFroRightFirst;            
            platforms[4].ObjectXMoveDistance = 400;
            
        }

        private void LoadEnemies()
        {
            enemies.Add(new Enemy(playerSprites, 100, 50, 50, 50));                           // Load enemy sprite
            enemies[0].ApplyGravity = true;
        }
    }
}
