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
                
        private float objectXMoveDistance;
        private float objectYMoveDistance;
        private float initialXPlacement;
        private float initialYPlacement;

        /// <summary>
        /// Default constructor.  Creates a GameObject with default values.
        /// </summary>
        /// <param name="spriteTexture">Texture2D image for object</param>
        /// <param name="x">Starting X coordinate of object</param>
        /// <param name="y">Starting Y coordinate of object</param>
        /// <param name="width">Width of object</param>
        /// <param name="height">Height of object</param>
        public Environment(Texture2D spriteTexture, int x, int y, int width, int height) : base(spriteTexture, x, y, width, height)
        {
            this.InitialXPlacement = x;
            this.InitialYPlacement = y;

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
        /// <param name="appliedObjectMass">This is the mass that should be applied to the object</param>
        public Environment(Texture2D spriteTexture, int x, int y, int width, int height,
                          bool addGravity, float appliedObjectMass) :
                base(spriteTexture, x, y, width, height, addGravity, appliedObjectMass)
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

        public virtual Vector2 ApplyMovement()
        {
            Vector2 returnValue;

            base.CalculateGravity();

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

                //if ((intersectedBy[i].GetType() == typeof(Player)) && (this.ApplyGravity == true) && 
                //    ((base.gravityDirection == GravityDirection.Left) || (base.gravityDirection == GravityDirection.Right)))
                //{
                //    intersectedBy[i].Rectangle = new Rectangle(
                //        (int)((base.MovementVelocity + intersectedBy[i].Rectangle.X) - 1),
                //        this.Rectangle.Y - intersectedBy[i].Rectangle.Height,
                //        intersectedBy[i].Rectangle.Width,
                //        intersectedBy[i].Rectangle.Height);
                //}

                if (!stillIntersecting)
                {   
                    base.hitObstacle = HitObstacle.None;
                                        
                    base.intersectedBy.Remove(intersectedBy[i]);
                }
                
            }

            switch (hitObstacle) {
                case HitObstacle.FromLeft:
                    if (gravityOnProximityFrom == GravityOnProximityFrom.Left)
                        base.ApplyGravity = true;

                    break;
                case HitObstacle.FromTop:
                    if (gravityOnProximityFrom == GravityOnProximityFrom.Top)
                        base.ApplyGravity = true;

                    break;
                case HitObstacle.FromRight:
                    if (gravityOnProximityFrom == GravityOnProximityFrom.Right)
                        base.ApplyGravity = true;

                    break;
                case HitObstacle.FromBottom:
                    if (gravityOnProximityFrom == GravityOnProximityFrom.Bottom)
                        base.ApplyGravity = true;

                    break;
            }

            if (base.ApplyGravity == true)
            {
                switch (platformMovement)
                {   
                    case PlatformMovement.ToAndFroUpFirst:
                        base.gravityDirection = GravityDirection.Up;

                        break;
                    case PlatformMovement.ToAndFroDownFirst:
                        base.gravityDirection = GravityDirection.Down;

                        break;
                    case PlatformMovement.ToAndFroLeftFirst:
                        base.gravityDirection = GravityDirection.Left;

                        break;
                    case PlatformMovement.ToAndFroRightFirst:
                        base.gravityDirection = GravityDirection.Right;

                        break;
                }

                switch (platformMovement)
                {
                    case PlatformMovement.ToAndFroRightFirst:
                        if ((
                                (base.gravityDirection == GravityDirection.Right) && 
                                (this.Rectangle.X >= (this.InitialXPlacement + this.ObjectXMoveDistance))
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
                                (this.Rectangle.X <= (this.InitialXPlacement - this.ObjectXMoveDistance))
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
                                (this.Rectangle.Y >= (this.InitialYPlacement + this.ObjectYMoveDistance))
                            ) || (
                                (base.gravityDirection == GravityDirection.Up) &&
                                (this.Rectangle.Y <= this.InitialYPlacement)
                            ))
                        {
                            SwitchDirections();
                        }

                        break;
                    case PlatformMovement.ToAndFroUpFirst:
                        if ((
                                (base.gravityDirection == GravityDirection.Up) &&
                                (this.Rectangle.Y <= (this.InitialYPlacement - this.ObjectYMoveDistance))
                            ) || (
                                (base.gravityDirection == GravityDirection.Down) &&
                                (this.Rectangle.Y >= this.InitialYPlacement)
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
    }
}
