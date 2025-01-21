using System.Numerics;
using Raylib_cs;

namespace Raylib_Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int Width = 800;
            int Height = 600;
            float Speed = 100f;
            Color color = Color.Yellow;

            Vector2 Position = new Vector2(Width/2, Height/2);
            Vector2 Direction = new Vector2(1, 1);

            int font = 32;
            string text = "DVD";

            Raylib.InitWindow(Width, Height, "Raylib");
            Vector2 textSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), text, font, 2.0f);
            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);
                Position += Direction * Speed * Raylib.GetFrameTime();

                int posX = (int)Position.X;
                int posY = (int)Position.Y;
                Raylib.DrawText(text, posX, posY, font, color);

                if (Position.X >= Width - textSize.X)
                {
                    Direction = new Vector2(-1, Direction.Y);
                    color = Color.Red;
                }
                if (Position.Y >= Height - textSize.Y)
                {
                    Direction = new Vector2(Direction.X, -1);
                    color = Color.Green;
                }
                if (Position.X <= 0)
                {
                    Direction = new Vector2(1, Direction.Y);
                    color = Color.Blue;
                }
                if (Position.Y <= 0)
                {
                    Direction = new Vector2(Direction.X, 1);
                    color = Color.Purple;
                }

                Raylib.EndDrawing();

            }
            Raylib.CloseWindow();

        }
    }
}
