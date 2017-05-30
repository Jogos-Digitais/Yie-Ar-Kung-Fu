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

        public static Texture2D stageTexture;

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

            playerTex = Content.Load<Texture2D>("Sprites/Player");
            enemyTex = Content.Load<Texture2D>("Sprites/FirstEnemy");
            bulletTex = Content.Load<Texture2D>("Sprites/Char35");

            stageTexture = Content.Load<Texture2D>("Sprites/Stage");

            debugCircleTex = Content.Load<Texture2D>("Sprites/debug_circle");

            fontNormal = Content.Load<SpriteFont>("Fonts/Normal");

            entities.Add(new Human(new Vector2(320, 100), new Vector2(88, 128)));

            entities.Add(new Enemy(new Vector2(220, 300), new Vector2(108, 160)));
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

            spriteBatch.Draw(stageTexture, new Vector2(0f, 0f), Color.White);

            spriteBatch.DrawString(
              fontNormal,
              "F1 - Debug Mode (" + (debugMode ? "DEBUG ON" : "DEBUG OFF") + ")\n" +
			  "Control - Shoot!",
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