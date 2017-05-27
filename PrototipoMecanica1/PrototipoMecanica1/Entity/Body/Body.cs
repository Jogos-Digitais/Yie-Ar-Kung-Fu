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
  public class Body : Entity
  {
    public Vector2 pos   = new Vector2(320, 240);
    public Vector2 size  = new Vector2(32, 32);
    public float   speed = 100f;

    public Body(Vector2 initPos)
    {
      pos = initPos;
    }

    public float GetRadius()
    {
      return Math.Max(size.X, size.Y) / 2f;
    }

    public Vector2 GetMin() { return pos - size / 2; }

    public Vector2 GetMax() { return pos + size / 2; }

    public virtual Vector2 GetDir()
    {
      return Vector2.Zero;
    }

    public virtual Texture2D GetSprite()
    {
      return null;
    }

    public bool TestPoint(Vector2 testPos)
    {
      return (testPos.X > (pos.X - size.X / 2f)) &&
             (testPos.Y > (pos.Y - size.Y / 2f)) &&
             (testPos.X < (pos.X + size.X / 2f)) &&
             (testPos.Y < (pos.Y + size.Y / 2f));
    }

    public override void Update(GameTime gameTime)
    {
      float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

      Vector2 dir = GetDir();

      float s = dir.Length();
      if (s > 0)
        dir = dir / s;

      //simulate physics independently in two axis
      for (int axis = 0; axis < 2; axis++)
      {
        Vector2 bkpPos = pos; //save current position

        if (axis == 0)
          pos += new Vector2(dir.X, 0f) * dt * speed; //only X
        else
          pos += new Vector2(0f, dir.Y) * dt * speed; //only Y

        Entity collider = null;

        Vector2 myMin = GetMin();
        Vector2 myMax = GetMax();

        //test collision against all world entities
        foreach (Entity e in World.entities)
        {
          if ((e != this) && //not myself?
              (IgnoreCollision(e) == false) && //ignore collision with other?
              (e.IgnoreCollision(this) == false) && //other ignores collision with me?
              e.TestCollisionRect(myMin, myMax)) //is colliding against other entity?
          {
            collider = e; //collision detected!
            CollisionDetected(e);
            break;
          }
        }

        if (collider != null) //undo movement!
          pos = bkpPos;
      }
    }

    public override void Draw(GameTime gameTime)
    {
      Texture2D sprite = GetSprite();
      if (sprite == null)
        return;

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
        0.0f
      );
    }
	
	public override bool TestCollisionRect(Vector2 testMin, Vector2 testMax)
    {
      Vector2 myMin = GetMin();
      Vector2 myMax = GetMax();

      //test collision between my rectangle and other's rectangle
      if ((testMax.X >= myMin.X) && (testMax.Y >= myMin.Y) &&
          (testMin.X <= myMax.X) && (testMin.Y <= myMax.Y))
        return true;
      else
        return false;
    }
  }
}