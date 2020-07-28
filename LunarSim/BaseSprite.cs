using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LunarSim
{
    public class BaseSprite
    {
        private bool debug = false;

        #region protected
        public Texture2D texture;
        public Vector2 position;
        protected Rectangle sourceRectangle;
        protected Color tint;
        protected float rotation;
        public Vector2 scale;
        protected float layerDepth;
        protected Vector2 origin;
        protected SpriteEffects effects;
        public Rectangle hitBox;
        #endregion

        public float transparency;

        public Random rand;

        public BaseSprite(Texture2D texture, Vector2 position)
            : this(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0, 1.0f)
        {
            origin = new Vector2(sourceRectangle.Width / 2, sourceRectangle.Height / 2);

        }

        public BaseSprite(Texture2D texture, Vector2 position, Color tint, Vector2 scale)
            : this(texture, position)
        {
            this.tint = tint;
            this.scale = scale;
        }

        public BaseSprite(Texture2D texture, Vector2 position, float rotation, Vector2 scale)
            : this(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotation, Vector2.Zero, scale, SpriteEffects.None, 0, 1.0f)
        {
            origin = new Vector2(sourceRectangle.Width / 2, sourceRectangle.Height / 2);
        }

        public BaseSprite(Texture2D texture, Vector2 position, Rectangle sourceRectangle, Color tint, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth, float transparency)
        {
            this.texture = texture;
            this.position = position;
            this.sourceRectangle = sourceRectangle;
            this.tint = tint;
            this.rotation = rotation;
            this.origin = origin;
            this.scale = scale;
            this.effects = effects;
            this.layerDepth = layerDepth;
            this.transparency = transparency;

            rand = new Random();
        }

        public virtual void Update(GameTime gameTime, Viewport screen) 
        {
            if (debug)
            {
                Console.WriteLine("Hitbox Values: {0}, {1}, {2}, {3}", hitBox.X, hitBox.Y, hitBox.Width, hitBox.Height);
            }

            UpdateHitBoxes();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, tint * transparency, rotation, origin, scale, effects, layerDepth);

            if (debug)
            {
                spriteBatch.Draw(texture, hitBox, Color.Purple * 0.5f);
            }
        }

        private void UpdateHitBoxes()
        {
            //hitBox.X = (int)(position.X - (texture.Width * scale.X / 2));
            //hitBox.Y = (int)(position.Y - (texture.Height * scale.Y / 2));

            hitBox.X = (int)(position.X - (texture.Width * scale.X / 2)) + 1;
            hitBox.Y = (int)(position.Y - (texture.Height * scale.Y / 2)) + 1;
            hitBox.Width = (int)(sourceRectangle.Width * scale.X);
            hitBox.Height = (int)(sourceRectangle.Height * scale.Y);
        }
    }
}
