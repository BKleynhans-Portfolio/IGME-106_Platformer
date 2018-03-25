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
        protected abstract override void Update(GameTime gameTime);

        private Texture2D objectTexture;                                                    // Texture and rectangle
        private Rectangle rectangle;

        private float acceleration;                                                         // Acceleration to apply to element during each iteration
        private float gravitationalVelocity;                                                // Gravitational velocity for vertical movement
        private float movementVelocity;                                                     // Movement velocity for horizontal movement
        
        private const float defaultHorizonalVelocity = 5f;                                  // Default velocity to implement horizontally
        private const float defaultVerticalVelocity = 5f;                                   // Default velocity to implement vertically

        private float objectMass;                                                           // Mass of objects                

        private bool applyGravity;                                                          // Should the object have gravity

        private bool falling;                                                               // Is the object falling?
        private bool jumpInProgress;                                                        // Is the object in a jump process?

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

            this.Acceleration = 0.25f;
            this.Lives = 3;
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
        /// Number of lives the player has left
        /// </summary>
        public int Lives
        {
            get { return this.lives; }
            set { this.lives = value; }
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
        /// Acceleration to add to velocity in any of the four cardinal directions
        /// </summary>
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
        /// Determines whether the object that is passed in, intersects the current object
        /// </summary>
        /// <param name="passedGameObject">object to be tested for intersection against this object</param>
        /// <returns>true if it intersects and false if it does not</returns>
        public virtual bool Intersects(GameObject passedGameObject)
        {
            bool returnValue = false;

            // Does this object's rectangle intersect with the passed in object's rectangle
            if (this.Rectangle.Intersects(passedGameObject.Rectangle))
            {
                returnValue = true;

                // New X and Y values in case there is an intersection
                float newX;
                float newY;

                // If the lower border of this object has a larger Y coordinate than the upper border
                //  of the passed in object, and the two are not in line (lower borders are not the same)
                if ((this.Rectangle.Bottom > passedGameObject.Rectangle.Top) && 
                    (this.Rectangle.Bottom != passedGameObject.Rectangle.Bottom))
                {
                    // If this object is of type Character and the passed in object is of type Platform
                        // *** Each continuation of the BaseType keyword goes up one additional level in the derived classes
                    if (((this.GetType().BaseType == typeof(Character)) || (this.GetType().BaseType.BaseType == typeof(Character))) && 
                        (passedGameObject.GetType() == typeof(Platform)))
                    {
                        this.hitObstacle = HitObstacle.FromTop;
                        this.JumpInProgress = false;

                        // Define new X and Y coordinates and create a new rectangle
                        newX = this.Rectangle.X;
                        newY = passedGameObject.Rectangle.Y - this.Rectangle.Height + 1;

                        if (newX != this.Rectangle.X || newY != this.Rectangle.Y)
                        {
                            CreateRectangle(new Vector2(newX, newY));
                        }
                    }
                    // If both this object as well as the passed in object are of type Character
                        // *** Each continuation of the BaseType keyword goes up one additional level in the derived classes
                    else if ((      
                                (this.GetType().BaseType == typeof(Character)) &&
                                (passedGameObject.GetType().BaseType.BaseType == typeof(Character))
                            ) || (
                                (this.GetType().BaseType.BaseType == typeof(Character)) &&
                                (passedGameObject.GetType().BaseType == typeof(Character))
                            ))
                    {
                        // Indicate that this object was hit by an NPC coming from the top
                        this.hitNPC = HitNPC.FromTop;

                        // If the player was hit by and NPC and it is not a friendly NPC, subtract one life.
                        if ((this.GetType() == typeof(Player)) && (passedGameObject.GetType() != typeof(Friendly)))
                        {
                            this.Lives--;
                        }                        
                    }
                    // If both this object as well as the passed in object are of type Environment
                    else if ((this.GetType().BaseType == typeof(Environment)) &&
                            (passedGameObject.GetType().BaseType == typeof(Environment)))
                    {
                        hitObstacle = HitObstacle.None;
                    }
                    else
                    {
                        this.hitObstacle = HitObstacle.FromTop;
                    }
                }
                // If the upper border of this object has a larger Y coordinate than the lower border
                //  of the passed in object, and the two are not in line (lower borders are not the same)
                else if ((this.Rectangle.Top < passedGameObject.Rectangle.Bottom) &&
                        (this.Rectangle.Bottom != passedGameObject.Rectangle.Bottom))
                {
                    // If this object is of type Character and the passed in object is of type Platform
                    // *** Each continuation of the BaseType keyword goes up one additional level in the derived classes
                    if (((this.GetType().BaseType == typeof(Character)) || (this.GetType().BaseType.BaseType == typeof(Character))) &&
                        (passedGameObject.GetType() == typeof(Platform)))
                    {
                        this.hitObstacle = HitObstacle.FromBottom;
                        this.JumpInProgress = false;

                        // Define new X and Y coordinates and create a new rectangle
                        newX = this.Rectangle.X;
                        newY = passedGameObject.Rectangle.Y + passedGameObject.Rectangle.Height - 1;

                        if (newX != this.Rectangle.X || newY != this.Rectangle.Y)
                        { 
                            CreateRectangle(new Vector2(newX, newY));
                        }
                    }
                    // If this object is of type Character and the passed in object is of type Platform
                    // *** Each continuation of the BaseType keyword goes up one additional level in the derived classes
                    else if
                       (
                           ((this.GetType().BaseType == typeof(Character)) && (passedGameObject.GetType().BaseType.BaseType == typeof(Character))) ||
                           ((this.GetType().BaseType.BaseType == typeof(Character)) && (passedGameObject.GetType().BaseType == typeof(Character)))
                       )
                    {
                        // Indicate that this object was hit by an NPC coming from the bottom
                        this.hitNPC = HitNPC.FromBottom;

                        // If the player was hit by and NPC and it is not a friendly NPC, subtract one life.
                        if ((this.GetType() == typeof(Player)) && (passedGameObject.GetType() != typeof(Friendly)))
                        {
                            this.Lives--;
                        }
                    }
                    // If both this object as well as the passed in object are of type Environment
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
                    // If this object is of type Character and the passed in object is of type Platform
                    // *** Each continuation of the BaseType keyword goes up one additional level in the derived classes
                    if (((this.GetType().BaseType == typeof(Character)) || (this.GetType().BaseType.BaseType == typeof(Character))) &&
                        (passedGameObject.GetType() == typeof(Platform)))
                    {
                        this.hitObstacle = HitObstacle.FromRight;
                        this.JumpInProgress = false;

                        newX = passedGameObject.Rectangle.X + passedGameObject.Rectangle.Width - 1;
                        newY = this.Rectangle.Y;

                        // Define new X and Y coordinates and create a new rectangle
                        if (newX != this.Rectangle.X || newY != this.Rectangle.Y)
                        {
                            CreateRectangle(new Vector2(newX, newY));
                        }
                    }
                    // If this object is of type Character and the passed in object is of type Platform
                    // *** Each continuation of the BaseType keyword goes up one additional level in the derived classes
                    else if
                       (
                           ((this.GetType().BaseType == typeof(Character)) && (passedGameObject.GetType().BaseType.BaseType == typeof(Character))) ||
                           ((this.GetType().BaseType.BaseType == typeof(Character)) && (passedGameObject.GetType().BaseType == typeof(Character)))
                       )
                    {
                        // Indicate that this object was hit by an NPC coming from the right
                        this.hitNPC = HitNPC.FromRight;

                        // If the player was hit by and NPC and it is not a friendly NPC, subtract one life.
                        if ((this.GetType() == typeof(Player)) && (passedGameObject.GetType() != typeof(Friendly)))
                        {
                            this.Lives--;
                        }
                    }
                    // If both this object as well as the passed in object are of type Environment
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
                    // If this object is of type Character and the passed in object is of type Platform
                    // *** Each continuation of the BaseType keyword goes up one additional level in the derived classes
                    if (((this.GetType().BaseType == typeof(Character)) || (this.GetType().BaseType.BaseType == typeof(Character))) &&
                        (passedGameObject.GetType() == typeof(Platform)))
                    {
                        this.hitObstacle = HitObstacle.FromLeft;
                        this.JumpInProgress = false;

                        // Define new X and Y coordinates and create a new rectangle
                        newX = passedGameObject.Rectangle.X - this.Rectangle.Width + 1;
                        newY = this.Rectangle.Y;

                        if (newX != this.Rectangle.X || newY != this.Rectangle.Y)
                        {
                            CreateRectangle(new Vector2(newX, newY));
                        }
                    }
                    // If this object is of type Character and the passed in object is of type Platform
                    // *** Each continuation of the BaseType keyword goes up one additional level in the derived classes
                    else if
                       (
                           ((this.GetType().BaseType == typeof(Character)) && (passedGameObject.GetType().BaseType.BaseType == typeof(Character))) ||
                           ((this.GetType().BaseType.BaseType == typeof(Character)) && (passedGameObject.GetType().BaseType == typeof(Character)))
                       )
                    {
                        // Indicate that this object was hit by an NPC coming from the left
                        this.hitNPC = HitNPC.FromLeft;

                        // If the player was hit by and NPC and it is not a friendly NPC, subtract one life.
                        if ((this.GetType() == typeof(Player)) && (passedGameObject.GetType() != typeof(Friendly)))
                        {
                            this.Lives--;
                        }
                    }
                    // If both this object as well as the passed in object are of type Environment
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
            // If the object requires gravity to be implemented
            if (this.ApplyGravity)
            {
                // Based on which direction the gravity is implemented in
                switch (gravityDirection)
                {
                    case GravityDirection.Up:
                        // If this object is hit from the bottom and it is not an environment object
                        if ((hitObstacle == HitObstacle.FromBottom) && (this.GetType().BaseType != typeof(Environment)))
                        {
                            // Set gravitational velocity to 0
                            this.GravitationalVelocity = 0;
                        }
                        else
                        {
                            // If it is not hit, or it is hit and is an environment object, apply appropriate gravity
                            if (this.GravitationalVelocity > -5)
                            {
                                this.GravitationalVelocity -= this.Acceleration;
                            }
                        }

                        break;
                    case GravityDirection.Down:
                        // If this object is hit from the top and it is not an environment object
                        if ((hitObstacle == HitObstacle.FromTop) && (this.GetType().BaseType != typeof(Environment)))
                        {
                            // Set gravitational velocity to 0
                            this.GravitationalVelocity = 0;
                        }
                        else
                        {
                            // If it is not hit, or it is hit and is an environment object, apply appropriate gravity
                            if (this.GravitationalVelocity < 5)
                            {
                                this.GravitationalVelocity += this.Acceleration;
                            }
                        }

                        break;
                    case GravityDirection.Left:
                        // If this object is hit from the right and it is not an environment object
                        if ((hitObstacle == HitObstacle.FromRight) && (this.GetType().BaseType != typeof(Environment)))
                        {
                            // Set gravitational velocity to 0
                            this.MovementVelocity = 0;
                        }
                        else
                        {
                            // If it is not hit, or it is hit and is an environment object, apply appropriate gravity
                            if (this.MovementVelocity > -5)
                            {
                                this.MovementVelocity -= this.Acceleration;
                            }
                        }

                        break;

                    case GravityDirection.Right:
                        // If this object is hit from the left and it is not an environment object
                        if ((hitObstacle == HitObstacle.FromLeft) && (this.GetType().BaseType != typeof(Environment)))
                        {
                            // Set gravitational velocity to 0
                            this.MovementVelocity = 0;
                        }
                        else
                        {
                            // If it is not hit, or it is hit and is an environment object, apply appropriate gravity
                            if (this.MovementVelocity < 5)
                            {
                                this.MovementVelocity += this.Acceleration;
                            }
                        }

                        break;
                }
            }
        }
    }
}
