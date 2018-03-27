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
                if (base.WasAlive == true)
                {
                    base.IsAlive = false;
                    base.WasAlive = false;
                }
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

        /////summary>
        ///// Determines whether the object that is passed in, intersects the current object
        ///// </summary>
        ///// <param name="passedGameObject">object to be tested for intersection against this object</param>
        ///// <returns>true if it intersects and false if it does not</returns>
        //public override bool Intersects(GameObject passedGameObject)
        //{
        //    bool returnValue = false;

        //    if (this.Rectangle.Intersects(passedGameObject.Rectangle))                      // Does this object's rectangle intersect with the passed in object's rectangle
        //    {
        //        returnValue = true;

        //        float newX;                                                                 // New X and Y values in case there is an intersection
        //        float newY;

        //        if ((// From Top
        //                (this.Rectangle.Bottom > passedGameObject.Rectangle.Top) &&         // If the lower border of this object has a larger Y coordinate than the upper border
        //                (this.Rectangle.Bottom < (passedGameObject.Rectangle.Top + 10))     // of the passed in object but a lower Y coordinate than the passed in objects
        //            ) && (                                                                  // Y coordinate + 10
        //                (this.Rectangle.Bottom != passedGameObject.Rectangle.Bottom)
        //            ))
        //        {
        //            if ((                                                                    // If this object is of type Character and the passed in object is of type Platform
        //                    (this.GetType().BaseType == typeof(Character)) ||               // *** Each continuation of the BaseType keyword goes up one additional level in the derived classes
        //                    (this.GetType().BaseType.BaseType == typeof(Character))
        //                ) && (
        //                    (passedGameObject.GetType() == typeof(Platform))
        //                ))
        //            {
        //                this.hitObstacle = HitObstacle.FromTop;
        //                this.JumpInProgress = false;

        //                newX = this.Rectangle.X;                                            // Define new X and Y coordinates and create a new rectangle
        //                newY = passedGameObject.Rectangle.Y - this.Rectangle.Height + 1;

        //                if (newX != this.Rectangle.X || newY != this.Rectangle.Y)
        //                {
        //                    CreateRectangle(new Vector2(newX, newY));
        //                }
        //            }
        //            else if ((                                                              // If both this object as well as the passed in object are of type Character
        //                        (this.GetType().BaseType == typeof(Character)) &&           // *** Each continuation of the BaseType keyword goes up one additional level in the derived classes
        //                        (passedGameObject.GetType().BaseType.BaseType == typeof(Character))
        //                    ) || (
        //                        (this.GetType().BaseType.BaseType == typeof(Character)) &&
        //                        (passedGameObject.GetType().BaseType == typeof(Character))
        //                    ))
        //            {
        //                this.hitNPC = HitNPC.FromTop;                                       // Indicate that this object was hit by an NPC coming from the top

        //                // If the player was hit by and NPC and it is not a friendly NPC, subtract one life.
        //                if ((this.GetType() == typeof(Player)) && (passedGameObject.GetType() != typeof(Friendly)))
        //                {
        //                    //if (!this.TookLife)
        //                    //{
        //                    //    this.Lives--;
        //                    //}
        //                }
        //            }
        //            else if (                                                               // If both this object as well as the passed in object are of type Environment
        //                        (this.GetType().BaseType == typeof(Environment)) &&
        //                        (passedGameObject.GetType().BaseType == typeof(Environment))
        //                    )
        //            {
        //                hitObstacle = HitObstacle.None;
        //            }
        //            else if ((
        //                       (this.GetType().BaseType == typeof(Environment))
        //                   ) && (
        //                       (passedGameObject.GetType().BaseType == typeof(Character)) ||
        //                       (passedGameObject.GetType().BaseType.BaseType == typeof(Character))
        //                   ))
        //            {
        //                hitObstacle = HitObstacle.FromBottom;
        //            }
        //            else
        //            {
        //                this.hitObstacle = HitObstacle.FromTop;
        //            }
        //        }
        //        else if ((// From Bottom
        //                    (this.Rectangle.Top < passedGameObject.Rectangle.Bottom) &&     // If the upper border of this object has a smaller Y coordinate than the lower border
        //                    (this.Rectangle.Top > (passedGameObject.Rectangle.Bottom - 10)) // of the passed in object but a higher Y coordinate than the passed in objects
        //                ) && (                                                              // Y coordinate - 10
        //                    (this.Rectangle.Bottom != passedGameObject.Rectangle.Bottom)
        //                ))
        //        {
        //            // If this object is of type Character and the passed in object is of type Platform
        //            // *** Each continuation of the BaseType keyword goes up one additional level in the derived classes
        //            if ((
        //                    (this.GetType().BaseType == typeof(Character)) ||
        //                    (this.GetType().BaseType.BaseType == typeof(Character))
        //                ) && (
        //                    (passedGameObject.GetType() == typeof(Platform))
        //                ))
        //            {
        //                this.hitObstacle = HitObstacle.FromBottom;
        //                this.JumpInProgress = false;

        //                newX = this.Rectangle.X;                                            // Define new X and Y coordinates and create a new rectangle
        //                newY = passedGameObject.Rectangle.Y + passedGameObject.Rectangle.Height - 1;

        //                if (newX != this.Rectangle.X || newY != this.Rectangle.Y)
        //                {
        //                    CreateRectangle(new Vector2(newX, newY));
        //                }
        //            }                                                                       // If this object is of type Character and the passed in object is of type Platform
        //            else if ((                                                               // *** Each continuation of the BaseType keyword goes up one additional level in the derived classes
        //                        (this.GetType().BaseType == typeof(Character)) &&
        //                        (passedGameObject.GetType().BaseType.BaseType == typeof(Character))
        //                    ) || (
        //                        (this.GetType().BaseType.BaseType == typeof(Character)) &&
        //                        (passedGameObject.GetType().BaseType == typeof(Character))
        //                    ))
        //            {
        //                this.hitNPC = HitNPC.FromBottom;                                    // Indicate that this object was hit by an NPC coming from the bottom

        //                // If the player was hit by and NPC and it is not a friendly NPC, subtract one life.
        //                if ((this.GetType() == typeof(Player)) && (passedGameObject.GetType() != typeof(Friendly)))
        //                {
        //                    //if (!this.TookLife)
        //                    //{
        //                    //    this.Lives--;
        //                    //}
        //                }
        //            }
        //            else if ((this.GetType().BaseType == typeof(Environment)) &&            // If both this object as well as the passed in object are of type Environment
        //                    (passedGameObject.GetType().BaseType == typeof(Environment)))
        //            {
        //                hitObstacle = HitObstacle.None;
        //            }
        //            else if ((
        //                        (this.GetType().BaseType == typeof(Environment))
        //                    ) && (
        //                        (passedGameObject.GetType().BaseType == typeof(Character)) ||
        //                        (passedGameObject.GetType().BaseType.BaseType == typeof(Character))
        //                    ))
        //            {
        //                hitObstacle = HitObstacle.FromTop;
        //            }
        //            else
        //            {
        //                this.hitObstacle = HitObstacle.FromBottom;
        //            }
        //        }
        //        else if ((// From Right
        //                    (this.Rectangle.Left < passedGameObject.Rectangle.Right) &&     // If the left border of this object has a smaller X coordinate than the right border
        //                    (this.Rectangle.Left > (passedGameObject.Rectangle.Right - 10)) // of the passed in object but a higher X coordinate than the passed in objects
        //                ) && (                                                              // X coordinate - 10
        //                    (this.Rectangle.Right != passedGameObject.Rectangle.Right)
        //                ))
        //        {                                                                           // If this object is of type Character and the passed in object is of type Platform
        //                                                                                    // *** Each continuation of the BaseType keyword goes up one additional level in the derived classes
        //            if ((
        //                    (this.GetType().BaseType == typeof(Character)) ||
        //                    (this.GetType().BaseType.BaseType == typeof(Character))
        //                ) && (
        //                    (passedGameObject.GetType() == typeof(Platform))
        //                ))
        //            {
        //                if (((JumpInProgress) || (Falling)) && (this.hitObstacle != HitObstacle.FromTop))
        //                {
        //                    this.hitObstacle = HitObstacle.FromRight;
        //                }

        //                this.JumpInProgress = false;

        //                newX = passedGameObject.Rectangle.X + passedGameObject.Rectangle.Width - 1;
        //                newY = this.Rectangle.Y;

        //                if (newX != this.Rectangle.X || newY != this.Rectangle.Y)           // Define new X and Y coordinates and create a new rectangle
        //                {
        //                    CreateRectangle(new Vector2(newX, newY));
        //                }
        //            }                                                                       // If this object is of type Character and the passed in object is of type Platform
        //            else if                                                                 // *** Each continuation of the BaseType keyword goes up one additional level in the derived classes
        //               ((
        //                    (this.GetType().BaseType == typeof(Character)) &&
        //                    (passedGameObject.GetType().BaseType.BaseType == typeof(Character))
        //                ) || (
        //                    (this.GetType().BaseType.BaseType == typeof(Character)) &&
        //                    (passedGameObject.GetType().BaseType == typeof(Character))
        //                ))
        //            {
        //                this.hitNPC = HitNPC.FromRight;                                     // Indicate that this object was hit by an NPC coming from the right

        //                // If the player was hit by and NPC and it is not a friendly NPC, subtract one life.
        //                if ((this.GetType() == typeof(Player)) && (passedGameObject.GetType() != typeof(Friendly)))
        //                {
        //                    //if (!this.TookLife)
        //                    //{
        //                    //    this.Lives--;
        //                    //}

        //                }
        //            }
        //            else if ((this.GetType().BaseType == typeof(Environment)) &&            // If both this object as well as the passed in object are of type Environment
        //                    (passedGameObject.GetType().BaseType == typeof(Environment)))
        //            {
        //                hitObstacle = HitObstacle.None;
        //            }
        //            else if ((
        //                       (this.GetType().BaseType == typeof(Environment))
        //                   ) && (
        //                       (passedGameObject.GetType().BaseType == typeof(Character)) ||
        //                       (passedGameObject.GetType().BaseType.BaseType == typeof(Character))
        //                   ))
        //            {
        //                hitObstacle = HitObstacle.FromLeft;
        //            }
        //            else
        //            {
        //                this.hitObstacle = HitObstacle.FromRight;
        //            }
        //        }
        //        else if ((// From Left
        //                    (this.Rectangle.Right > passedGameObject.Rectangle.Left) &&     // If the left border of this object has a smaller X coordinate than the right border
        //                    (this.Rectangle.Right < (passedGameObject.Rectangle.Left + 10)) // of the passed in object but a higher X coordinate than the passed in objects
        //                ) && (                                                              // X coordinate - 10
        //                    (this.Rectangle.Right != passedGameObject.Rectangle.Right)
        //                ))
        //        {                                                                           // If this object is of type Character and the passed in object is of type Platform
        //                                                                                    // *** Each continuation of the BaseType keyword goes up one additional level in the derived classes
        //            if ((
        //                    (this.GetType().BaseType == typeof(Character)) ||
        //                    (this.GetType().BaseType.BaseType == typeof(Character))
        //                ) && (
        //                    (passedGameObject.GetType() == typeof(Platform))
        //                ))
        //            {
        //                if (((JumpInProgress) || (Falling)) && (this.hitObstacle != HitObstacle.FromTop))
        //                {
        //                    this.hitObstacle = HitObstacle.FromLeft;
        //                }

        //                this.JumpInProgress = false;

        //                newX = passedGameObject.Rectangle.X - this.Rectangle.Width + 1;     // Define new X and Y coordinates and create a new rectangle
        //                newY = this.Rectangle.Y;

        //                if (newX != this.Rectangle.X || newY != this.Rectangle.Y)
        //                {
        //                    CreateRectangle(new Vector2(newX, newY));
        //                }
        //            }                                                                       // If this object is of type Character and the passed in object is of type Platform
        //            else if                                                                 // *** Each continuation of the BaseType keyword goes up one additional level in the derived classes
        //               ((
        //                    (this.GetType().BaseType == typeof(Character)) &&
        //                    (passedGameObject.GetType().BaseType.BaseType == typeof(Character))
        //                ) || (
        //                    (this.GetType().BaseType.BaseType == typeof(Character)) &&
        //                    (passedGameObject.GetType().BaseType == typeof(Character))
        //                ))
        //            {
        //                this.hitNPC = HitNPC.FromLeft;                                      // Indicate that this object was hit by an NPC coming from the left

        //                // If the player was hit by and NPC and it is not a friendly NPC, subtract one life.
        //                if ((this.GetType() == typeof(Player)) && (passedGameObject.GetType() != typeof(Friendly)))
        //                {
        //                    //if (!this.TookLife)
        //                    //{
        //                    //    this.Lives--;
        //                    //}                            
        //                }
        //            }
        //            else if ((this.GetType().BaseType == typeof(Environment)) &&            // If both this object as well as the passed in object are of type Environment
        //                    (passedGameObject.GetType().BaseType == typeof(Environment)))
        //            {
        //                hitObstacle = HitObstacle.None;
        //            }
        //            else if ((
        //                       (this.GetType().BaseType == typeof(Environment))
        //                   ) && (
        //                       (passedGameObject.GetType().BaseType == typeof(Character)) ||
        //                       (passedGameObject.GetType().BaseType.BaseType == typeof(Character))
        //                   ))
        //            {
        //                hitObstacle = HitObstacle.FromRight;
        //            }
        //            else
        //            {
        //                this.hitObstacle = HitObstacle.FromLeft;
        //            }
        //        }
        //    }

        //    return returnValue;
        //}
    }
}
