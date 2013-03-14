using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JustCoyote
{
    struct WallSegment
    {
        public bool Filled;
        public byte TextureIndex;
        public PlayerIndex PlayerIndex;
    }
}
