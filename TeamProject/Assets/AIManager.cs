using UnityEngine;
using System.Collections.Generic;

public class Move
{
    public Tile tile;
    public int x;
    public int y;
    //public List<int> meeplePosition;
    public Area meeplePosition;
    public float score = 0;
};

public class AIManager : MonoBehaviour {

    TilesManager TM;
    GameManager GM;

    //bool terminal()
    //{
    //    return true;
    //}

    //public Move Expectimax(GameObject[,] tiles, GameObject[,] meeples, List<Tile> tileList, int maxDepth = 2)
    //{
    //    List<Move> possibleMoves = GetPossibleMoves(ref tiles, ref meeples, tileList);
    //    Move bestMove = new Move();

    //    foreach (var move in possibleMoves)
    //    {
    //        move.score = Chance(DoMove(move)...);
    //    }
    //    foreach (var move in possibleMoves)
    //    {
    //        if (move.score > bestMove.score)
    //        {
    //            bestMove = move;
    //        }
    //    }
    //    return bestMove;
    //}

    //public float Max(int currentDepth, int maxDepth)
    //{
    //    if (maxDepth == currentDepth)
    //    {
    //        return CountCurrentPoints(int zdjetepunkty, board);
    //    }
    //    List<Move> possibleMoves = getPossiblesMoves(...);
    //    float result = 0;
    //    foreach (var move in possibleMoves)
    //    {
    //        float p = Probability(tilesList, move);
    //        result += p * selectMax(result, Chance(DoMove(move),...))
    //    }
    //    return result;
    //}

    //public float Chance(int currentDepth, int maxDepth)
    //{
    //    if (maxDepth == currentDepth)
    //    {
    //        return CountCurrentPoints(int zdjetepunkty, board);
    //    }
    //    List<Move> possibleMoves = GetPossiblesMoves(...);
    //    float result = 8192;
    //    foreach (var move in possibleMoves)
    //    {
    //        float p = probability(move);
    //        result += p * selectMin(result, max(DoMove(move),...))
    //    }
    //    return result;
    //}

    public float DoMove(Move move)
    {
        float result = 0.0f;

        return result;
    }

    public List<Move> GetPossibleMoves(ref GameObject[,] tiles, ref GameObject[,] meeples, List<Tile> tileList, int meeplesLeft)
    {
        List<Move> MovesList = null;
        List<int[]> possiblePositions;
        List<Area> possibleMeeple = new List<Area>();

        Move tmp = new Move();
        

        foreach (Tile tile in tileList)
        {
            tmp.tile = tile;
            possiblePositions = TM.findMatchingEdges(TM.findSourrounding(ref tiles), tile, ref tiles);
            if (possiblePositions != null)
            {
                foreach (int[] pos in possiblePositions)
                {
                    tmp.x = pos[0];
                    tmp.y = pos[1];

                    MovesList.Add(tmp); // dodaje bez meepla
                    possibleMeeple = TM.possibleMeepleAreas(ref tiles, pos[0], pos[1]);

                    if (meeplesLeft > 0)
                    {
                        foreach (Area area in possibleMeeple)
                        {
                            tmp.meeplePosition = area;
                            MovesList.Add(tmp);
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
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
   


}
