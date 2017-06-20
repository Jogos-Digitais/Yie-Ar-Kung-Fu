﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace PrototipoMecanica4
{
    public class HitContact : Body
    {
        public Vector2 dir = Vector2.Zero;

        public Character myShooter = null;

        public float lifeTime = 0.25f;

        public HitContact(Character shooter, Vector2 initPos, Vector2 initDir, Vector2 size)
            : base(initPos, size)
        {
            myShooter = shooter;
            dir = initDir;
            speed *= 2;
            size /= 2;
        }

        public override Vector2 GetDir()
        {
            return dir;
        }

        public override Texture2D GetSprite()
        {
            if (myShooter.Equals(Human.instance))
                return World.playerHitTexture;
            else if (myShooter.Equals(Enemy.instance))
                return World.enemyHitTexture;
            else
                return World.fireHitTexture;
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            lifeTime -= deltaTime;

            if (lifeTime <= 0)
            {
                World.entities.Remove(this);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Texture2D sprite = GetSprite();

            if (Human.instance.pos.X <= Enemy.instance.pos.X)
            {
                World.spriteBatch.Draw(sprite,
                  pos,
                  null,
                  Color.White,
                  0.0f,
                  new Vector2(sprite.Width / 2,
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
                  new Vector2(sprite.Width / 2,
                              sprite.Height), //pivot
                  Vector2.One, //scale
                  SpriteEffects.FlipHorizontally,
                  0.1f
                );
            }

            if (World.debugMode)
            {
                World.spriteBatch.DrawString(World.fontNormal, "Expire time: " + lifeTime, new Vector2(this.pos.X, this.pos.Y + 50f), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            }
        }

        public override bool IgnoreCollision(Entity other)
        {
            if (other == myShooter) //ignore collision against my shooter!
                return true;

            if (other is Hit) //ignore collision against other hits!
                return true;

            return false;
        }
    }
}
