using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace PrototipoMecanica2
{
    public class Human : Character
    {
        static public Human instance = null;

        float timerCounter = 0f; //Timer Counter
        float jumpTime = 0.500f; //tempo do salto
        float jumpHeight = 7f; //altura do salto
        float jumpDirection = 0f; //dire��o do salto
        float jumpDistance = 7f; //dist�ncia do salto
        float time;  //valor de tempo para calcular o seno
        float jumpEffectY; //efeito de pulo para o alto
        float jumpEffectX; //efeito de pulo para a dire��o

        float movingTime; //valor de tempo para calcular movimento
        int framePersistance; //Conta 3 e muda frame
        bool movingFrame = false; //Moving frame

        //Machine states
        public enum CharacterState { Null, Standing, Moving, Crouching, Jumping, Attacking, Recoiling, Dead }; //Nenhum estado, parado, movendo-se, abaixando-se, saltando, atacando, recuando, morto

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
                    currentTexture = World.playerTexture;
                    break;

                case CharacterState.Moving:
                    {
                        if (movingFrame)
                            currentTexture = World.playerMovingTexture;
                        else
                            currentTexture = World.playerTexture;
                    }
                    break;

                case CharacterState.Crouching:
                    currentTexture = World.playerJumpingTexture;
                    break;

                case CharacterState.Jumping:
                    currentTexture = World.playerJumpingTexture;
                    break;

                default:
                    currentTexture = World.playerTexture;
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

            if (pos.X < Enemy.instance.pos.X)
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

                World.spriteBatch.DrawString(World.fontNormal, "State: " + currentState.ToString() + " TC: " + timerCounter, new Vector2(this.pos.X, this.pos.Y + 50f), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
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
                        if (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.Left))
                        {
                            EnterCharacterState(CharacterState.Moving);
                        }

                        if (Keyboard.GetState().IsKeyDown(Keys.Left) && Keyboard.GetState().IsKeyDown(Keys.Up) && pos.Y == size.Y + (768f - size.Y))
                        {
                            jumpDirection = -1;
                            EnterCharacterState(CharacterState.Jumping);
                        }

                        else if (Keyboard.GetState().IsKeyDown(Keys.Right) && Keyboard.GetState().IsKeyDown(Keys.Up) && pos.Y == size.Y + (768f - size.Y))
                        {
                            jumpDirection = 1;
                            EnterCharacterState(CharacterState.Jumping);
                        }

                        else if (Keyboard.GetState().IsKeyDown(Keys.Up) && pos.Y == size.Y + (768f - size.Y))
                        {
                            jumpDirection = 0;
                            EnterCharacterState(CharacterState.Jumping);
                        }

                        if (Keyboard.GetState().IsKeyDown(Keys.Down))
                        {
                            EnterCharacterState(CharacterState.Crouching);
                        }
                    }
                    break;

                case CharacterState.Moving:
                    {
                        GetDir();

                        if(movingTime < 0.100)
                        {
                            if(movingFrame && framePersistance == 3)
                            {
                                movingFrame = false;
                                framePersistance = 0;
                            }
                            else if(movingFrame == false && framePersistance == 3)
                            {
                                movingFrame = true;
                                framePersistance = 0;
                            }

                            framePersistance++;
                            movingTime += deltaTime;
                        }
                        else
                            EnterCharacterState(CharacterState.Standing);
                    }
                    break;

                case CharacterState.Crouching:
                    {
                        if (Keyboard.GetState().IsKeyUp(Keys.Down))
                        {
                            EnterCharacterState(CharacterState.Standing);
                        }
                    }
                    break;

                case CharacterState.Jumping:
                    {
                        if (timerCounter >= 1f)
                        {
                            EnterCharacterState(CharacterState.Standing);
                        }

                        //Salto para cima
                        if (timerCounter >= 0f && jumpDirection == 0)
                        {
                            time = (timerCounter / jumpTime) * (float)Math.PI;
                            jumpEffectY = jumpHeight * (float)Math.Sin(time);
                            pos.Y -= jumpEffectY;

                            timerCounter += deltaTime;
                        }
                        //Salto para a direita
                        else if(timerCounter >= 0f && jumpDirection == 1)
                        {
                            time = (timerCounter / jumpTime) * (float)Math.PI;
                            jumpEffectY = jumpHeight * (float)Math.Sin(time);
                            jumpEffectX = jumpDistance * (float)Math.Sin(time);
                            pos.Y -= jumpEffectY;
                            pos.X += jumpDistance;

                            timerCounter += deltaTime;

                            if(pos.X <= size.X / 2f + 96f || pos.X >= size.X / 2f + (928f - size.X))
                            {
                                jumpDirection *= -1;
                            }
                        }
                        //Salto para a esquerda
                        else if(timerCounter >= 0f && jumpDirection == -1)
                        {
                            time = (timerCounter / jumpTime) * (float)Math.PI;
                            jumpEffectY = jumpHeight * (float)Math.Sin(time);
                            jumpEffectX = jumpDistance * (float)Math.Sin(time);
                            pos.Y -= jumpEffectY;
                            pos.X -= jumpDistance;

                            timerCounter += deltaTime;

                            if (pos.X <= size.X / 2f + 96f || pos.X >= size.X / 2f + (928f - size.X))
                            {
                                jumpDirection *= -1;
                            }
                        }
                    }
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

        public void DrawCharacterState(GameTime gameTime)
        {
            //Timer
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (currentState)
            {
                case CharacterState.Standing:
                    {

                    }
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
                        movingTime = 0.000f;
                    }
                    break;

                case CharacterState.Crouching:
                    { }
                    break;

                case CharacterState.Jumping:
                    {
                        timerCounter = 0.000000f;
                    }
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

        public override bool WantsToFire()
        {
            return Keyboard.GetState().IsKeyDown(Keys.LeftControl);
        }
    }
}