using System;
using System.Collections.Generic;
using System.Text;

namespace _437Project3
{
    static class Predict
    {

        //Have: isValidMove, getValidMoves, getMoveGains
        public static int [] predictAI (int player, int [,] grid, int playerGains, /*int playerGains,*/ int remainingDepth)//return int [i, j, AIgains, playerGains] AIturn, grid, 0, 100, difficulty
        {
            if (remainingDepth == 0) return null;

            List<int[]> moves = Program.getValidMoves(player, grid);

            int max = playerGains;

            int[] bestMove = null;

            for (int index = 0; index < moves.Count; index++)
            {
                int gains = Program.getMoveGains(player, grid, moves [index] [0], moves [index] [1]);

                {
                    int tempPlayer;
                    if (player == 1) tempPlayer = 2;
                    else tempPlayer = 1;

                    int[,] tempGrid = new int [8, 8];
                    for (int i = 0; i < 8; i++)
                        for (int j = 0; j < 8; j++)
                            tempGrid [i, j] = grid [i, j];

                    tempGrid = Program.addNewMove (player, tempGrid, moves [index] [0], moves [index] [1]);

                    int[] result = predictPlayer (tempPlayer, tempGrid, max, remainingDepth - 1);

                    if (result == null) continue;
                    else if (gains - result [2] >= max)
                    {
                        max = gains - result[2];//AI gains

                        bestMove = new int[3];
                        bestMove [0] = moves [index] [0];
                        bestMove [1] = moves [index] [1];

                        bestMove [2] = max;
                    }
                }
            }

            if (bestMove == null && moves != null)
            {
                bestMove = new int [3];
                bestMove [0] = moves [0] [0];
                bestMove [1] = moves [0] [1];
                bestMove [2] = Program.getMoveGains (player, grid, moves [0] [0], moves [0] [1]);
            }

            return bestMove;
        }

        public static int [] predictPlayer (int player, int[,] grid, int AIgains, int remainingDepth)//return int [i, j, AIgains, playerGains]
        {
            if (remainingDepth == 0) return null;

            List<int[]> moves = Program.getValidMoves(player, grid);

            int max = 0;

            int[] bestMove = null;

            for (int index = 0; index < moves.Count; index++)
            {
                int gains = Program.getMoveGains(player, grid, moves[index][0], moves[index][1]);

                if (gains >= max)
                {
                    max = gains;

                    int tempPlayer;
                    if (player == 1) tempPlayer = 2;
                    else tempPlayer = 1;

                    int [,] tempGrid = new int [8, 8];
                    for (int i = 0; i < 8; i++)
                        for (int j = 0; j < 8; j++)
                            tempGrid [i, j] = grid [i, j];

                    tempGrid = Program.addNewMove (player, tempGrid, moves [index] [0], moves [index] [1]);

                    int[] result = predictPlayer (tempPlayer, tempGrid, max, remainingDepth - 1);
                    if (result == null) continue;
                    else if (/*result[3] >= max &&*/  gains - result [2] >= max)
                    {
                        max = gains - result [2];//AI gains

                        bestMove = new int [3];
                        bestMove [0] = moves [index] [0];
                        bestMove [1] = moves [index] [1];

                        bestMove [2] = max;
                    }
                }
            }

            if (bestMove == null && moves.Count != 0)
            {
                bestMove = new int [3];
                bestMove [0] = moves [0] [0];
                bestMove [1] = moves [0] [1];
                bestMove [2] = Program.getMoveGains (player, grid, moves [0] [0], moves [0] [1]);
            }

            return bestMove;
        }
    }
}
