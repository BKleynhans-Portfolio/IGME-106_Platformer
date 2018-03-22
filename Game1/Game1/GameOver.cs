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
/// Class Description   : GameOver class
/// Author              : Benjamin Kleynhans
/// Modified By         : Benjamin Kleynhans
/// Date                : March 13, 2018
/// Filename            : GameOver.cs
/// </summary>

namespace Game1
{
    class GameOver : Menu
    {
        public GameOver(Texture2D texture2D, int x, int y, int width, int height) : base(texture2D, x, y, width, height)
        {

        }

        protected override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(                                                   // Draw the sprite from the spriteBatch
                base.ObjectTexture,
                base.Rectangle,
                Color.White
            );
        }
    }
}
