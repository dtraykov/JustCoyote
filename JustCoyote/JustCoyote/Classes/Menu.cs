using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JustCoyote
{
    public class Menu : IDrawable
    {
        KeyboardState prevKeyState;

        List<string> buttonList = new List<string>();
        int selected = 0;

        public Menu()
        {
            buttonList.Add("Single Player");
            buttonList.Add("Multi Player");
            buttonList.Add("Exit");

        }

        public int Update(GameTime gameTime)
        {
            KeyboardState keyState;
            keyState = Keyboard.GetState(PlayerIndex.One);

            if (keyState.IsKeyDown(Keys.Up) && !prevKeyState.IsKeyDown(Keys.Up))
            {
                if (selected > 0)
                {
                    selected--;
                }
            }

            if (keyState.IsKeyDown(Keys.Down) && !prevKeyState.IsKeyDown(Keys.Down))
            {
                if (selected < buttonList.Count - 1)
                {
                    selected++;
                }
            }

            if (keyState.IsKeyDown(Keys.Enter))
            {
                return selected;
            }

            prevKeyState = keyState;
            return -1;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            Color color;
            int linePadding = 3;

            for (int i = 0; i < buttonList.Count; i++)
            {
                color = (i == selected) ? Color.Yellow : Color.Black;

                spriteBatch.DrawString(spriteFont, buttonList[i],
                    new Vector2((JustCoyote.ScreenWidth / 2) - (spriteFont.MeasureString(buttonList[i]).X / 2),
                    (JustCoyote.ScreenHeight / 2) - (spriteFont.LineSpacing * buttonList.Count / 2)
                    + ((spriteFont.LineSpacing + linePadding) * i)), color);
            }
        }
    }
}
