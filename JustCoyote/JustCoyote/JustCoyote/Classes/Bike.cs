using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JustCoyote
{
    class Bike : Actor
    {
        private Vector2 direction;
        private Vector2 desiredDirection;
        public PlayerIndex PlayerIndex;

        private double timeRemaining;
        private double moveInterval;

        public double MoveInterval
        {
            get
            {
                return this.moveInterval;
            }

            set
            {
                this.moveInterval = value;
                if (this.timeRemaining > this.moveInterval)
                {
                    this.timeRemaining = this.moveInterval;
                }
            }
        }

        public Bike(PlayerIndex playerIndex, Vector2 position, Vector2 direction)
        {
            this.PlayerIndex = playerIndex;
            this.Position = position;
            this.direction = direction;
            this.desiredDirection = direction;

            this.moveInterval = JustCoyote.BikeMoveInterval;
            this.Origin = JustCoyote.BikeOrigion;
        }

        public override void Update(GameTime gameTime)
        {
            if (this.moveInterval < JustCoyote.BikeStopThreshold)
            {
                this.timeRemaining -= gameTime.ElapsedGameTime.TotalSeconds;
                if (this.timeRemaining <= 0)
                {
                    this.Move();
                    this.timeRemaining = this.moveInterval;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 drawPosition = this.Position * JustCoyote.GridBlockSize;
            float rotation = (float)Math.Atan2(this.direction.Y, this.direction.X); // + MathHelper.Pi / 2
            Color tailColor = JustCoyote.PlayerColors[(int)this.PlayerIndex];

            spriteBatch.Draw(JustCoyote.BikeTexture, drawPosition, null, Color.White, rotation, this.Origin, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(JustCoyote.TailTexture, drawPosition, null, tailColor, rotation, this.Origin, 1f, SpriteEffects.None, 0f);
        }

        private void Move()
        {
            this.Position += this.direction;
            this.direction = this.desiredDirection;
        }

        public void ChangeDirection(Vector2 desiredDirection)
        {
            if (desiredDirection != this.direction * -1)
            {
                this.desiredDirection = desiredDirection;
            }
        }
    }
}
