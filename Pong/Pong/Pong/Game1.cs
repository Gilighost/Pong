/*
 * Basketball Pong
 * by Frank McCown, Harding University
 * Spring 2012
 * 
 * Sounds: Creative Commons Sampling Plus 1.0 License.
 * http://www.freesound.org/samplesViewSingle.php?id=34201
 * http://www.freesound.org/samplesViewSingle.php?id=12658
 */

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
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Texture2D background;
        private Rectangle mainFrame;

        private SpriteFont font;

        private Ball ball;
        private Paddle paddle;
        private Enemy enemy;

        private int playerScore, enemyScore;

        private SoundEffect swishSound;
        private SoundEffect crashSound;

        private Song fairyMusic;
        private bool songStart = false;

        // Used to delay between rounds 
        private float delayTimer = 0;

        static Vector2 playerScorePos = new Vector2(350, 400);
        static Vector2 enemyScorePos = new Vector2(420, 400);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            ball = new Ball(this);
            paddle = new Paddle(this);
            enemy = new Enemy(this);

            playerScore = 0;
            enemyScore = 0;

            Components.Add(ball);
            Components.Add(paddle);
            Components.Add(enemy); 

            // Call Window_ClientSizeChanged when screen size is changed
            this.Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            // Move paddle back onto screen if it's off
            paddle.Y = GraphicsDevice.Viewport.Height - paddle.Height;
            if (paddle.X + paddle.Width > GraphicsDevice.Viewport.Width)
                paddle.X = GraphicsDevice.Viewport.Width - paddle.Width;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Make mouse visible
            IsMouseVisible = true;

            // Set the window's title bar
            Window.Title = "Basketball Pong!";

            graphics.ApplyChanges();

            // Don't allow ball to move just yet
            ball.Enabled = false;  



            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            background = Content.Load<Texture2D>(@"Images\fairy_castle");
            mainFrame = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            font = Content.Load<SpriteFont>(@"fairyFont");

            fairyMusic = Content.Load<Song>(@"Audio\fairy_song");
            MediaPlayer.IsRepeating = true; 

            swishSound = Content.Load<SoundEffect>(@"Audio\bell");
            crashSound = Content.Load<SoundEffect>(@"Audio\chime");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //start music
            if (!songStart)
            {
                MediaPlayer.Play(fairyMusic);
                songStart = true;
            }

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // Press F to toggle full-screen mode
            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                graphics.IsFullScreen = !graphics.IsFullScreen;
                graphics.ApplyChanges();
            }

            // Wait until a second has passed before animating ball 
            delayTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (delayTimer > 1)            
                ball.Enabled = true;
            
            int maxX = GraphicsDevice.Viewport.Width - ball.Width;
            int maxY = GraphicsDevice.Viewport.Height - ball.Height;

            // Check for bounce. Make sure to place ball back inside the screen
            // or it could remain outside the screen on the next iteration and cause
            // a back-and-forth bouncing logic error.
            if (ball.X > maxX)
            {
                crashSound.Play();
                ball.Reset();
                enemy.Reset();

                playerScore++;
                // Reset timer and stop ball's Update() from executing
                delayTimer = 0;
                ball.Enabled = false;
            }
            else if (ball.X < 0)
            {
                crashSound.Play();
                ball.Reset();
                enemy.Reset();

                enemyScore++;
                // Reset timer and stop ball's Update() from executing
                delayTimer = 0;
                ball.Enabled = false;
            }

            if (ball.Y < 0)
            {
                ball.ChangeVertDirection();
                ball.Y = 0;
            }
            else if (ball.Y > maxY)
            {
                ball.ChangeVertDirection();
                ball.Y = maxY;
            }

            // Collision?  Check rectangle intersection between ball and hand
            if (ball.Boundary.Intersects(paddle.Boundary))
            {
                swishSound.Play();

                // If hitting the side of the paddle the ball is coming toward, 
                // switch the ball's horz direction
                float ballMiddle = (ball.X + ball.Width) / 2;
                float paddleMiddle = (paddle.X + paddle.Width) / 2;
                if ((ballMiddle < paddleMiddle && ball.SpeedX > 0) ||
                    (ballMiddle > paddleMiddle && ball.SpeedX < 0))
                {
                    ball.ChangeVertDirection();
                }

                // Go back up the screen and speed up
                ball.ChangeHorzDirection();
                ball.ChangeVertDirection();
                ball.SpeedUp();
            }

            if (ball.Boundary.Intersects(enemy.Boundary))
            {
                swishSound.Play();

                // If hitting the side of the paddle the ball is coming toward, 
                // switch the ball's horz direction
                float ballMiddle = (ball.X + ball.Width) / 2;
                float paddleMiddle = (paddle.X + paddle.Width) / 2;
                if ((ballMiddle > paddleMiddle && ball.SpeedX > 0) ||
                    (ballMiddle < paddleMiddle && ball.SpeedX < 0))
                {
                    ball.ChangeVertDirection();
                }

                // Go back up the screen and speed up
                ball.ChangeHorzDirection();
                ball.ChangeVertDirection();
                ball.SpeedUp();
            }


            enemy.Move(ball, gameTime);
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            spriteBatch.Draw(background, mainFrame, Color.White);
            ball.Draw(gameTime);
            DrawText();
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawText()
        {
            spriteBatch.DrawString(font, playerScore.ToString(), playerScorePos, Color.White);
            spriteBatch.DrawString(font, enemyScore.ToString(), enemyScorePos, Color.White);
        }
    }
}
