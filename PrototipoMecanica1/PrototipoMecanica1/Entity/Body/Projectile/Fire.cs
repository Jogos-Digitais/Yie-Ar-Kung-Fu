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

    public Fire(Character shooter, Vector2 initPos, Vector2 initDir) : base(initPos)
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
      return World.bulletTex;
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);

      List<Entity> entitiesToDie = new List<Entity>();

      foreach (Entity e in World.entities)
      {
        if ((e is Character) && (e != myShooter))
        {
          Character c = (Character)e;

          if (c.TestPoint(pos))
          {
            c.health -= damage;
            if (c.health <= 0)
            {
              entitiesToDie.Add(c);
            }
          }
        }
      }

      foreach(Entity e in entitiesToDie)
        World.entities.Remove(e);

      if (entitiesToDie.Count > 0)
        World.entities.Remove(this);

    }
  }
}
