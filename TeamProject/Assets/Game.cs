using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System;

public class Game : MonoBehaviour {
    GameObject[,] tilesOnBoard = new GameObject[200, 200];
    GameObject[,] masks = new GameObject[200, 200];
    GameObject[,] possibleMoves = new GameObject[200, 200];
    string choosenArea = "";
    GameObject[,] meeples = new GameObject[200, 200];

    List<Area> possibleMeeple = new List<Area>();

    GameObject gameManager;
    public CameraManager CM;
    public TilesManager TM;
    GameManager GM;
    public GameObject Selected;
    public GameObject NextTileImage;
    public Text playername; 
    private bool currentlyPlacingTile;
    private bool currentlyPlacingMeeple;
    private bool placedMeeple;
    private int[] currentlyPlacedTile;
    private Tile choosenTile;
    public GameObject OKButton;
    public GameObject MeepleButton;
    public GameObject Mask;
    public GameObject StandingMeeple;
    public GameObject FarmerMeeple;
    private List<GameObject> placedMeepleObject;
    private Area placedMeepleArea;
    private int currentlyPlacedMeeple;
    Ray myRay;
    RaycastHit hit;
    public GameObject objectToinstantiate;
    public int currentNbrTiles;

    public void ButtonClicked()
    {
        currentlyPlacingMeeple = false;
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
        currentlyPlacedMeeple++;


    }

    public void MeepleButtonClicked()
    {
        currentlyPlacingMeeple = true;
        possibleMeeple = TM.possibleMeepleAreas(ref tilesOnBoard, currentlyPlacedTile[0], currentlyPlacedTile[1]);
        Debug.Log("--------------------------dla kliknięcia----------------------------------------------");
        foreach (var list in possibleMeeple)
        {

            String result = String.Join(" ", list.edges.Select(item => item.ToString()).ToArray());
            Debug.Log("Możliwy obszar:");
            Debug.Log(result);

        }
        Debug.Log("-------------------------------------------------------------------------------------");
        
        MeepleButton.SetActive(false);
    }

    void Awake()
    {
        GM = GameManager.Instance;
    }
        void Start()
    {
        currentlyPlacedTile = new int[2];
        currentNbrTiles = 0;
        TM.placeTile(ref objectToinstantiate, ref Mask, 100, 100,ref tilesOnBoard, ref masks);
        Tile startTile = tilesOnBoard[100, 100].AddComponent<Tile>();
        startTile.Init(terrainTypes.castle, terrainTypes.road, terrainTypes.grass, terrainTypes.road, 0, 0, TM.CRFR,TM.CRFR_Mask, 1,0, new List<Area>() { new Area {edges = new List<int>() {1,2,3} ,terrain = terrainTypes.castle,color = "#1600FF"},
            new Area { edges = new List<int>() { 5, 0, 11 }, terrain = terrainTypes.road,color = "#0CFF00" }, new Area { edges = new List<int>() { 4,12 }, terrain = terrainTypes.grass,color = "#FFFF01" },new Area { edges = new List<int>() {6,7,8,9,10}, terrain = terrainTypes.grass,color = "#FF0000" } });
        tilesOnBoard[100, 100].GetComponent<Renderer>().material = TM.CRFR;
        masks[100, 100].GetComponent<Renderer>().material = TM.CRFR_Mask;
        currentlyPlacingTile = false;
        currentlyPlacingMeeple = false;
        placedMeeple = false;
        currentlyPlacedMeeple = 0;
        TM.init();

    }
 
    public static class ColorTypeConverter
    {
        public static string ToRGBHex(Color c)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}", ToByte(c.r), ToByte(c.g), ToByte(c.b));
        }

        private static byte ToByte(float f)
        {
            f = Mathf.Clamp01(f);
            return (byte)(f * 255);
        }
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
                    if (currentlyPlacingMeeple == true)
                    {
                        tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Collider>().enabled = false;
                        if (hit.transform.GetComponent<Renderer>().material.mainTexture.name == masks[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Renderer>().material.mainTexture.name)
                        {
                            
                            
                            Texture2D tex = hit.collider.GetComponent<MeshRenderer>().material.mainTexture as Texture2D;
                            Vector2 pixelUV = hit.textureCoord;
                            float uvX = pixelUV.x * tex.width;
                            float uvY = pixelUV.y * tex.height;
                            Color hitColor = tex.GetPixel((int)uvX, (int)uvY);
                            
                            if (possibleMeeple.Any(a => a.color == ColorTypeConverter.ToRGBHex(hitColor)))
                            {
                                Debug.Log(ColorTypeConverter.ToRGBHex(hitColor));
                                if (ColorTypeConverter.ToRGBHex(hitColor) == choosenArea)
                                {
                                    Debug.Log("Usuwam kafelek który już był tutaj!");


                                    Destroy(meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]]);
                                    tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Find(a => a.color == ColorTypeConverter.ToRGBHex(hitColor)).player  = null;
                                    choosenArea = "";
                                    placedMeeple = false;
                                }
                                else
                                {
                                    if(meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]] != null)
                                    {
                                        Destroy(meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]]);
                                        tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Find(a => a.color == ColorTypeConverter.ToRGBHex(hitColor)).player = null;
                                        choosenArea = "";
                                        placedMeeple = false;
                                    }
                                    Debug.Log("TAK, tu można postawić meepla!");
                                    choosenArea = possibleMeeple.Find(a => a.color == ColorTypeConverter.ToRGBHex(hitColor)).color;
                                    placedMeeple = true;
                                    if (possibleMeeple.Find(a => a.color == ColorTypeConverter.ToRGBHex(hitColor)).terrain == terrainTypes.grass)
                                    {
                                        meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]] = Instantiate(FarmerMeeple, hit.point, Quaternion.identity) as GameObject;
                                        meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]].transform.Rotate(new Vector3(-90, 0, 0));
                                        meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Renderer>().material.color = GM.GetCurrentPlayer().rgbaColor;
                                        tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Find(a => a.color == ColorTypeConverter.ToRGBHex(hitColor)).player = GM.GetCurrentPlayer();
                                    }
                                    else
                                    {
                                        meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]] = Instantiate(StandingMeeple, hit.point, Quaternion.identity) as GameObject;
                                        meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Renderer>().material.color = GM.GetCurrentPlayer().rgbaColor;
                                        tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Find(a => a.color == ColorTypeConverter.ToRGBHex(hitColor)).player = GM.GetCurrentPlayer();

                                    }
                                }
                            }
                            else
                            {
                                Debug.Log("NIE MOŻNA tu postawić meepla!");

                            }

                        }                      
                    }
                    else
                    {
                        Vector3 position = hit.point;
                        position.x = Mathf.Round(position.x / 10) * 10; // 10 is the size of a Tile
                        position.z = Mathf.Round(position.z / 10) * 10; // 10 is the size of a Tile
                        //Debug.Log("Position x: " + position.x.ToString() + " Position z: " + position.z.ToString());


                        int[] arrayIndex = TM.getArrayPosition(position.x, position.z);
                        //Debug.Log("Position Array row: " + arrayIndex[0].ToString() + " Position array column: " + arrayIndex[1].ToString());

                        if (tilesOnBoard[arrayIndex[0], arrayIndex[1]] != null)
                        {
                            if (OKButton.activeSelf)
                            {
                                if (arrayIndex[0] == currentlyPlacedTile[0] && arrayIndex[1] == currentlyPlacedTile[1])
                                {
                                    TM.rotateFirstMatchingRotation(ref tilesOnBoard[arrayIndex[0], arrayIndex[1]], ref masks[arrayIndex[0], arrayIndex[1]], arrayIndex, ref tilesOnBoard);
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
                                    Destroy(masks[currentlyPlacedTile[0], currentlyPlacedTile[1]]);
                                    tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]] = null;
                                }
                                OKButton.SetActive(true);
                                MeepleButton.SetActive(true);
                                tilesOnBoard[arrayIndex[0], arrayIndex[1]] = Instantiate(objectToinstantiate, position, Quaternion.identity) as GameObject; // instatiate a prefab on the position where the ray hits the floor. 
                                masks[arrayIndex[0], arrayIndex[1]] = Instantiate(Mask, position, Quaternion.identity) as GameObject;

                                Tile tile = tilesOnBoard[arrayIndex[0], arrayIndex[1]].AddComponent<Tile>();
                                tile.Init(choosenTile.UpTerrain, choosenTile.RightTerrain, choosenTile.DownTerrain, choosenTile.LeftTerrain, position.x, position.z, choosenTile.Material, choosenTile.Mask, choosenTile.TypeCount, choosenTile.Turn, choosenTile.Areas);
                                TM.rotateFirstMatchingRotation(ref tilesOnBoard[arrayIndex[0], arrayIndex[1]], ref masks[arrayIndex[0], arrayIndex[1]], arrayIndex, ref tilesOnBoard);
                                tilesOnBoard[arrayIndex[0], arrayIndex[1]].GetComponent<Renderer>().material = choosenTile.Material;
                                masks[arrayIndex[0], arrayIndex[1]].GetComponent<Renderer>().material = choosenTile.Mask;
                                currentlyPlacedTile = arrayIndex;
                                CM.CheckCamera(arrayIndex);
                                /*
                                possibleMeeple = TM.possibleMeepleAreas(ref tilesOnBoard, currentlyPlacedTile[0], currentlyPlacedTile[1]);
                                foreach (var list in possibleMeeple)
                                {

                                    String result = String.Join(" ", list.edges.Select(item => item.ToString()).ToArray());
                                    Debug.Log("Możliwy obszar:");
                                    Debug.Log(result);

                                }
                                */

                            }
                        }
                    }
                }
            }
        }
    }
}
