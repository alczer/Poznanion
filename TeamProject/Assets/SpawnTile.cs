using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SpawnTile : MonoBehaviour {

    GameObject[,] tilesOnBoard = new GameObject[200, 200];
    List<Tile> tilesList = new List<Tile>(); //List of all disponible tiles (not on board)
    public Material startMaterial;
    public Material m1;
    public Material m2;
    public Material m3;
    public Material m4;
    public GameObject Canvas;
    public GameObject Button;
    private GameObject newCanvas;
    private GameObject newButton;
    private Button button;
    private bool currentlyPlacingTile;
    private int[] currentlyPlacedTile;
    private Tile choosenTile;

    Ray myRay;   
    RaycastHit hit;
    public GameObject objectToinstantiate;
    public int currentNbrTiles;

    int[] getArrayPosition(float x, float z)
    {
        float col = 100 + (x / 10);
        float row = 100 + (z / 10);
        int[] array = new int[2] { (int)row, (int)col };
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
        float rot = gameObject.transform.rotation.y+90;
        gameObject.transform.Rotate(new Vector3(0, 90, 0));
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

    void Start()
    {
        currentlyPlacedTile = new int[2];
        currentNbrTiles = 0;

        placeTile(ref objectToinstantiate, 100, 100);
        Tile startTile = tilesOnBoard[100, 100].AddComponent<Tile>();
        startTile.Init(terrainTypes.castle, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grass, 0, 0, startMaterial);
        tilesOnBoard[100, 100].GetComponent<Renderer>().material = startMaterial;
        currentlyPlacingTile = false;
        
        addTileToList(terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, 0, 0, m1);
        addTileToList(terrainTypes.grassRoad, terrainTypes.grass, terrainTypes.grass, terrainTypes.grass, 0, 0, m2);
        addTileToList(terrainTypes.grass, terrainTypes.grass, terrainTypes.grass, terrainTypes.grass, 0, 0, m3);
        addTileToList(terrainTypes.castle, terrainTypes.castle, terrainTypes.castle, terrainTypes.castle, 0, 0, m4);

    }

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
                newCanvas = Instantiate(Canvas) as GameObject;
                newButton = Instantiate(Button) as GameObject;
                newButton.transform.SetParent(newCanvas.transform, false);
                newButton.transform.localPosition = new Vector3((float)Screen.width / 2 - (newButton.GetComponent<RectTransform>().rect.width / 4), (- (float)Screen.height / 2) + (newButton.GetComponent<RectTransform>().rect.height / 1.5f), 0);
                button = newButton.GetComponent<Button>();
                button.onClick.AddListener(() => { currentlyPlacingTile = false; currentlyPlacedTile = null; Destroy(newButton); Destroy(newCanvas); });
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
                                rotateClockwise90(ref tilesOnBoard[arrayIndex[0], arrayIndex[1]]);
                            }

                        }                        
                        else
                        {
                         Debug.Log("A tile is already at this position");
                        }                       
                    }
                    else if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false)
                    {
                        if (currentlyPlacedTile != null) Destroy(tilesOnBoard[currentlyPlacedTile[0], currentlyPlacedTile[1]]);
                        newButton.SetActive(true);                                          
                        tilesOnBoard[arrayIndex[0], arrayIndex[1]] = Instantiate(objectToinstantiate, position, Quaternion.identity) as GameObject;// instatiate a prefab on the position where the ray hits the floor. 
                        Tile tile = tilesOnBoard[arrayIndex[0], arrayIndex[1]].AddComponent<Tile>();
                        tile.Init(choosenTile.UpTerrain, choosenTile.RightTerrain, choosenTile.DownTerrain, choosenTile.LeftTerrain, position.x, position.z, choosenTile.Material);
                        tilesOnBoard[arrayIndex[0], arrayIndex[1]].GetComponent<Renderer>().material = choosenTile.Material;
                        currentlyPlacedTile = arrayIndex;
                        //Debug.Log(hit.point);
                        currentNbrTiles++;
                    }
                }
            }
        }
    }
}
