using System.Numerics;
using Raylib_cs;

namespace TANKS
{
    internal class Program
    {
        public class Game()
        {
            bool gameRunning = false;
            int width = 1000;
            int height = 600;

            int speed = 20;

            int tank1x = 100;
            int tank1y = 400;

            int tank2x = 880;
            int tank2y = 400;

            int cannonWidth;
            int cannonHeight;
            int cannonX = 15;
            int cannonY = -10;

            int bulletX;
            int bulletY;

            public static void Main()
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
                Raylib.InitWindow(width, height, "TANKS");

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
                // checks for player input
                // Player 1
                if (gameRunning == true)
                {
                    // Movement
                    if (Raylib.IsKeyDown(KeyboardKey.W) && tank1y != 0)
                    {
                        tank1y -= speed * (int)Raylib.GetFrameTime();
                        cannonHeight = 10;
                        cannonWidth = 10;
                        cannonX = 15;
                        cannonY = 15;
                        
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.A) && tank1x != 0)
                    {
                        tank1x -= speed * (int)Raylib.GetFrameTime();
                        cannonHeight = 10;
                        cannonWidth = 15;
                        cannonX = -15;
                        cannonY = 15;
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.S) && tank1y! <= height + 40)
                    {
                        tank1y += speed * (int)Raylib.GetFrameTime();
                        cannonHeight = 15;
                        cannonWidth = 10;
                        cannonX = 15;
                        cannonY = -15;
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.D) && tank1x <= width + 40)
                    {
                        tank1x += speed * (int)Raylib.GetFrameTime();
                        cannonHeight = 10;
                        cannonWidth = 15;
                        cannonX = 15;
                        cannonY = 15;
                    }

                }

                // Player 2
                if (gameRunning == true)
                {
                    // Movement
                    if (Raylib.IsKeyDown(KeyboardKey.Up) && tank2y != 0)
                    {
                        tank2y -= speed * (int)Raylib.GetFrameTime();
                        cannonHeight = 10;
                        cannonWidth = 10;
                        cannonX = 15;
                        cannonY = 15;
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.Left) && tank2x != 0)
                    {
                        tank2x -= speed * (int)Raylib.GetFrameTime();
                        cannonHeight = 10;
                        cannonWidth = 15;
                        cannonX = -15;
                        cannonY = 15;
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.Down) && tank2y! <= height + 40)
                    {
                        tank2y += speed * (int)Raylib.GetFrameTime();
                        cannonHeight = 15;
                        cannonWidth = 10;
                        cannonX = 15;
                        cannonY = -15;
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.Right) && tank2x <= width + 40)
                    {
                        tank2x += speed * (int)Raylib.GetFrameTime();
                        cannonHeight = 10;
                        cannonWidth = 15;
                        cannonX = 15;
                        cannonY = 15;
                    }

                }


            }

            public void DrawGame()
            {
                // draws the game
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Green);
                Wall();
                Tank(tank1x, tank1y, Color.Blue);
                Tank(tank2x, tank2y, Color.Red);



                Raylib.EndDrawing();
            }

            public void Tank(int bodyX, int bodyY, Color color)
            {
                Rectangle tank1Body = new Rectangle(bodyX, bodyY, 40, 40);
                Rectangle tank1Cannon = new Rectangle(bodyX + cannonX, bodyY + cannonY, cannonWidth, cannonHeight);

                Raylib.DrawRectangleRec(tank1Cannon, Color.Black);
                Raylib.DrawRectangleRec(tank1Body, color);
            }

            public void Bullet(int x, int y, bool tank)
            {
                // Make the bullet appear
                Raylib.DrawEllipse(bulletX, bulletY, 5, 5, Color.White);

                // Moves the bullet
                int bulletSpeed = 50;

                while (bulletX < width && bulletX != 0 && bulletY < height && bulletY != 0)
                {
                    // chooses the tank (tank == player 1, !tank == player 2)
                    if (tank)
                    {
                        /// currently not correct, it just goes down right with this code
                        /// need to add something to actually check where to shoot (in the movement code)
                        bulletX = tank1x + bulletSpeed * (int)Raylib.GetFrameTime(); 
                        bulletY = tank1y + bulletSpeed * (int)Raylib.GetFrameTime();
                    }
                    if (!tank)
                    {
                        bulletX = tank2x + bulletSpeed * (int)Raylib.GetFrameTime(); 
                        bulletY = tank2y + bulletSpeed * (int)Raylib.GetFrameTime();
                    }
                }
                bulletSpeed = 50;
            }

            public void Wall()
            {
                int wallX = 200;
                Rectangle wall = new Rectangle(wallX, 100, 50, 400);
                Raylib.DrawRectangleRec(wall, Color.LightGray);

                wallX = 810;
                wall = new Rectangle(wallX, 100, 50, 400);
                Raylib.DrawRectangleRec(wall, Color.LightGray);

            }
        }

        static void Main(string[] args)
        {
            int speed = 20;

            int tank1x = 100;
            int tank1y = 400;

            int tank2x = 880;
            int tank2y = 400;

            int wallsWidth = 10;
            int wallsHeight = 400;
            int wallX = 200;
            int wallY = 100;
            Rectangle wall = new Rectangle(wallX, wallY, wallsWidth, wallsHeight);

            Raylib.InitWindow(1000, 600, "TANKS");

            while (!Raylib.WindowShouldClose())
            {
                //Game loop
                Raylib.BeginDrawing();

                // Clear so stuff doesn't duplicate
                Raylib.ClearBackground(Color.Lime);

                // Draw tanks

                // Draw walls
                wallX = 200;
                wall = new Rectangle(wallX, wallY, wallsWidth, wallsHeight);
                Raylib.DrawRectangleRec(wall, Color.DarkPurple);

                wallX = 810;
                wall = new Rectangle(wallX, wallY, wallsWidth, wallsHeight);
                Raylib.DrawRectangleRec(wall, Color.DarkPurple);

                // movement p1
                if (Raylib.IsKeyDown(KeyboardKey.W))
                {
                    tank1y -= speed * (int)Raylib.GetFrameTime();
                }
                if (Raylib.IsKeyDown(KeyboardKey.A))
                {
                    tank1x -= speed * (int)Raylib.GetFrameTime();
                }
                if (Raylib.IsKeyDown(KeyboardKey.S))
                {
                    tank1y += speed * (int)Raylib.GetFrameTime();
                }
                if (Raylib.IsKeyDown(KeyboardKey.D))
                {
                    tank1x += speed * (int)Raylib.GetFrameTime();
                }

                // movement p2
                if (Raylib.IsKeyDown(KeyboardKey.W))
                {
                    tank2y -= speed * (int)Raylib.GetFrameTime();
                }
                if (Raylib.IsKeyDown(KeyboardKey.A))
                {
                    tank2x -= speed * (int)Raylib.GetFrameTime();
                }
                if (Raylib.IsKeyDown(KeyboardKey.S))
                {
                    tank2y += speed * (int)Raylib.GetFrameTime();
                }
                if (Raylib.IsKeyDown(KeyboardKey.D))
                {
                    tank2x += speed * (int)Raylib.GetFrameTime();
                }


                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }
    }
}
