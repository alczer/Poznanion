using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TilesManager : MonoBehaviour {

    public List<Tile> tilesList = new List<Tile>(); //List of all disponible tiles (not on board)

    public Material CRFR; // START
    public Material CFFF;
    public Material CFFC;
    public Material CFFC_2;
    public Material CFFC_P;
    public Material CCFC;
    public Material CCFC_P;
    public Material CCRC;
    public Material CCRC_P;
    public Material CCCC_P;
    public Material CRRC;
    public Material CRRC_P;
    public Material FCFC;
    public Material FCFC_P;
    public Material CFCF_2;
    public Material CRRF;
    public Material CFRR;
    public Material CRRR;
    public Material FRFR;
    public Material FFRR;
    public Material FRRR;
    public Material RRRR;
    public Material FFRF_M;
    public Material FFFF_M;

    void addTileToList(terrainTypes up, terrainTypes right, terrainTypes down, terrainTypes left, float x, float y, Material m, int c=1)
    {
        Tile tmp = new Tile();
        tmp.Init(up, right, down, left, x, y, m, c);
        tilesList.Add(tmp);
    }

    void init ()
    {
        //public Material CRFR; 4 - START
        addTileToList(terrainTypes.castle, terrainTypes.grassRoad, terrainTypes.grass, terrainTypes.grassRoad, 0, 0, CRFR, 3);
        //public Material CFFF; 
        addTileToList(terrainTypes.castle, terrainTypes.grass, terrainTypes.grass, terrainTypes.grass, 0, 0, CFFF);
        //public Material CFFC;
        addTileToList(terrainTypes.castle, terrainTypes.grass, terrainTypes.grass, terrainTypes.castle, 0, 0, CFFC);
        //public Material CFFC_2;
        addTileToList(terrainTypes.castle, terrainTypes.grass, terrainTypes.grass, terrainTypes.castle, 0, 0, CFFC_2);
        //public Material CFFC_P;
        addTileToList(terrainTypes.castle, terrainTypes.grass, terrainTypes.grass, terrainTypes.castle, 0, 0, CFFC_P);
        //public Material CCFC;
        addTileToList(terrainTypes.castle, terrainTypes.castle, terrainTypes.grassRoad, terrainTypes.castle, 0, 0, CCFC);
        //public Material CCFC_P;
        addTileToList(terrainTypes.castle, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, 0, 0, CCFC_P);
        //public Material CCRC;
        addTileToList(terrainTypes.castle, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, 0, 0, CCRC);
        //public Material CCRC_P;
        addTileToList(terrainTypes.castle, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, 0, 0, CCRC_P);
        //public Material CCCC_P;
        addTileToList(terrainTypes.castle, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, 0, 0, CCCC_P);
        //public Material CRRC;
        addTileToList(terrainTypes.castle, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, 0, 0, CRRC);
        //public Material CRRC_P;
        addTileToList(terrainTypes.grass, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, 0, 0, CRRC_P);
        //public Material FCFC;
        addTileToList(terrainTypes.grass, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, 0, 0, FCFC);
        //public Material FCFC_P;
        addTileToList(terrainTypes.castle, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, 0, 0, FCFC_P);
        //public Material CFCF_2;
        addTileToList(terrainTypes.castle, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, 0, 0, CFCF_2);
        //public Material CRRF;
        addTileToList(terrainTypes.castle, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, 0, 0, CRRF);
        //public Material CFRR;
        addTileToList(terrainTypes.castle, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, 0, 0, CFRR);
        //public Material CRRR;
        addTileToList(terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, terrainTypes.grassRoad, 0, 0, CRRR);
        //public Material FRFR;
        //public Material FFRR;
        //public Material FRRR;
        //public Material RRRR;
        //public Material FFRF_M;
        //public Material FFFF_M;
    }




    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
