using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace YieArKungFu
{
    public class World : Game
    {
        GraphicsDeviceManager graphics;

        #region VARIABLES

        #region Sprites - Fonts

        public static SpriteFont fontNormal;

        #endregion

        #region Sprites - Characters

        public static Texture2D playerTexture;
        public static Texture2D playerMovingTexture;
        public static Texture2D playerJumpingTexture;

        public static Texture2D playerLowPunchTexture;
        public static Texture2D playerPunchTexture;

        public static Texture2D playerLowKickTexture;
        public static Texture2D playerMediumKickTexture;
        public static Texture2D playerHighKickTexture;
        public static Texture2D playerFlyingKickTexture;

        public static Texture2D playerDeadTexture;
        public static Texture2D playerDeadAnimationTexture;

        public static Texture2D enemy001Texture;

        public static Texture2D enemy001MovingTexture;
        public static Texture2D enemy001PrepareAttackTexture;

        public static Texture2D enemy001KickTexture;
        public static Texture2D enemy001LowStaffAttackTexture;
        public static Texture2D enemy001StaffAttackTexture;

        public static Texture2D enemy001DeadTexture;

        #endregion

        #region Sprites - Objects

        public static Texture2D selectedOptionTexture;

        public static Texture2D fireTexture;
        public static Texture2D fireMovingTexture;

        public static Texture2D lifeBarTexture;
        public static Texture2D lifeFragment;
        public static Texture2D extraLifeTexture;

        #endregion

        #region Sprites - Hitboxes

        public static Texture2D playerHitTexture;
        public static Texture2D enemyHitTexture;
        public static Texture2D fireHitTexture;

        #endregion

        #region Sprites - Stages

        public static Texture2D menuTexture;
        public static Texture2D stageLoadTexture;
        public static Texture2D stageTexture;
        public static Texture2D stage2Texture;

        #endregion

        #region Sprites - Messages

        public static Texture2D pauseTexture;
        public static Texture2D overTexture;
        public static Texture2D perfectTexture;

        public static Texture2D points0Texture;
        public static Texture2D points100Texture;
        public static Texture2D points200Texture;
        public static Texture2D points300Texture;

        public static Texture2D enemy001NameTexture;

        #endregion

        #region Sprites - Numbers

        public static Texture2D N0Texture;
        public static Texture2D N1Texture;
        public static Texture2D N2Texture;
        public static Texture2D N3Texture;
        public static Texture2D N4Texture;
        public static Texture2D N5Texture;
        public static Texture2D N6Texture;
        public static Texture2D N7Texture;
        public static Texture2D N8Texture;
        public static Texture2D N9Texture;

        #endregion

        #region Sprites - Debugs

        public static Texture2D debugCircleTex;
        public static Texture2D debugRectangleTex;
        public static Texture2D debugBigRectangleTex;
        public static Texture2D debugHitTex;
        public static Texture2D debugZones;

        #endregion

        #region Sound Library

        SoundEffect pauseSound;
        SoundEffect gameMusic;
        SoundEffect overSound;
        SoundEffect victorySound;
        SoundEffect lossSound;

        SoundEffect hitSound;
        SoundEffect lowLifeSound;
        SoundEffect wrongHitSound;



        //SoundEffect sound005;
        //SoundEffect sound006;
        //SoundEffect sound007;
        //SoundEffect sound008;
        //SoundEffect sound009;
        //SoundEffect sound010;
        //SoundEffect sound011;
        //SoundEffect sound012;
        //SoundEffect sound013;
        //SoundEffect sound014;
        //SoundEffect sound015;
        //SoundEffect sound016;
        //SoundEffect sound017;
        //SoundEffect sound018;
        //SoundEffect sound019;
        //SoundEffect sound020;
        //SoundEffect sound021;
        //SoundEffect sound022;
        //SoundEffect sound023;
        //SoundEffect sound024;
        //SoundEffect sound025;
        //SoundEffect sound026;
        //SoundEffect sound027;
        //SoundEffect sound028;
        //SoundEffect sound029;
        //SoundEffect sound030;

        SoundEffectInstance music;

        #endregion

        //Sprite Batches
        public static SpriteBatch spriteBatch;

        //Keyboard state
        public static KeyboardState prevKeyState = Keyboard.GetState();

        //Debug mode
        public static bool debugMode = false;

        //World entities
        public static List<Entity> menuEntities = new List<Entity>();
        public static List<Entity> entities = new List<Entity>();

        public static Entity stageSelector = new StageSelector();

        public static Entity scoreBoard = new ScoreBoard();

        //Machine states
        public enum GameState { Null, Menu, StageLoad, Stage, Pause, Over };

        //Current State
        public static GameState currentState = GameState.Null;

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

            #region TEXT LOAD

            //Load fonts
            fontNormal = Content.Load<SpriteFont>("Fonts/Normal");

            #endregion

            #region LOAD SPRITES

            LoadSprites();

            #endregion

            #region LOAD SOUNDS

            LoadSounds();

            #endregion

            //Create music instance
            music = gameMusic.CreateInstance();

            //Music properties
            music.IsLooped = true;
            music.Volume = 1.0f;

            //Enter in initial state
            EnterGameState(GameState.Menu);
        }

        private void LoadSprites()
        {
            #region Load sprites - Characters

            playerTexture = Content.Load<Texture2D>("Sprites/Player");
            playerMovingTexture = Content.Load<Texture2D>("Sprites/PlayerMoving");
            playerJumpingTexture = Content.Load<Texture2D>("Sprites/PlayerJumping");

            playerLowPunchTexture = Content.Load<Texture2D>("Sprites/PlayerLPunch");
            playerPunchTexture = Content.Load<Texture2D>("Sprites/PlayerPunch");

            playerLowKickTexture = Content.Load<Texture2D>("Sprites/PlayerLKick");
            playerMediumKickTexture = Content.Load<Texture2D>("Sprites/PlayerMKick");
            playerHighKickTexture = Content.Load<Texture2D>("Sprites/PlayerHKick");
            playerFlyingKickTexture = Content.Load<Texture2D>("Sprites/PlayerFKick");

            playerDeadTexture = Content.Load<Texture2D>("Sprites/PlayerDead");
            playerDeadAnimationTexture = Content.Load<Texture2D>("Sprites/PlayerDeadAnimation");

            enemy001Texture = Content.Load<Texture2D>("Sprites/Enemy001");

            enemy001MovingTexture = Content.Load<Texture2D>("Sprites/Enemy001Moving");
            enemy001PrepareAttackTexture = Content.Load<Texture2D>("Sprites/Enemy001PrepareAttack");

            enemy001KickTexture = Content.Load<Texture2D>("Sprites/Enemy001Kick");
            enemy001LowStaffAttackTexture = Content.Load<Texture2D>("Sprites/Enemy001LStaffAttack");
            enemy001StaffAttackTexture = Content.Load<Texture2D>("Sprites/Enemy001StaffAttack");

            enemy001DeadTexture = Content.Load<Texture2D>("Sprites/Enemy001Dead");

            #endregion

            #region Load sprites - Objects

            fireTexture = Content.Load<Texture2D>("Sprites/Fire");
            fireMovingTexture = Content.Load<Texture2D>("Sprites/FireMoving");

            lifeBarTexture = Content.Load<Texture2D>("Sprites/LifeBar");
            lifeFragment = Content.Load<Texture2D>("Sprites/LifeFragment");
            extraLifeTexture = Content.Load<Texture2D>("Sprites/extraLife");

            #endregion

            #region Load sprites - Hitboxes

            selectedOptionTexture = Content.Load<Texture2D>("Sprites/SelectedOption");

            playerHitTexture = Content.Load<Texture2D>("Sprites/playerHit");
            enemyHitTexture = Content.Load<Texture2D>("Sprites/enemyHit");
            fireHitTexture = Content.Load<Texture2D>("Sprites/fireHit");

            #endregion

            #region Load sprites - Stages

            menuTexture = Content.Load<Texture2D>("Sprites/Menu");
            stageLoadTexture = Content.Load<Texture2D>("Sprites/StageLoad");
            stageTexture = Content.Load<Texture2D>("Sprites/Stage");
            stage2Texture = Content.Load<Texture2D>("Sprites/Stage2");

            #endregion

            #region Load sprites - Messages

            pauseTexture = Content.Load<Texture2D>("Sprites/Pause");
            overTexture = Content.Load<Texture2D>("Sprites/Over");
            perfectTexture = Content.Load<Texture2D>("Sprites/Perfect");

            points0Texture = Content.Load<Texture2D>("Sprites/points0");
            points100Texture = Content.Load<Texture2D>("Sprites/points100");
            points200Texture = Content.Load<Texture2D>("Sprites/points200");
            points300Texture = Content.Load<Texture2D>("Sprites/points300");

            enemy001NameTexture = Content.Load<Texture2D>("Sprites/Enemy001Name");

            #endregion

            #region Load Sprites - Numbers

            N0Texture = Content.Load<Texture2D>("Sprites/N0");
            N1Texture = Content.Load<Texture2D>("Sprites/N1");
            N2Texture = Content.Load<Texture2D>("Sprites/N2");
            N3Texture = Content.Load<Texture2D>("Sprites/N3");
            N4Texture = Content.Load<Texture2D>("Sprites/N4");
            N5Texture = Content.Load<Texture2D>("Sprites/N5");
            N6Texture = Content.Load<Texture2D>("Sprites/N6");
            N7Texture = Content.Load<Texture2D>("Sprites/N7");
            N8Texture = Content.Load<Texture2D>("Sprites/N8");
            N9Texture = Content.Load<Texture2D>("Sprites/N9");

            #endregion

            #region Load sprites - Debugs

            debugCircleTex = Content.Load<Texture2D>("Sprites/debug_circle");
            debugRectangleTex = Content.Load<Texture2D>("Sprites/debug_rectangle");
            debugBigRectangleTex = Content.Load<Texture2D>("Sprites/debug_big_rectangle");
            debugHitTex = Content.Load<Texture2D>("Sprites/debug_hitbox");
            debugZones = Content.Load<Texture2D>("Sprites/debug_zones");

            #endregion
        }

        private void LoadSounds()
        {
            pauseSound = Content.Load<SoundEffect>("Sounds/pauseSound");
            gameMusic = Content.Load<SoundEffect>("Sounds/gameMusic");
            overSound = Content.Load<SoundEffect>("Sounds/overSound");
            victorySound = Content.Load<SoundEffect>("Sounds/victorySound");
            lossSound = Content.Load<SoundEffect>("Sounds/lossSound");

            hitSound = Content.Load<SoundEffect>("Sounds/hitSound");
            lowLifeSound = Content.Load<SoundEffect>("Sounds/lowLifeSound");
            wrongHitSound = Content.Load<SoundEffect>("Sounds/wrongHitSound");


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

        float startTime = 2; //Tempo de espera para passar para o LoadStage
        bool gameStarted = false; //Indica se o player apertou W ou Start
        float loadStageTime = 2; //Tempo de espera na tela LoadStage até o início da partida
        float checkLifeTime = 0.5f; //Tempo de espera para checar cada ponto de vida
        float overTime = 0.3f; //Tempo para encerrar a partida
        float countdownToRestart = 4; //Tempo para reiniciar a partida após perder uma vida

        public void EnterGameState(GameState newState)
        {
            LeaveGameState();

            currentState = newState;

            switch (currentState)
            {
                case GameState.Menu:
                    {
                        music.Stop();

                        entities.Clear();

                        StageSelector.instance.gameOver();

                        ScoreBoard.instance.resetScore();

                        if (SelectedOption.instance == null)
                            menuEntities.Add(new SelectedOption());
                    }
                    break;

                case GameState.StageLoad:
                    { }
                    break;

                case GameState.Stage:
                    { }
                    break;

                case GameState.Pause:
                    {
                        music.Pause();

                        pauseSound.Play();
                    }
                    break;

                case GameState.Over:
                    { }
                    break;
            }
        }

        public void UpdateGameState(GameTime gameTime)
        {
            switch (currentState)
            {
                case GameState.Menu:
                    {
                        logicMenu(gameTime);
                    }
                    break;

                case GameState.StageLoad:
                    {
                        logicStageLoad(gameTime);
                    }
                    break;

                case GameState.Stage:
                    {
                        logicStage(gameTime);
                    }
                    break;

                case GameState.Pause:
                    {
                        logicPause(gameTime);
                    }
                    break;
                
                case GameState.Over:
                    {
                        logicOver(gameTime);
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

                case GameState.StageLoad:
                    {
                        drawStageLoad(gameTime);
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
                    {
                        drawOver(gameTime);
                    }
                    break;
            }
        }

        public void LeaveGameState()
        {
            switch (currentState)
            {
                case GameState.Menu:
                    {
                        //Adding entities
                        entities.Add(new Human(new Vector2(260, 768), new Vector2(88, 128)));
                        entities.Add(new Enemy(new Vector2(642, 768), new Vector2(108, 160)));

                        entities.Add(new Lifebar());

                        entities.Add(new PlayerExtraLives());

                        gameStarted = false;
                        startTime = 2f;
                    }
                    break;

                case GameState.StageLoad:
                    {
                        Lifebar.instance.revivePlayer();

                        Lifebar.instance.reviveEnemy();

                        entities.Remove(Enemy.instance);
                        entities.Remove(Human.instance);

                        entities.Add(new Human(new Vector2(260, 768), new Vector2(88, 128)));
                        entities.Add(new Enemy(new Vector2(642, 768), new Vector2(108, 160)));
                        
                        loadStageTime = 2f;
                    }
                    break;

                case GameState.Stage:
                    {
                        if (Lifebar.instance.remainingPlayerLife() == 0 ||
                            Lifebar.instance.remainingEnemyLife() == 0)
                            music.Stop();

                        overTime = 0.3f;



                        if (Lifebar.instance.remainingPlayerLife() == 0)
                        {
                            if (PlayerExtraLives.instance.remainingLives() == 0)
                            {
                                overSound.Play();
                            }
                            else
                            {
                                lossSound.Play();
                                
                            }
                        }

                        
                            

                        if (Lifebar.instance.remainingEnemyLife() == 0)
                            victorySound.Play();
                    }
                    break;

                case GameState.Pause:
                    {
                        music.Resume();
                    }
                    break;

                case GameState.Over:
                    {
                        checkLifeTime = 0.5f;
                        countdownToRestart = 4;
                    }
                    break;
            }
        }

        #endregion

        #region LOGIC STATES

        private void logicMenu(GameTime gameTime)
        {
            //Timer
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) && prevKeyState.IsKeyUp(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && prevKeyState.IsKeyUp(Keys.Space) ||
                Keyboard.GetState().IsKeyDown(Keys.Q) && prevKeyState.IsKeyUp(Keys.Q))
                SelectedOption.instance.ChangeSelectedOption();

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && prevKeyState.IsKeyUp(Keys.Enter) ||
                Keyboard.GetState().IsKeyDown(Keys.W) && prevKeyState.IsKeyUp(Keys.W))
                gameStarted = true;

            if (startTime > 0 && gameStarted)
            {
                startTime -= deltaTime;

                if (music.State.Equals(SoundState.Stopped))
                    music.Play();
            }
            else if (startTime <= 0 && gameStarted)
                EnterGameState(GameState.StageLoad);
        }

        private void logicStageLoad(GameTime gameTime)
        {
            //Timer
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (music.State.Equals(SoundState.Stopped))
                music.Play();

            if (loadStageTime > 0)
                loadStageTime -= deltaTime;
            else
                EnterGameState(GameState.Stage);
        }

        private void logicStage(GameTime gameTime)
        {
            //Timer
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && prevKeyState.IsKeyUp(Keys.Escape))
                EnterGameState(GameState.Menu);

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && prevKeyState.IsKeyUp(Keys.Enter) ||
                Keyboard.GetState().IsKeyDown(Keys.W) && prevKeyState.IsKeyUp(Keys.W))
                EnterGameState(GameState.Pause);

            List<Entity> tmp = new List<Entity>(entities);
            foreach (Entity e in tmp)
                e.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.I) && !prevKeyState.IsKeyDown(Keys.I))
            {
                if (Lifebar.instance.godMode)
                    Lifebar.instance.godMode = false;
                else
                    Lifebar.instance.godMode = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F1) && !prevKeyState.IsKeyDown(Keys.F1))
                debugMode = !debugMode;

            if (debugMode)
                if (Keyboard.GetState().IsKeyDown(Keys.F5) && prevKeyState.IsKeyUp(Keys.F5))
                    Lifebar.instance.reviveEnemy();

            if (debugMode)
                if (Keyboard.GetState().IsKeyDown(Keys.F6) && prevKeyState.IsKeyUp(Keys.F6))
                    Lifebar.instance.revivePlayer();

            if (debugMode)
                if (Keyboard.GetState().IsKeyDown(Keys.Subtract) && prevKeyState.IsKeyUp(Keys.Subtract))
                    PlayerExtraLives.instance.reduceALife();

            if (debugMode)
                if (Keyboard.GetState().IsKeyDown(Keys.Add) && prevKeyState.IsKeyUp(Keys.Add))
                    PlayerExtraLives.instance.bonusLife();

            if (debugMode)
                if (Keyboard.GetState().IsKeyDown(Keys.NumPad1) && prevKeyState.IsKeyUp(Keys.NumPad1))
                    ScoreBoard.instance.add100points();

            if (debugMode)
                if (Keyboard.GetState().IsKeyDown(Keys.NumPad2) && prevKeyState.IsKeyUp(Keys.NumPad2))
                    ScoreBoard.instance.add200points();

            if (debugMode)
                if (Keyboard.GetState().IsKeyDown(Keys.NumPad3) && prevKeyState.IsKeyUp(Keys.NumPad3))
                    ScoreBoard.instance.add300points();

            if (debugMode)
                if (Keyboard.GetState().IsKeyDown(Keys.NumPad5) && prevKeyState.IsKeyUp(Keys.NumPad5))
                    ScoreBoard.instance.add5000points();

            if (debugMode)
                if (Keyboard.GetState().IsKeyDown(Keys.NumPad8) && prevKeyState.IsKeyUp(Keys.NumPad8))
                    ScoreBoard.instance.add800points();

            if (Lifebar.instance.remainingPlayerLife() == 0 || Lifebar.instance.remainingEnemyLife() == 0)
                if (overTime > 0)
                    overTime -= deltaTime;
                else if (Lifebar.instance.remainingEnemyLife() == 0)
                {
                    if (Lifebar.instance.remainingPlayerLife() == 9)
                        ScoreBoard.instance.perfect = true;

                    ScoreBoard.instance.playerWinner = true;
                    EnterGameState(GameState.Over);
                }
                else if (Lifebar.instance.remainingPlayerLife() == 0)
                    EnterGameState(GameState.Over);
        }

        private void logicPause(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && prevKeyState.IsKeyUp(Keys.Enter) ||
                Keyboard.GetState().IsKeyDown(Keys.W) && prevKeyState.IsKeyUp(Keys.W))
                EnterGameState(GameState.Stage);
        }

        private void logicOver(GameTime gameTime)
        {
            //Timer
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (ScoreBoard.instance.playerWinner)
            {
                if (checkLifeTime > 0)
                    checkLifeTime -= deltaTime;
                else if (Lifebar.instance.remainingPlayerLife() > 0)
                {
                    if (Lifebar.instance.remainingPlayerLife() == 9)
                        ScoreBoard.instance.add5000points();

                    Lifebar.instance.damagePlayerLife();
                    ScoreBoard.instance.add800points();
                    checkLifeTime = 0.5f;
                }
                else
                {
                    if (countdownToRestart > 0)
                        countdownToRestart -= deltaTime;
                    else
                    {
                        StageSelector.instance.nextStage();
                        ScoreBoard.instance.resetPlayerWinner();
                        ScoreBoard.instance.resetPerfect();
                        EnterGameState(GameState.StageLoad);
                    }
                }
            }
            else
            {
                if (countdownToRestart > 0)
                    countdownToRestart -= deltaTime;
                else
                {
                    if (PlayerExtraLives.instance.remainingLives() > 0)
                    {
                        PlayerExtraLives.instance.reduceALife();

                        EnterGameState(GameState.StageLoad);
                    }
                    else
                        EnterGameState(GameState.Menu);
                }
            }
        }

        #endregion

        #region DRAW STATES

        private void drawMenu(GameTime gameTime)
        {
            //Draw menu
            spriteBatch.Draw(menuTexture, new Vector2(0f, 0f), null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.4f);

            //Draw each menu entity
            foreach (Entity e in menuEntities)
                e.Draw(gameTime);
        }

        private void drawStageLoad(GameTime gameTime)
        {
            //Draw stage load
            spriteBatch.Draw(stageLoadTexture, new Vector2(0f, 0f), null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.4f);

            StageSelector.instance.StageDraw(gameTime);
        }

        private void drawStage(GameTime gameTime)
        {
            //Draw stage
            spriteBatch.Draw(StageSelector.instance.stageImage(), new Vector2(0f, 0f), null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.4f);

            if (Lifebar.instance.godMode)
            {
                string text = "GOD MODE ACTIVATED";

                Vector2 textSize = fontNormal.MeasureString(text);

                spriteBatch.DrawString(fontNormal, "GOD MODE ACTIVATED", textSize, Color.Red);
            }

            //Draw each entity
            foreach (Entity e in entities)
                e.Draw(gameTime);

            stageSelector.Draw(gameTime);

            scoreBoard.Draw(gameTime);
        }

        private void drawPause(GameTime gameTime)
        {
            spriteBatch.Draw(pauseTexture, new Vector2((graphics.PreferredBackBufferWidth - pauseTexture.Width) / 2, (graphics.PreferredBackBufferHeight - pauseTexture.Height) / 2), null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.3f);

            drawStage(gameTime);
        }

        private void drawCheckPoints(GameTime gameTime)
        {
            drawStage(gameTime);
        }

        private void drawOver(GameTime gameTime)
        {
            if (ScoreBoard.instance.perfect)
                spriteBatch.Draw(perfectTexture,
                    new Vector2((graphics.PreferredBackBufferWidth - perfectTexture.Width) / 2, 576),
                    null,
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    Vector2.One,
                    SpriteEffects.None,
                    0.0f);

            if (PlayerExtraLives.instance.remainingLives() == 0)
                spriteBatch.Draw(overTexture,
                    new Vector2((graphics.PreferredBackBufferWidth - overTexture.Width) / 2,
                    (graphics.PreferredBackBufferHeight - overTexture.Height) / 2),
                    null,
                    Color.White,
                    0.0f,
                    Vector2.Zero,
                    Vector2.One,
                    SpriteEffects.None,
                    0.3f);

            drawStage(gameTime);
        }

        #endregion
    }
}