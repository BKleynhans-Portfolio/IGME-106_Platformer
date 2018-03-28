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

        //public abstract void Draw(SpriteBatch spriteBatch);
        public abstract bool Intersects(GameObject passedGameObject);
        protected abstract override void Update(GameTime gameTime);
        
        private Texture2D objectTexture;                                                    // Texture and rectangle
        private Rectangle rectangle;
        private SpriteEffects spriteEffect;

        private float globalGlobalAcceleration;                                             // Acceleration to apply to characters during each iteration
        private float environmentAcceleration;                                              // Acceleration to apply to platforms
        private float gravitationalVelocity;                                                // Gravitational velocity for vertical movement
        private float movementVelocity;                                                     // Movement velocity for horizontal movement
        
        private const float defaultHorizonalVelocity = 5f;                                  // Default velocity to implement horizontally
        private const float defaultVerticalVelocity = 5f;                                   // Default velocity to implement vertically

        private float objectMass;                                                           // Mass of objects                

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

            this.Falling = true;

            this.ApplyGravity = false;

            this.ObjectMass = 50;
            
            this.GlobalAcceleration = 0.25f;
            this.EnvironmentAcceleration = 0.05f;
            this.spriteEffect = SpriteEffects.None;
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
        public GameObject(Texture2D spriteTexture, int x, int y, int width, int height,
                          bool addGravity, float appliedObjectMass)
        {
            this.ObjectTexture = spriteTexture;
            this.Rectangle = new Rectangle(x, y, width, height);

            this.ApplyGravity = addGravity;

            this.ObjectMass = appliedObjectMass;

            this.GlobalAcceleration = 0.25f;
            this.EnvironmentAcceleration = 0.05f;            
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

        public float EnvironmentAcceleration
        {
            get { return this.environmentAcceleration; }
            set { this.environmentAcceleration = value; }
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
                                (this.GravitationalVelocity > -5))
                            {
                                this.GravitationalVelocity -= this.EnvironmentAcceleration;
                            }
                            else if (this.GravitationalVelocity > -5)                            // If it is not hit, or it is hit and is an environment object, apply appropriate gravity
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
                                (this.GravitationalVelocity < 5))
                            {
                                this.GravitationalVelocity += this.EnvironmentAcceleration;       
                            }
                            else if (this.GravitationalVelocity < 5)                             // If it is not hit, or it is hit and is an environment object, apply appropriate gravity
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
                                (this.MovementVelocity > -5))
                            {
                                this.MovementVelocity -= this.EnvironmentAcceleration;       
                            }
                            else if (this.MovementVelocity > -5)                                 // If it is not hit, or it is hit and is an environment object, apply appropriate gravity
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
                                (this.MovementVelocity < 5))
                            {
                                this.MovementVelocity += this.EnvironmentAcceleration;                                              
                            } 
                            else if (this.MovementVelocity < 5)                                  // If it is not hit, or it is hit and is an environment object, apply appropriate gravity
                            {
                                this.MovementVelocity += this.GlobalAcceleration;
                            }
                        }

                        break;
                }
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
