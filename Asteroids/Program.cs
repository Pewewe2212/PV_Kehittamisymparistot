using System.Numerics;
using System.Timers;
using Raylib_cs;

namespace Asteroids
{
    internal class Program
    {
        /// <summary>
        /// TO DO:
        /// Player:
        /// make the player able to die
        /// 
        /// Asteroids:
        /// make the asteroids exist
        /// make the asteroids show up randomly
        /// make the asteroids deadly
        /// 
        /// Other:
        /// Make the enemies 
        /// Make the images work
        /// Make it so you can restart after losing
        /// 
        /// </summary>
        /// 

        /// CURRENTLY WORKING ON:
        /// Making asteroids split when hit
        public class Game()
        {
            public class Bullet
            {
                public GameObject gameObject;
                public Transform transform;
                public Collision collision;
                public Bullet()
                {
                    gameObject = new GameObject();
                    transform = new Transform();
                    transform.speed = 500;
                    collision = new Collision();
                }
            }

            public class Asteroid
            {
                public GameObject gameObject;
                public SpriteRenderer sr;
                public Collision collision;
                public Asteroid()
                {
                    gameObject = new GameObject();
                    sr = new SpriteRenderer();
                    collision = new Collision();

                    gameObject.speed = 200;
                    collision.Bounds.Size = new Vector2(100, 70);


                }

            }

            public class SmallAsteroid
            {
                public GameObject gameObject;
                public SpriteRenderer sr;
                public Collision collision;
                public SmallAsteroid()
                {
                    gameObject = new GameObject();
                    sr = new SpriteRenderer();
                    collision = new Collision();

                    gameObject.speed = 200;
                    collision.Bounds.Size = new Vector2(40, 40);
                }
            }

            // Asteroid things
            private System.Timers.Timer timer1;

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

            // movement of the ship 
            private float acceleration = 600;
            private float max_speed = 300;
            private Vector2 velocity = new Vector2(0, 0);
            private float deltaTime;
            private Vector2 position = new Vector2(100, 100);
            private float directionX;
            private float directionY;
            private Vector2 direction;
            private float rotationAngle;

            // UI things
            private int points;

            // lists
            private List<Bullet> bullets;
            private List<Asteroid> asteroids;
            private List<SmallAsteroid> smallAsteroids;

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
                // add the extra knowledge for the start here
                gameRunning = true;
                gameOver = false;
                bullets = new List<Bullet>();
                asteroids = new List<Asteroid>();
                smallAsteroids = new List<SmallAsteroid>();
                position = new Vector2(screenWidth / 2, screenHeight / 2);
                LoadImages();
                InitTimer();
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
                            Restart();
                            break;
                        }

                        Raylib.EndDrawing();
                    }


                }
                // win
                else if (result == true)
                {


                    while (true)
                    {
                        Raylib.BeginDrawing();
                        Raylib.ClearBackground(Color.White);
                        Raylib.DrawText("YOU WON", screenWidth / 2 - 40, screenHeight / 2 - 20, 40, Color.Black);
                        Raylib.DrawText("Press R to restart", screenWidth / 2 - 40, screenHeight / 2 + 60, 40, Color.Black);

                        if (Raylib.IsKeyPressed(KeyboardKey.R))
                        {
                            Restart();
                            break;
                        }

                        Raylib.EndDrawing();
                    }
                }
            }

            public void Restart()
            {
                gameRunning = true;
                gameOver = false;
                bullets = new List<Bullet>();
                asteroids = new List<Asteroid>();
                smallAsteroids = new List<SmallAsteroid>();
                position = new Vector2(screenWidth / 2, screenHeight / 2);
                points = 0;
                hp = 2;
                rotationAngle = 0;
                velocity = new Vector2(0, 0);
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
                // bullets
                if (gameOver != true)
                {
                    // checks shooting n makes the bullet
                    if (Raylib.IsKeyPressed(KeyboardKey.Space) && attackDelay <= 0)
                    {
                        attackDelay = attackDelayMax;
                        MakeBullet(position, direction);
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
                }


                // enemy spawning
                if (gameOver != true)
                {
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
                    if (smallAsteroids != null)
                    {
                        for (int i = smallAsteroids.Count - 1; i >= 0; i--)
                        {
                            if (!smallAsteroids[i].gameObject.isActive)
                            {
                                smallAsteroids.RemoveAt(i);
                            }
                            else
                            {
                                UpdateAsteroid(smallAsteroids[i]);
                            }
                        }
                    }
                }


                // movement
                if (gameOver != true)
                {
                    // player collision box
                    playerCollision = new Rectangle((int)position.X - 30, (int)position.Y - 30, player.Width - 50, player.Height - 10);

                    // player movement
                    deltaTime = Raylib.GetFrameTime();

                    /// Forward movement
                    directionX = (float)Math.Cos((rotationAngle - 90) * Math.PI / 180);
                    directionY = (float)Math.Sin((rotationAngle - 90) * Math.PI / 180);
                    direction = new Vector2(directionX, directionY);

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

                    // make the ship constantly go towards 0 speed 
                    if (!Raylib.IsKeyDown(KeyboardKey.W))
                    {
                        float friction = 200f;
                        if (velocity.Length() > 0)
                        {
                            Vector2 frictionForce = Vector2.Normalize(velocity) * friction * deltaTime;
                            if (frictionForce.Length() > velocity.Length())
                            {
                                velocity = Vector2.Zero;
                            }
                            else
                            {
                                velocity -= frictionForce;
                            }
                        }
                    }


                    // actually move the ship
                    position += velocity * deltaTime;

                    // makes the ship stay in bounds
                    if (position.X < 0)
                    {
                        position.X = 0;
                    }
                    if (position.X > screenWidth)
                    {
                        position.X = screenWidth;
                    }
                    if (position.Y < 0)
                    {
                        position.Y = 0;
                    }
                    if (position.Y > screenHeight)
                    {
                        position.Y = screenHeight;
                    }
                }

                // checks if the game should end
                if (gameOver != true)
                {
                    if (points >= 50)
                    {
                        GameEnd(true);
                    }

                    if (hp <= 0)
                    {
                        GameEnd(false);
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
                    playerRec = new Rectangle((int)position.X, (int)position.Y, player.Width, player.Height);

                    Raylib.DrawTexturePro(player, fullImageRect, playerRec, new Vector2(playerRec.Width / 2, playerRec.Height / 2), rotationAngle, Color.White);

                    // UI
                    Raylib.DrawText($"Points: {points}", 20, 20, 20, Color.SkyBlue);
                    Raylib.DrawText($"Health: {hp}", 20, 60, 20, Color.Red);

                    foreach (Asteroid asteroid in asteroids)
                    {
                        asteroid.sr.DrawTexture(asteroid.gameObject, asteroidBig);
                    }

                    foreach (SmallAsteroid asteroid in smallAsteroids)
                    {
                        asteroid.sr.DrawTexture(asteroid.gameObject, asteroidSmall);
                    }

                    foreach (Bullet bullet in bullets)
                    {
                        Raylib.DrawCircle((int)bullet.transform.position.X, (int)bullet.transform.position.Y, 2, Color.Yellow);
                    }
                }
                Raylib.EndDrawing();
            }

            // creates asteroids on a timer // Used AI to get the idea, but not the full code
            public void InitTimer()
            {
                timer1 = new System.Timers.Timer();
                timer1.Elapsed += Tick;
                timer1.Interval = 2000;
                timer1.Start();
            }

            public void Tick(Object source, ElapsedEventArgs e)
            {
                CreateAsteroid();
            }

            // bullet stuff
            public void MakeBullet(Vector2 pos, Vector2 direction)
            {
                Bullet bullet = new Bullet();
                bullet.transform.position = pos;
                bullet.transform.direction = direction;
                bullet.gameObject.isActive = true;

                bullets.Add(bullet);
            }
            public void MoveBullet(Bullet bullet)
            {
                bullet.transform.Move();
                bullet.gameObject.DieOutOfBounds(screenWidth, screenHeight);
                foreach (Asteroid asteroid in asteroids)
                {
                    CheckCollision(bullet, asteroid);
                }
                foreach (SmallAsteroid asteroid in smallAsteroids)
                {
                    CheckCollision(bullet, asteroid);
                }
            }

            // asteroid stuff
            public void CreateAsteroid()
            {
                Random random = new Random();
                Asteroid asteroid = new Asteroid();
                asteroid.gameObject.position = new Vector2(random.Next(0, screenWidth), -90);
                asteroid.gameObject.isActive = true;
                asteroid.gameObject.direction = new Vector2((float)random.NextDouble() * 2 - 1, 1);

                asteroids.Add(asteroid);
            }

            public void UpdateAsteroid(Asteroid asteroid)
            {
                asteroid.gameObject.Move();
                asteroid.collision.Bounds = new Rectangle(asteroid.gameObject.position + new Vector2(10, 10), asteroid.collision.Bounds.Size);
                asteroid.gameObject.DieOutOfBounds(screenWidth, screenHeight);

                if (asteroid.collision.CheckCollision(playerCollision))
                {
                    hp -= 1;
                    asteroid.gameObject.isActive = false;
                }
            }

            public void AsteroidSplit(Asteroid asteroidBig)
            {
                Random random = new Random();
                SmallAsteroid asteroid = new SmallAsteroid();
                asteroid.gameObject.position = new Vector2(asteroidBig.gameObject.position.X + random.Next(-50, 21), asteroidBig.gameObject.position.Y + random.Next(-50, 21));
                asteroid.gameObject.isActive = true;
                asteroid.gameObject.direction = new Vector2((float)random.NextDouble() * 2 - 1, 1); //AI helped with math (specifically the random.NextDouble() * 2 - 1)

                smallAsteroids.Add(asteroid);
            }

            public void UpdateAsteroid(SmallAsteroid asteroid)
            {
                asteroid.gameObject.Move();
                asteroid.collision.Bounds = new Rectangle(asteroid.gameObject.position + new Vector2(10, 10), asteroid.collision.Bounds.Size);
                asteroid.gameObject.DieOutOfBounds(screenWidth, screenHeight);

                if (asteroid.collision.CheckCollision(playerCollision))
                {
                    hp -= 1;
                    asteroid.gameObject.isActive = false;
                }
            }

            // every collision check
            // player collisions


            // bullet collisions
            public void CheckCollision(Bullet bullet, Asteroid asteroid)
            {
                if (bullet.collision.CheckCollision(asteroid.collision.Bounds, bullet.gameObject.position))
                {
                    bullet.gameObject.isActive = false;
                    asteroid.gameObject.isActive = false;
                    points += 5;
                    AsteroidSplit(asteroid);
                    AsteroidSplit(asteroid);
                }
            }

            public void CheckCollision(Bullet bullet, SmallAsteroid asteroid)
            {
                if (bullet.collision.CheckCollision(asteroid.collision.Bounds, bullet.gameObject.position))
                {
                    bullet.gameObject.isActive = false;
                    asteroid.gameObject.isActive = false;
                    points += 5;
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
