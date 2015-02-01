using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong
{
    public class Animation
    {
        int frameCounter;
        int switchFrame;

        Vector2 position, numFrames, currFrame;
        Texture2D image;
        Rectangle sourceRect;

        public Vector2 CurFrame
        {
            get { return currFrame; }
            set { currFrame = value; }
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Texture2D AnimationImage
        {
            set { image = value; }
        }

        public int FrameWidth
        {
            get { return image.Width / (int)numFrames.X; }
        }
        public int FrameHeight
        {
            get { return image.Height / (int)numFrames.Y; }
        }

        public void Initialize(Vector2 position, Vector2 Frames)
        {
            switchFrame = 60;
            this.position = position;
            this.numFrames = Frames;
        }

        public void Update(GameTime gameTime)
        {
            frameCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (frameCounter >= switchFrame)
            {
                frameCounter = 0;
                currFrame.X += FrameWidth;
                if(currFrame.X >= image.Width)
                {
                    currFrame.X = 0;
                    currFrame.Y += FrameHeight;
                }
                if(currFrame.Y >= image.Height)
                {
                    currFrame.Y = 0;
                }
            }
            sourceRect = new Rectangle((int)currFrame.X, (int)currFrame.Y, FrameWidth, FrameHeight);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(image, position, sourceRect, Color.White);
            spriteBatch.End();
        }
    }
}
