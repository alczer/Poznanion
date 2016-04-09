using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.UI;

public class TilesManager : MonoBehaviour {
    
    public List<Tile> tilesList = new List<Tile>(); //List of all disponible tiles (not on board)
    public Material CRFR; // START
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

    public void addTileToList(terrainTypes up, terrainTypes right, terrainTypes down, terrainTypes left, float x, float y, Material m, int count, terrainTypes middle,
        terrainTypes upL, terrainTypes upM, terrainTypes upR, terrainTypes rightU, terrainTypes rightM, terrainTypes rightD, terrainTypes downR, terrainTypes downM, terrainTypes downL,
        terrainTypes leftD, terrainTypes leftM, terrainTypes leftU, List<List<int>> grassAreas, List<List<int>> castleAreas, List<List<int>> roadAreas)
    {
        Tile tmp = new Tile();
        tmp.Init(up, right, down, left, x, y, m, count, middle, upL, upM, upR, rightU, rightM, rightD, downR, downM, downL, leftD, leftM, leftU, grassAreas, castleAreas, roadAreas);
        tilesList.Add(tmp);
    }
    public void placeTile(ref GameObject obj, int x, int y, ref GameObject[,] tilesOnBoard)
    {
        tilesOnBoard[x, y] = Instantiate(obj, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
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

    public void rotateClockwise90(ref GameObject gameObject)
    {
        terrainTypes up = gameObject.GetComponent<Tile>().UpTerrain;
        terrainTypes right = gameObject.GetComponent<Tile>().RightTerrain;
        terrainTypes down = gameObject.GetComponent<Tile>().DownTerrain;
        terrainTypes left = gameObject.GetComponent<Tile>().LeftTerrain;

        tileEdges upL = gameObject.GetComponent<Tile>().UpL;
        tileEdges upM = gameObject.GetComponent<Tile>().UpM;
        tileEdges upR = gameObject.GetComponent<Tile>().UpR;
        tileEdges rightU = gameObject.GetComponent<Tile>().RightU;
        tileEdges rightM = gameObject.GetComponent<Tile>().RightM;
        tileEdges rightD = gameObject.GetComponent<Tile>().RightD;
        tileEdges downR = gameObject.GetComponent<Tile>().DownR;
        tileEdges downM = gameObject.GetComponent<Tile>().DownM;
        tileEdges downL = gameObject.GetComponent<Tile>().DownL;
        tileEdges leftD = gameObject.GetComponent<Tile>().LeftD;
        tileEdges leftM = gameObject.GetComponent<Tile>().LeftM;
        tileEdges leftU = gameObject.GetComponent<Tile>().LeftU;

        gameObject.GetComponent<Tile>().UpTerrain = left;
        gameObject.GetComponent<Tile>().RightTerrain = up;
        gameObject.GetComponent<Tile>().DownTerrain = right;
        gameObject.GetComponent<Tile>().LeftTerrain = down;

        gameObject.GetComponent<Tile>().UpL = rightU;
        gameObject.GetComponent<Tile>().UpM = rightM;
        gameObject.GetComponent<Tile>().UpR = rightD;
        gameObject.GetComponent<Tile>().RightU = downR;
        gameObject.GetComponent<Tile>().RightM = downM;
        gameObject.GetComponent<Tile>().RightD = downL;
        gameObject.GetComponent<Tile>().DownR = leftD;
        gameObject.GetComponent<Tile>().DownM = leftM;
        gameObject.GetComponent<Tile>().DownL = leftU;
        gameObject.GetComponent<Tile>().LeftD = upL;
        gameObject.GetComponent<Tile>().LeftM = upM;
        gameObject.GetComponent<Tile>().LeftU = upR;

        gameObject.transform.Rotate(new Vector3(0, 90, 0));
    }

    public void rotateFirstMatchingRotation(ref GameObject gameObject, int[] gameObjectPosition, ref GameObject[,] tilesOnBoard)
    {
        if ((tilesOnBoard[gameObjectPosition[0] - 1, gameObjectPosition[1]] == null || tilesOnBoard[gameObjectPosition[0] - 1, gameObjectPosition[1]].GetComponent<Tile>().DownTerrain == gameObject.GetComponent<Tile>().LeftTerrain)
            && (tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] + 1] == null || tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] + 1].GetComponent<Tile>().LeftTerrain == gameObject.GetComponent<Tile>().UpTerrain)
            && (tilesOnBoard[gameObjectPosition[0] + 1, gameObjectPosition[1]] == null || tilesOnBoard[gameObjectPosition[0] + 1, gameObjectPosition[1]].GetComponent<Tile>().UpTerrain == gameObject.GetComponent<Tile>().RightTerrain)
            && (tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] - 1] == null || tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] - 1].GetComponent<Tile>().RightTerrain == gameObject.GetComponent<Tile>().DownTerrain))
        {
            rotateClockwise90(ref gameObject);
        }
        else if ((tilesOnBoard[gameObjectPosition[0] - 1, gameObjectPosition[1]] == null || tilesOnBoard[gameObjectPosition[0] - 1, gameObjectPosition[1]].GetComponent<Tile>().DownTerrain == gameObject.GetComponent<Tile>().DownTerrain)
            && (tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] + 1] == null || tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] + 1].GetComponent<Tile>().LeftTerrain == gameObject.GetComponent<Tile>().LeftTerrain)
            && (tilesOnBoard[gameObjectPosition[0] + 1, gameObjectPosition[1]] == null || tilesOnBoard[gameObjectPosition[0] + 1, gameObjectPosition[1]].GetComponent<Tile>().UpTerrain == gameObject.GetComponent<Tile>().UpTerrain)
            && (tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] - 1] == null || tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] - 1].GetComponent<Tile>().RightTerrain == gameObject.GetComponent<Tile>().RightTerrain))
        {

            rotateClockwise90(ref gameObject);
            rotateClockwise90(ref gameObject);
        }
        else if ((tilesOnBoard[gameObjectPosition[0] - 1, gameObjectPosition[1]] == null || tilesOnBoard[gameObjectPosition[0] - 1, gameObjectPosition[1]].GetComponent<Tile>().DownTerrain == gameObject.GetComponent<Tile>().RightTerrain)
            && (tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] + 1] == null || tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] + 1].GetComponent<Tile>().LeftTerrain == gameObject.GetComponent<Tile>().DownTerrain)
            && (tilesOnBoard[gameObjectPosition[0] + 1, gameObjectPosition[1]] == null || tilesOnBoard[gameObjectPosition[0] + 1, gameObjectPosition[1]].GetComponent<Tile>().UpTerrain == gameObject.GetComponent<Tile>().LeftTerrain)
            && (tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] - 1] == null || tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] - 1].GetComponent<Tile>().RightTerrain == gameObject.GetComponent<Tile>().UpTerrain))
        {
            rotateClockwise90(ref gameObject);
            rotateClockwise90(ref gameObject);
            rotateClockwise90(ref gameObject);
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
    public void init ()
    {
        //RRRR
        addTileToList(terrainTypes.road, terrainTypes.road, terrainTypes.road, terrainTypes.road, 0, 0, RRRR, 1, terrainTypes.intersection, terrainTypes.grass, terrainTypes.road, terrainTypes.grass,
            terrainTypes.grass, terrainTypes.road, terrainTypes.grass, terrainTypes.grass, terrainTypes.road, terrainTypes.grass, terrainTypes.grass, terrainTypes.road, terrainTypes.grass,
            new List<List<int>>() { new List<int>() { 3, 4 }, new List<int>() { 6, 7 }, new List<int>() { 9, 10 }, new List<int>() { 12, 1 } }, new List<List<int>>() { },
            new List<List<int>>() { new List<int>() { 2 }, new List<int>() { 5 }, new List<int>() { 8 }, new List<int>() { 11 } });
        //CCRC
        addTileToList(terrainTypes.castle, terrainTypes.castle, terrainTypes.road, terrainTypes.castle, 0, 0, CCRC, 1, terrainTypes.castle, terrainTypes.castle, terrainTypes.castle, terrainTypes.castle,
                terrainTypes.castle, terrainTypes.castle, terrainTypes.castle, terrainTypes.grass, terrainTypes.road, terrainTypes.grass, terrainTypes.castle, terrainTypes.castle, terrainTypes.castle,
                new List<List<int>>() { new List<int>() { 7 }, new List<int>() { 9 } }, new List<List<int>>() { new List<int>() { 1, 2, 3, 4, 5, 6, 10, 11, 12 } }, new List<List<int>>() { new List<int>() { 8 } });
        //CCFC
        addTileToList(terrainTypes.castle, terrainTypes.castle, terrainTypes.grass, terrainTypes.castle, 0, 0, CCFC, 3, terrainTypes.castle, terrainTypes.castle, terrainTypes.castle, terrainTypes.castle,
                terrainTypes.castle, terrainTypes.castle, terrainTypes.castle, terrainTypes.grass, terrainTypes.grass, terrainTypes.grass, terrainTypes.castle, terrainTypes.castle, terrainTypes.castle,
                new List<List<int>>() { new List<int>() { 7, 8, 9 } }, new List<List<int>>() { new List<int>() { 1, 2, 3, 4, 5, 6, 10, 11, 12 } }, new List<List<int>>() { new List<int>() { } });
        //CFFF
        addTileToList(terrainTypes.castle, terrainTypes.grass, terrainTypes.grass, terrainTypes.grass, 0, 0, CFFF, 5, terrainTypes.grass, terrainTypes.castle, terrainTypes.castle, terrainTypes.castle,
            terrainTypes.grass, terrainTypes.grass, terrainTypes.grass, terrainTypes.grass, terrainTypes.grass, terrainTypes.grass, terrainTypes.grass, terrainTypes.grass, terrainTypes.grass,
            new List<List<int>>() { new List<int>() { 4, 5, 6, 7, 8, 9, 10, 11, 12 } }, new List<List<int>>() { new List<int>() { 1, 2, 3 } }, new List<List<int>>() { });

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
