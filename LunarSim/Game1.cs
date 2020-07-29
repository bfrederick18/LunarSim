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

        BaseSprite[] corridors;
        BaseSprite[] doors;

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
            Random rand = new Random();

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
            aBase = new Base(new BaseSprite(roundRoom, centerBase, Color.White, new Vector2(1.0f, 1.0f)), new TimeSpan(0, 0, 3));

            //long rectangle: aBase-3
            q.Clear();
            q.Enqueue(3); //Med-Bay
            aBase.AddRoomAfter(new BaseSprite(squareRoom, centerBase + new Vector2(220, 45), Color.White, new Vector2(0.4f, 0.6f)), q, centerBase + new Vector2(180, 0), new TimeSpan(0, 0, 3));

            //small circle: aBase-3-9
            q.Clear();
            q.Enqueue(3);
            q.Enqueue(6);
            aBase.AddRoomAfter(new BaseSprite(roundRoom, centerBase + new Vector2(220, 135), Color.White, new Vector2(0.19f, 0.19f)), q, Vector2.Zero, new TimeSpan(0, 0, 2));

            int howFarFromMain = 225;
            for (int i = 0; i < 5; i++) //The 5 Living Quarters
            {
                q.Clear();
                q.Enqueue((i + 10) % 12);
                double tAngle = i == 0 ? Math.PI / 5 : i == 1 ? Math.PI / 5 * 2 : i == 2 ? Math.PI / 5 * 3 : i == 3 ? Math.PI / 5 * 4 : Math.PI / 1;
                double tHowFarMulti = 0.685f;
                aBase.AddRoomAfter(new BaseSprite(roundRoom, centerBase + new Vector2((float)(Math.Cos(tAngle) * howFarFromMain * tHowFarMulti), (float)(-Math.Sin(tAngle) * howFarFromMain * tHowFarMulti)), Color.White, new Vector2(0.35f, 0.35f)), q, Vector2.Zero, new TimeSpan(0, 0, 3));
            }

            //small circle: aBase-4
            q.Clear();
            q.Enqueue(4); //Main WC
            aBase.AddRoomAfter(new BaseSprite(squareRoom, centerBase + new Vector2((float)(Math.Cos(-Math.PI / 6) * howFarFromMain * 0.71f) + -10, (float)(-Math.Sin(-Math.PI / 6) * howFarFromMain * 0.71f) + 80), Color.White, new Vector2(0.29f, 0.59f)), q, new Vector2(782, 440), new TimeSpan(0, 0, 3));

            //main corridor: aBase-6
            //q.Clear();
            //q.Enqueue(6);
            //aBase.AddRoomAfter(new BaseSprite(pixel, centerBase + new Vector2(0, 200), Color.White, new Vector2(1f, 1f)), q, Vector2.Zero, new TimeSpan(0, 0, 0));

            //biq square: aBase-6-9
            q.Clear();
            q.Enqueue(7);
            aBase.AddRoomAfter(new BaseSprite(squareRoom, centerBase + new Vector2(-100, 200), Color.White, new Vector2(0.55f, 0.55f)), q, centerBase + new Vector2(0, 200), new TimeSpan(0, 0, 4));

            //small square: aBase-6-3
            q.Clear();
            q.Enqueue(5);
            aBase.AddRoomAfter(new BaseSprite(squareRoom, centerBase + new Vector2(62, 200), Color.White, new Vector2(0.25f, 0.25f)), q, centerBase + new Vector2(0, 200), new TimeSpan(0, 0, 4));

            //long rectangle at bottom: aBase-6-6
            q.Clear();
            q.Enqueue(6);
            aBase.AddRoomAfter(new BaseSprite(squareRoom, centerBase + new Vector2(0, 330), Color.White, new Vector2(0.8f, 0.42f)), q, centerBase + new Vector2(0, 200), new TimeSpan(0, 0, 4));

            RoomNode temp = aBase.head.adjRooms[6];
            aBase.head.adjRooms[5].adjRooms[6] = temp;
            aBase.head.adjRooms[6].adjRooms[5] = aBase.head.adjRooms[5];
            aBase.head.adjRooms[5].adjRoomsMidpoints[6] = aBase.head.adjRoomsMidpoints[6];
            aBase.head.adjRooms[6].adjRoomsMidpoints[5] = aBase.head.adjRoomsMidpoints[5];
            aBase.head.adjRooms[5].howManyAdj++;
            aBase.head.adjRooms[6].howManyAdj++;

            temp = aBase.head.adjRooms[7];
            aBase.head.adjRooms[5].adjRooms[7] = temp;
            aBase.head.adjRooms[7].adjRooms[5] = aBase.head.adjRooms[5];
            aBase.head.adjRooms[5].adjRoomsMidpoints[7] = aBase.head.adjRoomsMidpoints[7];
            aBase.head.adjRooms[7].adjRoomsMidpoints[5] = aBase.head.adjRoomsMidpoints[5];
            aBase.head.adjRooms[5].howManyAdj++;
            aBase.head.adjRooms[7].howManyAdj++;

            temp = aBase.head.adjRooms[7];
            aBase.head.adjRooms[6].adjRooms[7] = temp;
            aBase.head.adjRooms[7].adjRooms[6] = aBase.head.adjRooms[6];
            aBase.head.adjRooms[6].adjRoomsMidpoints[7] = aBase.head.adjRoomsMidpoints[7];
            aBase.head.adjRooms[7].adjRoomsMidpoints[6] = aBase.head.adjRoomsMidpoints[6];
            aBase.head.adjRooms[6].howManyAdj++;
            aBase.head.adjRooms[7].howManyAdj++;

            temp = aBase.head.adjRooms[4];
            aBase.head.adjRooms[5].adjRooms[4] = temp;
            aBase.head.adjRooms[4].adjRooms[5] = aBase.head.adjRooms[5];
            aBase.head.adjRooms[5].adjRoomsMidpoints[4] = centerBase + new Vector2(92, 200);
            aBase.head.adjRooms[4].adjRoomsMidpoints[5] = centerBase + new Vector2(92, 200);
            aBase.head.adjRooms[5].howManyAdj++;
            aBase.head.adjRooms[4].howManyAdj++;

            //tiny circle: aBase-6-9-9
            //q.Clear();
            //q.Enqueue(7);
            //q.Enqueue(9);
            //aBase.AddRoomAfter(new BaseSprite(roundRoom, centerBase + new Vector2(-220, 200), Color.White, new Vector2(0.15f, 0.15f)), q, Vector2.Zero, new TimeSpan(0, 0, 3));

            //small square at bottom: aBase-6-6-9
            q.Clear();
            q.Enqueue(6);
            q.Enqueue(9);
            aBase.AddRoomAfter(new BaseSprite(squareRoom, centerBase + new Vector2(-123, 330), Color.White, new Vector2(0.25f, 0.25f)), q, Vector2.Zero, new TimeSpan(0, 0, 3));

            corridors = new BaseSprite[]
            {
                new BaseSprite(squareRoom, new Vector2(821, 345), (float)Math.PI / 2, new Vector2(0.191f, 0.5f)),
                new BaseSprite(squareRoom, centerBase + new Vector2(0, 200), 0f, new Vector2(0.191f, 0.9f)),
                new BaseSprite(squareRoom, centerBase + new Vector2(0, 200), 0f, new Vector2(0.5f, 0.191f)),
                new BaseSprite(squareRoom, centerBase + new Vector2(92, 200), 0f, new Vector2(0.2f, 0.191f)),
                new BaseSprite(roundRoom, new Vector2(777, 437), 0f, new Vector2(0.17f, 0.17f))
            };
            doors = new BaseSprite[]
            {
                new BaseSprite(blank, new Vector2(821, 345), (float)Math.PI / 2, new Vector2(0.17f, 0.5f)),
                new BaseSprite(blank, centerBase + new Vector2(0, 200), 0f, new Vector2(0.18f, 0.9f)),
                new BaseSprite(blank, centerBase + new Vector2(0, 200), 0f, new Vector2(0.5f, 0.18f)),
                new BaseSprite(blank, centerBase + new Vector2(92, 200), 0f, new Vector2(0.2f, 0.18f)),
                new BaseSprite(blank, new Vector2(777, 437), (float)Math.PI / 4, new Vector2(0.3f, 0.12f)),
                new BaseSprite(blank, new Vector2(600, 675), 0f, new Vector2(0.5f, 0.17f)),
                new BaseSprite(blank, centerBase + new Vector2(220, 90), 0f, new Vector2(0.09f, 0.5f)),

                new BaseSprite(blank, centerBase + new Vector2((float)(Math.Cos(Math.PI / 5) * howFarFromMain * 0.5f), (float)(-Math.Sin(Math.PI / 5) * howFarFromMain * 0.5f)), (float)Math.PI / 5, new Vector2(0.11f, 0.11f)),
                new BaseSprite(blank, centerBase + new Vector2((float)(Math.Cos(Math.PI / 5 * 2) * howFarFromMain * 0.5f), (float)(-Math.Sin(Math.PI / 5 * 2) * howFarFromMain * 0.5f)), (float)Math.PI / 5 * 2, new Vector2(0.11f, 0.11f)),
                new BaseSprite(blank, centerBase + new Vector2((float)(Math.Cos(Math.PI / 5 * 3) * howFarFromMain * 0.5f), (float)(-Math.Sin(Math.PI / 5 * 3) * howFarFromMain * 0.5f)), (float)Math.PI / 5 * 3, new Vector2(0.11f, 0.11f)),
                new BaseSprite(blank, centerBase + new Vector2((float)(Math.Cos(Math.PI / 5 * 4) * howFarFromMain * 0.5f), (float)(-Math.Sin(Math.PI / 5 * 4) * howFarFromMain * 0.5f)), (float)Math.PI / 5 * 4, new Vector2(0.11f, 0.11f)),
                new BaseSprite(blank, centerBase + new Vector2((float)(Math.Cos(Math.PI) * howFarFromMain * 0.5f), (float)(-Math.Sin(Math.PI) * howFarFromMain * 0.5f)), (float)Math.PI, new Vector2(0.11f, 0.11f))
            };

            lunarians = new Lunarian[10];
            bool tempInInner = false;
            RoomNode[] allRooms = new RoomNode[]
            { 
                aBase.head,
                aBase.head.adjRooms[0],
                aBase.head.adjRooms[1],
                aBase.head.adjRooms[2],
                aBase.head.adjRooms[3],
                aBase.head.adjRooms[3].adjRooms[6],
                aBase.head.adjRooms[4],
                aBase.head.adjRooms[5],
                aBase.head.adjRooms[6],
                aBase.head.adjRooms[6].adjRooms[9],
                aBase.head.adjRooms[7],
                aBase.head.adjRooms[10],
                aBase.head.adjRooms[11],
            };
            for (int i = 0; i < lunarians.Length; i++)
            {
                lunarians[i] = new Lunarian(Content.Load<Texture2D>("Textures/topDownPerson_v2_0"), aBase, allRooms[(int)(rand.NextDouble() * allRooms.Length)], tempInInner);
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
                if (ks.IsKeyDown(Keys.Space) && prevKs.IsKeyUp(Keys.Space))
                {
                    Random rand = new Random();
                    lunarians[(int)(rand.NextDouble() * lunarians.Length)].GiveCovid();
                }

                for (int i = 0; i < lunarians.Length; i++)
                {
                    lunarians[i].Update(gameTime, GraphicsDevice.Viewport);
                    for (int j = 0; j < lunarians.Length; j++)
                    {
                        if (j == i)
                            continue;
                        else
                        {
                            if (lunarians[j].HasCovid() && lunarians[j].hitBox.Intersects(lunarians[i].hitBox) && !lunarians[i].HasCovid())
                            {
                                lunarians[i].ChanceGiveCovid();
                            }
                        }
                    }
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

                foreach (var e in corridors)
                {
                    e.Draw(spriteBatch);
                }

                aBase.Draw(spriteBatch);

                foreach (var e in doors)
                {
                    e.Draw(spriteBatch);
                }

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