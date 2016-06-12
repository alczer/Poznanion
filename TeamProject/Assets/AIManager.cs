using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
//using UnityEditor;

public class Move
{
    public TileAI tile;
    public int x;
    public int y;

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

    private TileAI[,] simulation;

    public Move Expectimax(GameObject[,] tiles, List<Tile> tileList, Tile currentTile, int maxDepth, List<Player> players)
    {
        // currentTile clone
        TileAI currentlyRolledTile = new TileAI();
        currentlyRolledTile.Clone(currentTile);

        // tiles
        simulation = new TileAI[200, 200];
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

        // players clone
        //List<Player> playersTmp = new List<Player>();
        //foreach (var player in players)
        //{
        //    Player tmp = new Player();
        //    tmp.Clone(player);
        //    playersTmp.Add(tmp);
        //}

        //int ioio = 1;
        //foreach (var tile in simulation)
        //{
        //    if(tile != null)
        //    {
        //        Debug.Log("Kafelek nr " + ioio);
        //        String result4 = "";
        //        for (int pp = 0; pp < tile.Areas.Count;pp++)
        //        {
        //            result4 += String.Join(" ", tile.Areas[pp].edges.Select(item => item.ToString()).ToArray());
        //            result4 += " | ";

        //        }
        //        Debug.Log(result4+"\n");
        //        ioio++;
        //    }
        //}
        
        // tileList
        //List<TileAI> tileListTemp = new List<TileAI>();
        
        //for (int i = tileList.Count -1 ; i > 0-1; i--)
        //{
        //    TileAI tmp = new TileAI();
        //    tmp.Clone(tileList[i]);
        //    tileListTemp.Add(tmp);
        //}
        //Debug.Log("Na liście jest "+tileListTemp.Count+" kafelków o numerach:");
        //foreach (TileAI t in tileListTemp)
        //{
        //    Debug.Log(t.IdNumber);
        //}
        // List<Move> possibleMoves = GetPossibleMoves1(simulation, tileListTemp, players[1]); // tutaj dla wszystkich tiles test

        List<Move> possibleMoves = GetPossibleMoves(currentlyRolledTile, players[1], tileList.Count); // 1 to AI jednak
        //foreach (Move move in possibleMoves)
        //{
        //    Debug.Log(" x: " + move.x + " y: " + move.y + " rot: " + move.tile.Rotation+" "+move.rotation  +" typ tila: "+move.tile.IdNumber+ " tyle razy ma się powtarzać dla każdej pozycji i obrotu (+1)" + move.tile.Areas.Count);
        //}
        // tymczasowy rand
        //int rand = UnityEngine.Random.Range(0, possibleMoves.Count);
        //Move bestMove = possibleMoves[rand];
       
        for (int u = 0; u < possibleMoves.Count; u++)
        {

            List<TileAI> tileListTemp = new List<TileAI>();

            for (int i = tileList.Count - 1; i > 0 - 1; i--)
            {
                TileAI tmp = new TileAI();
                tmp.Clone(tileList[i]);
                tileListTemp.Add(tmp);
            }
            //Debug.Log("Na liście jest " + tileListTemp.Count + " kafelków o numerach:");
            //foreach (TileAI t in tileListTemp)
            //{
            //    Debug.Log(t.IdNumber);
            //}

            // kopia
            //TileAI[,] tempBoard = new TileAI[200, 200];
            //for (int x = 0; x < simulation.GetLength(0); x++)
            //{
            //    for (int y = 0; y < simulation.GetLength(1); y++)
            //    {
            //        if (simulation[x, y] != null)
            //        {
            //            tempBoard[x, y] = new TileAI();
            //            tempBoard[x, y].Clone(simulation[x, y]);
            //        }
            //    }
            //}

            //Debug.Log("sprawdzamy czy temp się dobrze zresetował : "+tempBoard.Cast<TileAI>().Count(s => s != null));
            //Debug.Log("WYWOLUJE MINMAX DLA " + " x: " + possibleMoves[u].x + " y: " + possibleMoves[u].y + " rot: " + possibleMoves[u].tile.Rotation + " " + possibleMoves[u].rotation + " typ tila: " + possibleMoves[u].tile.IdNumber);
            // players clone
            List<Player> playersTmp = new List<Player>();
            foreach (var player in players)
            {
                Player tmp = new Player();
                tmp.Clone(player);
                tmp.points = 0;
                playersTmp.Add(tmp);
            }
            possibleMoves[u] = DoMove(possibleMoves[u], ref tileListTemp, ref playersTmp, 1);
            //possibleMoves[u].score += 0.5f * CountCurrentPoints(playersTmp, 1);

            // var itemtoremove = tileListTemp.Where(item => item.IdNumber == possibleMoves[u].tile.IdNumber).First();
            // tileListTemp.Remove(itemtoremove);
            //tileListTemp.Remove(possibleMoves[u].tile);
            
            //Debug.Log("sprawdzamy czy ruch się wykonał : " + tempBoard.Cast<TileAI>().Count(s => s != null));
            //Debug.Log("Na liście teraz jest " + tileListTemp.Count + " kafelków o numerach");
            //foreach (TileAI t in tileListTemp)
            //{
            //    Debug.Log(t.IdNumber);
            //}

            possibleMoves[u].score += MinMax(tileListTemp, 0, maxDepth, playersTmp, 1); //1 to chance
            simulation[possibleMoves[u].x, possibleMoves[u].y] = null;
        }

        float bestScore = -1;
        List<Move> bestMoves = new List<Move>();

        foreach (var move in possibleMoves)
        {
            //Debug.Log("tile "+ move.tile.IdNumber + " pos: " + move.x+","+move.y +" rot: " + move.tile.Rotation + " --punkty "+ move.score);
            if (move.score > bestScore)
            {
                bestScore = move.score;
            }
        }
        foreach (var move in possibleMoves)
        {
            if (move.score == bestScore)
            {
                Debug.Log("tile " + move.tile.IdNumber + " pos: " + move.x + "," + move.y + " rot: " + move.tile.Rotation + " --punkty " + move.score);
                bestMoves.Add(move);
            }
        }
        int rand = UnityEngine.Random.Range(0, bestMoves.Count);    

        return bestMoves[rand];
    }

    public float MinMax(List<TileAI> tileList, int currentDepth, int maxDepth, List<Player> players, int currentPlayer)
    {
        //Debug.Log("Jestem w minmaxie player " + currentPlayer + " głębokość "+ currentDepth+"/"+maxDepth);
        //Debug.Log("MINMAX Na liście teraz jest " + tileList.Count + " kafelków o numerach");
        //foreach (TileAI t in tileList)
        //{
        //    Debug.Log(t.IdNumber);

        //}
        int otherPlayer = 1;
        if (currentPlayer == 1)
        {
            otherPlayer = 0;
        }

        if (maxDepth == currentDepth)
        {
            //Debug.Log("nie jest inne "+ maxDepth+" == "+currentDepth+" więc returnuję");
            float currentp = CountCurrentPoints(players, otherPlayer);
            //Debug.Log("currentp: " + currentp);
            return currentp;
        }
        else
        {
            //Debug.Log(maxDepth+" jest inne niz "+currentDepth);
        }

        List<Move> possibleMoves;

        float result = 0;
        if (currentPlayer == 0)
        {
            result = 8192f;
            possibleMoves = GetPossibleMoves(tileList[currentDepth], players[1], tileList.Count);
            //possibleMoves = GetPossibleMoves1(tileList, players[1]);
        }
        else
        {
            possibleMoves = GetPossibleMoves(tileList[currentDepth], players[0], tileList.Count);
            //possibleMoves = GetPossibleMoves1(tileList, players[0]);
        }

        //foreach (Move move in possibleMoves)
        //{
        //    Debug.Log(" MINMAX x: " + move.x + " y: " + move.y + " rot: " + move.tile.Rotation + " " + move.rotation + " typ tila: " + move.tile.IdNumber + " tyle razy ma się powtarzać dla każdej pozycji i obrotu (+1)" + move.tile.Areas.Count);

        //}

        //Debug.Log("hihihi");
        foreach (var move in possibleMoves)
        {
            List<TileAI> tileListTemp = new List<TileAI>();
            for (int i = tileList.Count - 1; i > 0 - 1; i--)
            {
                TileAI tmp = new TileAI();
                tmp.Clone(tileList[i]);
                tileListTemp.Add(tmp);
            }
            //Debug.Log("### SPRAWDZAM DLA " + move.tile.IdNumber + " x: " + move.x + " y: " + move.y + " rot: " + move.tile.Rotation);


            //float p = Probability(tileListTemp, move.tile);
            int p = 1;

            //TileAI[,] tempBoard = new TileAI[200, 200];
            //// kopia planszy
            //for (int x = 0; x < tiles.GetLength(0); x++)
            //{
            //    for (int y = 0; y < tiles.GetLength(1); y++)
            //    {
            //        if ((object)tiles[x, y] != null)
            //        {
            //            tempBoard[x, y] = new TileAI();
            //            tempBoard[x, y].Clone(tiles[x, y]);
            //        }
            //    }
            //}

            //Debug.Log("Kopiuję tempBoard z " + tempBoard.Cast<TileAI>().Count(s => s != null) + " tilami");

            int copyDepyth = currentDepth + 1;

            //currentDepth += 1;
            //Debug.Log("GŁĘBOKOŚĆ +1 !!!!!!!!!!!!!!!! RAZEM : " + currentDepth);
            if (currentPlayer == 0)
            {
                //int copyDepyth = currentDepth + 1;
                //Debug.Log("Wywołuję minmaxa dla 0 "+ currentDepth + "/" + maxDepth);
                Move tmp = DoMove(move, ref tileListTemp, ref players, 0);
                //Debug.Log("sprawdzamy czy ruch się wykonał : " + tempBoard.Cast<TileAI>().Count(s => s != null));
                result = p * Math.Min(result, tmp.score + MinMax(tileListTemp, copyDepyth, maxDepth, players, 0));
                //Debug.Log(result);
            }
            else
            {
                //Debug.Log("Wywołuję minmaxa dla 0 " + currentDepth + "/" + maxDepth);
                Move tmp = DoMove(move, ref tileListTemp, ref players, 1);
                result = p * Math.Max(result, tmp.score + MinMax(tileListTemp, copyDepyth, maxDepth, players, 1));
                //Debug.Log(result);
            }
            simulation[move.x, move.y] = null;
        }
        return result;
    }

    public Move DoMove(Move move, ref List<TileAI> tileList, ref List<Player> players, int currentPlayer)
    {
        //Debug.Log("Na liście teraz jest " + tileList.Count + " kafelków o numerach");
        //foreach (TileAI t in tileList)
        //{
        // //   Debug.Log(t.IdNumber);

        //}
        //Debug.Log("Usuwamy tile o id " + move.tile.IdNumber);


        List<Player> playersTmp = new List<Player>();
        foreach (var player in players)
        {
            Player tmpp = new Player();
            tmpp.Clone(player);
            tmpp.points = 0;
            playersTmp.Add(tmpp);
        }


        simulation[move.x, move.y] = new TileAI();
        simulation[move.x, move.y].Clone(move.tile);


       
        //tileList.Remove(move.tile); // moze nie dziala nie weim
        if (tileList.Count>0)
        {
            var itemtoremove = tileList.Where(item => item.IdNumber == move.tile.IdNumber).First();
            tileList.Remove(itemtoremove);
        }
        

        int tmp = playersTmp[currentPlayer].points;
        PC.countPointsAfterMove(ref simulation, ref playersTmp, move.x, move.y, false);

        //Wykonaj obecny ruch
        //Policz punkty za obecny ruch i zmień odpowiednio planszę
        move.score = playersTmp[currentPlayer].points - tmp;
        return move;
    }

    public float CountCurrentPoints(List<Player> players, int currentPlayer)
    {
        List<Player> playersTmp = new List<Player>();
        foreach (var player in players)
        {
            Player tmp = new Player();
            tmp.Clone(player);
            tmp.points = 0;
            playersTmp.Add(tmp);
        }
        float result = 0f;
        //policz punkty za niedokończone rzeczy
        int pointsDiff = playersTmp[currentPlayer].points;

        for (int x = 0; x < simulation.GetLength(0); x += 1)
        {
            for (int y = 0; y < simulation.GetLength(1); y += 1)
            {
                if ((object)simulation[x, y] != null)
                {
                   // Debug.Log("X : " + x + " Y: " + y);
                    //String result4 = "";
                   // foreach (var l in simulation[x, y].Areas)
                   // {
                   //     result4 += String.Join(" ", l.edges.Select(item => item.ToString()).ToArray());
                   //     result4 += " | ";

                   // }

                    //if ((object)simulation[x, y].Areas != null)
                    //{
                    foreach (var area in simulation[x, y].Areas)
                    {
                        if (area.player != null)
                        {
                            PC.countPointsAfterMove(ref simulation, ref playersTmp, x, y, true);
                            //Debug.Log(x + ", " + y + "   " + players[0].points + ", " + players[1].points);
                            
                        }
                    }

                    //}

                     //   if ((object)tiles[x, y].Areas.Find(a => a.player != null).player != null)
                  //  {
                 //       PC.countPointsAfterMove(ref tiles, ref players, x, y, true);
                  //  }
                }
            }
        }

        result = playersTmp[currentPlayer].points - pointsDiff;
       // Debug.Log("counting return: " + result);
        return result;
    }

    public List<Move> GetPossibleMoves1(List<TileAI> tileList, Player player)
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
            //Debug.Log(tmp.tile.Material.name + " " + tmp.tile.IdNumber);

            if (tmp.tile.IdNumber != help)
            {
                help = tmp.tile.IdNumber;

                // Position
                possiblePositions = TM.findMatchingEdges(TM.findSourrounding(ref simulation), tmp.tile, ref simulation);
                if (possiblePositions != null)
                {
                    foreach (int[] pos in possiblePositions)
                    {
                        tmp.x = pos[0];
                        tmp.y = pos[1];

                        // Rotation
                        possibleRotations = TM.possibleRotations(tmp.tile, simulation, pos);
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
                            simulation[pos[0], pos[1]] = new TileAI();
                            simulation[pos[0], pos[1]].Clone(tmp2.tile);
                            possibleMeeple = TM.possibleMeepleAreas(ref simulation, pos[0], pos[1]);
                            simulation[pos[0], pos[1]] = null;
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
    public List<Move> GetPossibleMoves(TileAI tile, Player player, int tilesLeft)
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
        possiblePositions = TM.findMatchingEdges(TM.findSourrounding(ref simulation), tmp.tile, ref simulation);
        if (possiblePositions != null)
        {
            foreach (int[] pos in possiblePositions)
            {
                tmp.x = pos[0];
                tmp.y = pos[1];

                // Rotation
                possibleRotations = TM.possibleRotations(tmp.tile, simulation, pos);
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
                    simulation[pos[0], pos[1]] = new TileAI();
                    simulation[pos[0], pos[1]].Clone(tmp2.tile);
                    possibleMeeple = TM.possibleMeepleAreas(ref simulation, pos[0], pos[1]);
                    simulation[pos[0], pos[1]] = null;
                    if (player.meeples > 0)
                    {
                        foreach (Area area in tmp2.tile.Areas)
                        {
                            if (possibleMeeple.Any(a => a.meeplePlacementIndex == area.meeplePlacementIndex))
                            {
                                if (area.terrain == terrainTypes.grass && tilesLeft > 20)
	                            {
                                    //skip
	                            }
                                else
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
