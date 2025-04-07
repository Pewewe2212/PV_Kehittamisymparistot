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
            if (position.X > screenWidth || position.Y < -300 || position.X < -160 || position.Y > screenHeight)
            {
                isActive = false;
            }
        }
    }
}
