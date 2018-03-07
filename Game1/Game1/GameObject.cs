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
/// Authors             : Cullen Sullivan
///                     : Benjamin Kleynhans
/// Modified By         : Benjamin Kleynhans
/// Date                : March 7, 2018
/// Filename            : GameObjects.cs
/// </summary>

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
