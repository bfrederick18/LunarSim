using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LunarSim
{
    public class Base
    {
        public RoomNode head
        {
            get;
            set;
        }
        public int count
        {
            get;
            private set;
        }

        public Base(BaseSprite sprite, TimeSpan untilWalk)
        {
            RoomNode[] tempRoomArray = new RoomNode[12];
            Vector2[] tempRoomMidpointArray = new Vector2[12];
            head = new RoomNode(sprite, untilWalk, tempRoomArray, tempRoomMidpointArray);
            count = 1;
        }

        public void AddRoomAfter(BaseSprite sprite, Queue<int> index, Vector2 midpoint, TimeSpan untilWalk)
        {
            RoomNode tempNode = head;
            while (index.Count > 1)
            {
                tempNode = tempNode.adjRooms[index.Peek()];
                index.Dequeue();
            }

            int indexSuppliment = index.Peek() > 5 ? index.Peek() - 6 : index.Peek() + 6;

            RoomNode[] tempRoomArray = new RoomNode[12];
            tempRoomArray[indexSuppliment] = tempNode;
            Vector2[] tempRoomMidpointArray = new Vector2[12];
            tempRoomMidpointArray[indexSuppliment] = midpoint == Vector2.Zero ? (sprite.position + tempNode.sprite.position) / 2f : midpoint;

            RoomNode newNode = new RoomNode(sprite, untilWalk, tempRoomArray, tempRoomMidpointArray);
            newNode.howManyAdj++;
            tempNode.adjRooms[index.Peek()] = newNode;
            tempNode.adjRoomsMidpoints[index.Peek()] = midpoint == Vector2.Zero ? (sprite.position + tempNode.sprite.position) / 2f : midpoint;
            tempNode.howManyAdj++;

            count++;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            head.Draw(spriteBatch);
        }
    }
}
