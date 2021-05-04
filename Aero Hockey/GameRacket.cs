using SFML.Graphics;
using SFML.System;

namespace Aero_Hockey
{
    struct GameRacket
    {
        private CircleShape racketGO;

        public GameRacket(Color color)
        {
            racketGO = new CircleShape();
            racketGO.Radius = 30;
            racketGO.FillColor = color;
        }
        public void ChangePosition(Vector2f vector) => racketGO.Position = vector;

        public Vector2f GetPosition() => new Vector2f(racketGO.Position.X, racketGO.Position.Y); 

        public Vector2f GetCenter() =>  new Vector2f(racketGO.Position.X + racketGO.Radius, racketGO.Position.Y + racketGO.Radius);

        public float GetRadius()=>  racketGO.Radius;

        public CircleShape GetRacketGO() => racketGO;
    }
}
