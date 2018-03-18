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
        //public abstract void Move();                          // Not all game objects can move and die
        //public abstract void Die();
        public abstract void Draw();

        private Texture2D texture;
        private Rectangle spriteBox;
        private Color objectColor;

        private int xCoord;
        private int yCoord;

        private bool hasGravity;
        private bool isMoving;
        
        public GameObject(Texture2D texture2D, int x, int y, int width, int height)
        {
            Texture = texture2D;
                        
            spriteBox = new Rectangle(x, y, width, height);
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

                CreateSpriteBox();
            }
        }
        
        public int YCoord
        {
            get { return this.yCoord; }
            set
            {
                this.yCoord = value;

                CreateSpriteBox();
            }
        }

        public Color ObjectColor
        {
            get { return this.objectColor; }
            set { this.objectColor = value; }
        }

        public bool HasGravity
        {
            get { return this.hasGravity; }
            set { this.hasGravity = value; }
        }

        public bool IsMoving
        {
            get { return this.isMoving; }
            set { this.isMoving = value; }
        }

        public void CreateSpriteBox()
        {
            this.spriteBox = new Rectangle(
                                    XCoord,
                                    YCoord,
                                    this.spriteBox.Width,
                                    this.spriteBox.Height
                                );
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                this.spriteBox,
                ObjectColor
            );
        }

        public override string ToString()
        {
            string returnString;
            
            returnString = (
                "Texture        : " + this.Texture +
                "Rectangle      : " + this.spriteBox +
                "ObjectColor    : " + this.ObjectColor +
                "X Coordinate   : " + this.XCoord +
                "Y Coordinate   : " + this.YCoord +
                "Has Gravity    : " + this.HasGravity +
                "Is Static      : " + this.IsMoving);

            return returnString;
        }        
    }
}
