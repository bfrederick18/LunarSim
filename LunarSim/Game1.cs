using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;

namespace LunarSim
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Texture2D pixel;
        Vector2 centerScreen;

        BaseSprite middlePx;
        Lunarian testLunarian;
        BaseSprite circularRoom;

        KeyboardState ks;
        KeyboardState prevKs;

        GameState currState = GameState.Simulation;

        Base aBase;

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
            
            circularRoom = new BaseSprite(Content.Load<Texture2D>("Textures/circularBase_v1_4"), centerScreen, Color.White, new Vector2(2f, 2f));

            aBase = new Base(circularRoom);

            /*Queue<int> aQueue = new Queue<int>();
            aQueue.Enqueue(1);
            aBase.AddRoomAfter(middlePx, aQueue);
            aQueue.Enqueue(3);
            aBase.AddRoomAfter(middlePx, aQueue);
            aQueue.Clear();
            aQueue.Enqueue(6);
            aBase.AddRoomAfter(middlePx, aQueue);*/

            testLunarian = new Lunarian(Content.Load<Texture2D>("Textures/topDownPerson_v2_0"), aBase, aBase.head);
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
                testLunarian.Update(gameTime, GraphicsDevice.Viewport);

                if (ks.IsKeyDown(Keys.A) && prevKs.IsKeyUp(Keys.A))
                {
                    currState = GameState.MainMenu;
                }
            }

            middlePx.Update(gameTime, GraphicsDevice.Viewport);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Color boi = new Color(53, 53, 53);// 255, 255, 255);//40, 75, 99);//217, 217, 217);//60, 110, 113);
            GraphicsDevice.Clear(boi);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            if (currState == GameState.MainMenu)
            {

            }

            if (currState == GameState.Simulation)
            {
                //middlePx.Draw(spriteBatch);
                circularRoom.Draw(spriteBatch);

                testLunarian.Draw(spriteBatch);

                aBase.Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}