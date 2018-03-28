using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;

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
    public enum GameState
    {
        Title,
        Options,
        InGame,
        GameOver,
        Pause
    }
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        protected static GameState gameState = GameState.Title;

        // Define graphics devices
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Define input devices
        public static KeyboardState currentKeyboardState;
        public static KeyboardState previousKeyboardState;

        // Dictionary of player sprites
        public static Dictionary<string, Texture2D> playerElements = new Dictionary<string, Texture2D>();

        // Dictionary of enemy sprites
        public static Dictionary<string, Texture2D> enemyElements = new Dictionary<string, Texture2D>();

        // Dictionary of platform sprites
        public static Dictionary<string, Texture2D> platformElements = new Dictionary<string, Texture2D>();

        // Dictionary of general menu sprites
        public static Dictionary<string, Texture2D> menuElements = new Dictionary<string, Texture2D>();

        // Dictionary of general sprites
        public static Dictionary<string, Texture2D> graphicElements = new Dictionary<string, Texture2D>();

        // List of all GameObjects
        public static List<GameObject> gameObject = new List<GameObject>();

        // Create menu screens objects and parameters
        public static List<Title> titleElements = new List<Title>();
        public static List<Option> optionElements = new List<Option>();
        public static List<GameOver> gameOverElements = new List<GameOver>();

        // Create stack of lives
        public static List<GraphicElement> livesLeft = new List<GraphicElement>();

        // Create player object and parameters
        Player player;

        // Create enemy object and parameters
        List<Enemy> enemies = new List<Enemy>();

        // Create platform object and parameters
        List<Platform> platforms = new List<Platform>();

        // Create gameGraphics object and parameters
        List<GraphicElement> gameGraphics = new List<GraphicElement>();

        // List of objects currently being intersected
        protected List<GameObject> intersectedBy = new List<GameObject>();

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
            // Add player sprites
            playerElements.Add("PlayerCharacter", Content.Load<Texture2D>("TestImage"));

            // Add platform sprites
            platformElements.Add("FlatPlatform", Content.Load<Texture2D>("Platform"));

            // Add enemy sprites
            enemyElements.Add("GeneralEnemy", Content.Load<Texture2D>("TestEnemySprite"));

            // Add random other graphics sprites
            graphicElements.Add("LifeIcon", Content.Load<Texture2D>("GeneralElements\\LifeIcon"));

            // Add menu sprites
            menuElements.Add("MenuBackground", Content.Load<Texture2D>("MenuBackground"));
            menuElements.Add("Title", Content.Load<Texture2D>("Title\\Title"));
            menuElements.Add("SelectionFrame", Content.Load<Texture2D>("Title\\SelectionFrame"));
            menuElements.Add("LoadGame", Content.Load<Texture2D>("Title\\LoadGame"));
            menuElements.Add("NewGame", Content.Load<Texture2D>("Title\\NewGame"));
            menuElements.Add("Options", Content.Load<Texture2D>("Title\\Options"));

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

            LoadPlayerElements();
            LoadFloorElements();
            LoadEnemyElements();
            LoadMenuElements();
            LoadGeneralElements();
                                                                                            // All objects need to be added to the gameObject list
            gameObject.Add(player);                                                         // Add player to gameObject

            foreach (Enemy enemy in enemies)                                                // Add enemies to gameObject
            {
                gameObject.Add(enemy);
            }

            foreach (Platform platform in platforms)                                        // Add platforms to gameObject
            {
                gameObject.Add(platform);
            }

            foreach (GraphicElement graphic in gameGraphics)
            {
                gameObject.Add(graphic);
            }

            fps = 10.0f;                                                                    // Set up animation variables;
            secondsPerFrame = 1.0f / fps;
            timeCounter = 0;
        }

        private void LoadPlayerElements()
        {
            player = new Player(
                            playerElements["PlayerCharacter"],
                            x: 50,
                            y: 50,
                            width: 50,
                            height: 50
                         );

            player.ApplyGravity = true;


        }

        private void LoadFloorElements()
        {
            platforms.Add(                                                                  // Ceiling platform
                new Platform(
                    spriteTexture: platformElements["FlatPlatform"],
                    x: 200,
                    y: 400,
                    width: 200,
                    height: 50
                )
            );

            //platforms[0].ApplyGravity = true;                                              // MOST OF THESE ARE FOR TESTING PURPOSES
            //platforms[0].gravityDirection = GravityDirection.Right;                        // YOU CAN UNCOMMENT TO SEE WHAT HAPPENS
            //platforms[0].gravityOnProximityFrom = GravityOnProximityFrom.Top;


            platforms.Add(
                new Platform(
                    spriteTexture: platformElements["FlatPlatform"],
                    x: 0,
                    y: 100,
                    width: 100,
                    height: 100
                )
            );

            //platforms[1].ApplyGravity = false;                                            // State that the second platform should not have gravity
            //platforms[1].gravityDirection = GravityDirection.Down;                         // during instantiation because gravity will be implemented
            //platforms[1].gravityDirection = GravityDirection.Right;                       // to the right if the object receives a proximity warning
            //platforms[1].gravityOnProximityFrom = GravityOnProximityFrom.Top;             // from above and then moves back and forth
            //platforms[1].objectMovement = ObjectMovement.ToAndFroRightFirst;
            //platforms[1].objectMovement = ObjectMovement.ToAndFroLeftFirst;
            //platforms[1].objectMovement = ObjectMovement.ToAndFroDownFirst;

            platforms.Add(
                new Platform(
                    spriteTexture: platformElements["FlatPlatform"],
                    x: 90,
                    y: 200,
                    width: 210,
                    height: 50
                )
            );

            platforms.Add(
                new Platform(
                    spriteTexture: platformElements["FlatPlatform"],
                    x: 400,
                    y: 200,
                    width: 100,
                    height: 50
                )
            );

            platforms[3].ApplyGravity = true;
            platforms[3].objectMovement = ObjectMovement.ToAndFroUpFirst;
            platforms[3].ObjectYMoveDistance = 50;

            platforms.Add(
                new Platform(
                    spriteTexture: platformElements["FlatPlatform"],
                    x: 500,
                    y: 100,
                    width: 200,
                    height: 50
                )
            );

            platforms[4].ApplyGravity = false;
            platforms[4].gravityOnProximityFrom = GravityOnProximityFrom.Top;
            platforms[4].objectMovement = ObjectMovement.ToAndFroRightFirst;
            platforms[4].ObjectXMoveDistance = 400;

            //platforms.Add(new Platform(platformSprites, (SCREENWIDTH / 2) - 400, (SCREENHEIGHT / 2) - 400, 800, 800));        // Size of the menus

        }

        private void LoadEnemyElements()
        {
            enemies.Add(
                new Enemy(
                    spriteTexture: enemyElements["GeneralEnemy"],
                    x: 200,
                    y: 50,
                    width: 50,
                    height: 50
                )
            );

            enemies[0].ApplyGravity = true;
            enemies[0].objectMovement = ObjectMovement.ToAndFroRightFirst;
            enemies[0].ObjectXMoveDistance = 50;
        }

        private void LoadGeneralElements()
        {
            gameGraphics.Add(
                new GraphicElement(
                    elementName: "LifeIcon",
                    spriteTexture: graphicElements["LifeIcon"],
                    x: 10,
                    y: 10,
                    width: 20,
                    height: 20
                )
            );

            gameGraphics.Add(
                new GraphicElement(
                    elementName: "LifeIcon",
                    spriteTexture: graphicElements["LifeIcon"],
                    x: 40,
                    y: 10,
                    width: 20,
                    height: 20
                )
            );

            gameGraphics.Add(
                new GraphicElement(
                    elementName: "LifeIcon",
                    spriteTexture: graphicElements["LifeIcon"],
                    x: 70, 
                    y: 10, 
                    width: 20, 
                    height: 20
                )
            );

        }

        private void LoadMenuElements()
        {
            titleElements.Add(
                new Title(
                    menuItem: "MenuBackground",
                    spriteTexture: menuElements["MenuBackground"],
                    x: ((SCREENWIDTH / 2) - 400),
                    y: ((SCREENHEIGHT / 2) - 400),
                    width: 800,
                    height: 800,
                    addGravity: false,
                    appliedObjectMass: 0
                )
            );

            titleElements.Add(
                new Title(
                    menuItem: "Title",
                    spriteTexture: menuElements["Title"],
                    x: ((SCREENWIDTH / 2) - 300),
                    y: ((SCREENHEIGHT / 2) - 400),
                    width: 600,
                    height: 300,
                    addGravity: false,
                    appliedObjectMass: 0
                )
            );

            titleElements.Add(
                new Title(
                    menuItem: "LoadGame",
                    spriteTexture: menuElements["LoadGame"],
                    x: ((SCREENWIDTH / 2) - 200),
                    y: ((SCREENHEIGHT / 2) - 100),
                    width: 400,
                    height: 100,
                    addGravity: false,
                    appliedObjectMass: 0
                )
            );

            titleElements.Add(
                new Title(
                    menuItem: "NewGame",
                    spriteTexture: menuElements["NewGame"],
                    x: ((SCREENWIDTH / 2) - 200),
                    y: (SCREENHEIGHT / 2) + 50,
                    width: 400,
                    height: 100,
                    addGravity: false,
                    appliedObjectMass: 0
                )
            );

            titleElements.Add(
                new Title(
                    menuItem: "Options",
                    spriteTexture: menuElements["Options"],
                    x: ((SCREENWIDTH / 2) - 200),
                    y: ((SCREENHEIGHT / 2) + 200),
                    width: 400,
                    height: 100,
                    addGravity: false,
                    appliedObjectMass: 0
                )
            );

            titleElements.Add(
                new Title(
                    menuItem: "SelectionFrame",
                    spriteTexture: menuElements["SelectionFrame"],
                    x: ((SCREENWIDTH / 2) - 200),
                    y: ((SCREENHEIGHT / 2) - 100),
                    width: 400,
                    height: 100,
                    addGravity: false,
                    appliedObjectMass: 0
                )
            );
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

            previousKeyboardState = currentKeyboardState;                                   // Get comparative keyboard states
            currentKeyboardState = Keyboard.GetState();

            switch (gameState)
            {
                case GameState.Title:
                    for (int i = 0; i < titleElements.Count; i++) {
                        titleElements[i].Update(gameTime);
                    }

                    break;
                case GameState.Options:
                    break;
                case GameState.InGame:
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

                    break;
                case GameState.Pause:
                    break;
                case GameState.GameOver:
                    break;
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

            switch (gameState)
            {
                case GameState.Title:
                    for (int i = 0; i < titleElements.Count; i++)
                    {
                        titleElements[i].Draw(spriteBatch);
                    }

                    break;
                case GameState.Options:
                    break;
                case GameState.InGame:
                    for (int i = 0; i < gameObject.Count; i++)                                      // Draw all the objects in the game
                    {
                        if (gameObject[i].Visible)
                        {
                            gameObject[i].Draw(spriteBatch);
                        }
                    }

                    break;
                case GameState.Pause:
                    break;
                case GameState.GameOver:
                    break;
            }
            

            spriteBatch.End();

            base.Draw(gameTime);
        }

        
    }
}
