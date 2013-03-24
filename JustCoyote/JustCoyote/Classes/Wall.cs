using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JustCoyote
{
    public static class Wall
    {
        public static WallSegment[,] Segments;

        public static void Reset()
        {
            Segments = new WallSegment[JustCoyote.GridWidth, JustCoyote.GridHeight];
        }

        public static Rectangle GetPointBounds(int x, int y)
        {
            return new Rectangle(
                                 x * JustCoyote.GridBlockSize,
                                 y * JustCoyote.GridBlockSize,
                                 JustCoyote.GridBlockSize,
                                 JustCoyote.GridBlockSize);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < JustCoyote.GridWidth; x++)
            {
                for (int y = 0; y < JustCoyote.GridHeight; y++)
                {
                    if (Segments[x, y].Filled)
                    {
                        Texture2D wallTexture = JustCoyote.WallTextures[Segments[x, y].TextureIndex];
                        Color wallColor = JustCoyote.PlayerColors[(int)Segments[x, y].PlayerIndex];

                        spriteBatch.Draw(wallTexture, GetPointBounds(x, y), wallColor);
                    }
                }
            }
        }
    }
}
