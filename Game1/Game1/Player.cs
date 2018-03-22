using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Game1 - Platformer for Learning
/// Class Description   : Player class
/// Author              : Benjamin Kleynhans
/// Modified By         : Benjamin Kleynhans
/// Date                : March 13, 2018
/// Filename            : Player.cs
/// </summary>

namespace Game1
{
    class Player : Character
    {
        public Player(Texture2D spriteTexture, int x, int y, int width, int height) : base(spriteTexture, x, y, width, height)
        {
            base.IsAlive = true;
            base.JumpsAllowed = 2;
            base.JumpCount = 0;
        }

        public Player(Texture2D spriteTexture, int x, int y, int width, int height,
                          bool addGravity, float appliedMoveForce, float appliedVerticalMovementForce,
                          float appliedGravitationalAcceleration, float appliedObjectMass) :
                base(spriteTexture, x, y, width, height, addGravity, appliedMoveForce, appliedVerticalMovementForce,
                    appliedGravitationalAcceleration, appliedObjectMass)
        {
            base.IsAlive = true;
            base.JumpsAllowed = 2;
            base.JumpCount = 0;
        }

        protected override void Die()
        {
            Console.WriteLine("Player Died");

            base.CreateRectangle(new Vector2(50, 50));
            this.IsAlive = true;
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
            else if (((currentKeyboardState.IsKeyDown(Keys.Space)) && (previousKeyboardState.IsKeyUp(Keys.Space))) &&
                    ((this.hitObstacle == HitObstacle.Bottom) || (this.JumpCount == 1)))
            {
                base.movementAppliedTo = MovementAppliedTo.Up;

                this.hitObstacle = HitObstacle.None;

                if (this.HasJumped == false)
                {
                    this.HasJumped = true;
                    base.CalculatedVerticalForce = (base.VerticalMovementForce * -1);
                    this.CalculatedVerticalForce += this.GravitationalForce;
                }
            }
            else if ((currentKeyboardState.IsKeyUp(Keys.A)) && (currentKeyboardState.IsKeyUp(Keys.D)))
            {
                base.movementAppliedTo = MovementAppliedTo.None;
            }

            if ((base.CalculatedHorizontalForce >= -5) && (movementAppliedTo == MovementAppliedTo.Left))
            {
                base.CalculatedHorizontalForce -= 5;
            }
            else if ((base.CalculatedHorizontalForce <= 5) && (movementAppliedTo == MovementAppliedTo.Right))
            {
                base.CalculatedHorizontalForce += 5;
            }
            else if (((base.CalculatedHorizontalForce > 0) || (base.CalculatedHorizontalForce < 0)) && (movementAppliedTo == MovementAppliedTo.None))
            {
                base.CalculatedHorizontalForce = 0;
            }

            base.CalculateForces();

            returnValue = new Vector2(
                this.Rectangle.X + base.CalculatedHorizontalForce,
                this.Rectangle.Y + VerticalAcceleration
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
                        base.Falling = true;
                        base.hitObstacle = HitObstacle.None;

                        base.intersectedBy.Remove(intersectedBy[i]);
                    } else if ((!stillIntersecting) & (this.HasJumped == true))
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
