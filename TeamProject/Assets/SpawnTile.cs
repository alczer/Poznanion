﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

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
    public GameObject Canvas;
    public GameObject Button;
    private GameObject newCanvas;
    private GameObject newButton;
    private Button button;
    private bool currentlyPlacingTile;
    private int[] currentlyPlacedTile;
    private Tile choosenTile;
    private bool Escape = false;

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
    /*
    void rotateClockwise270(ref GameObject gameObject)
    {
        terrainTypes up = gameObject.GetComponent<Tile>().UpTerrain;
        terrainTypes right = gameObject.GetComponent<Tile>().RightTerrain;
        terrainTypes down = gameObject.GetComponent<Tile>().DownTerrain;
        terrainTypes left = gameObject.GetComponent<Tile>().LeftTerrain;
        gameObject.GetComponent<Tile>().UpTerrain = right;
        gameObject.GetComponent<Tile>().RightTerrain = down;
        gameObject.GetComponent<Tile>().DownTerrain = left;
        gameObject.GetComponent<Tile>().LeftTerrain = up;
        gameObject.transform.Rotate(new Vector3(0, 270, 0));
    }
    void rotateClockwise180(ref GameObject gameObject)
    {
        terrainTypes up = gameObject.GetComponent<Tile>().UpTerrain;
        terrainTypes right = gameObject.GetComponent<Tile>().RightTerrain;
        terrainTypes down = gameObject.GetComponent<Tile>().DownTerrain;
        terrainTypes left = gameObject.GetComponent<Tile>().LeftTerrain;
        gameObject.GetComponent<Tile>().UpTerrain = down;
        gameObject.GetComponent<Tile>().RightTerrain = left;
        gameObject.GetComponent<Tile>().DownTerrain = up;
        gameObject.GetComponent<Tile>().LeftTerrain = right;
        gameObject.transform.Rotate(new Vector3(0, 180, 0));
    }
    */
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
    void back_to_menu()
    {
        if (GUI.Button(new Rect(110, 10, 100, 50), "Quit game"))
        {
            Application.LoadLevel("Menu");
        }
        if (GUI.Button(new Rect(210, 10, 100, 50), "Continue game"))
        {
            Escape = false;
        }
    }
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 50), "Menu"))
        {
            Escape = true;
        }
        if (Escape == true)
        {
            back_to_menu();
        }
    }
    void Start()
    {
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
   
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) 
        {
            if (Escape == true)
                Application.LoadLevel("Menu");
            else
                Escape = true;
        }	
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
                        possibleMoves[arrPosition[0], arrPosition[1]] = Instantiate(Selected, new Vector3(position[0], (float)0.01, position[1]), Quaternion.identity) as GameObject;
                    }
                }
                newCanvas = Instantiate(Canvas) as GameObject;
                newButton = Instantiate(Button) as GameObject;
                newButton.transform.SetParent(newCanvas.transform, false);
                newButton.transform.localScale = new Vector3(1.66f, 2, 0);
                newButton.transform.localPosition = new Vector3((float)Screen.width * 0.5f - (newButton.GetComponent<RectTransform>().rect.width * 0.9f), (-(float)Screen.height * 0.5f) + (newButton.GetComponent<RectTransform>().rect.height * 1.3f), 0);
                button = newButton.GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    currentlyPlacingTile = false;
                    currentlyPlacedTile = null;
                    Destroy(newButton);
                    Destroy(newCanvas);
                    for (int row = 0; row < possibleMoves.GetLength(0); row++)
                    {
                        for (int col = 0; col < possibleMoves.GetLength(1); col++)
                        {
                            if (possibleMoves[row, col] != null)
                                Destroy(possibleMoves[row, col]);
                            possibleMoves[row, col] = null;
                        }
                    }
                });
                newButton.SetActive(false);
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
                        if (newButton.activeSelf)
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
                            newButton.SetActive(true);
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
