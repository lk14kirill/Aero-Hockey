using SFML.Graphics;
using SFML.System;

namespace Aero_Hockey
{
    class Bot
    {
       public  GameRacket racket = new GameRacket(Color.Blue);
        private float speed;
        public Bot(float speed)
        {
            this.speed = speed;
        }

        public void MoveTo(Vector2f vector,float time)
        {
            float distance = MathExt.VectorLength(vector, racket.GetCenter());

            Vector2f directionTemp = new Vector2f(speed * time * (vector.X - racket.GetCenter().X)  / distance,
                                                  speed * time * (vector.Y - racket.GetCenter().Y)  / distance);
            racket.gameObject.Position += directionTemp;
        }
     

    }
}
