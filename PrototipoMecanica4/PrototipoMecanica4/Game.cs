using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace PrototipoMecanica4
{
    public class World : Game
    {
        GraphicsDeviceManager graphics;

        //Sprites - Fonts
        public static SpriteFont fontNormal;

        //Sprites - Characters
        public static Texture2D playerTexture;
        public static Texture2D enemy001Texture;

        //Sprites - Projectiles
        public static Texture2D fireTexture;

        //Sprites - Stages
        public static Texture2D stageTexture;

        //Sprites - Debugs
        public static Texture2D debugCircleTex;
        public static Texture2D debugRectangleTex;

        //Sprite Batches
        public static SpriteBatch spriteBatch;

        //Keyboard state
        public static KeyboardState prevKeyState = Keyboard.GetState();

        //Debug mode
        public static bool debugMode = false;

        //World entities
        public static List<Entity> entities = new List<Entity>();
		
		//Machine states
		public enum GameState { Null, Menu, Stage };
		
		//Current State
		public static GameState currentState = GameState.Null;

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
            //Create new sprite batch
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load fonts
            fontNormal = Content.Load<SpriteFont>("Fonts/Normal");

            //Load sprites - Characters
            playerTexture = Content.Load<Texture2D>("Sprites/Player");
            enemy001Texture = Content.Load<Texture2D>("Sprites/Enemy001");

            //Load sprites - Objects
            fireTexture = Content.Load<Texture2D>("Sprites/Fire");

            //Load sprites - Stages
            stageTexture = Content.Load<Texture2D>("Sprites/Stage");

            //Load sprites - Debugs
            debugCircleTex = Content.Load<Texture2D>("Sprites/debug_circle");
            debugRectangleTex = Content.Load<Texture2D>("Sprites/debug_rectangle");

            //Adding entities
            entities.Add(new Human(new Vector2(260, 768), new Vector2(88, 128)));
            entities.Add(new Enemy(new Vector2(642, 768), new Vector2(108, 160)));

            //Enter in initial state
            GameStates.EnterGameState(GameState.Stage);
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
            //Clear screen
            GraphicsDevice.Clear(Color.Black);

            //Begin draws
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);

            //Draw stage
            spriteBatch.Draw(stageTexture, new Vector2(0f, 0f), null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.4f);

            //Draw texts
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

            //Draw each entity
            foreach (Entity e in entities)
                e.Draw(gameTime);

            //End draws
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}