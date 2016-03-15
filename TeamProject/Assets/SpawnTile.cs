using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SpawnTile : MonoBehaviour {

    List<Tile> tilesOnBoard = new List<Tile>();
    List<Tile> tilesList = new List<Tile>(); //List of all disponible tiles (not on board)
    Random rnd = new Random();

    public Material startMaterial;
    public Material m1;
    public Material m2;
    public Material m3;
    public Material m4;

    Ray myRay;   
    RaycastHit hit;
    public GameObject objectToinstantiate;
    public int currentNbrTiles;

    void Start()
    {
        currentNbrTiles = 0;
        Tile startTile = new Tile();
        startTile.Init(terrainTypes.castle, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grass, 0, 0, startMaterial);
        tilesOnBoard.Add(startTile);

        Tile t1 = new Tile();
        t1.Init(terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, 0, 0,m1);
        tilesList.Add(t1);
        Tile t2 = new Tile();
        t2.Init(terrainTypes.grass, terrainTypes.grass, terrainTypes.grassRoad, terrainTypes.grass, 0, 0, m2);
        tilesList.Add(t2);
        Tile t3 = new Tile();
        t3.Init(terrainTypes.grass, terrainTypes.grass, terrainTypes.grass, terrainTypes.grass, 0, 0, m3);
        tilesList.Add(t3);
        Tile t4 = new Tile();
        t4.Init(terrainTypes.castle, terrainTypes.castle, terrainTypes.castle, terrainTypes.castle, 0, 0, m4);
        tilesList.Add(t4);

    }

    void Update()
    {
        myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(myRay, out hit))
        { 

            if (Input.GetMouseButtonDown(0))
            {
                if (tilesList.Count > 0)
                {
                    Vector3 position = hit.point;
                    position.x = Mathf.Round(position.x / 10) * 10; // 10 is the size of a Tile
                    position.z = Mathf.Round(position.z / 10) * 10; // 10 is the size of a Tile
                    GameObject clone = Instantiate(objectToinstantiate, position, Quaternion.identity) as GameObject;// instatiate a prefab on the position where the ray hits the floor.
                    int i = Random.Range(0, tilesList.Count);
                    var item = tilesList[i];

                    Tile tile = clone.AddComponent<Tile>();
                    tile.Init(item.UpTerrain, item.RightTerrain, item.DownTerrain, item.LeftTerrain, position.x, position.z, item.Material);
                    clone.GetComponent<Renderer>().material = item.Material;
                    tilesList.RemoveAt(i);
                    Debug.Log(hit.point);
                    currentNbrTiles++;                  
                }
            }
        }  
    }
}
