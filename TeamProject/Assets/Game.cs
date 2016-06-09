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
    GameObject[,] possibleMoves = new GameObject[200, 200];
    GameObject[,] meeples = new GameObject[200, 200];
    List<Area> possibleMeeple = new List<Area>();

    GameObject[,] masks = new GameObject[200, 200];

    int choosenAreaColor = -1;
    int ColorType = -1;
    public Text redPlayerScore;
    public Text bluePlayerScore;
    public Text greenPlayerScore;
    public Text yellowPlayerScore;
    public Text blackPlayerScore;
    public Text meepleButtonText;

    public CameraManager CM;
    public TilesManager TM;
    public PointsCounter PC;
    public AIManager AM;

    GameManager GM;
    public GameObject Selected;
    public GameObject NextTileImage;
    public Text playername;
    public Text tilesLeftText;
    private bool currentlyPlacingTile;
    private bool currentlyPlacingMeeple;
    private bool placedMeeple;
    private int[] currentlyPlacedTile;
    private int[] lastPlacedTile;
    private Tile choosenTile;

    public GameObject OKButton;
    public GameObject MeepleButton;
    public GameObject Mask;
    public GameObject StandingMeeple;
    public GameObject FarmerMeeple;
    public GameObject Glow;
    GameObject currentGlow;

    private List<GameObject> placedMeepleObject;
    private Area placedMeepleArea;
    private int currentlyPlacedMeeple;
    Ray myRay;
    RaycastHit hit;
    public GameObject objectToinstantiate;
    public int currentNbrTiles;
    // int tilesLeft = 71;
    int tilesLeft = 15;
    int currentTileIndex;
    bool finished = true;

    public AudioClip impact;
    AudioSource audio;

    public NetworkView networkView;
    public bool round1;
    public int clientNumber;
    public int wait;
    public bool[] connections;
    public int actualTurn;
    public int SerwerAns;

    public void AcceptButtonClicked()
    {
        // FloatingTextController.Initialize();
        // FloatingTextController.CreateFloatingText2("OK",0, 10,Color.blue);
        SerwerAns = 0;
        if (Network.connections.Length == 0)
        {

            tilesLeft--;
            tilesLeftText.text = tilesLeft.ToString();
            Debug.Log("połozono x :" + currentlyPlacedTile[0] + " y: " + currentlyPlacedTile[1]);
            if (placedMeeple == true)
            {
                GM.GetCurrentPlayer().meeples--;
                placedMeeple = false;
            }

            if (tilesLeft > 0)
            {
                PC.countPointsAfterMove(ref tilesOnBoard, currentlyPlacedTile[0], currentlyPlacedTile[1], ref meeples);
            }
            else
            {
                PC.countPointsAfterMove(ref tilesOnBoard, currentlyPlacedTile[0], currentlyPlacedTile[1], ref meeples);
                //find all remaining meeples
                for (int x = 0; x < meeples.GetLength(0); x += 1)
                {
                    for (int y = 0; y < meeples.GetLength(1); y += 1)
                    {
                        if (meeples[x, y] != null)
                        {
                            PC.countPointsAfterMove(ref tilesOnBoard, x, y, ref meeples, true);
                        }
                    }
                }
            }
            if (GM.GetCurrentPlayer().glow != null)
            {
                Destroy(GM.GetCurrentPlayer().glow);
            }
            lastPlacedTile[0] = currentlyPlacedTile[0];
            lastPlacedTile[1] = currentlyPlacedTile[1];

            GM.GetCurrentPlayer().glow = Instantiate(Glow, new Vector3(TM.getCoordinates(lastPlacedTile[0], lastPlacedTile[1])[0], (float)0.1, TM.getCoordinates(lastPlacedTile[0], lastPlacedTile[1])[1]), Quaternion.identity) as GameObject;
            GM.GetCurrentPlayer().glow.GetComponentInChildren<ParticleSystem>().startColor = GM.GetCurrentPlayer().rgbaColor;


            //lastPlacedTile[0] = currentlyPlacedTile[0];
            // lastPlacedTile[1] = currentlyPlacedTile[1];
            // currentGlow = Instantiate(Glow, new Vector3(TM.getCoordinates(lastPlacedTile[0], lastPlacedTile[1])[0], (float)0.1, TM.getCoordinates(lastPlacedTile[0], lastPlacedTile[1])[1]), Quaternion.identity) as GameObject;

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

            TM.tilesList.RemoveAt(currentTileIndex);

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
            meepleButtonText.text = GM.GetCurrentPlayer().meeples.ToString();
        }
        else
        {
            networkView.RPC("LanButtonClick", RPCMode.All);
            if (placedMeeple == true)
            {
                GM.GetCurrentPlayer().meeples--;
                placedMeeple = false;
            }
            if (tilesLeft > 1)
            {
                PC.countPointsAfterMove(ref tilesOnBoard, currentlyPlacedTile[0], currentlyPlacedTile[1], ref meeples);
            }
            else
            {
                PC.countPointsAfterMove(ref tilesOnBoard, currentlyPlacedTile[0], currentlyPlacedTile[1], ref meeples);
                //find all remaining meeples
                for (int x = 0; x < meeples.GetLength(0); x += 1)
                {
                    for (int y = 0; y < meeples.GetLength(1); y += 1)
                    {
                        if (meeples[x, y] != null)
                        {
                            PC.countPointsAfterMove(ref tilesOnBoard, x, y, ref meeples, true);
                        }
                    }
                }
            }

            networkView.RPC("Something", RPCMode.All);

            if (CM.cameraCheck)
            {
                CM.UpdateCamera();
                CM.cameraCheck = false;
            }
            currentlyPlacedMeeple++;

            networkView.RPC("Something2", RPCMode.All);

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
            meepleButtonText.text = GM.GetCurrentPlayer().meeples.ToString();


            networkView.RPC("Turn_change", RPCMode.All);
        }
    }
    public void MeepleButtonClicked()
    {
        if (GM.GetCurrentPlayer().meeples > 0)
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
            else if (Network.connections.Length == 0)
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
            else
            {
                networkView.RPC("meepleclick", RPCMode.All);
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
        //lan[
        wait = 0;
        connections = new bool[4];
        connections = GM.Connections;
        if (Network.peerType == NetworkPeerType.Server)
        {
            for (int i = 0; i < 4; i++)
                if (connections[i])
                {
                    wait++;
                    Debug.Log("Connections[" + i + "]:" + connections[i]);
                }
            round1 = true;
        }
        else
            round1 = false;
        clientNumber = GM.Client_number;
        networkView = GetComponent<NetworkView>();
        //]

        currentlyPlacedTile = new int[2];
        lastPlacedTile = new int[2];
        currentNbrTiles = 0;
        TM.placeTile(ref objectToinstantiate, ref Mask, 100, 100, ref tilesOnBoard, ref masks);
        Tile startTile = tilesOnBoard[100, 100].AddComponent<Tile>();
        startTile.Init(1, terrainTypes.castle, terrainTypes.road, terrainTypes.grass, terrainTypes.road, 0, 0, TM.CRFR, TM.CRFR_Mask, 0, false, new List<Area>() { 
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
        audio = GetComponent<AudioSource>();

        TM.init(); // Inicjowanie Tiles
        //tilesLeft = TM.tilesList.Count;

        //lan[
        Debug.Log("client_number:" + clientNumber);
        if (Network.peerType == NetworkPeerType.Client)
            networkView.RPC("Gameconn", RPCMode.All);
        //]
    }

    void AIRand()
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

                // Probability check
                float prob = AM.Probability(TM.tilesList, choosenTile);
                Debug.Log("Prawdopodobieństwo wylosowania wylosowanego tile'a: " + prob);

                NextTileImage.GetComponent<Image>().material = choosenTile.Material;
                rolled = UnityEngine.Random.Range(0, possiblePositions.Count);

                TM.tilesList.RemoveAt(i);

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
                            tile.Init(choosenTile.IdNumber, choosenTile.UpTerrain, choosenTile.RightTerrain, choosenTile.DownTerrain, choosenTile.LeftTerrain, position[0], position[1], choosenTile.Material, choosenTile.Mask, choosenTile.Rotation, choosenTile.Plus, choosenTile.Areas);
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

                //  Debug.Log("Wykrywam obszar o indeksie: " + ed + "  //selected position: " + selectedPosition);

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
                //    Debug.Log("TAK, tu można postawić meepla!");

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
                //   Debug.Log("NIE MOŻNA tu postawić meepla!");
            }
        }
    }
    void AI()
    {
        finished = false;
        Debug.Log("wchodze");

        if (TM.tilesList.Count > 0)
        {
            int j = 0;

            List<int[]> possiblePositions;
            do
            {
                currentTileIndex = UnityEngine.Random.Range(0, TM.tilesList.Count);
                choosenTile.Clone(TM.tilesList[currentTileIndex]);
                possiblePositions = TM.findMatchingEdges(TM.findSourrounding(ref tilesOnBoard), choosenTile, ref tilesOnBoard);
                j++;
            } while (possiblePositions.Count == 0 && j < 10);

            NextTileImage.GetComponent<Image>().material = choosenTile.Material;

            // FIX na potem - lista w expectimax musi byc mniejsza o tego klocka
            //TM.tilesList.RemoveAt(currentTileIndex);

            currentlyPlacingTile = true;
            playername.text = GM.GetCurrentPlayer().name;
            //foreach (var t in TM.tilesList)
            //{
            //    Debug.Log(t.IdNumber);
            //

            //audio.PlayOneShot(impact, 1.0F);

            Move move = AM.Expectimax(tilesOnBoard, TM.tilesList, choosenTile, 1, GM.GetPlayerListCopy());

            if ((object)move.tile == null)
            {
                Debug.Log("Expectimax wyrzucił nulla :(");
            }

            choosenTile.Clone(move.tile); // Material is still in choosen tile Clone() copies all other params
            float[] pos = TM.getCoordinates(move.x, move.y);

            Debug.Log(move.x + " " + move.y);//
            tilesOnBoard[move.x, move.y] = Instantiate(objectToinstantiate, new Vector3(pos[0], 0.13f, pos[1]), Quaternion.identity) as GameObject;// instatiate a prefab on the position where the ray hits the floor.                         
            Tile tile = tilesOnBoard[move.x, move.y].AddComponent<Tile>();
            tile.Init(choosenTile.IdNumber, choosenTile.UpTerrain, choosenTile.RightTerrain, choosenTile.DownTerrain, choosenTile.LeftTerrain, move.x, move.y, choosenTile.Material, choosenTile.Mask, choosenTile.Rotation, choosenTile.Plus, choosenTile.Areas);
            masks[move.x, move.y] = Instantiate(Mask, new Vector3(pos[0], (float)0.1, pos[1]), Quaternion.identity) as GameObject;

            //Debug.Log("rotacja " + move.tile.Rotation);//

            TM.rotateXTimes(move.tile.Rotation, ref tilesOnBoard[move.x, move.y], ref masks[move.x, move.y]); // rotates materials to match other params

            tilesOnBoard[move.x, move.y].GetComponent<Renderer>().material = choosenTile.Material;
            masks[move.x, move.y].GetComponent<Renderer>().material = choosenTile.Mask;

            int[] movePos = { move.x, move.y };
            currentlyPlacedTile = movePos;
            CM.CheckCamera(movePos);

            if (move.tile.Areas.Find(a => a.player != null) == null)
            {
                Debug.Log("Brak Meepla na klocku");
            }
            if (move.tile.Areas.Find(a => a.player != null) != null)
            {
                int selectedPosition = move.tile.Areas.Find(a => a.player != null).meeplePlacementIndex;
                Debug.Log(" selected pos: " + selectedPosition);

                // meeple
                if (move.tile.Areas.Find(a => a.player != null).terrain == terrainTypes.grass)
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

            Debug.Log("wychodze");
            AcceptButtonClicked();
            finished = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        // click enter to accept turn
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (OKButton.activeSelf)
            {
                AcceptButtonClicked();
            }
        }

        // Tryb zwykły - nie sieciowy
        if (Network.connections.Length == 0)
        {
            // Sprawdzamy czy to komputer
            if (GM.GetCurrentPlayer().type == PlayerType.AI && finished)
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
                        currentTileIndex = UnityEngine.Random.Range(0, TM.tilesList.Count);
                        choosenTile = gameObject.AddComponent<Tile>();
                        choosenTile.Clone(TM.tilesList[currentTileIndex]);
                        possiblePositions = TM.findMatchingEdges(TM.findSourrounding(ref tilesOnBoard), choosenTile, ref tilesOnBoard);
                        j++;
                    } while (possiblePositions.Count == 0 && j < 10);

                    //Probability check
                    float prob = AM.Probability(TM.tilesList, choosenTile);
                    Debug.Log("Prawdopodobieństwo wylosowania wylosowanego tile'a: " + prob);

                    NextTileImage.GetComponent<Image>().material = choosenTile.Material;

                    if (choosenTile == null)
                    {
                        Debug.Log("wylosowany null!");
                        Debug.Log(choosenTile.Material.name);
                    }
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
                            possibleMoves[arrPosition[0], arrPosition[1]] = Instantiate(Selected, new Vector3(position[0], (float)0.13, position[1]), Quaternion.identity) as GameObject;
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
                                    //Debug.Log("Wykrywam obszar o krawędziach: " + resulthahahah);
                                }

                                if (possibleMeeple.Any(a => a.colorIndex == ColorType))
                                {
                                    if (ColorType == choosenAreaColor)
                                    {
                                        //Debug.Log("Usuwam meeple który już był tutaj!");

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
                                            tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Find(a => a.colorIndex == ColorType).player
                                                = new Player(GM.GetCurrentPlayer().name, GM.GetCurrentPlayer().color, GM.GetCurrentPlayer().rgbaColor);
                                            choosenAreaColor = -1;
                                            placedMeeple = false;
                                        }
                                        //Debug.Log("TAK, tu można postawić meepla!");
                                        choosenAreaColor = possibleMeeple.Find(a => a.colorIndex == ColorType).colorIndex;
                                        int selectedPosition = possibleMeeple.Find(a => a.colorIndex == ColorType).meeplePlacementIndex;
                                        //Debug.Log(selectedPosition);
                                        placedMeeple = true;
                                        if (possibleMeeple.Find(a => a.colorIndex == ColorType).terrain == terrainTypes.grass)
                                        {
                                            //meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]] = Instantiate(FarmerMeeple, TM.GetMeeplePosition(selectedPosition, TM.getCoordinates(currentlyPlacedTile[0], currentlyPlacedTile[1])), Quaternion.identity) as GameObject;
                                            meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]] = Instantiate(FarmerMeeple, hit.point, Quaternion.identity) as GameObject;
                                            meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]].transform.Translate(new Vector3(0, (float)0.44, 0));
                                            meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]].transform.Rotate(new Vector3(0, 0, 90));
                                            meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Renderer>().material.color = GM.GetCurrentPlayer().rgbaColor;
                                            tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Find(a => a.colorIndex == ColorType).player
                                                = new Player(GM.GetCurrentPlayer().name, GM.GetCurrentPlayer().color, GM.GetCurrentPlayer().rgbaColor);
                                        }
                                        else
                                        {
                                            //meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]] = Instantiate(StandingMeeple, TM.GetMeeplePosition(selectedPosition, TM.getCoordinates(currentlyPlacedTile[0], currentlyPlacedTile[1])), Quaternion.identity) as GameObject;
                                            meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]] = Instantiate(StandingMeeple, hit.point, Quaternion.identity) as GameObject;

                                            meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]].transform.Translate(new Vector3(0, (float)2.13, 0));
                                            meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]].transform.Rotate(new Vector3(0, 90, 0));
                                            meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Renderer>().material.color = GM.GetCurrentPlayer().rgbaColor;
                                            tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Find(a => a.colorIndex == ColorType).player
                                                = new Player(GM.GetCurrentPlayer().name, GM.GetCurrentPlayer().color, GM.GetCurrentPlayer().rgbaColor);
                                        }
                                    }
                                }
                                else
                                {
                                    //   Debug.Log("NIE MOŻNA tu postawić meepla!");
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
                                        // audio.PlayOneShot(impact, 1.0F);

                                        //  Debug.Log(result3);
                                        //rotateClockwise90(ref tilesOnBoard[arrayIndex[0], arrayIndex[1]]);              
                                    }
                                }
                                else
                                {
                                    // Debug.Log("A tile is already at this position");
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
                                    //Debug.Log("#################################");

                                    choosenTile.Clone(TM.tilesList[currentTileIndex]);
                                    tilesOnBoard[arrayIndex[0], arrayIndex[1]] = Instantiate(objectToinstantiate, position, Quaternion.identity) as GameObject; // instatiate a prefab on the position where the ray hits the floor. 
                                    masks[arrayIndex[0], arrayIndex[1]] = Instantiate(Mask, position, Quaternion.identity) as GameObject;

                                    //audio.PlayOneShot(impact, 1.0F);


                                    Tile tile = tilesOnBoard[arrayIndex[0], arrayIndex[1]].AddComponent<Tile>();
                                    //Debug.Log("choosen tile:");
                                    String result9 = "";
                                    foreach (var l in choosenTile.Areas)
                                    {
                                        result9 += String.Join(" ", l.edges.Select(item => item.ToString()).ToArray());
                                        result9 += " | ";
                                    }
                                    //Debug.Log(result9);


                                    tile.Init(choosenTile.IdNumber, choosenTile.UpTerrain, choosenTile.RightTerrain, choosenTile.DownTerrain, choosenTile.LeftTerrain, position.x, position.z, choosenTile.Material, choosenTile.Mask, choosenTile.Rotation, choosenTile.Plus, choosenTile.Areas);
                                    TM.rotateFirstMatchingRotation(ref tilesOnBoard[arrayIndex[0], arrayIndex[1]], ref masks[arrayIndex[0], arrayIndex[1]], arrayIndex, ref tilesOnBoard);

                                    String result4 = "";
                                    foreach (var l in tilesOnBoard[arrayIndex[0], arrayIndex[1]].GetComponent<Tile>().Areas)
                                    {
                                        result4 += String.Join(" ", l.edges.Select(item => item.ToString()).ToArray());
                                        result4 += " | ";

                                    }
                                    //Debug.Log(result4);(float)0.1


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
        else if (round1 == true)
        {
            Debug.Log(wait);
            if (wait == 0)
                round1 = false;
        }
        else
            if (clientNumber == actualTurn)
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
                        List<int[]> possiblePositions;
                        if (Network.peerType != NetworkPeerType.Client || SerwerAns == 0)
                        {
                            SerwerAns = 1;
                            networkView.RPC("debuglan", RPCMode.All, clientNumber);
                            //int i;
                            int j = 0;

                            do
                            {
                                networkView.RPC("RandomTitle", RPCMode.All);
                                possiblePositions = TM.findMatchingEdges(TM.findSourrounding(ref tilesOnBoard), choosenTile, ref tilesOnBoard);
                                j++;
                            } while (possiblePositions.Count == 0 && j < 10);
                        }
                        if (Network.peerType != NetworkPeerType.Client || SerwerAns == 2)
                        {
                            SerwerAns = 3;
                            networkView.RPC("SendTitle", RPCMode.All);
                        }
                        if (Network.peerType != NetworkPeerType.Client || SerwerAns == 4)
                        {
                            SerwerAns = 5;
                            currentlyPlacingTile = true;
                            /*
                            TM.tilesList[i].TypeCount--;
                            if (TM.tilesList[i].TypeCount == 0)
                            {
                                TM.tilesList.RemoveAt(i);
                            }
                            */
                            possiblePositions = TM.findMatchingEdges(TM.findSourrounding(ref tilesOnBoard), choosenTile, ref tilesOnBoard);
                            foreach (var arrPosition in possiblePositions)
                            {
                                //networkView.RPC("ShowPosition", RPCMode.All, arrPosition[0], arrPosition[1]);
                                float[] position = TM.getCoordinates(arrPosition[0], arrPosition[1]);
                                if (possibleMoves[arrPosition[0], arrPosition[1]] == null)
                                {
                                    possibleMoves[arrPosition[0], arrPosition[1]]
                                        = Instantiate(Selected, new Vector3(position[0], (float)0.1, position[1]), Quaternion.identity) as GameObject;
                                }
                            }
                            OKButton.SetActive(false);
                            MeepleButton.SetActive(false);
                        }
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


                                    //if (tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Find(a => a.colorIndex == ColorType) != null)
                                    //{
                                    //    List<int> ed = tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Find(a => a.colorIndex == ColorType).edges;
                                    //    string resulthahahah = String.Join(" ", ed.Select(item => item.ToString()).ToArray());
                                    //    Debug.Log("Wykrywam obszar o krawędziach: " + resulthahahah);

                                    //}

                                    if (possibleMeeple.Any(a => a.colorIndex == ColorType))
                                    {
                                        if (ColorType == choosenAreaColor)
                                        {
                                            networkView.RPC("meeple", RPCMode.All, -1, hit.point.x, hit.point.y, hit.point.z);
                                            //   Debug.Log("Usuwam meeple który już był tutaj!");
                                            /*
                                           
                                            */
                                            choosenAreaColor = -1;
                                            placedMeeple = false;

                                        }
                                        else
                                        {
                                            if (meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]] != null)
                                            {
                                                networkView.RPC("meeple", RPCMode.All, 0, hit.point.x, hit.point.y, hit.point.z);

                                                //Destroy(meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]]);
                                                //meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]] = null;
                                                //for (int index = 0; index < tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Count; index++)
                                                //{
                                                //    tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas[index].player = null;
                                                //}
                                                //tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Find(a => a.colorIndex == ColorType).player
                                                //    = new Player(GM.GetCurrentPlayer().name, GM.GetCurrentPlayer().color, GM.GetCurrentPlayer().rgbaColor);
                                                //choosenAreaColor = -1;
                                                //placedMeeple = false;
                                            }

                                            //   Debug.Log("TAK, tu można postawić meepla!");
                                            choosenAreaColor = possibleMeeple.Find(a => a.colorIndex == ColorType).colorIndex;
                                            int selectedPosition = possibleMeeple.Find(a => a.colorIndex == ColorType).meeplePlacementIndex;
                                            //   Debug.Log(selectedPosition);
                                            placedMeeple = true;
                                            if (possibleMeeple.Find(a => a.colorIndex == ColorType).terrain == terrainTypes.grass)
                                            {
                                                networkView.RPC("meeple", RPCMode.All, 1, hit.point.x, hit.point.y, hit.point.z);
                                                //meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]] = Instantiate(FarmerMeeple, TM.GetMeeplePosition(selectedPosition, TM.getCoordinates(currentlyPlacedTile[0], currentlyPlacedTile[1])), Quaternion.identity) as GameObject;
                                            }
                                            else
                                            {
                                                networkView.RPC("meeple", RPCMode.All, 2, hit.point.x, hit.point.y, hit.point.z);
                                                //meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]] = Instantiate(StandingMeeple, TM.GetMeeplePosition(selectedPosition, TM.getCoordinates(currentlyPlacedTile[0], currentlyPlacedTile[1])), Quaternion.identity) as GameObject;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //   Debug.Log("NIE MOŻNA tu postawić meepla!");
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
                                            if (Network.connections.Length != 0)
                                                networkView.RPC("Rotate", RPCMode.All, arrayIndex[0], arrayIndex[1], arrayIndex);
                                            else
                                            {
                                                TM.rotateFirstMatchingRotation(ref tilesOnBoard[arrayIndex[0], arrayIndex[1]], ref masks[arrayIndex[0], arrayIndex[1]], arrayIndex, ref tilesOnBoard);
                                            }
                                            //String result3 = "";
                                            //foreach (var l in tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas)
                                            //{
                                            //    result3 += String.Join(" ", l.edges.Select(item => item.ToString()).ToArray());
                                            //    result3 += " | ";
                                            //}
                                            //  Debug.Log(result3);
                                            //rotateClockwise90(ref tilesOnBoard[arrayIndex[0], arrayIndex[1]]);              
                                        }
                                    }
                                    else
                                    {
                                        // Debug.Log("A tile is already at this position");
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
                                        //  Debug.Log("#################################");

                                        choosenTile.Clone(TM.tilesList[currentTileIndex]);
                                        if (Network.connections.Length != 0)
                                            networkView.RPC("PutTitle", RPCMode.All, arrayIndex[0], arrayIndex[1], position.x, position.y, position.z, arrayIndex);
                                        else
                                        {
                                            tilesOnBoard[arrayIndex[0], arrayIndex[1]] = Instantiate(objectToinstantiate, position, Quaternion.identity) as GameObject; // instatiate a prefab on the position where the ray hits the floor. 
                                            masks[arrayIndex[0], arrayIndex[1]] = Instantiate(Mask, position, Quaternion.identity) as GameObject;

                                            Tile tile = tilesOnBoard[arrayIndex[0], arrayIndex[1]].AddComponent<Tile>();
                                            //    Debug.Log("choosen tile:");

                                            String result9 = "";
                                            foreach (var l in choosenTile.Areas)
                                            {
                                                result9 += String.Join(" ", l.edges.Select(item => item.ToString()).ToArray());
                                                result9 += " | ";
                                            }
                                            //   Debug.Log(result9);


                                            tile.Init(choosenTile.IdNumber, choosenTile.UpTerrain, choosenTile.RightTerrain, choosenTile.DownTerrain, choosenTile.LeftTerrain, position.x, position.z, choosenTile.Material, choosenTile.Mask, choosenTile.Rotation, choosenTile.Plus, choosenTile.Areas);
                                            TM.rotateFirstMatchingRotation(ref tilesOnBoard[arrayIndex[0], arrayIndex[1]], ref masks[arrayIndex[0], arrayIndex[1]], arrayIndex, ref tilesOnBoard);

                                            String result4 = "";
                                            foreach (var l in tilesOnBoard[arrayIndex[0], arrayIndex[1]].GetComponent<Tile>().Areas)
                                            {
                                                result4 += String.Join(" ", l.edges.Select(item => item.ToString()).ToArray());
                                                result4 += " | ";

                                            }
                                            //       Debug.Log(result4);
                                        }

                                        //tilesOnBoard[arrayIndex[0], arrayIndex[1]].GetComponent<Renderer>().material = choosenTile.Material;
                                        // masks[arrayIndex[0], arrayIndex[1]].GetComponent<Renderer>().material = choosenTile.Mask;

                                        //currentlyPlacedTile = arrayIndex;
                                        //networkView.RPC("PutTitle2", RPCMode.All, arrayIndex[0], arrayIndex[1], position.x, position.y, position.z, arrayIndex);
                                        CM.CheckCamera(arrayIndex);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                OKButton.SetActive(false);
                MeepleButton.SetActive(false);
            }

    }
    // ------------------------ RPC's ----------------------------

    [RPC]
    public void LanButtonClick()
    {
        tilesLeft--;
        tilesLeftText.text = tilesLeft.ToString();
    }
    [RPC]
    public void SetRandomTitle(int newi)
    {
        if (Network.peerType == NetworkPeerType.Client)
        {
            if (SerwerAns == 1)
                SerwerAns = 2;
            currentTileIndex = newi;
            choosenTile = new Tile();
            choosenTile.Clone(TM.tilesList[currentTileIndex]);
        }
    }
    [RPC]
    public void RandomTitle()
    {
        if (Network.peerType == NetworkPeerType.Server)
        {
            currentTileIndex = UnityEngine.Random.Range(0, TM.tilesList.Count);
            choosenTile = new Tile();
            choosenTile.Clone(TM.tilesList[currentTileIndex]);
            networkView.RPC("SetRandomTitle", RPCMode.All, currentTileIndex);
        }
    }
    [RPC]
    public void SendTitle()
    {
        if (Network.peerType == NetworkPeerType.Server)
        {
            NextTileImage.GetComponent<Image>().material = choosenTile.Material;
            playername.text = GM.GetCurrentPlayer().name;
            networkView.RPC("SendTitleClient", RPCMode.All);
        }
    }
    [RPC]
    public void SendTitleClient()
    {
        if (Network.peerType == NetworkPeerType.Client)
        {
            if (SerwerAns == 3)
                SerwerAns = 4;

            NextTileImage.GetComponent<Image>().material = choosenTile.Material;
            playername.text = GM.GetCurrentPlayer().name;
        }
    }
    [RPC]
    public void ShowPosition(int arr1, int arr2)
    {
        if (actualTurn == clientNumber)
        {
            float[] position = TM.getCoordinates(arr1, arr2);
            if (possibleMoves[arr1, arr2] == null)
            {
                possibleMoves[arr1, arr2] = Instantiate(Selected, new Vector3(position[0], (float)0.1, position[1]), Quaternion.identity) as GameObject;
            }
        }
    }
    [RPC]
    public void Something()
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

        GM.NextPlayer();
    }
    [RPC]
    public void Something2()
    {
        TM.tilesList.RemoveAt(currentTileIndex);
    }
    [RPC]
    public void Turn_change()
    {
        do
        {
            actualTurn++;
            if (actualTurn == 5)
                actualTurn = 0;
        }
        while ((actualTurn != 0) && !connections[actualTurn - 1]);
        Debug.Log("numer gracza:" + actualTurn);
    }
    [RPC]
    public void debuglan(int number)
    {
        Debug.Log("numer gracza podłączony:" + number);
    }
    [RPC]
    public void debuglan2()
    {
        Debug.Log("null");
    }
    [RPC]
    public void Gameconn()
    {
        if (Network.peerType == NetworkPeerType.Server)
        {
            Debug.Log("weeszedło");
            wait--;
        }
    }
    [RPC]
    public void PutTitle(int arr0, int arr1, float px, float py, float pz, int[] index)
    {
        if (Network.peerType == NetworkPeerType.Server)
            networkView.RPC("PutTitle2", RPCMode.All, arr0, arr1, px, py, pz, index);
    }
    [RPC]
    public void PutTitle2(int arr0, int arr1, float px, float py, float pz, int[] index)
    {
        if (currentlyPlacedTile != null)
        {
            Destroy(tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]]);
            Destroy(masks[currentlyPlacedTile[0], currentlyPlacedTile[1]]);
            tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]] = null;
            masks[currentlyPlacedTile[0], currentlyPlacedTile[1]] = null;
        }
        tilesOnBoard[arr0, arr1] = Instantiate(objectToinstantiate, new Vector3(px, py, pz), Quaternion.identity) as GameObject; // instatiate a prefab on the position where the ray hits the floor. 
        masks[arr0, arr1] = Instantiate(Mask, new Vector3(px, py, pz), Quaternion.identity) as GameObject;
        Tile tile = tilesOnBoard[arr0, arr1].AddComponent<Tile>();
        String result9 = "";
        foreach (var l in choosenTile.Areas)
        {
            result9 += String.Join(" ", l.edges.Select(item => item.ToString()).ToArray());
            result9 += " | ";
        }
        tile.Init(choosenTile.IdNumber, choosenTile.UpTerrain, choosenTile.RightTerrain, choosenTile.DownTerrain, choosenTile.LeftTerrain, px, pz, choosenTile.Material, choosenTile.Mask, choosenTile.Rotation, choosenTile.Plus, choosenTile.Areas);
        TM.rotateFirstMatchingRotation(ref tilesOnBoard[arr0, arr1], ref masks[arr0, arr1], index, ref tilesOnBoard);
        String result4 = "";
        foreach (var l in tilesOnBoard[arr0, arr1].GetComponent<Tile>().Areas)
        {
            result4 += String.Join(" ", l.edges.Select(item => item.ToString()).ToArray());
            result4 += " | ";

        }
        tilesOnBoard[arr0, arr1].GetComponent<Renderer>().material = choosenTile.Material;
        masks[arr0, arr1].GetComponent<Renderer>().material = choosenTile.Mask;
        currentlyPlacedTile = index;
    }
    [RPC]
    public void Rotate(int arr0, int arr1, int[] index)
    {
        if (Network.peerType == NetworkPeerType.Server)
            networkView.RPC("Rotate2", RPCMode.All, arr0, arr1, index);
    }
    [RPC]
    public void Rotate2(int arr0, int arr1, int[] index)
    {
        TM.rotateFirstMatchingRotation(ref tilesOnBoard[arr0, arr1], ref masks[arr0, arr1], index, ref tilesOnBoard);
        //String result3 = "";
        //foreach (var l in tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas)
        //{
        //    result3 += String.Join(" ", l.edges.Select(item => item.ToString()).ToArray());
        //    result3 += " | ";
        //}
    }
    //yooolo
    [RPC]
    public void meeple2(int lhit, float hitx, float hity, float hitz)
    {
        if (lhit == -1)
        {
            Destroy(meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]]);
            meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]] = null;
            int element = tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.FindIndex(a => a.colorIndex == ColorType);
            tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas[element].player = null;
        }
        else
            if (lhit == 0)
            {
                Debug.Log("zero");
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
            else
                if (lhit == 1)
                {
                    meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]] = Instantiate(FarmerMeeple, (new Vector3(hitx, hity, hitz)), Quaternion.identity) as GameObject;
                    meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]].transform.Translate(new Vector3(0, (float)0.44, 0));
                    meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]].transform.Rotate(new Vector3(0, 0, 90));
                    meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Renderer>().material.color = GM.GetCurrentPlayer().rgbaColor;
                    tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Find(a => a.colorIndex == ColorType).player = new Player(GM.GetCurrentPlayer().name, GM.GetCurrentPlayer().color, GM.GetCurrentPlayer().rgbaColor);
                }
                else
                {
                    meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]] = Instantiate(StandingMeeple, (new Vector3(hitx, hity, hitz)), Quaternion.identity) as GameObject;
                    meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]].transform.Translate(new Vector3(0, (float)2.13, 0));
                    meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]].transform.Rotate(new Vector3(0, 90, 0));
                    meeples[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Renderer>().material.color = GM.GetCurrentPlayer().rgbaColor;
                    tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]].GetComponent<Tile>().Areas.Find(a => a.colorIndex == ColorType).player = new Player(GM.GetCurrentPlayer().name, GM.GetCurrentPlayer().color, GM.GetCurrentPlayer().rgbaColor);
                }

    }
    [RPC]
    public void meeple(int lhit, float hitx, float hity, float hitz)
    {
        if (Network.peerType == NetworkPeerType.Server)
            networkView.RPC("meeple2", RPCMode.All, lhit, hitx, hity, hitz);
    }
    [RPC]
    public void meepleclick()
    {
        Debug.Log("meepleclick");
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
    }
}
