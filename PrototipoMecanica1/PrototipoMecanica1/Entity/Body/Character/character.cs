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
    public class Character : Body
    {
        public float health = 100f;

        public float fireRate = 10f; //hz

        float fireTimer = 0;

        float TC = 0; //Timer Counter
        float JT = 0.333f; //Jump Time
        float JH = 100f; //Jump height
        float t;  //valor de tempo para calcular o seno
        float dy; //efeito de pulo
        float dx; //direção do salto

         /* JT = 1;      Tempo de Pulo
          * JH = 100f;  100 pixels
          * TC = 
          * y-dy;
          *dy = JH * Math.Sin(t)
          *t = (TC/JT) * phi()
                      */

        Vector2 shootDir = new Vector2(1, 0);

        //Machine states
        public enum CharacterState { Null, Standing, Moving, Crouching, Jumping, Attacking, Recoiling, Dead }; //Nenhum estado, parado, movendo-se, saltando, atacando, recuando, morto

        //Current State
        public static CharacterState currentState = CharacterState.Null;

        public Character(Vector2 initPos, Vector2 size) : base(initPos, size)
        {
            //New position
            EnterCharacterState(CharacterState.Standing);
        }

        //States
        public static void EnterCharacterState(CharacterState newState)
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

        public static void UpdateCharacterState(GameTime gameTime)
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

        public static void DrawCharacterState(GameTime gameTime)
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

        public static void LeaveCharacterState()
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

        public virtual bool WantsToFire() { return false; }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);

            Vector2 dir = GetDir();
            if (dir.Length() > 0)
                shootDir = dir;

            if (fireTimer <= 0)
            {
                fireTimer = 1.0f / fireRate;

                if (WantsToFire())
                {
                    World.entities.Add(new Fire(this, pos, shootDir, new Vector2(32, 32)));
                }
            }
            else
            {
                fireTimer -= dt;
            }

            if (health <= 0.0f)
                World.entities.Remove(this);
        }

        public override bool IgnoreCollision(Entity other)
        {
            if (other is Enemy) //ignore collision against my enemy!
                return true;

            return false;
        }
    }
}