using System;
using System.Numerics;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Collections.Generic;
using System.IO;

namespace Aero_Hockey
{
    class CircleItemList
    {
        private List<CircleItem> circleItems = new List<CircleItem>();

        public void CreateItem(CircleItem item)
        {
            circleItems.Add(item);
        }
        public void UseSpawnOfItems(Vector2u window)
        {
            foreach(CircleItem item in circleItems)
            {
                 item.ChangePosition(item.RandomSpawn(window));
            }
        }
        public void CheckCircleOnCollisionWithItemsAndUseFeature(CircleObject circle,Vector2u window)
        {
            foreach(CircleItem item in circleItems)
                if (MathExt.CheckForIntersect(item, circle))
                {
                    item.UseFeature(circle);
                    item.ChangePosition(item.RandomSpawn(window)); 
                }
            
        }
        public List<CircleItem> GetList() => circleItems;
    }
    class ObjectsToDrawList
    {
        private List<Drawable> objectsToDraw = new List<Drawable>();
        public void Add(Drawable drawable)
        {
            objectsToDraw.Add(drawable);  
        }
        public void AddItemList(List<CircleItem> list)
        {
            foreach (CircleItem item in list)
            {
                objectsToDraw.Add(item.gameObject);
            }
        }
        public List<Drawable> GetList() => objectsToDraw;
    }
    class Game
    {
        private CircleItemList itemListRef = new CircleItemList();
        private ObjectsToDrawList objectsDrawListRef = new ObjectsToDrawList();

        private GameRacket gameRacket = new GameRacket(Color.Red);
        private Bot bot = new Bot(0.2f);
        private Ball ball = new Ball(Color.White);

        private Clock clock = new Clock();
        private RenderWindow window = new RenderWindow(new VideoMode(1000, 1000), "Game window");
        private Text scoreText = new Text();

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
                Cycle();
            }
        }
        private void Init()
        {
            WindowSetup();
            SetupFieldObjects();
            TextSetup(Color.Blue);
            itemListRef.CreateItem(InvisibleStopper.CreateStopper());
            itemListRef.UseSpawnOfItems(window.Size);
            AddAllDrawableObjectsToList();
        }
        private void Cycle()
        {
            time = clock.ElapsedTime.AsMicroseconds();
            clock.Restart();
            time /= 800;                                              //for smoother movement of ball

            window.Clear();
            window.DispatchEvents();

            
            itemListRef.CheckCircleOnCollisionWithItemsAndUseFeature(gameRacket,window.Size);
            itemListRef.CheckCircleOnCollisionWithItemsAndUseFeature(bot.racket,window.Size);

            gameRacket.ChangePosition(new Vector2f(mouseX - gameRacket.GetRadius(), mouseY - gameRacket.GetRadius()));

            ChangeDirection(MathExt.CheckForIntersectAndDetectDirection(gameRacket, ball));
            ChangeDirection(MathExt.CheckForIntersectAndDetectDirection(bot.racket, ball));

            if (!MathExt.IsVectorBiggerThenWindowY(bot.racket, window.Size))
                bot.MoveTo(ball.GetCenter(), (float)time);
            else
                bot.MoveTo(new Vector2f(window.Size.X / 2, MathExt.GetPercentOf(window.Size.Y, 10)), (float)time);


            if (direction != new Vector2f(0, 0))
                ChangeDirection(ball.Reflect(window.Size, direction, xBorder, yBorder));

            if (direction != new Vector2f(0, 0))
                ball.Move(direction, window.Size, (float)time);

            SomeoneScore();
            DrawObjects();
            window.Display();
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
            objectsDrawListRef.Add(gameRacket.GetGO());
            objectsDrawListRef.Add(ball.GetGO());
            objectsDrawListRef.Add(scoreText);
            objectsDrawListRef.Add(bot.racket.GetGO());
            objectsDrawListRef.AddItemList(itemListRef.GetList());
            
        }
        private void DrawObjects()
        {
            foreach(Drawable shape in objectsDrawListRef.GetList())
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
