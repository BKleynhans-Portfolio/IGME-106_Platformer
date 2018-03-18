﻿using System;
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
/// Created By          : Cullen Sullivan
/// Creation Date       : March 7, 2018
/// Authors             : Benjamin Kleynhans
///                       
///                       
///                       
/// Last Modified By    : Benjamin Kleynhans
/// Last Modified Date  : March 7, 2018
/// Filename            : Ground1.cs
/// </summary>

namespace Game1
{
    class Ground : Enemy
    {
        public Ground(Texture2D texture2D, int xCoord, int yCoord, int width, int height) :
                 base(texture2D, xCoord, yCoord, width, height)
        {

        }

        public override void Move()
        {

        }

        public override void Draw()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                rectangle,
                ObjectColor
            );
        }
    }
}