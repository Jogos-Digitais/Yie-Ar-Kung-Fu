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

        private float timerCounter = 0f; //Timer Counter
        private float jumpTime = 0.5f; //tempo do salto
        private float jumpHeight = 9f; //altura do salto
        private float jumpDirection = 0f; //dire��o do salto
        private float jumpDistance = 6.43f; //dist�ncia do salto
        private float time;  //valor de tempo para calcular o seno
        private float jumpEffectY; //efeito de pulo para o alto
        private float jumpEffectX; //efeito de pulo para a dire��o
        private bool jumpKick = false; //Flying kick
        private bool jumpKicked = false; //Saltou e chutou
        public Vector2 positionAtJump = Vector2.Zero; //Posi��o antes de saltar

        private float standingTime = 0f; //Valor de tempo para calcular tempo parado

        private float movingTime; //valor de tempo para calcular movimento
        private int framePersistance; //Conta 3 e muda frame
        private bool movingFrame = false; //Moving frame

        private float attackingTime = 0.25f; //valor de tempo para calcular ataques
        private bool hickKicked = false; //Determina se o personagem deu chute alto

        public Texture2D lastAttack = null;

        //Posi��o hitbox
        private Vector2 hitbox = Vector2.One;

        //Machine states
        public enum CharacterState { Null, Standing, Moving, Crouching, Jumping, Punching, Kicking, Dead }; //Nenhum estado, parado, movendo-se, abaixando-se, saltando, socando, chutando, morto

        //Current State
        public static CharacterState previousState = CharacterState.Null;
        public static CharacterState currentState = CharacterState.Null;

        //Posi��o antes de atacar
        private float previousPosition = 0f;

        public Human(Vector2 initPos, Vector2 size)
            : base(initPos, size)
        {
            instance = this;

            //New position
            EnterCharacterState(CharacterState.Standing);

            speed *= 2;
        }

        public Vector2 GetHitboxPosition(Vector2 hitbox)
        {
            float hitX = 0f;

            if (Enemy.instance.pos.X > pos.X)
                hitX += 55f;
            else
                hitX -= 55f;

            if (GetSprite().Equals(World.playerHighKickTexture)) //Chute alto
            {
                lastAttack = World.playerHighKickTexture;
                hitbox = new Vector2(pos.X + hitX, (pos.Y - World.playerTexture.Height) + 40f);
            }
            else if (GetSprite().Equals(World.playerPunchTexture)) //Soco
            {
                lastAttack = World.playerPunchTexture;
                hitbox = new Vector2(pos.X + hitX, pos.Y - (World.playerTexture.Height / 2));
            }
            else if (GetSprite().Equals(World.playerLowPunchTexture)) //Soco baixo
            {
                lastAttack = World.playerLowPunchTexture;
                hitbox = new Vector2(pos.X + hitX, pos.Y - (World.playerTexture.Height / 3));
            }
            else if (GetSprite().Equals(World.playerLowKickTexture)) //Chute baixo
            {
                lastAttack = World.playerLowKickTexture;
                hitbox = new Vector2(pos.X + hitX, pos.Y);
            }
            else if (GetSprite().Equals(World.playerMediumKickTexture)) //Chute m�dio
            {
                lastAttack = World.playerMediumKickTexture;
                hitbox = new Vector2(pos.X + hitX, pos.Y);
            }
            else if (GetSprite().Equals(World.playerFlyingKickTexture)) //Chute no salto
            {
                lastAttack = World.playerFlyingKickTexture;
                hitbox = new Vector2(pos.X + hitX, pos.Y);
            }

            return hitbox;
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
                    {
                        if (jumpKick)
                            currentTexture = World.playerFlyingKickTexture;
                        else
                            currentTexture = World.playerJumpingTexture;
                    }
                    break;

                case CharacterState.Punching:
                    {
                        if (previousState.Equals(CharacterState.Standing))
                            currentTexture = World.playerPunchTexture;

                        else if (previousState.Equals(CharacterState.Crouching))
                            currentTexture = World.playerLowPunchTexture;

                        else if (previousState.Equals(CharacterState.Moving) && movingFrame)
                            currentTexture = World.playerMovingTexture;

                        else
                            currentTexture = World.playerTexture;
                    }
                    break;

                case CharacterState.Kicking:
                    {
                        if ((previousState.Equals(CharacterState.Moving) && (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.Right))) ||
                            hickKicked)
                            currentTexture = World.playerHighKickTexture;

                        else if (previousState.Equals(CharacterState.Standing))
                            currentTexture = World.playerMediumKickTexture;

                        else if (previousState.Equals(CharacterState.Crouching))
                            currentTexture = World.playerLowKickTexture;

                        else if (previousState.Equals(CharacterState.Moving) && movingFrame)
                            currentTexture = World.playerMovingTexture;

                        else
                            currentTexture = World.playerTexture;
                    }
                    break;

                case CharacterState.Dead:
                    currentTexture = World.playerDeadTexture;
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

        private SpriteEffects VirarImagem()
        {
            SpriteEffects efeito = SpriteEffects.None;

            if (pos.X <= Enemy.instance.pos.X && positionAtJump.Equals(Vector2.Zero))
            {
                efeito = SpriteEffects.None;
            }
            else if (pos.X > Enemy.instance.pos.X && positionAtJump.Equals(Vector2.Zero))
            {
                efeito = SpriteEffects.FlipHorizontally;
            }
            else if (positionAtJump.X <= Enemy.instance.pos.X)
            {
                efeito = SpriteEffects.None;
            }
            else if (positionAtJump.X > Enemy.instance.pos.X)
            {
                efeito = SpriteEffects.FlipHorizontally;
            }

            return efeito;
        }

        public override void Draw(GameTime gameTime)
        {
            Texture2D sprite = GetSprite();

            World.spriteBatch.Draw(sprite,
                pos,
                null,
                Color.White,
                0.0f,
                new Vector2(sprite.Width / 2f,
                            sprite.Height), //pivot
                Vector2.One, //scale
                VirarImagem(),
                0.1f
            );

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
                    {
                        positionAtJump = pos;
                    }
                    break;

                case CharacterState.Punching:
                    {
                        if (!previousState.Equals(CharacterState.Moving))
                        {
                            previousPosition = pos.X;

                            if (Enemy.instance.pos.X > pos.X)
                                pos.X += 70;
                            else
                                pos.X -= 70;

                            World.entities.Add(new Hit(this, GetHitboxPosition(hitbox), GetDir(), new Vector2(32, 32)));
                        }
                    }
                    break;

                case CharacterState.Kicking:
                    {
                        if (!previousState.Equals(CharacterState.Moving) || GetSprite().Equals(World.playerHighKickTexture))
                        {
                            if (previousState.Equals(CharacterState.Moving))
                            {
                                hickKicked = true;
                                previousState = CharacterState.Standing;
                            }

                            previousPosition = pos.X;

                            if (Enemy.instance.pos.X > pos.X)
                                pos.X += 70;
                            else
                                pos.X -= 70;

                            World.entities.Add(new Hit(this, GetHitboxPosition(hitbox), GetDir(), new Vector2(32, 32)));
                        }
                    }
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
                        if (Lifebar.instance.remainingPlayerLife() > 0)
                        {
                            if (standingTime > 0)
                                standingTime -= deltaTime;
                            else
                            {
                                if (Keyboard.GetState().IsKeyDown(Keys.Right) ||
                                    Keyboard.GetState().IsKeyDown(Keys.Left))
                                {
                                    EnterCharacterState(CharacterState.Moving);
                                }

                                if (Keyboard.GetState().IsKeyDown(Keys.Left) &&
                                    Keyboard.GetState().IsKeyDown(Keys.Up) &&
                                    pos.Y <= size.Y + (768f - size.Y))
                                {
                                    jumpDirection = -1;
                                    EnterCharacterState(CharacterState.Jumping);
                                }

                                else if (Keyboard.GetState().IsKeyDown(Keys.Right) &&
                                         Keyboard.GetState().IsKeyDown(Keys.Up) &&
                                         pos.Y <= size.Y + (768f - size.Y))
                                {
                                    jumpDirection = 1;
                                    EnterCharacterState(CharacterState.Jumping);
                                }

                                else if (Keyboard.GetState().IsKeyDown(Keys.Up) &&
                                         pos.Y <= size.Y + (768f - size.Y))
                                {
                                    jumpDirection = 0;
                                    EnterCharacterState(CharacterState.Jumping);
                                }

                                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                                {
                                    EnterCharacterState(CharacterState.Crouching);
                                }

                                if (Keyboard.GetState().IsKeyDown(Keys.X) &&
                                    Keyboard.GetState().IsKeyUp(Keys.Z))
                                {
                                    previousState = currentState;
                                    EnterCharacterState(CharacterState.Punching);
                                }

                                if (Keyboard.GetState().IsKeyDown(Keys.Z) &&
                                    Keyboard.GetState().IsKeyUp(Keys.X))
                                {
                                    previousState = currentState;
                                    EnterCharacterState(CharacterState.Kicking);
                                }
                            }
                        }
                        else
                            EnterCharacterState(CharacterState.Dead);
                    }
                    break;

                case CharacterState.Moving:
                    {
                        GetDir();

                        if (Lifebar.instance.remainingPlayerLife() > 0)
                        {
                            if (Keyboard.GetState().IsKeyDown(Keys.Z) &&
                                (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.Right)))
                            {
                                previousState = currentState;
                                EnterCharacterState(CharacterState.Kicking);
                            }

                            else if (movingTime < 0.100)
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
                                movingTime += deltaTime;
                            }

                            else
                                EnterCharacterState(CharacterState.Standing);
                        }
                        else
                            EnterCharacterState(CharacterState.Dead);
                    }
                    break;

                case CharacterState.Crouching:
                    {
                        if (Lifebar.instance.remainingPlayerLife() > 0)
                        {
                            if (standingTime > 0)
                                standingTime -= deltaTime;
                            else
                            {
                                if (Keyboard.GetState().IsKeyUp(Keys.Down))
                                {
                                    EnterCharacterState(CharacterState.Standing);
                                }

                                if (currentState.Equals(CharacterState.Crouching) &&
                                    Keyboard.GetState().IsKeyDown(Keys.X))
                                {
                                    previousState = currentState;
                                    EnterCharacterState(CharacterState.Punching);
                                }

                                if (currentState.Equals(CharacterState.Crouching) &&
                                    Keyboard.GetState().IsKeyDown(Keys.Z))
                                {
                                    previousState = currentState;
                                    EnterCharacterState(CharacterState.Kicking);
                                }
                            }
                        }
                        else
                            EnterCharacterState(CharacterState.Dead);
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
                            if (Keyboard.GetState().IsKeyDown(Keys.Z) && attackingTime > 0f && jumpKicked == false)
                            {
                                jumpKick = true;
                                jumpKicked = true;
                                attackingTime -= deltaTime;
                                World.entities.Add(new Hit(this, GetHitboxPosition(hitbox), GetDir(), new Vector2(32, 32)));
                            }
                            else if (attackingTime > 0f && jumpKicked)
                            {
                                attackingTime -= deltaTime;
                            }
                            else
                            {
                                jumpKick = false;
                                attackingTime = 0.25f;
                            }

                            time = (timerCounter / jumpTime) * (float)Math.PI;
                            jumpEffectY = jumpHeight * (float)Math.Sin(time);
                            pos.Y -= jumpEffectY;

                            timerCounter += deltaTime;
                        }
                        //Salto para a direita
                        else if (timerCounter >= 0f && jumpDirection == 1)
                        {
                            if (Keyboard.GetState().IsKeyDown(Keys.Z) && attackingTime > 0f && jumpKicked == false)
                            {
                                jumpKick = true;
                                jumpKicked = true;
                                attackingTime -= deltaTime;
                                World.entities.Add(new Hit(this, GetHitboxPosition(hitbox), GetDir(), new Vector2(32, 32)));
                            }
                            else if (attackingTime > 0f && jumpKicked)
                            {
                                attackingTime -= deltaTime;
                            }
                            else
                            {
                                jumpKick = false;
                                attackingTime = 0.25f;
                            }

                            time = (timerCounter / jumpTime) * (float)Math.PI;
                            jumpEffectY = jumpHeight * (float)Math.Sin(time);
                            jumpEffectX = jumpDistance * (float)Math.Sin(time);
                            pos.Y -= jumpEffectY;
                            pos.X += jumpDistance;

                            timerCounter += deltaTime;

                            if (pos.X <= size.X / 2f + 96f || pos.X >= size.X / 2f + (928f - size.X))
                            {
                                jumpDirection *= -1;
                            }
                        }
                        //Salto para a esquerda
                        else if (timerCounter >= 0f && jumpDirection == -1)
                        {
                            if (Keyboard.GetState().IsKeyDown(Keys.Z) && attackingTime > 0f && jumpKicked == false)
                            {
                                jumpKick = true;
                                jumpKicked = true;
                                attackingTime -= deltaTime;
                                World.entities.Add(new Hit(this, GetHitboxPosition(hitbox), GetDir(), new Vector2(32, 32)));
                            }
                            else if (attackingTime > 0f && jumpKicked)
                            {
                                attackingTime -= deltaTime;
                            }
                            else
                            {
                                jumpKick = false;
                                attackingTime = 0.25f;
                            }

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

                case CharacterState.Punching:
                    {
                        if (!previousState.Equals(CharacterState.Moving))
                        {
                            if (attackingTime > 0f)
                            {
                                attackingTime -= deltaTime;
                            }
                            else
                            {
                                pos.X = previousPosition;
                                EnterCharacterState(previousState);
                            }
                        }
                        else
                            EnterCharacterState(previousState);
                    }
                    break;

                case CharacterState.Kicking:
                    {
                        if (!previousState.Equals(CharacterState.Moving) || hickKicked)
                        {
                            if (attackingTime > 0f)
                            {
                                attackingTime -= deltaTime;
                            }
                            else
                            {
                                pos.X = previousPosition;
                                EnterCharacterState(previousState);
                            }
                        }
                        else
                            EnterCharacterState(previousState);
                    }
                    break;

                case CharacterState.Dead:
                    {
                        if (Lifebar.instance.remainingPlayerLife() > 0)
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

                case CharacterState.Crouching:
                    { }
                    break;

                case CharacterState.Jumping:
                    {
                        timerCounter = 0f;
                        jumpKicked = false;
                        positionAtJump = Vector2.Zero;
                    }
                    break;

                case CharacterState.Punching:
                    {
                        standingTime = 0.2f;
                        attackingTime = 0.25f;
                    }
                    break;

                case CharacterState.Kicking:
                    {
                        standingTime = 0.2f;
                        attackingTime = 0.25f;
                        hickKicked = false;
                    }
                    break;

                case CharacterState.Dead:
                    { };
                    break;
            }
        }
    }
}