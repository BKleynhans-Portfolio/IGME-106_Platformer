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
/// Filename            : Environment.cs
/// </summary>

namespace Game1
{
    public abstract class NPC : Character
    {        
        /// <summary>
        /// Default constructor.  Creates a GameObject with default values.
        /// </summary>
        /// <param name="spriteTexture">Texture2D image for object</param>
        /// <param name="x">Starting X coordinate of object</param>
        /// <param name="y">Starting Y coordinate of object</param>
        /// <param name="width">Width of object</param>
        /// <param name="height">Height of object</param>
        public NPC(Texture2D spriteTexture, int x, int y, int width, int height) : base(spriteTexture, x, y, width, height)
        {
            this.ObjectXMoveDistance = 50;
            this.ObjectYMoveDistance = 50;
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
        public NPC(Texture2D spriteTexture, int spritesInSheet, int x, int y, int width, int height,
                          bool addGravity) :
                base(spriteTexture, spritesInSheet, x, y, width, height, addGravity)
        {
            this.ObjectXMoveDistance = 50;
            this.ObjectYMoveDistance = 50;
        }

        public override Vector2 ApplyMovement(GameTime gameTime)
        {
            Vector2 returnValue;

            base.UpdateMovementParameters(gameTime);
            this.CalculateGravity(gameTime);            

            returnValue = new Vector2(
                this.DrawLocation.X + base.MovementVelocity,
                this.DrawLocation.Y + base.GravitationalVelocity
            );

            return returnValue;
        }

        protected override void Update(GameTime gameTime)
        {
            base.TimeSinceLastUpdate += gameTime.ElapsedGameTime.Milliseconds;

            for (int i = 0; i < base.intersectedBy.Count; i++)
            {
                bool stillIntersecting = this.Intersects(intersectedBy[i]);

                if (!stillIntersecting)
                {
                    base.HitObstacle = HitObstacle.None;

                    base.intersectedBy.Remove(intersectedBy[i]);
                }
            }

            if (base.TimeSinceLastUpdate > 100)
            {
                UpdateMovementParameters(gameTime);
                base.UpdateSprite();
                base.SelectSprite(base.CurrentSpriteIndex);

                base.TimeSinceLastUpdate = 0;
            }

            CreateRectangle(ApplyMovement(gameTime));
        }

        ///summary>
        /// Determines whether the object that is passed in, intersects the current object
        /// </summary>
        /// <param name="passedGameObject">object to be tested for intersection against this object</param>
        /// <returns>true if it intersects and false if it does not</returns>
        public override bool Intersects(GameObject passedGameObject)
        {
            bool returnValue = false;

            if (this.DrawLocation.Intersects(passedGameObject.DrawLocation))                      // Does this object's drawLocation intersect with the passed in object's drawLocation
            {
                returnValue = true;

                if ((// From Top
                        (this.DrawLocation.Bottom > passedGameObject.DrawLocation.Top) &&         // If the lower border of this object has a larger Y coordinate than the upper border
                        (this.DrawLocation.Bottom < (passedGameObject.DrawLocation.Top + 10))     // of the passed in object but a lower Y coordinate than the passed in objects
                    ) && (                                                                  // Y coordinate + 10
                        (this.DrawLocation.Bottom != passedGameObject.DrawLocation.Bottom)
                    ))
                {
                    if (passedGameObject.GetType().BaseType == typeof(Environment))
                    {
                        base.HitObstacle = HitObstacle.None;
                    }
                    else if ((passedGameObject.GetType().BaseType == typeof(Character)) ||
                            (passedGameObject.GetType().BaseType.BaseType == typeof(Character)))
                    {
                        base.HitObstacle = HitObstacle.FromBottom;
                    }
                }
                else if ((// From Bottom
                            (this.DrawLocation.Top < passedGameObject.DrawLocation.Bottom) &&     // If the upper border of this object has a smaller Y coordinate than the lower border
                            (this.DrawLocation.Top > (passedGameObject.DrawLocation.Bottom - 10)) // of the passed in object but a higher Y coordinate than the passed in objects
                        ) && (                                                              // Y coordinate - 10
                            (this.DrawLocation.Bottom != passedGameObject.DrawLocation.Bottom)
                        ))
                {
                    // If this object is of type Character and the passed in object is of type Platform
                    // *** Each continuation of the BaseType keyword goes up one additional level in the derived classes                    
                    if (passedGameObject.GetType().BaseType == typeof(Environment))
                    {
                        base.HitObstacle = HitObstacle.None;
                    }
                    else if ((passedGameObject.GetType().BaseType == typeof(Character)) ||
                                (passedGameObject.GetType().BaseType.BaseType == typeof(Character)))
                    {
                        base.HitObstacle = HitObstacle.FromTop;
                    }
                }
                else if ((// From Right
                            (this.DrawLocation.Left < passedGameObject.DrawLocation.Right) &&     // If the left border of this object has a smaller X coordinate than the right border
                            (this.DrawLocation.Left > (passedGameObject.DrawLocation.Right - 10)) // of the passed in object but a higher X coordinate than the passed in objects
                        ) && (                                                              // X coordinate - 10
                            (this.DrawLocation.Right != passedGameObject.DrawLocation.Right)
                        ))
                {                                                                           // If this object is of type Character and the passed in object is of type Platform
                                                                                            // *** Each continuation of the BaseType keyword goes up one additional level in the derived classes
                    if (passedGameObject.GetType().BaseType == typeof(Environment))
                    {
                        base.HitObstacle = HitObstacle.None;
                    }
                    else if ((passedGameObject.GetType().BaseType == typeof(Character)) ||
                               (passedGameObject.GetType().BaseType.BaseType == typeof(Character)))
                    {
                        base.HitObstacle = HitObstacle.FromLeft;
                    }
                }
                else if ((// From Left
                            (this.DrawLocation.Right > passedGameObject.DrawLocation.Left) &&     // If the left border of this object has a smaller X coordinate than the right border
                            (this.DrawLocation.Right < (passedGameObject.DrawLocation.Left + 10)) // of the passed in object but a higher X coordinate than the passed in objects
                        ) && (                                                              // X coordinate - 10
                            (this.DrawLocation.Right != passedGameObject.DrawLocation.Right)
                        ))
                {
                    if (passedGameObject.GetType().BaseType == typeof(Environment))
                    {
                        base.HitObstacle = HitObstacle.None;
                    }
                    else if (
                               (passedGameObject.GetType().BaseType == typeof(Character)) ||
                               (passedGameObject.GetType().BaseType.BaseType == typeof(Character)))
                    {
                        base.HitObstacle = HitObstacle.FromRight;
                    }
                }
            }

            return returnValue;
        }

        /// Calculates the amount of force to apply for the object during each iteration of the game loop
        /// </summary>
        public override void CalculateGravity(GameTime gameTime)
        {
            if (base.ApplyGravity)
            {
                switch (base.GravityDirection)
                {
                    case GravityDirection.Up:
                        if
                            (base.GravitationalVelocity > -base.DefaultVerticalVelocity)
                        {
                            if ((base.GravitationalVelocity > -1) && (base.GravitationalVelocity < 1))
                            {
                                base.GravitationalVelocity = -1;
                                base.SpriteEffect = SpriteEffects.FlipVertically;
                            }
                            else
                            {
                                base.GravitationalVelocity -= base.EnvironmentalAcceleration;
                            }
                        }

                        break;
                    case GravityDirection.Down:
                        if
                            (base.GravitationalVelocity < base.DefaultVerticalVelocity)
                        {
                            if ((base.GravitationalVelocity > -1) && (base.GravitationalVelocity < 1))
                            {
                                base.GravitationalVelocity = 1;
                                base.SpriteEffect = SpriteEffects.None;
                            }
                            else
                            {
                                base.GravitationalVelocity += base.EnvironmentalAcceleration;
                            }
                        }

                        break;
                    case GravityDirection.Left:
                        if
                            (base.MovementVelocity > -base.DefaultHorizonalVelocity)
                        {
                            if ((base.MovementVelocity > -1) && (base.MovementVelocity < 1))
                            {
                                base.MovementVelocity = -1;
                                base.SpriteEffect = SpriteEffects.FlipHorizontally;
                            }
                            else
                            {
                                base.MovementVelocity -= base.EnvironmentalAcceleration;
                            }
                        }

                        break;

                    case GravityDirection.Right:
                        if
                            (base.MovementVelocity < base.DefaultHorizonalVelocity)
                        {
                            if ((base.MovementVelocity > -1) && (base.MovementVelocity < 1))
                            {
                                base.MovementVelocity = 1;
                                base.SpriteEffect = SpriteEffects.None;
                            }
                            else
                            {
                                base.MovementVelocity += base.EnvironmentalAcceleration;
                            }
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// Draw the sprite
        /// </summary>
        /// <param name="spriteBatch">Spritebatch Image</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                base.SpriteSheet,
                new Vector2(base.DrawLocation.X, (float)(base.DrawLocation.Y - 40)),
                base.SelectionArea,
                Color.White,
                0.0f,
                Vector2.Zero,
                0.22f,
                base.SpriteEffect,
                0.0f
            );
        }
    }
}
