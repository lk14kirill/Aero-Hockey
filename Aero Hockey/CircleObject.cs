using SFML.Graphics;
using SFML.System;

namespace Aero_Hockey
{
   public  class CircleObject
    {
        private bool canMove = true;
        public CircleShape gameObject = new CircleShape();

        public void ChangeBoolCanMove(bool a) => canMove = a;
        public bool GetCanMove() => canMove;
        public void ChangePosition(Vector2f vector)
        {
            if (canMove)
                gameObject.Position = new Vector2f(vector.X - GetRadius(), vector.Y - GetRadius());
        }
        public Vector2f GetPosition() => new Vector2f(gameObject.Position.X, gameObject.Position.Y);

        public Vector2f GetCenter() => new Vector2f(gameObject.Position.X + gameObject.Radius, gameObject.Position.Y + gameObject.Radius);

        public float GetRadius() => gameObject.Radius;

        public CircleShape GetGO() => gameObject;
    }
}
