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
/// Last Modified Date  : March 9, 2018
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
        private Rectangle rectangle;
        private Color objectColor;

        private int xCoord;
        private int yCoord;
        
        public GameObject(Texture2D texture2D, int x, int y, int width, int height)
        {
            Texture = texture2D;

            rectangle = new Rectangle(x, y, width, height);            
        }

        public Texture2D Texture
        {
            get { return this.texture; }
            private set { this.texture = value; }
        }

        public int XCoord
        {
            get { return this.xCoord; }
            set
            {
                this.xCoord = value;

                CreateRectangle();
            }
        }
        
        public int YCoord
        {
            get { return this.yCoord; }
            set
            {
                this.yCoord = value;

                CreateRectangle();
            }
        }

        public Color ObjectColor
        {
            get { return this.objectColor; }
            set { this.objectColor = value; }
        }

        public void CreateRectangle()
        {
            this.rectangle = new Rectangle(XCoord, YCoord, this.rectangle.Width, this.rectangle.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                rectangle,
                ObjectColor
            );
        }
    }
}
