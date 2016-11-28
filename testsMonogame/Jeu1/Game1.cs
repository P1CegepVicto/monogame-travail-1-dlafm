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
        GameObject[] ennemi;
        GameObject[] missile1;
        GameObject missile0;
        GameObject explosion;
        int nombreEnnemis = 0;
        int nombreMissile1 = 0;
        float temps;
        float tempsFinal;
        public int maxEnnemis = 5;
        public int maxMissile1 = 5;
        Vector2 max;

        Texture2D background;
        SpriteFont font;

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
            //this.graphics.ApplyChanges();
            //this.Window.Position = new Point(0, 0);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Font");
            background = Content.Load<Texture2D>("wut.png");

            fenetre = graphics.GraphicsDevice.Viewport.Bounds;
            fenetre.Width = graphics.GraphicsDevice.Viewport.Width;
            fenetre.Height = graphics.GraphicsDevice.Viewport.Height;

            heros = new GameObject();
            heros.estVivant = true;
            heros.vitesse = 15;
            heros.sprite = Content.Load<Texture2D>("robot.png");
            heros.position = heros.sprite.Bounds;
            heros.position.Offset((fenetre.Width / 4), (fenetre.Height / 4));

            this.ennemi = new GameObject[maxEnnemis];
            this.missile1 = new GameObject[maxMissile1];


            for (int i = 0; i < ennemi.Length; i++)
            {
                ennemi[i] = new GameObject();
                ennemi[i].estVivant = false;
                ennemi[i].sprite = Content.Load<Texture2D>("ufo.png");
                ennemi[i].vitesse = 12;
                ennemi[i].position = ennemi[i].sprite.Bounds;
                ennemi[i].position.X = fenetre.Width;
                ennemi[i].position.Y = fenetre.Height;
                ennemi[i].direction.X = rand.Next(-10, 10);
                ennemi[i].direction.Y = rand.Next(-10, 10);
            }

            for (int i = 0; i < missile1.Length; i++)
            {
                missile1[i] = new GameObject();
                missile1[i].estVivant = false;
                missile1[i].vitesse = 20;
                missile1[i].position = ennemi[i].position;
                missile1[i].sprite = Content.Load<Texture2D>("1.png");
                missile1[i].position = missile1[i].sprite.Bounds;
            }

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

            temps += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            UpdateEnnemis(gameTime);
            UpdateHeros();
            UpdateMissile1(gameTime);
            UpdateMissile0();
            base.Update(gameTime);
        }

        public void UpdateHeros()
        {
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
        public void UpdateMissile1(GameTime gameTime)
        {
            for (int i = 0; i < nombreEnnemis; i++)
            {
                if (nombreMissile1 < gameTime.TotalGameTime.Seconds && nombreMissile1 < maxMissile1)
                {
                    missile1[i].estVivant = true;
                    nombreMissile1++;
                }

                if (heros.position.Intersects(missile1[i].position))
                {
                    tempsFinal = temps;
                    heros.estVivant = false;
                }

                missile1[i].position.Y += (int)missile1[i].vitesse;

                if (missile1[i].estVivant == false)
                {
                    missile1[i].estVivant = true;
                    missile1[i].position = ennemi[i].position;
                }

                if (ennemi[i].position.Y > max.X || ennemi[i].position.Y < fenetre.Top)
                {
                    missile1[i].estVivant = false;
                }

            }
        }
        public void UpdateEnnemis(GameTime gameTime)
        {
            if (nombreEnnemis < gameTime.TotalGameTime.Seconds && nombreEnnemis < maxEnnemis)
            {
                ennemi[nombreEnnemis].estVivant = true;
                ennemi[nombreEnnemis].position.X = 0;
                ennemi[nombreEnnemis].position.Y = 0;
                nombreEnnemis++;
            }
            for (int i = 0; i < nombreEnnemis; i++)
            {
                if (heros.position.Intersects(ennemi[i].position))
                {
                    tempsFinal = temps;
                    heros.estVivant = false;
                }

                ennemi[i].position.X += (int)ennemi[i].direction.X;
                ennemi[i].position.Y += (int)ennemi[i].direction.Y;

                if (ennemi[i].estVivant == false)
                {
                    ennemi[i].estVivant = true;

                    ennemi[i].position.X = fenetre.Width / 2;
                    ennemi[i].position.Y = fenetre.Height / 2;
                }

                if (ennemi[i].position.Intersects(missile0.position))
                {
                    ennemi[i].estVivant = false;
                }

                max.X = fenetre.Width - ennemi[i].sprite.Width;
                max.Y = fenetre.Height - ennemi[i].sprite.Height;

                if (ennemi[i].position.X > max.X || ennemi[i].position.X < fenetre.Left)
                {
                    ennemi[i].direction.X = -(ennemi[i].direction.X);
                }
                if (ennemi[i].position.Y > max.X || ennemi[i].position.Y < fenetre.Top)
                {
                    ennemi[i].direction.Y = -(ennemi[i].direction.Y);
                }

            }

        }
        public void UpdateMissile0()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                if (heros.estVivant == true)
                {
                    missile0.position = heros.position;
                    if (missile0.position.Y < fenetre.Top)
                    {
                        missile0.position.Y = heros.position.Y + heros.sprite.Height;
                        missile0.position.X = heros.position.X;
                    }
                }
                else
                {
                    missile0.position.X = -700;
                    missile0.position.Y = 0;
                }

            }
            missile0.position.Y -= missile0.vitesse;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkViolet);

            spriteBatch.Begin();
            spriteBatch.Draw(background, fenetre, Color.White);

            if (missile0.estVivant == true)
            {
                spriteBatch.Draw(missile0.sprite, missile0.position, Color.White);
            }

            for (int i = 0; i < maxEnnemis; i++)
            {
                {
                    if (missile1[i].estVivant == true)
                    {
                        spriteBatch.Draw(missile1[i].sprite, missile1[i].position, Color.White);
                    }
                }
            }

            if (heros.estVivant == true)
            {
                spriteBatch.Draw(heros.sprite, heros.position, Color.White);
            }
            else
            {
                spriteBatch.Draw(explosion.sprite, heros.position, Color.White);

                spriteBatch.DrawString(font, "Time: " + Convert.ToInt16(tempsFinal).ToString(), new Vector2(100, 100), Color.Black);

                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    Exit();
                }
            }

            for (int i = 0; i < ennemi.Length; i++)
            {
                if (ennemi[i].estVivant)
                {
                    spriteBatch.Draw(ennemi[i].sprite, ennemi[i].position, Color.White);
                }
            }




            spriteBatch.End();
            base.Draw(gameTime);

        }
    }
}
