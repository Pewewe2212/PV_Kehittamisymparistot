using System.Diagnostics;
using System.Numerics;
using Raylib_cs;

namespace Artillery
{
    internal class Program
    {
        public class Game()
        {
            struct Bullet
            {
                public float x, y;
                public Vector2 xy;
                public bool active;
            }

            // GamePlay stuff
            public bool gameRunning = false;
            public bool gameOver = false;
            public int turn = 0; // 0 = player1, 1 = player2
            public int pointsP1 = 0;
            public int pointsP2 = 0;

            //the ground
            public List<Rectangle> ground;

            //// Player stuff
            // Tank position stuff
            public float tank1X = 55;
            public float tank1Y = 80;

            public float tank2X = 905;
            public float tank2Y = 80;

            /// cannons
            // p1
            Vector2 directionP1;
            public float angleP1;
            float turnP1 = 0;
            float angleCheckP1;

            // p2
            Vector2 directionP2;
            float angleP2;
            float turnP2 = 0;
            float angleCheckP2;

            bool moved = false;
            bool shot = false;

            // bullets
            Bullet bulletP1;
            Bullet bulletP2;
            bool poof = false;
            bool explosionP1 = false;
            bool explosionP2 = false;

            int bulletTypeP1;
            int bulletTypeP2;

            //// bullet movement
            // Player 1
            public float velocity = -240;
            public float acceleration;
            public float direction = 0;
            public float gravity = 100;


            // Player 2
            public float velocity2 = -240;
            public float acceleration2;
            public float direction2 = 0;
            public float gravity2 = 100;

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
                Raylib.InitWindow(1000, 600, "Artillery");

                gameRunning = true;

                bulletP1 = new Bullet { active = false };
                bulletP2 = new Bullet { active = false };

                bulletTypeP1 = 1;
                bulletTypeP2 = 1;

                ground = new List<Rectangle>();
                Ground();

                int pos = (int)tank1X / 50;
                tank1Y = 600 - ground[pos].Height - 40;

                pos = (int)tank2X / 50;
                tank2Y = 600 - ground[pos].Height - 40;

                pointsP1 = 0;
                pointsP2 = 0;
            }

            public void GameLoop()
            {
                while (!Raylib.WindowShouldClose() && gameRunning == true)
                {
                    Update();
                    DrawGame();
                }

                Raylib.CloseWindow();
            }

            public void Update()
            {
                // player 1
                if (!gameOver)
                {
                    if (turn == 0 && shot == false)
                    {
                        if (Raylib.IsKeyDown(KeyboardKey.One))
                        {
                            bulletTypeP1 = 1;
                        }
                        if (Raylib.IsKeyDown(KeyboardKey.Two))
                        {
                            // change the bullet properties for whatever you want cannon 2 to shoot
                            bulletTypeP1 = 2;
                        }

                        if (Raylib.IsKeyDown(KeyboardKey.Space))
                        {
                            shot = true;
                            explosionP1 = false;
                            explosionP2 = false;
                            // make the player shoot
                            _Bullet(false);
                        }

                        // the moving
                        if (Raylib.IsKeyDown(KeyboardKey.A) && moved != true && tank1X != 5)
                        {
                            moved = true;
                            tank1X -= 50;
                            int pos = (int)tank1X / 50;
                            tank1Y = 600 - ground[pos].Height - 40;
                        }
                        if (Raylib.IsKeyDown(KeyboardKey.D) && moved != true && tank1X != 955)
                        {
                            moved = true;
                            tank1X += 50;
                            int pos = (int)tank1X / 50;
                            tank1Y = 600 - ground[pos].Height - 40;
                        }

                        // the shooting
                        // cannon movement
                        if (Raylib.IsKeyDown(KeyboardKey.Q))
                        {
                            // turn left
                            turnP1 = -1;
                            angleP1 += turnP1 * Raylib.GetFrameTime();
                            directionP1 = Vector2.Normalize(directionP1);

                            Matrix4x4 rotation = Matrix4x4.CreateRotationZ(angleP1);

                            Vector2 Right = new Vector2(1.0f, 0.0f);
                            Debug.WriteLine(rotation);
                            directionP1 = Vector2.Transform(Right, rotation);

                        }
                        if (Raylib.IsKeyDown(KeyboardKey.E))
                        {
                            // turn right
                            turnP1 = 1;
                            angleP1 += turnP1 * Raylib.GetFrameTime();
                            directionP1 = Vector2.Normalize(directionP1);

                            Matrix4x4 rotation = Matrix4x4.CreateRotationZ(angleP1);

                            Vector2 Right = new Vector2(1.0f, 0.0f);
                            directionP1 = Vector2.Transform(Right, rotation);

                        }
                    }
                }



                // player 2
                if (!gameOver)
                {
                    if (turn == 1 && shot == false)
                    {
                        if (Raylib.IsKeyDown(KeyboardKey.One))
                        {
                            bulletTypeP2 = 1;
                        }
                        if (Raylib.IsKeyDown(KeyboardKey.Two))
                        {
                            bulletTypeP2 = 2;
                        }

                        if (Raylib.IsKeyDown(KeyboardKey.Space))
                        {
                            shot = true;
                            explosionP1 = false;
                            explosionP2 = false;
                            /// make the player shoot
                            _Bullet(true);

                        }

                        // the moving
                        if (Raylib.IsKeyDown(KeyboardKey.A) && moved != true && tank2X != 5)
                        {
                            moved = true;
                            tank2X -= 50;
                            int pos = (int)tank2X / 50;
                            tank2Y = 600 - ground[pos].Height - 40;
                        }
                        if (Raylib.IsKeyDown(KeyboardKey.D) && moved != true && tank2X != 955)
                        {
                            moved = true;
                            tank2X += 50;
                            int pos = (int)tank2X / 50;
                            tank2Y = 600 - ground[pos].Height - 40;
                        }

                        // the shooting
                        // cannon movement
                        if (Raylib.IsKeyDown(KeyboardKey.Q))
                        {
                            // turn left
                            turnP2 = -1;
                            angleP2 += turnP2 * Raylib.GetFrameTime();
                            directionP2 = Vector2.Normalize(directionP2);

                            Matrix4x4 rotation = Matrix4x4.CreateRotationZ(angleP2);

                            Vector2 Right = new Vector2(1.0f, 0.0f);
                            directionP2 = Vector2.Transform(Right, rotation);
                        }
                        if (Raylib.IsKeyDown(KeyboardKey.E))
                        {
                            // turn right
                            turnP2 = 1;
                            angleP2 += turnP2 * Raylib.GetFrameTime();
                            directionP2 = Vector2.Normalize(directionP2);

                            Matrix4x4 rotation = Matrix4x4.CreateRotationZ(angleP2);

                            Vector2 Right = new Vector2(1.0f, 0.0f);
                            directionP2 = Vector2.Transform(Right, rotation);
                        }
                    }
                }

                // bullets

                int again = 0;
                bool hit = false;

                if (bulletP1.active)
                {
                    acceleration = gravity;
                    bulletP1.xy = new Vector2(bulletP1.x, bulletP1.y);
                    angleCheckP1 = MathF.Atan2(directionP1.Y, directionP1.X);
                    Debug.WriteLine(angleCheckP1);
                    if (bulletTypeP1 == 1)
                    {
                        if (angleCheckP1 > -1.6)
                        {
                            while (again < 2)
                            {
                                velocity += acceleration * Raylib.GetFrameTime();
                                bulletP1.y += velocity * (angleCheckP1 * -1) * Raylib.GetFrameTime();
                                bulletP1.x += 100 * (angleCheckP1 * -1) * Raylib.GetFrameTime() / (angleCheckP1 * -1);
                                again += 1;
                            }
                            velocity += acceleration * Raylib.GetFrameTime();
                            bulletP1.y += velocity * Raylib.GetFrameTime();
                            bulletP1.x += 100 * (angleCheckP1 * -1) * Raylib.GetFrameTime() / (angleCheckP1 * -1);
                        }
                        if (angleCheckP1 < -1.6)
                        {
                            while (again < 2)
                            {
                                velocity += acceleration * Raylib.GetFrameTime();
                                bulletP1.y += velocity * (angleCheckP1 * -1) * Raylib.GetFrameTime();
                                bulletP1.x += 100 * angleCheckP1 * Raylib.GetFrameTime() / (angleCheckP1 * -1);
                                again += 1;
                            }
                            velocity += acceleration * Raylib.GetFrameTime();
                            bulletP1.y += velocity * Raylib.GetFrameTime();
                            bulletP1.x += 100 * angleCheckP1 * Raylib.GetFrameTime() / (angleCheckP1 * -1);
                        }

                    }
                    else if (bulletTypeP1 == 2)
                    {
                        if (angleCheckP1 > -1.6)
                        {
                            while (again < 2)
                            {
                                velocity += acceleration * Raylib.GetFrameTime();
                                bulletP1.y += velocity * (angleCheckP1 * -1) * Raylib.GetFrameTime();
                                bulletP1.x += 50 * (angleCheckP1 * -1) * Raylib.GetFrameTime() / (angleCheckP1 * -1);
                                again += 1;
                            }
                            velocity += acceleration * Raylib.GetFrameTime();
                            bulletP1.y += velocity * Raylib.GetFrameTime();
                            bulletP1.x += 50 * (angleCheckP1 * -1) * Raylib.GetFrameTime() / (angleCheckP1 * -1);
                        }
                        else if (angleCheckP1 < -1.6)
                        {
                            while (again < 2)
                            {
                                velocity += acceleration * Raylib.GetFrameTime();
                                bulletP1.y += velocity * Raylib.GetFrameTime();
                                bulletP1.x += 50 * (angleCheckP1 * -1) * Raylib.GetFrameTime() / (angleCheckP1 * -1);
                                again += 1;
                            }
                            velocity += acceleration * Raylib.GetFrameTime();
                            bulletP1.y += velocity * Raylib.GetFrameTime();
                            bulletP1.x += 50 * angleCheckP1 * Raylib.GetFrameTime() / (angleCheckP1 * -1);
                        }
                    }


                    // poof = true when the bullet collides with the ground or a player
                    foreach (Rectangle rec in ground)
                    {
                        Rectangle tank2 = Tank(tank2X, tank2Y);
                        if (Raylib.CheckCollisionCircleRec(bulletP1.xy, 4, rec) || Raylib.CheckCollisionCircleRec(bulletP1.xy, 4, tank2))
                        {
                            explosionP1 = true;
                            poof = true;
                            if (bulletTypeP1 == 1)
                            {
                                if (Raylib.CheckCollisionCircleRec(bulletP1.xy, 20, tank2))
                                {
                                    hit = true;
                                }
                            }
                            if (bulletTypeP1 == 2)
                            {
                                if (Raylib.CheckCollisionCircleRec(bulletP1.xy, 40, tank2))
                                {
                                    hit = true;
                                }
                            }
                        }

                        if (bulletP1.x < 0 || bulletP1.x > 1000)
                        {
                            poof = true;
                            bulletP1.active = false;
                        }
                    }

                    if (poof == true)
                    {
                        if (hit == true)
                        {
                            pointsP1 += 1;
                        }
                        Thread.Sleep(1000);
                        shot = false;
                        turn = 1;
                        moved = false;
                        poof = false;
                        bulletP1.active = false;
                        velocity = -240;
                    }
                }

                if (bulletP2.active)
                {
                    acceleration2 = gravity2;
                    bulletP2.xy = new Vector2(bulletP2.x, bulletP2.y);
                    angleCheckP2 = MathF.Atan2(directionP2.Y, directionP2.X);
                    Debug.WriteLine(angleCheckP2);

                    if (bulletTypeP2 == 1)
                    {
                        if (angleCheckP2 > -1.6)
                        {
                            while (again < 2)
                            {
                                velocity2 += acceleration2 * Raylib.GetFrameTime();
                                bulletP2.y += velocity2 * (angleCheckP2 * -1) * Raylib.GetFrameTime();
                                bulletP2.x += 100 * (angleCheckP2 * -1) * Raylib.GetFrameTime() / (angleCheckP2 * -1);
                                again += 1;
                            }
                            velocity2 += acceleration2 * Raylib.GetFrameTime();
                            bulletP2.y += velocity2 * Raylib.GetFrameTime();
                            bulletP2.x += 100 * (angleCheckP2 * -1) * Raylib.GetFrameTime() / (angleCheckP2 * -1);
                        }
                        else if (angleCheckP2 < -1.6)
                        {
                            while (again < 2)
                            {
                                velocity2 += acceleration2 * Raylib.GetFrameTime();
                                bulletP2.y += velocity2 * Raylib.GetFrameTime();
                                bulletP2.x += 100 * angleCheckP2 * Raylib.GetFrameTime() / (angleCheckP2 * -1);
                                again += 1;
                            }
                            velocity2 += acceleration2 * Raylib.GetFrameTime();
                            bulletP2.y += velocity2 * Raylib.GetFrameTime();
                            bulletP2.x += 100 * angleCheckP2 * Raylib.GetFrameTime() / (angleCheckP2 * -1);
                        }
                    }
                    else if (bulletTypeP2 == 2)
                    {
                        if (angleCheckP2 > -1.6)
                        {
                            while (again < 2)
                            {
                                velocity2 += acceleration2 * Raylib.GetFrameTime();
                                bulletP2.y += velocity2 * (angleCheckP2 * -1) * Raylib.GetFrameTime();
                                bulletP2.x += 50 * angleCheckP2 * Raylib.GetFrameTime() / (angleCheckP2 * -1);
                                again += 1;
                            }
                            velocity2 += acceleration2 * Raylib.GetFrameTime();
                            bulletP2.y += velocity2 * Raylib.GetFrameTime();
                            bulletP2.x += 50 * (angleCheckP2 * -1) * Raylib.GetFrameTime() / (angleCheckP2 * -1);
                        }
                        else if (angleCheckP2 < -1.6)
                        {
                            while (again < 2)
                            {
                                velocity2 += acceleration2 * Raylib.GetFrameTime();
                                bulletP2.y += velocity2 * Raylib.GetFrameTime();
                                bulletP2.x += 50 * angleCheckP2 * Raylib.GetFrameTime() / (angleCheckP2 * -1);
                                again += 1;
                            }
                            velocity2 += acceleration2 * Raylib.GetFrameTime();
                            bulletP2.y += velocity2 * Raylib.GetFrameTime();
                            bulletP2.x += 50 * angleCheckP2 * Raylib.GetFrameTime() / (angleCheckP2 * -1);
                        }
                    }


                    // poof = true when the bullet collides with the ground or a player
                    foreach (Rectangle rec in ground)
                    {
                        Rectangle tank1 = Tank(tank1X, tank1Y);
                        if (Raylib.CheckCollisionCircleRec(bulletP2.xy, 4, rec) || Raylib.CheckCollisionCircleRec(bulletP1.xy, 4, tank1))
                        {
                            explosionP2 = true;
                            poof = true;
                            if (bulletTypeP2 == 1)
                            {
                                if (Raylib.CheckCollisionCircleRec(bulletP2.xy, 20, tank1))
                                {
                                    hit = true;
                                }
                            }
                            if (bulletTypeP2 == 2)
                            {
                                if (Raylib.CheckCollisionCircleRec(bulletP2.xy, 40, tank1))
                                {
                                    hit = true;
                                }
                            }

                        }

                        if (bulletP2.x < 0 || bulletP2.x > 1000)
                        {
                            poof = true;
                            bulletP2.active = false;
                        }
                    }

                    if (poof == true)
                    {
                        if (hit == true)
                        {
                            pointsP2 += 1;
                        }
                        Thread.Sleep(1000);
                        shot = false;
                        turn = 0;
                        moved = false;
                        poof = false;
                        bulletP2.active = false;
                        velocity2 = -240;
                    }
                }

                if (pointsP1 == 3 || pointsP2 == 3)
                {
                    gameOver = true;
                    if (Raylib.IsKeyDown(KeyboardKey.R))
                    {
                        bulletTypeP1 = 1;
                        bulletTypeP2 = 1;

                        tank1X = 55;
                        tank1Y = 80;

                        tank2X = 905;
                        tank2Y = 80;


                        int pos = (int)tank1X / 50;
                        tank1Y = 600 - ground[pos].Height - 40;

                        pos = (int)tank2X / 50;
                        tank2Y = 600 - ground[pos].Height - 40;

                        pointsP1 = 0;
                        pointsP2 = 0;

                        gameOver = false;
                    }

                }

            }

            public void DrawGame()
            {
                Raylib.BeginDrawing();
                if (!gameOver)
                {
                    Raylib.ClearBackground(Color.Black);
                    int font = 20;

                    string p1Points = "Player 1: " + pointsP1.ToString();
                    Vector2 textSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), p1Points, font, 2.0f);

                    string p2Points = "Player 2: " + pointsP2.ToString();
                    Vector2 textSize2 = Raylib.MeasureTextEx(Raylib.GetFontDefault(), p2Points, font, 2.0f);

                    // score
                    Raylib.DrawText(p1Points, 50, 50 + (int)textSize.Y, font, Color.Blue);
                    Raylib.DrawText(p2Points, 950 - (int)textSize2.X, 50 + (int)textSize2.Y, font, Color.Red);


                    // ground
                    foreach (Rectangle tiles in ground)
                    {
                        Raylib.DrawRectangleRec(tiles, Color.Gray);
                    }

                    // p1
                    Rectangle tank1 = Tank(tank1X, tank1Y);
                    Raylib.DrawRectangleRec(tank1, Color.Blue);

                    Vector2 start = new Vector2(tank1X + 20, tank1Y);
                    Raylib.DrawLineV(start, start + directionP1 * 60, Color.Blue);

                    if (bulletP1.active)
                    {
                        Raylib.DrawCircle((int)bulletP1.x, (int)bulletP1.y, 4, Color.Yellow);
                    }

                    if (explosionP1 == true)
                    {
                        if (bulletTypeP1 == 1)
                        {
                            Raylib.DrawCircle((int)bulletP1.x, (int)bulletP1.y, 20, Color.Orange);
                        }

                        if (bulletTypeP1 == 2)
                        {
                            Raylib.DrawCircle((int)bulletP1.x, (int)bulletP1.y, 40, Color.Orange);
                        }
                    }

                    // p2
                    Rectangle tank2 = Tank(tank2X, tank2Y);
                    Raylib.DrawRectangleRec(tank2, Color.Red);

                    start = new Vector2(tank2X + 20, tank2Y);
                    Raylib.DrawLineV(start, start + directionP2 * 60, Color.Red);

                    if (bulletP2.active)
                    {
                        Raylib.DrawCircle((int)bulletP2.x, (int)bulletP2.y, 4, Color.Yellow);
                    }

                    if (explosionP2 == true)
                    {
                        if (bulletTypeP2 == 1)
                        {
                            Raylib.DrawCircle((int)bulletP2.x, (int)bulletP2.y, 20, Color.Orange);
                        }

                        if (bulletTypeP2 == 2)
                        {
                            Raylib.DrawCircle((int)bulletP2.x, (int)bulletP2.y, 40, Color.Orange);
                        }
                    }
                }
                else
                {
                    int font = 80;
                    Vector2 textSize;

                    Raylib.ClearBackground(Color.DarkGray);
                    if (pointsP1 == 3)
                    {
                        font = 80;
                        textSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), "Player 1 Winds!", font, 2.0f);
                        Raylib.DrawText("Player 1 Wins!", 500 - (int)textSize.X / 2, 300 - (int)textSize.Y, font, Color.Blue);
                    }
                    if (pointsP2 == 3)
                    {
                        font = 80;
                        textSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), "Player 2 Winds!", font, 2.0f);
                        Raylib.DrawText("Player 2 Wins!", 500 - (int)textSize.X / 2, 300 - (int)textSize.Y, font, Color.Red);
                    }
                    font = 60;
                    textSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), "Press R to restart", font, 2.0f);
                    Raylib.DrawText("Press R to restart", 500 - (int)textSize.X / 2, 500 - (int)textSize.Y, font, Color.Magenta);
                }

                Raylib.EndDrawing();
            }

            public Rectangle Tank(float x, float y)
            {
                Rectangle tank = new Rectangle(x, y, 40, 40);

                return tank;
            }

            public void Ground()
            {
                Random random = new Random();
                int h = random.Next(10, 80);
                int x = 0;
                int amount = 0;

                while (amount < 20)
                {
                    Rectangle idk = new Rectangle(x, 600 - h, 50, h);
                    h = random.Next(10, 80);
                    amount += 1;
                    x += 50;
                    ground.Add(idk);
                }
            }

            public void _Bullet(bool tank)
            {
                // p1
                if (!tank)
                {
                    if (!bulletP1.active)
                    {
                        Vector2 start = new Vector2(tank1X + 20, tank1Y);
                        bulletP1.xy = start + directionP1 * 60;
                        bulletP1.x = bulletP1.xy.X;
                        bulletP1.y = bulletP1.xy.Y;
                        bulletP1.active = true;
                    }

                }

                // p2
                if (tank)
                {
                    if (!bulletP2.active)
                    {
                        Vector2 start = new Vector2(tank2X + 20, tank2Y);
                        bulletP2.xy = start + directionP2 * 60;
                        bulletP2.x = bulletP2.xy.X;
                        bulletP2.y = bulletP2.xy.Y;
                        bulletP2.active = true;
                    }
                }

            }
        }

        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }
}
