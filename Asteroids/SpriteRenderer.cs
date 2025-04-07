using Raylib_cs;
using static Asteroids.Program.Game;

namespace Asteroids
{
    internal class SpriteRenderer
    {
        public void DrawTexture(GameObject gameObject, Texture2D texture)
        {
            Raylib.DrawTexture(texture, (int)gameObject.position.X, (int)gameObject.position.Y, Color.White);
        }
    }
}
