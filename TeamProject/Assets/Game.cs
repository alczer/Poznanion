using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class Game : MonoBehaviour {
    GameObject[,] tilesOnBoard = new GameObject[200, 200];
    GameObject[,] possibleMoves = new GameObject[200, 200];

    GameObject gameManager;
    public CameraManager CM;
    public TilesManager TM;
    public GameObject Selected;
    public GameObject NextTileImage;
    
    private bool currentlyPlacingTile;
    private int[] currentlyPlacedTile;
    private Tile choosenTile;
    public GameObject OKButton;
    Ray myRay;
    RaycastHit hit;
    public GameObject objectToinstantiate;
    public int currentNbrTiles;

    public void ButtonClicked()
    {
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

        if (CM.cameraCheck)
        {
            CM.UpdateCamera();
            CM.cameraCheck = false;
        }
    }
    // Start
    void Start()
    {
        currentlyPlacedTile = new int[2];
        currentNbrTiles = 0;
        TM.placeTile(ref objectToinstantiate, 100, 100,ref tilesOnBoard);
        Tile startTile = tilesOnBoard[100, 100].AddComponent<Tile>();
        startTile.Init(terrainTypes.castle, terrainTypes.road, terrainTypes.grass, terrainTypes.road, 0, 0, TM.CRFR, 1, terrainTypes.road, terrainTypes.castle, terrainTypes.castle, terrainTypes.castle,
            terrainTypes.grass, terrainTypes.road, terrainTypes.grass, terrainTypes.grass, terrainTypes.grass, terrainTypes.grass, terrainTypes.grass, terrainTypes.road, terrainTypes.grass,
            new List<List<int>>() { new List<int>() { 4, 12 }, new List<int>() { 6, 7, 8, 9, 10 } }, new List<List<int>>() { new List<int>() { 1, 2, 3 } }, new List<List<int>>() { new List<int>() { 5, 0, 11 } });
        tilesOnBoard[100, 100].GetComponent<Renderer>().material = TM.CRFR;
        currentlyPlacingTile = false;
        TM.init();
    }
    // Update is called once per frame
    void Update()
    {
        if (currentlyPlacingTile == false)
        {
            if (TM.tilesList.Count > 0)
            {
                int i;
                int j = 0;
                List<int[]> possiblePositions;
                do{
                    i = Random.Range(0, TM.tilesList.Count);
                    choosenTile = TM.tilesList[i];
                    possiblePositions = TM.findMatchingEdges(TM.findSourrounding(ref tilesOnBoard), choosenTile, ref tilesOnBoard);
                    j++;
                } while (possiblePositions.Count == 0 && j < 10);
                NextTileImage.GetComponent<Image>().material = choosenTile.Material;
                TM.tilesList[i].TypeCount--;
                if (TM.tilesList[i].TypeCount == 0)
                {
                    TM.tilesList.RemoveAt(i);
                }
                currentlyPlacingTile = true;
                
                
                foreach (var arrPosition in possiblePositions)
                {
                    float[] position = TM.getCoordinates(arrPosition[0], arrPosition[1]);
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
                    int[] arrayIndex = TM.getArrayPosition(position.x, position.z);
                    Debug.Log("Position Array row: " + arrayIndex[0].ToString() + " Position array column: " + arrayIndex[1].ToString());

                    if (tilesOnBoard[arrayIndex[0], arrayIndex[1]] != null)
                    {
                        if (OKButton.activeSelf)
                        {
                            if (arrayIndex[0] == currentlyPlacedTile[0] && arrayIndex[1] == currentlyPlacedTile[1])
                            {
                                TM.rotateFirstMatchingRotation(ref tilesOnBoard[arrayIndex[0], arrayIndex[1]], arrayIndex, ref tilesOnBoard);
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
                        if (possibleMoves[arrayIndex[0], arrayIndex[1]] != null)
                        {
                            if (currentlyPlacedTile != null)
                            {
                                Destroy(tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]]);
                                tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]] = null;
                            }
                            OKButton.SetActive(true);
                            tilesOnBoard[arrayIndex[0], arrayIndex[1]] = Instantiate(objectToinstantiate, position, Quaternion.identity) as GameObject; // instatiate a prefab on the position where the ray hits the floor.                         
                            Tile tile = tilesOnBoard[arrayIndex[0], arrayIndex[1]].AddComponent<Tile>();
                            tile.Init(choosenTile.UpTerrain, choosenTile.RightTerrain, choosenTile.DownTerrain, choosenTile.LeftTerrain, position.x, position.z, choosenTile.Material, choosenTile.TypeCount, choosenTile.Middle.terrain
                                , choosenTile.UpL.terrain, choosenTile.UpM.terrain, choosenTile.UpR.terrain, choosenTile.RightU.terrain, choosenTile.RightM.terrain, choosenTile.RightD.terrain, choosenTile.DownR.terrain, choosenTile.DownM.terrain,
                                choosenTile.DownL.terrain, choosenTile.LeftD.terrain, choosenTile.LeftM.terrain, choosenTile.LeftU.terrain, choosenTile.GrassAreas, choosenTile.CastleAreas, choosenTile.RoadAreas);
                            TM.rotateFirstMatchingRotation(ref tilesOnBoard[arrayIndex[0], arrayIndex[1]], arrayIndex, ref tilesOnBoard);
                            tilesOnBoard[arrayIndex[0], arrayIndex[1]].GetComponent<Renderer>().material = choosenTile.Material;

                            currentlyPlacedTile = arrayIndex;
                            CM.CheckCamera(arrayIndex);
                        }
                    }
                }
            }
        }
    }
}
