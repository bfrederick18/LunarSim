using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunarSim
{
    public class RoomNode
    {
        public BaseSprite sprite
        {
            get;
            set;
        }
        public float innerRadius
        {
            get;
            set;
        }
        public Vector2 outerRadius
        {
            get;
            set;
        }
        public RoomNode[] adjRooms
        {
            get;
            set;
        }
        public Vector2[] adjRoomsMidpoints
        {
            get;
            set;
        }
        public int howManyAdj;
        public TimeSpan untilWalk;
        private int drawn;

        public RoomNode(BaseSprite sprite, TimeSpan untilWalk, RoomNode[] adjRooms = null, Vector2[] adjRoomsMidpoints = null)
        {
            this.sprite = sprite;
            innerRadius = sprite.texture.Width * sprite.scale.X > sprite.texture.Height * sprite.scale.Y ? sprite.texture.Height * sprite.scale.Y / 4 : sprite.texture.Width * sprite.scale.X / 4;
            outerRadius = new Vector2(sprite.texture.Width * sprite.scale.X / 2 - 10, sprite.texture.Height * sprite.scale.Y / 2 - 10);
            this.adjRooms = adjRooms;
            this.adjRoomsMidpoints = adjRoomsMidpoints;
            howManyAdj = 0;
            this.untilWalk = untilWalk;

            drawn = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
            drawn++;
            foreach (var e in adjRooms)
            {
                if (e != null && e.drawn < drawn)
                {
                    e.Draw(spriteBatch);
                }
            }
        }
    }
}
