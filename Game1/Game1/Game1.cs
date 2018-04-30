using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using Game1.Properties;

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
/// Last Modified Date  : April 29, 2018
/// Filename            : Game1.cs
/// </summary>

namespace Game1
{
    public enum GameState
    {
        Title,
        LevelEditor,
        Options,
        InGame,
        AdvanceLevel,
        GameOver,
        Pause
    }

    //public enum SoundEvents { SoundEvent }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        protected static GameState gameState;

        // Define graphics devices
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Define leveleditor
        public static Form LevelForm { get; set; }        

        // Define input devices
        private static KeyboardState currentKeyboardState;
        private static KeyboardState previousKeyboardState;

        // Read File
        protected static FileStream ReadStream { get; set; }
        protected static StreamReader myReader { get; set; }

        // Write File
        protected static FileStream WriteStream { get; set; }
        protected static StreamWriter MyWriter { get; set; }

        // Add spritefont for score display
        public static SpriteFont SpriteFont { get; private set; }

        //Player score
        public static int CurrentScore { get; set; }
        public static Vector2 PlayerSpawnPoint { get; set; }

        // Dictionary of sounds
        private static Dictionary<string, SoundEffect> soundEffects = new Dictionary<string, SoundEffect>();

        // Dictionary of sound effect instances - required to adjust sound without recreation
        private static Dictionary<string, SoundEffectInstance> soundEffectInstances = new Dictionary<string, SoundEffectInstance>();

        // Dictionary of levelbackgrounds
        private static Dictionary<string, Texture2D> backgroundSprites = new Dictionary<string, Texture2D>();

        // Dictionary of player sprites
        private static Dictionary<string, Texture2D> playerSprites = new Dictionary<string, Texture2D>();

        // Dictionary of enemy sprites
        private static Dictionary<string, Texture2D> enemySprites = new Dictionary<string, Texture2D>();

        // Dictionary of goal sprites
        private static Dictionary<string, Texture2D> goalSprites = new Dictionary<string, Texture2D>();

        // Dictionary of platform sprites
        private static Dictionary<string, Texture2D> platformSprites = new Dictionary<string, Texture2D>();

        // Dictionary of collectible sprites
        private static Dictionary<string, Texture2D> collectibleSprites = new Dictionary<string, Texture2D>();

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
        private Player Player { get; set; }

        // Create goal object and parameters
        private Goal Goal { get; set; }

        // Create enemy object and parameters
        private List<Enemy> enemies = new List<Enemy>();

        // Create platform object and parameters
        private List<Platform> platforms = new List<Platform>();

        // Create collectible object and parameters
        private List<Collectible> collectibles = new List<Collectible>();

        // Create gameGraphics object and parameters
        private List<GraphicElement> gameGraphics = new List<GraphicElement>();

        // List of objects currently being intersected
        protected List<GameObject> intersectedBy = new List<GameObject>();

        // Level tracker
        private static int LevelTracker { get; set; }
        private static int LevelsAvailable { get; set; }

        // Level editor form tracker
        public bool FormCreated { get; set; }

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

        public static Dictionary<string, SoundEffect> SoundEffects
        {
            get { return soundEffects; }
            set { soundEffects = value; }
        }

        public static Dictionary<string, SoundEffectInstance> SoundEffectInstances
        {
            get { return soundEffectInstances; }
            set { soundEffectInstances = value; }
        }

        private static Dictionary<string, Texture2D> BackgroundSprites
        {
            get { return backgroundSprites; }
            set { backgroundSprites = value; }
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

        private static Dictionary<string, Texture2D> GoalSprites
        {
            get { return goalSprites; }
            set { goalSprites = value; }
        }

        private static Dictionary<string, Texture2D> CollectibleSprites
        {
            get { return collectibleSprites; }
            set { collectibleSprites = value; }
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

        public static List<Option> OptionElements
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

        private List<Collectible> Collectibles
        {
            get { return collectibles; }
            set { collectibles = value; }
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
            Application.EnableVisualStyles();

            //OpenDefaultFile = false;
            LevelTracker = 1;
            CurrentScore = 0;
            FormCreated = false;

            //Check if there are any levels available, if there aren't, inform the user
            // and load the default levels
            if (!Directory.Exists(Path.GetFullPath("Levels")))
            {
                Directory.CreateDirectory(Path.GetFullPath("Levels"));
            }

            LevelsAvailable = Directory.GetFiles(Path.GetFullPath("Levels")).Length;

            if (LevelsAvailable == 0)
            {
                MessageBox.Show(
                    "There are currently no levels in the game.  The default levels will be loaded!",
                    "Error while reading level file",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                
                CreateTempLevels();
            }
            
            LoadSoundEffects();
            LoadSprites();

            SoundEffectInstances["BackgroundMusic"].Play();

            graphics.PreferredBackBufferWidth = SCREENWIDTH;                                // Set desired width of window
            graphics.PreferredBackBufferHeight = SCREENHEIGHT;                              // Set desired height of window            
            graphics.ApplyChanges();

            Window.Position = new Point(                                                    // Center the game view on the screen
                (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) -
                    (graphics.PreferredBackBufferWidth / 2),
                (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2) -
                    (graphics.PreferredBackBufferHeight / 2)
            );

            this.IsMouseVisible = false;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {                                                                                            
            spriteBatch = new SpriteBatch(GraphicsDevice);                                  // Create a new SpriteBatch, which can be used to draw textures.            

            SpriteFont = Content.Load<SpriteFont>("Fonts\\SpriteFont");

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

            if (CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape) && 
                PreviousKeyboardState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                switch (gameState)                                                          // Manage game states
                {
                    case GameState.Title:
                        Exit();

                        break;
                    case GameState.LevelEditor:
                        GameState = GameState.Title;

                        break;
                    case GameState.Options:
                        GameState = GameState.Title;

                        break;
                    case GameState.AdvanceLevel:
                        GameState = GameState.Title;

                        break;
                    case GameState.InGame:
                        GameState = GameState.Title;

                        break;
                    case GameState.GameOver:
                        GameState = GameState.GameOver;                        

                        break;
                }
            }

            switch (gameState)                                                              // Perform action based on state
            {
                case GameState.Title:
                    for (int i = 0; i < TitleElements.Count; i++)
                    {
                        TitleElements[i].Update(gameTime);                                  // Update the title elements (menus)
                    }

                    break;
                case GameState.LevelEditor:

                    if (!FormCreated)                                                       // If the form isn't visible, create and launch the editor
                    {
                        LevelForm = new newLevelEditor.Form1();

                        LevelForm.Location = new System.Drawing.Point(
                                        (SCREENWIDTH / 2) - (LevelForm.Width / 2),
                                        (SCREENHEIGHT / 2) - (LevelForm.Height / 2)
                                     );

                        FormCreated = true;

                        if ((!LevelForm.Visible) && (FormCreated))
                        {
                            LevelForm.Show();
                        }
                    }
                    else if ((!LevelForm.Visible) && (FormCreated))
                    {
                        LevelTracker = 1;
                        // Get the path and current levels in the levels folder
                        LevelsAvailable = Directory.GetFiles(Path.GetFullPath("Levels")).Length;

                        GameState = GameState.Title;

                        // Clear the game objects and load the new level
                        ClearGameObjects();
                        InitializeGameObjects();

                        FormCreated = false;
                    }

                    break;
                case GameState.Options:                                                     // Open the options menu
                    for (int i = 0; i < OptionElements.Count; i++)
                    {
                        OptionElements[i].Update(gameTime);
                    }

                    break;
                case GameState.InGame:
                    for (int i = 0; i < (GameObject.Count - 1); i++)                        // Cycle through GameObject list and test
                    {                                                                       // all created objects for intersection
                        for (int j = i + 1; j < GameObject.Count; j++)
                        {
                            if (GameObject[i].Intersects(GameObject[j]))
                            {
                                if (!GameObject[i].intersectedBy.Contains(GameObject[j]))   // If there is an intersection, create
                                {                                                           // references in the objects to each other
                                    GameObject[i].intersectedBy.Add(GameObject[j]);
                                }

                                if (!GameObject[j].intersectedBy.Contains(GameObject[i]))
                                {
                                    GameObject[j].intersectedBy.Add(GameObject[i]);
                                }
                            }
                        }
                    }

                    for (int i = 0; i < GameObject.Count; i++)                              // Update all the objects in the game
                    {
                        GameObject[i].Update(gameTime);
                    }

                    break;
                case GameState.AdvanceLevel:                                                // If the level was completed successfully, advance
                    LevelTracker++;

                    if (LevelTracker > LevelsAvailable)
                    {
                        GameState = GameState.Title;                                        // If there are no more levels, return to the menu
                    }
                    else
                    {
                        ClearGameObjects();                                                 // If there are more levels, clear the objects and load the next level
                        InitializeGameObjects();
                        GameState = GameState.InGame;
                    }                    

                    break;
                case GameState.Pause:                                                       // If we have time
                    break;
                case GameState.GameOver:                                                    // Reset level counter, clear game objects and return to menu
                    LevelTracker = 1;
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

            // Draw background
            spriteBatch.Draw(
                BackgroundSprites["GameBackground"],
                new Vector2(0, 0),
                null,
                (Color.White * 0.5f),
                0.0f,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                0.0f
            );

            switch (GameState)                                                              // Draw elements based on state
            {
                case GameState.Title:
                    if (TitleElements.Count != 0)
                    {
                        for (int i = 0; i < TitleElements.Count; i++)
                        {
                            TitleElements[i].Draw(spriteBatch);
                        }
                    }                    

                    break;
                case GameState.Options:                                                     // Draw options menu objects
                    if (OptionElements.Count != 0)
                    {
                        for (int i = 0; i < OptionElements.Count; i++)
                        {
                            OptionElements[i].Draw(spriteBatch);
                        }
                    }
                    
                    break;
                case GameState.InGame:
                    if (GameObject.Count != 0)
                    {
                        for (int i = 0; i < GameObject.Count; i++)                          // Draw in-game objects
                        {
                            if (GameObject[i].Visible)
                            {
                                GameObject[i].Draw(spriteBatch);
                            }
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

        /// <summary>
        /// Load the image sprites
        /// </summary>
        private void LoadSprites()
        {
            // Add level sprites
            BackgroundSprites.Add("GameBackground", Content.Load<Texture2D>("Images\\Backgrounds\\GameBackground"));

            // Add player sprites
            PlayerSprites.Add("Player", Content.Load<Texture2D>("Images\\SpriteSheets\\Player_400x400"));
            //PlayerSprites.Add("Player", Content.Load<Texture2D>("Images\\TestImages\\Player_Overlay"));

            // Add player sprites
            GoalSprites.Add("BirdHouse", Content.Load<Texture2D>("Images\\SpriteSheets\\Goal_400x400"));

            // Add platform sprites
            PlatformSprites.Add("Grass", Content.Load<Texture2D>("Images\\SpriteSheets\\Grass_400x400"));
            PlatformSprites.Add("Water", Content.Load<Texture2D>("Images\\SpriteSheets\\Water_400x400"));
            PlatformSprites.Add("Stone", Content.Load<Texture2D>("Images\\SpriteSheets\\Stone_400x400"));
            PlatformSprites.Add("Wood", Content.Load<Texture2D>("Images\\SpriteSheets\\Wood_400x400"));

            // Add collectible sprites
            CollectibleSprites.Add("Seed", Content.Load<Texture2D>("Images\\SpriteSheets\\Seed_400x400"));
            CollectibleSprites.Add("Worm", Content.Load<Texture2D>("Images\\SpriteSheets\\Worm_400x400"));

            // Add enemy sprites
            EnemySprites.Add("Enemy", Content.Load<Texture2D>("Images\\SpriteSheets\\Enemy_400x400"));

            // Add random other graphics sprites            
            GeneralSprites.Add("LifeIcon", Content.Load<Texture2D>("Images\\GeneralElements\\LifeIcon"));

            // Add menu sprites
            MenuSprites.Add("MenuBackground", Content.Load<Texture2D>("Images\\Backgrounds\\MenuBackground"));
            MenuSprites.Add("Title", Content.Load<Texture2D>("Menus\\Title\\Title"));
            MenuSprites.Add("TitleSelectionFrame", Content.Load<Texture2D>("Menus\\Title\\TitleSelectionFrame"));
            MenuSprites.Add("LevelEditor", Content.Load<Texture2D>("Menus\\Title\\LevelEditor"));
            MenuSprites.Add("NewGame", Content.Load<Texture2D>("Menus\\Title\\NewGame"));
            MenuSprites.Add("Options", Content.Load<Texture2D>("Menus\\Title\\Options"));

            MenuSprites.Add("OptionsTitle", Content.Load<Texture2D>("Menus\\Options\\OptionsTitle"));
            MenuSprites.Add("Music", Content.Load<Texture2D>("Menus\\Options\\Music"));
            MenuSprites.Add("SFX", Content.Load<Texture2D>("Menus\\Options\\SFX"));
            MenuSprites.Add("OnButton", Content.Load<Texture2D>("Menus\\Options\\OnButton"));
            MenuSprites.Add("OffButton", Content.Load<Texture2D>("Menus\\Options\\OffButton"));
            MenuSprites.Add("HiText", Content.Load<Texture2D>("Menus\\Options\\HiText"));
            MenuSprites.Add("LowText", Content.Load<Texture2D>("Menus\\Options\\LowText"));
            MenuSprites.Add("SettingsBar", Content.Load<Texture2D>("Menus\\Options\\SettingsBar"));
            MenuSprites.Add("SettingsSlider", Content.Load<Texture2D>("Menus\\Options\\SettingsSlider"));
            MenuSprites.Add("OptionsSelectionFrame", Content.Load<Texture2D>("Menus\\Options\\OptionsSelectionFrame"));
        }

        /// <summary>
        /// Load the sound effects
        /// </summary>
        private void LoadSoundEffects()
        {
            // Add sound sprites and create instances of the sounds to allow manipulation
            SoundEffects.Add("JumpSound", Content.Load<SoundEffect>("Sounds\\JumpSound"));
            SoundEffectInstances.Add("JumpSound", SoundEffects["JumpSound"].CreateInstance());

            SoundEffects.Add("Error", Content.Load<SoundEffect>("Sounds\\Error"));
            SoundEffectInstances.Add("Error", SoundEffects["Error"].CreateInstance());

            SoundEffects.Add("GameOver", Content.Load<SoundEffect>("Sounds\\GameOver"));
            SoundEffectInstances.Add("GameOver", SoundEffects["GameOver"].CreateInstance());

            SoundEffects.Add("LevelComplete", Content.Load<SoundEffect>("Sounds\\LevelComplete"));
            SoundEffectInstances.Add("LevelComplete", SoundEffects["LevelComplete"].CreateInstance());

            SoundEffects.Add("MenuMove", Content.Load<SoundEffect>("Sounds\\MenuMove"));
            SoundEffectInstances.Add("MenuMove", SoundEffects["MenuMove"].CreateInstance());

            foreach (KeyValuePair<string, SoundEffectInstance> keyValuePair in SoundEffectInstances)
            {
                keyValuePair.Value.Volume = 0.5f;
                keyValuePair.Value.Pitch = 0.0f;
                keyValuePair.Value.Pan = 0.0f;
            }

            SoundEffects.Add("BackgroundMusic", Content.Load<SoundEffect>("Sounds\\BackgroundMusic"));
            SoundEffectInstances.Add("BackgroundMusic", SoundEffects["BackgroundMusic"].CreateInstance());

            SoundEffectInstances["BackgroundMusic"].Volume = 0.2f;
            SoundEffectInstances["BackgroundMusic"].Pitch = 0.0f;
            SoundEffectInstances["BackgroundMusic"].Pan = 0.0f;
            SoundEffectInstances["BackgroundMusic"].IsLooped = true;

            
        }

        /// <summary>
        /// Load the game objects from individual dictionaries into gameobjects dictionary
        /// </summary>
        private void InitializeGameObjects()
        {
            bool readSuccessful = ReadLevelFile();

            if (!readSuccessful)
            {
                Exit();
            }

            LoadBackgroundElements();
            LoadGeneralElements();

            if (TitleElements.Count == 0)
            {
                LoadMenuElements();
            }
            
            // All objects need to be added to the GameObject list
            GameObject.Add(Player);                                                         // Add player to GameObject
            GameObject.Add(Goal);

            foreach (Enemy enemy in Enemies)                                                // Add enemies to GameObject
            {
                GameObject.Add(enemy);
            }

            foreach (Platform platform in Platforms)                                        // Add platforms to GameObject
            {
                GameObject.Add(platform);
            }

            foreach (Collectible collectible in Collectibles)
            {
                GameObject.Add(collectible);
            }

            foreach (GraphicElement graphic in GameGraphics)
            {
                GameObject.Add(graphic);
            }
        }

        /// <summary>
        /// Clear all the game dictionaries
        /// </summary>
        private void ClearGameObjects()
        {
            GameObject.Clear();
            Platforms.Clear();
            Collectibles.Clear();
            Enemies.Clear();
            GameGraphics.Clear();
            LivesLeft.Clear();
        }

        /// <summary>
        /// Reads the text file containing the level information and creates the associated objects
        /// </summary>
        /// <returns>True if file read was successful and false if it failed</returns>
        private bool ReadLevelFile() {

            bool returnValue = true;

            if (File.Exists("Levels//Level" + LevelTracker + ".txt"))
            {
                ReadStream = File.OpenRead("Levels//Level" + LevelTracker + ".txt");
                myReader = new StreamReader(ReadStream);
            }
            else
            {
                returnValue = false;
            }

            if (returnValue != false)
            {
                string readString;
                string[] readLineArray;
                List<newLevelEditor.GameTile[]> gameTileList = new List<newLevelEditor.GameTile[]>();

                int platformCounter = 0;
                int enemyCounter = 0;
                int collectibleCounter = 0;

                try
                {
                    while ((readString = myReader.ReadLine()) != null)                      // Read the Level text file
                    {
                        readLineArray = readString.Split(new string[] { "|" }, StringSplitOptions.None);

                        bool gravityYesNo = false;

                        if (readLineArray[6].Equals("None") && (!readLineArray[7].Equals("None")))
                        {
                            gravityYesNo = true;
                        }

                        switch (readLineArray[0])                                           // Create appropriate object based on dictionary index
                        {
                            case "Player":
                                Player = new Player(
                                    PlayerSprites[readLineArray[1]],
                                    spritesInSheet: 4,
                                    slidesToCycle: 3,
                                    x: ParseToInt(readLineArray[1], 2, readLineArray[2]),
                                    y: ParseToInt(readLineArray[1], 3, readLineArray[3]),
                                    width: ParseToInt(readLineArray[1], 4, readLineArray[4]),
                                    height: ParseToInt(readLineArray[1], 5, readLineArray[5]),
                                    addGravity: true
                                 );

                                PlayerSpawnPoint = new Vector2(
                                                            ParseToInt(readLineArray[1], 2, readLineArray[2]),
                                                            ParseToInt(readLineArray[1], 3, readLineArray[3])
                                                        );

                                break;
                            case "Platform":
                                Platforms.Add(
                                    new Platform(
                                        platformType: readLineArray[1],
                                        spriteTexture: PlatformSprites[readLineArray[1]],
                                        x: ParseToInt(readLineArray[1], 2, readLineArray[2]),
                                        y: ParseToInt(readLineArray[1], 3, readLineArray[3]),
                                        width: ParseToInt(readLineArray[1], 4, readLineArray[4]),
                                        height: ParseToInt(readLineArray[1], 5, readLineArray[5])
                                    )
                                );


                                // Convert the string values from the text file back to their enum datatypes
                                Platforms[platformCounter].ApplyGravity = gravityYesNo;
                                Platforms[platformCounter].GravityOnProximityFrom = ProximityInterpreter(readLineArray[6]);
                                Platforms[platformCounter].ObjectMovement = MovementInterpreter(readLineArray[7]);
                                Platforms[platformCounter].GravityDirection = GravityInterpreter(readLineArray[8]);
                                Platforms[platformCounter].ObjectXMoveDistance = MoveDistanceInterpreter(
                                                                                        "X",
                                                                                        readLineArray[8],
                                                                                        ParseToInt(
                                                                                            readLineArray[1],
                                                                                            9,
                                                                                            readLineArray[9]
                                                                                        )
                                                                                      );
                                Platforms[platformCounter].ObjectYMoveDistance = MoveDistanceInterpreter(
                                                                                        "Y",
                                                                                        readLineArray[8],
                                                                                        ParseToInt(
                                                                                            readLineArray[1],
                                                                                            9,
                                                                                            readLineArray[9]
                                                                                        )
                                                                                      );

                                platformCounter++;

                                break;
                            case "Goal":
                                Goal = new Goal(
                                    GoalSprites[readLineArray[1]],
                                    spritesInSheet: 2,
                                    x: ParseToInt(readLineArray[1], 2, readLineArray[2]),
                                    y: ParseToInt(readLineArray[1], 3, readLineArray[3]),
                                    width: ParseToInt(readLineArray[1], 4, readLineArray[4]),
                                    height: ParseToInt(readLineArray[1], 5, readLineArray[5]),
                                    addGravity: false
                                 );
                                
                                break;
                            case "Enemy":
                                Enemies.Add(
                                    new Enemy(
                                        spriteTexture: EnemySprites[readLineArray[1]],
                                        spritesInSheet: 4,
                                        x: ParseToInt(readLineArray[1], 2, readLineArray[2]),
                                        y: ParseToInt(readLineArray[1], 3, readLineArray[3]),
                                        width: ParseToInt(readLineArray[1], 4, readLineArray[4]),
                                        height: ParseToInt(readLineArray[1], 5, readLineArray[5]),
                                        addGravity: true
                                    )
                                );

                                Enemies[enemyCounter].ApplyGravity = gravityYesNo;
                                Enemies[enemyCounter].GravityOnProximityFrom = ProximityInterpreter(readLineArray[6]);
                                Enemies[enemyCounter].ObjectMovement = MovementInterpreter(readLineArray[7]);
                                Enemies[enemyCounter].GravityDirection = GravityInterpreter(readLineArray[8]);
                                Enemies[enemyCounter].ObjectXMoveDistance = MoveDistanceInterpreter(
                                                                                        "X",
                                                                                        readLineArray[8],
                                                                                        ParseToInt(
                                                                                            readLineArray[1],
                                                                                            9,
                                                                                            readLineArray[9]
                                                                                        )
                                                                                      );
                                Enemies[enemyCounter].ObjectYMoveDistance = MoveDistanceInterpreter(
                                                                                        "Y",
                                                                                        readLineArray[8],
                                                                                        ParseToInt(
                                                                                            readLineArray[1],
                                                                                            9,
                                                                                            readLineArray[9]
                                                                                        )
                                                                                      );

                                enemyCounter++;

                                break;
                            case "Collectible":
                                Collectibles.Add(
                                    new Collectible(
                                        collectibleType: readLineArray[1],
                                        spriteTexture: CollectibleSprites[readLineArray[1]],
                                        spritesInSheet: 4,
                                        x: ParseToInt(readLineArray[1], 2, readLineArray[2]),
                                        y: ParseToInt(readLineArray[1], 3, readLineArray[3]),
                                        width: ParseToInt(readLineArray[1], 4, readLineArray[4]),
                                        height: ParseToInt(readLineArray[1], 5, readLineArray[5]),
                                        addGravity: true
                                    )
                                );


                                // Convert the string values from the text file back to their enum datatypes
                                Collectibles[collectibleCounter].ApplyGravity = gravityYesNo;
                                Collectibles[collectibleCounter].GravityOnProximityFrom = ProximityInterpreter(readLineArray[6]);
                                Collectibles[collectibleCounter].ObjectMovement = MovementInterpreter(readLineArray[7]);
                                Collectibles[collectibleCounter].GravityDirection = GravityInterpreter(readLineArray[8]);
                                Collectibles[collectibleCounter].ObjectXMoveDistance = MoveDistanceInterpreter(
                                                                                        "X",
                                                                                        readLineArray[8],
                                                                                        ParseToInt(
                                                                                            readLineArray[1],
                                                                                            9,
                                                                                            readLineArray[9]
                                                                                        )
                                                                                      );
                                Collectibles[collectibleCounter].ObjectYMoveDistance = MoveDistanceInterpreter(
                                                                                        "Y",
                                                                                        readLineArray[8],
                                                                                        ParseToInt(
                                                                                            readLineArray[1],
                                                                                            9,
                                                                                            readLineArray[9]
                                                                                        )
                                                                                      );

                                collectibleCounter++;

                                break;
                        }
                    }
                }
                catch (InvalidDataException myException)
                {
                    DialogResult messageBoxResult = MessageBox.Show(
                                                        myException.ToString(),
                                                        "Error while reading level file",
                                                        MessageBoxButtons.OK,
                                                        MessageBoxIcon.Error
                                                    );

                    if (messageBoxResult == DialogResult.OK)
                    {
                        returnValue = false;
                    }
                } finally
                {
                    myReader.Close();
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Converts a string value to an int value.
        /// 
        /// Provides exception information pertaining to which object is causing the error as well
        /// as any other useful information pertaining to the exception
        /// </summary>
        /// <param name="callingObject">Object calling the method</param>
        /// <param name="callingIndex">Index of the item in the array causing the exception</param>
        /// <param name="valueToParse">The actually parsed value</param>
        /// <returns>The value parsed from string to int</returns>
        private int ParseToInt(string callingObject, int callingIndex, string valueToParse)
        {
            int returnValue = 0;            

            try
            {
                returnValue = int.Parse(valueToParse);
            }
            catch (FormatException)
            {
                StringBuilder exceptionString = new StringBuilder();

                exceptionString.Append("\n");
                exceptionString.Append("Calling Object              : " + callingObject + "\n");
                exceptionString.Append("Calling Index               : " + callingIndex + "\n");
                exceptionString.Append("Value to Parse to string    : " + valueToParse);

                throw new InvalidDataException(exceptionString.ToString());                
            }

            return returnValue;
        }

        /// <summary>
        /// The values read from the input file are of type string.  The values that represent
        /// enums need to be converted from strings back to enums.
        /// 
        /// - This method converts the GravityOnProximityFrom
        /// 
        /// </summary>
        /// <param name="gravityDirection">Dynamics of how the object should move based on proximity of type string</param>
        /// <returns>Dynamics of how the object should move based on proximity of type GravityOnProximityFrom (ENUM)</returns>
        private GravityOnProximityFrom ProximityInterpreter(string proximity)
        {
            GravityOnProximityFrom returnValue = GravityOnProximityFrom.None;

            switch (proximity)
            {
                case "Left":
                    returnValue = GravityOnProximityFrom.Left;

                    break;
                case "Right":
                    returnValue = GravityOnProximityFrom.Right;

                    break;
                case "Top":
                    returnValue = GravityOnProximityFrom.Top;

                    break;
                case "Bottom":
                    returnValue = GravityOnProximityFrom.Bottom;

                    break;
                case "Center":
                    returnValue = GravityOnProximityFrom.Center;

                    break;
            }

            return returnValue;
        }

        /// <summary>
        /// The values read from the input file are of type string.  The values that represent
        /// enums need to be converted from strings back to enums.
        /// 
        /// - This method converts the ObjectMovement
        /// 
        /// </summary>
        /// <param name="gravityDirection">Dynamics of how the object should move of type string</param>
        /// <returns>Dynamics of how the object should move of type ObjectMovement (ENUM)</returns>
        private ObjectMovement MovementInterpreter(string movement)
        {
            ObjectMovement returnValue = ObjectMovement.OneDirection;

            switch (movement)
            {
                case "ToAndFroUpFirst":
                    returnValue = ObjectMovement.ToAndFroUpFirst;

                    break;
                case "ToAndFroDownFirst":
                    returnValue = ObjectMovement.ToAndFroDownFirst;

                    break;
                case "ToAndFroLeftFirst":
                    returnValue = ObjectMovement.ToAndFroLeftFirst;

                    break;
                case "ToAndFroRightFirst":
                    returnValue = ObjectMovement.ToAndFroRightFirst;

                    break;
            }

            return returnValue;
        }

        /// <summary>
        /// The values read from the input file are of type string.  The values that represent
        /// enums need to be converted from strings back to enums.
        /// 
        /// - This method converts the GravityDirection
        /// 
        /// </summary>
        /// <param name="gravityDirection">Direction in which gravity needs to be applied of type string</param>
        /// <returns>Direction in which gravity needs to be applied of type GravityDirection (ENUM)</returns>
        private GravityDirection GravityInterpreter(string gravityDirection)
        {
            GravityDirection returnValue = GravityDirection.Down;

            switch (gravityDirection)
            {
                case "Left":
                    returnValue = GravityDirection.Left;

                    break;
                case "Right":
                    returnValue = GravityDirection.Right;

                    break;
                case "Up":
                    returnValue = GravityDirection.Up;

                    break;
            }

            return returnValue;
        }

        /// <summary>
        /// Converts the distance to move read from file in number of blocks, to distance in pixels
        /// </summary>
        /// <param name="cartesianPlane">The X-Y plane which the conversion is for</param>
        /// <param name="direction">The direction in which the object should move (UP/DOWN/LEFT/RIGHT)</param>
        /// <param name="distance">The distance the object should move in number of blocks (50 pixels each)</param>
        /// <returns>The distance the object should move in Pixels</returns>
        private int MoveDistanceInterpreter(string cartesianPlane, string direction, int distance)
        {
            int returnValue = 0;

            switch (cartesianPlane)
            {
                case "X":
                    if (direction == "Right" || direction == "Left")
                    {
                        returnValue = distance * 50;
                    }

                    break;
                case "Y":
                    if (direction == "Up" || direction == "Down")
                    {
                        returnValue = distance * 50;
                    }

                    break;
            }

            return returnValue;
        }

        /// <summary>
        /// If there are no levels created, create temporary levels
        /// </summary>
        private void CreateTempLevels()
        {
            try
            {
                for (int i = 0; i < 3; i++)
                {
                    FileStream WriteStream = File.OpenWrite("Levels//Level" + (i + 1) + ".txt");
                    MyWriter = new StreamWriter(WriteStream);

                    string tempString = null;

                    switch (i)
                    {
                        case 0:
                            tempString = Resources.Level1;

                            break;
                        case 1:
                            tempString = Resources.Level2;

                            break;
                        case 2:
                            tempString = Resources.Level3;

                            break;
                    }
                    string[] tempArray = tempString.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                    foreach (string readString in tempArray)
                    {
                        MyWriter.WriteLine(readString);
                    }

                    MyWriter.Close();
                }

                LevelsAvailable = Directory.GetFiles(Path.GetFullPath("Levels")).Length;
            }
            catch (Exception)
            {
                Console.WriteLine("Error received while trying to create temporary Level1.txt from Resources.Level1");
            }
            finally
            {
                if (MyWriter.BaseStream != null)
                {
                    MyWriter.Close();
                }
            }
        }

        /// <summary>
        /// Load game background
        /// </summary>
        private void LoadBackgroundElements()
        {
            new GraphicElement(
                elementName: "GameBackground",
                spriteTexture: BackgroundSprites["GameBackground"],
                x: 0,
                y: 0,
                width: SCREENWIDTH,
                height: SCREENHEIGHT
            );
        }

        /// <summary>
        /// Load life icons
        /// </summary>
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

        /// <summary>
        /// Load title menu elements
        /// </summary>
        private void LoadMenuElements()
        {
// Title menu starts here
            TitleElements.Add(
                new Title(
                    menuItem: "MenuBackground",
                    spriteTexture: MenuSprites["MenuBackground"],
                    spritesInSheet: 1,
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
                    spritesInSheet: 1,
                    x: (SCREENWIDTH / 2) - 300,
                    y: (SCREENHEIGHT / 2) - 400,
                    width: 600,
                    height: 200,
                    addGravity: false
                )
            );

            TitleElements.Add(
                new Title(
                    menuItem: "NewGame",
                    spriteTexture: MenuSprites["NewGame"],
                    spritesInSheet: 1,
                    x: (SCREENWIDTH / 2) - 200,
                    y: (SCREENHEIGHT / 2) - 100,
                    width: 400,
                    height: 100,
                    addGravity: false
                )
            );

            TitleElements.Add(
                new Title(
                    menuItem: "LevelEditor",
                    spriteTexture: MenuSprites["LevelEditor"],                    
                    spritesInSheet: 1,
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
                    spritesInSheet: 1,
                    x: (SCREENWIDTH / 2) - 200,
                    y: (SCREENHEIGHT / 2) + 200,
                    width: 400,
                    height: 100,
                    addGravity: false
                )
            );

            TitleElements.Add(
                new Title(
                    menuItem: "TitleSelectionFrame",
                    spriteTexture: MenuSprites["TitleSelectionFrame"],
                    spritesInSheet: 1,
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
                    spritesInSheet: 1,
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
                    spritesInSheet: 1,
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
                    spritesInSheet: 1,
                    x: (SCREENWIDTH / 2) - 200,
                    y: (SCREENHEIGHT / 2) - 50,
                    width: 200,
                    height: 80,
                    addGravity: false
                )
            );

            OptionElements.Add(
                new Option(
                    menuItem: "MusicOff",
                    spriteTexture: MenuSprites["LowText"],
                    spritesInSheet: 1,
                    x: (SCREENWIDTH / 2) + 90,
                    y: (SCREENHEIGHT / 2) - 20,
                    width: 75,
                    height: 30,
                    addGravity: false
                )
            );

            OptionElements.Add(
                new Option(
                    menuItem: "MusicOn",
                    spriteTexture: MenuSprites["HiText"],
                    spritesInSheet: 1,
                    x: (SCREENWIDTH / 2) + 300,
                    y: (SCREENHEIGHT / 2) - 20,
                    width: 40,
                    height: 30,
                    addGravity: false
                )
            );

            OptionElements.Add(
                new Option(
                    menuItem: "MusicBar",
                    spriteTexture: MenuSprites["SettingsBar"],
                    spritesInSheet: 1,
                    x: (SCREENWIDTH / 2) + 175,
                    y: (SCREENHEIGHT / 2) - 10,
                    width: 110,
                    height: 10,
                    addGravity: false
                )
            );

            OptionElements.Add(
                new Option(
                    menuItem: "MusicSlider",
                    spriteTexture: MenuSprites["SettingsSlider"],
                    spritesInSheet: 1,
                    x: (SCREENWIDTH / 2) + 200,
                    y: (SCREENHEIGHT / 2) - 26,
                    width: 10,
                    height: 40,
                    addGravity: false
                )
            );

            OptionElements.Add(
                new Option(
                    menuItem: "SFX",
                    spriteTexture: MenuSprites["SFX"],
                    spritesInSheet: 1,
                    x: (SCREENWIDTH / 2) - 200,
                    y: (SCREENHEIGHT / 2) + 50,
                    width: 200,
                    height: 80,
                    addGravity: false
                )
            );

            OptionElements.Add(
                new Option(
                    menuItem: "SFXOff",
                    spriteTexture: MenuSprites["LowText"],
                    spritesInSheet: 1,
                    x: (SCREENWIDTH / 2) + 90,
                    y: (SCREENHEIGHT / 2) + 70,
                    width: 75,
                    height: 30,
                    addGravity: false
                )
            );

            OptionElements.Add(
                new Option(
                    menuItem: "SFXOn",
                    spriteTexture: MenuSprites["HiText"],
                    spritesInSheet: 1,
                    x: (SCREENWIDTH / 2) + 300,
                    y: (SCREENHEIGHT / 2) + 70,
                    width: 40,
                    height: 30,
                    addGravity: false
                )
            );

            OptionElements.Add(
                new Option(
                    menuItem: "SFXBar",
                    spriteTexture: MenuSprites["SettingsBar"],
                    spritesInSheet: 1,
                    x: (SCREENWIDTH / 2) + 175,
                    y: (SCREENHEIGHT / 2) + 80,
                    width: 110,
                    height: 10,
                    addGravity: false
                )
            );

            OptionElements.Add(
                new Option(
                    menuItem: "SFXSlider",
                    spriteTexture: MenuSprites["SettingsSlider"],
                    spritesInSheet: 1,
                    x: (SCREENWIDTH / 2) + 230,
                    y: (SCREENHEIGHT / 2) + 64,
                    width: 10,
                    height: 40,
                    addGravity: false
                )
            );

            OptionElements.Add(
                new Option(
                    menuItem: "OptionsSelectionFrame",
                    spriteTexture: MenuSprites["OptionsSelectionFrame"],
                    spritesInSheet: 1,
                    x: (SCREENWIDTH / 2) - 220,
                    y: (SCREENHEIGHT / 2) - 65,
                    width: 240,
                    height: 100,
                    addGravity: false
                )
            );
        }
    }
}