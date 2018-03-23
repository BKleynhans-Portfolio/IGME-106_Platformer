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

    public enum HitObstacle                                                                 // These are the intersection parameters used for gravity calculation
    {
        Left,
        Right,
        Top,
        Bottom,
        None
    }

    public enum MovementAppliedTo                                                           // Movement parameters used for all object movement
    {
        Left,
        Right,
        Up,
        Down,
        None
    }

    public abstract class GameObject : Game1
    {
        public GravityDirection gravityDirection = GravityDirection.Down;        
        public HitObstacle hitObstacle = HitObstacle.None;
        public MovementAppliedTo movementAppliedTo = MovementAppliedTo.None;

        public abstract void Draw(SpriteBatch spriteBatch);

        private Texture2D objectTexture;                                                    // Texture and rectangle
        private Rectangle rectangle;

        private float gravitationalForce;                                                   // Force of gravity on objects
        private float gravitationalAcceleration;

        private float surfaceForce;                                                         // Force object applies to surface

        private float horizontalMovementForce;                                              // Maximum force for horizontal movement (left/right movement)
        private float calculatedHorizontalForce;                                            // Horizontal force that will be applied to character each loop

        private float verticalMovementForce;                                                // Maximum force for upward movement (jumping)
        private float calculatedVerticalForce;                                              // Vertical force that will be applied to character each loop
        private float verticalAcceleration;                                                 // Acceleration of object in vertical direction

        private float objectMass;                                                           // Mass of objects                

        private bool applyGravity;                                                          // Should the object have gravity

        private bool falling;                                                               // Is the object falling?

        private float currentX;
        private float currentY;

        private float previousX;
        private float previousY;

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

            this.Falling = true;

            this.ApplyGravity = false;

            this.GravitationalAcceleration = 9.8f;
            this.HorizontalMovementForce = 3.0f;
            this.VerticalMovementForce = 1000.0f;

            this.ObjectMass = 50;

            this.GravitationalForce = this.ObjectMass * this.GravitationalAcceleration;
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
        /// <param name="appliedMoveForce">The force that is used to calculate horizontal movement</param>
        /// <param name="appliedVerticalMovementForce">The force that is used to calculate vertical movement.  This
        ///                 is the jump strength in case of characters</param>
        /// <param name="appliedGravitationalAcceleration">This is the unit used for gravitational acceleration</param>
        /// <param name="appliedObjectMass">This is the mass that should be applied to the object</param>
        public GameObject(Texture2D spriteTexture, int x, int y, int width, int height,
                          bool addGravity, float appliedHorizontalMoveForce, float appliedVerticalMovementForce,
                          float appliedGravitationalAcceleration, float appliedObjectMass)
        {
            this.ObjectTexture = spriteTexture;
            this.Rectangle = new Rectangle(x, y, width, height);

            this.ApplyGravity = addGravity;

            this.GravitationalAcceleration = appliedGravitationalAcceleration;

            this.GravitationalForce = this.ObjectMass * this.GravitationalAcceleration;
            this.HorizontalMovementForce = appliedHorizontalMoveForce;
            this.VerticalMovementForce = appliedVerticalMovementForce;

            this.ObjectMass = appliedObjectMass;
        }

        /// <summary>
        /// Properties for variable containing the mass of the object
        /// </summary>
        public float ObjectMass
        {
            get { return this.objectMass; }
            set { this.objectMass = value; }
        }

        /// <summary>
        /// Properties for variable containing the gravitational force applied to the object
        /// </summary>
        public float GravitationalForce
        {
            get { return this.gravitationalForce; }
            set { this.gravitationalForce = value; }
        }

        /// <summary>
        /// Properties for variable containing the gravitational acceleration applied to the object
        /// </summary>
        public float GravitationalAcceleration
        {
            get { return this.gravitationalAcceleration; }
            set { this.gravitationalAcceleration = value; }
        }

        /// <summary>
        /// Properties for variable containing the default horizontal movement force
        /// </summary>
        public float HorizontalMovementForce
        {
            get { return this.horizontalMovementForce; }
            set { this.horizontalMovementForce = value; }
        }

        /// <summary>
        /// Properties for variable containing the default vertical movement force
        /// </summary>
        public float VerticalMovementForce
        {
            get { return this.verticalMovementForce; }
            set { this.verticalMovementForce = value; }
        }

        /// <summary>
        /// Properties for variable containing this force is calculated using all applied forces
        /// to determine what horizontal force to apply to the object during a specific instance
        /// in the game.
        /// </summary>
        public float CalculatedHorizontalForce
        {
            get { return this.calculatedHorizontalForce; }
            set { this.calculatedHorizontalForce = value; }
        }

        /// <summary>
        /// Properties for variable containing this force is calculated using all applied forces
        /// to determine what vertical force to apply to the object during a specific instance
        /// in the game.
        /// </summary>
        public float CalculatedVerticalForce
        {
            get { return this.calculatedVerticalForce; }
            set { this.calculatedVerticalForce = value; }
        }

        /// <summary>
        /// Properties for variable containing the vertical acceleration applied to the object
        /// </summary>
        public float VerticalAcceleration
        {
            get { return this.verticalAcceleration; }
            set { this.verticalAcceleration = value; }
        }

        /// <summary>
        /// Properties for variable containing the surface (normal) force
        /// </summary>
        public float SurfaceForce
        {
            get { return this.surfaceForce; }
            set { this.surfaceForce = value; }
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
        /// Checks whether the current object (this) intersects with any other object (the one passed in)
        /// </summary>
        /// <param name="passedGameObject">Object of type GameObject to use for intersect checking</param>
        /// <returns>TRUE if the two objects intersect and FALSE if they do not</returns>
        public virtual bool Intersects(GameObject passedGameObject)
        {
            bool returnValue = false;

            if (this.Rectangle.Intersects(passedGameObject.Rectangle))
            {
                returnValue = true;               
            }

            return returnValue;
        }

        /// <summary>
        /// Calculates the amount of force to apply for the object during each iteration of the game loop
        /// </summary>
        public virtual void CalculateForces()
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

        protected override void Update(GameTime gameTime)
        {
            
        }        
    }
}
