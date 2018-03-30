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
    public abstract class Character : GameObject
    {
        protected abstract override void Update(GameTime gameTime);

        private float platformHorizontalAcceleration;
        private float platformVerticalAcceleration;

        private int lives;

        private bool tookLife;
        private bool isAlive;
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


                this.Lives = 3; ;
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
        public Character(Texture2D spriteTexture, int x, int y, int width, int height,
                          bool addGravity) :
                base(spriteTexture, x, y, width, height, addGravity)
        {
            this.HasJumped = false;
            base.JumpInProgress = false;

            this.Lives = 3;
        }


        public float PlatformHorizontalAcceleration
        {
            get { return this.platformHorizontalAcceleration; }
            set { this.platformHorizontalAcceleration = value; }
        }

        public float PlatformVerticalAcceleration
        {
            get { return this.platformVerticalAcceleration; }
            set { this.platformVerticalAcceleration = value; }
        }

        /// <summary>
        /// Properties for variable which indicates whether the character is alive or not
        /// </summary>
        public bool IsAlive
        {
            get { return this.isAlive; }
            set { this.isAlive = value; }
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
        /// Number of lives the player has left
        /// </summary>
        public int Lives
        {
            get { return this.lives; }
            set { this.lives = value; }
        }

        public bool TookLife
        {
            get { return this.tookLife; }
            set { this.tookLife = value; }
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
                this.MovementVelocity = this.PlatformHorizontalAcceleration;
            }

            if ((movementAppliedTo == MovementAppliedTo.Left) && (hitObstacle != HitObstacle.FromLeft))
            {
                this.MovementVelocity = -DefaultHorizonalVelocity + this.PlatformHorizontalAcceleration;
            }

            if ((movementAppliedTo == MovementAppliedTo.Right) && (hitObstacle != HitObstacle.FromRight))
            {
                this.MovementVelocity = DefaultHorizonalVelocity + this.PlatformHorizontalAcceleration;
            }

            if ((movementAppliedTo == MovementAppliedTo.Up) && (hitObstacle != HitObstacle.FromTop))
            {
                if ((HasJumped) && (GravitationalVelocity == 0))
                {
                    this.GravitationalVelocity -= (int)(this.DefaultVerticalVelocity * 2.2);    // number is multiplier for player gravity application during jump
                }
                else if ((HasJumped) && (GravitationalVelocity > -5))
                {
                    this.GravitationalVelocity -= this.GlobalAcceleration;// + this.PlatformVerticalAcceleration;
                }
                else if ((HasJumped) && (GravitationalVelocity <= -5))
                {
                    this.GravitationalVelocity += this.GlobalAcceleration;// + this.PlatformVerticalAcceleration;
                    HasJumped = false;
                }
                else if ((!HasJumped) && (JumpInProgress) && (GravitationalVelocity <= -5))
                {
                    this.GravitationalVelocity += this.GlobalAcceleration;// + this.PlatformVerticalAcceleration;
                }
            }
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

                float newX;                                                                 // New X and Y values in case there is an intersection
                float newY;

                if ((// From Top
                        (this.Rectangle.Bottom > passedGameObject.Rectangle.Top) &&         // If the lower border of this object has a larger Y coordinate than the upper border
                        (this.Rectangle.Bottom < (passedGameObject.Rectangle.Top + 10))     // of the passed in object but a lower Y coordinate than the passed in objects
                    ) && (                                                                  // Y coordinate + 10
                        (this.Rectangle.Bottom != passedGameObject.Rectangle.Bottom)
                    ))
                {
                    if (passedGameObject.GetType() == typeof(Platform))
                    {
                        this.hitObstacle = HitObstacle.FromTop;
                        this.JumpInProgress = false;
                        this.PlatformHorizontalAcceleration = passedGameObject.MovementVelocity;
                        this.PlatformVerticalAcceleration = passedGameObject.GravitationalVelocity;

                        newX = this.Rectangle.X;                                     // Define new X and Y coordinates and create a new rectangle                        
                        newY = passedGameObject.Rectangle.Y - this.Rectangle.Height + 1;

                        if (newX != this.Rectangle.X || newY != this.Rectangle.Y)
                        {
                            CreateRectangle(new Vector2(newX, newY));
                        }
                    }
                    else if ((passedGameObject.GetType().BaseType.BaseType == typeof(Character)) ||
                            (passedGameObject.GetType().BaseType == typeof(Character)))

                    {
                        this.hitNPC = HitNPC.FromTop;                                       // Indicate that this object was hit by an NPC coming from the top

                        // If the player was hit by and NPC and it is not a friendly NPC, subtract one life.
                        if ((this.GetType() == typeof(Player)) && (passedGameObject.GetType() != typeof(Friendly)))
                        {
                            if (!this.TookLife)
                            {
                                this.TakeLife();
                            }
                        }
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
                    if (passedGameObject.GetType() == typeof(Platform))
                    {
                        this.hitObstacle = HitObstacle.FromBottom;
                        this.JumpInProgress = false;

                        newX = this.Rectangle.X;                                            // Define new X and Y coordinates and create a new rectangle
                        newY = passedGameObject.Rectangle.Y + passedGameObject.Rectangle.Height - 1;

                        if (newX != this.Rectangle.X || newY != this.Rectangle.Y)
                        {
                            CreateRectangle(new Vector2(newX, newY));
                        }
                    }                                                                       // If this object is of type Character and the passed in object is of type Platform
                    else if ((passedGameObject.GetType().BaseType.BaseType == typeof(Character)) ||
                            (passedGameObject.GetType().BaseType == typeof(Character)))
                    {
                        this.hitNPC = HitNPC.FromBottom;                                    // Indicate that this object was hit by an NPC coming from the bottom

                        // If the player was hit by and NPC and it is not a friendly NPC, subtract one life.
                        if ((this.GetType() == typeof(Player)) && (passedGameObject.GetType() != typeof(Friendly)))
                        {
                            if (!this.TookLife)
                            {
                                this.TakeLife();
                            }
                        }
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
                    if (passedGameObject.GetType() == typeof(Platform))
                    {
                        if (((JumpInProgress) || (Falling)) && (this.hitObstacle != HitObstacle.FromTop))
                        {
                            this.hitObstacle = HitObstacle.FromRight;
                        }

                        this.JumpInProgress = false;

                        newX = passedGameObject.Rectangle.X + passedGameObject.Rectangle.Width - 1;
                        newY = this.Rectangle.Y;

                        if (newX != this.Rectangle.X || newY != this.Rectangle.Y)           // Define new X and Y coordinates and create a new rectangle
                        {
                            CreateRectangle(new Vector2(newX, newY));
                        }
                    }                                                                       // If this object is of type Character and the passed in object is of type Platform
                    else if                                                                 // *** Each continuation of the BaseType keyword goes up one additional level in the derived classes
                       ((passedGameObject.GetType().BaseType.BaseType == typeof(Character)) ||
                        (passedGameObject.GetType().BaseType == typeof(Character)))
                    {
                        this.hitNPC = HitNPC.FromRight;                                     // Indicate that this object was hit by an NPC coming from the right

                        // If the player was hit by and NPC and it is not a friendly NPC, subtract one life.
                        if ((this.GetType() == typeof(Player)) && (passedGameObject.GetType() != typeof(Friendly)))
                        {
                            if (!this.TookLife)
                            {
                                this.TakeLife();
                            }
                        }
                    }
                }
                else if ((// From Left
                            (this.Rectangle.Right > passedGameObject.Rectangle.Left) &&     // If the left border of this object has a smaller X coordinate than the right border
                            (this.Rectangle.Right < (passedGameObject.Rectangle.Left + 10)) // of the passed in object but a higher X coordinate than the passed in objects
                        ) && (                                                              // X coordinate - 10
                            (this.Rectangle.Right != passedGameObject.Rectangle.Right)
                        ))
                {                                                                           // If this object is of type Character and the passed in object is of type Platform
                                                                                            // *** Each continuation of the BaseType keyword goes up one additional level in the derived classes
                    if (passedGameObject.GetType() == typeof(Platform))
                    {
                        if (((JumpInProgress) || (Falling)) && (this.hitObstacle != HitObstacle.FromTop))
                        {
                            this.hitObstacle = HitObstacle.FromLeft;
                        }

                        this.JumpInProgress = false;

                        newX = passedGameObject.Rectangle.X - this.Rectangle.Width + 1;     // Define new X and Y coordinates and create a new rectangle
                        newY = this.Rectangle.Y;

                        if (newX != this.Rectangle.X || newY != this.Rectangle.Y)
                        {
                            CreateRectangle(new Vector2(newX, newY));
                        }
                    }                                                                       // If this object is of type Character and the passed in object is of type Platform
                    else if                                                                 // *** Each continuation of the BaseType keyword goes up one additional level in the derived classes
                       ((passedGameObject.GetType().BaseType.BaseType == typeof(Character)) ||
                        (passedGameObject.GetType().BaseType == typeof(Character)))
                    {
                        this.hitNPC = HitNPC.FromLeft;                                      // Indicate that this object was hit by an NPC coming from the left

                        // If the player was hit by and NPC and it is not a friendly NPC, subtract one life.
                        if ((this.GetType() == typeof(Player)) && (passedGameObject.GetType() != typeof(Friendly)))
                        {
                            if (!this.TookLife)
                            {
                                this.TakeLife();
                            }
                        }
                    }
                }
            }

            return returnValue;
        }

        protected void TakeLife()
        {
            Console.WriteLine("You lost a life, " + this.Lives + " lives left");

            bool heartTaken = false;
            int lifeCounter = Lives;

            do
            {
                if (livesLeft[lifeCounter - 1].Visible)
                {
                    livesLeft[lifeCounter - 1].Visible = false;
                    heartTaken = true;
                }
                else
                {
                    lifeCounter--;
                }

            } while (heartTaken == false || this.Lives <= 0);

            this.Lives--;

            this.TookLife = true;            
            this.IsAlive = true;
            base.Falling = true;
            base.JumpInProgress = false;
            this.HasJumped = false;
            base.MovementVelocity = 0f;

            
            if (this.Lives <= 0)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            Console.WriteLine("Player Died");

            foreach (GraphicElement graphicsElement in livesLeft)
            {
                graphicsElement.Visible = true;

                Lives++;
            }

            base.CreateRectangle(new Vector2(0, 800));

            gameState = GameState.Title;
        }
    }
}