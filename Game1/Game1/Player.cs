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
/// Filename            : Player.cs
/// </summary>

namespace Game1
{
    class Player : Character
    {
        private int jumpCount;

        public Player(Texture2D spriteTexture, int x, int y, int width, int height) : base(spriteTexture, x, y, width, height)
        {
            base.IsAlive = true;
        }

        public Player(Texture2D spriteTexture, int x, int y, int width, int height,
                          bool addGravity, float appliedMoveForce, float appliedVerticalMovementForce,
                          float appliedGravitationalAcceleration, float appliedObjectMass) :
                base(spriteTexture, x, y, width, height, addGravity, appliedMoveForce, appliedVerticalMovementForce,
                    appliedGravitationalAcceleration, appliedObjectMass)
        {
            base.IsAlive = true;
        }

        public int JumpCount
        {
            get { return this.jumpCount; }
            set { this.jumpCount = value; }
        }

        protected void TakeLife()
        {
            Console.WriteLine("You lost a life, " + base.Lives + " lives left");


            base.CreateRectangle(new Vector2(50, 50));
            this.IsAlive = true;

            //if (base.Lives == 0)
            //{
            //    //Die();
            //}
        }

        protected override void Die()
        {
            Console.WriteLine("Player Died");
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
                this.Rectangle.X + base.AVelocity,
                this.Rectangle.Y + base.GVelocity
            );

            return returnValue;
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
                TakeLife();
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
