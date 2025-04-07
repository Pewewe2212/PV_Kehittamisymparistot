using Raylib_cs;

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
