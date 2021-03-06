﻿using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

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
/// Last Modified Date  : April 29, 2018
/// Filename            : Player.cs
/// </summary>

namespace Game1
{
    class Player : Character
    {
        private int JumpCount { get; set; }
        private int SlidesToCycle { get; set; }
        
        /// <summary>
        /// Default constructor.  Creates a GameObject with default values.
        /// </summary>
        /// <param name="spriteTexture">Texture2D image for object</param>
        /// <param name="x">Starting X coordinate of object</param>
        /// <param name="y">Starting Y coordinate of object</param>
        /// <param name="width">Width of object</param>
        /// <param name="height">Height of object</param>
        public Player(Texture2D spriteTexture, int spritesInSheet, int slidesToCycle, int x, int y, int width,
                        int height) :
            base(spriteTexture, x, y, width, height)
        {
            base.IsAlive = true;
            this.SlidesToCycle = slidesToCycle;            
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
        public Player(Texture2D spriteTexture, int spritesInSheet, int slidesToCycle, int x, int y, int width, int height,
                          bool addGravity) :
                base(spriteTexture, spritesInSheet, x, y, width, height, addGravity)
        {
            base.IsAlive = true;
            this.SlidesToCycle = slidesToCycle;
        }

        /// <summary>
        /// Applies movement to the player character
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns>The new rectangle where the character needs to be drawn</returns>
        public override Vector2 ApplyMovement(GameTime gameTime)
        {
            Vector2 returnValue;
                                                                                            // Apply movement based on keys pressed
            if ((CurrentKeyboardState.IsKeyDown(Keys.A)) && (PreviousKeyboardState.IsKeyUp(Keys.A)) ||
                (CurrentKeyboardState.IsKeyDown(Keys.Left)) && (PreviousKeyboardState.IsKeyUp(Keys.Left)))
            {
                base.MovementAppliedTo = MovementAppliedTo.Left;
                base.SpriteEffect = SpriteEffects.FlipHorizontally;
            }
            else if ((CurrentKeyboardState.IsKeyDown(Keys.D)) && (PreviousKeyboardState.IsKeyUp(Keys.D)) ||
                     (CurrentKeyboardState.IsKeyDown(Keys.Right)) && (PreviousKeyboardState.IsKeyUp(Keys.Right)))
            {
                base.MovementAppliedTo = MovementAppliedTo.Right;
                base.SpriteEffect = SpriteEffects.None;
            }
            else if ((CurrentKeyboardState.IsKeyUp(Keys.A)) && (PreviousKeyboardState.IsKeyDown(Keys.A)) ||
                     (CurrentKeyboardState.IsKeyUp(Keys.Left)) && (PreviousKeyboardState.IsKeyDown(Keys.Left)))
            {
                if ((!base.JumpInProgress) && (!base.HasJumped))
                {
                    base.MovementAppliedTo = MovementAppliedTo.None;
                }                
            }
            else if ((CurrentKeyboardState.IsKeyUp(Keys.D)) && (PreviousKeyboardState.IsKeyDown(Keys.D)) ||
                     (CurrentKeyboardState.IsKeyUp(Keys.Right)) && (PreviousKeyboardState.IsKeyDown(Keys.Right)))
            {
                if ((!base.JumpInProgress) && (!base.HasJumped))
                {
                    base.MovementAppliedTo = MovementAppliedTo.None;
                    base.SpriteEffect = SpriteEffects.None;
                }
            }
                                                                                            // Implement the jump feature
            if (base.GravityDirection == GravityDirection.Down)
            {
                if (((CurrentKeyboardState.IsKeyDown(Keys.Space)) && (PreviousKeyboardState.IsKeyUp(Keys.Space)) ||
                     (CurrentKeyboardState.IsKeyDown(Keys.Up)) && (PreviousKeyboardState.IsKeyUp(Keys.Up))) &&
                    ((base.HitObstacle == HitObstacle.FromTop) || (this.JumpCount == 1)))
                {
                    base.TimeSinceJump = gameTime.ElapsedGameTime.Milliseconds;

                    base.MovementAppliedTo = MovementAppliedTo.Up;

                    base.HitObstacle = HitObstacle.None;

                    if (this.PlatformVerticalAcceleration > 0)
                    {
                        base.GravitationalVelocity -= this.PlatformVerticalAcceleration;
                    }

                    if (base.HasJumped == false)
                    {
                        base.HasJumped = true;
                        SoundEffectInstances["JumpSound"].Play();
                    }

                    if (base.JumpInProgress == false)
                    {
                        base.JumpInProgress = true;
                    }

                }
            }
            else if (base.GravityDirection == GravityDirection.Up)
            {
                if (((CurrentKeyboardState.IsKeyDown(Keys.Space)) && (PreviousKeyboardState.IsKeyUp(Keys.Space)) ||
                     (CurrentKeyboardState.IsKeyUp(Keys.Up)) && (PreviousKeyboardState.IsKeyDown(Keys.Up))) &&
                    ((base.HitObstacle == HitObstacle.FromTop) || (this.JumpCount == 1)))
                {                    
                    base.MovementAppliedTo = MovementAppliedTo.Down;

                    base.HitObstacle = HitObstacle.None;

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
                                                                                            // Cancel movement if nothing is being pressed (because of velocity)
            if ((CurrentKeyboardState.IsKeyUp(Keys.A)) && (CurrentKeyboardState.IsKeyUp(Keys.D)) &&
                (CurrentKeyboardState.IsKeyUp(Keys.Left)) && (CurrentKeyboardState.IsKeyUp(Keys.Right)) &&
                (base.Falling == false) && (base.HasJumped == false) && (base.JumpInProgress == false))
            {
                base.MovementAppliedTo = MovementAppliedTo.None;
            }
                                                                                            // Attempt at working with gametime
            if (base.TimeSinceLastUpdate > 100)
            {
                this.UpdateSprite();
                base.SelectSprite(base.CurrentSpriteIndex);

                base.TimeSinceLastUpdate = 0;
            }

            base.CalculateGravity(gameTime);            
            base.CalculateMovement(gameTime);

            returnValue = new Vector2(
                this.DrawLocation.X + base.MovementVelocity,
                this.DrawLocation.Y + base.GravitationalVelocity
            );

            return returnValue;
        }

        /// <summary>
        /// Update method
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            base.TimeSinceLastUpdate += gameTime.ElapsedGameTime.Milliseconds;

            if ((this.DrawLocation.Y + this.DrawLocation.Height) > SCREENHEIGHT)            // If the player goes off the bottom of the screen, take a life
            {
                base.TakeLife();
                base.TookLife = false;

                base.CreateRectangle(PlayerSpawnPoint);
                base.MovementVelocity = 0f;
                base.GravitationalVelocity = 0f;
            }
            else if (this.DrawLocation.Y < 0)                                               // Block the player from exiting the screen on left, top and right
            {
                CreateRectangle(this.DrawLocation.X, 1);
            }
            else if (this.DrawLocation.X < 0)
            {
                CreateRectangle(1, this.DrawLocation.Y);
            }
            else if (this.DrawLocation.X + this.DrawLocation.Width > 1600)
            {
                CreateRectangle(1600 - this.DrawLocation.Width - 1, this.DrawLocation.Y);
            }
                                                                                            // If the player is alive
            if (this.IsAlive)
            {
                for (int i = 0; i < base.intersectedBy.Count; i++)
                {
                    bool stillIntersecting = base.Intersects(intersectedBy[i]);             // Confirm the intersections hasn't changed

                    if ((!stillIntersecting) && (this.HasJumped == false))
                    {
                        if ((base.intersectedBy[i].GetType() == typeof(Platform)) && (base.intersectedBy[i].GravitationalVelocity >= 0))
                        {
                            base.Falling = true;                                            // If the player is falling
                            base.HitObstacle = HitObstacle.None;                            // and hasn't hit anything
                            base.GravitationalVelocity += (GlobalAcceleration * 2);         // add gravity
                        }
                        
                        if (base.intersectedBy[i].GetType() == typeof(Enemy))               // If the player hits an enemy
                        {
                            base.TookLife = false;                                          // take a life
                        }

                        base.intersectedBy.Remove(intersectedBy[i]);
                    }
                    else if ((!stillIntersecting) & (this.HasJumped == true))
                    {
                        base.HitObstacle = HitObstacle.None;

                        base.intersectedBy.Remove(intersectedBy[i]);
                    }
                    else if ((base.HitObstacle == HitObstacle.FromBottom) && (this.HasJumped == false))
                    {
                        base.GravitationalVelocity = 0;
                        base.Falling = true;
                    }
                    else if ((intersectedBy[i].GetType() == typeof(Collectible)) && (intersectedBy[i].Visible))
                    {
                        intersectedBy[i].Visible = false;

                        if (intersectedBy[i].CollectibleType.Equals("Worm"))
                        {
                            CurrentScore += 5;
                        }
                        else
                        {
                            CurrentScore++;
                        }
                        
                    }
                    else if (intersectedBy[i].GetType() == typeof(Goal))
                    {
                        GameState = GameState.AdvanceLevel;
                    }
                }

                CreateRectangle(ApplyMovement(gameTime));
            }
            else
            {
                base.TakeLife();
            }
        }

        /// <summary>
        /// Update the image of the sprite to show the next required one for animation purposes
        /// </summary>
        public override void UpdateSprite()
        {
            if ((CurrentSpriteIndex == 0 && PreviousSpriteIndex == 0) &&
                (base.MovementAppliedTo != MovementAppliedTo.None))
            {
                CurrentSpriteIndex++;
            }
            else if (((CurrentSpriteIndex == 1 && PreviousSpriteIndex == 0) ||
                    (CurrentSpriteIndex == 2 && PreviousSpriteIndex == 1)) &&
                    (base.MovementAppliedTo != MovementAppliedTo.None))
            {
                PreviousSpriteIndex = CurrentSpriteIndex;
                CurrentSpriteIndex++;
            }
            else if (((CurrentSpriteIndex == 3 && PreviousSpriteIndex == 2) ||
                      (CurrentSpriteIndex == 2 && PreviousSpriteIndex == 3)) &&
                    (base.MovementAppliedTo != MovementAppliedTo.None))
            {
                PreviousSpriteIndex = CurrentSpriteIndex;
                CurrentSpriteIndex--;
            }
            else if ((CurrentSpriteIndex == 1 && PreviousSpriteIndex == 2) &&
                    (base.MovementAppliedTo != MovementAppliedTo.None))
            {
                PreviousSpriteIndex = CurrentSpriteIndex;
                CurrentSpriteIndex++;
            }
            else if (base.MovementAppliedTo == MovementAppliedTo.None)
            {
                CurrentSpriteIndex = 0;
                PreviousSpriteIndex = 0;
            }
        }

        /// <summary>
        /// Draw the sprite
        /// </summary>
        /// <param name="spriteBatch">Spritebatch Image</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            Color drawColor = Color.White;

            if (base.TookLife)
            {
                drawColor = (Color.Red * 0.5f);                
            }
            
            spriteBatch.Draw(
                base.SpriteSheet,
                new Vector2(base.DrawLocation.X, base.DrawLocation.Y),
                base.SelectionArea,
                drawColor,
                0.0f,
                Vector2.Zero,
                0.12f,
                base.SpriteEffect,
                0.0f
            );
            
            spriteBatch.DrawString(
                SpriteFont,
                "Score      : " + CurrentScore,
                new Vector2(SCREENWIDTH - 100, 20),
                Color.White
            );
        }
    }
}
