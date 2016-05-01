using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.UI;
using System;

public class TilesManager : MonoBehaviour
{
    //TILES:
    public List<Tile> tilesList = new List<Tile>(); //List of all disponible tiles (not on board)
    public Material CRFR;   // START
    public Material CFFF;   //
    public Material CFFC;
    public Material CFFC_2;
    public Material CFFC_P;
    public Material CCFC;   //
    public Material CCFC_P; //
    public Material CCRC;   //
    public Material CCRC_P; //
    public Material CCCC_P; //
    public Material CRRC; 
    public Material CRRC_P;
    public Material FCFC; 
    public Material FCFC_P;
    public Material CFCF_2;
    public Material CRRF;
    public Material CFRR;
    public Material CRRR;
    public Material FRFR;
    public Material FFRR;
    public Material FRRR;
    public Material RRRR;   //
    public Material FFRF_M; //
    public Material FFFF_M; //

    //MASKS:
    public Material CRFR_Mask;      //
    public Material CFFF_Mask;      //
    public Material CFFC_Mask;
    public Material CFFC_2_Mask;
    public Material CFFC_P_Mask;
    public Material CCFC_Mask;
    public Material CCFC_P_Mask;    //
    public Material CCRC_Mask;      //
    public Material CCRC_P_Mask;    //
    public Material CCCC_P_Mask;    //
    public Material CRRC_Mask;
    public Material CRRC_P_Mask;
    public Material FCFC_Mask;
    public Material FCFC_P_Mask;
    public Material CFCF_2_Mask;
    public Material CRRF_Mask;
    public Material CFRR_Mask;
    public Material CRRR_Mask;
    public Material FRFR_Mask;
    public Material FFRR_Mask;
    public Material FRRR_Mask;
    public Material RRRR_Mask;      //
    public Material FFRF_M_Mask;    //
    public Material FFFF_M_Mask;    //

    public struct AreaTuple
    {
        public int x;
        public int y;
        public Area area;
        public bool initialized;
    }
    public void addTileToList(terrainTypes up, terrainTypes right, terrainTypes down, terrainTypes left, float x, float y, Material m, Material mask, int count, int turn, bool plus, List<Area> areas)
    {
        Tile tmp = new Tile();
        tmp.Init(up, right, down, left, x, y, m, mask, count, turn, plus, areas);
        tilesList.Add(tmp);
    }
    public void placeTile(ref GameObject tile, ref GameObject mask, int x, int y, ref GameObject[,] tilesOnBoard, ref GameObject[,] masks)
    {
        tilesOnBoard[x, y] = Instantiate(tile, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        masks[x, y] = Instantiate(mask, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
    }

    public int[] getArrayPosition(float x, float z)
    {
        float col = 100 + (x / 10);
        float row = 100 - (z / 10);
        int[] array = new int[2] { (int)row, (int)col };
        return array;
    }

    public float[] getCoordinates(int row, int col)
    {
        float x = ((float)col - 100) * 10;
        float z = ((float)row - 100) * (-10);
        float[] array = new float[2] { x, z };
        return array;
    }
    public List<Area> possibleMeepleAreas(ref GameObject[,] board, int x, int y)
    {
        List<Area> possibleAreas = new List<Area>();
        //Check which area can hold meeple:
        //For each area on the tile
        List<AreaTuple> list = new List<AreaTuple>();
        foreach (var area in board[x, y].GetComponent<Tile>().Areas)
        {
            AreaTuple temp;
            temp.x = x;
            temp.y = y;
            temp.area = area;
            temp.initialized = true;
            list.Add(temp);
        }

        foreach (var area in board[x, y].GetComponent<Tile>().Areas)
        {
            //Check if we can place meeple in there
            //And add area to the list if we can.
            if (isMovePossible(ref board, x, y, area, list))
                possibleAreas.Add(area);
        }
        //Return the list with areas that are ready for meeple.
        return possibleAreas;
    }

    public AreaTuple areaNeighbour(ref GameObject[,] board, int x, int y, int edge)
    {
        AreaTuple neighbour = new AreaTuple();
        neighbour.initialized = false;
        int[] correspondingEdges = new int[13];
        correspondingEdges[1] = 9;
        correspondingEdges[2] = 8;
        correspondingEdges[3] = 7;
        correspondingEdges[4] = 12;
        correspondingEdges[5] = 11;
        correspondingEdges[6] = 10;
        correspondingEdges[7] = 3;
        correspondingEdges[8] = 2;
        correspondingEdges[9] = 1;
        correspondingEdges[10] = 6;
        correspondingEdges[11] = 5;
        correspondingEdges[12] = 4;
        neighbour.x = x;
        neighbour.y = y;
        if (edge == 1 || edge == 2 || edge == 3)
        {
            (neighbour.x)--;
        }
        else if (edge == 4 || edge == 5 || edge == 6)
        {
            (neighbour.y)++;
        }
        else if (edge == 7 || edge == 8 || edge == 9)
        {
            (neighbour.x)++;
        }
        else if (edge == 10 || edge == 11 || edge == 12)
        {
            (neighbour.y)--;
        }
        if (board[neighbour.x, neighbour.y] != null)
        {
            foreach (var area in board[neighbour.x, neighbour.y].GetComponent<Tile>().Areas)
            {
                if (area.edges.Contains(correspondingEdges[edge]))
                {
                    neighbour.area = area;
                    break;
                }
            }

            neighbour.initialized = true;
        }
        return neighbour;
    }

    public List<AreaTuple> areaNeighbours(ref GameObject[,] board, int x, int y, List<int> edges)
    {
        List<AreaTuple> neighbours = new List<AreaTuple>();
        foreach (var edge in edges)
        {
            AreaTuple neighbour = areaNeighbour(ref board, x, y, edge);
            if (neighbour.initialized)
                neighbours.Add(neighbour);
        }
        foreach (var n in neighbours)
        {
            Debug.Log("Klocek sąsiad to : " + n.x + " " + n.y);
            Debug.Log("Obszar klokcka sąsiada to : " + String.Join(" ", n.area.edges.Select(item => item.ToString()).ToArray()));
        }
        Debug.Log("---");
        //Debug.Log("wielkość listy:"+neighbours.Count);
        return neighbours;
    }

    public bool isMovePossible(ref GameObject[,] board, int x, int y, Area currentlyChecked, List<AreaTuple> checkedAreas)
    {
        //If the tile has meeple already, we can't place meeple in there.
        if (currentlyChecked.player != null)
            return false;
        //Proper algorithm: ensure existence of checkedAreas
        if (checkedAreas == null)
            checkedAreas = new List<AreaTuple>();
        //By this line we've checked current area. Now it's time for neighbours.
        //For each neighbouring area:
        foreach (var neighbourTuple in areaNeighbours(ref board, x, y, currentlyChecked.edges))
        {
            //If we've already checked it, go on
            if (checkedAreas.Contains(neighbourTuple))
                continue;
            checkedAreas.Add(neighbourTuple);
            //Otherwise ensure move recurrently. If it's impossible, return false.
            if (!isMovePossible(ref board, neighbourTuple.x, neighbourTuple.y, neighbourTuple.area, checkedAreas))
                return false;
        }
        //If we got here, it means that move is possible.
        return true;
    }

    public void rotateClockwise90(ref GameObject gameObject, ref GameObject mask)
    {
        terrainTypes up = gameObject.GetComponent<Tile>().UpTerrain;
        terrainTypes right = gameObject.GetComponent<Tile>().RightTerrain;
        terrainTypes down = gameObject.GetComponent<Tile>().DownTerrain;
        terrainTypes left = gameObject.GetComponent<Tile>().LeftTerrain;
        /*
                foreach (var area in gameObject.GetComponent<Tile>().Areas)
                {
                    for (int i = 0; i < area.edges.Count; i++)
                    {
                        if (area.edges[i] == 0)
                        {
                            //skip
                        }
                        else if (area.edges[i] < 10)
                        {
                            area.edges[i] += 3;
                        }
                        else
                        {
                            area.edges[i] = area.edges[i] + 3 - 12;
                        }
                    }
                }
        */
        for (int j = 0; j < gameObject.GetComponent<Tile>().Areas.Count; j++)
        {
            for (int i = 0; i < gameObject.GetComponent<Tile>().Areas[j].edges.Count; i++)
            {
                if (gameObject.GetComponent<Tile>().Areas[j].edges[i] == 0)
                {
                    //skip
                }
                else if (gameObject.GetComponent<Tile>().Areas[j].edges[i] < 10)
                {
                    gameObject.GetComponent<Tile>().Areas[j].edges[i] += 3;
                }
                else
                {
                    gameObject.GetComponent<Tile>().Areas[j].edges[i] = gameObject.GetComponent<Tile>().Areas[j].edges[i] + 3 - 12;
                }
            }


        }

        gameObject.GetComponent<Tile>().UpTerrain = left;
        gameObject.GetComponent<Tile>().RightTerrain = up;
        gameObject.GetComponent<Tile>().DownTerrain = right;
        gameObject.GetComponent<Tile>().LeftTerrain = down;
        gameObject.transform.Rotate(new Vector3(0, 90, 0));
        mask.transform.Rotate(new Vector3(0, 90, 0));
    }

    public void rotateFirstMatchingRotation(ref GameObject gameObject, ref GameObject mask, int[] gameObjectPosition, ref GameObject[,] tilesOnBoard)
    {
        if ((tilesOnBoard[gameObjectPosition[0] - 1, gameObjectPosition[1]] == null || tilesOnBoard[gameObjectPosition[0] - 1, gameObjectPosition[1]].GetComponent<Tile>().DownTerrain == gameObject.GetComponent<Tile>().LeftTerrain)
            && (tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] + 1] == null || tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] + 1].GetComponent<Tile>().LeftTerrain == gameObject.GetComponent<Tile>().UpTerrain)
            && (tilesOnBoard[gameObjectPosition[0] + 1, gameObjectPosition[1]] == null || tilesOnBoard[gameObjectPosition[0] + 1, gameObjectPosition[1]].GetComponent<Tile>().UpTerrain == gameObject.GetComponent<Tile>().RightTerrain)
            && (tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] - 1] == null || tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] - 1].GetComponent<Tile>().RightTerrain == gameObject.GetComponent<Tile>().DownTerrain))
        {

            Debug.Log("%%%%%%%%%%%%%%%%%%%%%");
            Debug.Log("przed obrotem:");
            String result5 = "";
            foreach (var l in tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().Areas)
            {
                result5 += String.Join(" ", l.edges.Select(item => item.ToString()).ToArray());
                result5 += " | ";

            }
            Debug.Log(result5);
            Debug.Log("UP: "+tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().UpTerrain.ToString() +
            " RIGH: " + tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().RightTerrain.ToString() +
            " DOWN: " + tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().DownTerrain.ToString() +
            " LEFT: " + tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().LeftTerrain.ToString());


            rotateClockwise90(ref gameObject, ref mask);
            Debug.Log("turning 1 time!");

            Debug.Log("Po obrocie:");

            String result6 = "";
            foreach (var l in tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().Areas)
            {
                result6 += String.Join(" ", l.edges.Select(item => item.ToString()).ToArray());
                result6 += " | ";

            }
            Debug.Log(result6);
            Debug.Log("UP: " + tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().UpTerrain.ToString() +
           " RIGH: " + tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().RightTerrain.ToString() +
           " DOWN: " + tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().DownTerrain.ToString() +
           " LEFT: " + tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().LeftTerrain.ToString());
            Debug.Log("%%%%%%%%%%%%%%%%%%%%%");
        }
        else if ((tilesOnBoard[gameObjectPosition[0] - 1, gameObjectPosition[1]] == null || tilesOnBoard[gameObjectPosition[0] - 1, gameObjectPosition[1]].GetComponent<Tile>().DownTerrain == gameObject.GetComponent<Tile>().DownTerrain)
            && (tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] + 1] == null || tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] + 1].GetComponent<Tile>().LeftTerrain == gameObject.GetComponent<Tile>().LeftTerrain)
            && (tilesOnBoard[gameObjectPosition[0] + 1, gameObjectPosition[1]] == null || tilesOnBoard[gameObjectPosition[0] + 1, gameObjectPosition[1]].GetComponent<Tile>().UpTerrain == gameObject.GetComponent<Tile>().UpTerrain)
            && (tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] - 1] == null || tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] - 1].GetComponent<Tile>().RightTerrain == gameObject.GetComponent<Tile>().RightTerrain))
        {
            Debug.Log("%%%%%%%%%%%%%%%%%%%%%");
            Debug.Log("przed obrotem:");
            String result5 = "";
            foreach (var l in tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().Areas)
            {
                result5 += String.Join(" ", l.edges.Select(item => item.ToString()).ToArray());
                result5 += " | ";

            }
            Debug.Log(result5);
            Debug.Log("UP: " + tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().UpTerrain.ToString() +
           " RIGH: " + tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().RightTerrain.ToString() +
           " DOWN: " + tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().DownTerrain.ToString() +
           " LEFT: " + tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().LeftTerrain.ToString());
            rotateClockwise90(ref gameObject, ref mask);
            rotateClockwise90(ref gameObject, ref mask);
            Debug.Log("turning 2 times!");
            Debug.Log("Po obrocie:");

            String result6 = "";
            foreach (var l in tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().Areas)
            {
                result6 += String.Join(" ", l.edges.Select(item => item.ToString()).ToArray());
                result6 += " | ";

            }
            Debug.Log(result6);
            Debug.Log("UP: " + tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().UpTerrain.ToString() +
           " RIGH: " + tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().RightTerrain.ToString() +
           " DOWN: " + tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().DownTerrain.ToString() +
           " LEFT: " + tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().LeftTerrain.ToString());
            Debug.Log("%%%%%%%%%%%%%%%%%%%%%");
        }
        else if ((tilesOnBoard[gameObjectPosition[0] - 1, gameObjectPosition[1]] == null || tilesOnBoard[gameObjectPosition[0] - 1, gameObjectPosition[1]].GetComponent<Tile>().DownTerrain == gameObject.GetComponent<Tile>().RightTerrain)
            && (tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] + 1] == null || tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] + 1].GetComponent<Tile>().LeftTerrain == gameObject.GetComponent<Tile>().DownTerrain)
            && (tilesOnBoard[gameObjectPosition[0] + 1, gameObjectPosition[1]] == null || tilesOnBoard[gameObjectPosition[0] + 1, gameObjectPosition[1]].GetComponent<Tile>().UpTerrain == gameObject.GetComponent<Tile>().LeftTerrain)
            && (tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] - 1] == null || tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] - 1].GetComponent<Tile>().RightTerrain == gameObject.GetComponent<Tile>().UpTerrain))
        {
            Debug.Log("%%%%%%%%%%%%%%%%%%%%%");
            Debug.Log("przed obrotem:");
            String result5 = "";
            foreach (var l in tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().Areas)
            {
                result5 += String.Join(" ", l.edges.Select(item => item.ToString()).ToArray());
                result5 += " | ";

            }
            Debug.Log(result5);
            Debug.Log("UP: " + tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().UpTerrain.ToString() +
           " RIGH: " + tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().RightTerrain.ToString() +
           " DOWN: " + tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().DownTerrain.ToString() +
           " LEFT: " + tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().LeftTerrain.ToString());
            rotateClockwise90(ref gameObject, ref mask);
            rotateClockwise90(ref gameObject, ref mask);
            rotateClockwise90(ref gameObject, ref mask);
            Debug.Log("turning 3 times!");
            Debug.Log("Po obrocie:");

            String result6 = "";
            foreach (var l in tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().Areas)
            {
                result6 += String.Join(" ", l.edges.Select(item => item.ToString()).ToArray());
                result6 += " | ";

            }
            Debug.Log(result6);
            Debug.Log("UP: " + tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().UpTerrain.ToString() +
           " RIGH: " + tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().RightTerrain.ToString() +
           " DOWN: " + tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().DownTerrain.ToString() +
           " LEFT: " + tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1]].GetComponent<Tile>().LeftTerrain.ToString());
            Debug.Log("%%%%%%%%%%%%%%%%%%%%%");
        }
    }
    public List<int[]> findMatchingEdges(List<int[]> movesList, Tile choosenTile, ref GameObject[,] tilesOnBoard)
    {
        List<int[]> matchingEdges = new List<int[]>();
        foreach (var position in movesList)
        {
            //UP RIGHT DOWN LEFT 0
            if ((tilesOnBoard[position[0] - 1, position[1]] == null || tilesOnBoard[position[0] - 1, position[1]].GetComponent<Tile>().DownTerrain == choosenTile.UpTerrain)
                && (tilesOnBoard[position[0], position[1] + 1] == null || tilesOnBoard[position[0], position[1] + 1].GetComponent<Tile>().LeftTerrain == choosenTile.RightTerrain)
                && (tilesOnBoard[position[0] + 1, position[1]] == null || tilesOnBoard[position[0] + 1, position[1]].GetComponent<Tile>().UpTerrain == choosenTile.DownTerrain)
                && (tilesOnBoard[position[0], position[1] - 1] == null || tilesOnBoard[position[0], position[1] - 1].GetComponent<Tile>().RightTerrain == choosenTile.LeftTerrain))
            {
                matchingEdges.Add(position);
            }
            // UP RIGHT DOWN LEFT 90
            else if ((tilesOnBoard[position[0] - 1, position[1]] == null || tilesOnBoard[position[0] - 1, position[1]].GetComponent<Tile>().DownTerrain == choosenTile.LeftTerrain)
                && (tilesOnBoard[position[0], position[1] + 1] == null || tilesOnBoard[position[0], position[1] + 1].GetComponent<Tile>().LeftTerrain == choosenTile.UpTerrain)
                && (tilesOnBoard[position[0] + 1, position[1]] == null || tilesOnBoard[position[0] + 1, position[1]].GetComponent<Tile>().UpTerrain == choosenTile.RightTerrain)
                && (tilesOnBoard[position[0], position[1] - 1] == null || tilesOnBoard[position[0], position[1] - 1].GetComponent<Tile>().RightTerrain == choosenTile.DownTerrain))
            {
                matchingEdges.Add(position);
            }
            // UP RIGHT DOWN LEFT 180
            else if ((tilesOnBoard[position[0] - 1, position[1]] == null || tilesOnBoard[position[0] - 1, position[1]].GetComponent<Tile>().DownTerrain == choosenTile.DownTerrain)
                && (tilesOnBoard[position[0], position[1] + 1] == null || tilesOnBoard[position[0], position[1] + 1].GetComponent<Tile>().LeftTerrain == choosenTile.LeftTerrain)
                && (tilesOnBoard[position[0] + 1, position[1]] == null || tilesOnBoard[position[0] + 1, position[1]].GetComponent<Tile>().UpTerrain == choosenTile.UpTerrain)
                && (tilesOnBoard[position[0], position[1] - 1] == null || tilesOnBoard[position[0], position[1] - 1].GetComponent<Tile>().RightTerrain == choosenTile.RightTerrain))
            {
                matchingEdges.Add(position);
            }
            // UP RIGHT DOWN LEFT 270
            else if ((tilesOnBoard[position[0] - 1, position[1]] == null || tilesOnBoard[position[0] - 1, position[1]].GetComponent<Tile>().DownTerrain == choosenTile.RightTerrain)
                && (tilesOnBoard[position[0], position[1] + 1] == null || tilesOnBoard[position[0], position[1] + 1].GetComponent<Tile>().LeftTerrain == choosenTile.DownTerrain)
                && (tilesOnBoard[position[0] + 1, position[1]] == null || tilesOnBoard[position[0] + 1, position[1]].GetComponent<Tile>().UpTerrain == choosenTile.LeftTerrain)
                && (tilesOnBoard[position[0], position[1] - 1] == null || tilesOnBoard[position[0], position[1] - 1].GetComponent<Tile>().RightTerrain == choosenTile.UpTerrain))
            {
                matchingEdges.Add(position);
            }
        }
        return matchingEdges.Distinct().ToList();
    }
    public List<int[]> findSourrounding(ref GameObject[,] tilesOnBoard)
    {
        List<int[]> possiblePositions = new List<int[]>();
        for (int row = 0; row < tilesOnBoard.GetLength(0); row++)
        {
            for (int col = 0; col < tilesOnBoard.GetLength(1); col++)
            {
                if (tilesOnBoard[row, col] != null)
                {
                    //UP
                    if (tilesOnBoard[row - 1, col] == null)
                    {
                        possiblePositions.Add(new int[] { row - 1, col });
                    }
                    //RIGHT
                    if (tilesOnBoard[row, col + 1] == null)
                    {
                        possiblePositions.Add(new int[] { row, col + 1 });
                    }
                    //DOWN
                    if (tilesOnBoard[row + 1, col] == null)
                    {
                        possiblePositions.Add(new int[] { row + 1, col });
                    }
                    //LEFT
                    if (tilesOnBoard[row, col - 1] == null)
                    {
                        possiblePositions.Add(new int[] { row, col - 1 });
                    }
                }
            }
        }
        return possiblePositions.Distinct().ToList();
    }
    public void init()
    {
        //RRRR
        addTileToList(terrainTypes.road, terrainTypes.road, terrainTypes.road, terrainTypes.road, 0, 0, RRRR, RRRR_Mask, 3, 0, false, new List<Area>() { 
            new Area { edges = new List<int>() {12,1}, terrain = terrainTypes.grass, colorIndex = 1},
            new Area { edges = new List<int>() {2}, terrain = terrainTypes.road, colorIndex = 2},
            new Area { edges = new List<int>() {3,4} ,terrain = terrainTypes.grass, colorIndex = 3},
            new Area { edges = new List<int>() {5}, terrain = terrainTypes.road, colorIndex = 4},
            new Area { edges = new List<int>() {6,7}, terrain = terrainTypes.grass, colorIndex = 5},
            new Area { edges = new List<int>() {8}, terrain = terrainTypes.road, colorIndex = 6},
            new Area { edges = new List<int>() {9,10}, terrain = terrainTypes.grass, colorIndex = 7},
            new Area { edges = new List<int>() {11}, terrain = terrainTypes.road, colorIndex = 8},
            new Area { edges = new List<int>() {0}, terrain = terrainTypes.intersection, colorIndex = -1}});
        //CCRC
        addTileToList(terrainTypes.castle, terrainTypes.castle, terrainTypes.road, terrainTypes.castle, 0, 0, CCRC, CCRC_Mask, 2, 0, false, new List<Area>() { 
            new Area { edges = new List<int>() {0,1,2,3,4,5,6,10,11,12}, terrain = terrainTypes.castle, colorIndex = 1},
            new Area { edges = new List<int>() {7}, terrain = terrainTypes.grass, colorIndex = 2},
            new Area { edges = new List<int>() {8}, terrain = terrainTypes.road, colorIndex = 3},
            new Area { edges = new List<int>() {9}, terrain = terrainTypes.grass, colorIndex = 4}});
        //CCRC_P
        addTileToList(terrainTypes.castle, terrainTypes.castle, terrainTypes.road, terrainTypes.castle, 0, 0, CCRC_P, CCRC_Mask, 1, 0, true, new List<Area>() {
            new Area { edges = new List<int>() {0,1,2,3,4,5,6,10,11,12}, terrain = terrainTypes.castle, colorIndex = 1},
            new Area { edges = new List<int>() {7}, terrain = terrainTypes.grass, colorIndex = 2},
            new Area { edges = new List<int>() {8}, terrain = terrainTypes.road, colorIndex = 3},
            new Area { edges = new List<int>() {9}, terrain = terrainTypes.grass, colorIndex = 4}});
        //CCFC
        addTileToList(terrainTypes.castle, terrainTypes.castle, terrainTypes.grass, terrainTypes.castle, 0, 0, CCFC, CCFC_Mask, 3, 0, false, new List<Area>() { 
            new Area { edges = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 10, 11, 12 }, terrain = terrainTypes.castle, colorIndex = 1},
            new Area { edges = new List<int>() {7,8,9}, terrain = terrainTypes.grass, colorIndex = 2}});
        //CCFC_P
        addTileToList(terrainTypes.castle, terrainTypes.castle, terrainTypes.grass, terrainTypes.castle, 0, 0, CCFC_P, CCFC_Mask, 1, 0, true, new List<Area>() {
            new Area { edges = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 10, 11, 12 }, terrain = terrainTypes.castle, colorIndex = 1},
            new Area { edges = new List<int>() {7,8,9}, terrain = terrainTypes.grass, colorIndex = 2}});
        //CFFF
        addTileToList(terrainTypes.castle, terrainTypes.grass, terrainTypes.grass, terrainTypes.grass, 0, 0, CFFF, CFFF_Mask, 5, 0, false, new List<Area>() {
            new Area { edges = new List<int>() {1,2,3}, terrain = terrainTypes.castle, colorIndex = 1},
            new Area { edges = new List<int>() {4,5,6,7,8,9,10,11,12}, terrain = terrainTypes.grass, colorIndex = 2}});
        //CRFR
        addTileToList(terrainTypes.castle, terrainTypes.road, terrainTypes.grass, terrainTypes.road, 0, 0, CRFR, CRFR_Mask, 2, 0, false, new List<Area>() { 
            new Area { edges = new List<int>() {1,2,3} ,terrain = terrainTypes.castle, colorIndex = 1},
            new Area { edges = new List<int>() {4,12}, terrain = terrainTypes.grass, colorIndex = 2},
            new Area { edges = new List<int>() {5, 0, 11}, terrain = terrainTypes.road, colorIndex = 3}, 
            new Area { edges = new List<int>() {6,7,8,9,10}, terrain = terrainTypes.grass, colorIndex = 4}});
        //CCCC_P
        addTileToList(terrainTypes.castle, terrainTypes.castle, terrainTypes.castle, terrainTypes.castle, 0, 0, CCCC_P, CCCC_P_Mask, 1, 0, true, new List<Area>() {
            new Area { edges = new List<int>() {0,1,2,3,4,5,6,7,8,9,10,11,12}, terrain = terrainTypes.castle, colorIndex = 1}});
        //FFFF_M
        addTileToList(terrainTypes.grass, terrainTypes.grass, terrainTypes.grass, terrainTypes.grass, 0, 0, FFFF_M, FFFF_M_Mask, 2, 0, false, new List<Area>() {
            new Area { edges = new List<int>() {1,2,3,4,5,6,7,8,9,10,11,12}, terrain = terrainTypes.grass, colorIndex = 1},
            new Area { edges = new List<int>() {0}, terrain = terrainTypes.monastery, colorIndex = 2}});
        //FFRF_M
        addTileToList(terrainTypes.grass, terrainTypes.grass, terrainTypes.road, terrainTypes.grass, 0, 0, FFRF_M, FFRF_M_Mask, 2, 0, false, new List<Area>() {
            new Area { edges = new List<int>() {1,2,3,4,5,6,7,8,9,10,11,12}, terrain = terrainTypes.grass, colorIndex = 1},
            new Area { edges = new List<int>() {8}, terrain = terrainTypes.road, colorIndex = 2 },
            new Area { edges = new List<int>() {0}, terrain = terrainTypes.monastery, colorIndex = 3}});
        /*
        //public Material CRFR; 4 - START
        addTileToList(terrainTypes.castle, terrainTypes.grassRoad, terrainTypes.grass, terrainTypes.grassRoad, 0, 0, CRFR, 3);
        //public Material CFFF; 
        addTileToList(terrainTypes.castle, terrainTypes.grass, terrainTypes.grass, terrainTypes.grass, 0, 0, CFFF);
        //public Material CFFC;
        addTileToList(terrainTypes.castle, terrainTypes.grass, terrainTypes.grass, terrainTypes.castle, 0, 0, CFFC);
        //public Material CFFC_2;
        addTileToList(terrainTypes.castle, terrainTypes.grass, terrainTypes.grass, terrainTypes.castle, 0, 0, CFFC_2);
        //public Material CFFC_P;
        addTileToList(terrainTypes.castle, terrainTypes.grass, terrainTypes.grass, terrainTypes.castle, 0, 0, CFFC_P);
        //public Material CCFC;
        addTileToList(terrainTypes.castle, terrainTypes.castle, terrainTypes.grassRoad, terrainTypes.castle, 0, 0, CCFC);
        //public Material CCFC_P;
        addTileToList(terrainTypes.castle, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, 0, 0, CCFC_P);
        //public Material CCRC;
        addTileToList(terrainTypes.castle, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, 0, 0, CCRC);
        //public Material CCRC_P;
        addTileToList(terrainTypes.castle, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, 0, 0, CCRC_P);
        //public Material CCCC_P;
        addTileToList(terrainTypes.castle, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, 0, 0, CCCC_P);
        //public Material CRRC;
        addTileToList(terrainTypes.castle, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, 0, 0, CRRC);
        //public Material CRRC_P;
        addTileToList(terrainTypes.grass, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, 0, 0, CRRC_P);
        //public Material FCFC;
        addTileToList(terrainTypes.grass, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, 0, 0, FCFC);
        //public Material FCFC_P;
        addTileToList(terrainTypes.castle, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, 0, 0, FCFC_P);
        //public Material CFCF_2;
        addTileToList(terrainTypes.castle, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, 0, 0, CFCF_2);
        //public Material CRRF;
        addTileToList(terrainTypes.castle, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, 0, 0, CRRF);
        //public Material CFRR;
        addTileToList(terrainTypes.castle, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, 0, 0, CFRR);
        //public Material CRRR;
        addTileToList(terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, 0, 0, CRRR);
        //public Material FRFR;
        //public Material FFRR;
        //public Material FRRR;
        //public Material RRRR;
        //public Material FFRF_M;
        //public Material FFFF_M;
        */
    }




}
