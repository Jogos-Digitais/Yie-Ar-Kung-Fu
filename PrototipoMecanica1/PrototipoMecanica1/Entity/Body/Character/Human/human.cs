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

            if (Keyboard.GetState().IsKeyDown(Keys.Up) && pos.Y == size.Y / 2f + (768f - size.Y))
                dir.Y += -1.0f;

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                dir.Y += 1.0f;

            return dir;
        }

        public override Texture2D GetSprite()
        {
            return World.playerTexture;
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && pos.Y == size.Y / 2f + (768f - size.Y))
            {
                while (pos.Y != (size.Y / 2f) + 452f)
                {
                    pos.Y -= 1;
                }
            }

            if (pos.Y < size.Y / 2f + (768f - size.Y))
                pos.Y += 10;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Texture2D sprite = GetSprite();

            World.spriteBatch.Draw(sprite,
              pos,
              null,
              Color.White,
              0.0f,
              new Vector2(sprite.Width,
                          sprite.Height) / 2f, //pivot
              new Vector2(size.X / sprite.Width,
                          size.Y / sprite.Height), //scale
              SpriteEffects.None,
              0.1f
            );

            if (World.debugMode)
            {
                World.spriteBatch.Draw(World.debugRectangleTex,
                  pos,
                  null,
                  new Color(0.0f, 1.0f, 0.0f, 0.5f),
                  0.0f,
                  new Vector2(World.debugRectangleTex.Width,
                              World.debugRectangleTex.Height) / 2f, //pivot
                  Vector2.One, //scale
                  SpriteEffects.None,
                  0.3f
                );
            }
        }

        public override bool WantsToFire()
        {
            return Keyboard.GetState().IsKeyDown(Keys.LeftControl);
        }
    }
}