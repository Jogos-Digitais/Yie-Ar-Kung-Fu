using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YieArKungFu
{
    public class PlayerExtraLives : Entity
    {
        public static PlayerExtraLives instance = null;

        private int extraLives = 2;

        public PlayerExtraLives()
        {
            instance = this;
        }

        public void bonusLife()
        {
            extraLives++;

            if (extraLives > 9)
                extraLives = 9;
        }

        public void reduceALife()
        {
            extraLives--;

            if (extraLives < 0)
                extraLives = 0;
        }

        public int remainingLives()
        {
            return extraLives;
        }

        public override void Draw(GameTime gameTime)
        {
            for (int i = 0; i < extraLives; i++)
            {
                World.spriteBatch.Draw(World.extraLifeTexture,
                  new Vector2(672 + (32 * i), 192),
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
}
