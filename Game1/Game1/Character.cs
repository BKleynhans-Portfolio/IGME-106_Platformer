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
        public abstract Vector2 ApplyMovement(GameTime gameTime);

        public float PlatformHorizontalAcceleration { get; set; }
        public float PlatformVerticalAcceleration { get; set; }

        public int Lives { get; set; }

        public bool TookLife { get; set; }
        public bool IsAlive { get; set; }
        public bool HasJumped { get; set; }
        public int TimeSinceJump { get; set; }
        public int TimeSinceGravity { get; set; }

        public int JumpVelocity { get; set; }

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
            this.TimeSinceGravity = 0;

            this.Lives = 3;
            this.JumpVelocity = 6;
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
        public Character(Texture2D spriteTexture, int spritesInSheet, int x, int y, int width, int height,
                          bool addGravity) :
                base(spriteTexture, spritesInSheet, x, y, width, height, addGravity)
        {
            this.HasJumped = false;
            base.JumpInProgress = false;
            this.TimeSinceGravity = 0;

            this.Lives = 3;
            this.JumpVelocity = 6;
        }

        /// <summary>
        /// Calculate the gravity that needs to be applied to the character
        /// </summary>
        public override void CalculateGravity(GameTime gameTime)
        {            
            if (this.ApplyGravity)                                                          // If this character needs to have gravity applied
            {
                switch (base.GravityDirection)                                                   // Determine in which direction the gravity needs to be applied
                {
                    case GravityDirection.Up:                                               // If gravity needs to be applied in a upward direction
                                                                                            // If we have time

                        break;
                    case GravityDirection.Down:
                        if (base.HitObstacle == HitObstacle.FromTop)                             // If this object hit another object on it's top
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
        public void CalculateMovement(GameTime gameTime)
        {
            if (base.MovementAppliedTo == MovementAppliedTo.None)
            {
                if (this.PlatformHorizontalAcceleration != 0)
                {
                    base.MovementVelocity = this.PlatformHorizontalAcceleration;
                }
                else
                {
                    base.MovementVelocity = 0;
                }

                if (this.PlatformVerticalAcceleration > 0)
                {
                    base.GravitationalVelocity += this.PlatformVerticalAcceleration;
                }
            }

            if ((base.MovementAppliedTo == MovementAppliedTo.Left) && (base.HitObstacle != HitObstacle.FromLeft))
            {
                base.MovementVelocity = -DefaultHorizonalVelocity + this.PlatformHorizontalAcceleration;
            }

            if ((base.MovementAppliedTo == MovementAppliedTo.Right) && (base.HitObstacle != HitObstacle.FromRight))
            {
                base.MovementVelocity = DefaultHorizonalVelocity + this.PlatformHorizontalAcceleration;
            }

            
            if ((base.HitObstacle != HitObstacle.FromTop))
            {
                if ((HasJumped) && (GravitationalVelocity == 0))
                {
                    this.GravitationalVelocity -= this.JumpVelocity;
                }

                if (this.TimeSinceJump < 1)
                {
                    if ((HasJumped) && (GravitationalVelocity > -5))
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

                    this.TimeSinceJump = gameTime.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    this.TimeSinceJump--;
                }
            }

            if ((base.MovementAppliedTo == MovementAppliedTo.Up) && (base.HitObstacle == HitObstacle.FromBottom))
            {
                this.GravitationalVelocity = 0;

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

            if (this.DrawLocation.Intersects(passedGameObject.DrawLocation))                      // Does this object's drawLocation intersect with the passed in object's drawLocation                        
            {
                returnValue = true;                

                float newX;                                                                 // New X and Y values in case there is an intersection
                float newY;

                if ((// From Top
                        (this.DrawLocation.Bottom > passedGameObject.DrawLocation.Top) &&         // If the lower border of this object has a larger Y coordinate than the upper border
                        (this.DrawLocation.Bottom < (passedGameObject.DrawLocation.Top + 20))     // of the passed in object but a lower Y coordinate than the passed in objects
                    ) && (                                                                  // Y coordinate + 10
                        (this.DrawLocation.Bottom != passedGameObject.DrawLocation.Bottom)
                    ))
                {
                    if ((passedGameObject.GetType() == typeof(Platform)) && (!passedGameObject.PlatformType.Equals("Water")))
                    {                        
                        base.HitObstacle = HitObstacle.FromTop;
                        this.JumpInProgress = false;
                        this.HasJumped = false;
                        this.PlatformHorizontalAcceleration = passedGameObject.MovementVelocity;
                        this.PlatformVerticalAcceleration = passedGameObject.GravitationalVelocity;

                        newX = this.DrawLocation.X;                                     // Define new X and Y coordinates and create a new drawLocation                        
                        newY = passedGameObject.DrawLocation.Y - this.DrawLocation.Height + 1;

                        if (newX != this.DrawLocation.X || newY != this.DrawLocation.Y)
                        {
                            CreateRectangle(new Vector2(newX, newY));
                        }
                    }
                    else if ((passedGameObject.GetType().BaseType.BaseType == typeof(Character)) ||
                            (passedGameObject.GetType().BaseType == typeof(Character)))

                    {
                        base.HitNpc = HitNpc.FromTop;                                       // Indicate that this object was hit by an NPC coming from the top

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
                            (this.DrawLocation.Top < passedGameObject.DrawLocation.Bottom) &&     // If the upper border of this object has a smaller Y coordinate than the lower border
                            (this.DrawLocation.Top > (passedGameObject.DrawLocation.Bottom - 20)) // of the passed in object but a higher Y coordinate than the passed in objects
                        ) && (                                                              // Y coordinate - 10
                            (this.DrawLocation.Bottom != passedGameObject.DrawLocation.Bottom)
                        ))
                {
                    // If this object is of type Character and the passed in object is of type Platform
                    // *** Each continuation of the BaseType keyword goes up one additional level in the derived classes
                    if ((passedGameObject.GetType() == typeof(Platform)) && (!passedGameObject.PlatformType.Equals("Water")))
                    {
                        base.HitObstacle = HitObstacle.FromBottom;
                        this.JumpInProgress = false;
                        this.HasJumped = false;
                        base.Falling = true;
                        base.HitObstacle = HitObstacle.None;

                        if (base.GravitationalVelocity < 0)
                        {
                            base.GravitationalVelocity += 2;
                        }
                        else
                        {
                            base.GravitationalVelocity += GlobalAcceleration;
                        }

                        newX = this.DrawLocation.X;                                            // Define new X and Y coordinates and create a new drawLocation
                        newY = passedGameObject.DrawLocation.Y + passedGameObject.DrawLocation.Height - 1;

                        if (newX != this.DrawLocation.X || newY != this.DrawLocation.Y)
                        {
                            CreateRectangle(new Vector2(newX, newY));
                        }
                    }                                                                       // If this object is of type Character and the passed in object is of type Platform
                    else if ((passedGameObject.GetType().BaseType.BaseType == typeof(Character)) ||
                            (passedGameObject.GetType().BaseType == typeof(Character)))
                    {
                        base.HitNpc = HitNpc.FromBottom;                                    // Indicate that this object was hit by an NPC coming from the bottom

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
                            (this.DrawLocation.Left + 2 < passedGameObject.DrawLocation.Right - 2) &&     // If the left border of this object has a smaller X coordinate than the right border
                            (this.DrawLocation.Left + 2 > (passedGameObject.DrawLocation.Right - 20)) // of the passed in object but a higher X coordinate than the passed in objects
                        ) && (                                                              // X coordinate - 10
                            (this.DrawLocation.Right != passedGameObject.DrawLocation.Right)
                        ))
                {                                                                           // If this object is of type Character and the passed in object is of type Platform
                                                                                            // *** Each continuation of the BaseType keyword goes up one additional level in the derived classes
                    if ((passedGameObject.GetType() == typeof(Platform)) && (!passedGameObject.PlatformType.Equals("Water")))
                    {
                        if (((JumpInProgress) || (Falling)) && (base.HitObstacle != HitObstacle.FromTop))
                        {
                            base.HitObstacle = HitObstacle.FromRight;
                        }

                        this.JumpInProgress = false;

                        newX = passedGameObject.DrawLocation.X + passedGameObject.DrawLocation.Width - 1;
                        newY = this.DrawLocation.Y;

                        if (newX != this.DrawLocation.X || newY != this.DrawLocation.Y)           // Define new X and Y coordinates and create a new drawLocation
                        {
                            CreateRectangle(new Vector2(newX, newY));
                        }
                    }                                                                       // If this object is of type Character and the passed in object is of type Platform
                    else if                                                                 // *** Each continuation of the BaseType keyword goes up one additional level in the derived classes
                       ((passedGameObject.GetType().BaseType.BaseType == typeof(Character)) ||
                        (passedGameObject.GetType().BaseType == typeof(Character)))
                    {
                        base.HitNpc = HitNpc.FromRight;                                     // Indicate that this object was hit by an NPC coming from the right

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
                            (this.DrawLocation.Right - 2 > passedGameObject.DrawLocation.Left + 2) &&     // If the left border of this object has a smaller X coordinate than the right border
                            (this.DrawLocation.Right - 2 < (passedGameObject.DrawLocation.Left + 20)) // of the passed in object but a higher X coordinate than the passed in objects
                        ) && (                                                              // X coordinate - 10
                            (this.DrawLocation.Right != passedGameObject.DrawLocation.Right)
                        ))
                {                                                                           // If this object is of type Character and the passed in object is of type Platform
                                                                                            // *** Each continuation of the BaseType keyword goes up one additional level in the derived classes
                    if ((passedGameObject.GetType() == typeof(Platform)) && (!passedGameObject.PlatformType.Equals("Water")))
                    {
                        if (((JumpInProgress) || (Falling)) && (base.HitObstacle != HitObstacle.FromTop))
                        {
                            base.HitObstacle = HitObstacle.FromLeft;
                        }

                        this.JumpInProgress = false;

                        newX = passedGameObject.DrawLocation.X - this.DrawLocation.Width + 1;     // Define new X and Y coordinates and create a new drawLocation
                        newY = this.DrawLocation.Y;

                        if (newX != this.DrawLocation.X || newY != this.DrawLocation.Y)
                        {
                            CreateRectangle(new Vector2(newX, newY));
                        }
                    }                                                                       // If this object is of type Character and the passed in object is of type Platform
                    else if                                                                 // *** Each continuation of the BaseType keyword goes up one additional level in the derived classes
                       ((passedGameObject.GetType().BaseType.BaseType == typeof(Character)) ||
                        (passedGameObject.GetType().BaseType == typeof(Character)))
                    {
                        base.HitNpc = HitNpc.FromLeft;                                      // Indicate that this object was hit by an NPC coming from the left

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
            bool heartTaken = false;
            int lifeCounter = Lives;

            do
            {
                if (LivesLeft[lifeCounter - 1].Visible)
                {
                    LivesLeft[lifeCounter - 1].Visible = false;
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

            if (base.HitObstacle == HitObstacle.None)
            {
                base.Falling = true;
                base.MovementVelocity = 0f;
            }
            
            base.JumpInProgress = false;
            this.HasJumped = false;
            
            if (this.Lives <= 0)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            Console.WriteLine("Player Died");

            foreach (GraphicElement graphicsElement in LivesLeft)
            {
                graphicsElement.Visible = true;

                Lives++;
            }

            base.CreateRectangle(new Vector2(0, 800));

            gameState = GameState.GameOver;

            SoundEffectInstances["GameOver"].Play();
        }

        public virtual void UpdateSprite()
        {
            if ((CurrentSpriteIndex < SpritesInSheet) && (Math.Abs(MovementVelocity) > Math.Abs(PreviousMovementVelocity)))
            {
                if (CurrentSpriteIndex == (SpritesInSheet - 1))
                {
                    PreviousSpriteIndex = CurrentSpriteIndex;

                    CurrentSpriteIndex = (SpritesInSheet - 2);
                }
                else
                {
                    PreviousSpriteIndex = CurrentSpriteIndex;

                    CurrentSpriteIndex++;
                }                
            }
            else if (Math.Abs(MovementVelocity) < Math.Abs(PreviousMovementVelocity))            
            {
                if (CurrentSpriteIndex != 0)
                {
                    PreviousSpriteIndex = CurrentSpriteIndex;

                    CurrentSpriteIndex--;
                }                
            }

            PreviousMovementVelocity = MovementVelocity;            
        }

        public void SelectSprite(int spriteIndex)
        {
            base.SelectionArea = new Rectangle(
                                        (spriteIndex * SpriteWidth),
                                        0,
                                        SpriteWidth,
                                        SpriteHeight
                                    );
        }

        /// <summary>
        /// Draw the sprite
        /// </summary>
        /// <param name="spriteBatch">Spritebatch Image</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                base.SpriteSheet,
                new Vector2(base.DrawLocation.X, base.DrawLocation.Y),
                base.SelectionArea,
                Color.White,
                0.0f,
                Vector2.Zero,
                0.12f,
                base.SpriteEffect,
                0.0f
            );
        }        
    }
}