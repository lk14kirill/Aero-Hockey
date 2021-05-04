using System.Threading.Tasks;
using System.Threading;
using SFML.Graphics;
using SFML.System;
using System;
using System.Numerics;

namespace Aero_Hockey
{
    struct Ball
    {
        public float timeFromGame;
        public float speed;
        private CircleShape ballGO;
        
        public Ball(Color color)
        {
            ballGO = new CircleShape();
            timeFromGame = 0;
            speed = 0.3f;
            ballGO.Radius = 20;
            ballGO.FillColor = color;
        }
        public void ChangePosition(Vector2f vector) => ballGO.Position = vector; 

        public Vector2f GetPosition() =>  new Vector2f(ballGO.Position.X, ballGO.Position.Y);

        public Vector2f GetCenter() => new Vector2f(ballGO.Position.X + ballGO.Radius, ballGO.Position.Y + ballGO.Radius);

        public float GetRadius() =>  ballGO.Radius;

        public CircleShape GetBallGO() => ballGO;


        public void Move(Vector2f direction,Vector2u window)
        {
            if (direction != new Vector2f(0, 0)) 
            {
                float tempX = direction.X * window.X,tempY = direction.Y*window.Y;
                float distance = (float)Math.Sqrt(Math.Pow(tempX-GetCenter().X ,2)+Math.Pow(tempY - GetCenter().Y, 2)); // dont know correct formule to calculate
             //Vector2f directionTemp = new Vector2f(speed * direction.X * timeFromGame * GetCenter().X / distance,
                 //               speed * direction.Y * timeFromGame *  GetCenter().Y / distance);
              Vector2f directionTemp = new Vector2f(speed  * timeFromGame * (tempX-GetCenter().X) / distance,
                                  speed * timeFromGame * (tempY -GetCenter().Y) / distance);
                ballGO.Position += directionTemp;
                Console.WriteLine(directionTemp);
                    // direction.X and direction.Y on the start of formule makes x or y 0 if it is 0.If vector is 0,1 ,then x for moving will be 0.
                
            }
            
        }
        public void CheckForRickochet(Vector2u window,out Vector2f? direction)
        {
            direction = null;
            if(GetCenter().X + GetRadius()>window.X || GetCenter().Y+GetRadius() > window.Y)
            {
                speed = 0;
            }
        }
        public void Reflection(Vector2f direction,out Vector2f newDirection)
        {
            float normalizedVector = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);
            float dotProduct = (float)Vector2.Dot(new Vector2(ballGO.Position.X, ballGO.Position.Y), new Vector2(direction.X, direction.Y));
            newDirection.X = ballGO.Position.X - 2 * dotProduct * normalizedVector;
            newDirection.Y = ballGO.Position.Y - 2 * dotProduct * normalizedVector;
        }

    }
}
