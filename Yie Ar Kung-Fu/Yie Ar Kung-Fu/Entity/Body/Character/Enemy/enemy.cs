using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace YieArKungFu
{
    public class Enemy : Character
    {
        static public Enemy instance = null;

        private float safeZone = 0f; //Distância segura do player, para aproximação
        private float dangerZone = 0f; //Distância perigosa do player, para afastar-se
        private float advanceZone = 0f; //Distância de avanço da IA, vai alternando
        private float runZone = 0f; //Distância de corrida, multiplicar por pontos de vida perdidos
        private float safeDistance = 0f; //Distância segura atual

        private float standingTime = 0f; //Valor de tempo para calcular tempo parado

        private float movingTime; //valor de tempo para calcular movimento
        private int framePersistance; //Conta 10 e muda frame
        private bool movingFrame = false; //Moving frame

        private float attackingTime = 0.25f; //valor de tempo para calcular ataques
        private int attackingCombo = 0; //Attacking combo

        public bool hasBeenAttacked = false; //Indica se o inimigo recebeu danos, para usar em running

        //Último tipo de ataque do inimigo
        private int attackType = 0; //Tipo de ataque, -1 = soco, 1 = chute, 0 = outro

        //Posição hitbox
        private Vector2 hitbox = Vector2.One;

        //Machine states
        public enum CharacterState { Null, Standing, Moving, Advancing, PreparingAttack, Attacking, Running, Dead }; //Nenhum estado, parado, movendo-se, avançando, preparando ataque, atacando, correndo, morto

        //Current State
        public static CharacterState currentState = CharacterState.Null;

        public Enemy(Vector2 initPos, Vector2 size)
            : base(initPos, size)
        {
            instance = this;

            //New position
            EnterCharacterState(CharacterState.Standing);

            speed *= 2f;

            //Definir distâncias
            safeZone = 104 + (Human.instance.size.X / 2) + (size.X / 2);
            dangerZone = -4 + (Human.instance.size.X / 2) + (size.X / 2); //Nunca verificar com igual, sempre menor
            advanceZone = safeZone; //Distância inicial igual a safeZone
            runZone = safeZone;
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
                hitbox = new Vector2(pos.X + hitX, pos.Y - 60f);
            else if (GetSprite().Equals(World.enemy001LowStaffAttackTexture)) //Bastão baixo
                hitbox = new Vector2(pos.X + hitX, pos.Y - 35f);

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
                                positionToReturn.X = testBody.pos.X - pos.X + safeZone; //+safeZone
                            else
                                positionToReturn.X = testBody.pos.X - pos.X - safeZone; //-safeZone
                        }
                        break;

                    case CharacterState.Advancing:
                        {
                            if (pos.X >= Human.instance.pos.X)
                                positionToReturn.X = testBody.pos.X - pos.X + advanceZone; //+advanceZone
                            else
                                positionToReturn.X = testBody.pos.X - pos.X - advanceZone; //-advanceZone
                        }
                        break;

                    case CharacterState.Running:
                        {
                            if (pos.X >= Human.instance.pos.X)
                                positionToReturn.X = testBody.pos.X - pos.X + runZone;
                            else
                                positionToReturn.X = testBody.pos.X - pos.X - runZone;
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
                case CharacterState.Advancing:
                case CharacterState.Running:
                    {
                        if (movingFrame)
                            currentTexture = World.enemy001MovingTexture;
                        else
                            currentTexture = World.enemy001Texture;
                    }
                    break;

                case CharacterState.PreparingAttack:
                    {
                        if (standingTime > 0)
                            currentTexture = World.enemy001Texture;
                        else
                            currentTexture = World.enemy001PrepareAttackTexture;
                    }
                    break;

                case CharacterState.Attacking:
                    {
                        if (attackType == 0) //Se último ataque foi diferente de soco e chute
                            currentTexture = World.enemy001KickTexture;
                        else if (attackType == -1) //Se último ataque foi soco
                            currentTexture = World.enemy001StaffAttackTexture;
                        else if (attackType == 1) //Se último ataque foi chute
                            currentTexture = World.enemy001LowStaffAttackTexture;
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

                //Run zone
                World.spriteBatch.Draw(World.debugZones,
                  new Vector2((pos.X >= Human.instance.pos.X ? (Human.instance.pos.X - pos.X + runZone) + (pos.X - size.X / 2) : (Human.instance.pos.X - pos.X - runZone) + (pos.X + size.X / 2)), 518f),
                  null,
                  new Color(0.5f, 0.5f, 0f, 0.5f),
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
                    {
                        standingTime = 0.2f;
                    }
                    break;

                case CharacterState.Moving:
                    {
                        attackingCombo = 0;
                        advanceZone = safeZone;
                    }
                    break;

                case CharacterState.Advancing:
                    {
                        if (advanceZone > dangerZone)
                            advanceZone -= size.X / 2;
                    }
                    break;

                case CharacterState.PreparingAttack:
                    {
                        if (advanceZone < dangerZone)
                            advanceZone = dangerZone;
                    }
                    break;

                case CharacterState.Attacking:
                    {
                        attackingCombo++;

                        World.entities.Add(new Hit(this, GetHitboxPosition(hitbox), GetDir(), new Vector2(32, 32)));
                    }
                    break;

                case CharacterState.Running:
                    {
                        //Triplicar velocidade
                        speed = speed * 3f;

                        int multiplicadorDistancia = 9 - Lifebar.instance.remainingEnemyLife();

                        if (multiplicadorDistancia > 3)
                            multiplicadorDistancia = 3;

                        runZone = runZone + (multiplicadorDistancia * 55f);
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

            float distance = GetDir().X;

            switch (currentState)
            {
                case CharacterState.Standing:
                    {
                        if (Lifebar.instance.remainingEnemyLife() > 0)
                        {
                            if (standingTime > 0)
                                standingTime -= deltaTime;
                            else
                            {
                                if (distance < -4 || distance > 4)
                                    EnterCharacterState(CharacterState.Moving);
                                else if (distance >= -3 && distance <= 3)
                                    EnterCharacterState(CharacterState.PreparingAttack);
                            }
                        }
                        else
                            EnterCharacterState(CharacterState.Dead);
                    }
                    break;

                case CharacterState.Moving:
                    {
                        if (Lifebar.instance.remainingEnemyLife() > 0)
                        {
                            move(deltaTime);

                            safeDistance = Vector2.Distance(Human.instance.pos, pos);

                            if (distance >= -4 && distance <= 4)
                            {
                                EnterCharacterState(CharacterState.Standing);
                            }

                            else if (safeDistance < safeZone)
                            {
                                advanceZone = advanceZone -= size.X / 2;
                            
                                EnterCharacterState(CharacterState.PreparingAttack);
                            }

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
                        else
                            EnterCharacterState(CharacterState.Dead);
                    }
                    break;

                case CharacterState.Advancing:
                    {
                        if (Lifebar.instance.remainingEnemyLife() > 0)
                        {
                            move(deltaTime);

                            safeDistance = Vector2.Distance(Human.instance.pos, pos);

                            if (distance >= -4 && distance <= 4)
                            {
                                EnterCharacterState(CharacterState.PreparingAttack);
                            }

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
                        else
                            EnterCharacterState(CharacterState.Dead);
                    }
                    break;

                case CharacterState.PreparingAttack:
                    {
                        if (Lifebar.instance.remainingEnemyLife() > 0)
                        {
                            float safeDistance = Vector2.Distance(Human.instance.pos, pos);

                            if (standingTime > 0)
                            {
                                standingTime -= deltaTime;
                            }
                            else
                            {
                                if (!Human.instance.GetSprite().Equals(World.playerPunchTexture) &&
                                    !Human.instance.GetSprite().Equals(World.playerMediumKickTexture))
                                    attackType = 0;
                                else if (Human.instance.GetSprite().Equals(World.playerPunchTexture))
                                    attackType = -1;
                                else if (Human.instance.GetSprite().Equals(World.playerMediumKickTexture))
                                    attackType = 1;

                                if (safeDistance > safeZone + 4)
                                    EnterCharacterState(CharacterState.Moving);
                                else if (attackingCombo < 4)
                                    EnterCharacterState(CharacterState.Attacking);
                                else if (attackingCombo == 4)
                                    EnterCharacterState(CharacterState.Advancing);
                            }
                        }
                        else
                            EnterCharacterState(CharacterState.Dead);
                    }
                    break;

                case CharacterState.Attacking:
                    {
                        if (Lifebar.instance.remainingEnemyLife() > 0)
                        {
                            if (attackingTime > 0 && attackingCombo < 4)
                            {
                                attackingTime -= deltaTime;
                            }

                            else if (attackingTime <= 0 && attackingCombo < 4)
                            {
                                EnterCharacterState(CharacterState.PreparingAttack);
                            }

                            else if (attackingCombo == 4)
                            {
                                attackingCombo = 0;

                                EnterCharacterState(CharacterState.Advancing);
                            }
                        }
                        else
                            EnterCharacterState(CharacterState.Dead);
                    }
                    break;

                case CharacterState.Running:
                    { 
                        if (Lifebar.instance.remainingEnemyLife() > 0)
                        {
                            move(deltaTime);

                            if (distance >= -6 && distance <= 6)
                            {
                                EnterCharacterState(CharacterState.Standing);
                            }

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
                        else
                            EnterCharacterState(CharacterState.Dead);
                    }
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

                case CharacterState.PreparingAttack:
                    {
                        standingTime = 0.2f;
                    }
                    break;

                case CharacterState.Attacking:
                    {
                        attackingTime = 0.25f;
                    }
                    break;

                case CharacterState.Running:
                    {
                        //Normalizar velocidade
                        speed = speed / 3f;
                    }
                    break;

                case CharacterState.Dead:
                    { };
                    break;
            }
        }
    }
}