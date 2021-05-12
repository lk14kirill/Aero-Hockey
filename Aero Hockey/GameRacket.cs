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
        public void GoToStartPoint(Vector2u window,float percentage)
        {
            ChangePosition(new Vector2f(window.X / 2, MathExt.GetPercentOf(window.Y, percentage)));
        }
    }
}
