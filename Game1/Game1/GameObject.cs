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

    public enum HitNPC                                                                      // These are the intersection parameters used for gravity calculation
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
        public GravityDirection gravityDirection = GravityDirection.Down;
        public MovementAppliedTo movementAppliedTo = MovementAppliedTo.None;
        public HitObstacle hitObstacle = HitObstacle.None;
        public HitNPC hitNPC = HitNPC.None;
        public GravityOnProximityFrom gravityOnProximityFrom = GravityOnProximityFrom.None;
        public ObjectMovement objectMovement = ObjectMovement.OneDirection;

        private float objectXMoveDistance;
        private float objectYMoveDistance;
        private float initialXPlacement;
        private float initialYPlacement;

        public abstract bool Intersects(GameObject passedGameObject);
        protected abstract override void Update(GameTime gameTime);

        private int cyclesToThreshold;                                                      // The amount of times the game loop runs during on acceleration / deceleration
        private int accelerationCoefficientStartingPoint = 0;
        private int accelerationCoefficientEndingPoint = 0;
        private bool accelerationCoefficientStartingXSet = false;
        private bool accelerationCoefficientEndingXSet = false;
        private bool calculateCoefficient = true;
        private int accelerationCoefficient = 0;                                                    // Calculate pixels covered during acceleration

        private Texture2D objectTexture;                                                    // Texture and rectangle
        private Rectangle rectangle;
        private SpriteEffects spriteEffect;
        private bool visible;                                                               // Is this object visible in the scene

        private float globalGlobalAcceleration;                                             // Acceleration to apply to characters during each iteration
        private float environmentalAcceleration;                                              // Acceleration to apply to platforms
        private float gravitationalVelocity;                                                // Gravitational velocity for vertical movement
        private float movementVelocity;                                                     // Movement velocity for horizontal movement
        
        private const float defaultHorizonalVelocity = 5f;                                  // Default velocity to implement horizontally
        private const float defaultVerticalVelocity = 5f;                                   // Default velocity to implement vertically   

        private bool applyGravity;                                                          // Should the object have gravity

        private bool falling;                                                               // Is the object falling?
        private bool jumpInProgress;                                                        // Is the object in a jump process?

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
            this.ObjectTexture = spriteTexture;
            this.Rectangle = new Rectangle(x, y, width, height);

            this.Visible = true;

            this.Falling = true;

            this.ApplyGravity = false;

            this.GlobalAcceleration = 0.25f;
            this.EnvironmentalAcceleration =  0.05f;
            this.spriteEffect = SpriteEffects.None;

            this.InitialXPlacement = x;
            this.InitialYPlacement = y;

            this.AccelerationCoefficientStartingPoint = 0;
            this.AccelerationCoefficientEndingPoint = 0;
            this.AccelerationCoefficientStartingXSet = false;
            this.AccelerationCoefficientEndingXSet = false;
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
        public GameObject(Texture2D spriteTexture, int x, int y, int width, int height,
                          bool addGravity)
        {
            this.ObjectTexture = spriteTexture;
            this.Rectangle = new Rectangle(x, y, width, height);

            this.Visible = true;

            this.ApplyGravity = addGravity;

            this.GlobalAcceleration = 0.25f;
            this.EnvironmentalAcceleration = 0.05f;

            this.InitialXPlacement = x;
            this.InitialYPlacement = y;

            this.AccelerationCoefficientStartingPoint = 0;
            this.AccelerationCoefficientEndingPoint = 0;
            this.AccelerationCoefficientStartingXSet = false;
            this.AccelerationCoefficientEndingXSet = false;
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
        /// Properties for variable containing the texture used for the object
        /// </summary>
        public Texture2D ObjectTexture
        {
            get { return this.objectTexture; }
            set { this.objectTexture = value; }
        }

        /// <summary>
        /// Properties for variable containing the rectangle (X, Y, Width and Height) of the object
        /// </summary>
        public Rectangle Rectangle
        {
            get { return this.rectangle; }
            set { this.rectangle = value; }
        }

        /// <summary>
        /// Object containing sprite effects used for flipping
        /// </summary>
        public SpriteEffects SpriteEffect
        {
            get { return this.spriteEffect; }
            set { this.spriteEffect = value; }
        }

        public int CyclesToThreshold
        {
            get { return this.cyclesToThreshold; }
            set { this.cyclesToThreshold = value; }
        }

        public int AccelerationCoefficientStartingPoint
        {
            get { return accelerationCoefficientStartingPoint; }
            set { this.accelerationCoefficientStartingPoint = value; }
        }

        public int AccelerationCoefficientEndingPoint
        {
            get { return this.accelerationCoefficientEndingPoint; }
            set { this.accelerationCoefficientEndingPoint = value; }
        }

        public bool AccelerationCoefficientStartingXSet
        {
            get { return this.accelerationCoefficientStartingXSet; }
            set { this.accelerationCoefficientStartingXSet = value; }
        }

        public bool AccelerationCoefficientEndingXSet
        {
            get { return this.accelerationCoefficientEndingXSet; }
            set { this.accelerationCoefficientEndingXSet = value; }
        }
        

        public bool CalculateCoefficient
        {
            get { return this.calculateCoefficient; }
            set { this.calculateCoefficient = value; }
        }

        public int AccelerationCoefficient
        {
            get { return accelerationCoefficient; }
            set { this.accelerationCoefficient = value; }
        }

        public bool Visible
        {
            get { return this.visible; }
            set { this.visible = value; }
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

        /// <summary>
        /// Method used to recreate the rectangle containing the location and dimensions of the object.
        /// This is required because the rectangle is a struct datatype and we cannot change the values
        /// within the object.
        /// </summary>
        /// <param name="x">New X coordinate to draw object</param>
        /// <param name="y">New Y coordinate to draw object</param>
        public void CreateRectangle(int x, int y)
        {
            this.Rectangle = new Rectangle(x, y, this.Rectangle.Width, this.Rectangle.Height);
        }

        /// <summary>
        /// Method used to recreate the rectangle containing the location and dimensions of the object.
        /// This is required because the rectangle is a struct datatype and we cannot change the values
        /// within the object.
        /// </summary>
        /// <param name="vector2">New vector containing new X and Y coordinates</param>
        public void CreateRectangle(Vector2 vector2)
        {
            this.Rectangle = new Rectangle((int)vector2.X, (int)vector2.Y, this.Rectangle.Width, this.Rectangle.Height);            
        }

        /// <summary>
        /// Calculates the amount of force to apply for the object during each iteration of the game loop
        /// </summary>
        public virtual void CalculateGravity()
        {            
            if (this.ApplyGravity)                                                          // If the object requires gravity to be implemented
            {                
                switch (gravityDirection)                                                   // Based on which direction the gravity is implemented in
                {
                    case GravityDirection.Up:                        
                        if ((hitObstacle == HitObstacle.FromBottom) &&                      // If this object is hit from the bottom and it is not an environment object
                            (this.GetType().BaseType != typeof(Environment)))
                        {                            
                            this.GravitationalVelocity = 0;                                 // Set gravitational velocity to 0
                        }
                        else
                        {
                            if ((this.GetType().BaseType == typeof(Environment)) &&
                                (this.GravitationalVelocity > -this.DefaultVerticalVelocity))
                            {
                                this.GravitationalVelocity -= this.EnvironmentalAcceleration;
                            }
                            else if (this.GravitationalVelocity > -this.DefaultVerticalVelocity)                            // If it is not hit, or it is hit and is an environment object, apply appropriate gravity
                            {
                                this.GravitationalVelocity -= this.GlobalAcceleration;
                            }
                        }

                        break;
                    case GravityDirection.Down:
                        if ((hitObstacle == HitObstacle.FromTop) &&                         // If this object is hit from the top and it is not an environment object
                            (this.GetType().BaseType != typeof(Environment)))
                        {                            
                            this.GravitationalVelocity = 0;                                 // Set gravitational velocity to 0
                        }
                        else
                        {
                            if ((this.GetType().BaseType == typeof(Environment)) &&
                                (this.GravitationalVelocity < this.DefaultVerticalVelocity))
                            {
                                this.GravitationalVelocity += this.EnvironmentalAcceleration;                                
                            }
                            else if (this.GravitationalVelocity < this.DefaultVerticalVelocity)                             // If it is not hit, or it is hit and is an environment object, apply appropriate gravity
                            {
                                this.GravitationalVelocity += this.GlobalAcceleration;
                            }
                        }

                        break;
                    case GravityDirection.Left:                        
                        if ((hitObstacle == HitObstacle.FromRight) &&                       // If this object is hit from the right and it is not an environment object
                            (this.GetType().BaseType != typeof(Environment)))
                        {                            
                            this.MovementVelocity = 0;                                      // Set gravitational velocity to 0
                        }
                        else
                        {
                            if ((this.GetType().BaseType == typeof(Environment)) &&
                                (this.MovementVelocity > -this.DefaultHorizonalVelocity))
                            {                                
                                this.MovementVelocity -= this.EnvironmentalAcceleration;
                                this.CyclesToThreshold++;

                                if ((this.MovementVelocity > -0.02) && (this.MovementVelocity < 0.02))
                                {
                                    CyclesToThreshold = 0;
                                }
                            }
                            else if (this.MovementVelocity > -this.DefaultHorizonalVelocity)                                 // If it is not hit, or it is hit and is an environment object, apply appropriate gravity
                            {
                                this.MovementVelocity -= this.GlobalAcceleration;
                            }
                        }

                        break;

                    case GravityDirection.Right:                        
                        if ((hitObstacle == HitObstacle.FromLeft) &&                        // If this object is hit from the left and it is not an environment object
                            (this.GetType().BaseType != typeof(Environment)))
                        {                            
                            this.MovementVelocity = 0;                                      // Set gravitational velocity to 0
                        }
                        else
                        {
                            if ((this.GetType().BaseType == typeof(Environment)) &&
                                (this.MovementVelocity < this.DefaultHorizonalVelocity))
                            {
                                this.MovementVelocity += this.EnvironmentalAcceleration;
                                this.CyclesToThreshold++;

                                if ((this.MovementVelocity > -0.02) && (this.MovementVelocity < 0.02))
                                {
                                    CyclesToThreshold = 0;
                                }
                            } 
                            else if (this.MovementVelocity < this.DefaultHorizonalVelocity)                                  // If it is not hit, or it is hit and is an environment object, apply appropriate gravity
                            {
                                this.MovementVelocity += this.GlobalAcceleration;
                            }
                        }

                        break;
                }
            }
        }
        
        public void UpdateMovementParameters()
        {
            switch (hitObstacle)
            {
                case HitObstacle.FromLeft:
                    if (gravityOnProximityFrom == GravityOnProximityFrom.Left)
                        this.ApplyGravity = true;

                    break;
                case HitObstacle.FromTop:
                    if (gravityOnProximityFrom == GravityOnProximityFrom.Top)
                        this.ApplyGravity = true;

                    break;
                case HitObstacle.FromRight:
                    if (gravityOnProximityFrom == GravityOnProximityFrom.Right)
                        this.ApplyGravity = true;

                    break;
                case HitObstacle.FromBottom:
                    if (gravityOnProximityFrom == GravityOnProximityFrom.Bottom)
                        this.ApplyGravity = true;

                    break;
            }

            if (this.ApplyGravity == true)
            {
                switch (objectMovement)
                {
                    case ObjectMovement.ToAndFroUpFirst:
                        this.gravityDirection = GravityDirection.Up;

                        break;
                    case ObjectMovement.ToAndFroDownFirst:
                        this.gravityDirection = GravityDirection.Down;

                        break;
                    case ObjectMovement.ToAndFroLeftFirst:
                        this.gravityDirection = GravityDirection.Left;

                        break;
                    case ObjectMovement.ToAndFroRightFirst:
                        this.gravityDirection = GravityDirection.Right;

                        break;
                }

                if ((this.ToString().Equals("Game1.Platform") && (this.objectMovement == ObjectMovement.ToAndFroRightFirst)) && this.CalculateCoefficient == true)
                {
                    Console.ReadLine();
                }

                if ((this.ToString().Equals("Game1.Enemy") && (this.objectMovement == ObjectMovement.ToAndFroLeftFirst)) && this.CalculateCoefficient == true)
                {
                    Console.ReadLine();
                }

                if (((this.MovementVelocity == 0) && (!this.AccelerationCoefficientStartingXSet)) && this.CalculateCoefficient == true)
                {
                    this.AccelerationCoefficientStartingPoint = this.Rectangle.X;

                    this.AccelerationCoefficientStartingXSet = true;
                }
                else if ((((this.MovementVelocity >= 4.98) && (this.MovementVelocity < 5.02)) ||
                        ((this.MovementVelocity <= -4.98) && (this.MovementVelocity > -5.02))) && 
                        (!this.AccelerationCoefficientEndingXSet))
                {
                    this.AccelerationCoefficientEndingPoint = this.Rectangle.X;

                    this.AccelerationCoefficientEndingXSet = true;
                }

                if (this.AccelerationCoefficientStartingXSet && this.AccelerationCoefficientEndingXSet)
                {
                    this.AccelerationCoefficient = Math.Abs(this.AccelerationCoefficientEndingPoint - this.AccelerationCoefficientStartingPoint);
                    this.AccelerationCoefficient --;

                    this.AccelerationCoefficientStartingXSet = false;
                    this.AccelerationCoefficientEndingXSet = false;
                    this.CalculateCoefficient = false;
                }

                switch (objectMovement)
                {
                    case ObjectMovement.ToAndFroRightFirst:

                        switch (gravityDirection)
                        {
                            case GravityDirection.Right:
                                if ((this.Rectangle.X + this.AccelerationCoefficient) >= (this.InitialXPlacement + this.ObjectXMoveDistance))
                                {
                                    SwitchDirections();
                                }

                                break;
                            case GravityDirection.Left:
                                if (this.Rectangle.X - this.AccelerationCoefficient <= this.InitialXPlacement)
                                {
                                    SwitchDirections();
                                }

                                break;
                        }

                        break;
                    case ObjectMovement.ToAndFroLeftFirst:

                        switch (gravityDirection)
                        {
                            case GravityDirection.Left:
                                if ((this.Rectangle.X - this.AccelerationCoefficient) <= (this.InitialXPlacement - this.ObjectXMoveDistance))
                                {
                                    SwitchDirections();
                                }

                                break;
                            case GravityDirection.Right:                            
                                if (this.Rectangle.X + this.AccelerationCoefficient >= this.InitialXPlacement)
                                {
                                    SwitchDirections();
                                }

                                break;
                        }

                        break;
                    case ObjectMovement.ToAndFroDownFirst:

                        switch (gravityDirection)
                        {
                            case GravityDirection.Down:
                                if (this.Rectangle.Y + this.AccelerationCoefficient >= (this.InitialYPlacement + this.ObjectYMoveDistance))
                                {
                                    SwitchDirections();
                                }

                                break;
                            case GravityDirection.Up:
                                if (this.Rectangle.Y - this.AccelerationCoefficient <= this.InitialYPlacement)
                                {
                                    SwitchDirections();
                                }

                                break;
                        }

                        break;
                    case ObjectMovement.ToAndFroUpFirst:
                        switch (gravityDirection)
                        {
                            case GravityDirection.Up:
                                if (this.Rectangle.Y + this.AccelerationCoefficient <= (this.InitialYPlacement - this.ObjectYMoveDistance))
                                {
                                    SwitchDirections();
                                }

                                break;
                            case GravityDirection.Down:
                                if (this.Rectangle.Y - this.AccelerationCoefficient >= this.InitialYPlacement)
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
            if (this.gravityDirection == GravityDirection.Down)
            {
                this.gravityDirection = GravityDirection.Up;
            }
            else if (this.gravityDirection == GravityDirection.Up)
            {
                this.gravityDirection = GravityDirection.Down;
            }
            else if (this.gravityDirection == GravityDirection.Left)
            {
                this.gravityDirection = GravityDirection.Right;
            }
            else if (this.gravityDirection == GravityDirection.Right)
            {
                this.gravityDirection = GravityDirection.Left;
            }
        }

        /// <summary>
        /// Draw the sprite
        /// </summary>
        /// <param name="spriteBatch">Spritebatch Image</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                this.ObjectTexture,
                this.Rectangle,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                spriteEffect,
                0
            );
        }
    }
}
