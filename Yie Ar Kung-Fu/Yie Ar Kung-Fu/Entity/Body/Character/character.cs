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
    public class Character : Body
    {
        //Vector2 shootDir = new Vector2(1, 0);

        public Character(Vector2 initPos, Vector2 size) : base(initPos, size)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        public override bool IgnoreCollision(Entity other)
        {
            if (other is Enemy) //ignore collision against my enemy!
                return true;

            return false;
        }
    }
}