using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace PrototipoMecanica4
{
    public class World : Game
    {
        GraphicsDeviceManager graphics;

        #region VARIABLES

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

        public static Texture2D enemy001MovingTexture;
        public static Texture2D enemy001PrepareAttackTexture;

        public static Texture2D enemy001KickTexture;
        public static Texture2D enemy001LowStaffAttackTexture;
        public static Texture2D enemy001StaffAttackTexture;

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
        public static Texture2D menuTexture;
        public static Texture2D stageTexture;

        //Sprites messages
        public static Texture2D pauseTexture;

        //Sprites - Debugs
        public static Texture2D debugCircleTex;
        public static Texture2D debugRectangleTex;
        public static Texture2D debugBigRectangleTex;
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

        //Sound Library
        SoundEffect sound001;
        SoundEffect sound002;
        SoundEffect sound003;
        SoundEffect sound004;
        SoundEffect sound005;
        SoundEffect sound006;
        SoundEffect sound007;
        SoundEffect sound008;
        SoundEffect sound009;
        SoundEffect sound010;
        SoundEffect sound011;
        SoundEffect sound012;
        SoundEffect sound013;
        SoundEffect sound014;
        SoundEffect sound015;
        SoundEffect sound016;
        SoundEffect sound017;
        SoundEffect sound018;
        SoundEffect sound019;
        SoundEffect sound020;
        SoundEffect sound021;
        SoundEffect sound022;
        SoundEffect sound023;
        SoundEffect sound024;
        SoundEffect sound025;
        SoundEffect sound026;
        SoundEffect sound027;
        SoundEffect sound028;
        SoundEffect sound029;
        SoundEffect sound030;

        #endregion

        #region METHODS

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

            #region TEXTURE AND TEXT LOADS

            //Load fonts
            fontNormal = Content.Load<SpriteFont>("Fonts/Normal");

            //Load sprites - Characters
            playerTexture = Content.Load<Texture2D>("Sprites/Player");
            playerMovingTexture = Content.Load<Texture2D>("Sprites/PlayerMoving");
            playerJumpingTexture = Content.Load<Texture2D>("Sprites/PlayerJumping");

            playerLowPunchTexture = Content.Load<Texture2D>("Sprites/PlayerLPunch");
            playerPunchTexture = Content.Load<Texture2D>("Sprites/PlayerPunch");

            playerLowKickTexture = Content.Load<Texture2D>("Sprites/PlayerLKick");
            playerMediumKickTexture = Content.Load<Texture2D>("Sprites/PlayerMKick");
            playerHighKickTexture = Content.Load<Texture2D>("Sprites/PlayerHKick");
            playerFlyingKickTexture = Content.Load<Texture2D>("Sprites/PlayerFKick");

            enemy001Texture = Content.Load<Texture2D>("Sprites/Enemy001");

            enemy001MovingTexture = Content.Load<Texture2D>("Sprites/Enemy001Moving");
            enemy001PrepareAttackTexture = Content.Load<Texture2D>("Sprites/Enemy001PrepareAttack");

            enemy001KickTexture = Content.Load<Texture2D>("Sprites/Enemy001Kick");
            enemy001LowStaffAttackTexture = Content.Load<Texture2D>("Sprites/Enemy001LStaffAttack");
            enemy001StaffAttackTexture = Content.Load<Texture2D>("Sprites/Enemy001StaffAttack");

            enemy001DeadTexture = Content.Load<Texture2D>("Sprites/Enemy001Dead");

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
            menuTexture = Content.Load<Texture2D>("Sprites/Menu");
            stageTexture = Content.Load<Texture2D>("Sprites/Stage");

            //Sprites messages
            pauseTexture = Content.Load<Texture2D>("Sprites/Pause");

            //Load sprites - Debugs
            debugCircleTex = Content.Load<Texture2D>("Sprites/debug_circle");
            debugRectangleTex = Content.Load<Texture2D>("Sprites/debug_rectangle");
            debugBigRectangleTex = Content.Load<Texture2D>("Sprites/debug_big_rectangle");
            debugHitTex = Content.Load<Texture2D>("Sprites/debug_hitbox");

            #endregion

            #region LOAD SOUNDS

            LoadSounds();

            #endregion

            //Adding entities
            entities.Add(new Human(new Vector2(260, 768), new Vector2(88, 128)));
            entities.Add(new Enemy(new Vector2(642, 768), new Vector2(108, 160)));

            entities.Add(new Lifebar());

            //Enter in initial state
            EnterGameState(GameState.Menu);
        }

        private void LoadSounds()
        {
            //sound001 = Content.Load<SoundEffect>("Sounds/");
            //sound002 = Content.Load<SoundEffect>("Sounds/");
            //sound003 = Content.Load<SoundEffect>("Sounds/");
            //sound004 = Content.Load<SoundEffect>("Sounds/");
            //sound005 = Content.Load<SoundEffect>("Sounds/");
            //sound006 = Content.Load<SoundEffect>("Sounds/");
            //sound007 = Content.Load<SoundEffect>("Sounds/");
            //sound008 = Content.Load<SoundEffect>("Sounds/");
            //sound009 = Content.Load<SoundEffect>("Sounds/");
            //sound010 = Content.Load<SoundEffect>("Sounds/");
            //sound011 = Content.Load<SoundEffect>("Sounds/");
            //sound012 = Content.Load<SoundEffect>("Sounds/");
            //sound013 = Content.Load<SoundEffect>("Sounds/");
            //sound014 = Content.Load<SoundEffect>("Sounds/");
            //sound015 = Content.Load<SoundEffect>("Sounds/");
            //sound016 = Content.Load<SoundEffect>("Sounds/");
            //sound017 = Content.Load<SoundEffect>("Sounds/");
            //sound018 = Content.Load<SoundEffect>("Sounds/");
            //sound019 = Content.Load<SoundEffect>("Sounds/");
            //sound020 = Content.Load<SoundEffect>("Sounds/");
            //sound021 = Content.Load<SoundEffect>("Sounds/");
            //sound022 = Content.Load<SoundEffect>("Sounds/");
            //sound023 = Content.Load<SoundEffect>("Sounds/");
            //sound024 = Content.Load<SoundEffect>("Sounds/");
            //sound025 = Content.Load<SoundEffect>("Sounds/");
            //sound026 = Content.Load<SoundEffect>("Sounds/");
            //sound027 = Content.Load<SoundEffect>("Sounds/");
            //sound028 = Content.Load<SoundEffect>("Sounds/");
            //sound029 = Content.Load<SoundEffect>("Sounds/");
            //sound030 = Content.Load<SoundEffect>("Sounds/");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Game logic
            UpdateGameState(gameTime);

            prevKeyState = Keyboard.GetState();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //Clear screen
            GraphicsDevice.Clear(Color.Black);

            //Begin draws
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);

            //Draw game
            DrawGameState(gameTime);

            //End draws
            spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion

        #region FSM

        float countdownToMenu = 6;

        public void EnterGameState(GameState newState)
        {
            LeaveGameState();

            currentState = newState;

            switch (currentState)
            {
                case GameState.Menu:
                    { }
                    break;

                case GameState.Stage:
                    { }
                    break;

                case GameState.Pause:
                    { }
                    break;

                case GameState.Over:
                    { }
                    break;
            }
        }

        public void UpdateGameState(GameTime gameTime)
        {
            //Timer
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (currentState)
            {
                case GameState.Menu:
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter) && prevKeyState.IsKeyUp(Keys.Enter))
                            EnterGameState(GameState.Stage);
                    }
                    break;

                case GameState.Stage:
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter) && prevKeyState.IsKeyUp(Keys.Enter))
                            EnterGameState(GameState.Pause);

                        logicStage(gameTime);
                    }
                    break;

                case GameState.Pause:
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter) && prevKeyState.IsKeyUp(Keys.Enter))
                            EnterGameState(GameState.Stage);

                        logicPause(gameTime);
                    }
                    break;

                case GameState.Over:
                    {
                        if (countdownToMenu > 0)
                        {
                            countdownToMenu -= deltaTime;
                        }
                        else
                        {
                            countdownToMenu = 6;
                            EnterGameState(GameState.Menu);
                        }
                    }
                    break;
            }
        }

        public void DrawGameState(GameTime gameTime)
        {
            //Timer
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (currentState)
            {
                case GameState.Menu:
                    {
                        drawMenu(gameTime);
                    }
                    break;

                case GameState.Stage:
                    {
                        drawStage(gameTime);
                    }
                    break;

                case GameState.Pause:
                    {
                        drawPause(gameTime);
                    }
                    break;

                case GameState.Over:
                    { }
                    break;
            }
        }

        public void LeaveGameState()
        {
            switch (currentState)
            {
                case GameState.Menu:
                    { }
                    break;

                case GameState.Stage:
                    { }
                    break;

                case GameState.Pause:
                    { }
                    break;

                case GameState.Over:
                    { }
                    break;
            }
        }

        #endregion

        #region LOGIC STATES

        private void logicMenu(GameTime gameTime)
        {
        }

        private void logicStage(GameTime gameTime)
        {
            List<Entity> tmp = new List<Entity>(entities);
            foreach (Entity e in tmp)
                e.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.F1) && !prevKeyState.IsKeyDown(Keys.F1))
                debugMode = !debugMode;

            if (debugMode)
                if (Keyboard.GetState().IsKeyDown(Keys.F5))
                    Lifebar.instance.reviveEnemy();
        }

        private void logicPause(GameTime gameTime)
        {

        }

        private void logicOver(GameTime gameTime)
        {
        }

        #endregion

        #region DRAW STATES

        private void drawMenu(GameTime gameTime)
        {
            //Draw menu
            spriteBatch.Draw(menuTexture, new Vector2(0f, 0f), null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.4f);
        }

        private void drawStage(GameTime gameTime)
        {
            //Draw stage
            spriteBatch.Draw(stageTexture, new Vector2(0f, 0f), null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.4f);

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
        }

        private void drawPause(GameTime gameTime)
        {
            spriteBatch.Draw(pauseTexture, new Vector2((graphics.PreferredBackBufferWidth - pauseTexture.Width) / 2, (graphics.PreferredBackBufferHeight - pauseTexture.Height) / 2), null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.3f);

            drawStage(gameTime);
        }

        private void drawOver(GameTime gameTime)
        {

        }

        #endregion
    }
}