﻿using SFML.Graphics;
using SFML.System;

namespace Aero_Hockey
{
   public  class CircleObject
    {
        public CircleShape gameObject = new CircleShape();
        public void ChangePosition(Vector2f vector) => gameObject.Position = vector;

        public Vector2f GetPosition() => new Vector2f(gameObject.Position.X, gameObject.Position.Y);

        public Vector2f GetCenter() => new Vector2f(gameObject.Position.X + gameObject.Radius, gameObject.Position.Y + gameObject.Radius);

        public float GetRadius() => gameObject.Radius;

        public CircleShape GetGO() => gameObject;
    }
}
