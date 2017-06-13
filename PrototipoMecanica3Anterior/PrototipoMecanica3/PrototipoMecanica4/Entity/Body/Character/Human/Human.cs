using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace PrototipoMecanica4
{
    public class Human : Character
    {
        static public Human instance = null;

        float timerCounter = 0f; //Timer Counter
        float JT = 0.500f; //Jump Time
        float JH = 7f; //Jump height
        float dx = 0; //direção do salto

        int AttackState; //AttackState == 0, no attack
        bool movingFrame = false; //Moving frame
        int framePersistance; //Conta 3 e muda frame

        bool canPunch = true; // Verificação de soco
        bool canKick = true; // Verificação de chute
        bool Crouching = false; //verificação de personagem abaixado
        bool canJump = true; // Verificação de pulo

        float t;  //valor de tempo para calcular o seno
        float dy; //efeito de pulo

        //Machine states
        public enum CharacterState { Null, Standing, Moving, Crouching, Jumping, Attacking, Recoiling, Dead }; //Nenhum estado, parado, movendo-se, saltando, atacando, recuando, morto

        //Current State
        public static CharacterState currentState = CharacterState.Null;

        public Human(Vector2 initPos, Vector2 size) : base(initPos, size)
        {
            instance = this;

            //New position
            EnterCharacterState(CharacterState.Standing);

            speed *= 2;
        }

        public override Vector2 GetDir()
        {
            Vector2 dir = Vector2.Zero;

            if ((currentState.Equals(CharacterState.Standing) || currentState.Equals(CharacterState.Moving)) && Keyboard.GetState().IsKeyDown(Keys.Right))
                dir.X += 1.0f;

            if ((currentState.Equals(CharacterState.Standing) || currentState.Equals(CharacterState.Moving)) && Keyboard.GetState().IsKeyDown(Keys.Left))
                dir.X += -1.0f;

            return dir;
        }

        public override Texture2D GetSprite()
        {
            Texture2D currentTexture = null;

            switch (currentState)
            {
                case CharacterState.Standing:
                    currentTexture = World.playerStandingTexture;
                    break;

                case CharacterState.Moving:
                    {
                        if (movingFrame)
                            currentTexture = World.playerMovingTexture;
                        else
                            currentTexture = World.playerStandingTexture;
                    }
                    break;

                case CharacterState.Crouching:
                    currentTexture = World.playerJumpingTexture;
                    break;

                case CharacterState.Jumping:
                    currentTexture = World.playerJumpingTexture;
                    break;

                case CharacterState.Attacking:
                    {
                        if (AttackState == 1)
                        {
                            currentTexture = World.playerPunchTexture;
                        }
                        if (AttackState == 2)
                        {
                            currentTexture = World.playerLowPunchTexture;
                        }
                        if (AttackState == 3)
                        {
                            currentTexture = World.playerHighKickTexture;
                        }
                        if (AttackState == 4)
                        {
                            currentTexture = World.playerMediumKickTexture;
                        }
                        if (AttackState == 5)
                        {
                            currentTexture = World.playerLowKickTexture;
                        }
                    }
                    break;

                default:
                    currentTexture = World.playerStandingTexture;
                    break;
            }

            return currentTexture;
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateCharacterState(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Texture2D sprite = GetSprite();

            if (pos.X <= Enemy.instance.pos.X)
            {
                World.spriteBatch.Draw(sprite,
                    pos,
                    null,
                    Color.White,
                    0.0f,
                    new Vector2(sprite.Width / 2f,
                                sprite.Height), //pivot
                    Vector2.One, //scale
                    SpriteEffects.None,
                    0.1f
                );
            }
            else
            {
                World.spriteBatch.Draw(sprite,
                    pos,
                    null,
                    Color.White,
                    0.0f,
                    new Vector2(sprite.Width / 2f,
                                sprite.Height), //pivot
                    Vector2.One, //scale
                    SpriteEffects.FlipHorizontally,
                    0.1f
                );
            }

            if (World.debugMode)
            {
                World.spriteBatch.Draw(World.debugRectangleTex,
                  pos,
                  null,
                  new Color(0.0f, 1.0f, 0.0f, 0.5f),
                  0.0f,
                  new Vector2(sprite.Width / 2f,
                              sprite.Height), //pivot
                  Vector2.One, //scale
                  SpriteEffects.None,
                  0.3f
                );

                World.spriteBatch.DrawString(World.fontNormal, "State: " + currentState.ToString() + "\nTimer Counter: " + timerCounter, new Vector2(this.pos.X, this.pos.Y + 20f), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            }
        }

        //States
        public void EnterCharacterState(CharacterState newState)
        {
            LeaveCharacterState();

            currentState = newState;

            switch (currentState)
            {
                case CharacterState.Standing:
                    { }
                    break;

                case CharacterState.Moving:
                    { }
                    break;

                case CharacterState.Crouching:
                    { }
                    break;

                case CharacterState.Jumping:
                    { }
                    break;

                case CharacterState.Attacking:
                    { }
                    break;

                case CharacterState.Recoiling:
                    { }
                    break;

                case CharacterState.Dead:
                    { };
                    break;
            }
        }

        public void UpdateCharacterState(GameTime gameTime)
        {
            //Timer
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (currentState)
            {
                case CharacterState.Standing:
                    {
                        //Move
                        if (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.Left))
                        {
                            EnterCharacterState(CharacterState.Moving);
                        }

                        if (Keyboard.GetState().IsKeyDown(Keys.Up) && pos.Y == size.Y + (768f - size.Y) && canJump)
                        {
                            EnterCharacterState(CharacterState.Jumping);
                            canJump = false;
                        }

                        if (Keyboard.GetState().IsKeyDown(Keys.Down))
                        {
                            EnterCharacterState(CharacterState.Crouching);
                        }


                        //Attack
                        if (canPunch)
                        {
                            if (Keyboard.GetState().IsKeyDown(Keys.X)) //Soco Médio
                            {
                                EnterCharacterState(CharacterState.Attacking);
                                AttackState = 1;
                                canPunch = false;
                            }                                                        
                        }
                        if (canKick)
                        {
                            if (Keyboard.GetState().IsKeyDown(Keys.Z)) //Chute Médio
                            {
                                EnterCharacterState(CharacterState.Attacking);
                                AttackState = 4;
                                canKick = false;
                            }
                        }
                        

                        if (Keyboard.GetState().IsKeyUp(Keys.X))
                            canPunch = true;
                        if (Keyboard.GetState().IsKeyUp(Keys.Z))
                            canKick = true;
                        if (Keyboard.GetState().IsKeyUp(Keys.Up))
                            canJump = true;

                    }
                    break;

                case CharacterState.Moving:
                    {
                        GetDir();

                        if (Keyboard.GetState().IsKeyUp(Keys.Left) && Keyboard.GetState().IsKeyUp(Keys.Right))
                        {
                            EnterCharacterState(CharacterState.Standing);
                        }
                        else
                        {
                            if (movingFrame && framePersistance == 3)
                            {
                                movingFrame = false;
                                framePersistance = 0;
                            }
                            else if (movingFrame == false && framePersistance == 3)
                            {
                                movingFrame = true;
                                framePersistance = 0;
                            }

                            framePersistance++;

                            //Attack
                            if (canPunch)
                            {
                                if (Keyboard.GetState().IsKeyDown(Keys.X)) //Soco Médio
                                {
                                    EnterCharacterState(CharacterState.Attacking);
                                    AttackState = 1;
                                    canPunch = false;
                                }
                            }

                            if (canKick)
                            {
                                if (Keyboard.GetState().IsKeyDown(Keys.Z) && Keyboard.GetState().IsKeyDown(Keys.Left) ||
                                Keyboard.GetState().IsKeyDown(Keys.Z) && Keyboard.GetState().IsKeyDown(Keys.Right)) //Chute Alto
                                {
                                    EnterCharacterState(CharacterState.Attacking);
                                    AttackState = 3;
                                    canKick = false;
                                }
                            }

                            if (Keyboard.GetState().IsKeyUp(Keys.X))
                                canPunch = true;
                            if (Keyboard.GetState().IsKeyUp(Keys.Z))
                                canKick = true;

                        }
                    }
                    break;

                case CharacterState.Crouching:
                    {
                        if (Keyboard.GetState().IsKeyUp(Keys.Down))
                        {
                            EnterCharacterState(CharacterState.Standing);
                            Crouching = false;
                        }
                        if (canKick)
                        {
                            if (Keyboard.GetState().IsKeyDown(Keys.Z) && Keyboard.GetState().IsKeyDown(Keys.Down)) //Chute Baixo
                            {
                                Crouching = true;
                                EnterCharacterState(CharacterState.Attacking);
                                AttackState = 5;
                                canKick = false;
                            }                            
                        }
                        if (canPunch)
                        {
                            if (Keyboard.GetState().IsKeyDown(Keys.X) && Keyboard.GetState().IsKeyDown(Keys.Down)) //Soco Baixo
                            {
                                Crouching = true;
                                EnterCharacterState(CharacterState.Attacking);
                                AttackState = 2;
                                canPunch = false;
                            }
                        }

                        if (Keyboard.GetState().IsKeyUp(Keys.X))
                            canPunch = true;
                        if (Keyboard.GetState().IsKeyUp(Keys.Z))
                            canKick = true;
                    }
                    break;

                case CharacterState.Jumping:
                    {
                        if (timerCounter >= 1f)
                        {
                            EnterCharacterState(CharacterState.Standing);
                        }

                        if (timerCounter >= 0f)
                        {
                            t = (timerCounter / JT) * (float)Math.PI;
                            dy = JH * (float)Math.Sin(t);
                            pos.Y -= dy;

                            timerCounter += deltaTime;

                            //Attack
                        }
                    }
                    break;

                case CharacterState.Attacking:
                    {
                        if (timerCounter >= 1f)
                        {
                            if (Crouching)
                                EnterCharacterState(CharacterState.Crouching);
                            else
                                EnterCharacterState(CharacterState.Standing);
                        }

                        if (timerCounter >= 0f)
                        {
                            timerCounter += deltaTime;

                            World.entities.Add(new Hit(this, new Vector2(pos.X + 10, (pos.Y / 2) + pos.Y), GetDir(), new Vector2(32, 32)));
                        }
                    }

                    break;

                case CharacterState.Recoiling:
                    { }
                    break;

                case CharacterState.Dead:
                    { };
                    break;
            }
        }

        public void DrawCharacterState(GameTime gameTime)
        {
            //Timer
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (currentState)
            {
                case CharacterState.Standing:
                    { }
                    break;

                case CharacterState.Moving:
                    { }
                    break;

                case CharacterState.Crouching:
                    { }
                    break;

                case CharacterState.Jumping:
                    { }
                    break;

                case CharacterState.Attacking:
                    { }
                    break;

                case CharacterState.Recoiling:
                    { }
                    break;

                case CharacterState.Dead:
                    { };
                    break;
            }
        }

        public void LeaveCharacterState()
        {
            switch (currentState)
            {
                case CharacterState.Standing:
                    { }
                    break;

                case CharacterState.Moving:
                    {
                        
                    }
                    break;

                case CharacterState.Crouching:
                    { }
                    break;

                case CharacterState.Jumping:
                    {
                        timerCounter = 0.00f;
                    }
                    break;

                case CharacterState.Attacking:
                    {
                        timerCounter = 0.00f;
                    }
                    break;

                case CharacterState.Recoiling:
                    { }
                    break;

                case CharacterState.Dead:
                    { };
                    break;
            }
        }


        public override bool WantsToFire()
        {
            return Keyboard.GetState().IsKeyDown(Keys.LeftControl);
        }
    }
}