using System;
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
/// Last Modified Date  : March 25, 2018
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

        public override Vector2 ApplyMovement(GameTime gameTime)
        {
            Vector2 returnValue;

            //this.SelectSprite(0);

            if ((CurrentKeyboardState.IsKeyDown(Keys.A)) && (PreviousKeyboardState.IsKeyUp(Keys.A)))
            {
                base.MovementAppliedTo = MovementAppliedTo.Left;
                base.SpriteEffect = SpriteEffects.FlipHorizontally;
            }
            else if ((CurrentKeyboardState.IsKeyDown(Keys.D)) && (PreviousKeyboardState.IsKeyUp(Keys.D)))
            {
                base.MovementAppliedTo = MovementAppliedTo.Right;
                base.SpriteEffect = SpriteEffects.None;
            }
            else if ((CurrentKeyboardState.IsKeyUp(Keys.A)) && (PreviousKeyboardState.IsKeyDown(Keys.A)))
            {
                if ((!base.JumpInProgress) && (!base.HasJumped))
                {
                    base.MovementAppliedTo = MovementAppliedTo.None;
                }                
            }
            else if ((CurrentKeyboardState.IsKeyUp(Keys.D)) && (PreviousKeyboardState.IsKeyDown(Keys.D)))
            {
                if ((!base.JumpInProgress) && (!base.HasJumped))
                {
                    base.MovementAppliedTo = MovementAppliedTo.None;
                    base.SpriteEffect = SpriteEffects.None;
                }
            }

            if (base.GravityDirection == GravityDirection.Down)
            {
                if (((CurrentKeyboardState.IsKeyDown(Keys.Space)) && (PreviousKeyboardState.IsKeyUp(Keys.Space))) &&
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
                if (((CurrentKeyboardState.IsKeyDown(Keys.Space)) && (PreviousKeyboardState.IsKeyUp(Keys.Space))) &&
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

            if ((CurrentKeyboardState.IsKeyUp(Keys.A)) && (CurrentKeyboardState.IsKeyUp(Keys.D)) &&
                (base.Falling == false) && (base.HasJumped == false) && (base.JumpInProgress == false))
            {
                base.MovementAppliedTo = MovementAppliedTo.None;
            }

            if (base.TimeSinceLastUpdate > 100)
            {
                UpdateMovementParameters();
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

        protected override void Update(GameTime gameTime)
        {
            base.TimeSinceLastUpdate += gameTime.ElapsedGameTime.Milliseconds;

            if ((this.DrawLocation.Y + this.DrawLocation.Height) > SCREENHEIGHT)
            {
                base.TakeLife();
                base.TookLife = false;

                CreateRectangle(new Vector2(0, 800));
                base.MovementVelocity = 0f;
                base.GravitationalVelocity = 0f;
            }
            else if (this.DrawLocation.Y < 0)
            {
                CreateRectangle(this.DrawLocation.X, 1);
            }
            else if (this.DrawLocation.X < 0)
            {
                CreateRectangle(1, this.DrawLocation.Y);
            }
            else if (this.DrawLocation.X > 1600)
            {
                CreateRectangle(1599, this.DrawLocation.Y);
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
                            base.HitObstacle = HitObstacle.None;
                            base.GravitationalVelocity += (GlobalAcceleration * 2);
                        }
                        
                        if (base.intersectedBy[i].GetType() == typeof(Enemy))
                        {
                            base.TookLife = false;
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
                }

                CreateRectangle(ApplyMovement(gameTime));
            }
            else
            {
                base.TakeLife();
            }
        }

        public override void UpdateSprite()
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
                0.1f,
                base.SpriteEffect,
                0.0f
            );
        }
    }
}
