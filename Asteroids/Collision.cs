using System.Numerics;
using Raylib_cs;

namespace Asteroids
{
    internal class Collision
    {
        public Rectangle Bounds;

        public bool CheckCollision(Rectangle collision)
        {
            if (Raylib.CheckCollisionRecs(Bounds, collision))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckCollision(Rectangle collision, Vector2 point)
        {
            if (Raylib.CheckCollisionPointRec(point, collision))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
