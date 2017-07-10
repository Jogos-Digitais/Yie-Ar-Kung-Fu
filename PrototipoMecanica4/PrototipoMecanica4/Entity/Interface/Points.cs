using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototipoMecanica4
{
    public class Points : Entity
    {
        public float lifeTime = 0.50f;
        public Vector2 position = Vector2.Zero;

        public Points()
        {
            position = Enemy.instance.pos;

            position.Y = position.Y - 192f;
        }

        public static Texture2D GetSprite()
        {
            Texture2D sprite = null;

            if (Human.instance.lastAttack == null)
                sprite = World.points0Texture;

            else if (Human.instance.lastAttack == World.playerPunchTexture ||
                Human.instance.lastAttack == World.playerLowPunchTexture)
                sprite = World.points100Texture;

            else if (Human.instance.lastAttack == World.playerMediumKickTexture ||
                     Human.instance.lastAttack == World.playerHighKickTexture)
                sprite = World.points200Texture;

            else if (Human.instance.lastAttack == World.playerLowKickTexture ||
                     Human.instance.lastAttack == World.playerFlyingKickTexture)
                sprite = World.points300Texture;

            return sprite;
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            lifeTime -= deltaTime;

            if (lifeTime <= 0)
                World.entities.Remove(this);
        }

        public override void Draw(GameTime gameTime)
        {
            World.spriteBatch.Draw(GetSprite(),
              position,
              null,
              Color.White,
              0.0f,
              new Vector2(0, 0), //pivot
              new Vector2(1, 1), //scale
              SpriteEffects.None,
              0.1f
            );
        }
    }
}
