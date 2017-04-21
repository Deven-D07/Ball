using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Ball
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D samus, spritesheet;
        int score;
        Sprite ball;
        Boolean clicked = false;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            samus = Content.Load<Texture2D>("samus");
            spritesheet = Content.Load<Texture2D>("MorphBall");
            score = 0;
            ball = new Sprite(new Vector2(380, 0), spritesheet, new Rectangle(0,0 , 70, 70), new Vector2(0, 80));

           
            // TODO: use this.Content to load your game content here
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            ball.Velocity += new Vector2(0, 30);

            if (ball.Location.Y > this.Window.ClientBounds.Height - ball.BoundingBoxRect.Height)
            {
                ball.Velocity *= new Vector2(1, -1);
            }


            MouseState ms = Mouse.GetState();
            Vector2 clickPoint = new Vector2(ms.X, ms.Y);

            if (ms.LeftButton == ButtonState.Pressed && !clicked && Vector2.Distance(clickPoint, ball.Center) < ball.BoundingBoxRect.Width/2)
            {
                //ball.Velocity *= new Vector2(1, 0);
                //ball.Velocity += new Vector2(0, -800);
                float cSpeed = ball.Velocity.Length();

                Vector2 dir = ball.Center - clickPoint;
                dir.Normalize();
                dir.Y = Math.Abs(dir.Y) * -900;
                dir.X *= 200;

                ball.Velocity = dir;
                clicked = true;
            }
            else if (ms.LeftButton == ButtonState.Released)
            {
                clicked = false;
            }

            // Set a maximum speed so we stay on screen
            float maxSpeed = 600;

            Vector2 vel = ball.Velocity;
            float speed = vel.Length();
            if (speed > 1000) speed = 1000;
            vel.Normalize();
            vel *= speed;
            ball.Velocity = vel;

            ball.Update(gameTime);


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            spriteBatch.Draw(samus, Vector2.Zero, Color.White);
            ball.Draw(spriteBatch);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
