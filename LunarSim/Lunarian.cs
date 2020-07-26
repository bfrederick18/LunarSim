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

        public Lunarian(Texture2D texture, Base aBase, RoomNode startingRoom)
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

            inInner = true;
        }

        public override void Update(GameTime gameTime, Viewport screen)
        {
            if (state == LunarianState.Idle)
            {
                if (DateTime.Now - lastWander >= untilWander)
                {
                    if (inInner)
                    {
                        double tRadian = (rand.NextDouble() * 2 - 1) * Math.PI;
                        rotation = (float)(tRadian - (Math.PI / 2));
                        float pX = (float)(Math.Cos(tRadian) * currRoom.outerRadius.X);
                        float pY = (float)(Math.Sin(tRadian) * currRoom.outerRadius.Y);

                        destination = position - new Vector2(pX, pY);

                        velocity = rotation > 0 ? new Vector2((position.X - destination.Y), -(position.Y - destination.Y)) : new Vector2(-(position.X - destination.Y), -(position.Y - destination.Y));
                        velocity *= 0.01f;
                    }
                    else
                    {

                    }
                    state = LunarianState.Wandering;
                }
                if (DateTime.Now - lastWalk >= untilWalk)
                {
                    //state = LunarianState.Walking;
                }
            }
            if (state == LunarianState.Wandering)
            {
                if (((position.X <= destination.X + (velocity.X) + 2) && (position.X >= destination.X - (velocity.X) - 2)) && ((position.Y <= destination.Y + (velocity.Y) + 2) && (position.Y >= destination.Y - (velocity.Y) - 2)))
                {
                    velocity = Vector2.Zero;
                    //lastWander = DateTime.Now;
                    //state = LunarianState.Idle;
                }
            }

            base.Update(gameTime, screen);
        }
    }
}
