using System.Numerics;
using Raylib_cs;

namespace Asteroids
{
    internal class Transform
    {
        public Vector2 position;
        public Vector2 direction;
        public float speed;

        public virtual void Move()
        {
            position += direction * speed * Raylib.GetFrameTime();
        }
    }
}
