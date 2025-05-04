using System.Diagnostics;
using System.Numerics;
using Raylib_cs;

namespace Asteroids
{
    internal class GameObject : Transform
    {
        public bool isActive;

        public override void Move()
        {
            position += direction * speed * Raylib.GetFrameTime();
        }

        public void DieOutOfBounds(float screenWidth, float screenHeight)
        {
            if (position.X >= screenWidth)
            {
                isActive = false;
            }
            else if (position.X < 0)
            {
                isActive = false;

            }
            else if (position.Y >= screenHeight)
            {
                isActive = false;

            }
            else if (position.Y < 0)
            {
                isActive = false;

            }
        }
        public void OutOfBounds(float screenWidth, float screenHeight)
        {
            if (position.X > screenWidth + 1)
            {
                position.X = -60;
            }
            else if (position.X < -61)
            {
                position.X = screenWidth;
            }
            else if (position.Y > screenHeight + 1)
            {
                position.Y = -60;
            }
            else if (position.Y < -61)
            {
                position.Y = screenHeight;
            }
        }
    }
}
