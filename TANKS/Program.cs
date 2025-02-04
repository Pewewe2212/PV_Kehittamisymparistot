using System.Numerics;
using Raylib_cs;

namespace TANKS
{
    internal class Program
    {
        struct Bullet
        {
            public float x, y;
            public Vector2 xy;
            public float speedX, speedY;
            public bool active;
        }

        public class Game()
        {
            bool gameRunning = false;
            bool gameOver = false;
            int winner = 0;

            int width = 1000;
            int height = 600;

            int speed = 200;

            float tank1x = 100;
            float tank1y = 400;

            float tank2x = 880;
            float tank2y = 400;

            float cannonWidth;
            float cannonHeight;
            float cannonX = 15;
            float cannonY = -10;

            float cannonWidthp2;
            float cannonHeightp2;
            float cannonXp2 = 15;
            float cannonYp2 = -10;

            Bullet bulletP1;
            Bullet bulletP2;

            float bulletXDiretionp1 = 1;
            float bulletYDiretionp1 = 1;
            float bulletXDiretionp2 = 1;
            float bulletYDiretionp2 = 1;

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
                Raylib.InitWindow(width, height, "TANKS");

                bulletP1 = new Bullet { active = false };
                bulletP2 = new Bullet { active = false };

                gameRunning = true;

                // Extra stuff to make the cannons look right
                cannonHeight = 10;
                cannonWidth = 15;
                cannonX = tank1x + 25 + 15;
                cannonY = tank1y + 15;
                cannonHeightp2 = 10;
                cannonWidthp2 = 15;
                cannonXp2 = tank2x - 15;
                cannonYp2 = tank2y + 15;
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
                Rectangle wallL = Wall(200);
                Rectangle wallR = Wall(810);

                Rectangle tank1 = Tank(tank1x, tank1y);
                Rectangle tank2 = Tank(tank2x, tank2y);

                float movement = speed * Raylib.GetFrameTime();

                // checks for player input
                // Player 1
                if (gameOver == false)
                {
                    // Movement
                    if (Raylib.IsKeyDown(KeyboardKey.W) && tank1y >= 0)
                    {
                        tank1y -= movement;
                        cannonHeight = 15;
                        cannonWidth = 10;
                        cannonX = tank1x + 15;
                        cannonY = tank1y - 15;
                        bulletYDiretionp1 = -1;
                        bulletXDiretionp1 = 0;

                        if (Raylib.CheckCollisionRecs(tank1, wallL) || Raylib.CheckCollisionRecs(tank1, wallR) || Raylib.CheckCollisionRecs(tank1, tank2))
                        {
                            tank1y += movement + 1;
                        }
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.A) && tank1x >= 0)
                    {
                        tank1x -= movement;
                        cannonHeight = 10;
                        cannonWidth = 15;
                        cannonX = tank1x - 15;
                        cannonY = tank1y + 15;
                        bulletXDiretionp1 = -1;
                        bulletYDiretionp1 = 0;

                        if (Raylib.CheckCollisionRecs(tank1, wallL) || Raylib.CheckCollisionRecs(tank1, wallR) || Raylib.CheckCollisionRecs(tank1, tank2))
                        {
                            tank1x += movement + 1;
                        }
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.S) && tank1y <= height - 40)
                    {
                        tank1y += movement;
                        cannonHeight = 15;
                        cannonWidth = 10;
                        cannonX = tank1x + 15;
                        cannonY = tank1y + 15 + 25;
                        bulletYDiretionp1 = 1;
                        bulletXDiretionp1 = 0;

                        if (Raylib.CheckCollisionRecs(tank1, wallL) || Raylib.CheckCollisionRecs(tank1, wallR) || Raylib.CheckCollisionRecs(tank1, tank2))
                        {
                            tank1y -= movement + 1;
                        }
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.D) && tank1x <= width - 40)
                    {
                        tank1x += movement;
                        cannonHeight = 10;
                        cannonWidth = 15;
                        cannonX = tank1x + 25 + 15;
                        cannonY = tank1y + 15;
                        bulletXDiretionp1 = 1;
                        bulletYDiretionp1 = 0;

                        if (Raylib.CheckCollisionRecs(tank1, wallL) || Raylib.CheckCollisionRecs(tank1, wallR) || Raylib.CheckCollisionRecs(tank1, tank2))
                        {
                            tank1x -= movement + 1;
                        }

                    }
                    if (Raylib.IsKeyDown(KeyboardKey.Space))
                    {
                        Bullet(true);
                    }

                }

                // Player 2
                if (gameOver == false)
                {
                    // Movement
                    if (Raylib.IsKeyDown(KeyboardKey.Up) && tank2y >= 0)
                    {
                        tank2y -= movement;
                        cannonHeightp2 = 15;
                        cannonWidthp2 = 10;
                        cannonXp2 = tank2x + 15;
                        cannonYp2 = tank2y - 15;
                        bulletYDiretionp2 = -1;
                        bulletXDiretionp2 = 0;

                        if (Raylib.CheckCollisionRecs(tank2, wallL) || Raylib.CheckCollisionRecs(tank2, wallR) || Raylib.CheckCollisionRecs(tank1, tank2))
                        {
                            tank2y += movement + 1;
                        }
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.Left) && tank2x >= 0)
                    {
                        tank2x -= movement;
                        cannonHeightp2 = 10;
                        cannonWidthp2 = 15;
                        cannonXp2 = tank2x - 15;
                        cannonYp2 = tank2y + 15;
                        bulletXDiretionp2 = -1;
                        bulletYDiretionp2 = 0;

                        if (Raylib.CheckCollisionRecs(tank2, wallL) || Raylib.CheckCollisionRecs(tank2, wallR) || Raylib.CheckCollisionRecs(tank1, tank2))
                        {
                            tank2x += movement + 1;
                        }
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.Down) && tank2y <= height - 40)
                    {
                        tank2y += movement;
                        cannonHeightp2 = 15;
                        cannonWidthp2 = 10;
                        cannonXp2 = tank2x + 15;
                        cannonYp2 = tank2y + 15 + 25;
                        bulletYDiretionp2 = 1;
                        bulletXDiretionp2 = 0;

                        if (Raylib.CheckCollisionRecs(tank2, wallL) || Raylib.CheckCollisionRecs(tank2, wallR) || Raylib.CheckCollisionRecs(tank1, tank2))
                        {
                            tank2y -= movement + 1;
                        }
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.Right) && tank2x <= width - 40)
                    {
                        tank2x += movement;
                        cannonHeightp2 = 10;
                        cannonWidthp2 = 15;
                        cannonXp2 = tank2x + 25 + 15;
                        cannonYp2 = tank2y + 15;
                        bulletXDiretionp2 = 1;
                        bulletYDiretionp2 = 0;

                        if (Raylib.CheckCollisionRecs(tank2, wallL) || Raylib.CheckCollisionRecs(tank2, wallR) || Raylib.CheckCollisionRecs(tank1, tank2))
                        {
                            tank2x -= movement + 1;
                        }
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.Enter))
                    {
                        Bullet(false);
                    }

                }

                // Move Player 1 Bullet
                if (bulletP1.active)
                {
                    bulletP1.x += bulletP1.speedX * Raylib.GetFrameTime();
                    bulletP1.y += bulletP1.speedY * Raylib.GetFrameTime();


                    if (bulletP1.x <= 0 || bulletP1.x >= width || bulletP1.y <= 0 || bulletP1.y >= height || Raylib.CheckCollisionPointRec(bulletP1.xy, wallL) || Raylib.CheckCollisionPointRec(bulletP1.xy, wallR))
                    {
                        bulletP1.active = false;
                        bulletP1.xy = new Vector2(tank1x, tank1y);
                    }
                }

                // Move Player 2 Bullet
                if (bulletP2.active)
                {
                    bulletP2.x += bulletP2.speedX * Raylib.GetFrameTime();
                    bulletP2.y += bulletP2.speedY * Raylib.GetFrameTime();

                    if (bulletP2.x <= 0 || bulletP2.x >= width || bulletP2.y <= 0 || bulletP2.y >= height || Raylib.CheckCollisionPointRec(bulletP2.xy, wallL) || Raylib.CheckCollisionPointRec(bulletP2.xy, wallR))
                    {
                        bulletP2.active = false;
                        bulletP2.xy = new Vector2(tank2x, tank2y);
                    }
                }

                // Kills player when hit by enemy bullet
                if (Raylib.CheckCollisionPointRec(bulletP2.xy, tank1))
                {
                    gameOver = true;
                    winner = 2;
                }

                if (Raylib.CheckCollisionPointRec(bulletP1.xy, tank2))
                {
                    gameOver = true;
                    winner = 1;
                }

                // Reset the game
                if (gameOver == true)
                {
                    if (Raylib.IsKeyDown(KeyboardKey.LeftControl) || Raylib.IsKeyDown(KeyboardKey.RightControl))
                    {
                        gameOver = false;
                        winner = 0;
                        tank1x = 100;
                        tank1y = 400;
                        tank2x = 880;
                        tank2y = 400;
                        cannonHeight = 10;
                        cannonWidth = 15;
                        cannonX = tank1x + 25 + 15;
                        cannonY = tank1y + 15;
                        cannonHeightp2 = 10;
                        cannonWidthp2 = 15;
                        cannonXp2 = tank2x - 15;
                        cannonYp2 = tank2y + 15;
                    }
                }
            }

            public void DrawGame()
            {
                // draws the game
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.LightGray);
                if (gameOver == false)
                {
                    // draws bullets when shot
                    if (bulletP1.active)
                    {
                        Raylib.DrawRectangle((int)bulletP1.x, (int)bulletP1.y, 5, 5, Color.Black);
                        bulletP1.xy = new Vector2(bulletP1.x, bulletP1.y);
                    }

                    if (bulletP2.active)
                    {
                        Raylib.DrawRectangle((int)bulletP2.x, (int)bulletP2.y, 5, 5, Color.Black);
                        bulletP2.xy = new Vector2(bulletP2.x, bulletP2.y);
                    }

                    // draws the walls and player
                    Rectangle wallL = Wall(200);
                    Raylib.DrawRectangleRec(wallL, Color.DarkBlue);
                    Rectangle wallR = Wall(780);
                    Raylib.DrawRectangleRec(wallR, Color.Maroon);

                    Rectangle tank1 = Tank(tank1x, tank1y);
                    Raylib.DrawRectangleRec(tank1, Color.Blue);
                    Cannon(cannonX, cannonY, cannonWidth, cannonHeight, Color.Black);

                    Rectangle tank2 = Tank(tank2x, tank2y);
                    Raylib.DrawRectangleRec(tank2, Color.Red);
                    Cannon(cannonXp2, cannonYp2, cannonWidthp2, cannonHeightp2, Color.Black);
                }


                if (winner == 1 && gameOver == true)
                {
                    Raylib.DrawText("Blue tank has WON!", 200, 200, 50, Color.Gold);
                    Raylib.DrawText("Press Ctrl to restart", 200, 400, 50, Color.Gold);
                }
                else if (winner == 2 && gameOver == true)
                {
                    Raylib.DrawText("Red tank has WON!", 200, 200, 50, Color.Gold);
                    Raylib.DrawText("Press Ctrl to restart", 200, 400, 50, Color.Gold);
                }

                Raylib.EndDrawing();
            }

            public Rectangle Tank(float bodyX, float bodyY)
            {
                Rectangle tankBody = new Rectangle(bodyX, bodyY, 40, 40);

                return tankBody;
            }

            public void Cannon(float bodyX, float bodyY, float width, float height, Color color)
            {
                Rectangle tankCannon = new Rectangle(bodyX, bodyY, width, height);
                Raylib.DrawRectangleRec(tankCannon, color);
            }

            public void Bullet(bool tank)
            {
                // uses bool to determine which player is shooting (tank(true) = player 1, !tank(false) = player2)
                if (tank)
                {
                    if (!bulletP1.active)
                    {
                        bulletP1.x = tank1x + 20;
                        bulletP1.y = tank1y + 20;
                        bulletP1.speedX = bulletXDiretionp1 * 600;
                        bulletP1.speedY = bulletYDiretionp1 * 600;
                        bulletP1.active = true;
                    }
                }
                else
                {
                    if (!bulletP2.active)
                    {
                        bulletP2.x = tank2x + 20;
                        bulletP2.y = tank2y + 20;
                        bulletP2.speedX = bulletXDiretionp2 * 600;
                        bulletP2.speedY = bulletYDiretionp2 * 600;
                        bulletP2.active = true;
                    }
                }
            }

            public Rectangle Wall(int wallX)
            {
                Rectangle wall = new Rectangle(wallX, 100, 50, 400);
                return wall;
            }
        }

        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }
}
