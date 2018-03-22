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
/// Class Description   : GameObject class
/// Author              : Benjamin Kleynhans
/// Modified By         : Benjamin Kleynhans
/// Date                : March 13, 2018
/// Filename            : GameObject.cs
/// </summary>

namespace Game1
{
    public enum GravityDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public enum HitObstacle
    {
        Left,
        Right,
        Top,
        Bottom,
        None
    }

    public enum MovementAppliedTo
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

        public GameObject(Texture2D spriteTexture, int x, int y, int width, int height,
                          bool addGravity, float appliedMoveForce, float appliedVerticalMovementForce,
                          float appliedGravitationalAcceleration, float appliedObjectMass)
        {
            this.ObjectTexture = spriteTexture;
            this.Rectangle = new Rectangle(x, y, width, height);

            this.ApplyGravity = addGravity;

            this.GravitationalAcceleration = appliedGravitationalAcceleration;

            this.GravitationalForce = this.ObjectMass * this.GravitationalAcceleration;
            this.HorizontalMovementForce = appliedMoveForce;
            this.VerticalMovementForce = appliedVerticalMovementForce;

            this.ObjectMass = appliedObjectMass;
        }

        public float ObjectMass
        {
            get { return this.objectMass; }
            set { this.objectMass = value; }
        }

        public float GravitationalForce
        {
            get { return this.gravitationalForce; }
            set { this.gravitationalForce = value; }
        }

        public float GravitationalAcceleration
        {
            get { return this.gravitationalAcceleration; }
            set { this.gravitationalAcceleration = value; }
        }

        public float HorizontalMovementForce
        {
            get { return this.horizontalMovementForce; }
            set { this.horizontalMovementForce = value; }
        }

        public float VerticalMovementForce
        {
            get { return this.verticalMovementForce; }
            set { this.verticalMovementForce = value; }
        }

        public float CalculatedHorizontalForce
        {
            get { return this.calculatedHorizontalForce; }
            set { this.calculatedHorizontalForce = value; }
        }

        public float CalculatedVerticalForce
        {
            get { return this.calculatedVerticalForce; }
            set { this.calculatedVerticalForce = value; }
        }

        public float VerticalAcceleration
        {
            get { return this.verticalAcceleration; }
            set { this.verticalAcceleration = value; }
        }

        public float SurfaceForce
        {
            get { return this.surfaceForce; }
            set { this.surfaceForce = value; }
        }        

        public bool Falling
        {
            get { return this.falling; }
            set { this.falling = value; }
        }

        public bool ApplyGravity
        {
            get { return this.applyGravity; }
            set { this.applyGravity = value; }
        }

        public Texture2D ObjectTexture
        {
            get { return this.objectTexture; }
            set { this.objectTexture = value; }
        }

        public Rectangle Rectangle
        {
            get { return this.rectangle; }
            set { this.rectangle = value; }
        }

        public int XPosition
        {
            get { return this.Rectangle.X; }
            set
            {
                this.CreateRectangle(value, this.Rectangle.Y);                              // rectangle is a struct, therefore it
            }                                                                               // has to be recreated
        }

        public int YPosition
        {
            get { return this.Rectangle.Y; }
            set
            {
                this.CreateRectangle(this.Rectangle.X, value);                              // rectangle is a struct, therefore it
            }                                                                               // has to be recreated
        }

        public float CurrentX
        {
            get { return this.currentX; }
            set { this.currentX = value; }
        }

        public float CurrentY
        {
            get { return this.currentY; }
            set { this.currentY = value; }
        }

        public float PreviousX
        {
            get { return this.previousX; }
            set { this.previousX = value; }
        }

        public float PreviousY
        {
            get { return this.previousY; }
            set { this.previousY = value; }
        }

        public void CreateRectangle(int x, int y)
        {
            this.Rectangle = new Rectangle(x, y, this.Rectangle.Width, this.Rectangle.Height);
        }

        public void CreateRectangle(Vector2 vector2)
        {
            this.Rectangle = new Rectangle((int)vector2.X, (int)vector2.Y, this.Rectangle.Width, this.Rectangle.Height);
        }

        public virtual bool Intersects(GameObject passedGameObject)
        {
            bool returnValue = false;

            if (this.Rectangle.Intersects(passedGameObject.Rectangle))
            {
                returnValue = true;               
            }

            return returnValue;
        }

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
