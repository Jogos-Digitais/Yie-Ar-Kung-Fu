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
    public float visionRadius = 64f;

    public Enemy(Vector2 initPos) : base(initPos)
    {
      speed /= 2f;
    }

    public override Vector2 GetDir()
    {
      Body  nearestBody = null;
      float nearestDist = 9999.9f;

      foreach(Entity e in World.entities)
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
      return World.enemyTex;
    }

    public override void Draw(GameTime gameTime)
    {
      if (World.debugMode)
      {
        World.spriteBatch.Draw(World.debugCircleTex,
          pos,
          null,
          new Color(1.0f, 0.0f, 0.0f, 1.0f),
          0.0f,
          new Vector2(World.debugCircleTex.Width,
                      World.debugCircleTex.Height) / 2f, //pivot
          new Vector2(2 * visionRadius / World.debugCircleTex.Width,
                      2 * visionRadius / World.debugCircleTex.Height), //scale
          SpriteEffects.None,
          1.0f
        );
      }

      base.Draw(gameTime);
    }
  }
}