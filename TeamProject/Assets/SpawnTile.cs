using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.UI;

public class SpawnTile : MonoBehaviour
{
    

    GameObject[,] tilesOnBoard = new GameObject[200, 200];
    GameObject[,] possibleMoves = new GameObject[200, 200];
    List<Tile> tilesList = new List<Tile>(); //List of all disponible tiles (not on board)

    public Material startMaterial;
    public Material m1;
    public Material m2;
    public Material m3;
    public Material m4;
    public GameObject Selected;
    private MoveCamera moveCamera;
    private float maxX = 0;
    private float minX = 0;
    private float maxY = 0;
    private float minY = 0;
    private bool currentlyPlacingTile;
    private int[] currentlyPlacedTile;

    private Tile choosenTile;
    public GameObject OKButton;

    Ray myRay;
    RaycastHit hit;
    public GameObject objectToinstantiate;
    public int currentNbrTiles;

    int[] getArrayPosition(float x, float z)
    {
        float col = 100 + (x / 10);
        float row = 100 - (z / 10);
        int[] array = new int[2] { (int)row, (int)col };
        return array;
    }

    float[] getCoordinates(int row, int col)
    {
        float x = ((float)col - 100) * 10;
        float z = ((float)row - 100) * (-10);
        float[] array = new float[2] { x, z };
        return array;
    }
    void rotateClockwise90(ref GameObject gameObject)
    {
        terrainTypes up = gameObject.GetComponent<Tile>().UpTerrain;
        terrainTypes right = gameObject.GetComponent<Tile>().RightTerrain;
        terrainTypes down = gameObject.GetComponent<Tile>().DownTerrain;
        terrainTypes left = gameObject.GetComponent<Tile>().LeftTerrain;
        gameObject.GetComponent<Tile>().UpTerrain = left;
        gameObject.GetComponent<Tile>().RightTerrain = up;
        gameObject.GetComponent<Tile>().DownTerrain = right;
        gameObject.GetComponent<Tile>().LeftTerrain = down;
        gameObject.transform.Rotate(new Vector3(0, 90, 0));
    }

    void rotateFirstMatchingRotation(ref GameObject gameObject, int[] gameObjectPosition)
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

            //rotateClockwise180(ref gameObject);
            rotateClockwise90(ref gameObject);
            rotateClockwise90(ref gameObject);
        }
        else if ((tilesOnBoard[gameObjectPosition[0] - 1, gameObjectPosition[1]] == null || tilesOnBoard[gameObjectPosition[0] - 1, gameObjectPosition[1]].GetComponent<Tile>().DownTerrain == gameObject.GetComponent<Tile>().RightTerrain)
            && (tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] + 1] == null || tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] + 1].GetComponent<Tile>().LeftTerrain == gameObject.GetComponent<Tile>().DownTerrain)
            && (tilesOnBoard[gameObjectPosition[0] + 1, gameObjectPosition[1]] == null || tilesOnBoard[gameObjectPosition[0] + 1, gameObjectPosition[1]].GetComponent<Tile>().UpTerrain == gameObject.GetComponent<Tile>().LeftTerrain)
            && (tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] - 1] == null || tilesOnBoard[gameObjectPosition[0], gameObjectPosition[1] - 1].GetComponent<Tile>().RightTerrain == gameObject.GetComponent<Tile>().UpTerrain))
        {
            //rotateClockwise270(ref gameObject);
            rotateClockwise90(ref gameObject);
            rotateClockwise90(ref gameObject);
            rotateClockwise90(ref gameObject);
        }


    }

    void placeTile(ref GameObject obj, int x, int y)
    {
        tilesOnBoard[x, y] = Instantiate(obj, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
    }

    void addTileToList(terrainTypes up, terrainTypes right, terrainTypes down, terrainTypes left, float x, float y, Material m)
    {
        Tile tmp = new Tile();
        tmp.Init(up, right, down, left, x, y, m);
        tilesList.Add(tmp);
    }

    List<int[]> findMatchingEdges(List<int[]> movesList, Tile choosenTile)
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

    List<int[]> findSourrounding()
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

    //
    // START
    //
    void Start()
    {
        // Camera - sets initial range of camera
        GameObject go = GameObject.Find("Main Camera");
        moveCamera = go.GetComponent<MoveCamera>();
        // Initial overwrite settings - not needed
        moveCamera.Xmax = 40;
        moveCamera.Xmin = -40;
        moveCamera.Zmax = 40;
        moveCamera.Zmin = -40;
        // =======================================


        currentlyPlacedTile = new int[2];
        currentNbrTiles = 0;
        placeTile(ref objectToinstantiate, 100, 100);
        Tile startTile = tilesOnBoard[100, 100].AddComponent<Tile>();
        startTile.Init(terrainTypes.castle, terrainTypes.grassRoad, terrainTypes.grass, terrainTypes.grassRoad, 0, 0, startMaterial);
        tilesOnBoard[100, 100].GetComponent<Renderer>().material = startMaterial;
        currentlyPlacingTile = false;
        addTileToList(terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, 0, 0, m1);
        addTileToList(terrainTypes.grassRoad, terrainTypes.grass, terrainTypes.grass, terrainTypes.grass, 0, 0, m2);
        addTileToList(terrainTypes.grass, terrainTypes.grass, terrainTypes.grass, terrainTypes.grass, 0, 0, m3);
        addTileToList(terrainTypes.castle, terrainTypes.castle, terrainTypes.castle, terrainTypes.castle, 0, 0, m4);
    }

   public void ButtonClicked()
    {
 	//Change camera max range
        moveCamera.Xmax = 40 + maxX;
        moveCamera.Xmin = -40 + minX;
        moveCamera.Zmax = 40 + maxY;
        moveCamera.Zmin = -40 + minY;
        currentlyPlacingTile = false;
        currentlyPlacedTile = null;
        for (int row = 0; row < possibleMoves.GetLength(0); row++)
        {
            for (int col = 0; col < possibleMoves.GetLength(1); col++)
            {
                if (possibleMoves[row, col] != null)
                    Destroy(possibleMoves[row, col]);
                possibleMoves[row, col] = null;
            }
        }
        OKButton.SetActive(false);
    }

    //
    // UPDATE
    //

    void Update()
    {
        if (currentlyPlacingTile == false)
        {
            if (tilesList.Count > 0)
            {
                int i = Random.Range(0, tilesList.Count);
                choosenTile = tilesList[i];
                tilesList.RemoveAt(i);
                currentlyPlacingTile = true;
                List<int[]> possiblePositions = findMatchingEdges(findSourrounding(), choosenTile);
                foreach (var arrPosition in possiblePositions)
                {
                    float[] position = getCoordinates(arrPosition[0], arrPosition[1]);
                    if (possibleMoves[arrPosition[0], arrPosition[1]] == null)
                    {
                        possibleMoves[arrPosition[0], arrPosition[1]] = Instantiate(Selected, new Vector3(position[0], (float)0.1, position[1]), Quaternion.identity) as GameObject;
                    }
                }
                OKButton.SetActive(false);
            }
        }
        else
        {
            myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(myRay, out hit))
            {
                if (Input.GetMouseButtonUp(0))
                {
                    Vector3 position = hit.point;
                    position.x = Mathf.Round(position.x / 10) * 10; // 10 is the size of a Tile
                    position.z = Mathf.Round(position.z / 10) * 10; // 10 is the size of a Tile
                    Debug.Log("Position x: " + position.x.ToString() + " Position z: " + position.z.ToString());
                    int[] arrayIndex = getArrayPosition(position.x, position.z);
                    Debug.Log("Position Array row: " + arrayIndex[0].ToString() + " Position array column: " + arrayIndex[1].ToString());

                    if (tilesOnBoard[arrayIndex[0], arrayIndex[1]] != null)
                    {
                        if (OKButton.activeSelf)
                        {
                            if (arrayIndex[0] == currentlyPlacedTile[0] && arrayIndex[1] == currentlyPlacedTile[1])
                            {
                                rotateFirstMatchingRotation(ref tilesOnBoard[arrayIndex[0], arrayIndex[1]], arrayIndex);
                                //rotateClockwise90(ref tilesOnBoard[arrayIndex[0], arrayIndex[1]]);              
                            }
                        }
                        else
                        {
                            Debug.Log("A tile is already at this position");
                        }
                    }
                    else if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false)
                    {
                        if (possibleMoves[arrayIndex[0], arrayIndex[1]]!=null)
                        {
                            if (currentlyPlacedTile != null) Destroy(tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]]);                         
                            OKButton.SetActive(true);
                            tilesOnBoard[arrayIndex[0], arrayIndex[1]] = Instantiate(objectToinstantiate, position, Quaternion.identity) as GameObject;// instatiate a prefab on the position where the ray hits the floor.                         
                            Tile tile = tilesOnBoard[arrayIndex[0], arrayIndex[1]].AddComponent<Tile>();
                            tile.Init(choosenTile.UpTerrain, choosenTile.RightTerrain, choosenTile.DownTerrain, choosenTile.LeftTerrain, position.x, position.z, choosenTile.Material);
                            rotateFirstMatchingRotation(ref tilesOnBoard[arrayIndex[0], arrayIndex[1]], arrayIndex);
                            tilesOnBoard[arrayIndex[0], arrayIndex[1]].GetComponent<Renderer>().material = choosenTile.Material;
                            currentlyPlacedTile = arrayIndex;
                        }
                    }
                }
            }
        }
    }
}
