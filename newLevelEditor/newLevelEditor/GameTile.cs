﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newLevelEditor
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


    public class GameTile
    {
        private int X { get; set; }
        private int Y { get; set; }
        private const int WIDTH = 50;
        private const int HEIGHT = 50;

        public int Width
        {
            get { return WIDTH; }
        }

        public int Height
        {
            get { return HEIGHT; }
        }

        public GameTile(int x, int y)
        {
            this.X = x * WIDTH;
            this.Y = y * HEIGHT;
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
