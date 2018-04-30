using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
/// Filename            : Title.cs
/// </summary>

namespace Game1
{
    public class Title : Menu
    {
        private static Dictionary<string, Vector2> titleMenuDictionary = new Dictionary<string, Vector2>();

        private string name;

        /// <summary>
        /// Default constructor.  Creates a GameObject with default values.
        /// </summary>
        /// <param name="spriteTexture">Texture2D image for object</param>
        /// <param name="x">Starting X coordinate of object</param>
        /// <param name="y">Starting Y coordinate of object</param>
        /// <param name="width">Width of object</param>
        /// <param name="height">Height of object</param>
        public Title(string menuItem, Texture2D spriteTexture, int x, int y, int width, int height) : base(spriteTexture, x, y, width, height)
        {
            titleMenuDictionary.Add(menuItem, new Vector2(x, y));
            this.Name = menuItem;
        }

        /// <summary>
        /// This is a secondary constructor for the GameObject.
        /// </summary>
        /// <param name="spriteTexture">Texture2D image for object</param>
        /// <param name="x">Starting X coordinate of object</param>
        /// <param name="y">Starting Y coordinate of object</param>
        /// <param name="width">Width of object</param>
        /// <param name="height">Height of object</param>
        /// <param name="addGravity">Does this object require immediate gravity implementation</param>
        public Title(string menuItem, Texture2D spriteTexture, int spritesInSheet, int x, int y, int width, int height,
                          bool addGravity) :
                base(spriteTexture, spritesInSheet, x, y, width, height, addGravity)
        {
            titleMenuDictionary.Add(menuItem, new Vector2(x, y));
            this.Name = menuItem;
        }

        /// <summary>
        /// Name/Key of the title element being added
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        
        /// <summary>
        /// Update method
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            if (this.Name == "TitleSelectionFrame")
            {
                string currentPosition = null;

                foreach (KeyValuePair<string, Vector2> keyValuePair in titleMenuDictionary) // Find the current position of the menu selection image
                {
                    if ((
                            (keyValuePair.Value.X == this.DrawLocation.X)
                        ) && (
                            (keyValuePair.Value.Y == this.DrawLocation.Y)
                        ) && (
                            (keyValuePair.Key != "TitleSelectionFrame")
                        ))
                    {
                        currentPosition = keyValuePair.Key;
                    }
                }

                // Move the image down if the down key is pressed
                if (CurrentKeyboardState.IsKeyDown(Keys.Down) && PreviousKeyboardState.IsKeyUp(Keys.Down))                    
                {
                    SoundEffectInstances["MenuMove"].Play();

                    switch (currentPosition)
                    {
                        case "NewGame":
                            base.CreateRectangle(titleMenuDictionary["LevelEditor"]);

                            break;
                        case "LevelEditor":
                            base.CreateRectangle(titleMenuDictionary["Options"]);

                            break;
                    }
                }// Move the image up if the up key is pressed
                else if (CurrentKeyboardState.IsKeyDown(Keys.Up) && PreviousKeyboardState.IsKeyUp(Keys.Up))
                {
                    SoundEffectInstances["MenuMove"].Play();

                    switch (currentPosition)
                    {
                        case "LevelEditor":
                            base.CreateRectangle(titleMenuDictionary["NewGame"]);

                            break;
                        case "Options":
                            base.CreateRectangle(titleMenuDictionary["LevelEditor"]);

                            break;
                    }
                }// Go into the menu if the enter key is pressed
                else if (CurrentKeyboardState.IsKeyDown(Keys.Enter) && PreviousKeyboardState.IsKeyUp(Keys.Enter))
                {
                    switch (currentPosition)
                    {
                        case "NewGame":
                            gameState = GameState.InGame;

                            break;
                        case "LevelEditor":
                            gameState = GameState.LevelEditor;
                            
                            break;
                        case "Options":
                            gameState = GameState.Options;

                            break;
                    }
                }
            }
        }
    }
}
