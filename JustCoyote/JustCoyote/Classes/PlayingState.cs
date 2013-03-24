using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JustCoyote
{
    class PlayingState
    {
        public static int currentPlayer;
        public static void Playing(GameTime gameTime, KeyboardState keyState, bool isSingle)
        {
            for (currentPlayer = Actor.Actors.Count - 1; currentPlayer >= 0; currentPlayer--)
            {
                Actor actor = Actor.Actors[currentPlayer];
                actor.Update(gameTime);

                Player player = actor as Player;

                if (player != null)
                {
                    keyState = Keyboard.GetState(player.PlayerIndex);
                    if (currentPlayer == 1)
                    {
                        if (isSingle) // isSingle
                        {
                            if (keyState.IsKeyDown(Keys.W))
                            {
                                player.ChangeDirection(Direction.Up);
                            }

                            else if (keyState.IsKeyDown(Keys.S))
                            {
                                player.ChangeDirection(Direction.Down);
                            }

                            else if (keyState.IsKeyDown(Keys.A))
                            {
                                player.ChangeDirection(Direction.Left);
                            }

                            else if (keyState.IsKeyDown(Keys.D))
                            {
                                player.ChangeDirection(Direction.Right);
                            }

                        }
                        else
                        {
                            RunnerAI runnerAI = new RunnerAI();
                            player.ChangeDirection(runnerAI.NewDirection(player.CurrentDirection, player.Position, gameTime));
                        }
                    }
                    else
                    {

                        if (keyState.IsKeyDown(Keys.Up))
                        {
                            player.ChangeDirection(Direction.Up);
                        }

                        else if (keyState.IsKeyDown(Keys.Down)) //keyState.IsKeyDown(Keys.Down)
                        {
                            player.ChangeDirection(Direction.Down);
                        }

                        else if (keyState.IsKeyDown(Keys.Left))
                        {
                            player.ChangeDirection(Direction.Left);
                        }

                        else if (keyState.IsKeyDown(Keys.Right))
                        {
                            player.ChangeDirection(Direction.Right);
                        }
                    }

                    // TODO: accelerate and slow
                    //double speedRange = BikeStopThreshold - BikeMoveInterval;
                    //bike.MoveInterval = keyState.IsKeyDown() * speedRange + BikeMoveInterval;
                }
            }
        }
    }
}
