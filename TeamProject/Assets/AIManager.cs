using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class Move
{
    //public TilesManager TM;

    public TileAI tile;
    public int x;
    public int y;
    //public List<int> meeplePosition;
    //public Area meeplePosition;
    public float score = 0;
    public int rotation = 0;

    public void Clone(Move other)
    {
        this.tile = new TileAI();
        this.tile.Clone(other.tile);
        this.x = other.x;
        this.y = other.y;
        this.score = other.score;
        this.rotation = other.rotation;
    }
};

public class AIManager : MonoBehaviour {

    public TilesManager TM;
    public PointsCounter PC;
    public GameManager GM;

    public Move Expectimax(GameObject[,] tiles, List<Tile> tileList, Tile currentTile, int maxDepth, List<Player> players)
    {
        // currentTile
        TileAI currentlyRolledTile = new TileAI();
        currentlyRolledTile.Clone(currentTile);

        // tiles
        TileAI[,] simulation = new TileAI[200, 200];
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1);y++)
            {
                if (tiles[x,y] != null)
                {
                    if (tiles[x, y].GetComponent<Tile>() != null)
                    {
                        simulation[x, y] = new TileAI();
                        simulation[x, y].Clone(tiles[x, y].GetComponent<Tile>());
                        //Debug.Log(simulation[x, y].Material.name);
                    }  
                }
            }
        }

        // tileList
        List<TileAI> tileListTemp = new List<TileAI>();
        
        for (int i = tileList.Count -1 ; i > 0-1; i--)
        {
            TileAI tmp = new TileAI();
            tmp.Clone(tileList[i]);
            //Debug.Log(tmp.Material.name);
            tileListTemp.Add(tmp);
        }


        //List<Move> possibleMoves = GetPossibleMoves1(simulation, tileListTemp, players[1]); // tutaj dla wszystkich tiles test
        List<Move> possibleMoves = GetPossibleMoves(simulation, currentlyRolledTile, players[1]); // 1 to AI jednak

        //Debug.Log("---------------------------------------------------");
        //for (int i = 0; i < possibleMoves.Count; i++)
        //{
        //    Debug.Log(i+ ". Move: "+ possibleMoves[i].tile.Material.name + " pos: " + possibleMoves[i].x + ", " + possibleMoves[i].y +" rotation: " + possibleMoves[i].tile.Rotation);
        //}
        //Debug.Log("---------------------------------------------------");


        // tymczasowy rand
        int rand = UnityEngine.Random.Range(0, possibleMoves.Count);
        Move bestMove = possibleMoves[rand];


        //Move bestMove = new Move();

        //Move iterator = new Move();

        //for (int u = 0; u < possibleMoves.Count; u++)
        //{
        //    // kopia
        //    TileAI[,] tempBoard = new TileAI[200, 200];
        //    for (int x = 0; x < simulation.GetLength(0); x++)
        //    {
        //        for (int y = 0; y < simulation.GetLength(1); y++)
        //        {
        //            if (simulation[x, y] != null)
        //            {
        //                tempBoard[x, y] = new TileAI();
        //                tempBoard[x, y].Clone(simulation[x, y]);
        //            }
        //        }
        //    }

        //    possibleMoves[u] = DoMove(possibleMoves[u], ref tempBoard, ref tileListTemp, ref players, 1);
        //    possibleMoves[u].score += MinMax(tempBoard, tileListTemp, 0, maxDepth, players, 1); //1 to chance
        //}
        //foreach (var move in possibleMoves)
        //{
        //    if (move.score > bestMove.score)
        //    {
        //        bestMove = move;
        //    }
        //}
        return bestMove;
    }



    public float MinMax(TileAI[,] tiles, List<TileAI> tileList, int currentDepth, int maxDepth, List<Player> players, int currentPlayer)
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
            possibleMoves = GetPossibleMoves1(tiles, tileList, players[1]);
        }
        else
        {
            possibleMoves = GetPossibleMoves1(tiles, tileList, players[0]);
        }
        foreach (var move in possibleMoves)
        {
            float p = Probability(tileList, move.tile);
            TileAI[,] tempBoard = new TileAI[200, 200];
            // kopia planszy
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    if (tiles[x, y] != null)
                    {
                        tempBoard[x, y] = new TileAI();
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

    public Move DoMove(Move move, ref TileAI[,] tiles, ref List<TileAI> tileList, ref List<Player> players, int currentPlayer)
    {
        tiles[move.x, move.y] = new TileAI();
        tiles[move.x, move.y].Clone(move.tile);
        tileList.Remove(move.tile); // moze nie dziala nie weim

        int tmp = players[currentPlayer].points;
        PC.countPointsAfterMove(ref tiles, ref players, move.x, move.y, false);

        //Wykonaj obecny ruch
        //Policz punkty za obecny ruch i zmień odpowiednio planszę
        move.score = players[currentPlayer].points - tmp;
        return move;
    }

    public float CountCurrentPoints(ref TileAI[,] tiles, ref List<Player> players, int currentPlayer)
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

    public List<Move> GetPossibleMoves1(TileAI[,] tiles, List<TileAI> tileList, Player player)
    {
        // What we return;
        List<Move> MovesList = new List<Move>();

        List<int[]> possiblePositions = new List<int[]>();
        List<int> possibleRotations = new List<int>();
        List<Area> possibleMeeple = new List<Area>();

        Move tmp = new Move();
        Move tmp2 = new Move();
        Move tmp3 = new Move();
        tmp.tile = new TileAI();

        int help = -1;

        for (int k = 0; k < tileList.Count; k++)
        {
            tmp.tile.Clone(tileList[k]);
            Debug.Log(tmp.tile.Material.name + " " + tmp.tile.IdNumber);

            if (tmp.tile.IdNumber != help)
            {
                help = tmp.tile.IdNumber;

                // Position
                possiblePositions = TM.findMatchingEdges(TM.findSourrounding(ref tiles), tmp.tile, ref tiles);
                if (possiblePositions != null)
                {
                    foreach (int[] pos in possiblePositions)
                    {
                        tmp.x = pos[0];
                        tmp.y = pos[1];

                        // Rotation
                        possibleRotations = TM.possibleRotations(tmp.tile, tiles, pos);
                        foreach (int rotation in possibleRotations)
                        {
                            tmp2 = new Move();
                            tmp2.Clone(tmp);
                    
                            if (rotation == 0)
                            {
                                tmp2.rotation = rotation;
                                MovesList.Add(tmp2);
                            }
                            if (rotation == 1)
                            {
                                TM.rotateClockwise90(ref tmp2.tile);
                                tmp2.rotation = rotation;
                                MovesList.Add(tmp2); // dodaje bez meepla
                            }
                            else if (rotation == 2)
                            {

                                TM.rotateClockwise90(ref tmp2.tile);
                                TM.rotateClockwise90(ref tmp2.tile);
                                tmp2.rotation = rotation;
                                MovesList.Add(tmp2); // dodaje bez meepla
                            }
                            else if (rotation == 3)
                            {
                                TM.rotateClockwise90(ref tmp2.tile);
                                TM.rotateClockwise90(ref tmp2.tile);
                                TM.rotateClockwise90(ref tmp2.tile);
                                tmp2.rotation = rotation;
                                MovesList.Add(tmp2); // dodaje bez meepla
                            }

                            // Meeple
                            tiles[pos[0], pos[1]] = new TileAI();
                            tiles[pos[0], pos[1]].Clone(tmp2.tile);
                            possibleMeeple = TM.possibleMeepleAreas(ref tiles, pos[0], pos[1]);
                            tiles[pos[0], pos[1]] = null;
                            if (player.meeples > 0)
                            {
                                foreach (Area area in tmp2.tile.Areas)
                                {
                                    if (possibleMeeple.Any(a => a.meeplePlacementIndex == area.meeplePlacementIndex))
                                    {
                                        area.player = player;
                                        tmp3 = new Move();
                                        tmp3.Clone(tmp2);
                                        MovesList.Add(tmp3);
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

    public List<Move> GetPossibleMoves(TileAI[,] tiles, TileAI tile, Player player)
    {
        // What we return;
        List<Move> MovesList = new List<Move>();

        List<int[]> possiblePositions = new List<int[]>(); 
        List<int> possibleRotations = new List<int>();  
        List<Area> possibleMeeple = new List<Area>(); 

        Move tmp = new Move();
        Move tmp2 = new Move();
        Move tmp3 = new Move();

        tmp.tile = new TileAI();
        tmp.tile.Clone(tile);

        // Position
        possiblePositions = TM.findMatchingEdges(TM.findSourrounding(ref tiles), tmp.tile, ref tiles);
        if (possiblePositions != null)
        {
            Debug.Log("Ilość możliwych pozycji: "+possiblePositions.Count);
            foreach (int[] pos in possiblePositions)
            {
                tmp.x = pos[0];
                tmp.y = pos[1];

                // Rotation
                possibleRotations = TM.possibleRotations(tmp.tile, tiles, pos);
                foreach (int rotation in possibleRotations)
                {
                    tmp2 = new Move();
                    tmp2.Clone(tmp);
                    
                    if (rotation == 0)
                    {
                        Debug.Log("rotacja" + tmp2.tile.Rotation);
                        tmp2.rotation = rotation;
                        MovesList.Add(tmp2);
                    }
                    if (rotation == 1)
                    {
                        TM.rotateClockwise90(ref tmp2.tile);
                        Debug.Log("rotacja" + tmp2.tile.Rotation);
                        tmp2.rotation = rotation;
                        MovesList.Add(tmp2); // dodaje bez meepla
                    }
                    else if (rotation == 2)
                    {

                        TM.rotateClockwise90(ref tmp2.tile);
                        TM.rotateClockwise90(ref tmp2.tile);
                        tmp2.rotation = rotation;
                        Debug.Log("rotacja" + tmp2.tile.Rotation);
                        MovesList.Add(tmp2); // dodaje bez meepla
                    }
                    else if (rotation == 3)
                    {
                        TM.rotateClockwise90(ref tmp2.tile);
                        TM.rotateClockwise90(ref tmp2.tile);
                        TM.rotateClockwise90(ref tmp2.tile);
                        tmp2.rotation = rotation;
                        Debug.Log("rotacja" + tmp2.tile.Rotation);
                        MovesList.Add(tmp2); // dodaje bez meepla
                    }

                    // Meeple
                    tiles[pos[0], pos[1]] = new TileAI();
                    tiles[pos[0], pos[1]].Clone(tmp2.tile);
                    possibleMeeple = TM.possibleMeepleAreas(ref tiles, pos[0], pos[1]);
                    tiles[pos[0], pos[1]] = null;
                    Debug.Log("player meeples: " + player.meeples);
                    if (player.meeples > 0)
                    {
                        foreach (Area area in tmp2.tile.Areas)
                        {
                            if (possibleMeeple.Any(a => a.meeplePlacementIndex == area.meeplePlacementIndex))
                            {
                                area.player = player;
                                tmp3 = new Move();
                                tmp3.Clone(tmp2);
                                MovesList.Add(tmp3);
                                area.player = null;
                            }
                        }
                    }
                }
            }
        }
        Debug.Log("wychodze z pętli");
        if (MovesList != null)
        {
            Debug.Log("Ilość możliwych ruchów: " + MovesList.Count);
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
    public float Probability(List<TileAI> tilesList, TileAI tile)
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
}
