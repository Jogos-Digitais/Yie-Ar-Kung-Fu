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
    public class Fire : Body
    {
        public float damage = 50f;

        public Vector2 dir = Vector2.Zero;

        public Character myShooter = null;

        public Fire(Character shooter, Vector2 initPos, Vector2 initDir, Vector2 size)
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

        public override void CollisionDetected(Entity other)
        {
            if (other is Character)
            {
                Character c = (Character)other;
                c.health -= damage;
            }
        }

        public override bool IgnoreCollision(Entity other)
        {
            if (other == myShooter) //ignore collision against my shooter!
                return true;

            if (other is Fire) //ignore collision against other bullets!
                return true;

            return false;
        }
    }
}