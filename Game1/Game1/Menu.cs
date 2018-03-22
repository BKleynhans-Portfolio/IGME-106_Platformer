using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Game1 - Platformer for Learning
/// Class Description   : Menu class
/// Author              : Benjamin Kleynhans
/// Modified By         : Benjamin Kleynhans
/// Date                : March 13, 2018
/// Filename            : Menu.cs
/// </summary>

namespace Game1
{
    abstract class Menu : Screen
    {
        public abstract override void Draw(SpriteBatch spriteBatch);
        protected abstract override void Update(GameTime gameTime);

        public Menu(Texture2D texture2D, int x, int y, int width, int height) : base(texture2D, x, y, width, height)
        {

        }

        public override bool Intersects(GameObject passedGameObject)
        {
            return false;
        }
    }
}
