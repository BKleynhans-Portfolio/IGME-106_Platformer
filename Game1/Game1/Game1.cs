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
        protected static GameState gameState;

        // Define graphics devices
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Define input devices
        private static KeyboardState currentKeyboardState;
        private static KeyboardState previousKeyboardState;

        // Dictionary of player sprites
        private static Dictionary<string, Texture2D> playerSprites = new Dictionary<string, Texture2D>();

        // Dictionary of enemy sprites
        private static Dictionary<string, Texture2D> enemySprites = new Dictionary<string, Texture2D>();

        // Dictionary of platform sprites
        private static Dictionary<string, Texture2D> platformSprites = new Dictionary<string, Texture2D>();

        // Dictionary of general menu sprites
        private static Dictionary<string, Texture2D> menuSprites = new Dictionary<string, Texture2D>();

        // Dictionary of general sprites
        private static Dictionary<string, Texture2D> generalSprites = new Dictionary<string, Texture2D>();

        // List of all GameObjects
        private static List<GameObject> gameObject = new List<GameObject>();

        // Create menu screens objects and parameters
        private static List<Title> titleElements = new List<Title>();
        private static List<Option> optionElements = new List<Option>();
        private static List<GameOver> gameOverElements = new List<GameOver>();

        // Create stack of lives
        private static List<GraphicElement> livesLeft = new List<GraphicElement>();

        // Create player object and parameters
        private Player player;

        // Create enemy object and parameters
        private List<Enemy> enemies = new List<Enemy>();

        // Create platform object and parameters
        private List<Platform> platforms = new List<Platform>();

        // Create gameGraphics object and parameters
        private List<GraphicElement> gameGraphics = new List<GraphicElement>();

        // List of objects currently being intersected
        protected List<GameObject> intersectedBy = new List<GameObject>();

        // Animation variables        
        public static double fps;
        public static double secondsPerFrame;
        public static double timeCounter;

        // Window properties
        public const int SCREENWIDTH = 1600;
        public const int SCREENHEIGHT = 900;

        public GameState GameState
        {
            get { return gameState; }
            set { gameState = value; }
        }

        public KeyboardState CurrentKeyboardState
        {
            get { return currentKeyboardState; }
            set { currentKeyboardState = value; }
        }

        public KeyboardState PreviousKeyboardState
        {
            get { return previousKeyboardState; }
            set { previousKeyboardState = value; }
        }

        private static Dictionary<string, Texture2D> PlayerSprites
        {
            get { return playerSprites; }
            set { playerSprites = value; }
        }
                
        private static Dictionary<string, Texture2D> EnemySprites
        {
            get { return enemySprites; }
            set { enemySprites = value; }
        }

        private static Dictionary<string, Texture2D> PlatformSprites
        {
            get { return platformSprites; }
            set { platformSprites = value; }
        }

        private static Dictionary<string, Texture2D> MenuSprites
        {
            get { return menuSprites; }
            set { menuSprites = value; }
        }

        private static Dictionary<string, Texture2D> GeneralSprites
        {
            get { return generalSprites; }
            set { generalSprites = value; }
        }

        private static List<GameObject> GameObject
        {
            get { return gameObject; }
            set { gameObject = value; }
        }

        
        private static List<Title> TitleElements
        {
            get { return titleElements; }
            set { titleElements = value; }
        }

        private static List<Option> OptionElements
        {
            get { return optionElements; }
            set { optionElements = value; }
        }

        private static List<GameOver> GameOverElements
        {
            get { return gameOverElements; }
            set { gameOverElements = value; }
        }
                
        public static List<GraphicElement> LivesLeft
        {
            get { return livesLeft; }
            set { livesLeft = value; }
        }

        private Player Player
        {
            get { return player; }
            set { player = value; }
        }

        private List<Enemy> Enemies
        {
            get { return enemies; }
            set { enemies = value; }
        }
                
        private List<Platform> Platforms
        {
            get { return platforms; }
            set { platforms = value; }
        }

        private List<GraphicElement> GameGraphics
        {
            get { return gameGraphics; }
            set { gameGraphics = value; }
        }

        protected List<GameObject> IntersectedBy
        {
            get { return intersectedBy; }
            set { intersectedBy = value; }
        }

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

            Window.Position = new Point(                                                    // Center the game view on the screen
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

            PreviousKeyboardState = CurrentKeyboardState;                                   // Get comparative keyboard states
            CurrentKeyboardState = Keyboard.GetState();

            if (CurrentKeyboardState.IsKeyDown(Keys.Escape) && PreviousKeyboardState.IsKeyUp(Keys.Escape))
            {
                switch (gameState)
                {
                    case GameState.Title:
                        Exit();

                        break;
                    case GameState.Options:
                        GameState = GameState.Title;

                        break;
                    case GameState.InGame:
                        GameState = GameState.GameOver;

                        break;
                    case GameState.GameOver:
                        GameState = GameState.Title;

                        break;
                }
            }

            switch (gameState)
            {
                case GameState.Title:
                    for (int i = 0; i < TitleElements.Count; i++)
                    {
                        TitleElements[i].Update(gameTime);
                    }

                    break;
                case GameState.Options:
                    for (int i = 0; i < TitleElements.Count; i++)
                    {
                        TitleElements[i].Update(gameTime);
                    }

                    break;
                case GameState.InGame:
                    for (int i = 0; i < (GameObject.Count - 1); i++)                                // Cycle through GameObject list and test
                    {                                                                               // all created objects for intersection
                        for (int j = i + 1; j < GameObject.Count; j++)
                        {
                            if (GameObject[i].Intersects(GameObject[j]))
                            {
                                if (!GameObject[i].intersectedBy.Contains(GameObject[j]))           // If there is an intersection, create
                                {                                                                   // references in the objects to each other
                                    GameObject[i].intersectedBy.Add(GameObject[j]);
                                }

                                if (!GameObject[j].intersectedBy.Contains(GameObject[i]))
                                {
                                    GameObject[j].intersectedBy.Add(GameObject[i]);
                                }
                            }
                        }
                    }

                    for (int i = 0; i < GameObject.Count; i++)                                      // Update all the objects in the game
                    {
                        GameObject[i].Update(gameTime);
                    }

                    break;
                case GameState.Pause:
                    break;
                case GameState.GameOver:
                    ClearGameObjects();

                    InitializeGameObjects();
                    GameState = GameState.Title;

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

            switch (GameState)
            {
                case GameState.Title:
                    for (int i = 0; i < TitleElements.Count; i++)
                    {
                        TitleElements[i].Draw(spriteBatch);
                    }

                    break;
                case GameState.Options:
                    for (int i = 0; i < OptionElements.Count; i++)
                    {
                        OptionElements[i].Draw(spriteBatch);
                    }

                    break;
                case GameState.InGame:
                    for (int i = 0; i < GameObject.Count; i++)                                      // Draw all the objects in the game
                    {
                        if (GameObject[i].Visible)
                        {
                            GameObject[i].Draw(spriteBatch);
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
            PlayerSprites.Add("PlayerCharacter", Content.Load<Texture2D>("TestImage"));

            // Add platform sprites
            PlatformSprites.Add("FlatPlatform", Content.Load<Texture2D>("Platform"));

            // Add enemy sprites
            EnemySprites.Add("GeneralEnemy", Content.Load<Texture2D>("TestEnemySprite"));

            // Add random other graphics sprites
            GeneralSprites.Add("LifeIcon", Content.Load<Texture2D>("GeneralElements\\LifeIcon"));

            // Add menu sprites
            MenuSprites.Add("MenuBackground", Content.Load<Texture2D>("MenuBackground"));
            MenuSprites.Add("Title", Content.Load<Texture2D>("Title\\Title"));
            MenuSprites.Add("SelectionFrame", Content.Load<Texture2D>("Title\\SelectionFrame"));
            MenuSprites.Add("LoadGame", Content.Load<Texture2D>("Title\\LoadGame"));
            MenuSprites.Add("NewGame", Content.Load<Texture2D>("Title\\NewGame"));
            MenuSprites.Add("Options", Content.Load<Texture2D>("Title\\Options"));

            MenuSprites.Add("OptionsTitle", Content.Load<Texture2D>("Options\\OptionsTitle"));
            MenuSprites.Add("Music", Content.Load<Texture2D>("Options\\Music"));
            MenuSprites.Add("SFX", Content.Load<Texture2D>("Options\\SFX"));
            MenuSprites.Add("Difficulty", Content.Load<Texture2D>("Options\\Difficulty"));
            MenuSprites.Add("OnButton", Content.Load<Texture2D>("Options\\OnButton"));
            MenuSprites.Add("OffButton", Content.Load<Texture2D>("Options\\OffButton"));
            MenuSprites.Add("OnText", Content.Load<Texture2D>("Options\\OnText"));
            MenuSprites.Add("OffText", Content.Load<Texture2D>("Options\\OffText"));
            MenuSprites.Add("SettingsBar", Content.Load<Texture2D>("Options\\SettingsBar"));
            MenuSprites.Add("SettingsSlider", Content.Load<Texture2D>("Options\\SettingsSlider"));
        }

        private void InitializeGameObjects()
        {
            LoadPlayerElements();
            LoadFloorElements();
            LoadEnemyElements();            
            LoadGeneralElements();

            if (TitleElements.Count == 0)
            {
                LoadMenuElements();
            }

            // All objects need to be added to the GameObject list
            GameObject.Add(player);                                                         // Add player to GameObject

            foreach (Enemy enemy in Enemies)                                                // Add enemies to GameObject
            {
                GameObject.Add(enemy);
            }

            foreach (Platform platform in Platforms)                                        // Add platforms to GameObject
            {
                GameObject.Add(platform);
            }

            foreach (GraphicElement graphic in GameGraphics)
            {
                GameObject.Add(graphic);
            }
        }

        private void ClearGameObjects()
        {
            GameObject.Clear();
            Platforms.Clear();
            Enemies.Clear();
            GameGraphics.Clear();
        }

        private void LoadPlayerElements()
        {
            Player = new Player(
                            PlayerSprites["PlayerCharacter"],
                            x: 0,
                            y: 800,
                            width: 40,
                            height: 40
                         );

            Player.ApplyGravity = true;
        }

        private void LoadFloorElements()
        {
            //000
            Platforms.Add(                                                                  // Ceiling platform
                new Platform(
                    spriteTexture: PlatformSprites["FlatPlatform"],
                    x: 0,
                    y: 860,
                    width: 400,
                    height: 50
                )
            );

            //001
            Platforms.Add(
                new Platform(
                    spriteTexture: PlatformSprites["FlatPlatform"],
                    x: 490,
                    y: 860,
                    width: 110,
                    height: 50
                )
            );

            //002
            Platforms.Add(
                new Platform(
                    spriteTexture: PlatformSprites["FlatPlatform"],
                    x: 800,
                    y: 860,
                    width: 200,
                    height: 50
                )
            );

            //003
            Platforms.Add(
                new Platform(
                    spriteTexture: PlatformSprites["FlatPlatform"],
                    x: 1400,
                    y: 860,
                    width: 200,
                    height: 50
                )
            );

            //004
            Platforms.Add(
                new Platform(
                    spriteTexture: PlatformSprites["FlatPlatform"],
                    x: 1100,
                    y: 800,
                    width: 100,
                    height: 50
                )
            );

            Platforms[4].ApplyGravity = true;
            Platforms[4].ObjectMovement = ObjectMovement.ToAndFroUpFirst;
            Platforms[4].ObjectYMoveDistance = 300;

            //005
            Platforms.Add(
                new Platform(
                    spriteTexture: PlatformSprites["FlatPlatform"],
                    x: 600,
                    y: 700,
                    width: 100,
                    height: 50
                )
            );

            Platforms[5].ApplyGravity = true;
            Platforms[5].ObjectMovement = ObjectMovement.ToAndFroRightFirst;
            Platforms[5].ObjectXMoveDistance = 400;

            //006
            Platforms.Add(
                new Platform(
                    spriteTexture: PlatformSprites["FlatPlatform"],
                    x: 300,
                    y: 700,
                    width: 200,
                    height: 50
                )
            );

            //007
            Platforms.Add(
                new Platform(
                    spriteTexture: PlatformSprites["FlatPlatform"],
                    x: 0,
                    y: 510,
                    width: 510,
                    height: 50
                )
            );

            //008
            Platforms.Add(
                new Platform(
                    spriteTexture: PlatformSprites["FlatPlatform"],
                    x: 50,
                    y: 400,
                    width: 100,
                    height: 50
                )
            );

            Platforms[8].ApplyGravity = true;
            Platforms[8].ObjectMovement = ObjectMovement.ToAndFroUpFirst;
            Platforms[8].ObjectYMoveDistance = 100;

            //009
            Platforms.Add(
                new Platform(
                    spriteTexture: PlatformSprites["FlatPlatform"],
                    x: 1000,
                    y: 500,
                    width: 100,
                    height: 50
                )
            );

            Platforms[9].ApplyGravity = true;
            Platforms[9].ObjectMovement = ObjectMovement.ToAndFroUpFirst;
            Platforms[9].ObjectYMoveDistance = 200;

            //010
            Platforms.Add(
                new Platform(
                    spriteTexture: PlatformSprites["FlatPlatform"],
                    x: 1200,
                    y: 150,
                    width: 50,
                    height: 650
                )
            );

            //011
            Platforms.Add(
                new Platform(
                    spriteTexture: PlatformSprites["FlatPlatform"],
                    x: 800,
                    y: 250,
                    width: 200,
                    height: 50
                )
            );

            //012
            Platforms.Add(
                new Platform(
                    spriteTexture: PlatformSprites["FlatPlatform"],
                    x: 700,
                    y: 250,
                    width: 100,
                    height: 50
                )
            );

            Platforms[12].ApplyGravity = false;
            Platforms[12].GravityDirection = GravityDirection.Down;
            Platforms[12].ObjectMovement = ObjectMovement.OneDirection;
            Platforms[12].GravityOnProximityFrom = GravityOnProximityFrom.Top;

            //013
            Platforms.Add(
                new Platform(
                    spriteTexture: PlatformSprites["FlatPlatform"],
                    x: 600,
                    y: 250,
                    width: 100,
                    height: 50
                )
            );

            //014
            Platforms.Add(
                new Platform(
                    spriteTexture: PlatformSprites["FlatPlatform"],
                    x: 450,
                    y: 150,
                    width: 50,
                    height: 300
                )
            );

            //015
            Platforms.Add(
                new Platform(
                    spriteTexture: PlatformSprites["FlatPlatform"],
                    x: 400,
                    y: 250,
                    width: 50,
                    height: 50
                )
            );

            Platforms[15].ApplyGravity = true;            
            Platforms[15].ObjectMovement = ObjectMovement.ToAndFroLeftFirst;            
            Platforms[15].ObjectXMoveDistance = 100;

            //016
            Platforms.Add(
                new Platform(
                    spriteTexture: PlatformSprites["FlatPlatform"],
                    x: 150,
                    y: 250,
                    width: 100,
                    height: 50
                )
            );

            //017
            Platforms.Add(
                new Platform(
                    spriteTexture: PlatformSprites["FlatPlatform"],
                    x: 0,
                    y: 150,
                    width: 100,
                    height: 50
                )
            );

            Platforms[17].ApplyGravity = true;
            Platforms[17].GravityDirection = GravityDirection.Up;
            Platforms[17].ObjectMovement = ObjectMovement.ToAndFroUpFirst;
            Platforms[17].GravityOnProximityFrom = GravityOnProximityFrom.Top;
            Platforms[17].ObjectXMoveDistance = 100;

            //018
            Platforms.Add(
                new Platform(
                    spriteTexture: PlatformSprites["FlatPlatform"],
                    x: 100,
                    y: 150,
                    width: 300,
                    height: 50
                )
            );

            //019
            Platforms.Add(
                new Platform(
                    spriteTexture: PlatformSprites["FlatPlatform"],
                    x: 100,
                    y: 50,
                    width: 350,
                    height: 50
                )
            );

            //020
            Platforms.Add(
                new Platform(
                    spriteTexture: PlatformSprites["FlatPlatform"],
                    x: 450,
                    y: 100,
                    width: 550,
                    height: 50
                )
            );

            //021
            Platforms.Add(
                new Platform(
                    spriteTexture: PlatformSprites["FlatPlatform"],
                    x: 1000,
                    y: 100,
                    width: 100,
                    height: 50
                )
            );

            Platforms[21].ApplyGravity = false;
            Platforms[21].GravityDirection = GravityDirection.Down;
            Platforms[21].ObjectMovement = ObjectMovement.OneDirection;
            Platforms[21].GravityOnProximityFrom = GravityOnProximityFrom.Top;

            //022
            Platforms.Add(
                new Platform(
                    spriteTexture: PlatformSprites["FlatPlatform"],
                    x: 1100,
                    y: 100,
                    width: 100,
                    height: 50
                )
            );

            Platforms[22].ApplyGravity = false;
            Platforms[22].GravityDirection = GravityDirection.Down;
            Platforms[22].ObjectMovement = ObjectMovement.OneDirection;
            Platforms[22].GravityOnProximityFrom = GravityOnProximityFrom.Top;


        }

        private void LoadEnemyElements()
        {
            //000
            Enemies.Add(
                 new Enemy(
                     spriteTexture: enemySprites["GeneralEnemy"],
                     x: 350,
                     y: 800,
                     width: 50,
                     height: 50
                 )
             );

            Enemies[0].ApplyGravity = true;
            Enemies[0].ObjectMovement = ObjectMovement.ToAndFroLeftFirst;
            Enemies[0].ObjectXMoveDistance = 150;

            //001
            Enemies.Add(
                new Enemy(
                    spriteTexture: enemySprites["GeneralEnemy"],
                    x: 800,
                    y: 800,
                    width: 50,
                    height: 50
                )
            );

            Enemies[1].ApplyGravity = true;
            Enemies[1].ObjectMovement = ObjectMovement.ToAndFroRightFirst;
            Enemies[1].ObjectXMoveDistance = 150;

            //002
            Enemies.Add(
                new Enemy(
                    spriteTexture: enemySprites["GeneralEnemy"],
                    x: 200,
                    y: 460,
                    width: 50,
                    height: 50
                )
            );

            Enemies[2].ApplyGravity = true;
            Enemies[2].ObjectMovement = ObjectMovement.ToAndFroRightFirst;
            Enemies[2].ObjectXMoveDistance = 250;


            //added new enemy code, spawned randomly

            //Random rnd = new Random();

            //for(int i = 0;i<rnd.Next(1,21);i++)
            //{
            //    Enemies.Add(
            //    new Enemy(
            //        spriteTexture: enemySprites["GeneralEnemy"],
            //        x: 50+rnd.Next(0,1200),
            //        y: 50+rnd.Next(0,800),
            //        width: 50,
            //        height: 50
            //        )
            //    );
            //    int j = rnd.Next(1,5);
            //Enemies[i].ApplyGravity = true;
            //    switch(j)
            //       {
            //        case 1: Enemies[i].objectMovement = ObjectMovement.ToAndFroRightFirst;
            //            break;
            //        case 2: Enemies[i].objectMovement = ObjectMovement.ToAndFroLeftFirst;
            //            break;
            //        case 3: Enemies[i].objectMovement = ObjectMovement.ToAndFroUpFirst;
            //            break;
            //        case 4: Enemies[i].objectMovement = ObjectMovement.ToAndFroDownFirst;
            //            break;
            //       }

            //Enemies[i].ObjectXMoveDistance = 50;
            //}
            //Enemies.Add(
            //    new Enemy(
            //        spriteTexture: enemySprites["GeneralEnemy"],
            //        x: 1200,
            //        y: 400,
            //        width: 100,
            //        height: 100
            //    )
            //);

            //Enemies[1].ApplyGravity = true;
            //Enemies[1].objectMovement = ObjectMovement.ToAndFroUpFirst;
            //Enemies[1].ObjectXMoveDistance = 100;

        }

        private void LoadGeneralElements()
        {
            GameGraphics.Add(
                new GraphicElement(
                    elementName: "LifeIcon",
                    spriteTexture: GeneralSprites["LifeIcon"],
                    x: 10,
                    y: 10,
                    width: 40,
                    height: 40
                )
            );

            GameGraphics.Add(
                new GraphicElement(
                    elementName: "LifeIcon",
                    spriteTexture: GeneralSprites["LifeIcon"],
                    x: 60,
                    y: 10,
                    width: 40,
                    height: 40
                )
            );

            GameGraphics.Add(
                new GraphicElement(
                    elementName: "LifeIcon",
                    spriteTexture: GeneralSprites["LifeIcon"],
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
            TitleElements.Add(
                new Title(
                    menuItem: "MenuBackground",
                    spriteTexture: MenuSprites["MenuBackground"],
                    x: (SCREENWIDTH / 2) - 400,
                    y: (SCREENHEIGHT / 2) - 400,
                    width: 800,
                    height: 800,
                    addGravity: false
                )
            );

            TitleElements.Add(
                new Title(
                    menuItem: "Title",
                    spriteTexture: MenuSprites["Title"],
                    x: (SCREENWIDTH / 2) - 300,
                    y: (SCREENHEIGHT / 2) - 400,
                    width: 600,
                    height: 200,
                    addGravity: false
                )
            );

            TitleElements.Add(
                new Title(
                    menuItem: "LoadGame",
                    spriteTexture: MenuSprites["LoadGame"],
                    x: (SCREENWIDTH / 2) - 200,
                    y: (SCREENHEIGHT / 2) - 100,
                    width: 400,
                    height: 100,
                    addGravity: false
                )
            );

            TitleElements.Add(
                new Title(
                    menuItem: "NewGame",
                    spriteTexture: MenuSprites["NewGame"],
                    x: (SCREENWIDTH / 2) - 200,
                    y: (SCREENHEIGHT / 2) + 50,
                    width: 400,
                    height: 100,
                    addGravity: false
                )
            );

            TitleElements.Add(
                new Title(
                    menuItem: "Options",
                    spriteTexture: MenuSprites["Options"],
                    x: (SCREENWIDTH / 2) - 200,
                    y: (SCREENHEIGHT / 2) + 200,
                    width: 400,
                    height: 100,
                    addGravity: false
                )
            );

            TitleElements.Add(
                new Title(
                    menuItem: "SelectionFrame",
                    spriteTexture: MenuSprites["SelectionFrame"],
                    x: (SCREENWIDTH / 2) - 200,
                    y: (SCREENHEIGHT / 2) - 100,
                    width: 400,
                    height: 100,
                    addGravity: false
                )
            );

// Option menu starts here

            OptionElements.Add(
                new Option(
                    menuItem: "MenuBackground",
                    spriteTexture: MenuSprites["MenuBackground"],
                    x: (SCREENWIDTH / 2) - 400,
                    y: (SCREENHEIGHT / 2) - 400,
                    width: 800,
                    height: 800,
                    addGravity: false
                )
            );

            OptionElements.Add(
                new Option(
                    menuItem: "OptionsTitle",
                    spriteTexture: MenuSprites["OptionsTitle"],
                    x: (SCREENWIDTH / 2) - 300,
                    y: (SCREENHEIGHT / 2) - 400,
                    width: 600,
                    height: 200,
                    addGravity: false
                )
            );

            OptionElements.Add(
                new Option(
                    menuItem: "Music",
                    spriteTexture: MenuSprites["Music"],
                    x: (SCREENWIDTH / 2) - 200,
                    y: (SCREENHEIGHT / 2) - 100,
                    width: 200,
                    height: 80,
                    addGravity: false
                )
            );

            OptionElements.Add(
                new Option(
                    menuItem: "MusicOn",
                    spriteTexture: MenuSprites["OnText"],
                    x: (SCREENWIDTH / 2) + 100,
                    y: (SCREENHEIGHT / 2) - 70,
                    width: 50,
                    height: 30,
                    addGravity: false
                )
            );

            OptionElements.Add(
                new Option(
                    menuItem: "MusicOff",
                    spriteTexture: MenuSprites["OffText"],
                    x: (SCREENWIDTH / 2) + 300,
                    y: (SCREENHEIGHT / 2) - 70,
                    width: 75,
                    height: 30,
                    addGravity: false
                )
            );

            OptionElements.Add(
                new Option(
                    menuItem: "MusicBar",
                    spriteTexture: MenuSprites["SettingsBar"],
                    x: (SCREENWIDTH / 2) + 175,
                    y: (SCREENHEIGHT / 2) - 60,
                    width: 100,
                    height: 10,
                    addGravity: false
                )
            );

            OptionElements.Add(
                new Option(
                    menuItem: "MusicSlider",
                    spriteTexture: MenuSprites["SettingsSlider"],
                    x: (SCREENWIDTH / 2) + 180,
                    y: (SCREENHEIGHT / 2) - 76,
                    width: 10,
                    height: 40,
                    addGravity: false
                )
            );

            OptionElements.Add(
                new Option(
                    menuItem: "SFX",
                    spriteTexture: MenuSprites["SFX"],
                    x: (SCREENWIDTH / 2) - 200,
                    y: (SCREENHEIGHT / 2),
                    width: 200,
                    height: 80,
                    addGravity: false
                )
            );

            OptionElements.Add(
                new Option(
                    menuItem: "SFXOn",
                    spriteTexture: MenuSprites["OnText"],
                    x: (SCREENWIDTH / 2) + 100,
                    y: (SCREENHEIGHT / 2) + 20,
                    width: 50,
                    height: 30,
                    addGravity: false
                )
            );

            OptionElements.Add(
                new Option(
                    menuItem: "SFXOff",
                    spriteTexture: MenuSprites["OffText"],
                    x: (SCREENWIDTH / 2) + 300,
                    y: (SCREENHEIGHT / 2) + 20,
                    width: 75,
                    height: 30,
                    addGravity: false
                )
            );

            OptionElements.Add(
                new Option(
                    menuItem: "SFXBar",
                    spriteTexture: MenuSprites["SettingsBar"],
                    x: (SCREENWIDTH / 2) + 175,
                    y: (SCREENHEIGHT / 2) + 30,
                    width: 100,
                    height: 10,
                    addGravity: false
                )
            );

            OptionElements.Add(
                new Option(
                    menuItem: "SFXSlider",
                    spriteTexture: MenuSprites["SettingsSlider"],
                    x: (SCREENWIDTH / 2) + 180,
                    y: (SCREENHEIGHT / 2) + 14,
                    width: 10,
                    height: 40,
                    addGravity: false
                )
            );

            OptionElements.Add(
                new Option(
                    menuItem: "Difficulty",
                    spriteTexture: MenuSprites["Difficulty"],
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
