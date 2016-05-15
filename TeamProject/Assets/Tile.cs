using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum terrainTypes
{
    grass,
    castle,
    road,
    intersection,
    monastery
};


public class Area
{
    public List<int> edges;
    public Player player;
    public terrainTypes terrain;
    public int colorIndex;
    public int meeplePlacementIndex;
};

public class Tile : MonoBehaviour
{
    private terrainTypes upTerrain;
    private terrainTypes rightTerrain;
    private terrainTypes downTerrain;
    private terrainTypes leftTerrain;
    private Material material;
    private Material mask;
    private float xPosition;
    private float yPosition;
    private int typeCount; //how many tiles of this type exist    
    private int turn; //0 = 0 degrees, 1 = 90 degrees, 2 = 180 degrees, 3 = 270 degrees
    private bool plus;
    private List<Area> areas = new List<Area>();
    public void Init(terrainTypes up, terrainTypes right, terrainTypes down, terrainTypes left, float x, float y, Material m, Material mask1, int count, int turn1, bool plus, List<Area> areas1)
    {
        this.upTerrain = up;
        this.rightTerrain = right;
        this.downTerrain = down;
        this.leftTerrain = left;
        this.xPosition = x;
        this.yPosition = y;
        this.material = m;
        this.mask = mask1;
        this.typeCount = count;
        this.turn = turn1;
        this.areas = areas1;
        this.plus = plus;
    }

    public void Clone(Tile other)
    {         
        this.upTerrain = other.upTerrain;
        this.rightTerrain = other.rightTerrain;
        this.downTerrain = other.downTerrain;
        this.leftTerrain = other.leftTerrain;
        this.material = other.material;
        this.mask = other.mask;
        this.xPosition = other.xPosition;
        this.yPosition = other.yPosition;
        this.plus = other.plus;

        //turn = other.turn;
        List<Area> ar = new List<Area>();
        for (int k = 0; k < other.Areas.Count; k++)
        {
            ar.Add(new Area());

        }
        for (int h = 0; h < other.Areas.Count; h++)
        {
            ar[h].meeplePlacementIndex = other.Areas[h].meeplePlacementIndex;
            ar[h].colorIndex = other.Areas[h].colorIndex;
            ar[h].terrain = other.Areas[h].terrain;
            ar[h].edges = new List<int>();
            for (int y = 0; y < other.Areas[h].edges.Count; y++)
            {
                int o = other.Areas[h].edges[y];
                ar[h].edges.Add(o);
            }
        }
        this.areas = ar;  
    }
  

    public Material Material // This is your property
    {
        get { return this.material; }
    }
    public Material Mask
    {
        get { return this.mask; }
    }
    public terrainTypes UpTerrain // This is your property
    {
        get { return this.upTerrain; }
        set { this.upTerrain = value; }
    }
    public terrainTypes RightTerrain // This is your property
    {
        get { return this.rightTerrain; }
        set { this.rightTerrain = value; }
    }
    public terrainTypes DownTerrain // This is your property
    {
        get { return this.downTerrain; }
        set { this.downTerrain = value; }
    }
    public terrainTypes LeftTerrain // This is your property
    {
        get { return this.leftTerrain; }
        set { this.leftTerrain = value; }
    }
    public int TypeCount
    {
        get { return this.typeCount; }
        set { this.typeCount = value; }
    }
    public int Turn
    {
        get { return this.turn; }
        set { this.turn = value; }
    }
    public bool Plus
    {
        get { return this.plus; }
        set { this.plus = value; }
    }
    public List<Area> Areas
    {
        get { return this.areas; }
        set { this.areas = value; }
    }

}

