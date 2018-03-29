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
        /// <param name="appliedObjectMass">This is the mass that should be applied to the object</param>
        public Title(string menuItem, Texture2D spriteTexture, int x, int y, int width, int height,
                          bool addGravity, float appliedObjectMass) :
                base(spriteTexture, x, y, width, height, addGravity, appliedObjectMass)
        {
            titleMenuDictionary.Add(menuItem, new Vector2(x, y));
            this.Name = menuItem;
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (this.Name == "SelectionFrame")
            {
                string currentPosition = null;

                foreach (KeyValuePair<string, Vector2> keyValuePair in titleMenuDictionary)
                {
                    if ((
                            (keyValuePair.Value.X == this.Rectangle.X)
                        ) && (
                            (keyValuePair.Value.Y == this.Rectangle.Y)
                        ) && (
                            (keyValuePair.Key != "SelectionFrame")
                        ))
                    {
                        currentPosition = keyValuePair.Key;
                    }
                }

                if (currentKeyboardState.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down))                    
                {
                    switch (currentPosition)
                    {
                        case "LoadGame":
                            base.CreateRectangle(titleMenuDictionary["NewGame"]);

                            break;
                        case "NewGame":
                            base.CreateRectangle(titleMenuDictionary["Options"]);

                            break;
                    }
                }
                else if (currentKeyboardState.IsKeyDown(Keys.Up) && previousKeyboardState.IsKeyUp(Keys.Up))
                {
                    switch (currentPosition)
                    {
                        case "NewGame":
                            base.CreateRectangle(titleMenuDictionary["LoadGame"]);

                            break;
                        case "Options":
                            base.CreateRectangle(titleMenuDictionary["NewGame"]);

                            break;
                    }
                }
                else if (currentKeyboardState.IsKeyDown(Keys.Enter) && previousKeyboardState.IsKeyUp(Keys.Enter))
                {
                    switch (currentPosition)
                    {
                        case "LoadGame":
                            

                            break;
                        case "NewGame":
                            gameState = GameState.InGame;

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
