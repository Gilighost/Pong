using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pong
{
    class Enemy : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Private Members
        private SpriteBatch spriteBatch;
        private ContentManager contentManager;

        // enemy sprite
        private Texture2D enemySprite;

        // enemy location
        private Vector2 enemyPosition;

        private const float DEFAULT_X_SPEED = 250;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the enemy horizontal speed.
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// Gets or sets the X position of the enemy.
        /// </summary>
        public float X
        {
            get { return enemyPosition.X; }
            set { enemyPosition.X = value; }
        }

        /// <summary>
        /// Gets or sets the Y position of the enemy.
        /// </summary>
        public float Y
        {
            get { return enemyPosition.Y; }
            set { enemyPosition.Y = value; }
        }

        public int Width
        {
            get { return enemySprite.Width; }
        }

        /// <summary>
        /// Gets the height of the enemy's sprite.
        /// </summary>
        public int Height
        {
            get { return enemySprite.Height; }
        }

        /// <summary>
        /// Gets the bounding rectangle of the enemy.
        /// </summary>
        public Rectangle Boundary
        {
            get
            {
                return new Rectangle((int)enemyPosition.X, (int)enemyPosition.Y,
                    enemySprite.Width, enemySprite.Height);
            }
        }

        #endregion

         public Enemy(Game game)
            : base(game)
        {
            contentManager = new ContentManager(game.Services);
        }
        public override void Initialize()
        {
            base.Initialize();

            // Make sure base.Initialize() is called before this or handSprite will be null
            X = GraphicsDevice.Viewport.Width - enemySprite.Width;
            Y = (GraphicsDevice.Viewport.Height - Height) / 2;

            Speed = DEFAULT_X_SPEED;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            enemySprite = contentManager.Load<Texture2D>(@"Content\Images\fairy2");
        }

        public override void Update(GameTime gameTime)
        {
            // Scale the movement based on time
            float moveDistance = Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move paddle, but don't allow movement off the screen

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(enemySprite, enemyPosition, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Move(Ball ball, GameTime gameTime)
        {
            float moveDistance = Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (ball.Y > this.Y && Y + enemySprite.Height
               + moveDistance <= GraphicsDevice.Viewport.Height)
            {
                Y += moveDistance;
            }
            else if (ball.Y < this.Y && Y - moveDistance >= 0)
            {
                Y -= moveDistance;
            }
        }
    }
}
