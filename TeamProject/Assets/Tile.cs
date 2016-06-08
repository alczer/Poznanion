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

public class TileAI
{
    private int idNumber;
    private terrainTypes upTerrain;
    private terrainTypes rightTerrain;
    private terrainTypes downTerrain;
    private terrainTypes leftTerrain;
    private float xPosition;
    private float yPosition;
    //private int typeCount; //how many tiles of this type exist    
    private int rotation; //0 = 0 degrees, 1 = 90 degrees, 2 = 180 degrees, 3 = 270 degrees
    private bool plus;
    private List<Area> areas = new List<Area>();

    public void Init(int id, terrainTypes up, terrainTypes right, terrainTypes down, terrainTypes left, float x, float y, int rotation, bool plus, List<Area> areas1)
    {
        this.idNumber = id;
        this.upTerrain = up;
        this.rightTerrain = right;
        this.downTerrain = down;
        this.leftTerrain = left;
        this.xPosition = x;
        this.yPosition = y;
        //this.typeCount = count;
        this.rotation = rotation;
        this.areas = areas1;
        this.plus = plus;
    }

    public void Clone(TileAI other)
    {
        this.rotation = other.rotation;
        this.idNumber = other.idNumber;
        this.upTerrain = other.upTerrain;
        this.rightTerrain = other.rightTerrain;
        this.downTerrain = other.downTerrain;
        this.leftTerrain = other.leftTerrain;
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
            ar[h].player = other.Areas[h].player;
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
    public void Clone(Tile other)
    {
        this.rotation = other.Rotation;
        this.idNumber = other.IdNumber;
        this.upTerrain = other.UpTerrain;
        this.rightTerrain = other.RightTerrain;
        this.downTerrain = other.DownTerrain;
        this.leftTerrain = other.LeftTerrain;
        this.xPosition = other.XPosition;
        this.yPosition = other.YPosition;
        this.plus = other.Plus;

        //turn = other.turn;
        List<Area> ar = new List<Area>();
        for (int k = 0; k < other.Areas.Count; k++)
        {
            ar.Add(new Area());

        }
        for (int h = 0; h < other.Areas.Count; h++)
        {
            ar[h].player = other.Areas[h].player;

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
    public float XPosition
    {
        get { return this.xPosition; }
        set { this.xPosition = value; }
    }
    public float YPosition
    {
        get { return this.yPosition; }
        set { this.yPosition = value; }
    }
    public terrainTypes UpTerrain
    {
        get { return this.upTerrain; }
        set { this.upTerrain = value; }
    }
    public terrainTypes RightTerrain
    {
        get { return this.rightTerrain; }
        set { this.rightTerrain = value; }
    }
    public terrainTypes DownTerrain
    {
        get { return this.downTerrain; }
        set { this.downTerrain = value; }
    }
    public terrainTypes LeftTerrain
    {
        get { return this.leftTerrain; }
        set { this.leftTerrain = value; }
    }
    public int IdNumber
    {
        get { return this.idNumber; }
        set { this.idNumber = value; }
    }
    public int Turn
    {
        get { return this.rotation; }
        set { this.rotation = value; }
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

    public int Rotation
    {
        get { return rotation; }
        set { this.rotation = value; }
    }
}

public class Tile : MonoBehaviour
{
    private int idNumber;
    private terrainTypes upTerrain;
    private terrainTypes rightTerrain;
    private terrainTypes downTerrain;
    private terrainTypes leftTerrain;
    private Material material;
    private Material mask;
    private float xPosition;
    private float yPosition;
    //private int typeCount; //how many tiles of this type exist    
    private int rotation = 0; //0 = 0 degrees, 1 = 90 degrees, 2 = 180 degrees, 3 = 270 degrees
    private bool plus;
    private List<Area> areas = new List<Area>();

    public void Init(int id, terrainTypes up, terrainTypes right, terrainTypes down, terrainTypes left, float x, float y, Material m, Material mask1, int rotation, bool plus, List<Area> areas1)
    {
        this.rotation = 0;
        this.idNumber = id;
        this.upTerrain = up;
        this.rightTerrain = right;
        this.downTerrain = down;
        this.leftTerrain = left;
        this.xPosition = x;
        this.yPosition = y;
        this.material = m;
        this.mask = mask1;
        //this.typeCount = count;
        this.areas = areas1;
        this.plus = plus;
    }

    public void Clone(Tile other)
    {
        this.rotation = other.rotation;
        this.idNumber = other.idNumber; 
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
            ar[h].player = other.Areas[h].player;
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
    public void Clone(TileAI other)
    {
        this.rotation = other.Rotation;
        this.idNumber = other.IdNumber;
        this.upTerrain = other.UpTerrain;
        this.rightTerrain = other.RightTerrain;
        this.downTerrain = other.DownTerrain;
        this.leftTerrain = other.LeftTerrain;
        this.xPosition = other.XPosition;
        this.yPosition = other.YPosition;
        this.plus = other.Plus;

        //turn = other.turn;
        List<Area> ar = new List<Area>();
        for (int k = 0; k < other.Areas.Count; k++)
        {
            ar.Add(new Area());

        }
        for (int h = 0; h < other.Areas.Count; h++)
        {
            ar[h].player = other.Areas[h].player;
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
  

    public Material Material
    {
        get { return this.material; }
    }
    public Material Mask
    {
        get { return this.mask; }
    }
    public float XPosition
    {
        get { return this.xPosition; }
        set { this.xPosition = value; }
    }
    public float YPosition
    {
        get { return this.yPosition; }
        set { this.yPosition = value; }
    }
    public terrainTypes UpTerrain
    {
        get { return this.upTerrain; }
        set { this.upTerrain = value; }
    }
    public terrainTypes RightTerrain
    {
        get { return this.rightTerrain; }
        set { this.rightTerrain = value; }
    }
    public terrainTypes DownTerrain
    {
        get { return this.downTerrain; }
        set { this.downTerrain = value; }
    }
    public terrainTypes LeftTerrain
    {
        get { return this.leftTerrain; }
        set { this.leftTerrain = value; }
    }
    public int IdNumber
    {
        get { return this.idNumber; }
        set { this.idNumber = value; }
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
    public int Rotation
    {
        get{ return rotation; }
        set{ this.rotation = value; }
    }
}

