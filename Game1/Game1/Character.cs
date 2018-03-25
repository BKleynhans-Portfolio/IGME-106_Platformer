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
/// Last Modified Date  : March 25, 2018
/// Filename            : Character.cs
/// </summary>

namespace Game1
{
    abstract class Character : GameObject
    {
        public abstract override void Draw(SpriteBatch spriteBatch);
        protected abstract override void Update(GameTime gameTime);

        protected abstract void Die();

        private bool isAlive;
        private bool hasJumped;

        public Character(Texture2D spriteTexture, int x, int y, int width, int height) : base(spriteTexture, x, y, width, height)
        {
            this.HasJumped = false;
            base.JumpInProgress = false;
        }

        public Character(Texture2D spriteTexture, int x, int y, int width, int height,
                          bool addGravity, float appliedObjectMass) :
                base(spriteTexture, x, y, width, height, addGravity, appliedObjectMass)
        {
            this.HasJumped = false;
            base.JumpInProgress = false;
        }

        public bool IsAlive
        {
            get { return this.isAlive; }
            set { this.isAlive = value; }
        }

        public bool HasJumped
        {
            get { return this.hasJumped; }
            set { this.hasJumped = value; }
        }

        public virtual Vector2 ApplyMovement()
        {
            Vector2 returnValue;

            CalculateGravity();
            CalculateMovement();

            returnValue = new Vector2(
                this.Rectangle.X + base.MovementVelocity,
                this.Rectangle.Y + base.GravitationalVelocity
            );

            return returnValue;
        }

        public override void CalculateGravity()
        {
            if (this.ApplyGravity)
            {
                switch (gravityDirection)
                {
                    case GravityDirection.Up:
                        

                        break;
                    case GravityDirection.Down:
                        if (hitObstacle == HitObstacle.FromTop)
                        {
                            base.Falling = false;
                            this.GravitationalVelocity = 0;                            
                        }
                        else if (!HasJumped)
                        {
                            this.GravitationalVelocity += this.Acceleration;
                        }

                        break;
                    case GravityDirection.Left:

                        break;
                    case GravityDirection.Right:
                        
                        break;
                }
            }
        }

        public void CalculateMovement()
        {
            if (movementAppliedTo == MovementAppliedTo.None)
            {
                this.MovementVelocity = 0;
            }

            if ((movementAppliedTo == MovementAppliedTo.Left) && (hitObstacle != HitObstacle.FromLeft))            
            {
                this.MovementVelocity = -DefaultHorizonalVelocity;
            }

            if ((movementAppliedTo == MovementAppliedTo.Right) && (hitObstacle != HitObstacle.FromRight)) 
            {
                this.MovementVelocity = DefaultHorizonalVelocity;
            }

            if ((movementAppliedTo == MovementAppliedTo.Up) && (hitObstacle != HitObstacle.FromTop))
            {
                 if ((HasJumped) && (GravitationalVelocity == 0))
                {
                    this.GravitationalVelocity -= DefaultVerticalVelocity;
                }
                else if ((HasJumped) && (GravitationalVelocity > -5))                    
                {
                    this.GravitationalVelocity -= this.Acceleration;
                }
                else if ((HasJumped) && (GravitationalVelocity <= -5))
                {
                    this.GravitationalVelocity += this.Acceleration;
                    HasJumped = false;
                }
                else if ((!HasJumped) && (JumpInProgress) && (GravitationalVelocity <= -5))
                {
                    this.GravitationalVelocity += this.Acceleration;
                }
            }
        }        
    }
}
