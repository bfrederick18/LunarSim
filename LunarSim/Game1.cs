using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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

        Texture2D pixel;
        Texture2D roundRoom;
        Texture2D squareRoom;
        Texture2D blank;
        Texture2D oblong;
        Vector2 centerScreen;
        Vector2 centerBase;

        BaseSprite middlePx;

        KeyboardState ks;
        KeyboardState prevKs;

        GameState currState = GameState.Simulation;

        Base aBase;
        Lunarian[] lunarians;

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
            pixel = Content.Load<Texture2D>("Textures/white3By3");
            roundRoom = Content.Load<Texture2D>("Textures/circularBase_v2_0");
            squareRoom = Content.Load<Texture2D>("Textures/square_v2_0");
            blank = Content.Load<Texture2D>("Textures/blank_v1_0");
            oblong = Content.Load<Texture2D>("Textures/oblong_v2_0");
            centerScreen = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            centerBase = centerScreen + new Vector2(-70, -100);

            middlePx = new BaseSprite(pixel, centerScreen, Color.White, Vector2.One);

            Queue<int> q = new Queue<int>();

            //main circle: aBase
            aBase = new Base(new BaseSprite(roundRoom, centerBase, Color.White, new Vector2(1.0f, 1.0f)), new TimeSpan(0, 0, 10));

            //long rectangle: aBase-3
            q.Clear();
            q.Enqueue(3); //Med-Bay
            aBase.AddRoomAfter(new BaseSprite(squareRoom, centerBase + new Vector2(220, 45), Color.White, new Vector2(0.4f, 0.6f)), q, centerBase + new Vector2(180, 0), new TimeSpan(0, 0, 8));

            //small circle: aBase-3-9
            q.Clear();
            q.Enqueue(3);
            q.Enqueue(6);
            aBase.AddRoomAfter(new BaseSprite(roundRoom, centerBase + new Vector2(220, 145), Color.White, new Vector2(0.25f, 0.25f)), q, Vector2.Zero, new TimeSpan(0, 0, 3));

            int howFarFromMain = 225;
            for (int i = 0; i < 5; i++) //The 5 Living Quarters
            {
                q.Clear();
                q.Enqueue((i + 10) % 12);
                double tAngle = i == 0 ? Math.PI / 5 : i == 1 ? Math.PI / 5 * 2 : i == 2 ? Math.PI / 5 * 3 : i == 3 ? Math.PI / 5 * 4 : Math.PI / 1;
                double tHowFarMulti = 0.71f;
                aBase.AddRoomAfter(new BaseSprite(roundRoom, centerBase + new Vector2((float)(Math.Cos(tAngle) * howFarFromMain * tHowFarMulti), (float)(-Math.Sin(tAngle) * howFarFromMain * tHowFarMulti)), Color.White, new Vector2(0.35f, 0.35f)), q, Vector2.Zero, new TimeSpan(0, 0, 7));
            }

            //small circle: aBase-4
            q.Clear();
            q.Enqueue(4); //Main WC
            aBase.AddRoomAfter(new BaseSprite(squareRoom, centerBase + new Vector2((float)(Math.Cos(-Math.PI / 6) * howFarFromMain * 0.71f), (float)(-Math.Sin(-Math.PI / 6) * howFarFromMain * 0.71f) + 100), Color.White, new Vector2(0.3f, 0.5f)), q, Vector2.Zero, new TimeSpan(0, 0, 3));

            //main corridor: aBase-6
            //q.Clear();
            //q.Enqueue(6);
            //aBase.AddRoomAfter(new BaseSprite(pixel, centerBase + new Vector2(0, 200), Color.White, new Vector2(1f, 1f)), q, Vector2.Zero, new TimeSpan(0, 0, 0));

            //biq square: aBase-6-9
            q.Clear();
            q.Enqueue(7);
            aBase.AddRoomAfter(new BaseSprite(squareRoom, centerBase + new Vector2(-120, 200), Color.White, new Vector2(0.65f, 0.65f)), q, centerBase + new Vector2(0, 200), new TimeSpan(0, 0, 3));

            //small square: aBase-6-3
            q.Clear();
            q.Enqueue(5);
            aBase.AddRoomAfter(new BaseSprite(squareRoom, centerBase + new Vector2(60, 200), Color.White, new Vector2(0.25f, 0.25f)), q, centerBase + new Vector2(0, 200), new TimeSpan(0, 0, 3));

            //long rectangle at bottom: aBase-6-6
            q.Clear();
            q.Enqueue(6);
            aBase.AddRoomAfter(new BaseSprite(roundRoom, centerBase + new Vector2(0, 330), Color.White, new Vector2(0.45f, 0.45f)), q, centerBase + new Vector2(0, 200), new TimeSpan(0, 0, 3));

            //tiny circle: aBase-6-9-9
            q.Clear();
            q.Enqueue(7);
            q.Enqueue(9);
            aBase.AddRoomAfter(new BaseSprite(roundRoom, centerBase + new Vector2(-220, 200), Color.White, new Vector2(0.15f, 0.15f)), q, Vector2.Zero, new TimeSpan(0, 0, 3));

            //small square at bottom: aBase-6-6-9
            q.Clear();
            q.Enqueue(6);
            q.Enqueue(9);
            aBase.AddRoomAfter(new BaseSprite(roundRoom, centerBase + new Vector2(-90, 330), Color.White, new Vector2(0.25f, 0.25f)), q, Vector2.Zero, new TimeSpan(0, 0, 3));



            lunarians = new Lunarian[10];
            bool tempInInner = false;
            for (int i = 0; i < lunarians.Length; i++)
            {
                lunarians[i] = new Lunarian(Content.Load<Texture2D>("Textures/topDownPerson_v2_0"), aBase, aBase.head, tempInInner);
                tempInInner = !tempInInner;
                System.Threading.Thread.Sleep(100);
            }
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

                for (int i = 0; i < lunarians.Length; i++)
                {
                    lunarians[i].Update(gameTime, GraphicsDevice.Viewport);
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
                aBase.Draw(spriteBatch);

                for (int i = 0; i < lunarians.Length; i++)
                {
                    lunarians[i].Draw(spriteBatch);
                }
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}