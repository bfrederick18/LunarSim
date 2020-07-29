using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LunarSim
{
    public class Lunarian : MovingSprite
    {
        public LunarianState state;
        public DateTime lastWalk;
        public DateTime lastWander;
        public TimeSpan untilWander;
        public Vector2 destination;
        public Base aBase;
        private RoomNode currRoom;
        private bool inInner;
        private int velocityCount;
        private float VELOCITY_MULTI;
        private Vector2 p;
        private Vector2 oldP;
        private bool covid;

        public Lunarian(Texture2D texture, Base aBase, RoomNode startingRoom, bool inInner)
           : base(texture, startingRoom.sprite.position, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0.0f, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(0.5f, 0.5f), SpriteEffects.None, 0, 1.0f, Vector2.Zero)
        {
            rand = new Random();

            rotation = (float)(rand.NextDouble() * 2 * Math.PI);
            state = LunarianState.Idle;
            lastWalk = DateTime.Now;
            lastWander = DateTime.Now;
            untilWander = new TimeSpan(0, 0, 0, 1);
            this.aBase = aBase;
            currRoom = startingRoom;

            this.inInner = inInner;
            velocityCount = 0;
            VELOCITY_MULTI = 0.02f;
            p = Vector2.Zero;
            oldP = Vector2.Zero;
            covid = false;
        }

        public bool HasCovid()
        {
            return covid;
        }
        public void ChanceGiveCovid()
        {
            int tempChance = (int)(rand.NextDouble() * 100);
            if (tempChance == 1)
            {
                covid = true;
            }
        }
        public void GiveCovid()
        {
            covid = true;
        }

        public override void Update(GameTime gameTime, Viewport screen)
        {
            if (covid)
                tint = Color.Yellow;

            if (state == LunarianState.Idle)
            {
                if (DateTime.Now - lastWander >= untilWander)
                {
                    oldP = p;
                    double tRadian = (rand.NextDouble() * 2 * Math.PI);
                    //rotation = (float)(tRadian - (Math.PI / 2));

                    float pX = 0.0f, pY = 0.0f;
                    if (inInner)
                    {
                        pX = (float)(Math.Cos(tRadian) * (currRoom.innerRadius + (currRoom.outerRadius.X - currRoom.innerRadius) * rand.NextDouble())); //(rotRange.X + (rotRange.Y - rotRange.X)
                        pY = (float)(Math.Sin(tRadian) * (currRoom.innerRadius + (currRoom.outerRadius.Y - currRoom.innerRadius) * rand.NextDouble()));
                    }
                    else if(!inInner)
                    {
                        pX = (float)(Math.Cos(tRadian) * (currRoom.innerRadius) * rand.NextDouble()); //(rotRange.X + (rotRange.Y - rotRange.X)
                        pY = (float)(Math.Sin(tRadian) * (currRoom.innerRadius) * rand.NextDouble());
                    }
                    
                    p = new Vector2(pX, pY);
                    Vector2 r = -oldP + p;

                    rotation = (float)(Math.Atan2(r.Y, r.X) - (Math.PI / 2));

                    destination = currRoom.sprite.position - p;

                    velocity = new Vector2((position.X - destination.X), (position.Y - destination.Y));// rotation > Math.PI ? new Vector2(-(position.X - destination.Y), -(position.Y - destination.Y)) : new Vector2((position.X - destination.Y), -(position.Y - destination.Y));
                    velocity *= -VELOCITY_MULTI;

                    state = LunarianState.Wandering;
                }
                if (DateTime.Now - lastWalk >= currRoom.untilWalk)
                {
                    //state = LunarianState.Walking;
                }
            }
            if (state == LunarianState.Wandering)
            {
                velocityCount++;
                if (velocityCount > 1 / VELOCITY_MULTI)
                {
                    Vector2 debugVelocity = velocity;
                    velocity = Vector2.Zero;
                    position = destination;
                    lastWander = DateTime.Now;
                    untilWander = new TimeSpan(0, 0, 0, (int)(rand.NextDouble() * 2), (int)(rand.NextDouble() * 1000));
                    inInner = !inInner;
                    state = LunarianState.Idle;
                    velocityCount = 0;
                }
            }

            if(state == LunarianState.Idle && inInner)
            {
                if (DateTime.Now - lastWalk >= currRoom.untilWalk)
                {
                    int[] tempMidpoint = new int[currRoom.howManyAdj];
                    int howManySoFar = 0;

                    for (int i = 0; i < currRoom.adjRoomsMidpoints.Length; i++)
                    {
                        if (currRoom.adjRoomsMidpoints[i] != Vector2.Zero)
                        {
                            tempMidpoint[howManySoFar] = i;
                            howManySoFar++;
                        }
                    }

                    int whichRoom = (int)(rand.NextDouble() * (tempMidpoint.Length));

                    destination = currRoom.adjRoomsMidpoints[tempMidpoint[whichRoom]];

                    Vector2 r = -destination + position;

                    rotation = (float)(Math.Atan2(r.Y, r.X) - (Math.PI / 2));

                    velocity = new Vector2((position.X - destination.X), (position.Y - destination.Y));
                    velocity *= -VELOCITY_MULTI;

                    currRoom = currRoom.adjRooms[tempMidpoint[whichRoom]];
                    if (currRoom == null)
                    {
                        tint = Color.GreenYellow;
                    }
                    state = LunarianState.WalkingOne;
                }
            }
            if (state == LunarianState.WalkingOne)
            {
                velocityCount++;
                if (velocityCount > 1 / VELOCITY_MULTI)
                {
                    oldP = p;
                    double tRadian = (rand.NextDouble() * 2 * Math.PI);
                    //rotation = (float)(tRadian - (Math.PI / 2));

                    float pX = (float)(Math.Cos(tRadian) * (currRoom.innerRadius) * rand.NextDouble());
                    float pY = (float)(Math.Sin(tRadian) * (currRoom.innerRadius) * rand.NextDouble());

                    p = new Vector2(pX, pY);
                    destination = currRoom.sprite.position - p;
                    
                    Vector2 r = -destination + position;

                    rotation = (float)(Math.Atan2(r.Y, r.X) - (Math.PI / 2));

                    velocity = new Vector2((position.X - destination.X), (position.Y - destination.Y));// rotation > Math.PI ? new Vector2(-(position.X - destination.Y), -(position.Y - destination.Y)) : new Vector2((position.X - destination.Y), -(position.Y - destination.Y));
                    velocity *= -VELOCITY_MULTI;

                    state = LunarianState.WalkingTwo;
                    velocityCount = 0;
                }
            }
            if (state == LunarianState.WalkingTwo)
            {
                velocityCount++;
                if (velocityCount > 1 / VELOCITY_MULTI)
                {
                    Vector2 debugVelocity = velocity;
                    velocity = Vector2.Zero;
                    position = destination;
                    lastWalk = DateTime.Now;
                    lastWander = DateTime.Now;
                    state = LunarianState.Idle;
                    velocityCount = 0;
                }
            }

            base.Update(gameTime, screen);
        }
    }
}
