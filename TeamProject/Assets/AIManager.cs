using UnityEngine;
using System.Collections.Generic;

public class AIManager : MonoBehaviour {

    class Move
    {
        Tile tile;
        int x;
        int y;
        List<int> meeplePosition;
        float score = 0;
    };

    //bool terminal()
    //{
    //    return true;
    //}

    //public Move Expectimax(int maxDepth =2)
    //{
    //    List<Move> possibleMoves = getPossiblesMoves(...);
    //    List<Move> selectedMovesList;
    //    Move bestMove ;
    //    foreach (var move in possibleMoves)
    //    {
    //        move.score = chance(domove(move)...);
    //        selectedMovesList.Add(move);
    //    }
    //    foreach (var move in selectedMovesList)
    //    {
    //        if (move.score > bestMove.score)
    //        {
    //            bestMove = move;
    //        }
    //    }
    //    return bestMove;
    //}

    //public float max(int currentDepth, int maxDepth)
    //{
    //    if (maxDepth == currentDepth)
    //    {
    //        return countCurrentPoints(int zdjetepunkty, board);
    //    }
    //    List<Move> possibleMoves = getPossiblesMoves(...);
    //    float result = 0;
    //    foreach (var move in possibleMoves)
    //    {
    //        float p = probability(move);
    //        result += p * selectMax(result,chance(domove(move),...))
    //    }
    //    return result;
    //}

    //public float chance()
    //{
    //    if (maxDepth == currentDepth)
    //    {
    //        return countCurrentPoints(int zdjetepunkty, board);
    //    }
    //    List<Move> possibleMoves = getPossiblesMoves(...);
    //    float result = 8192;
    //    foreach (var move in possibleMoves)
    //    {
    //        float p = probability(move);
    //        result += p* selectMin(result, max(domove(move),...))
    //    }
    //    return result;
    //}

    public double probability(List<Tile> tilesList, Tile tile)
    {
        double result = 0.0;
        bool check = false;
        double count = 0;
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
