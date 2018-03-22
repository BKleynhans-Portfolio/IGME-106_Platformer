﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Game1 - Platformer for Learning
/// Class Description   : Character class
/// Author              : Benjamin Kleynhans
/// Modified By         : Benjamin Kleynhans
/// Date                : March 13, 2018
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
        private int jumpsAllowed;
        private int jumpCount;
        private int jumpCurveCounter;

        public Character(Texture2D spriteTexture, int x, int y, int width, int height) : base(spriteTexture, x, y, width, height)
        {
            this.HasJumped = false;

            this.JumpCurveCounter = 10;

        }

        public Character(Texture2D spriteTexture, int x, int y, int width, int height,
                          bool addGravity, float appliedMoveForce, float appliedVerticalMovementForce,
                          float appliedGravitationalAcceleration, float appliedObjectMass) :
                base(spriteTexture, x, y, width, height, addGravity, appliedMoveForce, appliedVerticalMovementForce,
                    appliedGravitationalAcceleration, appliedObjectMass)
        {
            this.HasJumped = false;

            this.JumpCurveCounter = 10;
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

        public int JumpsAllowed
        {
            get { return this.jumpsAllowed; }
            set { this.jumpsAllowed = value; }
        }

        public int JumpCount
        {
            get { return this.jumpCount; }
            set { this.jumpCount = value; }
        }

        public int JumpCurveCounter
        {
            get { return this.jumpCurveCounter; }
            set { this.jumpCurveCounter = value; }
        }

        public virtual Vector2 ApplyMovement()
        {
            Vector2 returnValue;
            
            CalculateForces();

            returnValue = new Vector2(
                this.Rectangle.X + base.CalculatedHorizontalForce,
                this.Rectangle.Y + VerticalAcceleration
            );

            return returnValue;
        }

        public override void CalculateForces()
        {
            if (base.ApplyGravity)
            {
                switch (hitObstacle)
                {
                    case HitObstacle.None:

                        this.SurfaceForce = 0;

                        if (this.HasJumped)
                        {
                            this.VerticalAcceleration = this.CalculatedVerticalForce / this.ObjectMass;

                            if (this.VerticalAcceleration < 0)
                            {
                                if (((this.GravitationalAcceleration + this.VerticalAcceleration) * this.ObjectMass) == 0)
                                {
                                    this.CalculatedVerticalForce -= (-0.4f * this.ObjectMass);
                                }
                                else
                                {
                                    this.CalculatedVerticalForce += ((this.GravitationalAcceleration + this.VerticalAcceleration) * this.ObjectMass);
                                }

                                this.CalculatedVerticalForce += 25;
                            }

                            if (this.VerticalAcceleration > 0)
                            {
                                this.HasJumped = false;
                            }
                        }
                        else if (base.Falling)
                        {
                            if (this.VerticalAcceleration <= 0)
                            {
                                this.VerticalAcceleration += this.GravitationalAcceleration;
                            }
                            else
                            {
                                this.VerticalAcceleration += 0.1f;
                            }
                        }
                        else
                        {                          
                            if (VerticalAcceleration > 0)
                            {
                                this.hasJumped = false;
                                base.Falling = true;
                            }
                            else
                            {
                                VerticalAcceleration++;
                            }
                        }

                        break;
                    case HitObstacle.Bottom:
                        this.SurfaceForce = (this.GravitationalForce + this.CalculatedVerticalForce) * -1;
                        this.CalculatedVerticalForce = this.GravitationalForce + this.SurfaceForce;
                        this.VerticalAcceleration = this.CalculatedVerticalForce / this.ObjectMass;

                        break;
                    case HitObstacle.Left:
                        this.CalculatedHorizontalForce = 0;

                        break;
                    case HitObstacle.Top:
                        break;
                    case HitObstacle.Right:
                        this.CalculatedHorizontalForce = 0;

                        break;
                }
            }
        }

        public override bool Intersects(GameObject passedGameObject)
        {
            bool returnValue = false;

            if (this.Rectangle.Intersects(passedGameObject.Rectangle))
            {
                returnValue = true;

                if ((this.HasJumped == false) && (this.Falling == true))
                {
                    if (this.Rectangle.Bottom >= passedGameObject.Rectangle.Top)
                    {
                        this.hitObstacle = HitObstacle.Bottom;
                        base.CalculatedVerticalForce = 0;
                        this.Falling = false;
                    }
                    else if (this.Rectangle.Top <= passedGameObject.Rectangle.Bottom)
                    {
                        this.hitObstacle = HitObstacle.Top;
                    }
                    else if (this.Rectangle.Left <= passedGameObject.Rectangle.Right)
                    {
                        this.hitObstacle = HitObstacle.Left;
                    }                    
                    else if (this.Rectangle.Right >= passedGameObject.Rectangle.Left)
                    {
                        this.hitObstacle = HitObstacle.Right;
                    }
                }
            }
            else
            {
                returnValue = false;
            }

            return returnValue;
        }
    }
}