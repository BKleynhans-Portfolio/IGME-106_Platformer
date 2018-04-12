using System;
using System.Collections;
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
/// Filename            : GameObject.cs
/// </summary>

namespace Game1
{
    public enum GravityDirection                                                            // Gravity can be applied in these directions
    {
        Up,
        Down,
        Left,
        Right
    }

    public enum MovementAppliedTo                                                           // Movement parameters used for all object movement
    {
        Left,
        Right,
        Up,
        Down,
        None
    }

    public enum HitObstacle                                                                 // These are the intersection parameters used for gravity calculation
    {
        FromLeft,
        FromRight,
        FromTop,
        FromBottom,
        None
    }

    public enum HitNpc                                                                      // These are the intersection parameters used for gravity calculation
    {
        FromLeft,
        FromRight,
        FromTop,
        FromBottom,
        None
    }

    public enum GravityOnProximityFrom
    {
        Left,
        Right,
        Top,
        Bottom,
        Center,
        None
    }

    public enum ObjectMovement
    {
        OneDirection,
        ToAndFroUpFirst,
        ToAndFroDownFirst,
        ToAndFroLeftFirst,
        ToAndFroRightFirst
    }

    public abstract class GameObject : Game1
    {
        public GravityDirection GravityDirection { get; set; }
        public MovementAppliedTo MovementAppliedTo { get; set; }
        public HitObstacle HitObstacle { get; set; }
        public HitNpc HitNpc { get; set; }
        public GravityOnProximityFrom GravityOnProximityFrom { get; set; }
        public ObjectMovement ObjectMovement { get; set; }

        public float ObjectXMoveDistance { get; set; }
        public float ObjectYMoveDistance { get; set; }
        public float InitialXPlacement { get; set; }
        public float InitialYPlacement { get; set; }
        
        public abstract bool Intersects(GameObject passedGameObject);
        protected abstract override void Update(GameTime gameTime);

        //public int CyclesToThreshold { get; set; }                                                      // The amount of times the game loop runs during on acceleration / deceleration
        //public int AccelerationCoefficientStartingPoint { get; set; }
        //public int AccelerationCoefficientEndingPoint { get; set; }
        //public bool AccelerationCoefficientStartingXSet { get; set; }
        //public bool AccelerationCoefficientEndingXSet { get; set; }
        //public bool CalculateCoefficient { get; set; }
        //public float AccelerationCoefficient { get; set; }                        // Calculate pixels covered during acceleration

        public Texture2D SpriteSheet { get; set; }                                                    // Texture and drawLocation
        public Rectangle DrawLocation { get; set; }        
        public Rectangle SelectionArea { get; set; }
        public int SpritesInSheet { get; set; }
        public int SpriteWidth { get; set; }
        public int SpriteHeight { get; set; }
        public SpriteEffects SpriteEffect { get; set; }
        public bool Visible { get; set; }                                                               // Is this object visible in the scene
        public int CurrentSpriteIndex { get; set; }
        public int PreviousSpriteIndex { get; set; }
        public float PreviousMovementVelocity { get; set; }
        public int TimeSinceLastUpdate { get; set; }

        private float globalGlobalAcceleration;                                             // Acceleration to apply to characters during each iteration
        private float environmentalAcceleration;                                              // Acceleration to apply to platforms
        private float gravitationalVelocity;                                                // Gravitational velocity for vertical movement
        private float movementVelocity;                                                     // Movement velocity for horizontal movement

        private const float defaultHorizonalVelocity = 5f;                                  // Default velocity to implement horizontally
        private const float defaultVerticalVelocity = 5f;                                   // Default velocity to implement vertically   

        private bool applyGravity;                                                          // Should the object have gravity

        private bool falling;                                                               // Is the object falling?
        private bool jumpInProgress;                                                        // Is the object in a jump process?
        public string PlatformType { get; set; }

        /// <summary>
        /// Default constructor.  Creates a GameObject with default values.
        /// </summary>
        /// <param name="spriteTexture">Texture2D image for object</param>
        /// <param name="x">Starting X coordinate of object</param>
        /// <param name="y">Starting Y coordinate of object</param>
        /// <param name="width">Width of object</param>
        /// <param name="height">Height of object</param>
        public GameObject(Texture2D spriteTexture, int x, int y, int width, int height)
        {
            this.SpriteSheet = spriteTexture;
            this.DrawLocation = new Rectangle(x, y, width, height);

            this.Visible = true;

            this.Falling = true;

            this.ApplyGravity = false;

            this.GlobalAcceleration = 0.25f;
            this.EnvironmentalAcceleration = 0.05f;
            this.SpriteEffect = SpriteEffects.None;

            this.InitialXPlacement = x;
            this.InitialYPlacement = y;

            //this.AccelerationCoefficientStartingPoint = 0;
            //this.AccelerationCoefficientEndingPoint = 0;
            //this.AccelerationCoefficientStartingXSet = false;
            //this.AccelerationCoefficientEndingXSet = false;
            GravityDirection = GravityDirection.Down;
            MovementAppliedTo = MovementAppliedTo.None;
            HitObstacle = HitObstacle.None;
            HitNpc = HitNpc.None;
            GravityOnProximityFrom = GravityOnProximityFrom.None;
            ObjectMovement = ObjectMovement.OneDirection;            
            //CalculateCoefficient = true;
            //AccelerationCoefficient = 0;

            SpritesInSheet = 4;
            SpriteWidth = 400;
            SpriteHeight = 400;

            this.CurrentSpriteIndex = 0;
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
        public GameObject(Texture2D spriteTexture, int spritesInSheet, int x, int y, int width, int height,
                          bool addGravity)
        {
            this.SpriteSheet = spriteTexture;
            this.DrawLocation = new Rectangle(x, y, width, height);

            this.Visible = true;

            this.ApplyGravity = addGravity;

            this.GlobalAcceleration = /*2f;//*/ 0.25f;
            this.EnvironmentalAcceleration = 0.05f;

            this.InitialXPlacement = x;
            this.InitialYPlacement = y;

            //this.AccelerationCoefficientStartingPoint = 0;
            //this.AccelerationCoefficientEndingPoint = 0;
            //this.AccelerationCoefficientStartingXSet = false;
            //this.AccelerationCoefficientEndingXSet = false;
            GravityDirection = GravityDirection.Down;
            MovementAppliedTo = MovementAppliedTo.None;
            HitObstacle = HitObstacle.None;
            HitNpc = HitNpc.None;
            GravityOnProximityFrom = GravityOnProximityFrom.None;
            ObjectMovement = ObjectMovement.OneDirection;

            SpritesInSheet = spritesInSheet;
            SpriteWidth = 400;
            SpriteHeight = 400;

            this.CurrentSpriteIndex = 0;
        }

        /// <summary>
        /// Default velocity applied for horizontal movement if none is specified
        /// </summary>
        public float DefaultHorizonalVelocity
        {
            get { return defaultHorizonalVelocity; }
        }

        /// <summary>
        /// Default velocity applied for vertical movement if none is specified
        /// </summary>
        public float DefaultVerticalVelocity
        {
            get { return defaultVerticalVelocity; }
        }

        /// <summary>
        /// Is the object in a jump process
        /// </summary>
        public bool JumpInProgress
        {
            get { return this.jumpInProgress; }
            set { this.jumpInProgress = value; }
        }

        /// <summary>
        /// Gravitational velocity to apply to an object (up and down)
        /// </summary>
        public float GravitationalVelocity
        {
            get { return this.gravitationalVelocity; }
            set { this.gravitationalVelocity = value; }
        }

        /// <summary>
        /// Movement velocity to apply to an object (left and right)
        /// </summary>
        public float MovementVelocity
        {
            get { return this.movementVelocity; }
            set { this.movementVelocity = value; }
        }

        /// <summary>
        /// GlobalAcceleration to add to velocity in any of the four cardinal directions
        /// </summary>
        public float GlobalAcceleration
        {
            get { return this.globalGlobalAcceleration; }
            set { this.globalGlobalAcceleration = value; }
        }

        public float EnvironmentalAcceleration
        {
            get { return this.environmentalAcceleration; }
            set { this.environmentalAcceleration = value; }
        }
        
        /// <summary>
        /// Properties for variable containing the boolean value which is true when the object
        /// is falling and false when the object is not falling.
        /// </summary>
        public bool Falling
        {
            get { return this.falling; }
            set { this.falling = value; }
        }

        /// <summary>
        /// Properties for variable containing the boolean value which is true when gravity needs
        /// to be applied to the object and false when it should not be applied to the object.
        /// </summary>
        public bool ApplyGravity
        {
            get { return this.applyGravity; }
            set { this.applyGravity = value; }
        }

        /// <summary>
        /// Method used to recreate the drawLocation containing the location and dimensions of the object.
        /// This is required because the drawLocation is a struct datatype and we cannot change the values
        /// within the object.
        /// </summary>
        /// <param name="x">New X coordinate to draw object</param>
        /// <param name="y">New Y coordinate to draw object</param>
        public void CreateRectangle(int x, int y)
        {
            this.DrawLocation = new Rectangle(x, y, this.DrawLocation.Width, this.DrawLocation.Height);
        }

        /// <summary>
        /// Method used to recreate the drawLocation containing the location and dimensions of the object.
        /// This is required because the drawLocation is a struct datatype and we cannot change the values
        /// within the object.
        /// </summary>
        /// <param name="vector2">New vector containing new X and Y coordinates</param>
        public void CreateRectangle(Vector2 vector2)
        {
            this.DrawLocation = new Rectangle((int)vector2.X, (int)vector2.Y, this.DrawLocation.Width, this.DrawLocation.Height);            
        }
        
        /// <summary>
        /// Calculates the amount of force to apply for the object during each iteration of the game loop
        /// </summary>
        public virtual void CalculateGravity(GameTime gameTime)
        {
            if (this.ApplyGravity)                                                          // If the object requires gravity to be implemented
            {
                switch (GravityDirection)                                                   // Based on which direction the gravity is implemented in
                {
                    case GravityDirection.Up:
                        this.GravitationalVelocity -= this.GlobalAcceleration;

                        break;
                    case GravityDirection.Down:
                        this.GravitationalVelocity += this.GlobalAcceleration;

                        break;
                    case GravityDirection.Left:
                        this.MovementVelocity -= this.GlobalAcceleration;                        

                        break;

                    case GravityDirection.Right:
                        this.MovementVelocity += this.GlobalAcceleration;

                        break;
                }
            }
        }

        public void UpdateMovementParameters()
        {
            switch (HitObstacle)
            {
                case HitObstacle.FromLeft:
                    if (GravityOnProximityFrom == GravityOnProximityFrom.Left)
                        this.ApplyGravity = true;

                    break;
                case HitObstacle.FromTop:
                    if (GravityOnProximityFrom == GravityOnProximityFrom.Top)
                        this.ApplyGravity = true;

                    break;
                case HitObstacle.FromRight:
                    if (GravityOnProximityFrom == GravityOnProximityFrom.Right)
                        this.ApplyGravity = true;

                    break;
                case HitObstacle.FromBottom:
                    if (GravityOnProximityFrom == GravityOnProximityFrom.Bottom)
                        this.ApplyGravity = true;

                    break;
            }

            if (this.ApplyGravity == true)
            {
                switch (ObjectMovement)
                {
                    case ObjectMovement.ToAndFroUpFirst:
                        this.GravityDirection = GravityDirection.Up;

                        break;
                    case ObjectMovement.ToAndFroDownFirst:
                        this.GravityDirection = GravityDirection.Down;

                        break;
                    case ObjectMovement.ToAndFroLeftFirst:
                        this.GravityDirection = GravityDirection.Left;

                        break;
                    case ObjectMovement.ToAndFroRightFirst:
                        this.GravityDirection = GravityDirection.Right;

                        break;
                }
                
                switch (ObjectMovement)
                {
                    case ObjectMovement.ToAndFroRightFirst:

                        switch (GravityDirection)
                        {
                            case GravityDirection.Right:
                                if (this.DrawLocation.X >= (this.InitialXPlacement + (this.ObjectXMoveDistance / 2)))
                                {                                    
                                    SwitchDirections();
                                }

                                break;
                            case GravityDirection.Left:
                                if (this.DrawLocation.X <= (this.InitialXPlacement + (this.ObjectXMoveDistance / 2)))
                                {
                                    SwitchDirections();
                                }

                                break;
                        }

                        break;
                    case ObjectMovement.ToAndFroLeftFirst:

                        switch (GravityDirection)
                        {
                            case GravityDirection.Left:
                                if (this.DrawLocation.X <= (this.InitialXPlacement - (this.ObjectXMoveDistance / 2)))
                                {
                                    SwitchDirections();
                                }

                                break;
                            case GravityDirection.Right:
                                if (this.DrawLocation.X >= (this.InitialXPlacement - (this.ObjectXMoveDistance / 2)))
                                {
                                    SwitchDirections();
                                }

                                break;
                        }

                        break;
                    case ObjectMovement.ToAndFroDownFirst:

                        switch (GravityDirection)
                        {
                            case GravityDirection.Down:
                                if (this.DrawLocation.Y >= (this.InitialYPlacement + (this.ObjectYMoveDistance / 2)))
                                {
                                    SwitchDirections();
                                }

                                break;
                            case GravityDirection.Up:
                                if (this.DrawLocation.Y <= (this.InitialYPlacement + (this.ObjectYMoveDistance / 2)))
                                {
                                    SwitchDirections();
                                }

                                break;
                        }

                        break;
                    case ObjectMovement.ToAndFroUpFirst:
                        switch (GravityDirection)
                        {
                            case GravityDirection.Up:
                                if (this.DrawLocation.Y <= (this.InitialYPlacement - (this.ObjectYMoveDistance / 2)))
                                {
                                    SwitchDirections();
                                }

                                break;
                            case GravityDirection.Down:
                                if (this.DrawLocation.Y >= (this.InitialYPlacement - (this.ObjectYMoveDistance / 2)))
                                {
                                    SwitchDirections();
                                }

                                break;
                        }

                        break;
                }
                
            }
        }

        private void SwitchDirections()
        {
            if (this.GravityDirection == GravityDirection.Down)
            {
                this.GravityDirection = GravityDirection.Up;
            }
            else if (this.GravityDirection == GravityDirection.Up)
            {
                this.GravityDirection = GravityDirection.Down;
            }
            else if (this.GravityDirection == GravityDirection.Left)
            {
                this.GravityDirection = GravityDirection.Right;
            }
            else if (this.GravityDirection == GravityDirection.Right)
            {
                this.GravityDirection = GravityDirection.Left;
            }
        }

        /// <summary>
        /// Draw the sprite
        /// </summary>
        /// <param name="spriteBatch">Spritebatch Image</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                this.SpriteSheet,
                this.DrawLocation,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                this.SpriteEffect,
                0
            );
        }
    }
}
