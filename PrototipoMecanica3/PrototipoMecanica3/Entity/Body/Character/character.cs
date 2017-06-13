using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace PrototipoMecanica3
{
    public class Character : Body
    {
        public float health = 100f;

        public float fireRate = 10f; //hz

        float fireTimer = 0;

        Vector2 shootDir = new Vector2(1, 0);

        public Character(Vector2 initPos, Vector2 size) : base(initPos, size)
        {
            
        }

        public virtual bool WantsToFire() { return false; }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);

            if (fireTimer <= 0)
            {
                fireTimer = 1.0f / fireRate;

                if (WantsToFire())
                {
                    World.entities.Add(new Hit(this, pos, shootDir, new Vector2(32, 32)));
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