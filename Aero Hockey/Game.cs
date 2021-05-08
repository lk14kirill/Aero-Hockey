using System;
using System.Numerics;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace Aero_Hockey
{
    class Game
    {
        private float mouseX;
        private float mouseY;
        public double time;
        private Vector2f? direction;
        //private int[] score = new int[2];
        //private int[] scoreForText = new int[2]                for future
        //private Text scoreText = new Text();

        private Clock clock = new Clock();

        private GameRacket gameRacket = new GameRacket(Color.Cyan);
        private Ball ball = new Ball(Color.Red);                             
        RenderWindow window = new RenderWindow(new VideoMode(1000,1000), "Game window");
        public   double GetTime() => time;
        public void GameCycle()
        {
            WindowSetup();
            //TextSetup(Color.Red);
            ball.ChangePosition(new Vector2f(window.Size.X / 2, window.Size.Y / 2));
            while (window.IsOpen)
            {
                time = clock.ElapsedTime.AsMicroseconds();
                clock.Restart();
                time /= 800;
                ball.timeFromGame = (float)time;

                window.Clear();
                window.DispatchEvents();

                gameRacket.ChangePosition(new Vector2f(mouseX - gameRacket.GetRadius(), mouseY - gameRacket.GetRadius()));
                CheckForIntersect(gameRacket, ball, out Vector2f? tempDirection);
                ChangeDirection(tempDirection);

              //  ball.Reflect(window.Size, direction, out Vector2f? tempDirectionFromRicochet);
               // ChangeDirection(tempDirectionFromRicochet);
                
                if ( direction != null)
                {

                    ball.Move(direction.Value,window.Size);
                }
               
                window.Draw(ball.GetBallGO());
                window.Draw(gameRacket.GetRacketGO());
                window.Display();
            }
        }
        private void WindowSetup()
        {
            window.MouseMoved += OnMouseMoved;
            window.Closed += WindowClosed;
            window.SetMouseCursorVisible(false);
        }
        private void ChangeDirection(Vector2f? vector)
        {
            if (vector != null && vector.Value != new Vector2f(0, 0))
            {
                direction = vector;

            }
        }
        //private void TextSetup(Color color)
        //{
        //    scoreText.FillColor = color;
        //    scoreText.Position = new Vector2f(540, 650);
        //    scoreText.Scale = new Vector2f(200, 200);
        //    scoreText.CharacterSize = 50;                                            for future

        //    scoreText.DisplayedString = "0:0";
        //}
        //private void ChangeTextScore()
        //{
        //    scoreText.DisplayedString = scoreForText[0]+":"+ scoreForText[1];
        //}
        public void OnMouseMoved(object sender, MouseMoveEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;
        }
        private void WindowClosed(object sender, EventArgs e)
        {
            RenderWindow w = (RenderWindow)sender;
            w.Close();
        }
        private void CheckForIntersect(GameRacket racket, Ball ball, out Vector2f? direction)     //Ricochets first circle
        {
            double distanceBetweenRadiuses;
            direction = new Vector2f(0, 0);
            distanceBetweenRadiuses = Math.Sqrt(Math.Pow(ball.GetCenter().X - racket.GetCenter().X, 2) + 
                                                Math.Pow(ball.GetCenter().Y - racket.GetCenter().Y, 2));
            if (distanceBetweenRadiuses<= racket.GetRadius() + ball.GetRadius())
            { 
                direction =  DetectSide(racket, ball);
            }
           
        }
        private Vector2f? DetectSide(GameRacket racket,Ball ball)
        {
            Vector2f centreOfRadiuses = new Vector2f((racket.GetCenter().X + ball.GetCenter().X) / 2,
                                                     (racket.GetCenter().Y + ball.GetCenter().Y) / 2);
            if (centreOfRadiuses.X > racket.GetCenter().X  &&
                centreOfRadiuses.X < racket.GetCenter().X + racket.GetRadius()- 10 &&
                centreOfRadiuses.Y > racket.GetCenter().Y  &&                               //checks for intersect on upLeft
               centreOfRadiuses.Y < racket.GetCenter().Y + racket.GetRadius() - 10)
            {
                return Vector2Directions.downRight;
            }
            if (centreOfRadiuses.X > racket.GetCenter().X &&
                centreOfRadiuses.X < racket.GetCenter().X + racket.GetRadius()  - 10&&
                centreOfRadiuses.Y < racket.GetCenter().Y &&                                    //checks for intersect on downLeft
               centreOfRadiuses.Y > racket.GetCenter().Y - racket.GetRadius() + 10)
            {
                return Vector2Directions.upRight;
            }
            if (centreOfRadiuses.X < racket.GetCenter().X &&
                centreOfRadiuses.X > racket.GetCenter().X - racket.GetRadius() + 10&&
                centreOfRadiuses.Y < racket.GetCenter().Y  &&                                 //checks for intersect on downRight
                centreOfRadiuses.Y > racket.GetCenter().Y - racket.GetRadius() + 10)
            {
                return Vector2Directions.upLeft;
            }
            if (centreOfRadiuses.X < racket.GetCenter().X &&
                centreOfRadiuses.X > racket.GetCenter().X - racket.GetRadius()  + 10&&
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
             centreOfRadiuses.Y > racket.GetCenter().Y - racket.GetRadius() + 20&&                                    //checks for intersect on left
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

            return null;
        }

    }
}
