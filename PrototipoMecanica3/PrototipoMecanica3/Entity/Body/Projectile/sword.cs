using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace PrototipoMecanica3
{
    public class Sword : Body
    {
        public Sword(Vector2 initPos, Vector2 size)
            : base(initPos, size)
        {
        }
    }
}