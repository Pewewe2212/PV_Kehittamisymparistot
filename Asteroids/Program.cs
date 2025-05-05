using System.Diagnostics;
using System.Numerics;
using Raylib_cs;

namespace Asteroids
{
    internal class Program
    {
        public class Game()
        {
            public class Bullet
            {
                public GameObject gameObject;
                public Collision collision;
                public Bullet()
                {
                    gameObject = new GameObject();
                    gameObject.speed = 500;
                    collision = new Collision();
                }
            }

            public class Asteroid
            {
                public GameObject gameObject;
                public SpriteRenderer sr;
                public Collision collision;

                public enum Size
                {
                    Big, Small
                }
                public Size size;

                public Asteroid()
                {
                    gameObject = new GameObject();
                    sr = new SpriteRenderer();
                    collision = new Collision();

                    gameObject.speed = 200;
                }

            }

            public class Ship
            {
                public GameObject gameObject;
                public SpriteRenderer sr;
                public Collision collision;

                public Ship()
                {
                    gameObject = new GameObject();
                    sr = new SpriteRenderer();
                    collision = new Collision();
                }
            }


            // Asteroid things
            public int asteroidAmount = 2;

            // game state booleans
            public bool gameRunning;
            public bool gameOver = false;

            // screen size
            public int screenWidth = 1000;
            public int screenHeight = 600;

            // images
            private Texture2D player;
            private Rectangle playerRec;
            private Texture2D asteroidBig;
            private Texture2D asteroidSmall;

            // extra ship stuff
            private float hp = 2;
            private Rectangle playerCollision;
            private float attackDelay;
            private float attackDelayMax = 500;
            private bool safetySwitch = false;

            // movement of the ship 
            Ship ship = new Ship();
            private float acceleration = 500;
            private float max_speed = 150;
            private Vector2 velocity = new Vector2(0, 0);
            private float deltaTime;
            private float directionX;
            private float directionY;
            private float rotationAngle;

            // UI things
            private int points;

            // lists
            private List<Bullet> bullets;
            private List<Asteroid> asteroids;

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
                Raylib.InitWindow(screenWidth, screenHeight, "Asteroids");
                Restart();
                LoadImages();
            }

            public void LoadImages()
            {
                player = Raylib.LoadTexture("zAsteroidsImages/playerShip2_red.png");
                asteroidBig = Raylib.LoadTexture("zAsteroidsImages/meteorGrey_big2.png");
                asteroidSmall = Raylib.LoadTexture("zAsteroidsImages/meteorGrey_med1.png");
            }

            public void GameEnd(bool result)
            {
                gameOver = true;
                // loss
                if (result == false)
                {
                    while (true)
                    {
                        Raylib.BeginDrawing();
                        Raylib.ClearBackground(Color.White);
                        Raylib.DrawText("YOU LOST", screenWidth / 2 - 100, screenHeight / 2 - 20, 40, Color.Black);
                        Raylib.DrawText("Press R to restart", screenWidth / 2 - 200, screenHeight / 2 + 60, 40, Color.Black);

                        if (Raylib.IsKeyPressed(KeyboardKey.R))
                        {
                            asteroidAmount = 2;
                            Restart();
                            break;
                        }

                        Raylib.EndDrawing();
                    }
                }
                // win
                else if (result == true)
                {
                    Restart();
                }
            }

            public void Restart()
            {
                Random random = new Random();
                gameRunning = true;
                gameOver = false;
                bullets = new List<Bullet>();
                asteroids = new List<Asteroid>();
                ship.gameObject.position = new Vector2(screenWidth / 2, screenHeight / 2);
                hp = 2;
                rotationAngle = 0;
                velocity = new Vector2(0, 0);
                asteroidAmount += 1;
                for (int i = 0; i < asteroidAmount; i++)
                {
                    CreateAsteroid(Asteroid.Size.Big, new Vector2(random.Next(0, screenWidth), random.Next(-300, -90)));
                }
            }

            public void GameLoop()
            {
                while (!Raylib.WindowShouldClose() && gameRunning == true)
                {
                    Update();
                    DrawGame();
                }

                Raylib.UnloadTexture(player);
                Raylib.UnloadTexture(asteroidBig);
                Raylib.UnloadTexture(asteroidSmall);
                Raylib.CloseWindow();
            }

            public void Update()
            {
                // movement
                if (gameOver != true)
                {
                    // player collision box
                    playerCollision = new Rectangle((int)ship.gameObject.position.X - 30, (int)ship.gameObject.position.Y - 30, player.Width - 50, player.Height - 10);

                    // player movement
                    deltaTime = Raylib.GetFrameTime();

                    /// Forward movement
                    directionX = (float)Math.Cos((rotationAngle - 90) * Math.PI / 180);
                    directionY = (float)Math.Sin((rotationAngle - 90) * Math.PI / 180);
                    ship.gameObject.direction = new Vector2(directionX, directionY);

                    if (Raylib.IsKeyDown(KeyboardKey.W))
                    {
                        velocity.X += directionX * acceleration * deltaTime;
                        velocity.Y += directionY * acceleration * deltaTime;

                        if (velocity.Length() > max_speed)
                        {
                            velocity = Vector2.Normalize(velocity) * max_speed;
                        }
                    }

                    // rotation
                    if (Raylib.IsKeyDown(KeyboardKey.A))
                    {
                        rotationAngle -= 180 * deltaTime;
                    }

                    if (Raylib.IsKeyDown(KeyboardKey.D))
                    {
                        rotationAngle += 180 * deltaTime;
                    }

                    if (rotationAngle >= 360 || rotationAngle <= -360)
                    {
                        rotationAngle = 0;
                    }

                    // checks shooting n makes the bullet
                    if (Raylib.IsKeyPressed(KeyboardKey.Space) && attackDelay <= 0)
                    {
                        attackDelay = attackDelayMax;
                        MakeBullet(ship.gameObject.position, ship.gameObject.direction);
                    }

                    // checks the delay
                    if (attackDelay > 0)
                    {
                        attackDelay -= 1000 * Raylib.GetFrameTime();
                    }

                    // removes inactive bullets and moves the needed ones
                    if (bullets != null)
                    {
                        for (int i = bullets.Count - 1; i >= 0; i--)
                        {
                            if (!bullets[i].gameObject.isActive)
                            {
                                bullets.RemoveAt(i);
                            }
                            else
                            {
                                MoveBullet(bullets[i]);
                            }
                        }
                    }

                    // Asteroids
                    if (asteroids != null)
                    {
                        for (int i = asteroids.Count - 1; i >= 0; i--)
                        {
                            if (!asteroids[i].gameObject.isActive)
                            {
                                asteroids.RemoveAt(i);
                            }
                            else
                            {
                                UpdateAsteroid(asteroids[i]);
                            }
                        }
                    }

                    // actually move the ship
                    ship.gameObject.position += velocity * deltaTime;
                    ship.gameObject.OutOfBounds(screenWidth, screenHeight);

                    // checks if the game should end
                    if (hp <= 0)
                    {
                        GameEnd(false);
                    }
                    if (!asteroids.Any())
                    {
                        GameEnd(true);
                    }
                }

            }

            public void DrawGame()
            {
                Raylib.BeginDrawing();
                if (gameOver != true)
                {
                    Raylib.ClearBackground(Color.Black);
                    Rectangle fullImageRect = new Rectangle(0, 0, player.Width, player.Height);
                    playerRec = new Rectangle((int)ship.gameObject.position.X, (int)ship.gameObject.position.Y, player.Width, player.Height);

                    Raylib.DrawTexturePro(player, fullImageRect, playerRec, new Vector2(playerRec.Width / 2, playerRec.Height / 2), rotationAngle, Color.White);

                    // UI
                    Raylib.DrawText($"Points: {points}", 20, 20, 20, Color.SkyBlue);
                    Raylib.DrawText($"Health: {hp}", 20, 60, 20, Color.Red);

                    foreach (Asteroid asteroid in asteroids)
                    {
                        if (asteroid.size == Asteroid.Size.Big)
                        {
                            asteroid.sr.DrawTexture(asteroid.gameObject, asteroidBig);
                        }
                        else if (asteroid.size == Asteroid.Size.Small)
                        {
                            asteroid.sr.DrawTexture(asteroid.gameObject, asteroidSmall);
                        }
                    }

                    foreach (Bullet bullet in bullets)
                    {
                        Raylib.DrawCircle((int)bullet.gameObject.position.X, (int)bullet.gameObject.position.Y, 2, Color.Yellow);
                    }
                }
                Raylib.EndDrawing();
            }

            // bullet stuff
            public void MakeBullet(Vector2 pos, Vector2 direction)
            {
                Bullet bullet = new Bullet();
                bullet.gameObject.position = pos;
                bullet.gameObject.direction = direction;
                bullet.gameObject.isActive = true;

                bullets.Add(bullet);
            }
            public void MoveBullet(Bullet bullet)
            {
                bullet.gameObject.Move();
                bullet.gameObject.OutOfBounds(screenWidth, screenHeight);
                foreach (Asteroid asteroid in asteroids)
                {
                    CheckCollision(bullet, asteroid);
                    if (safetySwitch == true)
                    {
                        safetySwitch = false;
                        break;
                    }
                }
            }

            // asteroid stuff
            public void CreateAsteroid(Asteroid.Size asteroidSize, Vector2 position)
            {
                Random random = new Random();
                Asteroid asteroid = new Asteroid();
                asteroid.gameObject.position = position;
                asteroid.gameObject.isActive = true;
                asteroid.gameObject.speed = random.Next(150, 200);
                asteroid.gameObject.direction = new Vector2((float)random.NextDouble() * 2 - 1, (float)random.NextDouble() * 2 - 1);

                if (asteroidSize == Asteroid.Size.Big)
                {
                    asteroid.collision.Bounds.Size = new Vector2(100, 70);
                    asteroid.size = Asteroid.Size.Big;

                    asteroids.Add(asteroid);
                }
                else if (asteroidSize == Asteroid.Size.Small)
                {
                    asteroid.collision.Bounds.Size = new Vector2(40, 40);
                    asteroid.size = Asteroid.Size.Small;

                    asteroids.Add(asteroid);
                }
            }

            public void UpdateAsteroid(Asteroid asteroid)
            {
                asteroid.gameObject.Move();
                asteroid.collision.Bounds = new Rectangle(asteroid.gameObject.position + new Vector2(10, 10), asteroid.collision.Bounds.Size);
                asteroid.gameObject.OutOfBounds(screenWidth, screenHeight);

                if (asteroid.collision.CheckCollision(playerCollision))
                {
                    hp -= 1;
                    asteroid.gameObject.isActive = false;
                }
            }

            public void AsteroidSplit(Asteroid asteroid)
            {
                if (asteroid.size == Asteroid.Size.Big)
                {
                    CreateAsteroid(Asteroid.Size.Small, asteroid.gameObject.position);
                    CreateAsteroid(Asteroid.Size.Small, asteroid.gameObject.position);
                }
                else if (asteroid.size == Asteroid.Size.Small)
                {
                    points += 5;
                }
                asteroids.Remove(asteroid);
            }

            // bullet collisions
            public void CheckCollision(Bullet bullet, Asteroid asteroid)
            {
                if (bullet.collision.CheckCollision(asteroid.collision.Bounds, bullet.gameObject.position))
                {
                    bullet.gameObject.isActive = false;
                    asteroid.gameObject.isActive = false;
                    points += 5;
                    AsteroidSplit(asteroid);
                    safetySwitch = true;
                }
            }
        }

        private static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }
}
