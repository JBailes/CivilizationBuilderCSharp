using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace CivilizationBuilder
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
		GraphicsDeviceManager gm;
		Renderer renderer;

		int windowWidth;
		int windowHeight;

		int gameState;

		public const int STATE_INTRO = 0;
		public const int STATE_MAIN_MENU = 1;
		public const int STATE_IN_GAME = 2;
		public const int STATE_START_GAME = 3;

		long curGameTime = 0;

		static int InputLastTime;

		static int tileHeightWidth = 124;

		static int terrainTileCount = 1420 / tileHeightWidth;

		private FrameCounter _frameCounter = new FrameCounter();

		public Game1()
        {
			gm = new GraphicsDeviceManager(this);
			gm.PreferredBackBufferHeight = 800;
			gm.PreferredBackBufferWidth = 1280;


			Content.RootDirectory = "Content";

			gm.PreferMultiSampling = true;

			TextureManager.SetContentManager(Content);
			FontManager.SetContentManager(Content);
		}

		public void RemoveVsync()
		{
			//IsFixedTimeStep = false;
			//gm.SynchronizeWithVerticalRetrace = false;
			//gm.ApplyChanges();
		}

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
			// Create a new SpriteBatch, which can be used to draw textures.
			renderer = new Renderer(gm, new SpriteBatch(GraphicsDevice), new SpriteBatch(GraphicsDevice));
		}

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
			windowWidth = Window.ClientBounds.Width;
			windowHeight = Window.ClientBounds.Height;

			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				World.WorldExit();
				Exit();
			}

			if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
			{
				if (InputLastTime + 600 > DateTime.Now.Millisecond)
				{
					InputLastTime = DateTime.Now.Millisecond;

					if (gameState == STATE_INTRO)
						gameState = STATE_MAIN_MENU;
					else if (gameState == STATE_MAIN_MENU)
						gameState = STATE_START_GAME;
				}
			}

			float time = 0;

			if (curGameTime == 0)
				curGameTime = DateTime.Now.Millisecond;
			else
			{
				time = DateTime.Now.Millisecond - curGameTime;
				time /= 10;
				curGameTime = DateTime.Now.Millisecond;
			}

			base.Update(gameTime);
        }

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			renderer.StartFrame();
			switch (gameState)
			{
				case STATE_START_GAME:
					StartGame();
					break;
				case STATE_IN_GAME:
					DrawInGame(gameTime);
					break;
				case STATE_MAIN_MENU:
					DrawMainMenu(gameTime);
					break;
				case STATE_INTRO:
				default:
					DrawIntro(gameTime);
					break;
			}
			renderer.EndFrame();
		}

		protected void DrawIntro(GameTime gameTime)
		{
			renderer.RenderFont("DebugSmall", "Some Intro Here - Hit Enter to Skip", 500, 10, Color.White);
		}

		protected void DrawMainMenu(GameTime gameTime)
		{
			renderer.RenderFont("DebugSmall", "Main Menu - Hit Enter to start", 500, 10, Color.White);
		}

		protected void StartGame()
		{
			renderer.RenderFullScreen("logo");
			World.WorldThreadCreation();
			gameState = STATE_IN_GAME;
		}

		protected void DrawInGame(GameTime gameTime)
		{
			renderer.RenderWorld();

			renderer.RenderImage("Dialogue", new Rectangle(10, 40, 200, 500), new Rectangle(0, 0, 50, 50));

			renderer.RenderFont("DebugSmall", Debug.worldTick, 10, 10, Color.White);
			renderer.RenderFont("DebugSmall", Debug.worldUpdate, 500, 10, Color.White);

			if (Debug.GetMessages().Count > 0)
			{
				int msgCount = (20 < Debug.GetMessages().Count) ? 20 : Debug.GetMessages().Count;

				for (int i = msgCount; i > 0; i--)
				{
					string newest = (i == 0) ? "(newest)" : "";
					renderer.RenderFont("DebugSmall", Debug.GetMessages()[Debug.GetMessages().Count - i] + newest, 20, (msgCount - i) * 25 + 45, Color.Black);
				}
			}

			var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

			_frameCounter.Update(deltaTime);

			var fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);

			renderer.RenderFont("DebugSmall", fps, 1000, 10, Color.White);
		}
	}
}
