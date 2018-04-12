using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newLevelEditor
{
    

    public enum MovementAppliedTo                                                           // Movement parameters used for all object movement
    {
        Left,
        Right,
        Up,
        Down,
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


    public class GameTile
    {
        public MovementAppliedTo MovementAppliedTo { get; set; }
        public GravityOnProximityFrom GravityOnProximityFrom { get; set; }
        public ObjectMovement ObjectMovement { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        private const int WIDTH = 50;
        private const int HEIGHT = 50;
        string Name { get; set; }
        

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
            Name = name;

        }

        public override string ToString()
        {
            StringBuilder myString = new StringBuilder();

            myString.Append(this.X);
            myString.Append("|");
            myString.Append(this.Y);
            myString.Append("|");
            myString.Append(WIDTH);
            myString.Append("|");
            myString.Append(HEIGHT);

            return myString.ToString();
        }
    }
}
