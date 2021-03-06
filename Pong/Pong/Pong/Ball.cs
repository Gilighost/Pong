using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Pong
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Ball : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Private Members
        private SpriteBatch spriteBatch;
        private ContentManager contentManager;

        // Texture stuff
        Point frameSize = new Point(110, 128);
        Point currentFrame = new Point(0, 0);
        Point sheetSize = new Point(8, 10);
        // Framerate stuff
        int timeSinceLastFrame = 0;
        int millisecondsPerFrame = 75;

        // Default speed of ball
        private const float DEFAULT_X_SPEED = -150;
        private const float DEFAULT_Y_SPEED = 150;

        // Initial location of the ball
        private const float INIT_X_POS = 370;
        private const float INIT_Y_POS = 200;

        // Increase in speed each hit
        private const float INCREASE_SPEED = 50;

        // Ball image
        private Texture2D ballSprite;

        // Ball location
        Vector2 ballPosition;

        // Ball's motion
        Vector2 ballSpeed = new Vector2(DEFAULT_X_SPEED, DEFAULT_Y_SPEED);
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the ball's horizontal speed.
        /// </summary>
        /// 
        public float SpeedX
        {
            get { return ballSpeed.X; }
            set { ballSpeed.X = value; }
        }

        /// <summary>
        /// Gets or sets the ball's vertical speed.
        /// </summary>
        public float SpeedY
        {
            get { return ballSpeed.Y; }
            set { ballSpeed.Y = value; }
        }

        /// <summary>
        /// Gets or sets the X position of the ball.
        /// </summary>
        public float X
        {
            get { return ballPosition.X; }
            set { ballPosition.X = value; }
        }

        /// <summary>
        /// Gets or sets the Y position of the ball.
        /// </summary>
        public float Y
        {
            get { return ballPosition.Y; }
            set { ballPosition.Y = value; }
        }

        /// <summary>
        /// Gets the width of the ball's sprite.
        /// </summary>
        public int Width
        {
            get { return frameSize.X; }
        }

        /// <summary>
        /// Gets the height of the ball's sprite.
        /// </summary>
        public int Height
        {
            get { return frameSize.Y; }
        }

        /// <summary>
        /// Gets the bounding rectangle of the ball.
        /// </summary>
        public Rectangle Boundary
        {
            get
            {
                return new Rectangle((int)ballPosition.X, (int)ballPosition.Y,
                    frameSize.X, frameSize.Y);
            }
        }
        /// <summary>
        /// Returns a BoundingSphere centered on the ball.
        /// </summary>
        public BoundingSphere CircleBoundary
        {
            get
            {
                Vector3 spherePos = new Vector3(ballPosition.X + (frameSize.X / 2), ballPosition.Y + (frameSize.Y / 2), 0);
                return new BoundingSphere(spherePos, 30);
            }
        }
        #endregion

        public Ball(Game game)
            : base(game)
        {
            contentManager = new ContentManager(game.Services);
            
        }

        /// <summary>
        /// Set the ball at the top of the screen with default speed.
        /// </summary>
        public void Reset()
        {
            ballSpeed.X = DEFAULT_X_SPEED;
            ballSpeed.Y = DEFAULT_Y_SPEED;

            ballPosition.X = 370;

            //random direction
            Random randNum = new Random();
            int direction = randNum.Next(2);

            if(direction == 0)
            {
                ballSpeed.X *= -1;
            }
            else
            {
                ballSpeed.X *= 1;
            }

            // Make sure ball is not positioned off the screen
            if (ballPosition.Y < 0)
                ballPosition.Y = 0;
            else if (ballPosition.Y + ballSprite.Height > GraphicsDevice.Viewport.Height)
            {
                ballPosition.Y = GraphicsDevice.Viewport.Height - ballSprite.Height;
                ballSpeed.X *= -1;
            }
        }

        /// <summary>
        /// Increase the ball's speed in the X and Y directions.
        /// </summary>
        public void SpeedUp()
        {
            if (ballSpeed.Y < 0)
                ballSpeed.Y -= INCREASE_SPEED;
            else
                ballSpeed.Y += INCREASE_SPEED;

            if (ballSpeed.X < 0)
                ballSpeed.X -= INCREASE_SPEED;
            else
                ballSpeed.X += INCREASE_SPEED;
        }

        /// <summary>
        /// Invert the ball's horizontal direction
        /// </summary>
        public void ChangeHorzDirection()
        {
            ballSpeed.X *= -1;
        }

        /// <summary>
        /// Invert the ball's vertical direction
        /// </summary>
        public void ChangeVertDirection()
        {
            ballSpeed.Y *= -1;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            ballPosition.X = INIT_X_POS;
            ballPosition.Y = INIT_Y_POS;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load the texture if it exists
            //sprite sheet found here: http://celestialcoding.com/index.php?action=dlattach;topic=1209.0;attach=685;image
            ballSprite = contentManager.Load<Texture2D>(@"Content\Images\dance");
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {

            // Move the sprite by speed, scaled by elapsed time.
            ballPosition += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds; if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame -= millisecondsPerFrame;
                ++currentFrame.X;
                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= sheetSize.Y)
                        currentFrame.Y = 0;
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
         
            base.Draw(gameTime);

            spriteBatch.Begin();
            spriteBatch.Draw(ballSprite, ballPosition, new Rectangle(currentFrame.X * frameSize.X,
                    currentFrame.Y * frameSize.Y,
                    frameSize.X,
                    frameSize.Y),
                Color.White);
            spriteBatch.End();

        }
    }
}
