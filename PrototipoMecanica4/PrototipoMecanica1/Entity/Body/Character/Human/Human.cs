using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace PrototipoMecanica1
{
    public class Human : Character
    {
        float TC = 0f; //Timer Counter
        float JT = 0.500f; //Jump Time
        float JH = 7f; //Jump height
        float dx = 0; //direção do salto

        float t;  //valor de tempo para calcular o seno
        float dy; //efeito de pulo

        //Machine states
        public enum CharacterState { Null, Standing, Moving, Crouching, Jumping, Attacking, Recoiling, Dead }; //Nenhum estado, parado, movendo-se, saltando, atacando, recuando, morto

        //Current State
        public static CharacterState currentState = CharacterState.Null;

        public Human(Vector2 initPos, Vector2 size) : base(initPos, size)
        {
            //New position
            EnterCharacterState(CharacterState.Standing);

            speed *= 2;
        }

        /*
         JT = 1;      Tempo de Pulo
         JH = 100f;  100 pixels
         TC = 
         y-dy;
         dy = JH * Math.Sin(t)
         t = (TC/JT) * phi()
        */

        public override Vector2 GetDir()
        {
            Vector2 dir = Vector2.Zero;

            if ((currentState.ToString() == "Standing" || currentState.ToString() == "Moving") && Keyboard.GetState().IsKeyDown(Keys.Right))
                dir.X += 1.0f;

            if ((currentState.ToString() == "Standing" || currentState.ToString() == "Moving") && Keyboard.GetState().IsKeyDown(Keys.Left))
                dir.X += -1.0f;

            return dir;
            //return Vector2.Zero;
            //if (Keyboard.GetState().IsKeyDown(Keys.Up) && pos.Y == size.Y / 2f + (768f - size.Y))
            //    dir.Y += -1.0f;
            //
            //if (Keyboard.GetState().IsKeyDown(Keys.Down))
            //    dir.Y += 1.0f;
        }

        public override Texture2D GetSprite()
        {
            return World.playerTexture;
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

            World.spriteBatch.Draw(sprite,
              pos,
              null,
              Color.White,
              0.0f,
              new Vector2(sprite.Width/2f,
                          sprite.Height) , //pivot
              new Vector2(size.X / sprite.Width,
                          size.Y / sprite.Height), //scale
              SpriteEffects.None,
              0.1f
            );

            if (World.debugMode)
            {
                World.spriteBatch.Draw(World.debugRectangleTex,
                  pos,
                  null,
                  new Color(0.0f, 1.0f, 0.0f, 0.5f),
                  0.0f,
                  new Vector2(World.debugRectangleTex.Width/2f,
                              World.debugRectangleTex.Height), //pivot
                  Vector2.One, //scale
                  SpriteEffects.None,
                  0.3f
                );

                World.spriteBatch.DrawString(World.fontNormal, "State: " + currentState.ToString() + " TC: " + TC, new Vector2(this.pos.X, this.pos.Y + 50f), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
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

                        if (Keyboard.GetState().IsKeyDown(Keys.Up) && pos.Y == size.Y + (768f - size.Y))
                        {
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
                        if (TC >= 1f)
                        {
                            EnterCharacterState(CharacterState.Standing);
                        }

                        if (TC >= 0f)
                        {
                            t = (TC / JT) * (float)Math.PI;
                            dy = JH * (float)Math.Sin(t);
                            pos.Y -= dy;

                            TC += deltaTime;
                        }
                    }
                    break;

                case CharacterState.Attacking:
                    {
                        //World.entities.Add(new Hit(this, pos, new Vector2(32, 32)));
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
                    { }
                    break;

                case CharacterState.Crouching:
                    { }
                    break;

                case CharacterState.Jumping:
                    {
                        TC = 0.00f;
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