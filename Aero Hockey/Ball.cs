using System.Threading.Tasks;
using System.Threading;
using SFML.Graphics;
using SFML.System;
using System;

namespace Aero_Hockey
{
    class Ball : CircleObject
    {
        public float speed;

        public Ball(Color color)
        {
            speed = 0.5f;
            gameObject.Radius = 20;
            gameObject.FillColor = color;
        }
        public void Move(Vector2f direction,Vector2u window,float time)
        {
            if (direction != new Vector2f(0, 0) && GetCanMove()) 
            {
                float directionXAbs = Math.Abs(direction.X), directionYAbs = Math.Abs(direction.Y);
                Vector2f tempVector = new Vector2f(direction.X * (window.X+window.X/2), direction.Y*(window.Y+window.Y/2)); // window. /2 here is to make movement not smooth when ball is close to border
                float distance = MathExt.VectorLength(tempVector, GetCenter()); 

                Vector2f directionTemp = new Vector2f(speed * time * (tempVector.X -GetCenter().X*directionXAbs) / distance,
                                                      speed * time * (tempVector.Y -GetCenter().Y*directionYAbs) / distance);
                gameObject.Position += directionTemp;
                    // directionXAbs and directionYAbs in the formule makes x or y 0 if it is 0.If vector is 0,1 ,then x for moving will be 0.
            }
        }
        public Vector2f Reflect(Vector2u window, Vector2f oldDirection,float xBorder,float yBorder)
        {
            if (GetCenter().X + GetRadius()>window.X-xBorder || GetCenter().X-GetRadius() <xBorder)
            {
                return new Vector2f(-oldDirection.X, oldDirection.Y);
            }
            if(GetCenter().Y + GetRadius() > window.Y-yBorder  || GetCenter().Y - GetRadius() <yBorder)
            {
                return new Vector2f(oldDirection.X, -oldDirection.Y);
            }
            return new Vector2f(0, 0);
        }
        public bool CheckInteractionWithGate(Vector2f firstPoint,Vector2f secondPoint,bool upSideZone)
        {
            if (GetCenter().X > firstPoint.X && GetCenter().X < secondPoint.X )
            {
                switch (upSideZone)
                {
                    case true:
                        if (GetCenter().Y - GetRadius() < secondPoint.Y)
                            return true;
                        break;
                    case false:
                        if (GetCenter().Y + GetRadius() > secondPoint.Y)
                            return true;
                        break;
                }
            }
            return false;
        }
    }
}
