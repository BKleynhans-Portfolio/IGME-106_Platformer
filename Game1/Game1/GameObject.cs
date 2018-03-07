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
/// Created By          : Cullen Sullivan
/// Creation Date       : March 7, 2018
/// Authors             : Benjamin Kleynhans
///                       
///                       
///                       
/// Last Modified By    : Benjamin Kleynhans
/// Last Modified Date  : March 7, 2018
/// Filename            : GameObjects.cs
/// </summary>

namespace Game1
{
    abstract class GameObject
    {
        public abstract void Move();
        public abstract void Die();
        public abstract void Draw();

        private Texture2D texture;
        private Rectangle position;

        private int xCoordinate;
        private int yCoordinate;
        private int width;
        private int height;
        
        public GameObject(Texture2D texture2D, int xCoordinate, int yCoordinate, int width, int height)
        {
            Texture = texture2D;
            XCoord = xCoordinate;
            YCoord = yCoordinate;
            Width = width;
            Height = height;
        }

        public Texture2D Texture
        {
            get { return this.texture; }
            private set { this.texture = value; }
        }
        
        public Rectangle Position
        {
            get { return this.position; }
            private set
            {
                this.position = new Rectangle(XCoord, YCoord, Width, Height);
            }
        }

        public int XCoord
        {
            get { return this.xCoordinate; }
            set { this.xCoordinate = value; }
        }
        
        public int YCoord
        {
            get { return this.yCoordinate; }
            set { this.xCoordinate = value; }
        }

        public int Width
        {
            get { return this.width; }
            set { this.width = value; }
        }

        public int Height
        {
            get { return this.height; }
            set { this.height = value; }
        }
    }
}
