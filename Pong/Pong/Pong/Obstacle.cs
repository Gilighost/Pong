using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Pong
{
    class Obstacle : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Private Members
        private SpriteBatch spriteBatch;
        private ContentManager contentManager;

        // enemy sprite
        private Texture2D obstacleSprite;

        // enemy location
        private Vector2 obstaclePosition;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the X position of the obstacle.
        /// </summary>
        public float X
        {
            get { return obstaclePosition.X; }
            set { obstaclePosition.X = value; }
        }

        /// <summary>
        /// Gets or sets the Y position of the obstacle.
        /// </summary>
        public float Y
        {
            get { return obstaclePosition.Y; }
            set { obstaclePosition.Y = value; }
        }

        public int Width
        {
            get { return obstacleSprite.Width; }
        }

        /// <summary>
        /// Gets the height of the obstacle's sprite.
        /// </summary>
        public int Height
        {
            get { return obstacleSprite.Height; }
        }

        /// <summary>
        /// Gets the bounding rectangle of the obstacle.
        /// </summary>
        public Rectangle Boundary
        {
            get
            {
                return new Rectangle((int)obstaclePosition.X, (int)obstaclePosition.Y,
                    obstacleSprite.Width, obstacleSprite.Height);
            }
        }
        public BoundingSphere CircleBoundary
        {
            get
            {
                Vector3 spherePos = new Vector3(obstaclePosition.X + (obstacleSprite.Width / 2), obstaclePosition.Y + (obstacleSprite.Width / 2), 0);
                return new BoundingSphere(spherePos, (obstacleSprite.Width / 2));
            }
        }

        #endregion

         public Obstacle(Game game)
            : base(game)
        {
            contentManager = new ContentManager(game.Services);
        }

         public override void Initialize()
         {
             base.Initialize();

             // Make sure base.Initialize() is called before this or handSprite will be null
             X = (GraphicsDevice.Viewport.Width / 2) - (Width / 2);
             Y = (GraphicsDevice.Viewport.Height / 2) - (Height / 2);
         }
         protected override void LoadContent()
         {
             spriteBatch = new SpriteBatch(GraphicsDevice);

             obstacleSprite = contentManager.Load<Texture2D>(@"Content\Images\navi");
         }
         public override void Draw(GameTime gameTime)
         {
             spriteBatch.Begin();
             spriteBatch.Draw(obstacleSprite, obstaclePosition, Color.White);
             spriteBatch.End();

             base.Draw(gameTime);
         }
    }
}
