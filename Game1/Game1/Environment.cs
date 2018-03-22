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
    public enum GravityOnProximityFrom
    {
        Left,
        Right,
        Top,
        Bottom,
        Center,
        None
    }

    public enum PlatformMovement
    {
        OneDirection,
        ToAndFroUpFirst,
        ToAndFroDownFirst,
        ToAndFroLeftFirst,
        ToAndFroRightFirst
    }

    abstract class Environment : Screen
    {
        public GravityOnProximityFrom gravityOnProximityFrom = GravityOnProximityFrom.None;
        public PlatformMovement platformMovement = PlatformMovement.OneDirection;

        public abstract override void Draw(SpriteBatch spriteBatch);
                
        private float objectXMoveDistance;
        private float objectYMoveDistance;
        private float initialXPlacement;
        private float initialYPlacement;

        public Environment(Texture2D spriteTexture, int x, int y, int width, int height) : base(spriteTexture, x, y, width, height)
        {
            this.InitialXPlacement = x;
            this.InitialYPlacement = y;

            this.ObjectXMoveDistance = 50;
            this.ObjectYMoveDistance = 50;
            
        }

        public Environment(Texture2D spriteTexture, int x, int y, int width, int height,
                          bool addGravity, float appliedMoveForce, float appliedVerticalMovementForce,
                          float appliedGravitationalAcceleration, float appliedObjectMass) :
                base(spriteTexture, x, y, width, height, addGravity, appliedMoveForce, appliedVerticalMovementForce,
                    appliedGravitationalAcceleration, appliedObjectMass)
        {
            this.InitialXPlacement = x;
            this.InitialYPlacement = y;

            this.ObjectXMoveDistance = 50;
            this.ObjectYMoveDistance = 50;
        }

        public float ObjectXMoveDistance
        {
            get { return this.objectXMoveDistance; }
            set { this.objectXMoveDistance = value; }
        }

        public float ObjectYMoveDistance
        {
            get { return this.objectYMoveDistance; }
            set { this.objectYMoveDistance = value; }
        }

        public float InitialXPlacement
        {
            get { return this.initialXPlacement; }
            private set { this.initialXPlacement = value; }
        }

        public float InitialYPlacement
        {
            get { return this.initialYPlacement; }
            private set { this.initialYPlacement = value; }
        }
        
        public override bool Intersects(GameObject passedGameObject)
        {
            bool returnValue = false;

            if (this.Rectangle.Intersects(passedGameObject.Rectangle))
            {
                returnValue = true;

                if (this.Rectangle.Top <= passedGameObject.Rectangle.Bottom)
                {
                    this.hitObstacle = HitObstacle.Top;
                }                
                else if (this.Rectangle.Bottom >= passedGameObject.Rectangle.Top)
                {
                    this.hitObstacle = HitObstacle.Bottom;
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
            else
            {
                returnValue = false;
            }

            return returnValue;
        }

        public virtual Vector2 ApplyMovement()
        {
            Vector2 returnValue;

            base.CalculateForces();

            returnValue = new Vector2(
                this.Rectangle.X + base.CalculatedHorizontalForce,
                this.Rectangle.Y + VerticalAcceleration
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

            switch (hitObstacle) {
                case HitObstacle.Left:
                    if (gravityOnProximityFrom == GravityOnProximityFrom.Left)
                        base.ApplyGravity = true;

                    break;
                case HitObstacle.Top:
                    if (gravityOnProximityFrom == GravityOnProximityFrom.Top)
                        base.ApplyGravity = true;

                    break;
                case HitObstacle.Right:
                    if (gravityOnProximityFrom == GravityOnProximityFrom.Right)
                        base.ApplyGravity = true;

                    break;
                case HitObstacle.Bottom:
                    if (gravityOnProximityFrom == GravityOnProximityFrom.Bottom)
                        base.ApplyGravity = true;

                    break;
            }

            if (base.ApplyGravity == true)
            {
                switch (platformMovement)
                {
                    case PlatformMovement.ToAndFroRightFirst:
                        if ((
                                (base.gravityDirection == GravityDirection.Right) && 
                                (this.Rectangle.X > (this.InitialXPlacement + this.ObjectXMoveDistance))
                                ) || (
                                (base.gravityDirection == GravityDirection.Left) && 
                                (this.Rectangle.X <= this.InitialXPlacement)
                            ))
                        {
                            SwitchDirections();
                        }

                        break;
                    case PlatformMovement.ToAndFroLeftFirst:
                        if ((
                                (base.gravityDirection == GravityDirection.Left) &&
                                (this.Rectangle.X < (this.InitialXPlacement - this.ObjectXMoveDistance))
                                ) || (
                                (base.gravityDirection == GravityDirection.Right) &&
                                (this.Rectangle.X >= this.InitialXPlacement)
                            ))
                        {
                            SwitchDirections();
                        }

                        break;
                    case PlatformMovement.ToAndFroDownFirst:
                        if ((
                                (base.gravityDirection == GravityDirection.Down) &&
                                (this.Rectangle.Y > (this.InitialYPlacement + this.ObjectYMoveDistance))
                                ) || (
                                (base.gravityDirection == GravityDirection.Up) &&
                                (this.Rectangle.Y <= this.InitialYPlacement)
                            ))
                        {
                            SwitchDirections();
                        }

                        break;
                }   
            }
        
            CreateRectangle(ApplyMovement());
        }

        private void SwitchDirections()
        {
            if (base.gravityDirection == GravityDirection.Down)
            {
                base.gravityDirection = GravityDirection.Up;
            }
            else if (base.gravityDirection == GravityDirection.Up)
            {
                base.gravityDirection = GravityDirection.Down;
            }
            else if (base.gravityDirection == GravityDirection.Left)
            {
                base.gravityDirection = GravityDirection.Right;
            }
            else if (base.gravityDirection == GravityDirection.Right)
            {
                base.gravityDirection = GravityDirection.Left;
            }
        }

        public override void CalculateForces()
        {
            if (this.ApplyGravity)
            {
                switch (gravityDirection)
                {
                    case GravityDirection.Up://This is where I'm working
                        switch (hitObstacle)
                        {
                            case HitObstacle.None:
                                this.SurfaceForce = 0;
                                this.CalculatedVerticalForce -= this.GravitationalForce;
                                this.VerticalAcceleration = this.CalculatedVerticalForce / this.ObjectMass;

                                break;
                            case HitObstacle.Bottom:
                                Console.ReadLine();
                                this.SurfaceForce = this.SurfaceForce - this.GravitationalForce - this.CalculatedVerticalForce;
                                this.SurfaceForce *= -1;                                            //Surface force pushes up and therefore should be negative
                                this.VerticalAcceleration = this.SurfaceForce / this.ObjectMass;

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

                        break;
                    case GravityDirection.Down:
                        switch (hitObstacle)
                        {
                            case HitObstacle.None:
                                this.SurfaceForce = 0;
                                this.CalculatedVerticalForce += this.GravitationalForce;
                                this.VerticalAcceleration = this.CalculatedVerticalForce / this.ObjectMass;

                                break;
                            case HitObstacle.Bottom:
                                Console.ReadLine();
                                this.SurfaceForce = this.SurfaceForce + this.GravitationalForce + this.CalculatedVerticalForce;
                                this.SurfaceForce *= -1;                                            //Surface force pushes up and therefore should be negative
                                this.VerticalAcceleration = this.SurfaceForce / this.ObjectMass;

                                break;
                            case HitObstacle.Left:
                                this.CalculatedHorizontalForce = 0;

                                break;
                            case HitObstacle.Top:
                                this.SurfaceForce = 0;
                                this.CalculatedVerticalForce += this.GravitationalForce;
                                this.VerticalAcceleration = this.CalculatedVerticalForce / this.ObjectMass;

                                break;
                            case HitObstacle.Right:
                                this.CalculatedHorizontalForce = 0;

                                break;
                        }

                        break;
                    case GravityDirection.Left:
                        this.SurfaceForce = this.GravitationalForce;

                        switch (hitObstacle)
                        {
                            case HitObstacle.None:
                                if (this.CalculatedHorizontalForce > -5)
                                {
                                    this.CalculatedHorizontalForce -= 0.2f;
                                }

                                break;
                            case HitObstacle.Bottom:
                                if (this.CalculatedHorizontalForce > -5)
                                {
                                    this.CalculatedHorizontalForce -= 0.2f;
                                }

                                break;
                            case HitObstacle.Left:
                                this.CalculatedHorizontalForce = 0;

                                break;
                            case HitObstacle.Top:
                                if (this.CalculatedHorizontalForce > -5)
                                {
                                    this.CalculatedHorizontalForce -= 0.2f;
                                }

                                break;
                            case HitObstacle.Right:
                                this.CalculatedHorizontalForce = 0;

                                break;
                        }

                        break;

                    case GravityDirection.Right:
                        this.SurfaceForce = this.GravitationalForce;

                        switch (hitObstacle)
                        {
                            case HitObstacle.None:
                                if (this.CalculatedHorizontalForce < 5)
                                {
                                    this.CalculatedHorizontalForce += 0.2f;
                                }

                                break;
                            case HitObstacle.Bottom:
                                if (this.CalculatedHorizontalForce < 5)
                                {
                                    this.CalculatedHorizontalForce += 0.2f;
                                }

                                break;
                            case HitObstacle.Left:
                                this.CalculatedHorizontalForce = 0;

                                break;
                            case HitObstacle.Top:
                                if (this.CalculatedHorizontalForce < 5)
                                {
                                    this.CalculatedHorizontalForce += 0.2f;
                                }

                                break;
                            case HitObstacle.Right:
                                this.CalculatedHorizontalForce = 0;

                                break;
                        }

                        break;
                }

            }

            CalculatedVerticalForce *= (float)(secondsPerFrame * 2);
        }
    }
}
