
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace PrototipoMecanica4
{
	public class GameStates
	{
        public static void EnterGameState(World.GameState newState)
        {
            LeaveGameState();

            World.currentState = newState;

            switch (World.currentState)
            {
                case World.GameState.Menu:
                    { }
                    break;

                case World.GameState.Stage:
                    { }
                    break;
            }
        }

        public static void UpdateGameState(GameTime gameTime)
        {
            //Timer
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (World.currentState)
            {
                case World.GameState.Menu:
                    { }
                    break;

                case World.GameState.Stage:
                    { }
                    break;
            }
        }

        public static void DrawGameState(GameTime gameTime)
        {
            //Timer
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (World.currentState)
            {
                case World.GameState.Menu:
                    { }
                    break;

                case World.GameState.Stage:
                    { }
                    break;
            }
        }

        public static void LeaveGameState()
        {
            switch (World.currentState)
            {
                case World.GameState.Menu:
                    { }
                    break;

                case World.GameState.Stage:
                    { }
                    break;
            }
        }
	}
}