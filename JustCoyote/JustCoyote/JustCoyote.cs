using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace JustCoyote
{
    //enum GameState   // TODO: For Remove
    //{
    //    Plaing,
    //    Stoped
    //}

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class JustCoyote : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ContentManager content;
        SpriteBatch spriteBatch;

        public const int ScreenWidth = 640;
        public const int ScreenHeight = 480;
        public const int GridBlockSize = 8;
        public const int GridWidth = ScreenWidth / GridBlockSize;
        public const int GridHeight = ScreenHeight / GridBlockSize;

        public static double BikeMoveInterval = 0.05d;
        public static double BikeStopThreshold = 0.2d;
        public static Vector2 BikeOrigion = new Vector2(6f, 18f);

        public static Texture2D BackgroundTexture;
        public static Texture2D BikeTexture;
        public static Texture2D TailTexture;
        public static Texture2D[] WallTextures;
        public static Color[] PlayerColors = new Color[] 
        {
            new Color(0,255,0),
            new Color(255,0,0)
        };

        private static GameState gameState;

        private Bike player1;
        private Bike player2;

        private void CreateScene()
        {
            Actor.Actors.Clear();
            Wall.Reset();

            player1 = new Bike(PlayerIndex.One, new Vector2(-1f, 10f), Direction.Right);
            player2 = new Bike(PlayerIndex.Two, new Vector2(GridWidth, GridHeight - 10f), Direction.Left);
        }

        public static void CollideWall()
        {
            gameState = GameState.Stoped;
        }

        public JustCoyote()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            CreateScene();
            gameState = GameState.Playing;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            BackgroundTexture = Content.Load<Texture2D>("background");
            BikeTexture = Content.Load<Texture2D>("coyote");
            TailTexture = Content.Load<Texture2D>("tail");

            WallTextures = new Texture2D[6];
            WallTextures[0] = Content.Load<Texture2D>("wall_h");
            WallTextures[1] = Content.Load<Texture2D>("wall_v");
            WallTextures[2] = Content.Load<Texture2D>("wall_TopLeft");
            WallTextures[3] = Content.Load<Texture2D>("wal_TopRight");
            WallTextures[4] = Content.Load<Texture2D>("wall_BottomRight");
            WallTextures[5] = Content.Load<Texture2D>("wall_BottomLeft");


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Escape))
                this.Exit();

            KeyboardState keyState;

            switch (gameState)
            {
                case GameState.Playing:

                    for (int i = Actor.Actors.Count - 1; i >= 0; i--)
                    {
                        Actor actor = Actor.Actors[i];
                        actor.Update(gameTime);

                        Bike bike = actor as Bike;
                        if (bike != null)
                        {
                            keyState = Keyboard.GetState(bike.PlayerIndex);
                            if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
                            {
                                bike.ChangeDirection(Direction.Up);
                            }

                            else if (keyState.IsKeyDown(Keys.Down))
                            {
                                bike.ChangeDirection(Direction.Down);
                            }

                            else if (keyState.IsKeyDown(Keys.Left))
                            {
                                bike.ChangeDirection(Direction.Left);
                            }

                            else if (keyState.IsKeyDown(Keys.Right))
                            {
                                bike.ChangeDirection(Direction.Right);
                            }

                            // TODO: accelerate and slow
                            //double speedRange = BikeStopThreshold - BikeMoveInterval;
                            //bike.MoveInterval = keyState.IsKeyDown() * speedRange + BikeMoveInterval;
                        }
                    }

                    break;

                case GameState.Stoped:

                    keyState = Keyboard.GetState(PlayerIndex.One);
                    if (keyState.IsKeyDown(Keys.Space))
                    {
                        CreateScene();
                        gameState = GameState.Playing;
                    }
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(BackgroundTexture, Vector2.Zero, Color.White);

            Wall.Draw(spriteBatch);

            foreach (Actor actor in Actor.Actors)
            {
                actor.Draw(spriteBatch);
            }
            // player2.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
