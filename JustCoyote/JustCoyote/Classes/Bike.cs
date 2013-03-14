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

        public override void Draw(SpriteBatch spriteBatch,Texture2D bikeTexture)
        {
            Vector2 drawPosition;
            Vector2 startPosition = this.Position * JustCoyote.GridBlockSize;
            Vector2 endPosition = (this.Position + this.direction) * JustCoyote.GridBlockSize;

            if (this.timeRemaining > 0 && this.moveInterval > 0)
            {
                float percent = 1f - (float)(this.timeRemaining / this.moveInterval);
                drawPosition = Vector2.Lerp(startPosition, endPosition, percent);
            }
            else
            {
                drawPosition = startPosition;
            }


            drawPosition += new Vector2(JustCoyote.GridBlockSize / 2);

            float rotation = (float)Math.Atan2(this.direction.Y, this.direction.X); // + MathHelper.Pi / 2
            Color tailColor = JustCoyote.PlayerColors[(int)this.PlayerIndex];

            spriteBatch.Draw(bikeTexture, drawPosition, null, Color.White, rotation, this.Origin, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(JustCoyote.TailTexture, drawPosition, null, tailColor, rotation, this.Origin, 1f, SpriteEffects.None, 0f);
        }

        private void Move()
        {
            this.Position += this.direction;

            int x = (int)this.Position.X;
            int y = (int)this.Position.Y;

            if (x > -1 && y > -1 && x < JustCoyote.GridWidth && y < JustCoyote.GridHeight)
            {
                if (Wall.Segments[x, y].Filled)
                {
                    JustCoyote.CollideWall();
                }

                Wall.Segments[x, y].Filled = true;
                Wall.Segments[x, y].PlayerIndex = this.PlayerIndex;

                if (this.direction != this.desiredDirection)
                {
                    if (this.direction == Direction.Left && this.desiredDirection == Direction.Down ||
                        this.direction == Direction.Up && this.desiredDirection == Direction.Right)
                    {
                        Wall.Segments[x, y].TextureIndex = 2;
                    }

                    if (this.direction == Direction.Right && this.desiredDirection == Direction.Down ||
                        this.direction == Direction.Up && this.desiredDirection == Direction.Left)
                    {
                        Wall.Segments[x, y].TextureIndex = 3;
                    }

                    if (this.direction == Direction.Right && this.desiredDirection == Direction.Up ||
                        this.direction == Direction.Down && this.desiredDirection == Direction.Left)
                    {
                        Wall.Segments[x, y].TextureIndex = 4;
                    }

                    if (this.direction == Direction.Left && this.desiredDirection == Direction.Up ||
                        this.direction == Direction.Down && this.desiredDirection == Direction.Right)
                    {
                        Wall.Segments[x, y].TextureIndex = 5;
                    }

                    this.direction = this.desiredDirection;
                }
                else
                {
                    if (this.direction.X != 0f)
                    {
                        Wall.Segments[x, y].TextureIndex = 0;
                    }
                    else
                    {
                        Wall.Segments[x, y].TextureIndex = 1;
                    }
                }
            }
            else
            {
                JustCoyote.CollideWall();
            }
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
