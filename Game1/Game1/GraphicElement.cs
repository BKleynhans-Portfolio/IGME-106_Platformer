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
        private string name;

        public GraphicElement(string elementName, Texture2D spriteTexture, int x, int y, int width, int height) : base(spriteTexture, x, y, width, height)
        {
            this.Name = elementName;

            if (elementName.Equals("LifeIcon"))
            {
                LivesLeft.Add(this);
            }
        }

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

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        protected override void Update(GameTime gameTime)
        {

        }


    }
}
