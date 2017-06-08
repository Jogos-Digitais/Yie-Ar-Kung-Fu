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
    public class Enemy : Character
    {
        public float visionRadius;

        public Enemy(Vector2 initPos, Vector2 size)
            : base(initPos, size)
        {
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
                return nearestBody.pos - pos;
            }

            return Vector2.Zero;
        }

        public override Texture2D GetSprite()
        {
            return World.enemy001Texture;
        }

        public override void Draw(GameTime gameTime)
        {
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
            }

            base.Draw(gameTime);
        }
    }
}