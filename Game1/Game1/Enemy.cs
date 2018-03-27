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
/// Filename            : Enemy.cs
/// </summary>

namespace Game1
{
    class Enemy : NPC
    {
        /// <summary>
        /// Default constructor.  Creates a GameObject with default values.
        /// </summary>
        /// <param name="spriteTexture">Texture2D image for object</param>
        /// <param name="x">Starting X coordinate of object</param>
        /// <param name="y">Starting Y coordinate of object</param>
        /// <param name="width">Width of object</param>
        /// <param name="height">Height of object</param>
        public Enemy(Texture2D spriteTexture, int x, int y, int width, int height) : base(spriteTexture, x, y, width, height)
        {
            base.IsAlive = true;
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
        public Enemy(Texture2D spriteTexture, int x, int y, int width, int height,
                          bool addGravity, float appliedObjectMass) :
                base(spriteTexture, x, y, width, height, addGravity, appliedObjectMass)
        {
            base.IsAlive = true;
        }

        protected override void Die()
        {
            // I have died
        }

        protected override void Update(GameTime gameTime)
        {
            if ((this.Rectangle.Y + this.Rectangle.Height) > SCREENHEIGHT)
            {
                base.IsAlive = false;
            }

            if (this.IsAlive)
            {
                for (int i = 0; i < base.intersectedBy.Count; i++)
                {
                    bool stillIntersecting = this.Intersects(intersectedBy[i]);

                    if ((!stillIntersecting) && (this.HasJumped == false))
                    {
                        if (base.intersectedBy[i].GetType() == typeof(Platform))
                        {
                            base.Falling = true;
                            base.hitObstacle = HitObstacle.None;
                        }

                        base.intersectedBy.Remove(intersectedBy[i]);
                    }
                    else if ((!stillIntersecting) & (this.HasJumped == true))
                    {
                        base.hitObstacle = HitObstacle.None;

                        base.intersectedBy.Remove(intersectedBy[i]);
                    }
                }

                CreateRectangle(ApplyMovement());
            }
            else
            {
                Die();
            }
        }        
    }
}
