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
    public class Human : Character
    {
        public Human(Vector2 initPos, Vector2 size) : base(initPos, size)
        {
            speed *= 2;
        }

        public override Vector2 GetDir()
        {
            Vector2 dir = Vector2.Zero;

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                dir.X += 1.0f;

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                dir.X += -1.0f;

            //if (Keyboard.GetState().IsKeyDown(Keys.Up))
            //    dir.Y += -1.0f;

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                dir.Y += 1.0f;

            return dir;
        }

        public override Texture2D GetSprite()
        {
            return World.playerTexture;
        }

        public override bool WantsToFire()
        {
            return Keyboard.GetState().IsKeyDown(Keys.LeftControl);
        }
    }
}