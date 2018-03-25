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

        //private int jumpDuration;                                                           // In seconds

        public Character(Texture2D spriteTexture, int x, int y, int width, int height) : base(spriteTexture, x, y, width, height)
        {
            this.HasJumped = false;
            base.JumpInProgress = false;

            //this.JumpDuration = 2;

        }

        public Character(Texture2D spriteTexture, int x, int y, int width, int height,
                          bool addGravity, float appliedMoveForce, float appliedVerticalMovementForce,
                          float appliedGravitationalAcceleration, float appliedObjectMass) :
                base(spriteTexture, x, y, width, height, addGravity, appliedMoveForce, appliedVerticalMovementForce,
                    appliedGravitationalAcceleration, appliedObjectMass)
        {
            this.HasJumped = false;
            base.JumpInProgress = false;

            //this.JumpDuration = 2;
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

        //public int JumpDuration
        //{
        //    get { return this.jumpDuration; }
        //    set { this.jumpDuration = value; }
        //}

        public virtual Vector2 ApplyMovement()
        {
            Vector2 returnValue;

            CalculateGravity();
            CalculateMovement();

            returnValue = new Vector2(
                this.Rectangle.X + base.AVelocity,
                this.Rectangle.Y + base.GVelocity
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
                            this.GVelocity = 0;                            
                        }
                        else if (!HasJumped)
                        {
                            this.GVelocity += this.Acceleration;
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
                this.AVelocity = 0;
            }

            if ((movementAppliedTo == MovementAppliedTo.Left) && (hitObstacle != HitObstacle.FromLeft))            
            {
                this.AVelocity = -DefaultHorizonalVelocity;
            }

            if ((movementAppliedTo == MovementAppliedTo.Right) && (hitObstacle != HitObstacle.FromRight)) 
            {
                this.AVelocity = DefaultHorizonalVelocity;
            }

            if ((movementAppliedTo == MovementAppliedTo.Up) && (hitObstacle != HitObstacle.FromTop))
            {
                 if ((HasJumped) && (GVelocity == 0))
                {
                    this.GVelocity -= DefaultVerticalVelocity;
                }
                else if ((HasJumped) && (GVelocity > -5))                    
                {
                    this.GVelocity -= this.Acceleration;
                }
                else if ((HasJumped) && (GVelocity <= -5))
                {
                    this.GVelocity += this.Acceleration;
                    HasJumped = false;
                }
                else if ((!HasJumped) && (JumpInProgress) && (GVelocity <= -5))
                {
                    this.GVelocity += this.Acceleration;
                }
            }
        }        
    }
}
