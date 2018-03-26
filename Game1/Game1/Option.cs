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
/// Filename            : Option.cs
/// </summary>

namespace Game1
{
    class Option : Menu
    {
        public Option(Texture2D texture2D, int x, int y, int width, int height) : base(texture2D, x, y, width, height)
        {

        }

        protected override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
