using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrototipoMecanica4
{
    public class StageSelector : Entity
    {
        static public StageSelector instance = null;

        private string stage = "01";

        public StageSelector()
        {
            instance = this;
        }

        public void gameOver()
        {
            stage = "01";
        }

        public void nextStage()
        {
            int tempStage = int.Parse(stage) + 1;

            if (tempStage > 99)
                tempStage = 99;

            stage = tempStage.ToString("00");
        }

        public Texture2D stageImage()
        {
            Texture2D stageSprite = null;

            int tempStage = int.Parse(stage);

            if (tempStage % 2 == 0)
                return stageSprite = World.stage2Texture;
            else
                return stageSprite = World.stageTexture;
        }

        private Texture2D numberSprite(int number)
        {
            Texture2D numberSprite = null;

            switch (number)
            {
                case 48: numberSprite = World.N0Texture;
                    break;

                case 49: numberSprite = World.N1Texture;
                    break;

                case 50: numberSprite = World.N2Texture;
                    break;

                case 51: numberSprite = World.N3Texture;
                    break;

                case 52: numberSprite = World.N4Texture;
                    break;

                case 53: numberSprite = World.N5Texture;
                    break;

                case 54: numberSprite = World.N6Texture;
                    break;

                case 55: numberSprite = World.N7Texture;
                    break;

                case 56: numberSprite = World.N8Texture;
                    break;

                case 57: numberSprite = World.N9Texture;
                    break;
            }

            return numberSprite;
        }

        public void StageDraw(GameTime gameTime)
        {
            int i = 0;

            foreach (char number in stage)
            {
                World.spriteBatch.Draw(numberSprite(number),
                  new Vector2(576 + (32 * i), 384),
                  null,
                  Color.White,
                  0.0f,
                  new Vector2(0, 0), //pivot
                  new Vector2(1, 1), //scale
                  SpriteEffects.None,
                  0.1f
                );

                i++;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            int i = 0;

            foreach (char number in stage)
            {
                World.spriteBatch.Draw(numberSprite(number),
                  new Vector2(864 + (32 * i), 160),
                  null,
                  Color.White,
                  0.0f,
                  new Vector2(0, 0), //pivot
                  new Vector2(1, 1), //scale
                  SpriteEffects.None,
                  0.1f
                );

                i++;
            }

            //Nome do inimigo
            World.spriteBatch.Draw(World.enemy001NameTexture,
                  new Vector2(704, 864),
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
