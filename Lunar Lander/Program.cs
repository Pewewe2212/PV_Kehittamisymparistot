using System.Diagnostics;
using System.Numerics;
using Raylib_cs;

namespace Lunar_Lander
{
    /// <summary>
    /// Needed:
    /// Make the ship collision end the game **
    /// /// </summary>
    internal class Program
    {
        public class Game
        {
            public bool gameRunning = false;

            public bool gameEnd = false;

            public int fuel = 5000;

            public float landerP1x = 200;
            public float landerP1y = 25;

            public float landerP2x = 175;
            public float landerP2y = 75;

            public float landerP3x = 225;
            public float landerP3y = 75;

            // the real numbers for the Vector2s are applied in Update(), so these are just placeholders
            public Vector2 landerP1 = new Vector2(200, 175);
            public Vector2 landerP2 = new Vector2(175, 225);
            public Vector2 landerP3 = new Vector2(225, 225);

            public Vector2 boosterP1 = new Vector2(40, 70);
            public Vector2 boosterP2 = new Vector2(50, 120);
            public Vector2 boosterP3 = new Vector2(60, 70);

            public float velocity = 0;
            public float acceleration;
            public float engineAcceleration = 30;
            public float direction = 0;
            public float engineDirection = -1;
            public float gravity = 10;

            public Rectangle landingPad = new Rectangle(50, 275, 300, 25);

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
                Raylib.InitWindow(400, 300, "Lunar Lander");
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
                float deltaTime = Raylib.GetFrameTime();

                acceleration = gravity;

                if (Raylib.IsKeyDown(KeyboardKey.Space) && fuel >= 0)
                {
                    acceleration -= engineAcceleration;
                    fuel -= 1;
                }

                if (landerP2y <= 275)
                {
                    velocity += acceleration * deltaTime;

                    landerP1y += velocity * deltaTime;
                    landerP2y += velocity * deltaTime;
                    landerP3y += velocity * deltaTime;

                    landerP1 = new Vector2(landerP1x, landerP1y);
                    boosterP1 = new Vector2(landerP1x - 10, landerP1y + 35);

                    landerP2 = new Vector2(landerP2x, landerP2y);
                    boosterP2 = new Vector2(landerP2x + 25, landerP2y + 35);

                    landerP3 = new Vector2(landerP3x, landerP3y);
                    boosterP3 = new Vector2(landerP3x - 15, landerP3y - 5);
                }

                if (Raylib.CheckCollisionPointRec(landerP2, landingPad))
                {
                    gameEnd = true;
                }

                Debug.WriteLine(velocity);

                if (gameEnd == true)
                {
                    if (velocity <= 15)
                    {
                        Raylib.DrawText("You Won", 200, 30, 30, Color.Gold);
                    }
                    else
                    {
                        Raylib.DrawText("You Failed", 200, 30, 30, Color.Maroon);
                        landerP1 = new Vector2(276, 276);
                        landerP2 = new Vector2(500, 500);
                        landerP3 = new Vector2(500, 500);
                    }
                }

            }


            public void DrawGame()
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.DarkGray);

                if (Raylib.IsKeyDown(KeyboardKey.Space) && fuel >= 0)
                {
                    //the booster    
                    Raylib.DrawTriangle(boosterP1, boosterP2, boosterP3, Color.Orange);
                }

                //the lander
                Raylib.DrawTriangle(landerP1, landerP2, landerP3, Color.Maroon);

                // the fuel
                Raylib.DrawRectangle(20, 20, 110, 30, Color.LightGray);
                Raylib.DrawRectangle(25, 25, fuel / 50, 20, Color.Red);

                //the landing pad
                Raylib.DrawRectangle(50, 275, 300, 25, Color.White);


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
