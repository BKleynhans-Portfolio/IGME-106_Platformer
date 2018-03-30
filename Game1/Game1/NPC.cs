//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;

///// <summary>
///// IGME-106 - Game Development and Algorithmic Problem Solving
///// Group Project
///// Class Description   : 
///// Created By          : Benjamin Kleynhans
///// Creation Date       : March 22, 2018
///// Authors             : Benjamin Kleynhans
/////                       
/////                       
/////                       
///// Last Modified By    : Benjamin Kleynhans
///// Last Modified Date  : March 22, 2018
///// Filename            : NPC.cs
///// </summary>

//namespace Game1
//{
//    abstract class NPC : Character
//    {
//        /// <summary>
//        /// Default constructor.  Creates a GameObject with default values.
//        /// </summary>
//        /// <param name="spriteTexture">Texture2D image for object</param>
//        /// <param name="x">Starting X coordinate of object</param>
//        /// <param name="y">Starting Y coordinate of object</param>
//        /// <param name="width">Width of object</param>
//        /// <param name="height">Height of object</param>
//        public NPC(Texture2D spriteTexture, int x, int y, int width, int height) : base(spriteTexture, x, y, width, height)
//        {

//        }

//        /// <summary>
//        /// This is a secondary constructor for the GameObject.
//        /// </summary>
//        /// <param name="spriteTexture">Texture2D image for object</param>
//        /// <param name="x">Starting X coordinate of object</param>
//        /// <param name="y">Starting Y coordinate of object</param>
//        /// <param name="width">Width of object</param>
//        /// <param name="height">Height of object</param>
//        /// <param name="addGravity">Does this object require immediate gravity implementation</param>
//        public NPC(Texture2D spriteTexture, int x, int y, int width, int height,
//                          bool addGravity) :
//                base(spriteTexture, x, y, width, height, addGravity)
//        {

//        }

//        public override Vector2 ApplyMovement()
//        {
//            Vector2 returnValue;

//            base.CalculateGravity();
//            base.CalculateMovement();

//            returnValue = new Vector2(
//                this.Rectangle.X + base.MovementVelocity,
//                this.Rectangle.Y + base.GravitationalVelocity
//            );

//            return returnValue;
//        }
//    }
//}


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
        public NPC(Texture2D spriteTexture, int x, int y, int width, int height,
                          bool addGravity) :
                base(spriteTexture, x, y, width, height, addGravity)
        {
            this.ObjectXMoveDistance = 50;
            this.ObjectYMoveDistance = 50;
        }

        public override Vector2 ApplyMovement()
        {
            Vector2 returnValue;

            this.CalculateGravity();

            returnValue = new Vector2(
                this.Rectangle.X + base.MovementVelocity,
                this.Rectangle.Y + base.GravitationalVelocity
            );

            return returnValue;
        }

        protected override void Update(GameTime gameTime)
        {
            for (int i = 0; i < base.intersectedBy.Count; i++)
            {
                bool stillIntersecting = this.Intersects(intersectedBy[i]);

                if (!stillIntersecting)
                {
                    base.hitObstacle = HitObstacle.None;

                    base.intersectedBy.Remove(intersectedBy[i]);
                }
            }

            UpdateMovementParameters();

            CreateRectangle(ApplyMovement());
        }

        ///summary>
        /// Determines whether the object that is passed in, intersects the current object
        /// </summary>
        /// <param name="passedGameObject">object to be tested for intersection against this object</param>
        /// <returns>true if it intersects and false if it does not</returns>
        public override bool Intersects(GameObject passedGameObject)
        {
            bool returnValue = false;

            if (this.Rectangle.Intersects(passedGameObject.Rectangle))                      // Does this object's rectangle intersect with the passed in object's rectangle
            {
                returnValue = true;

                if ((// From Top
                        (this.Rectangle.Bottom > passedGameObject.Rectangle.Top) &&         // If the lower border of this object has a larger Y coordinate than the upper border
                        (this.Rectangle.Bottom < (passedGameObject.Rectangle.Top + 10))     // of the passed in object but a lower Y coordinate than the passed in objects
                    ) && (                                                                  // Y coordinate + 10
                        (this.Rectangle.Bottom != passedGameObject.Rectangle.Bottom)
                    ))
                {
                    if (passedGameObject.GetType().BaseType == typeof(Environment))
                    {
                        hitObstacle = HitObstacle.None;
                    }
                    else if ((passedGameObject.GetType().BaseType == typeof(Character)) ||
                            (passedGameObject.GetType().BaseType.BaseType == typeof(Character)))
                    {
                        hitObstacle = HitObstacle.FromBottom;
                    }
                }
                else if ((// From Bottom
                            (this.Rectangle.Top < passedGameObject.Rectangle.Bottom) &&     // If the upper border of this object has a smaller Y coordinate than the lower border
                            (this.Rectangle.Top > (passedGameObject.Rectangle.Bottom - 10)) // of the passed in object but a higher Y coordinate than the passed in objects
                        ) && (                                                              // Y coordinate - 10
                            (this.Rectangle.Bottom != passedGameObject.Rectangle.Bottom)
                        ))
                {
                    // If this object is of type Character and the passed in object is of type Platform
                    // *** Each continuation of the BaseType keyword goes up one additional level in the derived classes                    
                    if (passedGameObject.GetType().BaseType == typeof(Environment))
                    {
                        hitObstacle = HitObstacle.None;
                    }
                    else if ((passedGameObject.GetType().BaseType == typeof(Character)) ||
                                (passedGameObject.GetType().BaseType.BaseType == typeof(Character)))
                    {
                        hitObstacle = HitObstacle.FromTop;
                    }
                }
                else if ((// From Right
                            (this.Rectangle.Left < passedGameObject.Rectangle.Right) &&     // If the left border of this object has a smaller X coordinate than the right border
                            (this.Rectangle.Left > (passedGameObject.Rectangle.Right - 10)) // of the passed in object but a higher X coordinate than the passed in objects
                        ) && (                                                              // X coordinate - 10
                            (this.Rectangle.Right != passedGameObject.Rectangle.Right)
                        ))
                {                                                                           // If this object is of type Character and the passed in object is of type Platform
                                                                                            // *** Each continuation of the BaseType keyword goes up one additional level in the derived classes
                    if (passedGameObject.GetType().BaseType == typeof(Environment))
                    {
                        hitObstacle = HitObstacle.None;
                    }
                    else if ((passedGameObject.GetType().BaseType == typeof(Character)) ||
                               (passedGameObject.GetType().BaseType.BaseType == typeof(Character)))
                    {
                        hitObstacle = HitObstacle.FromLeft;
                    }
                }
                else if ((// From Left
                            (this.Rectangle.Right > passedGameObject.Rectangle.Left) &&     // If the left border of this object has a smaller X coordinate than the right border
                            (this.Rectangle.Right < (passedGameObject.Rectangle.Left + 10)) // of the passed in object but a higher X coordinate than the passed in objects
                        ) && (                                                              // X coordinate - 10
                            (this.Rectangle.Right != passedGameObject.Rectangle.Right)
                        ))
                {
                    if (passedGameObject.GetType().BaseType == typeof(Environment))
                    {
                        hitObstacle = HitObstacle.None;
                    }
                    else if (
                               (passedGameObject.GetType().BaseType == typeof(Character)) ||
                               (passedGameObject.GetType().BaseType.BaseType == typeof(Character)))
                    {
                        hitObstacle = HitObstacle.FromRight;
                    }
                }
            }

            return returnValue;
        }

        /// Calculates the amount of force to apply for the object during each iteration of the game loop
        /// </summary>
        public override void CalculateGravity()
        {
            if (base.ApplyGravity)                                                          // If the object requires gravity to be implemented
            {
                switch (gravityDirection)                                                   // Based on which direction the gravity is implemented in
                {
                    case GravityDirection.Up:                        
                            if 
                                (base.GravitationalVelocity > -base.DefaultVerticalVelocity)
                            {
                                base.GravitationalVelocity -= base.EnvironmentalAcceleration;
                            }
                            else if (base.GravitationalVelocity > -base.DefaultVerticalVelocity)                            // If it is not hit, or it is hit and is an environment object, apply appropriate gravity
                            {
                                base.GravitationalVelocity -= base.GlobalAcceleration;
                            }

                        break;
                    case GravityDirection.Down:                        
                            if 
                                (base.GravitationalVelocity < base.DefaultVerticalVelocity)
                            {
                                base.GravitationalVelocity += base.EnvironmentalAcceleration;
                            }
                            else if (base.GravitationalVelocity < base.DefaultVerticalVelocity)                             // If it is not hit, or it is hit and is an environment object, apply appropriate gravity
                            {
                                base.GravitationalVelocity += base.GlobalAcceleration;
                            }

                        break;
                    case GravityDirection.Left:                        
                            if 
                                (base.MovementVelocity > -base.DefaultHorizonalVelocity)
                            {
                                base.MovementVelocity -= base.EnvironmentalAcceleration;
                                base.CyclesToThreshold++;

                                if ((base.MovementVelocity > -0.02) && (base.MovementVelocity < 0.02))
                                {
                                    CyclesToThreshold = 0;
                                }
                            }
                            else if (base.MovementVelocity > -base.DefaultHorizonalVelocity)                                 // If it is not hit, or it is hit and is an environment object, apply appropriate gravity
                            {
                                base.MovementVelocity -= base.GlobalAcceleration;
                            }

                        break;

                    case GravityDirection.Right:                        
                            if 
                                (base.MovementVelocity < base.DefaultHorizonalVelocity)
                            {
                                base.MovementVelocity += base.EnvironmentalAcceleration;
                                base.CyclesToThreshold++;

                                if ((base.MovementVelocity > -0.02) && (base.MovementVelocity < 0.02))
                                {
                                    CyclesToThreshold = 0;
                                }
                            }
                            else if (base.MovementVelocity < base.DefaultHorizonalVelocity)                                  // If it is not hit, or it is hit and is an environment object, apply appropriate gravity
                            {
                                base.MovementVelocity += base.GlobalAcceleration;
                            }

                        break;
                }
            }
        }
    }
}
