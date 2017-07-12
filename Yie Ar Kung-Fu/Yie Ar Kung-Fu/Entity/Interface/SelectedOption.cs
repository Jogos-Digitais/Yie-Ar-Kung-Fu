using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YieArKungFu
{
    public class SelectedOption : Entity
    {
        public static SelectedOption instance = null;

        public bool levelOneSelected = true;

        public SelectedOption()
        {
            instance = this;
        }

        public void ChangeSelectedOption()
        {
            if (levelOneSelected)
                levelOneSelected = false;
            else
                levelOneSelected = true;
        }

        private Vector2 OptionPosition()
        {
            Vector2 position = Vector2.One;

            if (levelOneSelected)
                position = new Vector2(368f, 689f);
            else
                position = new Vector2(368f, 753f);

            return position;
        }

        public override void Draw(GameTime gameTime)
        {
            World.spriteBatch.Draw(World.selectedOptionTexture,
              OptionPosition(),
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
