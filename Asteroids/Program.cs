using System.Diagnostics;
using System.Numerics;
using Raylib_cs;

namespace Asteroids
{
    internal class Program
    {
        /// <summary>
        /// TO DO:
        /// Player:
        /// Make the player able to attack
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
        /// Ship Image moving correctly
        public class Game()
        {
            public struct Bullet
            {
                public Vector2 position;
                public Vector2 direction;
                public bool isActive;
            }

            // game state booleans
            public bool gameRunning;
            public bool gameOver = false;

            // images
            Texture2D player;
            float playerWidth;
            Rectangle playerRec;
            //Texture2D enemy = Raylib.LoadTexture("C:/Tiedostot/zAsteroidsImages/PNG/ufoBlue.png");
            //Texture2D asteroidBig = Raylib.LoadTexture("C:/Tiedostot/zAsteroidsImages/PNG/Meteors/meteorGrey_big2.png");

            // movement of the ship 
            float acceleration = 1000;
            float max_speed = 200;
            Vector2 velocity = new Vector2(0, 0);
            float deltaTime;
            Vector2 position = new Vector2(100, 100);
            float directionX;
            float directionY;
            Vector2 direction;
            float rotationAngle;

            // bullet stuff
            List<Bullet> bullets;

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
                Raylib.InitWindow(1000, 600, "Asteroids");

                // add the extra knowledge for the start here
                gameRunning = true;
                gameOver = false;
                bullets = new List<Bullet>();
                LoadImages();
            }

            public void LoadImages()
            {
                player = Raylib.LoadTexture("C:/zAsteroidsImages/PNG/playerShip2_red.png");
                playerWidth = player.Width;
            }

            public void GameLoop()
            {
                while (!Raylib.WindowShouldClose() && gameRunning == true)
                {
                    Update();
                    DrawGame();
                }

                Raylib.UnloadTexture(player);
                Raylib.CloseWindow();
            }

            public void Update()
            {
                // movement
                if (gameOver != true)
                {
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
                    Delay(100);


                    Debug.WriteLine(velocity);

                    // actually move the shit
                    position += velocity * deltaTime;
                }

                // bullets
                if (gameOver != true)
                {
                    // checks shooting n makes the bullet
                    if (Raylib.IsKeyPressed(KeyboardKey.Space))
                    {
                        MakeBullet(position, direction);
                    }

                    // removes unneccesary bullets
                    if (bullets != null)
                    {
                        foreach (Bullet bullet in bullets)
                        {
                            if (bullet.isActive == false)
                            {
                                bullets.Remove(bullet);
                            }
                            MoveBullet(bullet);
                        }
                    }
                }


                // enemy spawning
                if (gameOver != true)
                {
                    // input the enemy spawners here
                }
            }

            // slows down the ship
            public async Task Delay(int time)
            {
                await Task.Delay(time);
                SlowDown();
            }
            public void SlowDown()
            {
                if (velocity.Y > 0 && !Raylib.IsKeyDown(KeyboardKey.W))
                {
                    velocity.Y -= 1;
                    velocity.Y = (int)velocity.Y;
                }
                if (velocity.Y < 0 && !Raylib.IsKeyDown(KeyboardKey.W))
                {
                    velocity.Y += 1;
                    velocity.Y = (int)velocity.Y;
                }
                if (velocity.X > 0 && !Raylib.IsKeyDown(KeyboardKey.W))
                {
                    velocity.X -= 1;
                    velocity.X = (int)velocity.X;
                }
                if (velocity.X < 0 && !Raylib.IsKeyDown(KeyboardKey.W))
                {
                    velocity.X += 1;
                    velocity.X = (int)velocity.X;
                }
            }


            // bullet shit
            public void MakeBullet(Vector2 pos, Vector2 direction)
            {
                Bullet bullet = new Bullet();
                bullet.position = pos;
                bullet.direction = direction;
                bullet.isActive = true;

                bullets.Add(bullet);
            }
            public void MoveBullet(Bullet bullet)
            {
                bullet.position += bullet.direction;
            }

            public void DrawGame()
            {
                Raylib.BeginDrawing();
                if (gameOver != true)
                {
                    Raylib.ClearBackground(Color.LightGray);
                    Rectangle fullImageRect = new Rectangle(0, 0, player.Width, player.Height);
                    playerRec = new Rectangle((int)position.X, (int)position.Y, player.Width, player.Height);
                    //Raylib.DrawRectanglePro(rect, new Vector2(rect.Width / 2, rect.Height / 2), rotationAngle, Color.DarkPurple);

                    Raylib.DrawTexturePro(player, fullImageRect, playerRec, new Vector2(playerRec.Width / 2, playerRec.Height / 2), rotationAngle, Color.White);

                    foreach (Bullet bullet in bullets)
                    {
                        Raylib.DrawCircle((int)bullet.position.X, (int)bullet.position.Y, 2, Color.Yellow);
                    }
                }

                Raylib.EndDrawing();

            }

            public void CreateEnemy(Random random)
            {

            }

            public void CreateAsteroid(Random random)
            {

            }
        }

        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }
}
