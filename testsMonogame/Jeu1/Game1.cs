using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Jeu1
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Rectangle fenetre;
        GameObject heros;
        GameObject missile1;
        GameObject missile0;
        GameObject explosion;
        GameObject ennemi;
        Texture2D background;
        Random rand = new Random();
        public object Break { get; private set; }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width;
            this.graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;
            this.graphics.ToggleFullScreen();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            background = Content.Load<Texture2D>("wut.png");
 
            fenetre = graphics.GraphicsDevice.Viewport.Bounds;
            fenetre.Width = graphics.GraphicsDevice.DisplayMode.Width;
            fenetre.Height = graphics.GraphicsDevice.DisplayMode.Height;

            heros = new GameObject();
            heros.estVivant = true;
            heros.vitesse = 10;
            heros.sprite = Content.Load<Texture2D>("robot.png");
            heros.position = heros.sprite.Bounds;
            heros.position.Offset((fenetre.Width / 2), (fenetre.Height / 2));

            ennemi = new GameObject();
            ennemi.estVivant = true;
            ennemi.sprite = Content.Load<Texture2D>("ufo.png");
            ennemi.vitesse = 10;
            ennemi.position = ennemi.sprite.Bounds;

            missile1 = new GameObject();
            missile1.estVivant = true;
            missile1.vitesse = 25;
            missile1.position.X = ennemi.position.X;
            missile1.position.Y = ennemi.position.Y;
            missile1.sprite = Content.Load<Texture2D>("1.png");
            missile1.position = missile1.sprite.Bounds;

            missile0 = new GameObject();
            missile0.estVivant = true;
            missile0.vitesse = 20;
            missile0.sprite = Content.Load<Texture2D>("0.png");
            missile0.position = missile0.sprite.Bounds;
            missile0.position.Offset(-64, 0);

            explosion = new GameObject();
            explosion.sprite = Content.Load<Texture2D>("explosion.png");

        }

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                heros.position.X += heros.vitesse;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                heros.position.X -= heros.vitesse;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                heros.position.Y += heros.vitesse;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                heros.position.Y -= heros.vitesse;
            }

            UpdateEnnemi();
            UpdateHeros();
            UpdateMissile1();
            UpdateMissile0();
            base.Update(gameTime);
        }

        public void UpdateHeros()
        {
            if (heros.position.Intersects(missile1.position))
            {
                heros.estVivant = false;
            }

            if (heros.position.X > fenetre.Width - heros.position.Width)
            {
                heros.position.X = fenetre.Width - heros.position.Width;
            }

            if (heros.position.Y > fenetre.Height - heros.position.Height)
            {
                heros.position.Y = fenetre.Height - heros.position.Height;
            }

            if (heros.position.X < 0)
            {
                heros.position.X = 0;
            }

            if (heros.position.Y < 0)
            {
                heros.position.Y = 0;
            }
        }
        public void UpdateMissile1()
        {
            if (missile1.estVivant == true)
            {
                missile1.estVivant = true;
                if (missile1.position.Y > fenetre.Bottom)
                {
                    missile1.position.X = ennemi.position.X;
                    missile1.position.Y = ennemi.position.Y + ennemi.sprite.Height;
                }
                missile1.position.Y += missile1.vitesse;
            }
        }
        public void UpdateEnnemi()
        {
            if (ennemi.estVivant == true)
            {
                if (ennemi.position.Intersects(missile0.position))
                {
                    ennemi.estVivant = false;
                }

                ennemi.position.X += (int)ennemi.vitesse;

                int maxX = fenetre.Width - (ennemi.sprite.Width);
                int maxY = fenetre.Height - (ennemi.sprite.Height);

                if (ennemi.position.X > maxX || ennemi.position.X < 0)
                {   
                    ennemi.vitesse = -(ennemi.vitesse);
                }
            }
            else
            {
                ennemi.position.X = -500;
                ennemi.position.Y = 0;
            }
        }
        public void UpdateMissile0()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                missile0.position = heros.position;
                if (missile0.position.Y < fenetre.Top)
                {
                    missile0.position.Y = heros.position.Y + heros.sprite.Height;
                    missile0.position.X = heros.position.X;
                }
            }
            missile0.position.Y -= missile0.vitesse;
        }
        
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkViolet);

            spriteBatch.Begin();
            spriteBatch.Draw(background, fenetre, Color.White);

            if (missile0.estVivant == true)
            {
                spriteBatch.Draw(missile0.sprite, missile0.position, Color.White);
            }

            if (missile1.estVivant == true)
            {
                spriteBatch.Draw(missile1.sprite, missile1.position, Color.White);
            }

            if (heros.estVivant == true)
            {
                spriteBatch.Draw(heros.sprite, heros.position, Color.White);
            }

            else
            {
                spriteBatch.Draw(explosion.sprite, heros.position, Color.White);
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    Exit();
                }
            }
            if (ennemi.estVivant == true)
            {
                spriteBatch.Draw(ennemi.sprite, ennemi.position, Color.White);
            }
           
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
