using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JustCoyote
{
    public struct Direction
    {
        public static Vector2 Up
        {
            get
            {
                return new Vector2(0f, -1f);
            }
        }

        public static Vector2 Down
        {
            get
            {
                return new Vector2(0f, 1f);
            }
        }

        public static Vector2 Left
        {
            get
            {
                return new Vector2(-1f, 0f);
            }
        }

        public static Vector2 Right
        {
            get
            {
                return new Vector2(1f, 0f);
            }
        }
    }
}
