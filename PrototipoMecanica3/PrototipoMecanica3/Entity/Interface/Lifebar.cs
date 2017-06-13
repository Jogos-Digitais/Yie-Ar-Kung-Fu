using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototipoMecanica3
{
    public class Lifebar : Entity
    {
        static public Lifebar instance = null;

        private int playerLife = 9;
        private int enemyLife = 9;
        private Color playerLifeColor = new Color(92, 228, 48);
        private Color enemyLifeColor = new Color(92, 228, 48);

        public Lifebar()
        {
            instance = this;
        }

        public void damagePlayerLife()
        {
            playerLife -= 1;

            if (playerLife > 4)
                playerLifeColor = new Color(92, 228, 48);
            else
                playerLifeColor = new Color(183, 30, 123);
        }

        public int remainingPlayerLife()
        {
            return playerLife;
        }

        public void damageEnemyLife()
        {
            enemyLife -= 1;

            if (enemyLife > 4)
                enemyLifeColor = new Color(92, 228, 48);
            else
                enemyLifeColor = new Color(183, 30, 123);
        }

        public int remainingEnemyLife()
        {
            return enemyLife;
        }

        public override void Draw(GameTime gameTime)
        {
            World.spriteBatch.Draw(World.lifeBarTexture,
              new Vector2(152, 841),
              null,
              Color.White,
              0.0f,
              new Vector2(0, 0), //pivot
              new Vector2(1, 1), //scale
              SpriteEffects.None,
              0.1f
            );

            for (int i = 0; i < playerLife; i++)
                World.spriteBatch.Draw(World.lifeFragment, new Vector2(416 - (32 * i), 849), null, playerLifeColor, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.1f);

            for (int i = 0; i < enemyLife; i++)
                World.spriteBatch.Draw(World.lifeFragment, new Vector2(576 + (32 * i), 849), null, enemyLifeColor, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.1f);

            if (World.debugMode)
            {
                World.spriteBatch.DrawString(World.fontNormal, "Player life: " + playerLife + "\nEnemy life: " + enemyLife, new Vector2(400, 880), Color.White, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            }
        }
    }
}
