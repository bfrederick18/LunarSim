using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq.Expressions;

namespace LunarSim
{
    public enum GameState { MainMenu, Simulation, MapCreator };

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Texture2D pixel;
        Vector2 centerScreen;

        BaseSprite middlePx;
        MovingSprite testPerson;

        KeyboardState ks;
        KeyboardState prevKs;

        GameState currState = GameState.Simulation;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1503;
            graphics.PreferredBackBufferHeight = 891;

            graphics.ApplyChanges();

            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            pixel = Content.Load<Texture2D>("Textures/white2By2");
            centerScreen = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

            middlePx = new BaseSprite(pixel, centerScreen, Color.Green, Vector2.One);
            testPerson = new MovingSprite(Content.Load<Texture2D>("Textures/topDownPerson_v2_0"), centerScreen, new Vector2(1f, 1f), Vector2.Zero);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            prevKs = ks;
            ks = Keyboard.GetState();

            if (currState == GameState.MainMenu)
            {
                if (ks.IsKeyDown(Keys.W) && prevKs.IsKeyUp(Keys.W))
                {
                    currState = GameState.Simulation;
                }
            }

            if (currState == GameState.Simulation)
            {
                if (ks.IsKeyDown(Keys.Q) && prevKs.IsKeyUp(Keys.Q))
                {
                    currState = GameState.MainMenu;
                }
                testPerson.Update(gameTime, GraphicsDevice.Viewport);
            }

            middlePx.Update(gameTime, GraphicsDevice.Viewport);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            if (currState == GameState.MainMenu)
            {

            }

            if (currState == GameState.Simulation)
            {
                testPerson.Draw(spriteBatch);

                middlePx.Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}