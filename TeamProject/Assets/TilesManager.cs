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

    public Material CCC_Mask;
    public Material CCCR_Mask;
    public Material C_Mask;
    public Material CRFR_Mask;
    public Material RRRR_Mask;


    public struct AreaTuple
    {
        public int x;
        public int y;
        public Area area;
        public bool initialized;
    }
    public void addTileToList(terrainTypes up, terrainTypes right, terrainTypes down, terrainTypes left, float x, float y, Material m, Material mask, int count, int turn,List<Area> areas)
    {
        Tile tmp = new Tile();
        tmp.Init(up, right, down, left, x, y, m, mask, count, turn, areas);
        tilesList.Add(tmp);
    }
    public void placeTile(ref GameObject tile, ref GameObject mask, int x, int y, ref GameObject[,] tilesOnBoard, ref GameObject[,] masks)
    {
        tilesOnBoard[x, y] = Instantiate(tile, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        masks[x,y] = Instantiate(mask, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
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
        foreach (var area in board[x, y].GetComponent<Tile>().Areas)
        {
            //Check if we can place meeple in there
            //And add area to the list if we can.
            if (isMovePossible(ref board, x, y, area, null))
                possibleAreas.Add(area);
        }
        //Return the list with areas that are ready for meeple.
        return possibleAreas;
    }

    public AreaTuple areaNeighbour(ref GameObject[,] board, int x, int y, int edge)
    {
        AreaTuple neighbour = new AreaTuple();
        neighbour.initialized = false;
        int[] neighbourCor = new int[2];
        Hashtable correspondingEdges = new Hashtable();
        correspondingEdges.Add(1, 5);
        correspondingEdges.Add(2, 8);
        correspondingEdges.Add(3, 7);
        correspondingEdges.Add(4, 12);
        correspondingEdges.Add(5, 11);
        correspondingEdges.Add(6, 10);
        correspondingEdges.Add(7, 3);
        correspondingEdges.Add(8, 2);
        correspondingEdges.Add(9, 1);
        correspondingEdges.Add(10, 6);
        correspondingEdges.Add(11, 5);
        correspondingEdges.Add(12, 4);
        if (edge == 1 || edge == 2 || edge == 3)
        {
            neighbourCor[0] = x - 1;
            neighbourCor[1] = y;
        }
        else if (edge == 4 || edge == 5 || edge == 6)
        {
            neighbourCor[0] = x;
            neighbourCor[1] = y + 1;
        }
        else if (edge == 7 || edge == 8 || edge == 9)
        {
            neighbourCor[0] = x + 1;
            neighbourCor[1] = y;
        }
        else if (edge == 10 || edge == 11 || edge == 12)
        {
            neighbourCor[0] = x;
            neighbourCor[1] = y - 1;
        }
        if (board[neighbourCor[0], neighbourCor[1]] != null)
        {
            foreach (var area in board[neighbourCor[0], neighbourCor[1]].GetComponent<Tile>().Areas)
            {
                if (area.edges.Contains(edge))
                {
                 neighbour.area = area;
                }
    
            }
            neighbour.x = neighbourCor[0];
            neighbour.y = neighbourCor[1];
            neighbour.initialized = true;    

        }

        
        return neighbour;
    }

    public List<AreaTuple> areaNeighbours(ref GameObject[,] board, int x, int y, List<int> edges)
    {
        List<AreaTuple> neighbours = new List<AreaTuple>();
        foreach(var edge in edges)
        {
            AreaTuple neighbour = areaNeighbour(ref board, x, y, edge);
            if(neighbour.initialized)
                neighbours.Add(neighbour);
        }
        return neighbours;
    }

    public bool isMovePossible(ref GameObject[,] board, int x, int y, Area currentlyChecked, List<AreaTuple> checkedAreas)
    {
        //If the tile has meeple already, we can't place meeple in there.
        if (currentlyChecked.player != null)
            return false;
        //Proper algorithm: ensure existence of checkedAreas
        if (checkedAreas==null)
            checkedAreas = new List<AreaTuple>();
        //By this line we've checked current area. Now it's time for neighbours.
        //For each neighbouring area:
        foreach(var neighbourTuple in areaNeighbours(ref board, x, y, currentlyChecked.edges))
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

    public void rotateClockwise90(ref GameObject gameObject,ref GameObject mask)
    {
        terrainTypes up = gameObject.GetComponent<Tile>().UpTerrain;
        terrainTypes right = gameObject.GetComponent<Tile>().RightTerrain;
        terrainTypes down = gameObject.GetComponent<Tile>().DownTerrain;
        terrainTypes left = gameObject.GetComponent<Tile>().LeftTerrain;
        foreach (var area in gameObject.GetComponent<Tile>().Areas)
        {
            for (int i = 0; i < area.edges.Count; i++)
            {
                if (area.edges[i] < 10)
                {
                    area.edges[i] += 3;
                }
                else
                {
                    area.edges[i] = area.edges[i] + 3 - 12;
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

    public void rotateFirstMatchingRotation(ref GameObject gameObject,ref GameObject mask, int[] gameObjectPosition, ref GameObject[,] tilesOnBoard)
    {
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
        addTileToList(terrainTypes.road, terrainTypes.road, terrainTypes.road, terrainTypes.road, 0, 0, RRRR,RRRR_Mask, 1,0, new List<Area>() { new Area {edges = new List<int>() {3,4} ,terrain = terrainTypes.grass, color = "#810000"},
            new Area { edges = new List<int>() {6,7}, terrain = terrainTypes.grass, color = "#FF0000" },new Area { edges = new List<int>() {9,10}, terrain = terrainTypes.grass,color = "#FD05FF" },new Area { edges = new List<int>() {12,1}, terrain = terrainTypes.grass, color = "#0CFF00" },
            new Area { edges = new List<int>() {2}, terrain = terrainTypes.road ,color = "#FFFF01"},new Area { edges = new List<int>() {5}, terrain = terrainTypes.road , color = "#000100"},new Area { edges = new List<int>() {8}, terrain = terrainTypes.road ,color = "#1700FF"},
            new Area { edges = new List<int>() {11}, terrain = terrainTypes.road,color = "#00FFFF" },new Area { edges = new List<int>() {0}, terrain = terrainTypes.intersection ,color = "000000"}});
        //CCRC
        addTileToList(terrainTypes.castle, terrainTypes.castle, terrainTypes.road, terrainTypes.castle, 0, 0, CCRC,CCCR_Mask, 1,0, new List<Area>() { new Area { edges = new List<int>() {0,1,2,3,4,5,6,10,11,12}, terrain = terrainTypes.castle,color = "#1600FF" },
            new Area { edges = new List<int>() {7}, terrain = terrainTypes.grass, color = "#0CFF00"},new Area { edges = new List<int>() {9}, terrain = terrainTypes.grass, color = "#FF0000"},new Area { edges = new List<int>() {8}, terrain = terrainTypes.road ,color = "#FFFF01"}});
        //CCFC
        addTileToList(terrainTypes.castle, terrainTypes.castle, terrainTypes.grass, terrainTypes.castle, 0, 0, CCFC,CCC_Mask, 3,0, new List<Area>() { new Area { edges = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 10, 11, 12 }, terrain = terrainTypes.castle, color = "#1600FF" },
        new Area { edges = new List<int>() {7,8,9}, terrain = terrainTypes.grass, color = "#FF0000" }});
        //CFFF
        addTileToList(terrainTypes.castle, terrainTypes.grass, terrainTypes.grass, terrainTypes.grass, 0, 0, CFFF,C_Mask, 5,0, new List<Area>() {new Area{edges = new List<int>() {1,2,3}, terrain = terrainTypes.castle,color = "#1600FF" },
            new Area { edges = new List<int>() {4,5,6,7,8,9,10,11,12}, terrain = terrainTypes.grass,color = "#FF0000" } });

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
