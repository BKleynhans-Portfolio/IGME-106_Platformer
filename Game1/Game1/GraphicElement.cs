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
/// Filename            : Obstacle.cs
/// </summary>

namespace Game1
{
    public class GraphicElement : Environment
    {   
        public string Name { get; set; }

        /// <summary>
        /// Default constructor.  Creates a GameObject with default values.
        /// </summary>
        /// <param name="spriteTexture">Texture2D image for object</param>
        /// <param name="x">Starting X coordinate of object</param>
        /// <param name="y">Starting Y coordinate of object</param>
        /// <param name="width">Width of object</param>
        /// <param name="height">Height of object</param>
        public GraphicElement(string elementName, Texture2D spriteTexture, int x, int y, int width, int height) : base(spriteTexture, x, y, width, height)
        {
            this.Name = elementName;

            if (elementName.Equals("LifeIcon"))
            {
                LivesLeft.Add(this);
            }
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
        public GraphicElement(string elementName, Texture2D spriteTexture, int SpritesInSheet, int x, int y, int width, int height,
                          bool addGravity) :
                base(spriteTexture, SpritesInSheet, x, y, width, height, addGravity)
        {            
            this.Name = elementName;

            if (elementName.Equals("LifeIcon"))
            {
                LivesLeft.Add(this);
            }
        }

        protected override void Update(GameTime gameTime) { }
    }
}
