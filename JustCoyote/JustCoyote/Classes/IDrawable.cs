using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JustCoyote
{
    public interface IDrawable
    {
        void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont);
    }
}
