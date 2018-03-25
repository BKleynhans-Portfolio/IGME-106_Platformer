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
/// Filename            : NPC.cs
/// </summary>

namespace Game1
{
    abstract class NPC : Character
    {    
        public NPC(Texture2D spriteTexture, int x, int y, int width, int height) : base(spriteTexture, x, y, width, height)
        {

        }

        public NPC(Texture2D spriteTexture, int x, int y, int width, int height,
                          bool addGravity, float appliedObjectMass) :
                base(spriteTexture, x, y, width, height, addGravity, appliedObjectMass)
        {

        }

        public override Vector2 ApplyMovement()
        {
            Vector2 returnValue;
            
            base.CalculateGravity();
            base.CalculateMovement();

            returnValue = new Vector2(
                this.Rectangle.X + base.AVelocity,
                this.Rectangle.Y + base.GVelocity
            );

            return returnValue;
        }
    }
}
