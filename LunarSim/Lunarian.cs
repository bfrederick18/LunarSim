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
        public TimeSpan untilWalk;
        public DateTime lastWander;
        public TimeSpan untilWander;
        public Vector2 destination;
        public Base aBase;
        public RoomNode currRoom;
        private bool inInner;
        private int velocityCount;
        private float VELOCITY_MULTI;
        private Vector2 p;
        private Vector2 oldP;

        public Lunarian(Texture2D texture, Base aBase, RoomNode startingRoom, bool inInner)
           : base(texture, startingRoom.sprite.position, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0.0f, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(0.5f, 0.5f), SpriteEffects.None, 0, 1.0f, Vector2.Zero)
        {
            rand = new Random();

            rotation = (float)(rand.NextDouble() * 2 * Math.PI);
            state = LunarianState.Idle;
            lastWalk = DateTime.Now;
            untilWalk = new TimeSpan(0, 0, 0, 11);
            lastWander = DateTime.Now;
            untilWander = new TimeSpan(0, 0, 0, 1);
            this.aBase = aBase;
            currRoom = startingRoom;

            this.inInner = inInner;
            velocityCount = 0;
            VELOCITY_MULTI = 0.01f;
            p = Vector2.Zero;
            oldP = Vector2.Zero;
        }

        public override void Update(GameTime gameTime, Viewport screen)
        {
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
                if (DateTime.Now - lastWalk >= untilWalk)
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

            base.Update(gameTime, screen);
        }
    }
}
