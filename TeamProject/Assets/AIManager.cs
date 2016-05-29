using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class Move
{
    TilesManager TM;

    public Tile tile;
    public int x;
    public int y;
    //public List<int> meeplePosition;
    //public Area meeplePosition;
    public float score = 0;
    public int rotation;

    public void Clone(Move other)
    {
        this.tile.Clone(other.tile);
        this.x = other.x;
        this.y = other.y;
        this.score = other.score;
        this.rotation = other.rotation;
    }
};

public class AIManager : MonoBehaviour {

    TilesManager TM;
    PointsCounter PC;
    GameManager GM;

    bool terminal()
    {
        return true;
    }

    public Move Expectimax(ref GameObject[,] tiles, List<Tile> tileList, int maxDepth, List<Player> players)
    {
        
        Tile[,] simulation = new Tile[200, 200];
        for (int i = 0; i < 200; i++)
        {
            for (int j = 0; j < 200; j++)
            {
                simulation[i, j] = new Tile();
            }
        }

        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1);y++)
            {
                if (tiles[x,y] != null)
                {
                    if (tiles[x, y].GetComponent<Tile>() != null)
                    {
                        simulation[x, y].Clone(tiles[x, y].GetComponent<Tile>());
                    }  
                }
            }
        }

        List<Move> possibleMoves = GetPossibleMoves(simulation, tileList, players[0]); // 0 to AI
        Move bestMove = new Move();
        Move iterator = new Move();

        for (int u = 0; u < possibleMoves.Count; u++)
        {
            // kopia
            Tile[,] tempBoard = new Tile[200, 200];
            for (int x = 0; x < simulation.GetLength(0); x++)
            {
                for (int y = 0; y < simulation.GetLength(1); y++)
                {
                    if (simulation[x, y] != null)
                    {
                        tempBoard[x, y].Clone(simulation[x, y]);
                    }
                }
            }

            possibleMoves[u] = DoMove(possibleMoves[u], ref tempBoard, ref tileList, ref players, 1);
            possibleMoves[u].score += MinMax(tempBoard, tileList, 0, maxDepth, players, 1); //1 to chance
        }
        foreach (var move in possibleMoves)
        {
            if (move.score > bestMove.score)
            {
                bestMove = move;
            }
        }
        return bestMove;
    }

    //public float Max(Tile[,] tiles, List<Tile> tileList, int meeplesLeft, int currentDepth, int maxDepth)
    //{
    //    if (maxDepth == currentDepth)
    //    {
    //        return CountCurrentPoints(int zdj, board);
    //    }
    //    List<Move> possibleMoves = GetPossibleMoves(tiles, tileList, meeplesLeft);
    //    float result = 0;
    //    foreach (var move in possibleMoves)
    //    {
    //        float p = Probability(tileList, move.tile);
    //        // kopia
    //        result += p * selectMax(result, Chance(DoMove(move),currentDepth +1,...))
    //    }
    //    return result;
    //}

    //public float Chance(Tile[,] tiles, List<Tile> tileList, int meeplesLeft, int currentDepth, int maxDepth)
    //{
    //    if (maxDepth == currentDepth)
    //    {
    //        return CountCurrentPoints(int zdjetepunkty, board);
    //    }
    //    List<Move> possibleMoves = GetPossibleMoves(tiles, tileList, meeplesLeft);
    //    float result = 8192;
    //    foreach (var move in possibleMoves)
    //    {
    //        float p = Probability(tileList, move.tile);

    //        // kopia planszy
    //        result += p * selectMin(result, max(DoMove(move),...))
    //    }
    //    return result;
    //}

    public float MinMax(Tile[,] tiles, List<Tile> tileList, int currentDepth, int maxDepth, List<Player> players, int currentPlayer)
    {
        if (maxDepth == currentDepth)
        {
            return CountCurrentPoints(ref tiles, ref players, currentPlayer);
        }
        List<Move> possibleMoves;

        float result = 0;
        if (currentPlayer == 1)
        {
            result = 8192f;
            possibleMoves = GetPossibleMoves(tiles, tileList, players[1]);
        }
        else
        {
            possibleMoves = GetPossibleMoves(tiles, tileList, players[0]);
        }
        foreach (var move in possibleMoves)
        {
            float p = Probability(tileList, move.tile);
            Tile[,] tempBoard = new Tile[200, 200];
            // kopia planszy
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    if (tiles[x, y] != null)
                    {
                        tempBoard[x, y].Clone(tiles[x, y]);
                    }
                }
            }
            
            if (currentPlayer == 1)
            {
                Move tmp = DoMove(move, ref tiles, ref tileList, ref players, 0);
                result = p * Math.Min(result, tmp.score + MinMax(tempBoard, tileList, currentDepth++, maxDepth, players, 0));
            }
            else
            {
                Move tmp = DoMove(move, ref tiles, ref tileList, ref players, 1);
                result = p * Math.Max(result, tmp.score + MinMax(tempBoard, tileList, currentDepth++, maxDepth, players, 1));
            }
        }
        return result;
    }

    public Move DoMove(Move move, ref Tile[,] tiles, ref List<Tile> tileList, ref List <Player> players, int currentPlayer)
    {
        tiles[move.x, move.y].Clone(move.tile);
        tileList.Remove(move.tile); // moze nie dziala nie weim

        int tmp = players[currentPlayer].points;
        PC.countPointsAfterMove(ref tiles, ref players, move.x, move.y, false);

        //Wykonaj obecny ruch
        //Policz punkty za obecny ruch i zmień odpowiednio planszę
        move.score = players[currentPlayer].points - tmp;
        return move;
    }

    public float CountCurrentPoints(ref Tile[,] tiles, ref List<Player> players, int currentPlayer)
    {
        float result = 0f;
        //policz punkty za niedokończone rzeczy
        int tmp = players[currentPlayer].points;

        for (int x = 0; x < tiles.GetLength(0); x += 1)
        {
            for (int y = 0; y < tiles.GetLength(1); y += 1)
            {
                if (tiles[x, y] != null)
                {
                    if (tiles[x, y].Areas.Find(a => a.player != null).player != null)
                    {
                        PC.countPointsAfterMove(ref tiles, ref players, x, y, true);
                    }
                }
            }
        }

        result = players[currentPlayer].points - tmp;
        return result;
    }

    public List<Move> GetPossibleMoves(Tile[,] tiles, List<Tile> tileList, Player player)
    {
        List<Move> MovesList = null;
        List<int[]> possiblePositions;
        List<int> possibleRotations;
        List<Area> possibleMeeple = new List<Area>();

        Move tmp = new Move();
        Tile tmptile = new Tile();

        int help = -1;

        for (int k = 0; k < tileList.Count; k++)
        {
            if (tmptile.IdNumber != help || tmptile == null)
            {
                help = tmptile.IdNumber;
                tmp.tile = tmptile;
                possiblePositions = TM.findMatchingEdges(TM.findSourrounding(ref tiles), tmp.tile, ref tiles);
                if (possiblePositions != null)
                {
                    foreach (int[] pos in possiblePositions)
                    {
                        tmp.x = pos[0];
                        tmp.y = pos[1];

                        Move move = new Move();
                        move.Clone(tmp);
                        MovesList.Add(move); // dodaje bez meepla

                        possibleRotations = TM.possibleRotations(tmptile, tiles, pos);
                        foreach (int rotation in possibleRotations)
                        {
                            if (rotation == 1)
                            {
                                TM.rotateClockwise90(ref tmptile);
                            }
                            else if(rotation == 2)
                            {
                                TM.rotateClockwise90(ref tmptile);
                                TM.rotateClockwise90(ref tmptile);
                            }
                            else if (rotation == 3)
                            {
                                TM.rotateClockwise90(ref tmptile);
                                TM.rotateClockwise90(ref tmptile);
                                TM.rotateClockwise90(ref tmptile);
                            }

                            tiles[pos[0], pos[1]] = tmptile;
                            possibleMeeple = TM.possibleMeepleAreas(ref tiles, pos[0], pos[1]);
                            tiles[pos[0], pos[1]] = null;

                            if (player.meeples > 0)
                            {


                                foreach (Area area in tmp.tile.Areas )
                                {
                                    if (possibleMeeple.Any(a => a.meeplePlacementIndex == area.meeplePlacementIndex))
                                    {
                                        area.player = player;
                                        move = new Move();
                                        move.Clone(tmp);
                                        MovesList.Add(move);

                                        area.player = null;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        return MovesList;
    }

    public float Probability(List<Tile> tilesList, Tile tile)
    {
        float result = 0.0f;
        bool check = false;
        float count = 0;
        int listSize = tilesList.Count;

        for (int i = 0; i < listSize; i++)
        {
            if (tilesList[i].IdNumber == tile.IdNumber)
            {
                count++;
                check = true;
            }
            else if (check)
            {
                break;
            }
        }
        if (count == 0)
        {
            return 0;
        }
        result = count / listSize;
        return result;
    }
    
	// Use this for initialization
}
