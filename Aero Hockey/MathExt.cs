using System;
using SFML.System;

namespace Aero_Hockey
{
    public static class MathExt
    {
        public static float VectorLength(Vector2f firstVector, Vector2f secondVector)
        {

            return (float)Math.Sqrt(Math.Pow(secondVector.X - firstVector.X, 2) +
                                    Math.Pow(secondVector.Y - firstVector.Y, 2));
        }
        public static float GetPercentOf(float value,float percent)
        {
            return value / 100 * percent;
        }
        public static  Vector2f CheckForIntersectAndDetectDirection(CircleObject firstCircle, CircleObject secondCircle)     //Ricochets first circle
        {
            double distanceBetweenRadiuses = MathExt.VectorLength(secondCircle.GetCenter(), firstCircle.GetCenter()); ;

            if (distanceBetweenRadiuses <= firstCircle.GetRadius() + secondCircle.GetRadius())
            {
                return DetectSide(firstCircle, secondCircle);
            }
            return new Vector2f(0, 0);
        }
        public static (Vector2f,Vector2f) CreateGates(Vector2u window,bool upside)
        {
            float xPos1 = window.X / 2 - GetPercentOf(window.X, 10),xPos2 =  window.X / 2 + GetPercentOf(window.X, 10) ;
            Vector2f firstPoint, secondpoint;
            switch (upside)
            {
                case true:
                    float yPos1 = 0 + GetPercentOf(window.Y, 2.01f);
                    firstPoint = secondpoint = new Vector2f(xPos1, yPos1);
                    secondpoint.X = xPos2;
                    return (firstPoint, secondpoint);
                    break;
                case false:
                    float yPos2 = window.Y - GetPercentOf(window.Y, 2.01f);
                    firstPoint = secondpoint = new Vector2f(xPos1, yPos2);
                    secondpoint.X = xPos2;
                    return (firstPoint, secondpoint);
                    break;
            }
        }
        private static Vector2f DetectSide(CircleObject racket, CircleObject ball)
        {
            Vector2f centreOfRadiuses = new Vector2f((racket.GetCenter().X + ball.GetCenter().X) / 2,
                                                     (racket.GetCenter().Y + ball.GetCenter().Y) / 2);
            if (centreOfRadiuses.X > racket.GetCenter().X &&
                centreOfRadiuses.X < racket.GetCenter().X + racket.GetRadius() - 10 &&
                centreOfRadiuses.Y > racket.GetCenter().Y &&                               //checks for intersect on upLeft
               centreOfRadiuses.Y < racket.GetCenter().Y + racket.GetRadius() - 10)
            {
                return Vector2Directions.downRight;
            }
            if (centreOfRadiuses.X > racket.GetCenter().X &&
                centreOfRadiuses.X < racket.GetCenter().X + racket.GetRadius() - 10 &&
                centreOfRadiuses.Y < racket.GetCenter().Y &&                                    //checks for intersect on downLeft
               centreOfRadiuses.Y > racket.GetCenter().Y - racket.GetRadius() + 10)
            {
                return Vector2Directions.upRight;
            }
            if (centreOfRadiuses.X < racket.GetCenter().X &&
                centreOfRadiuses.X > racket.GetCenter().X - racket.GetRadius() + 10 &&
                centreOfRadiuses.Y < racket.GetCenter().Y &&                                 //checks for intersect on downRight
                centreOfRadiuses.Y > racket.GetCenter().Y - racket.GetRadius() + 10)
            {
                return Vector2Directions.upLeft;
            }
            if (centreOfRadiuses.X < racket.GetCenter().X &&
                centreOfRadiuses.X > racket.GetCenter().X - racket.GetRadius() + 10 &&
                centreOfRadiuses.Y > racket.GetCenter().Y &&                                    //checks for intersect on upRight
                centreOfRadiuses.Y < racket.GetCenter().Y + racket.GetRadius() - 10)
            {
                return Vector2Directions.downLeft;
            }

            if (centreOfRadiuses.X > racket.GetCenter().X - racket.GetRadius() + 20 &&
                centreOfRadiuses.X < racket.GetCenter().X + racket.GetRadius() - 20 &&
                centreOfRadiuses.Y > racket.GetCenter().Y &&                                    //checks for intersect on up
                centreOfRadiuses.Y < racket.GetCenter().Y + racket.GetRadius() + 10)
            {
                return Vector2Directions.down;
            }
            if (centreOfRadiuses.X > racket.GetCenter().X - racket.GetRadius() + 20 &&
                centreOfRadiuses.X < racket.GetCenter().X + racket.GetRadius() - 20 &&
                centreOfRadiuses.Y < racket.GetCenter().Y &&                                    //checks for intersect on down
                centreOfRadiuses.Y > racket.GetCenter().Y - racket.GetRadius() - 10)
            {
                return Vector2Directions.up;
            }
            if (centreOfRadiuses.X > racket.GetCenter().X &&
             centreOfRadiuses.X < racket.GetCenter().X + racket.GetRadius() + 10 &&
             centreOfRadiuses.Y > racket.GetCenter().Y - racket.GetRadius() + 20 &&                                    //checks for intersect on left
             centreOfRadiuses.Y < racket.GetCenter().Y + racket.GetRadius() - 20)
            {
                return Vector2Directions.right;
            }
            if (centreOfRadiuses.X < racket.GetCenter().X &&
                centreOfRadiuses.X > racket.GetCenter().X - racket.GetRadius() - 10 &&
                centreOfRadiuses.Y > racket.GetCenter().Y - racket.GetRadius() + 20 &&                                    //checks for intersect on right
                centreOfRadiuses.Y < racket.GetCenter().Y + racket.GetRadius() - 20)
            {
                return Vector2Directions.left;
            }

            return new Vector2f(0, 0);
        }
        public static bool IsVectorBiggerThenWindowY(CircleObject circle, Vector2u window)
        {
            if (circle.GetCenter().Y - circle.GetRadius() > GetPercentOf(window.Y, 50))
                return true;
            return false;
        }
    }
}
