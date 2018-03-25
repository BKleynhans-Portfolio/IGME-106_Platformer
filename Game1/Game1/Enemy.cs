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

        public Enemy(Texture2D spriteTexture, int x, int y, int width, int height) : base(spriteTexture, x, y, width, height)
        {
            base.IsAlive = true;
        }

        public Enemy(Texture2D spriteTexture, int x, int y, int width, int height,
                          bool addGravity, float appliedObjectMass) :
                base(spriteTexture, x, y, width, height, addGravity, appliedObjectMass)
        {
            base.IsAlive = true;
        }

        protected override void Die()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if ((this.Rectangle.Y + this.Rectangle.Height) > screenHeight)
            {
                this.IsAlive = false;
            }

            if (this.IsAlive)
            {
                for (int i = 0; i < base.intersectedBy.Count; i++)
                {
                    bool stillIntersecting = base.Intersects(intersectedBy[i]);

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

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(                                                   // Draw the sprite from the spriteBatch
                base.ObjectTexture,
                base.Rectangle,
                Color.White
            );
        }
    }
}
