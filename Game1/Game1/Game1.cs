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
        public static Dictionary<string, Texture2D> playerSprites = new Dictionary<string, Texture2D>();

        // Dictionary of enemy sprites
        public static Dictionary<string, Texture2D> enemySprites = new Dictionary<string, Texture2D>();

        // Dictionary of platform sprites
        public static Dictionary<string, Texture2D> platformSprites = new Dictionary<string, Texture2D>();

        // Dictionary of general menu sprites
        public static Dictionary<string, Texture2D> menuSprites = new Dictionary<string, Texture2D>();

        // Dictionary of general sprites
        public static Dictionary<string, Texture2D> generalSprites = new Dictionary<string, Texture2D>();

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
            LoadSprites();

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

            InitializeGameObjects();

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
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            previousKeyboardState = currentKeyboardState;                                   // Get comparative keyboard states
            currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape))
            {
                switch (gameState)
                {
                    case GameState.Title:
                        Exit();

                        break;
                    case GameState.Options:
                        gameState = GameState.Title;

                        break;
                    case GameState.InGame:
                        gameState = GameState.GameOver;

                        break;
                    case GameState.GameOver:
                        gameState = GameState.Title;

                        break;
                }
            }

            switch (gameState)
            {
                case GameState.Title:
                    for (int i = 0; i < titleElements.Count; i++)
                    {
                        titleElements[i].Update(gameTime);
                    }

                    break;
                case GameState.Options:
                    for (int i = 0; i < titleElements.Count; i++)
                    {
                        titleElements[i].Update(gameTime);
                    }

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
                    //ClearGameObjects();

                    //InitializeGameObjects();
                    gameState = GameState.Title;

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
                    for (int i = 0; i < optionElements.Count; i++)
                    {
                        optionElements[i].Draw(spriteBatch);
                    }

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

        private void LoadSprites()
        {
            // Add player sprites
            playerSprites.Add("PlayerCharacter", Content.Load<Texture2D>("TestImage"));

            // Add platform sprites
            platformSprites.Add("FlatPlatform", Content.Load<Texture2D>("Platform"));

            // Add enemy sprites
            enemySprites.Add("GeneralEnemy", Content.Load<Texture2D>("TestEnemySprite"));

            // Add random other graphics sprites
            generalSprites.Add("LifeIcon", Content.Load<Texture2D>("GeneralElements\\LifeIcon"));

            // Add menu sprites
            menuSprites.Add("MenuBackground", Content.Load<Texture2D>("MenuBackground"));
            menuSprites.Add("Title", Content.Load<Texture2D>("Title\\Title"));
            menuSprites.Add("SelectionFrame", Content.Load<Texture2D>("Title\\SelectionFrame"));
            menuSprites.Add("LoadGame", Content.Load<Texture2D>("Title\\LoadGame"));
            menuSprites.Add("NewGame", Content.Load<Texture2D>("Title\\NewGame"));
            menuSprites.Add("Options", Content.Load<Texture2D>("Title\\Options"));

            menuSprites.Add("OptionsTitle", Content.Load<Texture2D>("Options\\OptionsTitle"));
            menuSprites.Add("Music", Content.Load<Texture2D>("Options\\Music"));
            menuSprites.Add("SFX", Content.Load<Texture2D>("Options\\SFX"));
            menuSprites.Add("Difficulty", Content.Load<Texture2D>("Options\\Difficulty"));
            menuSprites.Add("OnButton", Content.Load<Texture2D>("Options\\OnButton"));
            menuSprites.Add("OffButton", Content.Load<Texture2D>("Options\\OffButton"));
            menuSprites.Add("OnText", Content.Load<Texture2D>("Options\\OnText"));
            menuSprites.Add("OffText", Content.Load<Texture2D>("Options\\OffText"));
            menuSprites.Add("SettingsBar", Content.Load<Texture2D>("Options\\SettingsBar"));
            menuSprites.Add("SettingsSlider", Content.Load<Texture2D>("Options\\SettingsSlider"));
        }

        private void InitializeGameObjects()
        {
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
        }

        private void ClearGameObjects()
        {
            gameObject.Clear();
            platforms.Clear();
            enemies.Clear();
            gameGraphics.Clear();
            titleElements.Clear();
            optionElements.Clear();
        }

        private void LoadPlayerElements()
        {
            player = new Player(
                            playerSprites["PlayerCharacter"],
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
                    spriteTexture: platformSprites["FlatPlatform"],
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
                    spriteTexture: platformSprites["FlatPlatform"],
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
                    spriteTexture: platformSprites["FlatPlatform"],
                    x: 90,
                    y: 200,
                    width: 210,
                    height: 50
                )
            );

            platforms.Add(
                new Platform(
                    spriteTexture: platformSprites["FlatPlatform"],
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
                    spriteTexture: platformSprites["FlatPlatform"],
                    x: 500,
                    y: 100,
                    width: 200,
                    height: 50
                )
            );

            platforms[4].ApplyGravity = true;
            platforms[4].gravityOnProximityFrom = GravityOnProximityFrom.Top;
            platforms[4].objectMovement = ObjectMovement.ToAndFroRightFirst;
            platforms[4].ObjectXMoveDistance = 400;

            //platforms.Add(new Platform(platformSprites, (SCREENWIDTH / 2) - 400, (SCREENHEIGHT / 2) - 400, 800, 800));        // Size of the menus

        }

        private void LoadEnemyElements()
        {
           enemies.Add(
                new Enemy(
                    spriteTexture: enemySprites["GeneralEnemy"],
                    x: 200,
                    y: 50,
                    width: 50,
                    height: 50
                )
            );

            enemies[0].ApplyGravity = true;
            //enemies[0].objectMovement = ObjectMovement.ToAndFroRightFirst;
            enemies[0].ObjectXMoveDistance = 50;

            enemies.Add(
                new Enemy(
                    spriteTexture: enemySprites["GeneralEnemy"],
                    x: 700,
                    y: 150,
                    width: 50,
                    height: 50
                )
            );

            enemies[1].ApplyGravity = true;
            //enemies[1].objectMovement = ObjectMovement.ToAndFroRightFirst;
            enemies[1].ObjectXMoveDistance = 50;
            

            //added new enemy code, spawned randomly

            //Random rnd = new Random();

            //for(int i = 0;i<rnd.Next(1,21);i++)
            //{
            //    enemies.Add(
            //    new Enemy(
            //        spriteTexture: enemySprites["GeneralEnemy"],
            //        x: 50+rnd.Next(0,1200),
            //        y: 50+rnd.Next(0,800),
            //        width: 50,
            //        height: 50
            //        )
            //    );
            //    int j = rnd.Next(1,5);
            //enemies[i].ApplyGravity = true;
            //    switch(j)
            //       {
            //        case 1: enemies[i].objectMovement = ObjectMovement.ToAndFroRightFirst;
            //            break;
            //        case 2: enemies[i].objectMovement = ObjectMovement.ToAndFroLeftFirst;
            //            break;
            //        case 3: enemies[i].objectMovement = ObjectMovement.ToAndFroUpFirst;
            //            break;
            //        case 4: enemies[i].objectMovement = ObjectMovement.ToAndFroDownFirst;
            //            break;
            //       }
            
            //enemies[i].ObjectXMoveDistance = 50;
            //}
            //enemies.Add(
            //    new Enemy(
            //        spriteTexture: enemySprites["GeneralEnemy"],
            //        x: 1200,
            //        y: 400,
            //        width: 100,
            //        height: 100
            //    )
            //);

            //enemies[1].ApplyGravity = true;
            //enemies[1].objectMovement = ObjectMovement.ToAndFroUpFirst;
            //enemies[1].ObjectXMoveDistance = 100;
                         
        }

        private void LoadGeneralElements()
        {
            gameGraphics.Add(
                new GraphicElement(
                    elementName: "LifeIcon",
                    spriteTexture: generalSprites["LifeIcon"],
                    x: 10,
                    y: 10,
                    width: 40,
                    height: 40
                )
            );

            gameGraphics.Add(
                new GraphicElement(
                    elementName: "LifeIcon",
                    spriteTexture: generalSprites["LifeIcon"],
                    x: 60,
                    y: 10,
                    width: 40,
                    height: 40
                )
            );

            gameGraphics.Add(
                new GraphicElement(
                    elementName: "LifeIcon",
                    spriteTexture: generalSprites["LifeIcon"],
                    x: 110, 
                    y: 10, 
                    width: 40, 
                    height: 40
                )
            );

        }

        private void LoadMenuElements()
        {
// Title menu starts here
            titleElements.Add(
                new Title(
                    menuItem: "MenuBackground",
                    spriteTexture: menuSprites["MenuBackground"],
                    x: (SCREENWIDTH / 2) - 400,
                    y: (SCREENHEIGHT / 2) - 400,
                    width: 800,
                    height: 800,
                    addGravity: false
                )
            );

            titleElements.Add(
                new Title(
                    menuItem: "Title",
                    spriteTexture: menuSprites["Title"],
                    x: (SCREENWIDTH / 2) - 300,
                    y: (SCREENHEIGHT / 2) - 400,
                    width: 600,
                    height: 200,
                    addGravity: false
                )
            );

            titleElements.Add(
                new Title(
                    menuItem: "LoadGame",
                    spriteTexture: menuSprites["LoadGame"],
                    x: (SCREENWIDTH / 2) - 200,
                    y: (SCREENHEIGHT / 2) - 100,
                    width: 400,
                    height: 100,
                    addGravity: false
                )
            );

            titleElements.Add(
                new Title(
                    menuItem: "NewGame",
                    spriteTexture: menuSprites["NewGame"],
                    x: (SCREENWIDTH / 2) - 200,
                    y: (SCREENHEIGHT / 2) + 50,
                    width: 400,
                    height: 100,
                    addGravity: false
                )
            );

            titleElements.Add(
                new Title(
                    menuItem: "Options",
                    spriteTexture: menuSprites["Options"],
                    x: (SCREENWIDTH / 2) - 200,
                    y: (SCREENHEIGHT / 2) + 200,
                    width: 400,
                    height: 100,
                    addGravity: false
                )
            );

            titleElements.Add(
                new Title(
                    menuItem: "SelectionFrame",
                    spriteTexture: menuSprites["SelectionFrame"],
                    x: (SCREENWIDTH / 2) - 200,
                    y: (SCREENHEIGHT / 2) - 100,
                    width: 400,
                    height: 100,
                    addGravity: false
                )
            );

// Option menu starts here

            optionElements.Add(
                new Option(
                    menuItem: "MenuBackground",
                    spriteTexture: menuSprites["MenuBackground"],
                    x: (SCREENWIDTH / 2) - 400,
                    y: (SCREENHEIGHT / 2) - 400,
                    width: 800,
                    height: 800,
                    addGravity: false
                )
            );

            optionElements.Add(
                new Option(
                    menuItem: "OptionsTitle",
                    spriteTexture: menuSprites["OptionsTitle"],
                    x: (SCREENWIDTH / 2) - 300,
                    y: (SCREENHEIGHT / 2) - 400,
                    width: 600,
                    height: 200,
                    addGravity: false
                )
            );

            optionElements.Add(
                new Option(
                    menuItem: "Music",
                    spriteTexture: menuSprites["Music"],
                    x: (SCREENWIDTH / 2) - 200,
                    y: (SCREENHEIGHT / 2) - 100,
                    width: 200,
                    height: 80,
                    addGravity: false
                )
            );

            optionElements.Add(
                new Option(
                    menuItem: "MusicOn",
                    spriteTexture: menuSprites["OnText"],
                    x: (SCREENWIDTH / 2) + 100,
                    y: (SCREENHEIGHT / 2) - 70,
                    width: 50,
                    height: 30,
                    addGravity: false
                )
            );

            optionElements.Add(
                new Option(
                    menuItem: "MusicOff",
                    spriteTexture: menuSprites["OffText"],
                    x: (SCREENWIDTH / 2) + 300,
                    y: (SCREENHEIGHT / 2) - 70,
                    width: 75,
                    height: 30,
                    addGravity: false
                )
            );

            optionElements.Add(
                new Option(
                    menuItem: "MusicBar",
                    spriteTexture: menuSprites["SettingsBar"],
                    x: (SCREENWIDTH / 2) + 175,
                    y: (SCREENHEIGHT / 2) - 60,
                    width: 100,
                    height: 10,
                    addGravity: false
                )
            );

            optionElements.Add(
                new Option(
                    menuItem: "MusicSlider",
                    spriteTexture: menuSprites["SettingsSlider"],
                    x: (SCREENWIDTH / 2) + 180,
                    y: (SCREENHEIGHT / 2) - 76,
                    width: 10,
                    height: 40,
                    addGravity: false
                )
            );

            optionElements.Add(
                new Option(
                    menuItem: "SFX",
                    spriteTexture: menuSprites["SFX"],
                    x: (SCREENWIDTH / 2) - 200,
                    y: (SCREENHEIGHT / 2),
                    width: 200,
                    height: 80,
                    addGravity: false
                )
            );

            optionElements.Add(
                new Option(
                    menuItem: "SFXOn",
                    spriteTexture: menuSprites["OnText"],
                    x: (SCREENWIDTH / 2) + 100,
                    y: (SCREENHEIGHT / 2) + 20,
                    width: 50,
                    height: 30,
                    addGravity: false
                )
            );

            optionElements.Add(
                new Option(
                    menuItem: "SFXOff",
                    spriteTexture: menuSprites["OffText"],
                    x: (SCREENWIDTH / 2) + 300,
                    y: (SCREENHEIGHT / 2) + 20,
                    width: 75,
                    height: 30,
                    addGravity: false
                )
            );

            optionElements.Add(
                new Option(
                    menuItem: "SFXBar",
                    spriteTexture: menuSprites["SettingsBar"],
                    x: (SCREENWIDTH / 2) + 175,
                    y: (SCREENHEIGHT / 2) + 30,
                    width: 100,
                    height: 10,
                    addGravity: false
                )
            );

            optionElements.Add(
                new Option(
                    menuItem: "SFXSlider",
                    spriteTexture: menuSprites["SettingsSlider"],
                    x: (SCREENWIDTH / 2) + 180,
                    y: (SCREENHEIGHT / 2) + 14,
                    width: 10,
                    height: 40,
                    addGravity: false
                )
            );

            optionElements.Add(
                new Option(
                    menuItem: "Difficulty",
                    spriteTexture: menuSprites["Difficulty"],
                    x: (SCREENWIDTH / 2) - 200,
                    y: (SCREENHEIGHT / 2) + 100,
                    width: 200,
                    height: 80,
                    addGravity: false
                )
            );
        }
    }
}
