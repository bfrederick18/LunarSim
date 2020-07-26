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

        public Base(BaseSprite sprite)
        {
            RoomNode[] tempRoomArray = new RoomNode[12];
            head = new RoomNode(sprite, tempRoomArray);
            count = 1;
        }

        public void AddRoomAfter(BaseSprite sprite, Queue<int> index)
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

            RoomNode newNode = new RoomNode(sprite, tempRoomArray);
            tempNode.adjRooms[index.Peek()] = newNode;

            count++;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            head.Draw(spriteBatch);
        }
    }
}
