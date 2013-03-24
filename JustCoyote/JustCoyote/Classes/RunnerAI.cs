using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JustCoyote
{
    class RunnerAI
    {
        private Vector2 nextPosition;
        private DirectionEnum changeDirection;
        private static int timeInterval;

        public Vector2 NextPosition
        {
            get { return this.nextPosition; }
            private set { this.nextPosition = value; }
        }

        public Vector2 NewDirection(Vector2 currentDirection, Vector2 position, GameTime gameTime)
        {
            Array values = Enum.GetValues(typeof(DirectionEnum));
            Random randomDirect = new Random();
            Vector2 newDirection = new Vector2();

            if (timeInterval == 0)
            { 
                timeInterval = ChaneTime(gameTime);
            }
            for (int i = 0; i < 50; i++)
            {
                //position += currentDirection;
                this.NextPosition = position + currentDirection;

                if (this.NextPosition.X < 1 || this.NextPosition.Y < 1 || this.NextPosition.X > JustCoyote.GridWidth - 1 ||
                    this.NextPosition.Y > JustCoyote.GridHeight - 1 || timeInterval <= gameTime.TotalGameTime.TotalMilliseconds ||
                    Wall.Segments[(int)this.NextPosition.X, (int)this.NextPosition.Y].Filled)
                {
                    this.changeDirection = (DirectionEnum)values.GetValue(randomDirect.Next(values.Length));

                    switch (changeDirection)
                    {
                        case DirectionEnum.Up: newDirection = Direction.Up;
                            break;
                        case DirectionEnum.Left: newDirection = Direction.Left;
                            break;
                        case DirectionEnum.Down: newDirection = Direction.Down;
                            break;
                        case DirectionEnum.Right: newDirection = Direction.Right;
                            break;
                    }

                    // position -= currentDirection;
                    currentDirection = newDirection;
                    if (timeInterval <= gameTime.TotalGameTime.TotalMilliseconds)
                    {
                        timeInterval = ChaneTime(gameTime);
                    }            
                }
                else
                {
                    break;
                }
            }
            
            return currentDirection;
        }

        private int ChaneTime(GameTime gameTime)
        {
            Random randTime = new Random();
            int changeTime = randTime.Next(500, 1000) + (int)gameTime.TotalGameTime.TotalMilliseconds;
            return changeTime;
        }
    }
}
