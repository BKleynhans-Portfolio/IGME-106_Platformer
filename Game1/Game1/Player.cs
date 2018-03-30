using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

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
/// Filename            : Player.cs
/// </summary>

namespace Game1
{
    class Player : Character
    {
        private int jumpCount;

        /// <summary>
        /// Default constructor.  Creates a GameObject with default values.
        /// </summary>
        /// <param name="spriteTexture">Texture2D image for object</param>
        /// <param name="x">Starting X coordinate of object</param>
        /// <param name="y">Starting Y coordinate of object</param>
        /// <param name="width">Width of object</param>
        /// <param name="height">Height of object</param>
        public Player(Texture2D spriteTexture, int x, int y, int width, int height) : base(spriteTexture, x, y, width, height)
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
        public Player(Texture2D spriteTexture, int x, int y, int width, int height,
                          bool addGravity) :
                base(spriteTexture, x, y, width, height, addGravity)
        {
            base.IsAlive = true;
        }

        public int JumpCount
        {
            get { return this.jumpCount; }
            set { this.jumpCount = value; }
        }

        public override Vector2 ApplyMovement()
        {
            Vector2 returnValue;

            if ((currentKeyboardState.IsKeyDown(Keys.A)) && (previousKeyboardState.IsKeyUp(Keys.A)))
            {
                base.movementAppliedTo = MovementAppliedTo.Left;
            }
            else if ((currentKeyboardState.IsKeyDown(Keys.D)) && (previousKeyboardState.IsKeyUp(Keys.D)))
            {
                base.movementAppliedTo = MovementAppliedTo.Right;
            }

            if (gravityDirection == GravityDirection.Down)
            {
                if (((currentKeyboardState.IsKeyDown(Keys.Space)) && (previousKeyboardState.IsKeyUp(Keys.Space))) &&
                    ((this.hitObstacle == HitObstacle.FromTop) || (this.JumpCount == 1)))
                {
                    base.movementAppliedTo = MovementAppliedTo.Up;

                    base.hitObstacle = HitObstacle.None;

                    if (base.HasJumped == false)
                    {
                        base.HasJumped = true;
                    }

                    if (base.JumpInProgress == false)
                    {
                        base.JumpInProgress = true;
                    }

                }
            }
            else if (gravityDirection == GravityDirection.Up)
            {
                if (((currentKeyboardState.IsKeyDown(Keys.Space)) && (previousKeyboardState.IsKeyUp(Keys.Space))) &&
                    ((this.hitObstacle == HitObstacle.FromTop) || (this.JumpCount == 1)))
                {
                    base.movementAppliedTo = MovementAppliedTo.Down;

                    base.hitObstacle = HitObstacle.None;

                    if (base.HasJumped == false)
                    {
                        base.HasJumped = true;
                    }

                    if (base.JumpInProgress == false)
                    {
                        base.JumpInProgress = true;
                    }
                }
            }

            if ((currentKeyboardState.IsKeyUp(Keys.A)) && (currentKeyboardState.IsKeyUp(Keys.D)) &&
                (base.Falling == false) && (base.HasJumped == false) && (base.JumpInProgress == false))
            {
                base.movementAppliedTo = MovementAppliedTo.None;
            }

            base.CalculateGravity();
            base.CalculateMovement();

            returnValue = new Vector2(
                this.Rectangle.X + base.MovementVelocity,
                this.Rectangle.Y + base.GravitationalVelocity
            );

            return returnValue;
        }

        protected override void Update(GameTime gameTime)
        {
            if ((this.Rectangle.Y + this.Rectangle.Height) > SCREENHEIGHT)
            {
                base.TakeLife();

                CreateRectangle(new Vector2(50, 50));
                base.MovementVelocity = 0f;
                base.GravitationalVelocity = 0f;
            }
            else if (this.Rectangle.Y < 0)
            {
                CreateRectangle(this.Rectangle.X, 1);
            }
            
            if (this.IsAlive)
            {
                for (int i = 0; i < base.intersectedBy.Count; i++)
                {
                    bool stillIntersecting = base.Intersects(intersectedBy[i]);

                    if ((!stillIntersecting) && (this.HasJumped == false))
                    {
                        if ((base.intersectedBy[i].GetType() == typeof(Platform)) && (base.intersectedBy[i].GravitationalVelocity >= 0))
                        {
                            base.Falling = true;
                            base.hitObstacle = HitObstacle.None;
                        }

                        //if ((base.intersectedBy[i].GetType() == typeof(Platform)) && (base.intersectedBy[i].GravitationalVelocity < 0))
                        //{
                        //    base.Falling = true;
                        //    base.hitObstacle = HitObstacle.None;
                        //}

                        if (base.intersectedBy[i].GetType() == typeof(Enemy))
                        {
                            base.TookLife = false;
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
                base.TakeLife();
            }
        }
    }
}
