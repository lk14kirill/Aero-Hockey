using System;
using System.Numerics;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Collections.Generic;
using System.IO;

namespace Aero_Hockey
{
    class Game
    {
        private List<Drawable> objectsToDraw = new List<Drawable>();
        private Text scoreText = new Text();
        private GameRacket gameRacket = new GameRacket(Color.Red);
        private Ball ball = new Ball(Color.White);
        private RenderWindow window = new RenderWindow(new VideoMode(1000, 1000), "Game window");
        private Clock clock = new Clock();
        private Bot bot = new Bot(0.2f);

        private Vector2f[] UpperGate = new Vector2f[2];
        private Vector2f[] Gate = new Vector2f[2];
        private Vector2f direction;
        private float mouseX;
        private float mouseY;
        private float xBorder;
        private float yBorder;

        public double time;
        private int[] score = new int[2];
        private int[] scoreForText = new int[2];

        public void GameCycle()
        {
            Init();
            while (window.IsOpen && score[0] != 10 && score[1] != 10) 
            {
                time = clock.ElapsedTime.AsMicroseconds();
                clock.Restart();
                time /= 800;                                              //for smoother movement of ball

                window.Clear();
                window.DispatchEvents();

                    gameRacket.ChangePosition(new Vector2f(mouseX - gameRacket.GetRadius(), mouseY - gameRacket.GetRadius()));

                ChangeDirection(MathExt.CheckForIntersectAndDetectDirection(gameRacket, ball));
                ChangeDirection(MathExt.CheckForIntersectAndDetectDirection(bot.racket, ball));
                if (!MathExt.IsVectorBiggerThenWindowY(bot.racket, window.Size))
                    bot.MoveTo(ball.GetCenter(), (float)time);
                else
                    bot.MoveTo(new Vector2f(window.Size.X / 2, MathExt.GetPercentOf(window.Size.Y, 10)),(float)time);


                if (direction != new Vector2f(0, 0))
                    ChangeDirection(ball.Reflect(window.Size, direction, xBorder, yBorder));

                if (direction != new Vector2f(0, 0))
                    ball.Move(direction, window.Size,(float)time);

                SomeoneScore();
                DrawObjects();
                window.Display();
            }
        }
        private void SomeoneScore()
        {
            if (ball.CheckInteractionWithGate(UpperGate[0],UpperGate[1],true))
            {
                scoreForText[0] += 1;
                score[0] += 1;
                ActionsAfterScore();
            }
            if (ball.CheckInteractionWithGate(Gate[0], Gate[1], false))
            {
                scoreForText[1] += 1;
                score[1] += 1;
                ActionsAfterScore();
            }
        }
        private void ActionsAfterScore()
        {
            ChangeTextScore();
            ball.ChangePosition(new Vector2f(window.Size.X / 2, window.Size.Y / 2));
            direction = new Vector2f(0, 0);
            bot.racket.GoToStartPoint(window.Size,10);
        }
        private void Init()
        {
            WindowSetup();
            SetupFieldObjects();
            AddAllDrawableObjectsToList();
            TextSetup(Color.Blue);
        }
        private void SetupFieldObjects()
        {
             xBorder = MathExt.GetPercentOf(window.Size.X, 2);
             yBorder = MathExt.GetPercentOf(window.Size.Y, 2);
            (UpperGate[0], UpperGate[1]) = MathExt.CreateGates(window.Size, true);
            (Gate[0], Gate[1]) = MathExt.CreateGates(window.Size, false);
            ball.ChangePosition(new Vector2f(window.Size.X / 2, window.Size.Y / 2));
            bot.racket.GoToStartPoint(window.Size, 10);
            gameRacket.GoToStartPoint(window.Size, 80);
        }
        private void AddAllDrawableObjectsToList()
        {
            objectsToDraw.Add(gameRacket.GetGO());
            objectsToDraw.Add(ball.GetGO());
            objectsToDraw.Add(scoreText);
            objectsToDraw.Add(bot.racket.GetGO());
        }
        private void DrawObjects()
        {
            foreach(Drawable shape in objectsToDraw)
            {
                window.Draw(shape);
            }         
        }
        private void WindowSetup()
        {
            window.MouseMoved += OnMouseMoved;
            window.Closed += WindowClosed;
            window.SetMouseCursorVisible(false);
        }
        private void ChangeDirection(Vector2f vector)
        {
            if (vector != new Vector2f(0, 0))
            {
                direction = vector;
            }
        }
        private void TextSetup(Color color)
        {
            scoreText.FillColor = color;
            scoreText.Position = new Vector2f(0, 0);
            scoreText.CharacterSize = 60;
            Font font = new Font(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) +"\\font.ttf");   
            scoreText.Font = font;
            scoreText.DisplayedString = "0:0";
        }
        private void ChangeTextScore()
        {
            scoreText.DisplayedString = scoreForText[0]+":"+ scoreForText[1];
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
    }
}
