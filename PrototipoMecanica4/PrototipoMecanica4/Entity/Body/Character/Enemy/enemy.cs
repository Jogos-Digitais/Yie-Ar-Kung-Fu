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

        private float safeZone = 0f; //Distância segura do player, para aproximação
        private float dangerZone = 0f; //Distância perigosa do player, para afastar-se
        private float advanceZone = 0f; //Distância de avanço da IA, vai alternando
        private float runDistance = 0f; //Distância de corrida, multiplicar por pontos de vida perdidos

        float movingTime; //valor de tempo para calcular movimento
        int framePersistance; //Conta 3 e muda frame
        bool movingFrame = false; //Moving frame

        float attackingTime = 0.25f; //valor de tempo para calcular ataques
        int attackingFramePersistance; //Conta 3 e muda frame
        bool attackingFrame = false; //Attacking frame
        int attackingCombo = 0; //Attacking combo

        //Posição hitbox
        Vector2 hitbox = Vector2.One;

        //Machine states
        public enum CharacterState { Null, Standing, Moving, Advancing, Retreating, Attacking, Recoiling, Running, Dead }; //Nenhum estado, parado, movendo-se, avançando, recuando, atacando, absorvendo, correndo, morto

        //Current State
        public static CharacterState previousState = CharacterState.Null;
        public static CharacterState currentState = CharacterState.Null;

        public Enemy(Vector2 initPos, Vector2 size) : base(initPos, size)
        {
            instance = this;

            //New position
            EnterCharacterState(CharacterState.Standing);

            speed *= 2f;

            //Definir distâncias
            safeZone = 104 + (Human.instance.size.X / 2) + (size.X / 2);
            dangerZone = -4 + (Human.instance.size.X / 2) + (size.X / 2); //Nunca verificar com igual, sempre menor
            advanceZone = safeZone - (size.X / 2);
        }

        //TODO
        public Vector2 GetHitboxPosition(Vector2 hitbox)
        {
            float hitX = 0f;

            if (Human.instance.pos.X > pos.X)
                hitX += 85f;
            else
                hitX -= 85f;

            if (GetSprite().Equals(World.enemy001KickTexture)) //Chute
                hitbox = new Vector2(pos.X + hitX, pos.Y - 55f);
            else if (GetSprite().Equals(World.enemy001StaffAttackTexture)) //Bastão
                hitbox = new Vector2(pos.X + hitX, pos.Y - 55f);
            else if (GetSprite().Equals(World.enemy001LowStaffAttackTexture)) //Bastão baixo
                hitbox = new Vector2(pos.X + hitX, pos.Y - 55f);

            return hitbox;
        }

        public override Vector2 GetDir()
        {
            Body testBody = Human.instance;

            if (!currentState.Equals(CharacterState.Dead))
            {
                Vector2 positionToReturn = Vector2.Zero;

                switch (currentState)
                {
                    case CharacterState.Standing:
                    case CharacterState.Moving:
                        {
                            if (pos.X >= Human.instance.pos.X)
                                positionToReturn.X = testBody.pos.X - pos.X + safeZone; //+safedistance
                            else
                                positionToReturn.X = testBody.pos.X - pos.X - safeZone; //-safedistance
                        }
                        break;

                    case CharacterState.Advancing:
                        {
                            if (pos.X >= Human.instance.pos.X)
                                positionToReturn.X = testBody.pos.X - pos.X + dangerZone; //+safedistance
                            else
                                positionToReturn.X = testBody.pos.X - pos.X - dangerZone; //-safedistance
                        }
                        break;

                    case CharacterState.Retreating:
                        {

                        }
                        break;
                }

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

                case CharacterState.Attacking:
                    {
                        if (attackingFrame)
                            currentTexture = World.enemy001KickTexture;
                        else
                            currentTexture = World.enemy001PrepareAttackTexture;
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

        public void move(float deltaTime)
        {
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
                    pos += new Vector2(dir.X, 0f) * deltaTime * speed; //only X
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
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //if (currentState.Equals(CharacterState.Moving))
            //    move(deltaTime);

            UpdateCharacterState(gameTime);
        }

        private SpriteEffects VirarImagem()
        {
            SpriteEffects efeito = SpriteEffects.None;

            if (pos.X >= Human.instance.pos.X && Human.instance.positionAtJump.Equals(Vector2.Zero))
            {
                efeito = SpriteEffects.None;
            }
            else if (pos.X < Human.instance.pos.X && Human.instance.positionAtJump.Equals(Vector2.Zero))
            {
                efeito = SpriteEffects.FlipHorizontally;
            }
            else if (pos.X >= Human.instance.positionAtJump.X)
            {
                efeito = SpriteEffects.None;
            }
            else if (pos.X < Human.instance.positionAtJump.X)
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
                0.2f
            );

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

                //Safe zone
                World.spriteBatch.Draw(World.debugZones,
                  new Vector2((pos.X >= Human.instance.pos.X ? (Human.instance.pos.X - pos.X + safeZone) + (pos.X - size.X / 2) : (Human.instance.pos.X - pos.X - safeZone) + (pos.X + size.X / 2)), 518f),
                  null,
                  new Color(0.0f, 1.0f, 0.0f, 0.5f),
                  0.0f,
                  Vector2.Zero, //pivot
                  Vector2.One, //scale
                  SpriteEffects.None,
                  0.3f
                );

                //Advance zone
                World.spriteBatch.Draw(World.debugZones,
                  new Vector2((pos.X >= Human.instance.pos.X ? (Human.instance.pos.X - pos.X + advanceZone) + (pos.X - size.X / 2) : (Human.instance.pos.X - pos.X - advanceZone) + (pos.X + size.X / 2)), 518f),
                  null,
                  new Color(0.0f, 0.0f, 1.0f, 0.5f),
                  0.0f,
                  Vector2.Zero, //pivot
                  Vector2.One, //scale
                  SpriteEffects.None,
                  0.3f
                );

                //Danger zone
                World.spriteBatch.Draw(World.debugZones,
                    new Vector2((pos.X >= Human.instance.pos.X ? (Human.instance.pos.X - pos.X + dangerZone) + (pos.X - size.X / 2) : (Human.instance.pos.X - pos.X - dangerZone) + (pos.X + size.X / 2)), 518f),
                  null,
                  new Color(1.0f, 0.0f, 0.0f, 0.5f),
                  0.0f,
                  Vector2.Zero, //pivot
                  Vector2.One, //scale
                  SpriteEffects.None,
                  0.3f
                );

                World.spriteBatch.DrawString(World.fontNormal, "State: " + currentState.ToString() + Environment.NewLine + "Combo: " + attackingCombo, new Vector2(this.pos.X, this.pos.Y + 20f), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
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

                case CharacterState.Advancing:
                    { }
                    break;

                case CharacterState.Retreating:
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

            float distance = GetDir().X;

            switch (currentState)
            {
                case CharacterState.Standing:
                    {
                        if ((distance < -4 || distance > 4) && Lifebar.instance.remainingEnemyLife() >= 1)
                            EnterCharacterState(CharacterState.Moving);

                        else if (Lifebar.instance.remainingEnemyLife() <= 0)
                            EnterCharacterState(CharacterState.Dead);

                        else if (distance >= -3 && distance <= 3)
                            EnterCharacterState(CharacterState.Attacking);
                    }
                    break;

                case CharacterState.Moving:
                    {
                        move(deltaTime);

                        if (distance >= -4 && distance <= 4 && Lifebar.instance.remainingEnemyLife() >= 1)
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

                        else if (movingTime > 0.200)
                        {
                            movingTime = 0;
                            framePersistance = 0;
                        }
                    }
                    break;

                case CharacterState.Advancing:
                    {
                        move(deltaTime);

                        if (distance >= -4 && distance <= 4 && Lifebar.instance.remainingEnemyLife() >= 1)
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

                        else if (movingTime > 0.200)
                        {
                            movingTime = 0;
                            framePersistance = 0;
                        }
                    }
                    break;

                case CharacterState.Retreating:
                    { }
                    break;

                case CharacterState.Attacking:
                    {
                        if (Lifebar.instance.remainingEnemyLife() >= 1)
                        {
                            if (attackingTime > 0 && attackingCombo < 4)
                            {
                                if (attackingFrame && attackingFramePersistance == 10)
                                {
                                    attackingFrame = false;
                                }
                                else if (attackingFrame == false && attackingFramePersistance == 10)
                                {
                                    attackingFrame = true;
                                    attackingCombo++;

                                    World.entities.Add(new Hit(this, GetHitboxPosition(hitbox), GetDir(), new Vector2(32, 32)));
                                }

                                attackingFramePersistance++;
                                attackingTime -= deltaTime;
                            }

                            else if (attackingTime <= 0 && attackingCombo < 4)
                            {
                                attackingTime = 0.25f;
                                attackingFramePersistance = 0;
                            }

                            else if(attackingCombo == 4)
                            {
                                EnterCharacterState(CharacterState.Advancing);
                            }
                        }

                        else
                        {
                            EnterCharacterState(CharacterState.Dead);
                        }
                    }
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

                case CharacterState.Advancing:
                    {
                        movingTime = 0f;
                    }
                    break;

                case CharacterState.Retreating:
                    { }
                    break;

                case CharacterState.Attacking:
                    {
                        attackingTime = 0.25f;
                        attackingCombo = 0;
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