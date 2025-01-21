using System.Numerics;
using Raylib_cs;

namespace Pong
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Screen size
            int Width = 1000;
            int Height = 500;

            // Players
            int P1Y = 100;
            int P2Y = 100;
            int P1Points = 0;
            int P2Points = 0;
            int RectangleHeight = 150;

            // Ball things
            float BallX = Width / 2;
            float BallY = Height / 2;
            float BallXDirection = -1;
            float BallYDirection = 1;
            float BallSpeed = 200;

            Raylib.InitWindow(Width, Height, "Pong");

            while (!Raylib.WindowShouldClose())
            {
                // Draw everything
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);
                Rectangle P1 = new Rectangle(100, P1Y, 40, RectangleHeight);
                Raylib.DrawRectangleRec(P1, Color.White);
                Rectangle P2 = new Rectangle(Width - 100, P2Y, 40, RectangleHeight);
                Raylib.DrawRectangleRec(P2, Color.White);
                Vector2 Ball = new Vector2(BallX, BallY);
                Raylib.DrawCircleV(Ball, 10, Color.White);

                // Scoreboard visuals
                Vector2 P1textSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), P1Points.ToString(), 80, 2.0f);
                Vector2 P2textSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), P2Points.ToString(), 80, 2.0f);
                Raylib.DrawText(P1Points.ToString(), Width / 2 - (int)P1textSize.X * 2, 20, 80, Color.White);
                Raylib.DrawText(P2Points.ToString(), Width / 2 + (int)P2textSize.X, 20, 80, Color.White);


                // P1 Movement
                if (Raylib.IsKeyDown(KeyboardKey.W))
                {
                    if (P1Y > 0)
                    {
                        P1Y -= 1;
                        System.Threading.Thread.Sleep(1);
                    }
                }

                if (Raylib.IsKeyDown(KeyboardKey.S))
                {
                    if (P1Y < Height - RectangleHeight)
                    {
                        P1Y += 1;
                        System.Threading.Thread.Sleep(1);
                    }
                }

                // P2 Movement
                if (Raylib.IsKeyDown(KeyboardKey.Up))
                {
                    if (P2Y > 0)
                    {
                        P2Y -= 1;
                        System.Threading.Thread.Sleep(1);
                    }
                }

                if (Raylib.IsKeyDown(KeyboardKey.Down))
                {
                    if (P2Y < Height - RectangleHeight)
                    {
                        P2Y += 1;
                        System.Threading.Thread.Sleep(1);
                    }
                }

                // Ball movement
                BallX += BallXDirection * BallSpeed * Raylib.GetFrameTime(); 
                BallY += BallYDirection * BallSpeed * Raylib.GetFrameTime();

                if (Raylib.CheckCollisionPointRec(Ball, P1))
                {
                    BallXDirection *= -1;
                    BallX += 10;
                    BallSpeed += 20;
                }

                if (Raylib.CheckCollisionPointRec(Ball, P2))
                {
                    BallXDirection *= -1;
                    BallX -= 10;
                    BallSpeed += 20;
                }

                if (BallX < 0)
                {
                    BallX = Width / 2;
                    BallY = Height / 2;
                    P2Points += 1;
                    BallSpeed = 200;
                }

                if (BallX > Width)
                {
                    BallX = Width / 2;
                    BallY = Height / 2;
                    P1Points += 1;
                    BallSpeed = 200;
                }

                if (BallY < 0)
                {
                    BallYDirection *= -1;
                    BallY += 5;
                }

                if (BallY > Height)
                {
                    BallYDirection *= -1;
                    BallY -= 5;
                }
                
                Raylib.EndDrawing();
            }
            Raylib.CloseWindow();
        }
    }
}
