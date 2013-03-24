using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JustCoyote
{
    class Player : Actor, IMovable
    {
        private Vector2 currentDirection;
        private Vector2 desiredDirection;
        private Vector2 nextPosition;
        public PlayerIndex PlayerIndex;

        private double timeRemaining;
        private double moveInterval;

        public Vector2 CurrentDirection
        {
            get { return this.currentDirection; }
        }

        public Vector2 NextPosition
        {
            get { return this.nextPosition; }
            private set { this.nextPosition = value; }
        }

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

        public Player(PlayerIndex playerIndex, Vector2 position, Vector2 direction)
        {
            this.PlayerIndex = playerIndex;
            this.Position = position;
            this.currentDirection = direction;
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

        public override void Draw(SpriteBatch spriteBatch, Texture2D playerTexture)
        {
            Vector2 drawPosition;
            Vector2 startPosition = this.Position * JustCoyote.GridBlockSize;
            Vector2 endPosition = (this.Position + this.currentDirection) * JustCoyote.GridBlockSize;

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

            float rotation = (float)Math.Atan2(this.currentDirection.Y, this.currentDirection.X); // + MathHelper.Pi / 2
            Color tailColor = Color.White;

            SpriteEffects flip = new SpriteEffects();
            flip = SpriteEffects.None;

            if (Math.Round(rotation, MidpointRounding.AwayFromZero) == 3)
            {
                flip = SpriteEffects.FlipVertically;
            }

            spriteBatch.Draw(JustCoyote.TailTexture, drawPosition, null, tailColor, rotation, this.Origin, 1f, flip, 0f);
            spriteBatch.Draw(playerTexture, drawPosition, null, Color.White, rotation, this.Origin, 1f, flip, 0f);

        }

        public void Move()
        {
            this.Position += this.currentDirection;
            this.NextPosition = this.Position + this.currentDirection;
            if (this.NextPosition.X < 0 || this.NextPosition.Y < 0 || this.NextPosition.X > JustCoyote.GridWidth ||
                this.NextPosition.Y > JustCoyote.GridHeight || Wall.Segments[(int)this.Position.X, (int)this.Position.Y].Filled)
            {
                this.Position -= this.currentDirection;
                this.currentDirection = this.desiredDirection;
                this.Position += this.currentDirection;
            }

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

                if (this.currentDirection != this.desiredDirection)
                {
                    if (this.currentDirection == Direction.Left && this.desiredDirection == Direction.Down ||
                        this.currentDirection == Direction.Up && this.desiredDirection == Direction.Right)
                    {
                        Wall.Segments[x, y].TextureIndex = 2;
                    }

                    if (this.currentDirection == Direction.Right && this.desiredDirection == Direction.Down ||
                        this.currentDirection == Direction.Up && this.desiredDirection == Direction.Left)
                    {
                        Wall.Segments[x, y].TextureIndex = 3;
                    }

                    if (this.currentDirection == Direction.Right && this.desiredDirection == Direction.Up ||
                        this.currentDirection == Direction.Down && this.desiredDirection == Direction.Left)
                    {
                        Wall.Segments[x, y].TextureIndex = 4;
                    }

                    if (this.currentDirection == Direction.Left && this.desiredDirection == Direction.Up ||
                        this.currentDirection == Direction.Down && this.desiredDirection == Direction.Right)
                    {
                        Wall.Segments[x, y].TextureIndex = 5;
                    }

                    this.currentDirection = this.desiredDirection;
                }
                else
                {
                    if (this.currentDirection.X != 0f)
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
            if (desiredDirection != this.currentDirection * -1)
            {
                this.desiredDirection = desiredDirection;
            }
        }
    }
}
