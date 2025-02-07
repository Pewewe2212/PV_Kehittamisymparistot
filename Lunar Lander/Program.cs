using System.Diagnostics;
using System.Numerics;
using Raylib_cs;

namespace Lunar_Lander
{
    internal class Program
    {
        public class Game
        {
            public bool gameRunning = false;

            public float landerP1x = 50;
            public float landerP1y = 25;

            public float landerP2x = 25;
            public float landerP2y = 75;

            public float landerp3x = 75;
            public float landerp3y = 75;

            public Vector2 landerP1 = new Vector2(50, 25);
            public Vector2 landerP2 = new Vector2(25, 75);
            public Vector2 landerP3 = new Vector2(75, 75);

            public Vector2 boosterP1 = new Vector2(40, 70);
            public Vector2 boosterP2 = new Vector2(50, 120);
            public Vector2 boosterP3 = new Vector2(60, 70);

            public float speed = 0;
            public float startSpeed = 100;
            public float acceleration = 5; // no fucking idea what I'm supposed to imput here

            public void Start()
            {
                Game game = new Game();
                game.Run();
            }

            public void Run()
            {
                Init();
                GameLoop();
            }

            public void Init()
            {
                Raylib.InitWindow(1000, 600, "Lunar Lander");
                gameRunning = true;
            }

            public void GameLoop()
            {
                while (!Raylib.WindowShouldClose() && gameRunning == true)
                {
                    Update();
                    DrawGame();
                }

                Raylib.CloseWindow();
            }

            public void Update()
            {
                landerP1y += speed;
                landerP2y += speed;
                landerp3y += speed;

                landerP1y += (startSpeed + acceleration * Raylib.GetFrameTime());

                if (Raylib.IsKeyDown(KeyboardKey.Space))
                {
                    landerP1y -= speed * 2;
                    landerP2y -= speed * 2;
                    landerp3y -= speed * 2;

                }

                landerP1 = new Vector2(landerP1x, landerP1y);
                boosterP1 = new Vector2(landerP1x - 10, landerP1y + 35);

                landerP2 = new Vector2(landerP2x, landerP2y);
                boosterP2 = new Vector2(landerP2x + 25, landerP2y + 35);

                landerP3 = new Vector2(landerp3x, landerp3y);
                boosterP3 = new Vector2(landerp3x - 15, landerp3y - 5);
            }

            public void DrawGame()
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.DarkGray);

                //the booster    
                Raylib.DrawTriangle(boosterP1, boosterP2, boosterP3, Color.Orange);
                //the lander
                Raylib.DrawTriangle(landerP1, landerP2, landerP3, Color.Maroon);

                //the landing pad
                Raylib.DrawRectangle(800, 550, 300, 50, Color.White);


                Raylib.EndDrawing();
            }
        }

        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }
}
