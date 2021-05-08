using System.Threading.Tasks;
using System.Threading;
using SFML.Graphics;
using SFML.System;
using System;

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
            speed = 0.5f;
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
                float directionXAbs = Math.Abs(direction.X), directionYAbs = Math.Abs(direction.Y);
                float tempX = direction.X * (window.X+500),tempY = direction.Y*(window.Y+500);
                float distance = (float)Math.Sqrt(Math.Pow(tempX-GetCenter().X, 2) + Math.Pow(tempY - GetCenter().Y , 2)); // dont know correct formule to calculate


                Vector2f directionTemp = new Vector2f(speed * timeFromGame * (tempX -GetCenter().X*directionXAbs) / distance,
                                                      speed * timeFromGame * (tempY -GetCenter().Y*directionYAbs) / distance);
                ballGO.Position += directionTemp;
               
                    // direction.X and direction.Y on the start of formule makes x or y 0 if it is 0.If vector is 0,1 ,then x for moving will be 0.
                
            }
            
        }
        public void Reflect(Vector2u window, Vector2f? oldDirection, out Vector2f? newDirection)
        {

            newDirection = null;
            if(oldDirection == null)
            {
                return;
            }
            float xBorder = window.X / 100 * 2;
            float yBorder = window.Y / 100 * 2;
            if (GetCenter().X + GetRadius()>window.X-xBorder || GetCenter().X-GetRadius() <xBorder)
            {
                newDirection = new Vector2f(-oldDirection.Value.X, oldDirection.Value.Y);
            }
            if(GetCenter().Y + GetRadius() > window.Y-yBorder  || GetCenter().Y - GetRadius() <yBorder)
            {
                newDirection = new Vector2f(oldDirection.Value.X, -oldDirection.Value.Y);
            }
        }
        public bool CheckInteractionWithZone(Vector2f firstPoint,Vector2f secondPoint,bool upSideZone)
        {
            return false;
        }

    }
}
