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
    public class Paddle : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Private Members
        private SpriteBatch spriteBatch;
        private ContentManager contentManager;

        private bool mouseControl;
        private KeyboardState oldKeyState;

        // Paddle sprite
        private Texture2D paddleSprite;

        // Paddle location
        private Vector2 paddlePosition;

        private const float DEFAULT_X_SPEED = 250;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the paddle horizontal speed.
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// Gets or sets the X position of the paddle.
        /// </summary>
        public float X
        {
            get { return paddlePosition.X; }
            set { paddlePosition.X = value; }
        }

        /// <summary>
        /// Gets or sets the Y position of the paddle.
        /// </summary>
        public float Y
        {
            get { return paddlePosition.Y; }
            set { paddlePosition.Y = value; }
        }

        public int Width
        {
            get { return paddleSprite.Width; }
        }

        /// <summary>
        /// Gets the height of the paddle's sprite.
        /// </summary>
        public int Height
        {
            get { return paddleSprite.Height; }
        }

        /// <summary>
        /// Gets the bounding rectangle of the paddle.
        /// </summary>
        public Rectangle Boundary
        {
            get
            {
                return new Rectangle((int)paddlePosition.X, (int)paddlePosition.Y,
                    paddleSprite.Width, paddleSprite.Height);
            }
        }

        public BoundingSphere CircleBoundary
        {
            get
            {
                Vector3 spherePos = new Vector3(paddlePosition.X + (paddleSprite.Width / 2), paddlePosition.Y + (paddleSprite.Width / 2), 0);
                return new BoundingSphere(spherePos, (paddleSprite.Width / 2));
            }
        }

        #endregion

        public Paddle(Game game)
            : base(game)
        {
            contentManager = new ContentManager(game.Services);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // Make sure base.Initialize() is called before this or handSprite will be null
            X = 0;
            Y = (GraphicsDevice.Viewport.Height - Height) / 2;

            Speed = DEFAULT_X_SPEED;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            paddleSprite = contentManager.Load<Texture2D>(@"Content\Images\butter_fairy");
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Scale the movement based on time
            float moveDistance = Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move paddle, but don't allow movement off the screen

            KeyboardState newKeyState = Keyboard.GetState();
            if (oldKeyState != null && oldKeyState.IsKeyUp(Keys.M) && newKeyState.IsKeyDown(Keys.M))
            {
                mouseControl = !mouseControl;
            }

            if (mouseControl)
            {
                int mouseY = Mouse.GetState().Y;
                if (mouseY - (paddleSprite.Height / 2) < 0)
                {
                    Y = 0;
                }
                else if (mouseY + (paddleSprite.Height / 2) > GraphicsDevice.Viewport.Height)
                {
                    Y = GraphicsDevice.Viewport.Height - paddleSprite.Height;
                }
                else
                {
                    Y = mouseY - (paddleSprite.Height / 2);
                }
            }
            else
            {
                if (newKeyState.IsKeyDown(Keys.Down) && Y + paddleSprite.Height
                + moveDistance <= GraphicsDevice.Viewport.Height)
                {
                    Y += moveDistance;
                }
                else if (newKeyState.IsKeyDown(Keys.Up) && Y - moveDistance >= 0)
                {
                    Y -= moveDistance;
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
            spriteBatch.Begin();
            spriteBatch.Draw(paddleSprite, paddlePosition, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
