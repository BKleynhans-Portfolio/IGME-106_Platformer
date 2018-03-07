using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Game1
{
    abstract class GameObject
    {
        Texture2D Texture;
        Rectangle Position;
        abstract public void Move();
        public abstract void Die();
        public abstract void Draw();
    }
}
