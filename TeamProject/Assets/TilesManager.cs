using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System;

public class TilesManager : MonoBehaviour
{
    //TILES:
    public List<Tile> tilesList = new List<Tile>();         //List of all disponible tiles (not on board)
    public List<Tile> distinctTileList = new List<Tile>();  //List of all distinct disponible tiles (not on board)

    //Start tile
    public Material CRFR;
    public Material CFFF;   
    public Material CFFC;
    public Material CFFC_2;
    public Material CFFC_P;
    public Material CCFC;   
    public Material CCFC_P; 
    public Material CCRC;   
    public Material CCRC_P; 
    public Material CCCC_P; 
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
    public Material RRRR;   
    public Material FFRF_M; 
    public Material FFFF_M; 

    //MASKS:
    public Material CRFR_Mask;      
    public Material CFFF_Mask;      
    public Material CFFC_Mask;
    public Material CFFC_2_Mask;
    public Material CCFC_Mask;
    public Material CCRC_Mask;      
    public Material CCCC_Mask;    
    public Material CRRC_Mask;
    public Material FCFC_Mask;
    public Material CFCF_2_Mask;
    public Material CRRF_Mask;
    public Material CFRR_Mask;
    public Material CRRR_Mask;
    public Material FRFR_Mask;
    public Material FFRR_Mask;
    public Material FRRR_Mask;
    public Material RRRR_Mask;      
    public Material FFRF_M_Mask;    
    public Material FFFF_M_Mask;    

    public struct AreaTuple
    {
        public int x;
        public int y;
        public Area area;
        public bool initialized;
    }
    public void addTileToList(int id, terrainTypes up, terrainTypes right, terrainTypes down, terrainTypes left, float x, float y, Material m, Material mask, int count, int turn, bool plus, List<Area> areas)
    {
        Tile tmp = gameObject.AddComponent<Tile>();
        tmp.Init(id, up, right, down, left, x, y, m, mask, turn, plus, areas);
        distinctTileList.Add(tmp);
        for (int i = 0; i < count; i++)
        {
            tilesList.Add(tmp);
            //Debug.Log(i);
        }
    }
    public void placeTile(ref GameObject tile, ref GameObject mask, int x, int y, ref GameObject[,] tilesOnBoard, ref GameObject[,] masks)
    {
        tilesOnBoard[x, y] = Instantiate(tile, new Vector3(0, (float)0.1, 0), Quaternion.identity) as GameObject;
        masks[x, y] = Instantiate(mask, new Vector3(0, (float)0.1, 0), Quaternion.identity) as GameObject;
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
    public Vector3 GetMeeplePosition(int meeplePosIndex, float[] coords)
    {
        Vector3 pos = new Vector3(0, 0, 0);

        switch (meeplePosIndex)
        {
            case 0:
                pos = new Vector3(coords[0], 0, coords[1]);
                break;
            case 1:
                pos = new Vector3(coords[0] - 3.4f, 0, coords[1] + 3.4f);
                break;
            case 2:
                pos = new Vector3(coords[0] - 2.5f, 0, coords[1] + 4);
                break;
            case 3:
                pos = new Vector3(coords[0], 0, coords[1] + 4);
                break;
            case 4:
                pos = new Vector3(coords[0] + 2.5f, 0, coords[1] + 4);
                break;
            case 5:
                pos = new Vector3(coords[0] - 1, 0, coords[1] + 1);
                break;
            case 11:
                pos = new Vector3(coords[0] + 3.4f, 0, coords[1] + 3.4f);
                break;
            case 12:
                pos = new Vector3(coords[0] + 4, 0, coords[1] + 2.5f);
                break;
            case 13:
                pos = new Vector3(coords[0] + 4, 0, coords[1]);
                break;
            case 14:
                pos = new Vector3(coords[0] + 4, 0, coords[1] - 2.5f);
                break;
            case 15:
                pos = new Vector3(coords[0] + 1, 0, coords[1] + 1);
                break;
            case 21:
                pos = new Vector3(coords[0] + 3.4f, 0, coords[1] - 3.4f);
                break;
            case 22:
                pos = new Vector3(coords[0] + 2.5f, 0, coords[1] - 4);
                break;
            case 23:
                pos = new Vector3(coords[0], 0, coords[1] - 4);
                break;
            case 24:
                pos = new Vector3(coords[0] + 2.5f, 0, coords[1] - 4);
                break;
            case 25:
                pos = new Vector3(coords[0] + 1, 0, coords[1] - 1);
                break;
            case 31:
                pos = new Vector3(coords[0] - 3.4f, 0, coords[1] - 3.4f);
                break;
            case 32:
                pos = new Vector3(coords[0] - 4, 0, coords[1] - 2.5f);
                break;
            case 33:
                pos = new Vector3(coords[0] - 4, 0, coords[1]);
                break;
            case 34:
                pos = new Vector3(coords[0] - 4, 0, coords[1] + 2.5f);
                break;
            case 35:
                pos = new Vector3(coords[0] - 1, 0, coords[1] - 1);
                break;
        }
        return pos;
    }
    public int ColorClassify(float R)
    {
        int x = 0;
        if (R <= 0.066f)
        {
            return 1;
        }
        if (R <= 0.133f)
        {
            return 2;
        }
        if (R <= 0.200f)
        {
            return 3;
        }
        if (R <= 0.266f)
        {
            return 4;
        }
        if (R <= 0.333f)
        {
            return 5;
        }
        if (R <= 0.400f)
        {
            return 6;
        }
        if (R <= 0.466f)
        {
            return 7;
        }
        if (R <= 0.533f)
        {
            return 8;
        }
        return x;
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
    public List<Area> possibleMeepleAreas(ref TileAI[,] board, int x, int y)
    {
        List<Area> possibleAreas = new List<Area>();
        //Check which area can hold meeple:
        //For each area on the tile
        List<AreaTuple> list = new List<AreaTuple>();
        foreach (var area in board[x, y].Areas)
        {
            AreaTuple temp;
            temp.x = x;
            temp.y = y;
            temp.area = area;
            temp.initialized = true;
            list.Add(temp);
        }

        foreach (var area in board[x, y].Areas)
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
    public AreaTuple areaNeighbour(ref TileAI[,] board, int x, int y, int edge)
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
        if ((object)board[neighbour.x, neighbour.y] != null)
        {
            foreach (var area in board[neighbour.x, neighbour.y].Areas)
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
        return neighbours;
    }
    public List<AreaTuple> areaNeighbours(ref TileAI[,] board, int x, int y, List<int> edges)
    {
        List<AreaTuple> neighbours = new List<AreaTuple>();
        foreach (var edge in edges)
        {
            AreaTuple neighbour = areaNeighbour(ref board, x, y, edge);
            if (neighbour.initialized)
            {
                neighbours.Add(neighbour);
                //Debug.Log("initialized");
            }
                
        }
        //foreach (var n in neighbours)
        //{
        //    Debug.Log("Klocek sąsiad to : " + n.x + " " + n.y);
        //    Debug.Log("Obszar klokcka sąsiada to : " + String.Join(" ", n.area.edges.Select(item => item.ToString()).ToArray()));
        //}
        //Debug.Log("---");
        //Debug.Log("wielkość listy:" + neighbours.Count);
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
    public bool isMovePossible(ref TileAI[,] board, int x, int y, Area currentlyChecked, List<AreaTuple> checkedAreas)
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
        
        for (int j = 0; j < gameObject.GetComponent<Tile>().Areas.Count; j++)
        {
            if (gameObject.GetComponent<Tile>().Areas[j].meeplePlacementIndex == 0)
            {
                //skip
            }
            
            else if(gameObject.GetComponent<Tile>().Areas[j].meeplePlacementIndex <=30)
            {
                gameObject.GetComponent<Tile>().Areas[j].meeplePlacementIndex += 10;
            }
            else
            {
                gameObject.GetComponent<Tile>().Areas[j].meeplePlacementIndex -= 30;
            }
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

        if (gameObject.GetComponent<Tile>().Rotation == 3)
        {
            gameObject.GetComponent<Tile>().Rotation = 0;
        }
        else
        {
            gameObject.GetComponent<Tile>().Rotation++;
        }
    }
    public void rotateClockwise90(ref TileAI tile)          
    {
        terrainTypes up = tile.UpTerrain;
        terrainTypes right = tile.RightTerrain;
        terrainTypes down = tile.DownTerrain;
        terrainTypes left = tile.LeftTerrain;

        for (int j = 0; j < tile.Areas.Count; j++)
        {
            if (tile.Areas[j].meeplePlacementIndex == 0)
            {
                //skip
            }
            else if (tile.Areas[j].meeplePlacementIndex <= 30)
            {
                tile.Areas[j].meeplePlacementIndex += 10;
            }
            else
            {
                tile.Areas[j].meeplePlacementIndex -= 30;
            }
            for (int i = 0; i < tile.Areas[j].edges.Count; i++)
            {
                if (tile.Areas[j].edges[i] == 0)
                {
                    //skip
                }
                else if (tile.Areas[j].edges[i] < 10)
                {
                    tile.Areas[j].edges[i] += 3;
                }
                else
                {
                    tile.Areas[j].edges[i] = tile.Areas[j].edges[i] + 3 - 12;
                }
            }
        }

        tile.UpTerrain = left;
        tile.RightTerrain = up;
        tile.DownTerrain = right;
        tile.LeftTerrain = down;

        if (tile.Rotation == 3)
        {
            tile.Rotation = 0;
        }
        else
        {
            tile.Rotation++;
        }
    }
    public List<int> possibleRotations(TileAI tile, TileAI[,] tiles, int[] tilePosition)
    {
        List<int> possibleRotations = new List<int>();
        if (tile.UpTerrain == tile.DownTerrain && tile.LeftTerrain == tile.RightTerrain && tile.RightTerrain == tile.UpTerrain)
        {
            possibleRotations.Add(0);
        }
        else if (tile.UpTerrain == tile.DownTerrain && tile.LeftTerrain == tile.RightTerrain)
        {
            if (((object)tiles[tilePosition[0] - 1, tilePosition[1]] == null || tiles[tilePosition[0] - 1, tilePosition[1]].DownTerrain == tile.UpTerrain)
               && ((object)tiles[tilePosition[0], tilePosition[1] + 1] == null || tiles[tilePosition[0], tilePosition[1] + 1].LeftTerrain == tile.RightTerrain)
               && ((object)tiles[tilePosition[0] + 1, tilePosition[1]] == null || tiles[tilePosition[0] + 1, tilePosition[1]].UpTerrain == tile.DownTerrain)
               && ((object)tiles[tilePosition[0], tilePosition[1] - 1] == null || tiles[tilePosition[0], tilePosition[1] - 1].RightTerrain == tile.LeftTerrain))
            {
                possibleRotations.Add(0);
            }
            if (((object)tiles[tilePosition[0] - 1, tilePosition[1]] == null || tiles[tilePosition[0] - 1, tilePosition[1]].DownTerrain == tile.LeftTerrain)
               && ((object)tiles[tilePosition[0], tilePosition[1] + 1] == null || tiles[tilePosition[0], tilePosition[1] + 1].LeftTerrain == tile.UpTerrain)
               && ((object)tiles[tilePosition[0] + 1, tilePosition[1]] == null || tiles[tilePosition[0] + 1, tilePosition[1]].UpTerrain == tile.RightTerrain)
               && ((object)tiles[tilePosition[0], tilePosition[1] - 1] == null || tiles[tilePosition[0], tilePosition[1] - 1].RightTerrain == tile.DownTerrain))
            {
                possibleRotations.Add(1);
            }
        }
        else
        {
            if (((object)tiles[tilePosition[0] - 1, tilePosition[1]] == null || tiles[tilePosition[0] - 1, tilePosition[1]].DownTerrain == tile.UpTerrain)
               && ((object)tiles[tilePosition[0], tilePosition[1] + 1] == null || tiles[tilePosition[0], tilePosition[1] + 1].LeftTerrain == tile.RightTerrain)
               && ((object)tiles[tilePosition[0] + 1, tilePosition[1]] == null || tiles[tilePosition[0] + 1, tilePosition[1]].UpTerrain == tile.DownTerrain)
               && ((object)tiles[tilePosition[0], tilePosition[1] - 1] == null || tiles[tilePosition[0], tilePosition[1] - 1].RightTerrain == tile.LeftTerrain))
            {
                possibleRotations.Add(0);
            }
            if (((object)tiles[tilePosition[0] - 1, tilePosition[1]] == null || tiles[tilePosition[0] - 1, tilePosition[1]].DownTerrain == tile.LeftTerrain)
               && ((object)tiles[tilePosition[0], tilePosition[1] + 1] == null || tiles[tilePosition[0], tilePosition[1] + 1].LeftTerrain == tile.UpTerrain)
               && ((object)tiles[tilePosition[0] + 1, tilePosition[1]] == null || tiles[tilePosition[0] + 1, tilePosition[1]].UpTerrain == tile.RightTerrain)
               && ((object)tiles[tilePosition[0], tilePosition[1] - 1] == null || tiles[tilePosition[0], tilePosition[1] - 1].RightTerrain == tile.DownTerrain))
            {
                possibleRotations.Add(1);
            }
            if (((object)tiles[tilePosition[0] - 1, tilePosition[1]] == null || tiles[tilePosition[0] - 1, tilePosition[1]].DownTerrain == tile.DownTerrain)
                && ((object)tiles[tilePosition[0], tilePosition[1] + 1] == null || tiles[tilePosition[0], tilePosition[1] + 1].LeftTerrain == tile.LeftTerrain)
                && ((object)tiles[tilePosition[0] + 1, tilePosition[1]] == null || tiles[tilePosition[0] + 1, tilePosition[1]].UpTerrain == tile.UpTerrain)
                && ((object)tiles[tilePosition[0], tilePosition[1] - 1] == null || tiles[tilePosition[0], tilePosition[1] - 1].RightTerrain == tile.RightTerrain))
            {
                possibleRotations.Add(2);
            }
            if (((object)tiles[tilePosition[0] - 1, tilePosition[1]] == null || tiles[tilePosition[0] - 1, tilePosition[1]].DownTerrain == tile.RightTerrain)
                && ((object)tiles[tilePosition[0], tilePosition[1] + 1] == null || tiles[tilePosition[0], tilePosition[1] + 1].LeftTerrain == tile.DownTerrain)
                && ((object)tiles[tilePosition[0] + 1, tilePosition[1]] == null || tiles[tilePosition[0] + 1, tilePosition[1]].UpTerrain == tile.LeftTerrain)
                && ((object)tiles[tilePosition[0], tilePosition[1] - 1] == null || tiles[tilePosition[0], tilePosition[1] - 1].RightTerrain == tile.UpTerrain))
            {
                possibleRotations.Add(3);
            }
        }
        return possibleRotations;
    }
    public void rotateXTimes(int times, ref GameObject gameObject, ref GameObject mask)
    {
        if (times == 0)
        {
            //do nothing
        }
        else if (times == 1)
        {
            gameObject.transform.Rotate(new Vector3(0, 90, 0));
            mask.transform.Rotate(new Vector3(0, 90, 0));
        }
        else if (times == 2)
        {
            gameObject.transform.Rotate(new Vector3(0, 90, 0));
            mask.transform.Rotate(new Vector3(0, 90, 0));
            gameObject.transform.Rotate(new Vector3(0, 90, 0));
            mask.transform.Rotate(new Vector3(0, 90, 0));
        }
        else if (times == 3)
        {
            gameObject.transform.Rotate(new Vector3(0, 90, 0));
            mask.transform.Rotate(new Vector3(0, 90, 0));
            gameObject.transform.Rotate(new Vector3(0, 90, 0));
            mask.transform.Rotate(new Vector3(0, 90, 0));
            gameObject.transform.Rotate(new Vector3(0, 90, 0));
            mask.transform.Rotate(new Vector3(0, 90, 0));
        }
    }
    public void rotateFirstMatchingRotation(ref GameObject gameObject, ref GameObject mask, int[] gameObjectPosition, ref GameObject[,] tilesOnBoard)
    {
        //if ((tilesOnBoard[gameObjectPosition[0] - 1, gameObjectPosition[1]] == null || tilesOnBoard[gameObjectPosition[0] - 1, gameObjectPosition[1]].GetComponent<Tile>().DownTerrain == gameObject.GetComponent<Tile>().UpTerrain)
        //   && (tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] + 1] == null || tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] + 1].GetComponent<Tile>().LeftTerrain == gameObject.GetComponent<Tile>().RightTerrain)
        //   && (tilesOnBoard[gameObjectPosition[0] + 1, gameObjectPosition[1]] == null || tilesOnBoard[gameObjectPosition[0] + 1, gameObjectPosition[1]].GetComponent<Tile>().UpTerrain == gameObject.GetComponent<Tile>().DownTerrain)
        //   && (tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] - 1] == null || tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] - 1].GetComponent<Tile>().RightTerrain == gameObject.GetComponent<Tile>().LeftTerrain))
        //{
        //    // Do nothing
        //}
        //else 
        if ((tilesOnBoard[gameObjectPosition[0] - 1, gameObjectPosition[1]] == null || tilesOnBoard[gameObjectPosition[0] - 1, gameObjectPosition[1]].GetComponent<Tile>().DownTerrain == gameObject.GetComponent<Tile>().LeftTerrain)
            && (tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] + 1] == null || tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] + 1].GetComponent<Tile>().LeftTerrain == gameObject.GetComponent<Tile>().UpTerrain)
            && (tilesOnBoard[gameObjectPosition[0] + 1, gameObjectPosition[1]] == null || tilesOnBoard[gameObjectPosition[0] + 1, gameObjectPosition[1]].GetComponent<Tile>().UpTerrain == gameObject.GetComponent<Tile>().RightTerrain)
            && (tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] - 1] == null || tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] - 1].GetComponent<Tile>().RightTerrain == gameObject.GetComponent<Tile>().DownTerrain))
        {
            rotateClockwise90(ref gameObject, ref mask);
        }
        else if ((tilesOnBoard[gameObjectPosition[0] - 1, gameObjectPosition[1]] == null || tilesOnBoard[gameObjectPosition[0] - 1, gameObjectPosition[1]].GetComponent<Tile>().DownTerrain == gameObject.GetComponent<Tile>().DownTerrain)
            && (tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] + 1] == null || tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] + 1].GetComponent<Tile>().LeftTerrain == gameObject.GetComponent<Tile>().LeftTerrain)
            && (tilesOnBoard[gameObjectPosition[0] + 1, gameObjectPosition[1]] == null || tilesOnBoard[gameObjectPosition[0] + 1, gameObjectPosition[1]].GetComponent<Tile>().UpTerrain == gameObject.GetComponent<Tile>().UpTerrain)
            && (tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] - 1] == null || tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] - 1].GetComponent<Tile>().RightTerrain == gameObject.GetComponent<Tile>().RightTerrain))
        {
            rotateClockwise90(ref gameObject, ref mask);
            rotateClockwise90(ref gameObject, ref mask);
        }
        else if ((tilesOnBoard[gameObjectPosition[0] - 1, gameObjectPosition[1]] == null || tilesOnBoard[gameObjectPosition[0] - 1, gameObjectPosition[1]].GetComponent<Tile>().DownTerrain == gameObject.GetComponent<Tile>().RightTerrain)
            && (tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] + 1] == null || tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] + 1].GetComponent<Tile>().LeftTerrain == gameObject.GetComponent<Tile>().DownTerrain)
            && (tilesOnBoard[gameObjectPosition[0] + 1, gameObjectPosition[1]] == null || tilesOnBoard[gameObjectPosition[0] + 1, gameObjectPosition[1]].GetComponent<Tile>().UpTerrain == gameObject.GetComponent<Tile>().LeftTerrain)
            && (tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] - 1] == null || tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] - 1].GetComponent<Tile>().RightTerrain == gameObject.GetComponent<Tile>().UpTerrain))
        {
            rotateClockwise90(ref gameObject, ref mask);
            rotateClockwise90(ref gameObject, ref mask);
            rotateClockwise90(ref gameObject, ref mask);
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
    public List<int[]> findMatchingEdges(List<int[]> movesList, TileAI choosenTile, ref TileAI[,] tilesOnBoard)
    {
        List<int[]> matchingEdges = new List<int[]>();
        foreach (var position in movesList)
        {
            //UP RIGHT DOWN LEFT 0
            if (((object)tilesOnBoard[position[0] - 1, position[1]] == null || tilesOnBoard[position[0] - 1, position[1]].DownTerrain == choosenTile.UpTerrain)
                && ((object)tilesOnBoard[position[0], position[1] + 1] == null || tilesOnBoard[position[0], position[1] + 1].LeftTerrain == choosenTile.RightTerrain)
                && ((object)tilesOnBoard[position[0] + 1, position[1]] == null || tilesOnBoard[position[0] + 1, position[1]].UpTerrain == choosenTile.DownTerrain)
                && ((object)tilesOnBoard[position[0], position[1] - 1] == null || tilesOnBoard[position[0], position[1] - 1].RightTerrain == choosenTile.LeftTerrain))
            {
                matchingEdges.Add(position);
            }
            // UP RIGHT DOWN LEFT 90
            else if (((object)tilesOnBoard[position[0] - 1, position[1]] == null || tilesOnBoard[position[0] - 1, position[1]].DownTerrain == choosenTile.LeftTerrain)
                && ((object)tilesOnBoard[position[0], position[1] + 1] == null || tilesOnBoard[position[0], position[1] + 1].LeftTerrain == choosenTile.UpTerrain)
                && ((object)tilesOnBoard[position[0] + 1, position[1]] == null || tilesOnBoard[position[0] + 1, position[1]].UpTerrain == choosenTile.RightTerrain)
                && ((object)tilesOnBoard[position[0], position[1] - 1] == null || tilesOnBoard[position[0], position[1] - 1].RightTerrain == choosenTile.DownTerrain))
            {
                matchingEdges.Add(position);
            }
            // UP RIGHT DOWN LEFT 180
            else if (((object)tilesOnBoard[position[0] - 1, position[1]] == null || tilesOnBoard[position[0] - 1, position[1]].DownTerrain == choosenTile.DownTerrain)
                && ((object)tilesOnBoard[position[0], position[1] + 1] == null || tilesOnBoard[position[0], position[1] + 1].LeftTerrain == choosenTile.LeftTerrain)
                && ((object)tilesOnBoard[position[0] + 1, position[1]] == null || tilesOnBoard[position[0] + 1, position[1]].UpTerrain == choosenTile.UpTerrain)
                && ((object)tilesOnBoard[position[0], position[1] - 1] == null || tilesOnBoard[position[0], position[1] - 1].RightTerrain == choosenTile.RightTerrain))
            {
                matchingEdges.Add(position);
            }
            // UP RIGHT DOWN LEFT 270
            else if (((object)tilesOnBoard[position[0] - 1, position[1]] == null || tilesOnBoard[position[0] - 1, position[1]].DownTerrain == choosenTile.RightTerrain)
                && ((object)tilesOnBoard[position[0], position[1] + 1] == null || tilesOnBoard[position[0], position[1] + 1].LeftTerrain == choosenTile.DownTerrain)
                && ((object)tilesOnBoard[position[0] + 1, position[1]] == null || tilesOnBoard[position[0] + 1, position[1]].UpTerrain == choosenTile.LeftTerrain)
                && ((object)tilesOnBoard[position[0], position[1] - 1] == null || tilesOnBoard[position[0], position[1] - 1].RightTerrain == choosenTile.UpTerrain))
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
    public List<int[]> findSourrounding(ref TileAI[,] tilesOnBoard)
    {
        List<int[]> possiblePositions = new List<int[]>();
        for (int row = 0; row < tilesOnBoard.GetLength(0); row++)
        {
            for (int col = 0; col < tilesOnBoard.GetLength(1); col++)
            {
                if ((object)tilesOnBoard[row, col] != null)
                {
                    //UP
                    if ((object)tilesOnBoard[row - 1, col] == null)
                    {
                        possiblePositions.Add(new int[] { row - 1, col });
                    }
                    //RIGHT
                    if ((object)tilesOnBoard[row, col + 1] == null)
                    {
                        possiblePositions.Add(new int[] { row, col + 1 });
                    }
                    //DOWN
                    if ((object)tilesOnBoard[row + 1, col] == null)
                    {
                        possiblePositions.Add(new int[] { row + 1, col });
                    }
                    //LEFT
                    if ((object)tilesOnBoard[row, col - 1] == null)
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
        //CRFR
        addTileToList(1, terrainTypes.castle, terrainTypes.road, terrainTypes.grass, terrainTypes.road, 0, 0, CRFR, CRFR_Mask, 1, 0, false, new List<Area>() {
            new Area { edges = new List<int>() {1,2,3} ,terrain = terrainTypes.castle, colorIndex = 1, meeplePlacementIndex = 3},
            new Area { edges = new List<int>() {4,12}, terrain = terrainTypes.grass, colorIndex = 2, meeplePlacementIndex = 5},
            new Area { edges = new List<int>() {5, 11}, terrain = terrainTypes.road, colorIndex = 3, meeplePlacementIndex = 0},
            new Area { edges = new List<int>() {6,7,8,9,10}, terrain = terrainTypes.grass, colorIndex = 4, meeplePlacementIndex = 23}});
        //CFFC
        addTileToList(3, terrainTypes.castle, terrainTypes.grass, terrainTypes.grass, terrainTypes.castle, 0, 0, CFFC, CFFC_Mask, 1, 0, false, new List<Area>() {
            new Area { edges = new List<int>() {1,2,3,10,11,12}, terrain = terrainTypes.castle, colorIndex = 1, meeplePlacementIndex =1},
            new Area { edges = new List<int>() {0,4,5,6,7,8,9}, terrain = terrainTypes.grass, colorIndex = 2, meeplePlacementIndex = 25}});
        //CFFC_2
        addTileToList(4, terrainTypes.castle, terrainTypes.grass, terrainTypes.grass, terrainTypes.castle, 0, 0, CFFC_2, CFFC_2_Mask, 1, 0, false, new List<Area>() {
            new Area { edges = new List<int>() {1,2,3}, terrain = terrainTypes.castle, colorIndex = 1, meeplePlacementIndex = 3},
            new Area { edges = new List<int>() {0,4,5,6,7,8,9}, terrain = terrainTypes.grass, colorIndex = 2, meeplePlacementIndex = 25},
            new Area { edges = new List<int>() {10,11,12}, terrain = terrainTypes.castle, colorIndex = 3, meeplePlacementIndex = 33}});
        /*
        //CCRC
        addTileToList(8, terrainTypes.castle, terrainTypes.castle, terrainTypes.road, terrainTypes.castle, 0, 0, CCRC, CCRC_Mask, 1, 0, false, new List<Area>() {
            new Area { edges = new List<int>() {0,1,2,3,4,5,6,10,11,12}, terrain = terrainTypes.castle, colorIndex = 1, meeplePlacementIndex = 3},
            new Area { edges = new List<int>() {7}, terrain = terrainTypes.grass, colorIndex = 2, meeplePlacementIndex = 22},
            new Area { edges = new List<int>() {8}, terrain = terrainTypes.road, colorIndex = 3, meeplePlacementIndex = 23},
            new Area { edges = new List<int>() {9}, terrain = terrainTypes.grass, colorIndex = 4, meeplePlacementIndex = 24}});
        //CCFC_P
        addTileToList(7, terrainTypes.castle, terrainTypes.castle, terrainTypes.grass, terrainTypes.castle, 0, 0, CCFC_P, CCFC_Mask, 1, 0, true, new List<Area>() {
            new Area { edges = new List<int>() {0,1,2,3,4,5,6,10,11,12}, terrain = terrainTypes.castle, colorIndex = 1, meeplePlacementIndex = 3},
            new Area { edges = new List<int>() {7,8,9}, terrain = terrainTypes.grass, colorIndex = 2, meeplePlacementIndex = 23}});
        //CCRC*/
        /*
        addTileToList(8, terrainTypes.castle, terrainTypes.castle, terrainTypes.road, terrainTypes.castle, 0, 0, CCRC, CCRC_Mask, 1, 0, false, new List<Area>() {
            new Area { edges = new List<int>() {0,1,2,3,4,5,6,10,11,12}, terrain = terrainTypes.castle, colorIndex = 1, meeplePlacementIndex = 3},
            new Area { edges = new List<int>() {7}, terrain = terrainTypes.grass, colorIndex = 2, meeplePlacementIndex = 22},
            new Area { edges = new List<int>() {8}, terrain = terrainTypes.road, colorIndex = 3, meeplePlacementIndex = 23},
            new Area { edges = new List<int>() {9}, terrain = terrainTypes.grass, colorIndex = 4, meeplePlacementIndex = 24}});
        //CCRC_P
        addTileToList(9, terrainTypes.castle, terrainTypes.castle, terrainTypes.road, terrainTypes.castle, 0, 0, CCRC_P, CCRC_Mask, 1, 0, true, new List<Area>() {
            new Area { edges = new List<int>() {0,1,2,3,4,5,6,10,11,12}, terrain = terrainTypes.castle, colorIndex = 1, meeplePlacementIndex = 3},
            new Area { edges = new List<int>() {7}, terrain = terrainTypes.grass, colorIndex = 2, meeplePlacementIndex = 22},
            new Area { edges = new List<int>() {8}, terrain = terrainTypes.road, colorIndex = 3, meeplePlacementIndex = 23},
            new Area { edges = new List<int>() {9}, terrain = terrainTypes.grass, colorIndex = 4, meeplePlacementIndex = 24}});
        //CCCC_P
        addTileToList(10, terrainTypes.castle, terrainTypes.castle, terrainTypes.castle, terrainTypes.castle, 0, 0, CCCC_P, CCCC_Mask, 1, 0, true, new List<Area>() {
            new Area { edges = new List<int>() {0,1,2,3,4,5,6,7,8,9,10,11,12}, terrain = terrainTypes.castle, colorIndex = 1, meeplePlacementIndex = 0}});
        //CRRC
        addTileToList(11, terrainTypes.castle, terrainTypes.road, terrainTypes.road, terrainTypes.castle, 0, 0, CRRC, CRRC_Mask, 1, 0, false, new List<Area>() {
            new Area { edges = new List<int>() {1,2,3,10,11,12}, terrain = terrainTypes.castle, colorIndex = 1, meeplePlacementIndex = 1},
            new Area { edges = new List<int>() {0,4,9}, terrain = terrainTypes.grass, colorIndex = 2, meeplePlacementIndex = 0},
            new Area { edges = new List<int>() {5,8}, terrain = terrainTypes.road, colorIndex = 3, meeplePlacementIndex = 25},
            new Area { edges = new List<int>() {7,6}, terrain = terrainTypes.grass, colorIndex = 4, meeplePlacementIndex = 21}});
*/

        /*
        //CRFR
        addTileToList(1, terrainTypes.castle, terrainTypes.road, terrainTypes.grass, terrainTypes.road, 0, 0, CRFR, CRFR_Mask, 3, 0, false, new List<Area>() {
            new Area { edges = new List<int>() {1,2,3} ,terrain = terrainTypes.castle, colorIndex = 1, meeplePlacementIndex = 3},
            new Area { edges = new List<int>() {4,12}, terrain = terrainTypes.grass, colorIndex = 2, meeplePlacementIndex = 5},
            new Area { edges = new List<int>() {5, 11}, terrain = terrainTypes.road, colorIndex = 3, meeplePlacementIndex = 0},
            new Area { edges = new List<int>() {6,7,8,9,10}, terrain = terrainTypes.grass, colorIndex = 4, meeplePlacementIndex = 23}});
        //CFFF
        addTileToList(2, terrainTypes.castle, terrainTypes.grass, terrainTypes.grass, terrainTypes.grass, 0, 0, CFFF, CFFF_Mask, 5, 0, false, new List<Area>() {
            new Area { edges = new List<int>() {1,2,3}, terrain = terrainTypes.castle, colorIndex = 1, meeplePlacementIndex = 3},
            new Area { edges = new List<int>() {0,4,5,6,7,8,9,10,11,12}, terrain = terrainTypes.grass, colorIndex = 2, meeplePlacementIndex = 0}});
        //CFFC
        addTileToList(3, terrainTypes.castle, terrainTypes.grass, terrainTypes.grass, terrainTypes.castle, 0, 0, CFFC, CFFC_Mask, 3, 0, false, new List<Area>() {
            new Area { edges = new List<int>() {1,2,3,10,11,12}, terrain = terrainTypes.castle, colorIndex = 1, meeplePlacementIndex =1},
            new Area { edges = new List<int>() {0,4,5,6,7,8,9}, terrain = terrainTypes.grass, colorIndex = 2, meeplePlacementIndex = 25}});
        //CFFC_2
        addTileToList(4, terrainTypes.castle, terrainTypes.grass, terrainTypes.grass, terrainTypes.castle, 0, 0, CFFC_2, CFFC_2_Mask, 2, 0, false, new List<Area>() {
            new Area { edges = new List<int>() {1,2,3}, terrain = terrainTypes.castle, colorIndex = 1, meeplePlacementIndex = 3},
            new Area { edges = new List<int>() {0,4,5,6,7,8,9}, terrain = terrainTypes.grass, colorIndex = 2, meeplePlacementIndex = 25},
            new Area { edges = new List<int>() {10,11,12}, terrain = terrainTypes.castle, colorIndex = 3, meeplePlacementIndex = 33}});
        //CFFC_P
        addTileToList(5, terrainTypes.castle, terrainTypes.grass, terrainTypes.grass, terrainTypes.castle, 0, 0, CFFC_P, CFFC_Mask, 2, 0, true, new List<Area>() {
            new Area { edges = new List<int>() {1,2,3,10,11,12}, terrain = terrainTypes.castle, colorIndex = 1, meeplePlacementIndex =1},
            new Area { edges = new List<int>() {0,4,5,6,7,8,9}, terrain = terrainTypes.grass, colorIndex = 2, meeplePlacementIndex = 25}});
        //CCFC
        addTileToList(6, terrainTypes.castle, terrainTypes.castle, terrainTypes.grass, terrainTypes.castle, 0, 0, CCFC, CCFC_Mask, 3, 0, false, new List<Area>() {
            new Area { edges = new List<int>() {0,1,2,3,4,5,6,10,11,12}, terrain = terrainTypes.castle, colorIndex = 1, meeplePlacementIndex = 3},
            new Area { edges = new List<int>() {7,8,9}, terrain = terrainTypes.grass, colorIndex = 2, meeplePlacementIndex = 23}});
        //CCFC_P
        addTileToList(7, terrainTypes.castle, terrainTypes.castle, terrainTypes.grass, terrainTypes.castle, 0, 0, CCFC_P, CCFC_Mask, 1, 0, true, new List<Area>() {
            new Area { edges = new List<int>() {0,1,2,3,4,5,6,10,11,12}, terrain = terrainTypes.castle, colorIndex = 1, meeplePlacementIndex = 3},
            new Area { edges = new List<int>() {7,8,9}, terrain = terrainTypes.grass, colorIndex = 2, meeplePlacementIndex = 23}});
        //CCRC
        addTileToList(8, terrainTypes.castle, terrainTypes.castle, terrainTypes.road, terrainTypes.castle, 0, 0, CCRC, CCRC_Mask, 1, 0, false, new List<Area>() {
            new Area { edges = new List<int>() {0,1,2,3,4,5,6,10,11,12}, terrain = terrainTypes.castle, colorIndex = 1, meeplePlacementIndex = 3},
            new Area { edges = new List<int>() {7}, terrain = terrainTypes.grass, colorIndex = 2, meeplePlacementIndex = 22},
            new Area { edges = new List<int>() {8}, terrain = terrainTypes.road, colorIndex = 3, meeplePlacementIndex = 23},
            new Area { edges = new List<int>() {9}, terrain = terrainTypes.grass, colorIndex = 4, meeplePlacementIndex = 24}});
        //CCRC_P
        addTileToList(9, terrainTypes.castle, terrainTypes.castle, terrainTypes.road, terrainTypes.castle, 0, 0, CCRC_P, CCRC_Mask, 2, 0, true, new List<Area>() {
            new Area { edges = new List<int>() {0,1,2,3,4,5,6,10,11,12}, terrain = terrainTypes.castle, colorIndex = 1, meeplePlacementIndex = 3},
            new Area { edges = new List<int>() {7}, terrain = terrainTypes.grass, colorIndex = 2, meeplePlacementIndex = 22},
            new Area { edges = new List<int>() {8}, terrain = terrainTypes.road, colorIndex = 3, meeplePlacementIndex = 23},
            new Area { edges = new List<int>() {9}, terrain = terrainTypes.grass, colorIndex = 4, meeplePlacementIndex = 24}});
        //CCCC_P
        addTileToList(10, terrainTypes.castle, terrainTypes.castle, terrainTypes.castle, terrainTypes.castle, 0, 0, CCCC_P, CCCC_Mask, 1, 0, true, new List<Area>() {
            new Area { edges = new List<int>() {0,1,2,3,4,5,6,7,8,9,10,11,12}, terrain = terrainTypes.castle, colorIndex = 1, meeplePlacementIndex = 0}});
        //CRRC
        addTileToList(11, terrainTypes.castle, terrainTypes.road, terrainTypes.road, terrainTypes.castle, 0, 0, CRRC, CRRC_Mask, 3, 0, false, new List<Area>() {
            new Area { edges = new List<int>() {1,2,3,10,11,12}, terrain = terrainTypes.castle, colorIndex = 1, meeplePlacementIndex = 1},
            new Area { edges = new List<int>() {0,4,9}, terrain = terrainTypes.grass, colorIndex = 2, meeplePlacementIndex = 0},
            new Area { edges = new List<int>() {5,8}, terrain = terrainTypes.road, colorIndex = 3, meeplePlacementIndex = 25},
            new Area { edges = new List<int>() {7,6}, terrain = terrainTypes.grass, colorIndex = 4, meeplePlacementIndex = 21}});
        //CRRC_P
        addTileToList(12, terrainTypes.castle, terrainTypes.road, terrainTypes.road, terrainTypes.castle, 0, 0, CRRC_P, CRRC_Mask, 2, 0, true, new List<Area>() {
            new Area { edges = new List<int>() {1,2,3,10,11,12}, terrain = terrainTypes.castle, colorIndex = 1, meeplePlacementIndex = 1},
            new Area { edges = new List<int>() {0,4,9}, terrain = terrainTypes.grass, colorIndex = 2, meeplePlacementIndex = 0},
            new Area { edges = new List<int>() {5,8}, terrain = terrainTypes.road, colorIndex = 3, meeplePlacementIndex = 25},
            new Area { edges = new List<int>() {7,6}, terrain = terrainTypes.grass, colorIndex = 4, meeplePlacementIndex = 21}});
        //FCFC
        addTileToList(13, terrainTypes.grass, terrainTypes.castle, terrainTypes.grass, terrainTypes.castle, 0, 0, FCFC, FCFC_Mask, 1, 0, false, new List<Area>() {
            new Area { edges = new List<int>() {1,2,3}, terrain = terrainTypes.grass, colorIndex = 1, meeplePlacementIndex = 3},
            new Area { edges = new List<int>() {4,5,6,0,10,11,12}, terrain = terrainTypes.castle, colorIndex = 2, meeplePlacementIndex = 0},
            new Area { edges = new List<int>() {7,8,9}, terrain = terrainTypes.grass, colorIndex = 3, meeplePlacementIndex = 23}});
        //FCFC_P
        addTileToList(14, terrainTypes.grass, terrainTypes.castle, terrainTypes.grass, terrainTypes.castle, 0, 0, FCFC_P, FCFC_Mask, 2, 0, true, new List<Area>() {
            new Area { edges = new List<int>() {1,2,3}, terrain = terrainTypes.grass, colorIndex = 1, meeplePlacementIndex = 3},
            new Area { edges = new List<int>() {4,5,6,0,10,11,12}, terrain = terrainTypes.castle, colorIndex = 2, meeplePlacementIndex = 0},
            new Area { edges = new List<int>() {7,8,9}, terrain = terrainTypes.grass, colorIndex = 3, meeplePlacementIndex = 23}});
        //CFCF_2
        addTileToList(15, terrainTypes.castle, terrainTypes.grass, terrainTypes.castle, terrainTypes.grass, 0, 0, CFCF_2, CFCF_2_Mask, 3, 0, false, new List<Area>() {
            new Area { edges = new List<int>() {1,2,3}, terrain = terrainTypes.castle, colorIndex = 1, meeplePlacementIndex = 3},
            new Area { edges = new List<int>() {4,5,6,0,10,11,12}, terrain = terrainTypes.grass, colorIndex = 2, meeplePlacementIndex = 0},
            new Area { edges = new List<int>() {7,8,9}, terrain = terrainTypes.castle, colorIndex = 3, meeplePlacementIndex = 23}});
        //CRRF
        addTileToList(16, terrainTypes.castle, terrainTypes.road, terrainTypes.road, terrainTypes.grass, 0, 0, CRRF, CRRF_Mask, 3, 0, false, new List<Area>() {
            new Area { edges = new List<int>() {1,2,3}, terrain = terrainTypes.castle, colorIndex = 1, meeplePlacementIndex = 3},
            new Area { edges = new List<int>() {0,4,9,10,11,12}, terrain = terrainTypes.grass, colorIndex = 2, meeplePlacementIndex = 35},
            new Area { edges = new List<int>() {5,8}, terrain = terrainTypes.road, colorIndex = 3, meeplePlacementIndex = 25},
            new Area { edges = new List<int>() {7,6}, terrain = terrainTypes.grass, colorIndex = 4, meeplePlacementIndex =  21}});
        //CFRR
        addTileToList(17, terrainTypes.castle, terrainTypes.grass, terrainTypes.road, terrainTypes.road, 0, 0, CFRR, CFRR_Mask, 3, 0, false, new List<Area>() {
            new Area { edges = new List<int>() {1,2,3}, terrain = terrainTypes.castle, colorIndex = 1, meeplePlacementIndex = 3},
            new Area { edges = new List<int>() {0,4,5,6,7,12}, terrain = terrainTypes.grass, colorIndex = 2, meeplePlacementIndex = 15},
            new Area { edges = new List<int>() {8,11}, terrain = terrainTypes.road, colorIndex = 3, meeplePlacementIndex = 35},
            new Area { edges = new List<int>() {9,10}, terrain = terrainTypes.grass, colorIndex = 4, meeplePlacementIndex = 31}});
        //CRRR
        addTileToList(18, terrainTypes.castle, terrainTypes.road, terrainTypes.road, terrainTypes.road, 0, 0, CRRR, CRRR_Mask, 3, 0, false, new List<Area>() {
            new Area { edges = new List<int>() {1,2,3}, terrain = terrainTypes.castle, colorIndex = 1, meeplePlacementIndex = 3},
            new Area { edges = new List<int>() {4,12}, terrain = terrainTypes.grass, colorIndex = 2, meeplePlacementIndex = 15},
            new Area { edges = new List<int>() {5}, terrain = terrainTypes.road, colorIndex = 3, meeplePlacementIndex = 13},
            new Area { edges = new List<int>() {7,6}, terrain = terrainTypes.grass, colorIndex = 4, meeplePlacementIndex = 21},
            new Area { edges = new List<int>() {8}, terrain = terrainTypes.road, colorIndex = 5, meeplePlacementIndex = 23},
            new Area { edges = new List<int>() {9,10}, terrain = terrainTypes.grass, colorIndex = 6, meeplePlacementIndex = 31},
            new Area { edges = new List<int>() {11}, terrain = terrainTypes.road, colorIndex = 7, meeplePlacementIndex = 33},
            new Area { edges = new List<int>() {0}, terrain = terrainTypes.intersection, colorIndex = -1, meeplePlacementIndex = -1}});
        //FRFR
        addTileToList(19, terrainTypes.grass, terrainTypes.road, terrainTypes.grass, terrainTypes.road, 0, 0, FRFR, FRFR_Mask, 8, 0, false, new List<Area>() {
            new Area { edges = new List<int>() {1,2,3,4,12}, terrain = terrainTypes.grass, colorIndex = 1, meeplePlacementIndex = 3},
            new Area { edges = new List<int>() {5, 0, 11}, terrain = terrainTypes.road, colorIndex = 2, meeplePlacementIndex = 0},
            new Area { edges = new List<int>() {6,7,8,9,10}, terrain = terrainTypes.grass, colorIndex = 3, meeplePlacementIndex = 23}});
        //FFRR
        addTileToList(20, terrainTypes.grass, terrainTypes.grass, terrainTypes.road, terrainTypes.road, 0, 0, FFRR, FFRR_Mask, 9, 0, false, new List<Area>() {
            new Area { edges = new List<int>() {0,1,2,3,4,5,6,7,12}, terrain = terrainTypes.grass, colorIndex = 1, meeplePlacementIndex = 15},
            new Area { edges = new List<int>() {8,11}, terrain = terrainTypes.road, colorIndex = 2, meeplePlacementIndex = 0},
            new Area { edges = new List<int>() {9,10}, terrain = terrainTypes.grass, colorIndex = 3, meeplePlacementIndex = 31}});
        //FRRR
        addTileToList(21, terrainTypes.grass, terrainTypes.road, terrainTypes.road, terrainTypes.road, 0, 0, FRRR, FRRR_Mask, 4, 0, false, new List<Area>() {
            new Area { edges = new List<int>() {1,2,3,4,12}, terrain = terrainTypes.grass, colorIndex = 1, meeplePlacementIndex = 3},
            new Area { edges = new List<int>() {5}, terrain = terrainTypes.road, colorIndex = 2, meeplePlacementIndex = 13},
            new Area { edges = new List<int>() {7,6}, terrain = terrainTypes.grass, colorIndex = 3, meeplePlacementIndex = 21},
            new Area { edges = new List<int>() {8}, terrain = terrainTypes.road, colorIndex = 4, meeplePlacementIndex = 23},
            new Area { edges = new List<int>() {9,10}, terrain = terrainTypes.grass, colorIndex = 5, meeplePlacementIndex = 31},
            new Area { edges = new List<int>() {11}, terrain = terrainTypes.road, colorIndex = 6, meeplePlacementIndex = 33},
            new Area { edges = new List<int>() {0}, terrain = terrainTypes.intersection, colorIndex = -1, meeplePlacementIndex = -1}});
        //RRRR
        addTileToList(22, terrainTypes.road, terrainTypes.road, terrainTypes.road, terrainTypes.road, 0, 0, RRRR, RRRR_Mask, 1, 0, false, new List<Area>() {
            new Area { edges = new List<int>() {12,1}, terrain = terrainTypes.grass, colorIndex = 1, meeplePlacementIndex = 1},
            new Area { edges = new List<int>() {2}, terrain = terrainTypes.road, colorIndex = 2, meeplePlacementIndex = 3},
            new Area { edges = new List<int>() {3,4} ,terrain = terrainTypes.grass, colorIndex = 3, meeplePlacementIndex = 11},
            new Area { edges = new List<int>() {5}, terrain = terrainTypes.road, colorIndex = 4, meeplePlacementIndex = 13},
            new Area { edges = new List<int>() {6,7}, terrain = terrainTypes.grass, colorIndex = 5, meeplePlacementIndex = 21},
            new Area { edges = new List<int>() {8}, terrain = terrainTypes.road, colorIndex = 6, meeplePlacementIndex = 23},
            new Area { edges = new List<int>() {9,10}, terrain = terrainTypes.grass, colorIndex = 7, meeplePlacementIndex = 31},
            new Area { edges = new List<int>() {11}, terrain = terrainTypes.road, colorIndex = 8, meeplePlacementIndex = 33},
            new Area { edges = new List<int>() {0}, terrain = terrainTypes.intersection, colorIndex = -1, meeplePlacementIndex = -1}});
        //FFRF_M
        addTileToList(23, terrainTypes.grass, terrainTypes.grass, terrainTypes.road, terrainTypes.grass, 0, 0, FFRF_M, FFRF_M_Mask, 2, 0, false, new List<Area>() {
            new Area { edges = new List<int>() {1,2,3,4,5,6,7,9,10,11,12}, terrain = terrainTypes.grass, colorIndex = 1, meeplePlacementIndex = 21},
            new Area { edges = new List<int>() {8}, terrain = terrainTypes.road, colorIndex = 2, meeplePlacementIndex = 23},
            new Area { edges = new List<int>() {0}, terrain = terrainTypes.monastery, colorIndex = 3, meeplePlacementIndex = 0}});
        //FFFF_M
        addTileToList(24, terrainTypes.grass, terrainTypes.grass, terrainTypes.grass, terrainTypes.grass, 0, 0, FFFF_M, FFFF_M_Mask, 4, 0, false, new List<Area>() {
            new Area { edges = new List<int>() {1,2,3,4,5,6,7,8,9,10,11,12}, terrain = terrainTypes.grass, colorIndex = 1, meeplePlacementIndex = 21},
            new Area { edges = new List<int>() {0}, terrain = terrainTypes.monastery, colorIndex = 2, meeplePlacementIndex = 0}});
            

    */



    }




}
