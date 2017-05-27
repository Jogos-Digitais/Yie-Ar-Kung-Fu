using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace PrototipoMecanica1
{
    public class World : Game
    {
        GraphicsDeviceManager graphics;

        public static Texture2D playerTex;
        public static Texture2D enemyTex;
        public static Texture2D bulletTex;
        public static Texture2D debugCircleTex;

        public static SpriteFont fontNormal;

        public static SpriteBatch spriteBatch;

        public static List<Entity> entities = new List<Entity>();

        public static KeyboardState prevKeyState = Keyboard.GetState();

        public static bool debugMode = false;

        public World()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 960;

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            playerTex = Content.Load<Texture2D>("Sprites/Char19");
            enemyTex = Content.Load<Texture2D>("Sprites/Char02");
            bulletTex = Content.Load<Texture2D>("Sprites/Char35");

            debugCircleTex = Content.Load<Texture2D>("Sprites/debug_circle");

            fontNormal = Content.Load<SpriteFont>("Fonts/Normal");

            entities.Add(new Human(new Vector2(320, 100)));

            entities.Add(new Enemy(new Vector2(220, 300)));
            entities.Add(new Enemy(new Vector2(420, 300)));
            entities.Add(new Enemy(new Vector2(220, 200)));
            entities.Add(new Enemy(new Vector2(420, 200)));
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            List<Entity> tmp = new List<Entity>(entities);
            foreach (Entity e in tmp)
                e.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.F1) && !prevKeyState.IsKeyDown(Keys.F1))
                debugMode = !debugMode;

            prevKeyState = Keyboard.GetState();

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.BackToFront,
                        BlendState.NonPremultiplied);

            spriteBatch.DrawString(
              fontNormal,
              debugMode ? "DEBUG ON" : "DEBUG OFF",
              new Vector2(10, 10),  //position
              Color.White,          //color
              0.0f,                 //rotation
              Vector2.Zero,         //origin (pivot)
              Vector2.One,          //scale
              SpriteEffects.None,
              0.0f
            );

            foreach (Entity e in entities)
                e.Draw(gameTime);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
