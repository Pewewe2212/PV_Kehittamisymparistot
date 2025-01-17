using System.Numerics;
using Raylib_cs;

namespace Raylib_Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Vector2 Position = new Vector2(100, 100);
            Vector2 Direction = new Vector2(1, 1);
            float Speed = 10f;

            Raylib.InitWindow(1000, 600, "Raylib");
            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);
                Position += Direction * Speed * Raylib.GetFrameTime();
                int posX = (int)Position.X;
                int posY = (int)Position.Y;
                Raylib.DrawText("DVD", posX, posY, 32, Color.Yellow);
                if (Position.X >= Raylib.GetScreenWidth())
                {
                    Direction = new Vector2(-1, Direction.Y);
                }
                if (Position.Y >= Raylib.GetScreenHeight())
                {
                    Direction = new Vector2(Direction.X, -1);
                }
                if (Position.X <= 0)
                {
                    Direction = new Vector2(1, Direction.Y);
                }
                if (Position.Y <= 0)
                {
                    Direction = new Vector2(Direction.X, 1);
                }

            }
            Raylib.EndDrawing();
            Raylib.CloseWindow();

        }
    }
}
