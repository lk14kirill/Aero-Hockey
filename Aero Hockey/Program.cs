using System;
using System.Threading.Tasks;
using System.Threading;
using SFML;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace Aero_Hockey
{
    public enum MovingStates
    {
        readyToMove,
        moving,
        stopped
    }
    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game();
            game.GameCycle();
        }
    }
    class Game
    {
        private float mouseX;
        private float mouseY;
        public double time;
        private Vector2f? direction;
        private int[] score = new int[2];
        private int[] scoreForText = new int[2];
        private Text scoreText = new Text();

        private Clock clock = new Clock();

        private GameRacket gameRacket = new GameRacket(Color.Cyan);
        private Ball ball = new Ball(Color.Red);
        public   double GetTime() => time;
        public void GameCycle()
        {

            RenderWindow window = new RenderWindow(new VideoMode(1000,1000), "Game window");
            window.MouseMoved += OnMouseMoved;
            window.Closed += WindowClosed;
            window.SetMouseCursorVisible(false);

            TextSetup(Color.Red);
            ball.ChangePosition(new Vector2f(window.Size.X / 2, window.Size.Y / 2));
            RectangleShape s = new RectangleShape();
            while (window.IsOpen)
            {
                time = clock.ElapsedTime.AsMicroseconds();
                clock.Restart();
                time /= 800;
                window.Clear();
                window.DispatchEvents();
                
                ball.timeFromGame = (float)time;

                gameRacket.ChangePosition(new Vector2f(mouseX- gameRacket.GetRadius(),mouseY-gameRacket.GetRadius()));
                CheckForIntersect(gameRacket,ball,out Vector2f? tempDirection);
                ChangeDirection(tempDirection);
                ball.CheckForRickochet(window.Size,out Vector2f? tempDirectionFromRicochet);
                ChangeDirection(tempDirectionFromRicochet);

                if (ball.GetStateOfMoving() == MovingStates.readyToMove && direction != null)
                {

                    ball.Move(direction.Value,window.Size);
                }
                MakeBallReadyToMove();

                
                window.Draw(ball.GetBallGO());
                window.Draw(gameRacket.GetRacketGO());
                window.Draw(scoreText);
                //System.Threading.Thread.Sleep(0001);
                window.Display();
            }
        }
        private void ChangeDirection(Vector2f? vector)
        {
            if (vector != null && ball.GetStateOfMoving() != MovingStates.stopped && vector.Value != new Vector2f(0, 0))
            {
                direction = vector;

            }
        }
        private void Reflect(Vector2f? vector)
        {
            if (vector != null && ball.GetStateOfMoving() != MovingStates.stopped && vector.Value != new Vector2f(0, 0))
            {
                
                direction = vector;

            }
        }
        private void TextSetup(Color color)
        {
            scoreText.FillColor = color;
            scoreText.Position = new Vector2f(540, 650);
            scoreText.Scale = new Vector2f(200, 200);
            scoreText.DisplayedString = "0:0";
        }
        private void ChangeTextScore()
        {
            scoreText.DisplayedString = scoreForText[0]+":"+ scoreForText[1];
        }
        private void MakeBallReadyToMove()
        {
            if(ball.GetStateOfMoving() == MovingStates.stopped)
            {
                Console.WriteLine(ball.GetStateOfMoving());
                direction = new Vector2f(0, 0);
            }
        }
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
               centreOfRadiuses.Y < racket.GetCenter().Y + racket.GetRadius() - 10
               )
            {
                //  Console.WriteLine("Center"+centreOfRadiuses+" racketPosition"+racket.GetPosition()+
                //  "racketCenter"+racket.GetCenter());
                // Console.WriteLine("centre" + centreOfRadiuses + " ballPosition" + ball.GetPosition() +
                // "ballCenter" + ball.GetCenter());
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
    struct Ball
    {
        public float timeFromGame;
        public float speed;
        public float radius;
        private CircleShape ballGO;
        public Color color;
        public Vector2f center;
        public Vector2f position;
        public MovingStates moveState;
        public Ball(Color color)
        {
            ballGO = new CircleShape();
            timeFromGame = 0;
            speed = 0.1f;
            moveState = MovingStates.readyToMove;
            center = new Vector2f( ballGO.Position.X + ballGO.Radius, ballGO.Position.Y + ballGO.Radius);
            position = new Vector2f(ballGO.Position.X, ballGO.Position.Y);
            radius = ballGO.Radius;
            ballGO.Radius = 20;
            this.color = color;
            ballGO.FillColor = this.color;
        }
        public void ChangePosition(Vector2f vector) => ballGO.Position = vector; 

        public Vector2f GetPosition() =>  position = new Vector2f(ballGO.Position.X, ballGO.Position.Y);

        public Vector2f GetCenter() => center = new Vector2f(ballGO.Position.X + ballGO.Radius, ballGO.Position.Y + ballGO.Radius);

        public float GetRadius() => radius = ballGO.Radius;

        public CircleShape GetBallGO() => ballGO;

        public void SetMovingState(MovingStates state) => moveState = state;
        public MovingStates GetStateOfMoving() => moveState;
        public void Move(Vector2f direction,Vector2u window)
        {
            if (direction != new Vector2f(0, 0)) 
            {
               AsyncStopMoving();
                float distance =250;
                ballGO.Position += new Vector2f(speed * direction.X * timeFromGame * (direction.X + ballGO.Position.X) / distance,
                                   direction.Y * timeFromGame * (direction.Y + ballGO.Position.Y)/distance);
                    // direction.X and direction.Y on the start of formule makes x or y 0 if it is 0.If vector is 0,1 ,then x for moving will be 0.
                
            }
            
        }
        public void CheckForRickochet(Vector2u window,out Vector2f? direction)
        {
            direction = null;
            if(GetCenter().X + GetRadius()>window.X || GetCenter().Y+GetRadius() > window.Y)
            {
                speed = 0;
              //  direction = Vector2Directions.up;
            }
        }
        private async void AsyncStopMoving()
        {
           Ball thisA = this;
            await Task.Run(() => thisA.StopMoving());
        }
        private void StopMoving()
        {
            Thread.Sleep(3000);
            moveState = MovingStates.stopped;
            Thread.Sleep(100);
            moveState = MovingStates.readyToMove;
            ballGO.Rotation = 10;
        }

    }
    struct GameRacket
    {
        private CircleShape racketGO;
        public Color color;
        private Vector2f center;
        private Vector2f position;
        private float radius;
        public GameRacket(Color color)
        {
            this.color = color;
            racketGO = new CircleShape();
            center = new Vector2f( racketGO.Position.X + racketGO.Radius, racketGO.Position.Y + racketGO.Radius);
            position = new Vector2f(racketGO.Position.X, racketGO.Position.Y);
            radius = racketGO.Radius;
            racketGO.Radius = 30;
            //racket.OutlineColor = Color.Red;
            //racket.OutlineThickness = 8;
            racketGO.FillColor = this.color;
        }
        public void ChangePosition(Vector2f vector) => racketGO.Position = vector;

        public Vector2f GetPosition() => position = new Vector2f(racketGO.Position.X, racketGO.Position.Y); 

        public Vector2f GetCenter() => center = new Vector2f(racketGO.Position.X + racketGO.Radius, racketGO.Position.Y + racketGO.Radius);

        public float GetRadius()=> radius = racketGO.Radius;

        public CircleShape GetRacketGO() => racketGO;
    }
    public class Vector2Directions
    {
        public static Vector2f left = new Vector2f(-1, 0);
        public static Vector2f right = new Vector2f(1, 0);
        public static Vector2f up = new Vector2f(0, -1);
        public static Vector2f down = new Vector2f(0, 1);

        public static Vector2f upLeft = new Vector2f(-1, -1);
        public static Vector2f upRight = new Vector2f(1, -1);
        public static Vector2f downLeft = new Vector2f(-1, 1);
        public static Vector2f downRight = new Vector2f(1, 1);
    }
}
