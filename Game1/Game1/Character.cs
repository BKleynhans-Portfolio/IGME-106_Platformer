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
        protected abstract override void Update(GameTime gameTime);

        protected abstract void Die();

        private bool isAlive;
        private bool wasAlive;
        private bool hasJumped;

        /// <summary>
        /// Default constructor.  Creates a GameObject with default values.
        /// </summary>
        /// <param name="spriteTexture">Texture2D image for object</param>
        /// <param name="x">Starting X coordinate of object</param>
        /// <param name="y">Starting Y coordinate of object</param>
        /// <param name="width">Width of object</param>
        /// <param name="height">Height of object</param>
        public Character(Texture2D spriteTexture, int x, int y, int width, int height) : base(spriteTexture, x, y, width, height)
        {
            this.HasJumped = false;
            base.JumpInProgress = false;
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
        public Character(Texture2D spriteTexture, int x, int y, int width, int height,
                          bool addGravity, float appliedObjectMass) :
                base(spriteTexture, x, y, width, height, addGravity, appliedObjectMass)
        {
            this.HasJumped = false;
            base.JumpInProgress = false;
        }

        /// <summary>
        /// Properties for variable which indicates whether the character is alive or not
        /// </summary>
        public bool IsAlive
        {
            get { return this.isAlive; }
            set { this.isAlive = value; }
        }

        public bool WasAlive
        {
            get { return this.wasAlive; }
            set { this.wasAlive = value; }
        }

        /// <summary>
        /// Properties for variable keeps track of whether the player has jumped (not current jump status)
        /// </summary>
        public bool HasJumped
        {
            get { return this.hasJumped; }
            set { this.hasJumped = value; }
        }

        /// <summary>
        /// Calls and applies the methods required to move the character
        /// </summary>
        /// <returns>A vector with the new X-Y coordinates of the character</returns>
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

        /// <summary>
        /// Calculate the gravity that needs to be applied to the character
        /// </summary>
        public override void CalculateGravity()
        {   
            if (this.ApplyGravity)                                                          // If this character needs to have gravity applied
            {                
                switch (gravityDirection)                                                   // Determine in which direction the gravity needs to be applied
                {                    
                    case GravityDirection.Up:                                               // If gravity needs to be applied in a upward direction
                        // If we have time

                        break;
                    case GravityDirection.Down:                        
                        if (hitObstacle == HitObstacle.FromTop)                             // If this object hit another object on it's top
                        {                            
                            base.Falling = false;                                           // Stop falling
                            this.GravitationalVelocity = 0;                            
                        }           
                        else if (!HasJumped)                                                // If this object did not hit another object on its top and is not
                        {                                                                   // busy with a jump action                            
                            this.GravitationalVelocity += this.GlobalAcceleration;                // Continue to implement gravity
                        }

                        break;                    
                    case GravityDirection.Left:                                             // If gravity needs to be applied in a left direction
                        // If we have time

                        break;
                    case GravityDirection.Right:                                            // If gravity needs to be applied in a right direction
                        // If we have time

                        break;
                }
            }
        }

        /// <summary>
        /// Calculate the movement velocity for the object
        /// </summary>
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
                    this.GravitationalVelocity -= this.GlobalAcceleration;
                }
                else if ((HasJumped) && (GravitationalVelocity <= -5))
                {
                    this.GravitationalVelocity += this.GlobalAcceleration;
                    HasJumped = false;
                }
                else if ((!HasJumped) && (JumpInProgress) && (GravitationalVelocity <= -5))
                {
                    this.GravitationalVelocity += this.GlobalAcceleration;
                }
            }
        }
    }
}