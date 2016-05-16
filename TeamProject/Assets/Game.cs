using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Globalization;

public class Game : MonoBehaviour
{
    GameObject[,] tilesOnBoard = new GameObject[200, 200];
    GameObject[,] masks = new GameObject[200, 200];
    GameObject[,] possibleMoves = new GameObject[200, 200];
    int choosenAreaColor = -1;
    int ColorType = -1;
    GameObject[,] meeples = new GameObject[200, 200];
    List<Area> possibleMeeple = new List<Area>();
    public Text redPlayerScore;
    public Text bluePlayerScore;
    public Text greenPlayerScore;
    public Text yellowPlayerScore;
    public Text blackPlayerScore;
    public Text meepleButtonText;

    GameObject gameManager;
    public CameraManager CM;
    public TilesManager TM;
    public PointsCounter PC;
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



    int i;


    public void ButtonClicked()
    {
        // count meeples
        if (placedMeeple == true)
        {
            GM.GetCurrentPlayer().meeples--;
            placedMeeple = false;
        }
        
        if(tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Any(m => m.player != null))
        {
            String result = "TU STAWIAMY MEEPLA"+String.Join(" ", tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Find(m => m.player != null).edges.Select(item => item.ToString()).ToArray());
            Debug.Log(result);

        }
        // Debug.Log("Położono" + currentlyPlacedTile[0] + " " + currentlyPlacedTile[1]);
        // Debug.Log("ZARAZ LICZE PUNKTY");
        PC.countPointsAfterMove(ref tilesOnBoard, currentlyPlacedTile[0], currentlyPlacedTile[1], ref meeples);
        // Debug.Log("PUNKTY GRACZA:" + GM.GetCurrentPlayer().points);
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

        TM.tilesList[i].TypeCount--;
        if (TM.tilesList[i].TypeCount == 0)
        {
            TM.tilesList.RemoveAt(i);
        }

        // Score text
        List<Player> players = GM.GetPlayerListCopy();
        if (players.Any(it => it.color == PlayerColor.RED))
        {
            redPlayerScore.text = GM.GetRedPlayerCopy().points.ToString();
        }
        if (players.Any(it => it.color == PlayerColor.GREEN))
        {
            greenPlayerScore.text = GM.GetGreenPlayerCopy().points.ToString();
        }
        if (players.Any(it => it.color == PlayerColor.BLUE))
        {
            bluePlayerScore.text = GM.GetBluePlayerCopy().points.ToString();
        }
        if (players.Any(it => it.color == PlayerColor.YELLOW))
        {
            yellowPlayerScore.text = GM.GetYellowPlayerCopy().points.ToString();
        }
        if (players.Any(it => it.color == PlayerColor.BLACK))
        {
            blackPlayerScore.text = GM.GetBlackPlayerCopy().points.ToString();
        }

        // Meeple count text
        meepleButtonText.text = GM.GetCurrentPlayer().meeples.ToString();

          
    }

    public void MeepleButtonClicked()
    {
        if (GM.GetCurrentPlayer().meeples > 0 )
        {
            if (currentlyPlacingMeeple == true)
            {
                for (int row = 0; row < possibleMoves.GetLength(0); row++)
                {
                    for (int col = 0; col < possibleMoves.GetLength(1); col++)
                    {
                        if (possibleMoves[row, col] != null)

                            possibleMoves[row, col].GetComponent<Renderer>().enabled = true;
                    }
                }
                OKButton.SetActive(false);
                MeepleButton.SetActive(false);
                currentlyPlacingMeeple = false;

                Destroy(meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]]);
                meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]] = null;
                for (int index = 0; index < tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Count; index++)
                {
                    tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas[index].player = null;
                }

                // tutaj trzeba poprawic bo czasem jest null
                if (choosenAreaColor != -1)
                {
                    tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Find(a => a.colorIndex == ColorType).player = new Player(GM.GetCurrentPlayer().name, GM.GetCurrentPlayer().color, GM.GetCurrentPlayer().rgbaColor);
                    choosenAreaColor = -1;
                }

                if (currentlyPlacedTile != null)
                {
                    Destroy(tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]]);
                    Destroy(masks[currentlyPlacedTile[0], currentlyPlacedTile[1]]);
                    tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]] = null;
                    masks[currentlyPlacedTile[0], currentlyPlacedTile[1]] = null;

                    //choosenTile.Clone(TM.tilesList[i]);
                }
                placedMeeple = false;
            }
            else
            {
                for (int row = 0; row < possibleMoves.GetLength(0); row++)
                {
                    for (int col = 0; col < possibleMoves.GetLength(1); col++)
                    {
                        if (possibleMoves[row, col] != null)

                            possibleMoves[row, col].GetComponent<Renderer>().enabled = false;
                    }
                }
                currentlyPlacingMeeple = true;
                possibleMeeple = TM.possibleMeepleAreas(ref tilesOnBoard, currentlyPlacedTile[0], currentlyPlacedTile[1]);
                //    string koordynaty = "Jesteśmy na klocku x: " + currentlyPlacedTile[0] + " y: " + currentlyPlacedTile[1];
                //    Debug.Log(koordynaty);
                //    Debug.Log("--------------------------dla kliknięcia----------------------------------------");
                //    String result1 = "";
                //    foreach (var l in tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas)
                //    {
                //        result1 += String.Join(" ", l.edges.Select(item => item.ToString()).ToArray());
                //        result1 += " | ";

                //    }
                //    Debug.Log("Istniejące obszary:");
                //    Debug.Log(result1);

                //    Debug.Log("--------------------------możliwe:----------------------------------------------");

                //    foreach (var list in possibleMeeple)
                //    {

                //        String result = String.Join(" ", list.edges.Select(item => item.ToString()).ToArray());
                //        Debug.Log("Możliwy obszar:");
                //        Debug.Log(result);

            }
            //    Debug.Log("--------------------------------------------------------------------------------");
            //}
            //MeepleButton.SetActive(false);
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
        TM.placeTile(ref objectToinstantiate, ref Mask, 100, 100, ref tilesOnBoard, ref masks);
        Tile startTile = tilesOnBoard[100, 100].AddComponent<Tile>();
        startTile.Init(terrainTypes.castle, terrainTypes.road, terrainTypes.grass, terrainTypes.road, 0, 0, TM.CRFR, TM.CRFR_Mask, 1, 0, false, new List<Area>() { 
            new Area { edges = new List<int>() {1,2,3} ,terrain = terrainTypes.castle, colorIndex = 1},
            new Area { edges = new List<int>() {4,12}, terrain = terrainTypes.grass, colorIndex = 2},
            new Area { edges = new List<int>() {5, 0, 11}, terrain = terrainTypes.road, colorIndex = 3}, 
            new Area { edges = new List<int>() {6,7,8,9,10}, terrain = terrainTypes.grass, colorIndex = 4}});
        tilesOnBoard[100, 100].GetComponent<Renderer>().material = TM.CRFR;
        masks[100, 100].GetComponent<Renderer>().material = TM.CRFR_Mask;
        currentlyPlacingTile = false;
        currentlyPlacingMeeple = false;
        placedMeeple = false;
        currentlyPlacedMeeple = 0;

        TM.init(); // Inicjowanie Tiles
    }

    void AI()
    {
        if (currentlyPlacingTile == false)
        {
            if (TM.tilesList.Count > 0)
            {
                int i;
                int j = 0;
                int rolled = 0;

                List<int[]> possiblePositions;
                do
                {
                    i = UnityEngine.Random.Range(0, TM.tilesList.Count);
                    choosenTile.Clone(TM.tilesList[i]);
                    possiblePositions = TM.findMatchingEdges(TM.findSourrounding(ref tilesOnBoard), choosenTile, ref tilesOnBoard);
                    j++;
                } while (possiblePositions.Count == 0 && j < 10);

                NextTileImage.GetComponent<Image>().material = choosenTile.Material;
                rolled = UnityEngine.Random.Range(0, possiblePositions.Count);

                TM.tilesList[i].TypeCount--;
                if (TM.tilesList[i].TypeCount == 0)
                {
                    TM.tilesList.RemoveAt(i);
                }
                currentlyPlacingTile = true;
                playername.text = GM.GetCurrentPlayer().name;

                for (int k = 0; k < possiblePositions.Count; k++)
                {
                    int[] arrPosition = possiblePositions[k];

                    float[] position = TM.getCoordinates(arrPosition[0], arrPosition[1]);
                    if (possibleMoves[arrPosition[0], arrPosition[1]] == null)
                    {
                        //possibleMoves[arrPosition[0], arrPosition[1]] = Instantiate(Selected, new Vector3(position[0], (float)0.1, position[1]), Quaternion.identity) as GameObject;

                        if (k == rolled)
                        {
                            tilesOnBoard[arrPosition[0], arrPosition[1]] = Instantiate(objectToinstantiate, new Vector3(position[0], 0, position[1]), Quaternion.identity) as GameObject; // instatiate a prefab on the position where the ray hits the floor.                         
                            Tile tile = tilesOnBoard[arrPosition[0], arrPosition[1]].AddComponent<Tile>();
                            tile.Init(choosenTile.UpTerrain, choosenTile.RightTerrain, choosenTile.DownTerrain, choosenTile.LeftTerrain, position[0], position[1], choosenTile.Material, choosenTile.Mask, choosenTile.TypeCount, choosenTile.Turn, choosenTile.Plus, choosenTile.Areas);
                            masks[arrPosition[0], arrPosition[1]] = Instantiate(Mask, new Vector3(position[0], (float)0.1, position[1]), Quaternion.identity) as GameObject;
                            TM.rotateFirstMatchingRotation(ref tilesOnBoard[arrPosition[0], arrPosition[1]], ref masks[arrPosition[0], arrPosition[1]], arrPosition, ref tilesOnBoard);

                            tilesOnBoard[arrPosition[0], arrPosition[1]].GetComponent<Renderer>().material = choosenTile.Material;
                            masks[arrPosition[0], arrPosition[1]].GetComponent<Renderer>().material = choosenTile.Mask;

                            currentlyPlacedTile = arrPosition;
                            CM.CheckCamera(arrPosition);
                        }
                    }
                }
                OKButton.SetActive(true);
                MeepleButton.SetActive(true);
            }
        }
        // MEEEPLEEE
        if (currentlyPlacingMeeple == true)
        {
            // roll position
            int rolledPosition = UnityEngine.Random.Range(0, possibleMeeple.Count);
            int selectedPosition = possibleMeeple[rolledPosition].meeplePlacementIndex;
            currentlyPlacingMeeple = false;
            
        
            tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Collider>().enabled = false;
            if (tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Find(a => a.meeplePlacementIndex == selectedPosition) != null)
            {
                int ed = tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Find(a => a.meeplePlacementIndex == selectedPosition).meeplePlacementIndex;
                
                Debug.Log("Wykrywam obszar o indeksie: " + ed + "  //selected position: " + selectedPosition);

            }
            if (possibleMeeple.Any(a => a.meeplePlacementIndex == selectedPosition))
            {
                if (meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]] != null)
                {
                    Destroy(meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]]);
                    meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]] = null;
                    for (int index = 0; index < tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Count; index++)
                    {
                        tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas[index].player = null;
                    }
                    tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Find(a => a.meeplePlacementIndex == selectedPosition).player = new Player(GM.GetCurrentPlayer().name, GM.GetCurrentPlayer().color, GM.GetCurrentPlayer().rgbaColor);
                    placedMeeple = false;
                }
                Debug.Log("TAK, tu można postawić meepla!");

                if (possibleMeeple.Find(a => a.meeplePlacementIndex == selectedPosition).terrain == terrainTypes.grass)
                {
                    meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]] = Instantiate(FarmerMeeple, TM.GetMeeplePosition(selectedPosition, TM.getCoordinates(currentlyPlacedTile[0], currentlyPlacedTile[1])), Quaternion.identity) as GameObject;
                    meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]].transform.Translate(new Vector3(0, (float)0.44, 0));
                    meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]].transform.Rotate(new Vector3(0, 0, 90));
                    meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Renderer>().material.color = GM.GetCurrentPlayer().rgbaColor;
                    tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Find(a => a.meeplePlacementIndex == selectedPosition).player = new Player(GM.GetCurrentPlayer().name, GM.GetCurrentPlayer().color, GM.GetCurrentPlayer().rgbaColor);

                    placedMeeple = true;
                }
                else
                {
                    meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]] = Instantiate(StandingMeeple, TM.GetMeeplePosition(selectedPosition, TM.getCoordinates(currentlyPlacedTile[0], currentlyPlacedTile[1])), Quaternion.identity) as GameObject;
                    meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]].transform.Translate(new Vector3(0, (float)2.13, 0));
                    meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]].transform.Rotate(new Vector3(0, 90, 0));
                    meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Renderer>().material.color = GM.GetCurrentPlayer().rgbaColor;
                    tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Find(a => a.meeplePlacementIndex == selectedPosition).player = new Player(GM.GetCurrentPlayer().name, GM.GetCurrentPlayer().color, GM.GetCurrentPlayer().rgbaColor);

                    placedMeeple = true;
                }
            }
            else
            {
                Debug.Log("NIE MOŻNA tu postawić meepla!");
            }
        }
    }
    
    

    // Update is called once per frame
    void Update()
    {
        // Sprawdzamy czy to komputer
        if (GM.GetCurrentPlayer().type == PlayerType.AI)
        {
            AI(); // Funkcja wyzej tymczasowo
                
        }
        else if (currentlyPlacingTile == false)
        {
            if (TM.tilesList.Count > 0)
            {
                //int i;
                int j = 0;
                List<int[]> possiblePositions;
                do
                {
                    i = UnityEngine.Random.Range(0, TM.tilesList.Count);
                    choosenTile = new Tile();
                    choosenTile.Clone(TM.tilesList[i]);
                    possiblePositions = TM.findMatchingEdges(TM.findSourrounding(ref tilesOnBoard), choosenTile, ref tilesOnBoard);
                    j++;
                } while (possiblePositions.Count == 0 && j < 10);

                NextTileImage.GetComponent<Image>().material = choosenTile.Material;

                /*
                TM.tilesList[i].TypeCount--;
                if (TM.tilesList[i].TypeCount == 0)
                {
                    TM.tilesList.RemoveAt(i);
                }
                */


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
        else // USTAWIANIE MEEPLE'A
        {
            myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(myRay, out hit))
            {
                if (Input.GetMouseButtonUp(0))
                {
                    if (currentlyPlacingMeeple == true)
                    {
                        tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Collider>().enabled = false;                      
                        //Debug.Log(hit.transform.GetInstanceID());
                        if (hit.transform.GetInstanceID() == masks[currentlyPlacedTile[0], currentlyPlacedTile[1]].transform.GetInstanceID())
                        {
                            Texture2D tex = hit.collider.GetComponent<MeshRenderer>().material.mainTexture as Texture2D;
                            Vector2 pixelUV = hit.textureCoord;
                            float uvX = pixelUV.x * tex.width;
                            float uvY = pixelUV.y * tex.height;

                            Color hitColor = tex.GetPixel((int)uvX, (int)uvY);
                            ColorType = TM.ColorClassify(hitColor.r);
                            //Debug.Log(ColorType.ToString() + " - typ koloru");
                            //Debug.Log(hitColor.ToString());

                            
                            if (tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Find(a => a.colorIndex == ColorType) != null)
                            {
                                List<int> ed = tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Find(a => a.colorIndex == ColorType).edges;
                                string resulthahahah = String.Join(" ", ed.Select(item => item.ToString()).ToArray());
                                Debug.Log("Wykrywam obszar o krawędziach: " + resulthahahah);

                            }
                            
                            if (possibleMeeple.Any(a => a.colorIndex == ColorType))
                            {
                                if (ColorType == choosenAreaColor)
                                {
                                    Debug.Log("Usuwam meeple który już był tutaj!");

                                    Destroy(meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]]);
                                    meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]] = null;

                                    int element = tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.FindIndex(a => a.colorIndex == ColorType);
                                    tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas[element].player = null;

                                    choosenAreaColor = -1;
                                    placedMeeple = false;
                                }
                                else
                                {
                                    if (meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]] != null)
                                    {
                                        Destroy(meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]]);
                                        meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]] = null;
                                        for (int index = 0; index < tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Count; index++)
                                        {
                                            tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas[index].player = null;
                                        }
                                        tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Find(a => a.colorIndex == ColorType).player = new Player(GM.GetCurrentPlayer().name, GM.GetCurrentPlayer().color, GM.GetCurrentPlayer().rgbaColor);
                                        
                                        choosenAreaColor = -1;
                                        
                                        placedMeeple = false;
                                    }
                                    Debug.Log("TAK, tu można postawić meepla!");
                                    choosenAreaColor = possibleMeeple.Find(a => a.colorIndex == ColorType).colorIndex;
                                    int selectedPosition = possibleMeeple.Find(a => a.colorIndex == ColorType).meeplePlacementIndex;
                                    Debug.Log(selectedPosition);
                                    placedMeeple = true;
                                    if (possibleMeeple.Find(a => a.colorIndex == ColorType).terrain == terrainTypes.grass)
                                    {
                                        //meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]] = Instantiate(FarmerMeeple, TM.GetMeeplePosition(selectedPosition, TM.getCoordinates(currentlyPlacedTile[0], currentlyPlacedTile[1])), Quaternion.identity) as GameObject;
                                        meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]] = Instantiate(FarmerMeeple, hit.point, Quaternion.identity) as GameObject;

                                        meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]].transform.Translate(new Vector3(0, (float)0.44, 0));
                                        meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]].transform.Rotate(new Vector3(0, 0, 90));
                                        meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Renderer>().material.color = GM.GetCurrentPlayer().rgbaColor;
                                        tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Find(a => a.colorIndex == ColorType).player = new Player(GM.GetCurrentPlayer().name, GM.GetCurrentPlayer().color, GM.GetCurrentPlayer().rgbaColor);
                                           
                                       
                                    }
                                    else
                                    {
                                        //meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]] = Instantiate(StandingMeeple, TM.GetMeeplePosition(selectedPosition, TM.getCoordinates(currentlyPlacedTile[0], currentlyPlacedTile[1])), Quaternion.identity) as GameObject;
                                        meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]] = Instantiate(StandingMeeple, hit.point, Quaternion.identity) as GameObject;

                                        meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]].transform.Translate(new Vector3(0, (float)2.13, 0));
                                        meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]].transform.Rotate(new Vector3(0, 90, 0));
                                        meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Renderer>().material.color = GM.GetCurrentPlayer().rgbaColor;
                                        tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Find(a => a.colorIndex == ColorType).player = new Player(GM.GetCurrentPlayer().name, GM.GetCurrentPlayer().color, GM.GetCurrentPlayer().rgbaColor);
                                        
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
                                    String result3 = "";
                                    foreach (var l in tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas)
                                    {
                                        result3 += String.Join(" ", l.edges.Select(item => item.ToString()).ToArray());
                                        result3 += " | ";
                                    }
                                    Debug.Log(result3);
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
                                    masks[currentlyPlacedTile[0], currentlyPlacedTile[1]] = null;

                                    //choosenTile.Clone(TM.tilesList[i]);
                                }
                                OKButton.SetActive(true);
                                MeepleButton.SetActive(true);
                                Debug.Log("#################################");

                                choosenTile.Clone(TM.tilesList[i]);
                                tilesOnBoard[arrayIndex[0], arrayIndex[1]] = Instantiate(objectToinstantiate, position, Quaternion.identity) as GameObject; // instatiate a prefab on the position where the ray hits the floor. 
                                masks[arrayIndex[0], arrayIndex[1]] = Instantiate(Mask, position, Quaternion.identity) as GameObject;

                                Tile tile = tilesOnBoard[arrayIndex[0], arrayIndex[1]].AddComponent<Tile>();
                                Debug.Log("choosen tile:");
                                String result9 = "";
                                foreach (var l in choosenTile.Areas)
                                {
                                    result9 += String.Join(" ", l.edges.Select(item => item.ToString()).ToArray());
                                    result9 += " | ";
                                }
                                Debug.Log(result9);


                                tile.Init(choosenTile.UpTerrain, choosenTile.RightTerrain, choosenTile.DownTerrain, choosenTile.LeftTerrain, position.x, position.z, choosenTile.Material, choosenTile.Mask, choosenTile.TypeCount, choosenTile.Turn, choosenTile.Plus, choosenTile.Areas);
                                TM.rotateFirstMatchingRotation(ref tilesOnBoard[arrayIndex[0], arrayIndex[1]], ref masks[arrayIndex[0], arrayIndex[1]], arrayIndex, ref tilesOnBoard);

                                String result4 = "";
                                foreach (var l in tilesOnBoard[arrayIndex[0], arrayIndex[1]].GetComponent<Tile>().Areas)
                                {
                                    result4 += String.Join(" ", l.edges.Select(item => item.ToString()).ToArray());
                                    result4 += " | ";

                                }
                                Debug.Log(result4);


                                tilesOnBoard[arrayIndex[0], arrayIndex[1]].GetComponent<Renderer>().material = choosenTile.Material;
                                masks[arrayIndex[0], arrayIndex[1]].GetComponent<Renderer>().material = choosenTile.Mask;
                                currentlyPlacedTile = arrayIndex;
                                CM.CheckCamera(arrayIndex);
                            }
                        }
                    }
                }
            }
        }
    }
}
