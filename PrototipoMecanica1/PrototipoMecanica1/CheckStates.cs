using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace PrototipoMecanica1
{
	public class CheckStates
	{
		public void check()
		{
			
		}

        void EnterGameState(World.GameState newState)
        {
            LeaveGameState();

            World.currentState = newState;
        }

        void UpdateGameState()
        {

        }

        void DrawGameState()
        {

        }

        void LeaveGameState()
        {

        }
	}
}