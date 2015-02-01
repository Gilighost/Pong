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

        //added for animation
        Texture2D ballImage;
        Vector2 framePosition;
        Animation ballAnimation = new Animation();

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
            get { return ballImage.Width; }
        }

        /// <summary>
        /// Gets the height of the ball's sprite.
        /// </summary>
        public int Height
        {
            get { return ballImage.Height; }
        }

        /// <summary>
        /// Gets the bounding rectangle of the ball.
        /// </summary>
        public Rectangle Boundary
        {
            get
            {
                return new Rectangle((int)ballPosition.X, (int)ballPosition.Y,
                    ballImage.Width, ballImage.Height);
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

            // Make sure ball is not positioned off the screen
            if (ballPosition.Y < 0)
                ballPosition.Y = 0;
            else if (ballPosition.Y + ballImage.Height > GraphicsDevice.Viewport.Height)
            {
                ballPosition.Y = GraphicsDevice.Viewport.Height - ballImage.Height;
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

            ballAnimation.Initialize(framePosition, new Vector2(8, 10));
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //added for anim
            ballImage = contentManager.Load<Texture2D>(@"Content/Images/dance");
            ballAnimation.AnimationImage = ballImage;

            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the texture if it exists
            //ballSprite = contentManager.Load<Texture2D>(@"Content\Images\basketball");
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //added for anim
            ballAnimation.Update(gameTime);

            // Move the sprite by speed, scaled by elapsed time.
            ballPosition += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            ballAnimation.Position = ballPosition;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            //added for anim
            ballAnimation.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
