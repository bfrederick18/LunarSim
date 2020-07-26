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

        public RoomNode(BaseSprite sprite, RoomNode[] adjRooms = null)
        {
            this.sprite = sprite;
            innerRadius = sprite.texture.Width * sprite.scale.X > sprite.texture.Height * sprite.scale.Y ? sprite.texture.Height * sprite.scale.Y / 4 : sprite.texture.Width * sprite.scale.X / 4;
            outerRadius = new Vector2(sprite.texture.Width * sprite.scale.X / 2, sprite.texture.Height * sprite.scale.Y / 2);
            this.adjRooms = adjRooms;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }
    }
}
