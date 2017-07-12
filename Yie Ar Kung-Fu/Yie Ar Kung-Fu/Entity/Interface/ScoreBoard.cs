using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YieArKungFu
{
    public class ScoreBoard : Entity
    {
        public static ScoreBoard instance = null;

        public bool playerWinner = false, perfect = false;
        private string score = "000000", highScore = "000000";

        public ScoreBoard()
        {
            instance = this;
        }

        public void resetPlayerWinner()
        {
            playerWinner = false;
        }

        public void resetPerfect()
        {
            perfect = false;
        }

        public void resetScore()
        {
            score = "000000";
        }

        private void checkHighScore()
        {
            if (int.Parse(score) > int.Parse(highScore))
                highScore = score;
        }

        public void add100points() //For all punchs
        {
            int tempScore = int.Parse(score) + 100;

            if (tempScore > 999999)
                tempScore = 999999;

            score = tempScore.ToString("000000");

            checkHighScore();
        }

        public void add200points() //For normal and high kicks
        {
            int tempScore = int.Parse(score) + 200;

            if (tempScore > 999999)
                tempScore = 999999;

            score = tempScore.ToString("000000");

            checkHighScore();
        }

        public void add300points() //For low and flying kicks
        {
            int tempScore = int.Parse(score) + 300;

            if (tempScore > 999999)
                tempScore = 999999;

            score = tempScore.ToString("000000");

            checkHighScore();
        }

        public void add800points() //For each lifeFragment remaining at end of fight
        {
            int tempScore = int.Parse(score) + 800;

            if (tempScore > 999999)
                tempScore = 999999;

            score = tempScore.ToString("000000");

            checkHighScore();
        }

        public void add5000points() //For perfects (Position for perfect message 320x576)
        {
            int tempScore = int.Parse(score) + 5000;

            if (tempScore > 999999)
                tempScore = 999999;

            score = tempScore.ToString("000000");

            checkHighScore();
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

        public override void Draw(GameTime gameTime)
        {
            int i = 0;

            foreach (char number in score)
            {
                World.spriteBatch.Draw(numberSprite(number),
                  new Vector2(96 + (32 * i), 192),
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

            i = 0;

            foreach (char number in highScore)
            {
                World.spriteBatch.Draw(numberSprite(number),
                  new Vector2(384 + (32 * i), 192),
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
    }
}
