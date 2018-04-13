using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newLevelEditor
{
    

    public enum MovementAppliedTo                                                           // Movement parameters used for all object movement
    {
        None,
        Left,
        Right,
        Up,
        Down        
    }


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

        public MovementAppliedTo MovementAppliedTo { get; set; }
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
        public GameTile(int x, int y, string name, MovementAppliedTo movementAppliedTo, GravityOnProximityFrom gravityOnProximityFrom, ObjectMovement objectMovement)
        {
            this.X = x * WIDTH;
            this.Y = y * HEIGHT;
            this.Name = name;
            this.MovementAppliedTo = movementAppliedTo;
            this.GravityOnProximityFrom = gravityOnProximityFrom;
            this.ObjectMovement = objectMovement;

            DefineTypeOfObject();
        }

        private void DefineTypeOfObject()
        {
            if (Name.Equals("Background"))
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
            myString.Append(this.MovementAppliedTo);
            myString.Append("|");
            myString.Append(this.GravityOnProximityFrom);
            myString.Append("|");
            myString.Append(this.ObjectMovement);


            return myString.ToString();
        }
    }
}
