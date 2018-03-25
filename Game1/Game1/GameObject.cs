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

    public abstract class GameObject : Game1
    {
        public GravityDirection gravityDirection = GravityDirection.Down;
        public MovementAppliedTo movementAppliedTo = MovementAppliedTo.None;
        public HitObstacle hitObstacle = HitObstacle.None;
        public HitNPC hitNPC = HitNPC.None;

        public abstract void Draw(SpriteBatch spriteBatch);

        private Texture2D objectTexture;                                                    // Texture and rectangle
        private Rectangle rectangle;

        private float acceleration;
        private float gVelocity;        
        private float aVelocity;

        private float actionTime;

        private const float defaultHorizonalVelocity = 5f;
        private const float defaultVerticalVelocity = 5f;

        private float objectMass;                                                           // Mass of objects                

        private bool applyGravity;                                                          // Should the object have gravity

        private bool falling;                                                               // Is the object falling?
        private bool jumpInProgress;

        private int lives;

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

            this.ObjectMass = 50;

            this.ActionTime = 2f;
            this.Acceleration = 0.25f;
            this.Lives = 3;
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
                          bool addGravity, float appliedObjectMass)
        {
            this.ObjectTexture = spriteTexture;
            this.Rectangle = new Rectangle(x, y, width, height);

            this.ApplyGravity = addGravity;

            this.ObjectMass = appliedObjectMass;
        }

        public float DefaultHorizonalVelocity
        {
            get { return defaultHorizonalVelocity; }
        }

        public float DefaultVerticalVelocity
        {
            get { return defaultVerticalVelocity; }
        }

        public int Lives
        {
            get { return this.lives; }
            set { this.lives = value; }
        }
        
        public bool JumpInProgress
        {
            get { return this.jumpInProgress; }
            set { this.jumpInProgress = value; }
        }

        public float ActionTime
        {
            get { return this.actionTime; }
            set { this.actionTime = value; }
        }

        public float GVelocity
        {
            get { return this.gVelocity; }
            set { this.gVelocity = value; }
        }

        public float AVelocity
        {
            get { return this.aVelocity; }
            set { this.aVelocity = value; }
        }

        public float Acceleration
        {
            get { return this.acceleration; }
            set { this.acceleration = value; }
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

        public virtual bool Intersects(GameObject passedGameObject)
        {
            bool returnValue = false;

            if (this.Rectangle.Intersects(passedGameObject.Rectangle))
            {
                returnValue = true;

                float newX;
                float newY;
                                
                if ((this.Rectangle.Bottom > passedGameObject.Rectangle.Top) && (this.Rectangle.Bottom != passedGameObject.Rectangle.Bottom))
                {
                    if (((this.GetType().BaseType == typeof(Character)) || (this.GetType().BaseType.BaseType == typeof(Character))) && (passedGameObject.GetType() == typeof(Platform)))
                    {
                        this.hitObstacle = HitObstacle.FromTop;
                        this.JumpInProgress = false;

                        newX = this.Rectangle.X;
                        newY = passedGameObject.Rectangle.Y - this.Rectangle.Height + 1;

                        if (newX != this.Rectangle.X || newY != this.Rectangle.Y)
                        {
                            CreateRectangle(new Vector2(newX, newY));
                        }
                    }
                    else if 
                        (
                            ((this.GetType().BaseType == typeof(Character)) && (passedGameObject.GetType().BaseType.BaseType == typeof(Character))) ||
                            ((this.GetType().BaseType.BaseType == typeof(Character)) && (passedGameObject.GetType().BaseType == typeof(Character)))
                        )
                    {
                        this.hitNPC = HitNPC.FromTop;

                        if (this.GetType() == typeof(Player))
                        {
                            this.Lives--;
                        }                        
                    }
                    else  if ((this.GetType().BaseType == typeof(Environment)) && (passedGameObject.GetType().BaseType == typeof(Environment)))
                    {
                        hitObstacle = HitObstacle.None;
                    }
                    else
                    {
                        this.hitObstacle = HitObstacle.FromTop;
                    }
                }
                else if ((this.Rectangle.Top < passedGameObject.Rectangle.Bottom) && (this.Rectangle.Bottom != passedGameObject.Rectangle.Bottom))
                {
                    if ((this.GetType().BaseType == typeof(Character)) && (passedGameObject.GetType() == typeof(Platform)))
                    {
                        this.hitObstacle = HitObstacle.FromBottom;
                        this.JumpInProgress = false;


                        newX = this.Rectangle.X;
                        newY = passedGameObject.Rectangle.Y + passedGameObject.Rectangle.Height - 1;

                        if (newX != this.Rectangle.X || newY != this.Rectangle.Y)
                        { 
                            CreateRectangle(new Vector2(newX, newY));
                        }
                    }
                    else if
                       (
                           ((this.GetType().BaseType == typeof(Character)) && (passedGameObject.GetType().BaseType.BaseType == typeof(Character))) ||
                           ((this.GetType().BaseType.BaseType == typeof(Character)) && (passedGameObject.GetType().BaseType == typeof(Character)))
                       )
                    {
                        this.hitNPC = HitNPC.FromBottom;

                        if (this.GetType() == typeof(Player))
                        {
                            this.Lives--;
                        }
                    }
                    else if ((this.GetType().BaseType == typeof(Environment)) && (passedGameObject.GetType().BaseType == typeof(Environment)))
                    {
                        hitObstacle = HitObstacle.None;
                    }
                    else
                    {
                        this.hitObstacle = HitObstacle.FromBottom;
                    }
                }
                else if ((this.Rectangle.Left < passedGameObject.Rectangle.Right) && (this.Rectangle.Right != passedGameObject.Rectangle.Right))                  
                {
                    if ((this.GetType().BaseType == typeof(Character)) && (passedGameObject.GetType() == typeof(Platform)))
                    {
                        this.hitObstacle = HitObstacle.FromRight;
                        this.JumpInProgress = false;

                        newX = passedGameObject.Rectangle.X + passedGameObject.Rectangle.Width - 1;
                        newY = this.Rectangle.Y;

                        if (newX != this.Rectangle.X || newY != this.Rectangle.Y)
                        {
                            CreateRectangle(new Vector2(newX, newY));
                        }
                    }
                    else if
                       (
                           ((this.GetType().BaseType == typeof(Character)) && (passedGameObject.GetType().BaseType.BaseType == typeof(Character))) ||
                           ((this.GetType().BaseType.BaseType == typeof(Character)) && (passedGameObject.GetType().BaseType == typeof(Character)))
                       )
                    {
                        this.hitNPC = HitNPC.FromRight;

                        if (this.GetType() == typeof(Player))
                        {
                            this.Lives--;
                        }
                    }
                    else if ((this.GetType().BaseType == typeof(Environment)) && (passedGameObject.GetType().BaseType == typeof(Environment)))
                    {
                        hitObstacle = HitObstacle.None;
                    }
                    else
                    {
                        this.hitObstacle = HitObstacle.FromRight;
                    }
                }
                else if ((this.Rectangle.Right > passedGameObject.Rectangle.Left) && (this.Rectangle.Left != passedGameObject.Rectangle.Left))
                {
                    if ((this.GetType().BaseType == typeof(Character)) && (passedGameObject.GetType() == typeof(Platform)))
                    {
                        this.hitObstacle = HitObstacle.FromLeft;
                        this.JumpInProgress = false;

                        newX = passedGameObject.Rectangle.X - this.Rectangle.Width + 1;
                        newY = this.Rectangle.Y;

                        if (newX != this.Rectangle.X || newY != this.Rectangle.Y)
                        {
                            CreateRectangle(new Vector2(newX, newY));
                        }
                    }
                    else if
                       (
                           ((this.GetType().BaseType == typeof(Character)) && (passedGameObject.GetType().BaseType.BaseType == typeof(Character))) ||
                           ((this.GetType().BaseType.BaseType == typeof(Character)) && (passedGameObject.GetType().BaseType == typeof(Character)))
                       )
                    {
                        this.hitNPC = HitNPC.FromLeft;

                        if (this.GetType() == typeof(Player))
                        {
                            this.Lives--;
                        }
                    }
                    else if ((this.GetType().BaseType == typeof(Environment)) && (passedGameObject.GetType().BaseType == typeof(Environment)))
                    {
                        hitObstacle = HitObstacle.None;
                    }
                    else
                    {
                        this.hitObstacle = HitObstacle.FromLeft;
                    }
                }                
            }

            return returnValue;
        }

        /// <summary>
        /// Calculates the amount of force to apply for the object during each iteration of the game loop
        /// </summary>
        public virtual void CalculateGravity()
        {
            if (this.ApplyGravity)
            {
                switch (gravityDirection)
                {
                    case GravityDirection.Up:
                        if ((hitObstacle == HitObstacle.FromBottom) && (this.GetType().BaseType != typeof(Environment)))
                        {
                            this.GVelocity = 0;
                        }
                        else
                        {
                            if (this.GVelocity > -5)
                            {
                                this.GVelocity -= this.Acceleration;
                            }
                        }

                        break;
                    case GravityDirection.Down:
                        if ((hitObstacle == HitObstacle.FromTop) && (this.GetType().BaseType != typeof(Environment)))
                        {
                            this.GVelocity = 0;
                        }
                        else
                        {
                            if (this.GVelocity < 5)
                            {
                                this.GVelocity += this.Acceleration;
                            }
                        }

                        break;
                    case GravityDirection.Left:
                        if ((hitObstacle == HitObstacle.FromRight) && (this.GetType().BaseType != typeof(Environment)))
                        {
                            this.AVelocity = 0;
                        }
                        else
                        {
                            if (this.AVelocity > -5)
                            {
                                this.AVelocity -= this.Acceleration;
                            }
                        }

                        break;

                    case GravityDirection.Right:
                        if ((hitObstacle == HitObstacle.FromLeft) && (this.GetType().BaseType != typeof(Environment)))
                        {
                            this.AVelocity = 0;
                        }
                        else
                        {
                            if (this.AVelocity < 5)
                            {
                                this.AVelocity += this.Acceleration;
                            }
                        }

                        break;
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            
        }        
    }
}
