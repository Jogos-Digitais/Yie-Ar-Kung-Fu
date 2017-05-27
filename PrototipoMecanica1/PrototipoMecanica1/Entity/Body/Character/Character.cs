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
  public class Character : Body
  {
    public float health = 100f;

    public float fireRate = 10f; //hz

    float fireTimer = 0;

    Vector2 shootDir = new Vector2(1, 0);

    public Character(Vector2 initPos) : base(initPos) { }

    public virtual bool WantsToFire() { return false; }

    public override void Update(GameTime gameTime)
    {
      float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

      base.Update(gameTime);

      Vector2 dir = GetDir();
      if (dir.Length() > 0)
        shootDir = dir;

      if (fireTimer <= 0)
      {
        fireTimer = 1.0f / fireRate;

        if (WantsToFire())
        {
          World.entities.Add(new Fire(this, pos, shootDir));
        }
      }
      else
      {
        fireTimer -= dt;
      }
    }
  }
}
