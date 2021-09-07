using System;
using System.Collections.Generic;

namespace _437Project3
{
    class Program
    {
        int diff = 10;

        //0 = blank
        //1 = blue (AI)
        //2 = red (player)

        static void Main (string [] args)
        {
            int[,] grid = new int[8, 8];
            grid[3, 3] = 2;
            grid[3, 4] = 1;
            grid[4, 3] = 1;
            grid[4, 4] = 2;

            int AIturn, playerTurn;

            Console.WriteLine ("Please enter a diffucilty within a range from 1-10");
            String diffStr = Console.ReadLine ();
            int diff = 5;
            try
            {
                diff = Convert.ToInt32 (diffStr);
            }
            catch
            {
                Console.WriteLine ("--Invalid input-- Restarting game >:(");
                Main (args);
            }
            Console.WriteLine ("Would you like to go FIRST or SECOND? (Type the word)");
            String ansStr = Console.ReadLine ();
            if (ansStr.ToUpper ().Equals ("FIRST"))
            {
                AIturn = 2;
                playerTurn = 1;
            }
            else
            {
                AIturn = 1;
                playerTurn = 2;
            }

            bool validInput = true;
            bool inProg = true;
            while (inProg)
            {
                if (getValidMoves (AIturn, grid).Count == 0 && getValidMoves (playerTurn, grid).Count == 0)
                {
                    Console.WriteLine ("*--*Game Over*--*");

                    int AIscore = 0;
                    int playerScore = 0;
                    for (int i = 0; i < 8; i++)
                        for (int j = 0; j < 8; j++)
                            if (grid [i, j] == AIturn) AIscore++;
                            else if (grid [i, j] == playerTurn) playerScore++;

                    if (AIscore == playerScore)
                        Console.WriteLine ("-The result is a tie-");
                    else if (AIscore > playerScore)
                        Console.WriteLine ("-Opponent wins!-");
                    else
                        Console.WriteLine ("-You win!-");

                    Console.WriteLine ("-AI scoare: " + AIscore);
                    Console.WriteLine ("-Player score: " + playerScore);
                    break;
                }

                drawBoard (grid);

                if (AIturn == 1 && getValidMoves (AIturn, grid).Count != 0 && validInput == true)
                {
                    Console.WriteLine ("***[X] AI Turn [X]***\n");

                    int[] move = Predict.predictAI (AIturn, grid, 0, diff);//return int [i, j, AIgains]

                    if (move == null)
                    {
                        Console.WriteLine ("--Opponent has no moves; Turn skipped--\n");
                        continue;
                    }
                    else
                    {
                        grid = addNewMove (AIturn, grid, move [0], move [1]);
                        grid [move [0], move [1]] = AIturn;
                    }

                    drawBoard (grid);
                }
                else if (getValidMoves (AIturn, grid).Count == 0)
                {
                    Console.WriteLine ("***[X] AI Turn [X]***\n");
                    Console.WriteLine ("--Opponent has no moves; Turn skipped--\n");
                    continue;
                }

                validInput = true;
                if (playerTurn == 1) Console.WriteLine ("***[X] Player Turn [X]***");
                else if (playerTurn == 2) Console.WriteLine ("***[O] Player Turn [O]***");

                if (getValidMoves (playerTurn, grid).Count != 0)
                {
                    Console.WriteLine ("\nType your next position in terms of i and j. (E.g. '5 3' for the fifth row, third column)");
                    String input = Console.ReadLine ();
                    int i, j = 0;

                    try
                    {
                        i = Convert.ToInt32 (input.Substring (0, 1));
                        j = Convert.ToInt32 (input.Substring (2, 1));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine ("!INVALID INPUT! Try again...");
                        validInput = false;
                        continue;
                    }
                    if (i < 1 || i > 8 || j < 1 || j > 8)
                    {
                        Console.WriteLine ("!INVALID RANGE! Try again...");
                        validInput = false;
                        continue;
                    }
                    else if (!isValidMove (playerTurn, grid, i - 1, j - 1))
                    {
                        Console.WriteLine ("!INVALID SPACE! Try again...");
                        validInput = false;
                        continue;
                    }

                    grid = addNewMove (playerTurn, grid, i - 1, j - 1);
                    grid [i - 1, j - 1] = playerTurn;
                }
                else
                {
                    Console.WriteLine ("--You have no moves; Turn skipped--\n");
                }

                if (AIturn == 2 && getValidMoves (AIturn, grid).Count != 0 && getValidMoves (AIturn, grid).Count != 0)
                {
                    drawBoard (grid);

                    Console.WriteLine ("***[O] AI Turn [O]***\n");

                    int [] move = Predict.predictAI (AIturn, grid, 0, diff);//return int [i, j, AIgains, playerGains]

                    if (move == null)
                    {
                        Console.WriteLine ("--Opponent has no moves; Turn skipped--\n");
                        continue;
                    }
                    else
                    {
                        grid = addNewMove (AIturn, grid, move [0], move [1]);
                        grid [move [0], move [1]] = AIturn;
                    }
                }
                else if (getValidMoves (AIturn, grid).Count == 0)
                {
                    Console.WriteLine ("***[O] AI Turn [O]***\n");
                    Console.WriteLine ("--Opponent has no moves; Turn skipped--\n");
                    continue;
                }
            }
        }

        public static bool isValidMove(int player, int[,] grid, int i, int j)
        {
            if (i != 7)
            {
                if (grid [i + 1, j] == 0 || grid [i + 1, j] == player) ;
                else
                {
                    for (int iter = i + 2; iter < 8; iter++)
                        if (grid [iter, j] == player) return true;
                        else if (grid [iter, j] == 0) break;
                }
            }

            if (i != 0)
            {
                if (grid [i - 1, j] == 0 || grid [i - 1, j] == player) ;
                else
                {
                    for (int iter = i - 2; iter >= 0; iter--)
                        if (grid [iter, j] == player) return true;
                        else if (grid [iter, j] == 0) break;
                }
            }

            if (j != 7)
            {
                if (grid [i, j + 1] == 0 || grid [i, j + 1] == player) ;
                else
                {
                    for (int iter = j + 2; iter < 8; iter++)
                        if (grid [i, iter] == player) return true;
                        else if (grid [i, iter] == 0) break;
                }
            }

            if (j != 0)
            {
                if (grid [i, j - 1] == 0 || grid [i, j - 1] == player) ;
                else
                {
                    for (int iter = j - 2; iter >= 0; iter--)
                        if (grid [i, iter] == player) return true;
                        else if (grid [i, iter] == 0) break;
                }
            }

            if (i != 7 && j != 7)
            {
                if (grid [i + 1, j + 1] == 0 || grid [i + 1, j + 1] == player) ;
                else
                {
                    for (int iteri = i + 2, iterj = j + 2; iteri < 8 && iterj < 8; iteri++, iterj++)
                        if (grid [iteri, iterj] == player) return true;
                        else if (grid [iteri, iterj] == 0) break;
                }
            }

            if (i != 7 && j != 0)
            {
                if (grid[i + 1, j - 1] == 0 || grid [i + 1, j - 1] == player) ;
                else
                {
                    for (int iteri = i + 2, iterj = j - 2; iteri < 8 && iterj >= 0; iteri++, iterj--)
                        if (grid [iteri, iterj] == player) return true;
                        else if (grid [iteri, iterj] == 0) break;

                }
            }

            if (i != 0 && j != 7)
            {
                if (grid [i - 1, j + 1] == 0 || grid [i - 1, j + 1] == player) ;
                else
                {
                    for (int iteri = i - 2, iterj = j + 2; iteri >= 0 && iterj < 8; iteri--, iterj++)
                        if (grid [iteri, iterj] == player) return true;
                        else if (grid [iteri, iterj] == 0) break;
               }
            }

            if (i != 0 && j != 0)
            {
                if (grid [i - 1, j - 1] == 0 || grid [i - 1, j - 1] == player) ;
                else
                {
                    for (int iteri = i - 2, iterj = j - 2; iteri >= 0 && iterj >= 0; iteri--, iterj--)
                        if (grid [iteri, iterj] == player) return true;
                        else if (grid [iteri, iterj] == 0) break;
                }
            }

            return false;
        }
        
        public static List<int []> getValidMoves (int player, int [,] grid)
        {
            List<int[]> moves = new List <int []> ();

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (grid[i, j] == 0 && isValidMove(player, grid, i, j)) moves.Add(new int [] {i, j});

            return moves;
        }
        
        public static int getMoveGains (int player, int [,] grid, int i, int j)
        {
            //Console.WriteLine("Board for gains:");
            //drawBoard(grid);

            int gains = 0;

            int potGains;
                
            potGains = 0;
            for (int iter = i + 1; iter < 8; iter++)
            {
                if (grid[iter, j] == 0 || grid[iter, j] == player)
                {
                    if (grid[iter, j] == 0);
                    else gains += potGains;
                    break;
                }
                else potGains++;
            }

            potGains = 0;
            for (int iter = i - 1; iter >= 0; iter--)
            {
                if (grid[iter, j] == 0 || grid[iter, j] == player)
                {
                    if (grid[iter, j] == 0) ;
                    else gains += potGains;
                    break;
                }
                else potGains++;
            }

            potGains = 0;
            for (int iter = j + 1; iter < 8; iter++)
            {
                if (grid[i, iter] == 0 || grid[i, iter] == player)
                {
                    if (grid[i, iter] == 0) ;
                    else gains += potGains;
                    break;
                }
                else potGains++;
            }

            potGains = 0;
            for (int iter = j - 1; iter >= 0; iter--)
            {
                if (grid[i, iter] == 0 || grid[i, iter] == player)
                {
                    if (grid[i, iter] == 0) ;
                    else gains += potGains;
                    break;
                }
                else potGains++;
            }

            potGains = 0;
            for (int iteri = i + 1, iterj = j + 1; iteri < 8 && iterj < 8; iteri++, iterj++)
            {
                if (grid[iteri, iterj] == 0 || grid[iteri, iterj] == player)
                {
                    if (grid[iteri, iterj] == 0) ;
                    else gains += potGains;
                    break;
                }
                else potGains++;
            }

            potGains = 0;
            for (int iteri = i + 1, iterj = j - 1; iteri < 8 && iterj >= 0; iteri++, iterj--)
            {
                if (grid[iteri, iterj] == 0 || grid[iteri, iterj] == player)
                {
                    if (grid[iteri, iterj] == 0) ;
                    else gains += potGains;
                    break;
                }
                else potGains++;
            }

            potGains = 0;
            for (int iteri = i - 1, iterj = j + 1; iteri >= 0 && iterj < 8; iteri--, iterj++)
            {
                if (grid[iteri, iterj] == 0 || grid[iteri, iterj] == player)
                {
                    if (grid[iteri, iterj] == 0) ;
                    else gains += potGains;
                    break;
                }
                else potGains++;
            }

            potGains = 0;
            for (int iteri = i - 1, iterj = j - 1; iteri >= 0 && iterj >= 0; iteri--, iterj--)
            {
                if (grid[iteri, iterj] == 0 || grid[iteri, iterj] == player)
                {
                    if (grid[iteri, iterj] == 0) ;
                    else gains += potGains;
                    break;
                }
                else potGains++;
            }

            //Console.WriteLine("player: " + player + ", i: " + i + ", j: " + j);

            return gains;
        }
    
        public static int [,] addNewMove (int player, int [,] grid, int i, int j)
        {
            for (int iter1 = i + 1; iter1 < 8; iter1++)
            {
                if (grid[iter1, j] == player)
                {
                    for (int iter2 = i + 1; iter2 < 8; iter2++)
                    {
                        if (grid[iter2, j] == 0 || grid[iter2, j] == player) break;
                        else grid[iter2, j] = player;
                    }
                    break;
                }
                else if (grid[iter1, j] == 0) break;
            }

            for (int iter1 = i - 1; iter1 >= 0; iter1--)
            {
                if (grid[iter1, j] == player)
                {
                    for (int iter2 = i - 1; iter2 >= 0; iter2--)
                    {
                        if (grid[iter2, j] == 0 || grid[iter2, j] == player) break;
                        else grid[iter2, j] = player;
                    }
                    break;
                }
                else if (grid[iter1, j] == 0) break;
            }

            for (int iter1 = j + 1; iter1 < 8; iter1++)
            {
                if (grid[i, iter1] == player)
                {
                    for (int iter2 = j + 1; iter2 < 8; iter2++)
                    {
                        if (grid[i, iter2] == 0 || grid[i, iter2] == player) break;
                        else grid[i, iter2] = player;
                    }
                    break;
                }
                else if (grid[i, iter1] == 0) break;
            }

            for (int iter1 = j - 1; iter1 >= 0; iter1--)
            {
                if (grid[i, iter1] == player)
                {
                    for (int iter2 = j - 1; iter2 >= 0; iter2--)
                    {
                        if (grid[i, iter2] == 0 || grid[i, iter2] == player) break;
                        else grid[i, iter2] = player;
                    }
                    break;
                }
                else if (grid[i, iter1] == 0) break;
            }

            for (int iteri1 = i + 1, iterj1 = j + 1; iteri1 < 8 && iterj1 < 8; iteri1++, iterj1++)
            {
                if (grid[iteri1, iterj1] == player)
                {
                    for (int iteri2 = i + 1, iterj2 = j + 1; iteri2 < 8 && iterj2 < 8; iteri2++, iterj2++)
                    {
                        if (grid[iteri2, iterj2] == 0 || grid[iteri2, iterj2] == player) break;
                        else grid[iteri2, iterj2] = player;
                    }
                    break;
                }
                else if (grid[iteri1, iterj1] == 0) break;
            }

            for (int iteri1 = i + 1, iterj1 = j - 1; iteri1 < 8 && iterj1 >= 0; iteri1++, iterj1--)
            {
                if (grid[iteri1, iterj1] == player)
                {
                    for (int iteri2 = i + 1, iterj2 = j - 1; iteri2 < 8 && iterj2 >= 0; iteri2++, iterj2--)
                    {
                        if (grid[iteri2, iterj2] == 0 || grid[iteri2, iterj2] == player) break;
                        else grid[iteri2, iterj2] = player;
                    }
                    break;
                }
                else if (grid[iteri1, iterj1] == 0) break;
            }

            for (int iteri1 = i - 1, iterj1 = j + 1; iteri1 >= 0 && iterj1 < 8; iteri1--, iterj1++)
            {
                if (grid[iteri1, iterj1] == player)
                {
                    for (int iteri2 = i - 1, iterj2 = j + 1; iteri2 >= 0 && iterj2 < 8; iteri2--, iterj2++)
                    {
                        if (grid[iteri2, iterj2] == 0 || grid[iteri2, iterj2] == player) break;
                        else grid[iteri2, iterj2] = player;
                    }
                    break;
                }
                else if (grid[iteri1, iterj1] == 0) break;
            }

            for (int iteri1 = i - 1, iterj1 = j - 1; iteri1 >= 0 && iterj1 >= 0; iteri1--, iterj1--)
            {
                if (grid[iteri1, iterj1] == player)
                {
                    for (int iteri2 = i - 1, iterj2 = j - 1; iteri2 >= 0 && iterj2 >= 0; iteri2--, iterj2--)
                    {
                        if (grid[iteri2, iterj2] == 0 || grid[iteri2, iterj2] == player) break;
                        else grid[iteri2, iterj2] = player;
                    }
                    break;
                }
                else if (grid[iteri1, iterj1] == 0) break;
            }

            return grid;
        }

        public static void drawBoard(int[,] grid)
        {
            char[,] board =
            {
                {'+', '1', '+', '2','+', '3','+', '4','+', '5','+', '6','+', '7','+', '8', '+',},
                {'1', '.', '|', '.','|', '.','|', '.','|', '.','|', '.','|', '.','|', '.', '|',},
                {'+', '-', '+', '-','+', '-','+', '-','+', '-','+', '-','+', '-','+', '-', '+',},
                {'2', '.', '|', '.','|', '.','|', '.','|', '.','|', '.','|', '.','|', '.', '|',},
                {'+', '-', '+', '-','+', '-','+', '-','+', '-','+', '-','+', '-','+', '-', '+',},
                {'3', '.', '|', '.','|', '.','|', '.','|', '.','|', '.','|', '.','|', '.', '|',},
                {'+', '-', '+', '-','+', '-','+', '-','+', '-','+', '-','+', '-','+', '-', '+',},
                {'4', '.', '|', '.','|', '.','|', '.','|', '.','|', '.','|', '.','|', '.', '|',},
                {'+', '-', '+', '-','+', '-','+', '-','+', '-','+', '-','+', '-','+', '-', '+',},
                {'5', '.', '|', '.','|', '.','|', '.','|', '.','|', '.','|', '.','|', '.', '|',},
                {'+', '-', '+', '-','+', '-','+', '-','+', '-','+', '-','+', '-','+', '-', '+',},
                {'6', '.', '|', '.','|', '.','|', '.','|', '.','|', '.','|', '.','|', '.', '|',},
                {'+', '-', '+', '-','+', '-','+', '-','+', '-','+', '-','+', '-','+', '-', '+',},
                {'7', '.', '|', '.','|', '.','|', '.','|', '.','|', '.','|', '.','|', '.', '|',},
                {'+', '-', '+', '-','+', '-','+', '-','+', '-','+', '-','+', '-','+', '-', '+',},
                {'8', '.', '|', '.','|', '.','|', '.','|', '.','|', '.','|', '.','|', '.', '|',},
                {'+', '-', '+', '-','+', '-','+', '-','+', '-','+', '-','+', '-','+', '-', '+',},
            };

            for (int i = 0; i < 17; i++)
            {
                for (int j = 0; j < 17; j++)
                {
                    if (i % 2  == 1 && j % 2 == 1)
                    {
                        if (grid[(i - 1)/2, (j - 1)/2] == 1) board[i, j] = 'X';
                        else if (grid[(i - 1)/2, (j - 1)/2] == 2) board[i, j] = 'O';
                    }
                    Console.Write(board[i, j] + " ");
                }
                Console.Write("\n");
            }
        }
    }
}