using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace PrototipoMecanica3
{
    public class World : Game
    {
        GraphicsDeviceManager graphics;

        //Sprites - Fonts
        public static SpriteFont fontNormal;

        //Sprites - Characters
        public static Texture2D playerTexture;
        public static Texture2D playerMovingTexture;
        public static Texture2D playerJumpingTexture;

        public static Texture2D playerLowPunchTexture;
        public static Texture2D playerPunchTexture;

        public static Texture2D playerLowKickTexture;
        public static Texture2D playerMediumKickTexture;
        public static Texture2D playerHighKickTexture;
        public static Texture2D playerFlyingKickTexture;

        public static Texture2D enemy001Texture;
        public static Texture2D enemy001DeadTexture;

        //Sprites - Objects
        public static Texture2D fireTexture;
        public static Texture2D fireMovingTexture;

        public static Texture2D lifeBarTexture;
        public static Texture2D lifeFragment;
        public static Texture2D extraLifeTexture;

        //Sprites - hitboxes
        public static Texture2D playerHitTexture;
        public static Texture2D enemyHitTexture;
        public static Texture2D fireHitTexture;

        //Sprites - Stages
        public static Texture2D stageTexture;

        //Sprites messages
        public static Texture2D pauseTexture;

        //Sprites - Debugs
        public static Texture2D debugCircleTex;
        public static Texture2D debugRectangleTex;
        public static Texture2D debugHitTex;

        //Sprite Batches
        public static SpriteBatch spriteBatch;

        //Keyboard state
        public static KeyboardState prevKeyState = Keyboard.GetState();

        //Debug mode
        public static bool debugMode = false;

        //World entities
        public static List<Entity> entities = new List<Entity>();
		
		//Machine states
		public enum GameState { Null, Menu, Stage, Pause, Over };
		
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
            playerTexture           = Content.Load<Texture2D>("Sprites/Player");
            playerMovingTexture     = Content.Load<Texture2D>("Sprites/PlayerMoving");
            playerJumpingTexture    = Content.Load<Texture2D>("Sprites/PlayerJumping");

            playerLowPunchTexture   = Content.Load<Texture2D>("Sprites/PlayerLPunch");
            playerPunchTexture      = Content.Load<Texture2D>("Sprites/PlayerPunch");

            playerLowKickTexture    = Content.Load<Texture2D>("Sprites/PlayerLKick");
            playerMediumKickTexture = Content.Load<Texture2D>("Sprites/PlayerMKick");
            playerHighKickTexture   = Content.Load<Texture2D>("Sprites/PlayerHKick");
            playerFlyingKickTexture = Content.Load<Texture2D>("Sprites/PlayerFKick");
            
            enemy001Texture         = Content.Load<Texture2D>("Sprites/Enemy001");
            enemy001DeadTexture     = Content.Load<Texture2D>("Sprites/Enemy001Dead");

            //Load sprites - Objects
            fireTexture = Content.Load<Texture2D>("Sprites/Fire");
            fireMovingTexture = Content.Load<Texture2D>("Sprites/FireMoving");

            lifeBarTexture = Content.Load<Texture2D>("Sprites/LifeBar");
            lifeFragment = Content.Load<Texture2D>("Sprites/LifeFragment");
            extraLifeTexture = Content.Load<Texture2D>("Sprites/extraLife");

            //Load sprites - Hitboxes
            playerHitTexture = Content.Load<Texture2D>("Sprites/playerHit");
            enemyHitTexture = Content.Load<Texture2D>("Sprites/enemyHit");
            fireHitTexture = Content.Load<Texture2D>("Sprites/fireHit");

            //Load sprites - Stages
            stageTexture = Content.Load<Texture2D>("Sprites/Stage");

            //Sprites messages
            pauseTexture = Content.Load<Texture2D>("Sprites/Pause");

            //Load sprites - Debugs
            debugCircleTex = Content.Load<Texture2D>("Sprites/debug_circle");
            debugRectangleTex = Content.Load<Texture2D>("Sprites/debug_rectangle");
            debugHitTex = Content.Load<Texture2D>("Sprites/debug_hitbox");

            //Adding entities
            entities.Add(new Human(new Vector2(260, 768), new Vector2(88, 128)));
            entities.Add(new Enemy(new Vector2(642, 768), new Vector2(108, 160)));

            entities.Add(new Lifebar());

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

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                GameStates.EnterGameState(GameState.Pause);

            List<Entity> tmp = new List<Entity>(entities);
            foreach (Entity e in tmp)
                e.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.F1) && !prevKeyState.IsKeyDown(Keys.F1))
                debugMode = !debugMode;

            if (debugMode)
                if (Keyboard.GetState().IsKeyDown(Keys.F5))
                    Lifebar.instance.reviveEnemy();

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

            //Draw game paused
            if (currentState.Equals(GameState.Pause))
                spriteBatch.Draw(pauseTexture, new Vector2((graphics.PreferredBackBufferWidth - pauseTexture.Width) / 2, (graphics.PreferredBackBufferHeight - pauseTexture.Height) / 2), null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.3f);

            //Draw texts
            spriteBatch.DrawString(
              fontNormal,
              "F1 - Debug Mode (" + (debugMode ? "DEBUG ON" : "DEBUG OFF") + ")\n" +
              "Z - Kick | X - Punch | F5 - In debug mode, revive enemy",
              new Vector2(65, 151),  //position
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