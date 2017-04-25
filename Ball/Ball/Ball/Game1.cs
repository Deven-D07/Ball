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
using System.Media;

namespace Ball
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D samus, spritesheet, titleScreen;
        int score;
        Sprite ball;
        Boolean clicked = false;
        SoundPlayer player;
        SoundPlayer ultra;
        SoundPlayer combo;
        SoundPlayer boing;
        SoundPlayer music;
        SpriteFont Font1;
        private float titleScreenTimer = 0f;
        private float titleScreenDelayTime = 1f;
        private float playerDeathDelayTime = 10f;
        private float playerDeathTimer = 0f;
        private Vector2 scoreLocation = new Vector2(20, 10);
        enum GameStates { TitleScreen, Playing, GameOver };
        GameStates gameState = GameStates.TitleScreen;
        MouseState ms2 = Mouse.GetState();

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
            titleScreen = Content.Load<Texture2D>("metroid2");
            score = 0;
            ball = new Sprite(new Vector2(380, 0), spritesheet, new Rectangle(0,0 , 70, 70), new Vector2(0, 80));

            player = new SoundPlayer("C:\\Users\\eahscs\\Desktop\\Ball\\Ball\\Ball\\BallContent\\spin_jump-Brandino480-2020916281.wav");
            boing = new SoundPlayer("C:\\Users\\eahscs\\Desktop\\Ball\\Ball\\Ball\\BallContent\\boing2_1.wav");
            ultra = new SoundPlayer("C:\\Users\\eahscs\\Desktop\\Ball\\Ball\\Ball\\BallContent\\ultra.wav");
            music = new SoundPlayer("C:\\Users\\eahscs\\Desktop\\Ball\\Ball\\Ball\\BallContent\\Killer_Instinct_XboxOne_Sabrewulf_Theme_Full_Versi.wav");
            combo = new SoundPlayer("C:\\Users\\eahscs\\Desktop\\Ball\\Ball\\Ball\\BallContent\\combobreaker.wav");
            Font1 = Content.Load<SpriteFont>(@"Pericles14");
            music.PlayLooping();
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

            switch (gameState)
            {
                case GameStates.TitleScreen:
                    titleScreenTimer +=
                        (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (titleScreenTimer >= titleScreenDelayTime)
                    {
                        if ((Keyboard.GetState().IsKeyDown(Keys.Space)) ||
                            (GamePad.GetState(PlayerIndex.One).Buttons.A ==
                            ButtonState.Pressed))
                        {

                            gameState = GameStates.Playing;
                        }
                    }
                    break;

                case GameStates.Playing:

                    ball.Velocity += new Vector2(0, 30);

                    if (ball.Location.Y > this.Window.ClientBounds.Height - ball.BoundingBoxRect.Height)
                    {
                        ball.Velocity *= new Vector2(1, -1);
                        boing.Play();
                    }

                   /* if (ball.Location.X > this.Window.ClientBounds.Width - ball.BoundingBoxRect.Width)
                    {
                        ball.Velocity *= new Vector2(-1, 1);
                        boing.Play();
                    } */



                    MouseState ms = Mouse.GetState();
                    Vector2 clickPoint = new Vector2(ms.X, ms.Y);

                    if (ms.LeftButton == ButtonState.Pressed && !clicked && Vector2.Distance(clickPoint, ball.Center) < ball.BoundingBoxRect.Width / 2)
                    {
                        //ball.Velocity *= new Vector2(1, 0);
                        //ball.Velocity += new Vector2(0, -800);
                        float cSpeed = ball.Velocity.Length();

                        Vector2 dir = ball.Center - clickPoint;
                        dir.Normalize();
                        dir.Y = Math.Abs(dir.Y) * -900;
                        dir.X *= 200;
                        player.Play();
                        ball.Velocity = dir;
                        clicked = true;
                        score++;
                        if (score >= 5)
                        {
                            ultra.Play();
                        }
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


                    break;

                case GameStates.GameOver:
                    ball.Location = new Vector2(380, 0);
                    ball.Velocity = new Vector2(0, 30);
                    playerDeathTimer +=
                        (float)gameTime.ElapsedGameTime.TotalSeconds;
                    
                    if (playerDeathTimer >= playerDeathDelayTime)
                    {
                        gameState = GameStates.TitleScreen;
                    }
                    break;
            }


            
                    base.Update(gameTime);
            
        }
        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            if (gameState == GameStates.TitleScreen)
            {
                spriteBatch.Draw(titleScreen,
                    new Rectangle(0, 0, this.Window.ClientBounds.Width,
                        this.Window.ClientBounds.Height),
                        Color.White);
            }

            if (gameState == GameStates.Playing) {
               
                spriteBatch.Draw(samus, Vector2.Zero, Color.White);
                ball.Draw(spriteBatch);
                spriteBatch.DrawString(
                  Font1,
                  "Combo: " + score.ToString(),
                  scoreLocation,
                  Color.Cyan);


                if (ball.Location.X > this.Window.ClientBounds.Width - ball.BoundingBoxRect.Width || ball.Location.Y > this.Window.ClientBounds.Height - ball.BoundingBoxRect.Height) {
                    gameState = GameStates.GameOver;
                    combo.Play();
                }
            }

            if (gameState == GameStates.GameOver) {
                
                spriteBatch.DrawString(
                    Font1,
                    "Combo: " + score.ToString(),
                    scoreLocation,
                    Color.White);

                spriteBatch.DrawString(
                    Font1,
                    "CCCCCCOMBO BREAKER!!!!",
                    new Vector2(
                        this.Window.ClientBounds.Width / 2 -
                         Font1.MeasureString("CCCCCCOMBO BREAKER!!!!").X / 2,
                        50),
                    Color.Cyan);
                if(Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    gameState = GameStates.TitleScreen;
                    score = 0;
                }
            }
            
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
