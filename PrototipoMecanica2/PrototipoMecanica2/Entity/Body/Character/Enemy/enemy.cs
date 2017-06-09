using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace PrototipoMecanica2
{
    public class Enemy : Character
    {
        static public Enemy instance = null;

        public float visionRadius;

        public Enemy(Vector2 initPos, Vector2 size)
            : base(initPos, size)
        {
            instance = this;

            speed /= 2f;
            visionRadius = size.X * 2;
        }

        public override Vector2 GetDir()
        {
            Body nearestBody = null;
            float nearestDist = 9999.9f;

            foreach (Entity e in World.entities)
            {
                if (e is Human)
                {
                    Body testBody = (Body)e;

                    float testDist = Vector2.Distance(this.pos, testBody.pos) - testBody.GetRadius();

                    if ((testDist <= visionRadius) && (testDist < nearestDist))
                    {
                        nearestDist = testDist;
                        nearestBody = testBody;
                    }
                }
            }

            if (nearestBody != null)
            {
                if (pos.X >= Human.instance.pos.X)
                    return (nearestBody.pos + Vector2.One) - pos;
                else
                    return (nearestBody.pos - Vector2.One) - pos;
            }

            return Vector2.Zero;
        }

        public override Texture2D GetSprite()
        {
            return World.enemy001Texture;
        }

        public override void Draw(GameTime gameTime)
        {
            Texture2D sprite = GetSprite();

            if (pos.X >= Human.instance.pos.X)
            {
                World.spriteBatch.Draw(sprite,
                    pos,
                    null,
                    Color.White,
                    0.0f,
                    new Vector2(sprite.Width / 2f,
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
                    new Vector2(sprite.Width / 2f,
                                sprite.Height), //pivot
                    Vector2.One, //scale
                    SpriteEffects.FlipHorizontally,
                    0.1f
                );
            }

            if (World.debugMode)
            {
                World.spriteBatch.Draw(World.debugCircleTex,
                  pos,
                  null,
                  new Color(1.0f, 0.0f, 0.0f, 0.5f),
                  0.0f,
                  new Vector2(World.debugCircleTex.Width,
                              World.debugCircleTex.Height) / 2f, //pivot
                  new Vector2(2f * visionRadius / (float)World.debugCircleTex.Width,
                              2f * visionRadius / (float)World.debugCircleTex.Height), //scale
                  SpriteEffects.None,
                  0.3f
                );

                World.spriteBatch.DrawString(World.fontNormal, "State: Undefined", new Vector2(this.pos.X, this.pos.Y + 20f), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            }
        }
    }
}