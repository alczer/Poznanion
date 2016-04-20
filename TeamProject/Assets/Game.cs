using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System;

public class Game : MonoBehaviour {
    GameObject[,] tilesOnBoard = new GameObject[200, 200];
    GameObject[,] possibleMoves = new GameObject[200, 200];

    GameObject gameManager;
    public CameraManager CM;
    public TilesManager TM;
    GameManager GM;
    public GameObject Selected;
    public GameObject NextTileImage;
    public Text playername; 
    private bool currentlyPlacingTile;
    private int[] currentlyPlacedTile;
    private Tile choosenTile;
    public GameObject OKButton;
    public GameObject MeepleButton;
    public GameObject Mask;
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
        MeepleButton.SetActive(false);
        GM.NextPlayer();

        if (CM.cameraCheck)
        {
            CM.UpdateCamera();
            CM.cameraCheck = false;
        }
    }
    void Awake()
    {
        GM = GameManager.Instance;
    }
        void Start()
    {
        currentlyPlacedTile = new int[2];
        currentNbrTiles = 0;
        TM.placeTile(ref objectToinstantiate, 100, 100,ref tilesOnBoard);
        Tile startTile = tilesOnBoard[100, 100].AddComponent<Tile>();
        startTile.Init(terrainTypes.castle, terrainTypes.road, terrainTypes.grass, terrainTypes.road, 0, 0, TM.CRFR, 1,0, new List<Area>() { new Area {edges = new List<int>() {1,2,3} ,terrain = terrainTypes.castle},
            new Area { edges = new List<int>() { 5, 0, 11 }, terrain = terrainTypes.road }, new Area { edges = new List<int>() { 4,12 }, terrain = terrainTypes.grass },new Area { edges = new List<int>() {6,7,8,9,10}, terrain = terrainTypes.grass } });
        tilesOnBoard[100, 100].GetComponent<Renderer>().material = TM.CRFR;
        currentlyPlacingTile = false;
        TM.init();

    }
    /*
    public static Color hexToColor(string hex)
    {
        hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
        byte a = 255;//assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }
    Color yellowColor = hexToColor("ffff00");
    Color blueColor = hexToColor("0000ff");
    Color greenColor = hexToColor("00ff00");
    Color redColor = hexToColor("ff0000");
    */
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
                    i = UnityEngine.Random.Range(0, TM.tilesList.Count);
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
                playername.text = GM.GetCurrentPlayer().name;
                 possiblePositions = TM.findMatchingEdges(TM.findSourrounding(ref tilesOnBoard), choosenTile, ref tilesOnBoard);

                foreach (var arrPosition in possiblePositions)
                {
                    float[] position = TM.getCoordinates(arrPosition[0], arrPosition[1]);
                    if (possibleMoves[arrPosition[0], arrPosition[1]] == null)
                    {
                        possibleMoves[arrPosition[0], arrPosition[1]] = Instantiate(Selected, new Vector3(position[0], (float)0.1, position[1]), Quaternion.identity) as GameObject;
                    }
                }
                OKButton.SetActive(false);
                MeepleButton.SetActive(false);
            }
        }
        else
        {
            myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(myRay, out hit))
            {
                if (Input.GetMouseButtonUp(0))
                {

                    //Texture2D tex = (Texture2D) Mask.GetComponent<Renderer>().material.mainTexture;
                    // Texture2D tex = (Texture2D)hit.collider.gameObject.GetComponent<Renderer>().material.mainTexture; // Get texture of object under mouse pointer
                    //Debug.Log(tex.GetPixelBilinear(hit.textureCoord2.x, hit.textureCoord2.y));
                    /*
                    if (tex.GetPixelBilinear(hit.textureCoord2.x, hit.textureCoord2.y) == blueColor)
                    {

                        Debug.Log("Clicked CASTLE !!!!!!!!!!!!!!!");
                    }
                    else if (tex.GetPixelBilinear(hit.textureCoord2.x, hit.textureCoord2.y) == redColor)
                    {

                        Debug.Log("Clicked GRASS 1 !!!!!!!!!!!!!!!");
                    }
                    else if (tex.GetPixelBilinear(hit.textureCoord2.x, hit.textureCoord2.y) == yellowColor)
                    {

                        Debug.Log("Clicked ROAD !!!!!!!!!!!!!!!");
                    }
                    else if (tex.GetPixelBilinear(hit.textureCoord2.x, hit.textureCoord2.y) == greenColor)
                    {

                        Debug.Log("Clicked GRASS 2 !!!!!!!!!!!!!!!");
                    }
                    else
                    {
                        Debug.Log("No color detected!");

                    }

    */

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
                            MeepleButton.SetActive(true);
                            tilesOnBoard[arrayIndex[0], arrayIndex[1]] = Instantiate(objectToinstantiate, position, Quaternion.identity) as GameObject; // instatiate a prefab on the position where the ray hits the floor.                         
                            Tile tile = tilesOnBoard[arrayIndex[0], arrayIndex[1]].AddComponent<Tile>();
                            tile.Init(choosenTile.UpTerrain, choosenTile.RightTerrain, choosenTile.DownTerrain, choosenTile.LeftTerrain, position.x, position.z, choosenTile.Material, choosenTile.TypeCount, choosenTile.Turn,choosenTile.Areas);
                            TM.rotateFirstMatchingRotation(ref tilesOnBoard[arrayIndex[0], arrayIndex[1]], arrayIndex, ref tilesOnBoard);
                            tilesOnBoard[arrayIndex[0], arrayIndex[1]].GetComponent<Renderer>().material = choosenTile.Material;
                            currentlyPlacedTile = arrayIndex;
                            CM.CheckCamera(arrayIndex);
                            List<Area> lists = TM.possibleMeepleAreas(ref tilesOnBoard, arrayIndex[0], arrayIndex[1]);
                            foreach (var list in lists)
                            { 

                              String result = String.Join(" ", list.edges.Select(item => item.ToString()).ToArray());
                             Debug.Log("Możliwy obszar:");
                             Debug.Log(result);

                            }
                        }
                    }
                }
            }
        }
    }
}
