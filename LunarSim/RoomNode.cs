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
        public float innerRadii
        {
            get;
            set;
        }
        public RoomNode[] adjRooms
        {
            get;
            set;
        }

        public RoomNode(BaseSprite sprite, float innerRadii, RoomNode[] adjRooms = null)
        {
            this.sprite = sprite;
            this.innerRadii = innerRadii;
            this.adjRooms = adjRooms;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }
    }
}
