
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace PrototipoMecanica2
{
	public class GameStates
	{
        private static float countdownToMenu = 6;

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

                case World.GameState.Pause:
                    { }
                    break;

                case World.GameState.Over:
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

                case World.GameState.Pause:
                    { }
                    break;

                case World.GameState.Over:
                    {
                        if (countdownToMenu > 0)
                        {
                            countdownToMenu -= deltaTime;
                        }
                        else
                        {
                            countdownToMenu = 6;
                            EnterGameState(World.GameState.Menu);
                        }
                    }
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

                case World.GameState.Pause:
                    { }
                    break;

                case World.GameState.Over:
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

                case World.GameState.Pause:
                    { }
                    break;

                case World.GameState.Over:
                    { }
                    break;
            }
        }
	}
}