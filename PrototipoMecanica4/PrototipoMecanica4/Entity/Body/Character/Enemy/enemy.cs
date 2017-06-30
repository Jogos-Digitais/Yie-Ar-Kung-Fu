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

        private float safeDistance = 0f; //Dist�ncia segura do player, para aproxima��o
        private float dangerDistance = 0f; //Dist�ncia perigosa do player, para afastar-se
        private float runDistance = 0f; //Dist�ncia de corrida, multiplicar por pontos de vida perdidos

        float movingTime; //valor de tempo para calcular movimento
        int framePersistance; //Conta 3 e muda frame
        bool movingFrame = false; //Moving frame

        float attackingTime = 0.25f; //valor de tempo para calcular ataques

        //Machine states
        public enum CharacterState { Null, Standing, Moving, Attacking, Recoiling, Running, Dead }; //Nenhum estado, parado, movendo-se, atacando, recuando, correndo, morto

        //Current State
        public static CharacterState previousState = CharacterState.Null;
        public static CharacterState currentState = CharacterState.Null;

        public Enemy(Vector2 initPos, Vector2 size) : base(initPos, size)
        {
            instance = this;

            //New position
            EnterCharacterState(CharacterState.Standing);

            speed /= 8f;

            //Definir dist�ncias
            safeDistance = 104 + (Human.instance.size.X / 2) + (size.X / 2);
            dangerDistance = -4 + (Human.instance.size.X / 2) + (size.X / 2); //Nunca verificar com igual, sempre menor
        }

        public override Vector2 GetDir()
        {
            Body testBody = Human.instance;

            if (!currentState.Equals(CharacterState.Dead))
            {
                Vector2 positionToReturn;

                if (pos.X >= Human.instance.pos.X)
                    positionToReturn.X = testBody.pos.X - pos.X + safeDistance; //+safedistance
                else
                    positionToReturn.X = testBody.pos.X - pos.X - safeDistance; //-safedistance

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

                case CharacterState.Moving:
                    {
                        if (movingFrame)
                            currentTexture = World.enemy001MovingTexture;
                        else
                            currentTexture = World.enemy001Texture;
                    }
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

            Vector2 dir = GetDir();

            float s = dir.Length();

            if (s > 0)
                dir = dir / s;

            //simulate physics independently in two axis
            for (int axis = 0; axis < 2; axis++)
            {
                Vector2 backupPos = pos; //save current position

                if (axis == 0)
                {
                    pos += new Vector2(dir.X, 0f) * speed; //only X * deltaTime
                }
                else
                {
                    pos += new Vector2(0f, dir.Y) * deltaTime * speed; //only Y
                }

                Entity collider = null;

                Vector2 myMin = GetMin();
                Vector2 myMax = GetMax();

                //test collision against all world entities
                foreach (Entity e in World.entities)
                {
                    if ((e != this) && //not myself?
                        (IgnoreCollision(e) == false) && //ignore collision with other?
                        (e.IgnoreCollision(this) == false) && //other ignores collision with me?
                        e.TestCollisionRect(myMin, myMax)) //is colliding against other entity?
                    {
                        collider = e; //collision detected!
                        CollisionDetected(e);
                        break;
                    }
                }

                if (collider != null) //undo movement!
                    pos = backupPos;
            }

            //Arena limits
            if (pos.X <= size.X / 2f + 96f) //(char width / 2) + limit
                pos.X = size.X / 2f + 96f;
            
            if (pos.X >= size.X / 2f + (928f - size.X)) //(char width / 2) + (limit - char width)
                pos.X = size.X / 2f + (928f - size.X);
            
            if (pos.Y >= size.Y + (768f - size.Y)) //(char height / 2) + (limit - char height)
                pos.Y = size.Y + (768f - size.Y);

            UpdateCharacterState(gameTime);
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

            float distance = Math.Abs(GetDir().X);

            switch (currentState)
            {
                case CharacterState.Standing:
                    {
                        if (!((int)distance == 0) && Lifebar.instance.remainingEnemyLife() >= 1)
                            EnterCharacterState(CharacterState.Moving);
                        else if (Lifebar.instance.remainingEnemyLife() <= 0)
                            EnterCharacterState(CharacterState.Dead);
                    }
                    break;

                case CharacterState.Moving:
                    {
                        if ((int)distance == 0 && Lifebar.instance.remainingEnemyLife() >= 1)
                            EnterCharacterState(CharacterState.Standing);

                        else if (Lifebar.instance.remainingEnemyLife() <= 0)
                            EnterCharacterState(CharacterState.Dead);

                        else if (movingTime < 0.200)
                        {
                            if (movingFrame && framePersistance == 10)
                            {
                                movingFrame = false;
                            }

                            else if (movingFrame == false && framePersistance == 10)
                            {
                                movingFrame = true;
                            }

                            framePersistance++;
                            movingTime += deltaTime;
                        }

                        else if(movingTime > 0.200)
                        {
                            movingTime = 0;
                            framePersistance = 0;
                        }
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
                    { 
						movingTime = 0f;
					}
                    break;

                case CharacterState.Attacking:
                    {
						attackingTime = 0.25f;
					}
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