using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newLevelEditor
{
    public enum GravityOnProximityFrom                                                      // Parameters used if object needs to start moving on proximity
    {
        None,
        Left,
        Right,
        Top,
        Bottom,
        Center        
    }

    public enum ObjectMovement                                                              // Define if object needs to move
    {
        None,
        OneDirection,
        ToAndFroUpFirst,
        ToAndFroDownFirst,
        ToAndFroLeftFirst,
        ToAndFroRightFirst
    }

    public enum GravityAppliedTo                                                            // Gravity can be applied in these directions (autoconfigured)
    {        
        Down,
        Up,
        Left,
        Right
    }

    public enum ObjectType                                                                  // Type of object, required for object creation
    {
        Background,
        Player,
        Platform,
        Enemy,
        Collectible,
        Goal
    }

    public class GameTile
    {
        private ObjectType ObjectType { get; set; }

        private const int WIDTH = 50;
        private const int HEIGHT = 50;

        private string Name { get; set; }
        private string ObjectMoveDistance { get; set; }

        public GravityAppliedTo GravityAppliedTo { get; set; }
        public GravityOnProximityFrom GravityOnProximityFrom { get; set; }
        public ObjectMovement ObjectMovement { get; set; }
        
        public int X { get; set; }
        public int Y { get; set; }
        
        public int Width
        {
            get { return WIDTH; }
        }

        public int Height
        {
            get { return HEIGHT; }
        }

        //constructor
        public GameTile(int x, int y, string name, GravityOnProximityFrom gravityOnProximityFrom, ObjectMovement objectMovement,
                        GravityAppliedTo gravityDirection, string objectMoveDistance)
        {
            this.X = x * WIDTH;
            this.Y = y * HEIGHT;
            this.Name = name;            
            this.GravityOnProximityFrom = gravityOnProximityFrom;
            this.ObjectMovement = objectMovement;
            this.ObjectMoveDistance = objectMoveDistance;

            DefineTypeOfObject();
            DefineGravityDirection();
        }

        /// <summary>
        /// Assign the appropriate object type to the object.  This is required for object
        /// creation during object instantiation in the game
        /// </summary>
        private void DefineTypeOfObject()
        {
            if (this.Name.Equals("Background"))
            {
                this.ObjectType = ObjectType.Background;
            }
            else if (Name.Equals("Player"))
            {
                this.ObjectType = ObjectType.Player;
            }
            else if (Name.Equals("Enemy"))
            {
                this.ObjectType = ObjectType.Enemy;
            }
            else if (Name.Equals("Grass") ||
                Name.Equals("Water") ||
                Name.Equals("Stone") ||
                Name.Equals("Wood"))
            {
                this.ObjectType = ObjectType.Platform;
            }
            else if (Name.Equals("Worm") ||
                Name.Equals("Seed"))
            {
                this.ObjectType = ObjectType.Collectible;
            }
            else if (Name.Equals("BirdHouse"))
            {
                this.ObjectType = ObjectType.Goal;
            }
        }

        private void DefineGravityDirection()
        {
            if (this.ObjectMovement == ObjectMovement.ToAndFroDownFirst)
            {
                this.GravityAppliedTo = GravityAppliedTo.Down;
            }
            if (this.ObjectMovement == ObjectMovement.ToAndFroLeftFirst)
            {
                this.GravityAppliedTo = GravityAppliedTo.Left;
            }
            if (this.ObjectMovement == ObjectMovement.ToAndFroRightFirst)
            {
                this.GravityAppliedTo = GravityAppliedTo.Right;
            }
            if (this.ObjectMovement == ObjectMovement.ToAndFroUpFirst)
            {
                this.GravityAppliedTo = GravityAppliedTo.Up;
            }
        }

        /// <summary>
        /// ToString method used to print entire object for writing to text file
        /// </summary>
        /// <returns>Formatted string containing all properties</returns>
        public override string ToString()
        {
            StringBuilder myString = new StringBuilder();

            myString.Append(this.ObjectType);
            myString.Append("|");
            myString.Append(this.Name);
            myString.Append("|");
            myString.Append(this.X);
            myString.Append("|");
            myString.Append(this.Y);
            myString.Append("|");
            myString.Append(WIDTH);
            myString.Append("|");
            myString.Append(HEIGHT);
            myString.Append("|");            
            myString.Append(this.GravityOnProximityFrom);
            myString.Append("|");
            myString.Append(this.ObjectMovement);
            myString.Append("|");
            myString.Append(this.GravityAppliedTo);
            myString.Append("|");
            myString.Append(this.ObjectMoveDistance);

            return myString.ToString();
        }
    }
}
