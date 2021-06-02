using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RC_Framework;

namespace Assign
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private string dir;
        public  MouseState currentMouseState;
        public  MouseState previouseMouseState;
        public static KeyboardState keyState;     // must use or keystate can be unstable on level change
        public static KeyboardState prevKeyState; //
        int screenWidth;
        int ScreenHeight;
        public static Rectangle screenRect;

        RC_GameStateManager levelManager;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
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
            spriteBatch = new SpriteBatch(GraphicsDevice);

            screenWidth = GraphicsDevice.Viewport.Width;
            ScreenHeight = GraphicsDevice.Viewport.Height;
            screenRect = new Rectangle(0, 0, screenWidth, ScreenHeight);
            LineBatch.init(GraphicsDevice);


            levelManager = new RC_GameStateManager();

            levelManager.AddLevel(0, new GameLevel_0());
            levelManager.getLevel(0).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(0).LoadContent();

            levelManager.AddLevel(1, new GameLevel_1());
            levelManager.getLevel(1).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(1).LoadContent();


            levelManager.AddLevel(2, new GameLevel_2());
            levelManager.getLevel(2).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(2).LoadContent();
           // levelManager.setLevel(2);

            levelManager.AddLevel(3, new GameLevel_3()); // note pause screen is level 2
            levelManager.getLevel(3).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(3).LoadContent();
            //levelManager.setLevel(3);

            levelManager.AddLevel(4, new GameLevel_4());
            levelManager.getLevel(4).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(4).LoadContent();
            levelManager.setLevel(4);

            levelManager.AddLevel(5, new GameLevel_5_Pause());
            levelManager.getLevel(5).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(5).LoadContent();

            levelManager.AddLevel(6, new GameLevel_6());
            levelManager.getLevel(6).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(6).LoadContent();

            levelManager.AddLevel(7, new GameLevel_7());
            levelManager.getLevel(7).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(7).LoadContent();

            levelManager.AddLevel(8, new GameLevel_8());
            levelManager.getLevel(8).InitializeLevel(GraphicsDevice, spriteBatch, Content, levelManager);
            levelManager.getLevel(8).LoadContent();


            // TODO: use this.Content to load your game content here
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            prevKeyState = keyState;
            keyState = Keyboard.GetState();
            this.previouseMouseState = this.currentMouseState;
            this.currentMouseState = Mouse.GetState();

            levelManager.getCurrentLevel().Update(gameTime);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            levelManager.getCurrentLevel().Draw(gameTime);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
