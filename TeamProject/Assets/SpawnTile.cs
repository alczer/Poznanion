using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SpawnTile : MonoBehaviour {

    GameObject[,] tilesOnBoard = new GameObject[200, 200];
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

    int[] getArrayPosition(float x, float z)
    {
        float col = 100 + (x / 10);
        float row = 100 + (z / 10);
        int[] array = new int[2] { (int)row, (int)col };
        return array;
    }

    void Start()
    {
        currentNbrTiles = 0;

        tilesOnBoard[100, 100] = Instantiate(objectToinstantiate, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        Tile startTile = tilesOnBoard[100, 100].AddComponent<Tile>();
        startTile.Init(terrainTypes.castle, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grass, 0, 0, startMaterial);
        tilesOnBoard[100, 100].GetComponent<Renderer>().material = startMaterial;

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

            if (Input.GetMouseButtonUp(0))
            {
                if (tilesList.Count > 0)
                {
                    Vector3 position = hit.point;
                    position.x = Mathf.Round(position.x / 10) * 10; // 10 is the size of a Tile
                    position.z = Mathf.Round(position.z / 10) * 10; // 10 is the size of a Tile

                    Debug.Log("Position x: " + position.x.ToString() + " Position z: " + position.z.ToString());
                    int[] arrayIndex = getArrayPosition(position.x, position.z);
                    Debug.Log("Position Array row: " + arrayIndex[0].ToString() + " Position array column: " + arrayIndex[1].ToString());


                    if (tilesOnBoard[arrayIndex[0], arrayIndex[1]] != null)
                    {
                        Debug.Log("A tile is already at this position");
                    }
                    else
                    {
                        tilesOnBoard[arrayIndex[0], arrayIndex[1]] = Instantiate(objectToinstantiate, position, Quaternion.identity) as GameObject;// instatiate a prefab on the position where the ray hits the floor.
                        int i = Random.Range(0, tilesList.Count);
                        var item = tilesList[i];
                        Tile tile = tilesOnBoard[arrayIndex[0], arrayIndex[1]].AddComponent<Tile>();
                        tile.Init(item.UpTerrain, item.RightTerrain, item.DownTerrain, item.LeftTerrain, position.x, position.z, item.Material);
                        tilesOnBoard[arrayIndex[0], arrayIndex[1]].GetComponent<Renderer>().material = item.Material;
                        tilesList.RemoveAt(i);
                        //Debug.Log(hit.point);
                        currentNbrTiles++;
  
                    }            
                }
            }
        }  
    }
}
