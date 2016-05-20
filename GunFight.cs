using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GunFight
{
    class GunFight
    {
        class Point
        {
            public int row;
            public int col;
        }
        #region Constants
        const int WindowWidth = 60;
        const int WindowHeight = 20;
        const int ScreenUpperBorder = 2;
        const int ScreenLowerBorder = WindowHeight - 2;
        const int EnemyStartOffset = WindowWidth;
        static bool IsGameOver = false;
        const int CollisionAOE = 1;
        const int CollisionAOE1 = 3;
        static Random randGenerator = new Random();
        static int Score = 0;
        static int counter = 5;
        static int fireballs = 5;

        /* Player Info */
        static int playerRow = 3;
        static int playerCol = 0;
        static string playerFigure = "[0]";
        static ConsoleColor playerColor = ConsoleColor.Green;

        /* Enemy Info */
        static List<Point> enemies = new List<Point>();
        static string enemyFigure = "#";
        static ConsoleColor enemyColor = ConsoleColor.Red;
        static int EnemySpawnChance = 10;
        

        /* Bullet Info */
        static List<Point> bullets = new List<Point>();
        static string bulletFigure = "*";
        static ConsoleColor bulletColor = ConsoleColor.Yellow;

        /*FireBall Info*/
        static List<Point> fireball = new List<Point>();
        static string fireBallFigure = "@";
        static ConsoleColor fireBallColor = ConsoleColor.DarkYellow;
        const int currentFireBalls = 5;
        #endregion


        static void Main(string[] args)
        
       {
            InitialiseSettings();
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.Clear();

            /*IsGameOver = true;
            
            if (IsGameOver == true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.SetCursorPosition(20, 7);
                Console.WriteLine("New game");
                Console.SetCursorPosition(20, 8);
                Console.WriteLine("High Score");
                Console.SetCursorPosition(20,9);
                Console.WriteLine("Exit");
                
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo usersInput = Console.ReadKey();
                    
                    while (Console.KeyAvailable)
                    {
                        Console.ReadKey(true);
                    }
                    if (usersInput.Key == ConsoleKey.N)
                    {
                        IsGameOver = false;
                    }
                    else if (usersInput.Key == ConsoleKey.H)
                    {
                        Console.WriteLine("High score:"+Score);
                    }
                    else if (usersInput.Key == ConsoleKey.E)
                    {
                        IsGameOver = true;
                    }
                }
            }*/
            while (!IsGameOver)
            {
                Clear();
                CheckCollision();
                Update();
                Draw();
                Fireballs();
                Thread.Sleep(100);
            }
        }
   

        static void DrawStats()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(0,0);
            Console.Write("Score: " + Score);
        }

        static void UpdateStats()
        {
            Score++;
        }
      

        static void DrawLives()
        {
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(51,0);
            Console.WriteLine("Lives: " + counter);
            
        }

        static void UpdateLives()
        {
            counter--;
        }
        static void Fireballs()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(21, 0);
            Console.WriteLine("FireBalls: " + fireballs);
            
        }
       

        #region Utility Methods
        static void PrintOnPosition(int row, int col, string text, ConsoleColor color)
        {
            Console.SetCursorPosition(col, row);
            Console.ForegroundColor = color;
            Console.Write(text);
        }

        static void InitialiseSettings()
        {
            Console.WindowWidth = WindowWidth;
            Console.WindowHeight = WindowHeight;
            Console.BufferWidth = WindowWidth;
            Console.BufferHeight = WindowHeight;
            Console.CursorVisible = false;
        }

        static bool IsObjectInBounds(int row, int col)
        {
            return (row >= ScreenUpperBorder &&
                    row <= WindowHeight - 1 &&
                    col >= 0 &&
                    col <= WindowWidth - 1);
        }

        static bool DoObjectsCollide(int firstRow, int firstCol, int secondCol, int secondRow)
        {
            int callOffset = Math.Abs(firstCol - secondCol);

            return firstRow == secondRow &&
                callOffset <= CollisionAOE;
        }

        static bool DoObjectsCollide1(int firstRow, int firstCol, int secondCol, int secondRow)
        {
            int callOffset1 = Math.Abs(firstCol - secondCol);

            return firstRow == secondRow &&
                callOffset1 <= CollisionAOE1;
        }
        #endregion

        #region Player Methods
        static void ClearPlayer()
        {
            PrintOnPosition(playerRow, playerCol, "   ", playerColor);
        }

        static void DrawPlayer()
        {
            PrintOnPosition(playerRow, playerCol, playerFigure, playerColor);
        }

        static void UpdatePlayer()
        {
            ReadInput();
        }
        static void ReadInput()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo userInput = Console.ReadKey();
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }

                if (userInput.Key == ConsoleKey.LeftArrow && playerCol > 0)
                {
                    playerCol--;
                }
                else if (userInput.Key == ConsoleKey.RightArrow && playerCol < WindowWidth -3)
                {
                    playerCol++;
                }
                else if (userInput.Key == ConsoleKey.DownArrow && playerRow < ScreenLowerBorder)
                {
                    playerRow++;
                }
                else if (userInput.Key == ConsoleKey.UpArrow && playerRow > ScreenUpperBorder)
                {
                    playerRow--;
                }
                else if (userInput.Key == ConsoleKey.Spacebar)
                {
                    bullets.Add(new Point()
                    {
                        row = playerRow,
                        col = playerCol + 2
                    });
                }
                else if (fireballs == 0)
                {
                    return;
                }
                else if (userInput.Key == ConsoleKey.Tab)
                {
                    fireballs--;
                    fireball.Add(new Point()
                    {
                        row = playerRow,
                        col = playerCol
                    });
                }                
            }
        }
        #endregion

        #region Enemy Methods

        static void ClearEnemies()
        {
            for (int cnt = 0; cnt < enemies.Count; cnt++)
            {
                if (IsObjectInBounds(enemies[cnt].row, enemies[cnt].col))
                {
                    PrintOnPosition(enemies[cnt].row, enemies[cnt].col, " ", enemyColor);
                }

                
            }
        }
        static void DrawEnemies()
        {
            for (int cnt = 0; cnt < enemies.Count; cnt++)
            {
                if (IsObjectInBounds(enemies[cnt].row, enemies[cnt].col))
                {
                    PrintOnPosition(enemies[cnt].row, enemies[cnt].col, enemyFigure, enemyColor);
                }
                
            }
        }
        
        static void UpdateEnemies()
        {
            SpawnEnemy();
            for (int cnt = 0; cnt < enemies.Count; cnt++)
            {
                enemies[cnt].col--;
                if (enemies[cnt].col < 0)
                {
                    enemies.RemoveAt(cnt);
                    cnt--;
                }
            }
        }
        static void SpawnEnemy()
        {
            int chance = randGenerator.Next(100);
            if (chance < EnemySpawnChance)
            {
                int row = randGenerator.Next(ScreenUpperBorder, ScreenLowerBorder);
                int col = randGenerator.Next(0, WindowWidth) + EnemyStartOffset + 20;

                enemies.Add(new Point()
                    {
                        row = row,
                        col = col
                    });

            }
        }
        #endregion

        #region Bullets Methods
        static void ClearBullets()
        {
            for (int cnt = 0; cnt < bullets.Count; cnt++)
            {
                if (IsObjectInBounds(bullets[cnt].row, bullets[cnt].col))
                {
                    PrintOnPosition(bullets[cnt].row, bullets[cnt].col, " ", bulletColor);
                }
                
            }
        }

        static void DrawBullets()
        {
            for (int cnt = 0; cnt < bullets.Count; cnt++)
            {
                if (IsObjectInBounds(bullets[cnt].row, bullets[cnt].col))
                {
                    PrintOnPosition(bullets[cnt].row, bullets[cnt].col, bulletFigure, bulletColor);
                }
            }
        }

        static void UpdateBullets()
        {
            for (int cnt = 0; cnt < bullets.Count; cnt++)
            {
                bullets[cnt].col++;
                if (bullets[cnt].col > WindowWidth - 1)
                {
                    bullets.RemoveAt(cnt);
                    cnt--;
                }
            }           
        }

        static void ClearFireBall()
        {
            for (int cnt1 = 0; cnt1 < fireball.Count; cnt1++)
            {
                if (IsObjectInBounds(fireball[cnt1].row, fireball[cnt1].col))
                {
                    PrintOnPosition(fireball[cnt1].row, fireball[cnt1].col, " ", fireBallColor);
                }
            }
        }
        static void DrawFireBall()
        {
            for (int cnt1 = 0; cnt1 < fireball.Count; cnt1++)
            {
                if (IsObjectInBounds(fireball[cnt1].row, fireball[cnt1].col))
                {
                    PrintOnPosition(fireball[cnt1].row, fireball[cnt1].col, fireBallFigure, fireBallColor);
                }
            }
        }
        static void UpdateFireBall()
        {
            for (int cnt1 = 0; cnt1 < fireball.Count; cnt1++)
            {
                fireball[cnt1].col++;
                if (fireball[cnt1].col > WindowWidth - 1)
                {
                    fireball.RemoveAt(cnt1);
                    cnt1--;
                }
            }
            
            if (Score == 10)
            {
                fireballs += 1;
                return;
            }
           
        }
        #endregion

        #region Collisions Methods

        static void CheckEnemyBulletsCollision()
        {
            for (int bulletIndex = 0; bulletIndex < bullets.Count; bulletIndex++)
            {
                for (int enemyIndex = 0; enemyIndex < enemies.Count; enemyIndex++)
                {
                    
                    if (DoObjectsCollide(
                        bullets[bulletIndex].row, 
                        bullets[bulletIndex].col, 
                        enemies[enemyIndex].col, 
                        enemies[enemyIndex].row))
                    {
                        
                        bullets.RemoveAt(bulletIndex);
                        enemies.RemoveAt(enemyIndex);
                        bulletIndex--;
                        enemyIndex--;
                        Score ++;
                       
                        break;
                    }
                }
                
            }
        }

        static void CheckEnemyFireballCollision()
        {
            for (int fireBallIndex = 0; fireBallIndex < fireball.Count; fireBallIndex++)
            {
                bool hasFireballCollided = false;
                for (int enemyIndex = 0; enemyIndex < enemies.Count; enemyIndex++)
                {

                    if (DoObjectsCollide1(
                        fireball[fireBallIndex].row,
                        fireball[fireBallIndex].col,
                        enemies[enemyIndex].col,
                        enemies[enemyIndex].row))
                    {
                        hasFireballCollided = true;
                        enemies.RemoveAt(enemyIndex);

                        enemyIndex--;
                        Score++;
                    }
                }

                if (hasFireballCollided)
                {
                    fireball.RemoveAt(fireBallIndex);
                }

            }
        }

        static void CheckEnemyPlayerCollision()
        {
            for (int enemyIndex = 0; enemyIndex < enemies.Count; enemyIndex++)
            {

                if (DoObjectsCollide1(
                    enemies[enemyIndex].row,
                    enemies[enemyIndex].col,
                    playerCol,
                    playerRow ))
                {
                    counter--;
                        
                    
                 
                    
                }
                else if (counter == 0)
                {
                    IsGameOver = true;
                }
            }
        }

        static void CheckCollision()
        {
            CheckEnemyBulletsCollision();
            CheckEnemyPlayerCollision();
            CheckEnemyFireballCollision();
        }
        #endregion

        #region Main Methods
        static void Clear()
        {
            ClearPlayer();
            ClearEnemies();
            ClearBullets();
            ClearFireBall();
        }

        static void Draw()
        {
            DrawPlayer();
            DrawEnemies();
            DrawBullets();
            DrawStats();
            DrawLives();
            DrawFireBall();
        }
        static void Update()
        {
            UpdatePlayer();
            UpdateEnemies();
            UpdateBullets();
            UpdateFireBall();
        }
        #endregion
    }
}
