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
    public class Enemy : Character
    {
        static public Enemy instance = null;

        public float visionRadius;

        private float safeDistance = 124f; //Distância segura do player, para aproximação
        private float dangerDistance = 0f; //Distância perigosa do player, para afastar-se
        private float runDistance = 0f; //Distância de corrida, multiplicar por pontos de vida perdidos

        //Machine states
        public enum CharacterState { Null, Standing, Moving, Attacking, Recoiling, Running, Dead }; //Nenhum estado, parado, movendo-se, atacando, recuando, correndo, morto

        //Current State
        public static CharacterState previousState = CharacterState.Null;
        public static CharacterState currentState = CharacterState.Null;

        public Enemy(Vector2 initPos, Vector2 size)
            : base(initPos, size)
        {
            instance = this;

            //New position
            EnterCharacterState(CharacterState.Standing);

            speed *= 2f;
            visionRadius = size.X * 2;

            //Definir distâncias
            safeDistance = 104 + (Human.instance.size.X / 2) + (Enemy.instance.size.X / 2);
        }

        public override Vector2 GetDir()
        {
            Body testBody = Human.instance;

            if (!currentState.Equals(CharacterState.Dead))
            {
                Vector2 positionToReturn;

                if (pos.X >= Human.instance.pos.X)
                    positionToReturn.X = testBody.pos.X - pos.X + safeDistance;
                else
                    positionToReturn.X = testBody.pos.X - pos.X - safeDistance;

                positionToReturn.Y = pos.Y;

                return positionToReturn;
            }

            return Vector2.Zero;
        }

        public override Texture2D GetSprite()
        {
            Texture2D currentTexture = null;

            switch (currentState)
            {
                case CharacterState.Standing:
                    currentTexture = World.enemy001Texture;
                    break;

                case CharacterState.Dead:
                    currentTexture = World.enemy001DeadTexture;
                    break;

                default:
                    currentTexture = World.enemy001Texture;
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

            if (pos.X >= Human.instance.pos.X)
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
                    0.2f
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
                    0.2f
                );
            }

            if (World.debugMode)
            {
                World.spriteBatch.Draw(World.debugBigRectangleTex,
                  pos,
                  null,
                  new Color(0.0f, 0.0f, 1.0f, 0.5f),
                  0.0f,
                  new Vector2(sprite.Width / 2f,
                              sprite.Height), //pivot
                  Vector2.One, //scale
                  SpriteEffects.None,
                  0.3f
                );

                //World.spriteBatch.Draw(World.debugCircleTex,
                //  pos,
                //  null,
                //  new Color(1.0f, 0.0f, 0.0f, 0.5f),
                //  0.0f,
                //  new Vector2(World.debugCircleTex.Width,
                //              World.debugCircleTex.Height) / 2f, //pivot
                //  new Vector2(2f * visionRadius / (float)World.debugCircleTex.Width,
                //              2f * visionRadius / (float)World.debugCircleTex.Height), //scale
                //  SpriteEffects.None,
                //  0.3f
                //);

                World.spriteBatch.DrawString(World.fontNormal, "State: " + currentState.ToString(), new Vector2(this.pos.X, this.pos.Y + 20f), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
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

                case CharacterState.Attacking:
                    { }
                    break;

                case CharacterState.Recoiling:
                    { }
                    break;

                case CharacterState.Running:
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
                        if (!((int)GetDir().X).Equals((int)Vector2.Zero.X) && Lifebar.instance.remainingEnemyLife() >= 1)
                            EnterCharacterState(CharacterState.Moving);
                        else if (Lifebar.instance.remainingEnemyLife() <= 0)
                            EnterCharacterState(CharacterState.Dead);
                    }
                    break;

                case CharacterState.Moving:
                    {
                        //if (Math.Abs(GetDir().X) < dangerDistance)
                        //{
                        //    EnterCharacterState(CharacterState.Running);
                        //}
                        //
                        //else if (Math.Abs(GetDir().X) > dangerDistance && Math.Abs(GetDir().X) <= safeDistance)
                        //{
                        //    EnterCharacterState(CharacterState.Attacking);
                        //}
                        //
                        //else
                        //{
                        //
                        //}

                        if (((int)GetDir().X).Equals((int)Vector2.Zero.X) && Lifebar.instance.remainingEnemyLife() >= 1)
                            EnterCharacterState(CharacterState.Standing);
                        else if (Lifebar.instance.remainingEnemyLife() <= 0)
                            EnterCharacterState(CharacterState.Dead);
                    }
                    break;

                case CharacterState.Attacking:
                    { }
                    break;

                case CharacterState.Recoiling:
                    { }
                    break;

                case CharacterState.Running:
                    { }
                    break;

                case CharacterState.Dead:
                    {
                        if (Lifebar.instance.remainingEnemyLife() > 0)
                            EnterCharacterState(CharacterState.Standing);
                    };
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

                case CharacterState.Attacking:
                    { }
                    break;

                case CharacterState.Recoiling:
                    { }
                    break;

                case CharacterState.Running:
                    { }
                    break;

                case CharacterState.Dead:
                    { };
                    break;
            }
        }
    }
}