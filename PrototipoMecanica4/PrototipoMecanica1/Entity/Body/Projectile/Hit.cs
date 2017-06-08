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
    public class Hit : Body
    {
        public float damage = 1f;

        public Vector2 dir = Vector2.Zero;

        public Character myShooter = null;

        public float lifeTime = 5f;

        public Hit(Character shooter, Vector2 initPos, Vector2 initDir, Vector2 size)
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
            return World.fireTexture;
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //base.Update(gameTime);

            lifeTime -= deltaTime;

            if (lifeTime <= 0)
            {
                World.entities.Remove(this);
            }

            Vector2 myMin = GetMin();
            Vector2 myMax = GetMax();

            foreach (Entity e in World.entities)
            {
                if ((e != this) && //not myself?
                    (e != myShooter) && //not my shooter?
                    (e is Character) && //is character?
                    e.TestCollisionRect(myMin, myMax)) //is colliding against other entity?
                {
                    Character opponent = (Character)e;




                    break;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (World.debugMode)
            {
                World.spriteBatch.DrawString(World.fontNormal, "Expire time: " + lifeTime, new Vector2(this.pos.X, this.pos.Y + 50f), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            }

            base.Draw(gameTime);
        }

    }
}