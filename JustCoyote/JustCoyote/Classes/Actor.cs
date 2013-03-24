using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JustCoyote
{
    abstract class Actor
    {
        public static List<Actor> Actors;

        public Vector2 Position;
        public Vector2 Origin;

        static Actor()
        {
            Actors = new List<Actor>();
        }

        public Actor()
        {
            Actors.Add(this);
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch, Texture2D bikeTexture) { }
    }
}
