using SFML.Graphics;
using SFML.System;

namespace Aero_Hockey
{
    class GameRacket : CircleObject
    {
        public GameRacket(Color color)
        {
            gameObject.Radius = 30;
            gameObject.FillColor = color;
        }

    }
}
