using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototipoMecanica4
{
    public class PlayerExtraLives : Entity
    {
        static public PlayerExtraLives instance = null;

        private int extraLives = 2;

        public PlayerExtraLives()
        {
            instance = this;
        }

        public void bonusLife()
        {
            extraLives++;
        }

        public void reduceALife()
        {
            extraLives--;
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
