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
        TileAI[,] simulation = new TileAI[200, 200];
        TileAI current = new TileAI();
        current.Clone(currentTile);

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

        List<Move> possibleMoves = GetPossibleMoves(simulation, current, players[0]); // 0 to AI
        Debug.Log("---------------------------------------------------");
        for (int i = 0; i < possibleMoves.Count; i++)
        {
            Debug.Log(i+ ". Move: "+ possibleMoves[i].tile.Material.name + " pos: " + possibleMoves[i].x + ", " + possibleMoves[i].y +" rotation: " + possibleMoves[i].tile.Rotation);
        }
        Debug.Log("---------------------------------------------------");
        Move bestMove = possibleMoves[possibleMoves.Count /2];
        //Move bestMove = new Move();

        //Move iterator = new Move();

        //for (int u = 0; u < possibleMoves.Count; u++)
        //{
        //    // kopia
        //    Tile[,] tempBoard = new Tile[200, 200];
        //    for (int x = 0; x < simulation.GetLength(0); x++)
        //    {
        //        for (int y = 0; y < simulation.GetLength(1); y++)
        //        {
        //            if (simulation[x, y] != null)
        //            {
        //                tempBoard[x, y].Clone(simulation[x, y]);
        //            }
        //        }
        //    }

        //    possibleMoves[u] = DoMove(possibleMoves[u], ref tempBoard, ref tileList, ref players, 1);
        //    possibleMoves[u].score += MinMax(tempBoard, tileList, 0, maxDepth, players, 1); //1 to chance
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
            possibleMoves = GetPossibleMoves1(ref tiles, tileList, players[1]);
        }
        else
        {
            possibleMoves = GetPossibleMoves1(ref tiles, tileList, players[0]);
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

    public List<Move> GetPossibleMoves1(ref TileAI[,] tiles, List<TileAI> tileList, Player player)
    {
        //for (int x = 0; x < tiles.GetLength(0); x++)
        //{
        //    for (int y = 0; y < tiles.GetLength(1); y++)
        //    {
        //        if (!System.Object.Equals(tiles[x, y], null))
        //        {
        //            Debug.Log(tiles[x, y].Material.name);
        //        }
        //    }
        //}

        List<Move> MovesList = new List<Move>();
        List<int[]> possiblePositions = new List<int[]>();
        List<int> possibleRotations = new List<int>();
        List<Area> possibleMeeple = new List<Area>();

        Move tmp = new Move();

        //Tile tmptile = new Tile();
        tmp.tile = new TileAI();

        int help = -1;

       for (int k = 0; k < tileList.Count; k++)
        {
            tmp.tile.Clone(tileList[k]);
           

            if (tmp.tile.IdNumber != help /*|| tmptile == null*/)
            {
                help = tmp.tile.IdNumber;
                Debug.Log("jiji" + tileList[k].IdNumber);
                //if (tmp.tile == null)
                //{
                //    Debug.Log("tmp.tile null!");
                //}
                Debug.Log("surrounding count" + TM.findSourrounding(ref tiles).Count);

                possiblePositions = TM.findMatchingEdges(TM.findSourrounding(ref tiles), tmp.tile, ref tiles);
                Debug.Log("positions count" + possiblePositions.Count);
                if (possiblePositions == null)
                {
                    Debug.Log("brak pozycji");
                }

                if (possiblePositions != null)
                {
                    foreach (int[] pos in possiblePositions)
                    {
                        tmp.x = pos[0];
                        tmp.y = pos[1];

                        Move move = new Move();
                        Debug.Log(tmp.x + " " + tmp.y);
                        move.Clone(tmp);
                        Debug.Log(move.x + " " + move.y);
                        //MovesList.Add(move); // dodaje bez meepla

                        possibleRotations = TM.possibleRotations(tmp.tile, tiles, pos);
                        foreach (int rotation in possibleRotations)
                        {
                            if (rotation == 1)
                            {
                                TM.rotateClockwise90(ref tmp.tile);
                                
                            }
                            else if(rotation == 2)
                            {
                                TM.rotateClockwise90(ref tmp.tile);
                                TM.rotateClockwise90(ref tmp.tile);
                            }
                            else if (rotation == 3)
                            {
                                TM.rotateClockwise90(ref tmp.tile);
                                TM.rotateClockwise90(ref tmp.tile);
                                TM.rotateClockwise90(ref tmp.tile);
                            }

                            tiles[pos[0], pos[1]] = new TileAI();
                            tiles[pos[0], pos[1]].Clone(tmp.tile);

                            possibleMeeple = TM.possibleMeepleAreas(ref tiles, pos[0], pos[1]);
                            //tiles[pos[0], pos[1]] = null;

                            if (player.meeples > 0)
                            {
                                foreach (Area area in tmp.tile.Areas )
                                {
                                    if (possibleMeeple.Any(a => a.meeplePlacementIndex == area.meeplePlacementIndex))
                                    {
                                        area.player = player;
                                        Debug.Log(tmp.x + " " + tmp.y);

                                        move.Clone(tmp);

                                        Debug.Log(move.x + " " + move.y);
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
        Debug.Log("wychodze z pętli");
        return MovesList;
    }
    public List<Move> GetPossibleMoves(TileAI[,] tiles, TileAI tile, Player player)
    {
        // What we return;
        List<Move> MovesList = new List<Move>();

        List<int[]> possiblePositions = new List<int[]>(); //OK
        List<int> possibleRotations = new List<int>(); 
        List<Area> possibleMeeple = new List<Area>();

        Move tmp = new Move();
        tmp.tile = new TileAI();
        tmp.tile.Clone(tile);

        possiblePositions = TM.findMatchingEdges(TM.findSourrounding(ref tiles), tmp.tile, ref tiles);

        if (possiblePositions != null)
        {
            Debug.Log("Ilość możliwych pozycji: "+possiblePositions.Count);
            foreach (int[] pos in possiblePositions)
            {
                tmp.x = pos[0];
                tmp.y = pos[1];

                possibleRotations = TM.possibleRotations(tmp.tile, tiles, pos);
                if (possibleRotations == null)
                {
                    Debug.Log("brak pozycji");
                }
                else
                {
                    Debug.Log("ilość możliwych rotacji: " + possibleRotations.Count);
                }

                foreach (int rotation in possibleRotations)
                {
                    Move move = new Move();
                    move.Clone(tmp);
                    

                    if (rotation == 0)
                    {
                        Debug.Log("rotacja" + move.tile.Rotation);
                        move.rotation = rotation;
                        MovesList.Add(move);
                    }
                    if (rotation == 1)
                    {
                        TM.rotateClockwise90(ref move.tile);
                        Debug.Log("rotacja" + move.tile.Rotation);
                        move.rotation = rotation;
                        MovesList.Add(move); // dodaje bez meepla
                    }
                    else if (rotation == 2)
                    {

                        TM.rotateClockwise90(ref move.tile);
                        TM.rotateClockwise90(ref move.tile);
                        move.rotation = rotation;
                        Debug.Log("rotacja" + move.tile.Rotation);
                        MovesList.Add(move); // dodaje bez meepla
                    }
                    else if (rotation == 3)
                    {
                        TM.rotateClockwise90(ref move.tile);
                        TM.rotateClockwise90(ref move.tile);
                        TM.rotateClockwise90(ref move.tile);
                        move.rotation = rotation;
                        Debug.Log("rotacja" + move.tile.Rotation);
                        MovesList.Add(move); // dodaje bez meepla
                    }

                    //tiles[pos[0], pos[1]] = new TileAI();
                    //tiles[pos[0], pos[1]].Clone(tmp.tile);
                    //possibleMeeple = TM.possibleMeepleAreas(ref tiles, pos[0], pos[1]);
                    //Debug.Log("possible meeple count: "+ possibleMeeple.Count);


                    //tiles[pos[0], pos[1]] = null;

                    //if (player.meeples > 0)
                    //{
                    //    foreach (Area area in tmp.tile.Areas)
                    //    {
                    //        Debug.Log("HOphp");
                    //        //if (possibleMeeple.Any(a => a. == area.colorIndex))
                    //        //{
                            
                    //            area.player = player;
                            

                    //            if (area.player == null)
                    //            {
                    //                Debug.Log("coś nie tak");
                    //            }
                    //            Debug.Log(tmp.x + " " + tmp.y);

                    //            move.Clone(tmp);
                    //            if (move.tile.Areas.Find(a => a.player != null) == null)
                    //            {
                    //                Debug.Log("coś nie tak !!!!!!!!!!!!!!");
                    //            }



                    //            Debug.Log(move.tile.Areas.Find(a => a.player != null).colorIndex);

                    //            Debug.Log(move.x + " " + move.y);
                    //            MovesList.Add(move);

                    //            area.player = null;
                    //        //}
                    //    }
                    //}
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
