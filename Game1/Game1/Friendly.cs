﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Game1 - Platformer for Learning
/// Class Description   : Friendly class
/// Author              : Benjamin Kleynhans
/// Modified By         : Benjamin Kleynhans
/// Date                : March 13, 2018
/// Filename            : Friendly.cs
/// </summary>

namespace Game1
{
    class Friendly : NPC
    {

        public Friendly(Texture2D spriteTexture, int x, int y, int width, int height) : base(spriteTexture, x, y, width, height)
        {
            base.IsAlive = true;
        }

        public Friendly(Texture2D spriteTexture, int x, int y, int width, int height,
                          bool addGravity, float appliedMoveForce, float appliedVerticalMovementForce,
                          float appliedGravitationalAcceleration, float appliedObjectMass) :
                base(spriteTexture, x, y, width, height, addGravity, appliedMoveForce, appliedVerticalMovementForce,
                    appliedGravitationalAcceleration, appliedObjectMass)
        {
            base.IsAlive = true;
        }

        protected override void Die()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if ((this.Rectangle.Y + this.Rectangle.Height) > screenHeight)
            {
                this.IsAlive = false;
            }
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