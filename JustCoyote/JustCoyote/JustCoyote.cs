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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class JustCoyote : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        Menu menu;

        public const int ScreenWidth = 990;
        public const int ScreenHeight = 680;
        public const int GridBlockSize = 8;
        public const int GridWidth = ScreenWidth / GridBlockSize;
        public const int GridHeight = ScreenHeight / GridBlockSize;

        public static double BikeMoveInterval = 0.05d;
        public static double BikeStopThreshold = 0.2d;
        public static Vector2 BikeOrigion = new Vector2(6f, 18f);
        Vector2 origin = new Vector2(0f, 0f);

        public static Texture2D TitleScreen;
        public static Texture2D GameOverCoyote;
        public static Texture2D GameOverRunner;
        public static Texture2D BackgroundTexture;
        public static Texture2D CoyoteBikeTexture;
        public static Texture2D RunnerBikeTexture;
        public static Texture2D TailTexture;
        public static Texture2D[] WallTextures;
        public static Color[] PlayerColors = new Color[] 
        {
            new Color(51, 51, 153),
            new Color(255, 51, 0)
        };

        private static GameState gameState;
        private bool isSingle = true;

        private Player player1;
        private Player player2;
        private static int playerWin;

        private void CreateScene()
        {
            Actor.Actors.Clear();
            Wall.Reset();

            player1 = new Player(PlayerIndex.One, new Vector2(-1f, 10f), Direction.Right);
            player2 = new Player(PlayerIndex.Two, new Vector2(GridWidth, GridHeight - 10f), Direction.Left);
        }

        public static void CollideWall()
        {
            gameState = GameState.Stoped;
            playerWin = PlayingState.currentPlayer;

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
            menu = new Menu();
            CreateScene();
            gameState = GameState.TitleScreen;

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
            spriteFont = Content.Load<SpriteFont>("SpriteFont");


            TitleScreen = Content.Load<Texture2D>("titlescreen");
            GameOverCoyote = Content.Load<Texture2D>("gameovercoyote");
            GameOverRunner = Content.Load<Texture2D>("gameoverrunner");
            BackgroundTexture = Content.Load<Texture2D>("background");
            CoyoteBikeTexture = Content.Load<Texture2D>("coyote");
            RunnerBikeTexture = Content.Load<Texture2D>("runner");
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

            KeyboardState keyState = new KeyboardState();

            switch (gameState)
            {
                case GameState.TitleScreen:
                    int selected = menu.Update(gameTime);

                    switch (selected)
                    {
                        case 0:
                            isSingle = false;
                            gameState = GameState.Playing;
                            break;
                        case 1:
                            isSingle = true;
                            gameState = GameState.Playing;
                            break;
                        case 2:
                            this.Exit();
                            break;
                    }
                    break;

                case GameState.Playing:
                    PlayingState.Playing(gameTime, keyState, isSingle);
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
            switch (gameState)
            {
                case GameState.TitleScreen:
                    spriteBatch.Draw(TitleScreen, Vector2.Zero, Color.White);
                    menu.Draw(spriteBatch, spriteFont);
                    break;

                case GameState.Playing:
                    spriteBatch.Draw(BackgroundTexture, Vector2.Zero, Color.White);

                    Wall.Draw(spriteBatch);

                    player1.Draw(spriteBatch, CoyoteBikeTexture);

                    player2.Draw(spriteBatch, RunnerBikeTexture);
                    break;

                case GameState.Stoped:
                    if (playerWin == 0)
                    {
                        spriteBatch.Draw(GameOverCoyote, Vector2.Zero, Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(GameOverRunner, Vector2.Zero, Color.White);
                    }
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
