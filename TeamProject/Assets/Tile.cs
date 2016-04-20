using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum terrainTypes
{
    grass,
    castle,
    road,
    intersection,
    monastery
};

public struct Area
{
    public List<int> edges;
    public Player player;
    public terrainTypes terrain;
};
public class Tile : MonoBehaviour
{
    private terrainTypes upTerrain;
    private terrainTypes rightTerrain;
    private terrainTypes downTerrain;
    private terrainTypes leftTerrain;
    private Material material;
    private float xPosition;
    private float yPosition;
    private int typeCount; //how many tiles of this type exist    
    private int turn; //0 = 0 degrees, 1 = 90 degrees, 2 = 180 degrees, 3 = 270 degrees
    private List<Area> areas = new List<Area>();
    public void Init(terrainTypes up, terrainTypes right, terrainTypes down, terrainTypes left, float x, float y, Material m, int count, int turn1, List<Area> areas1)
    {
        this.upTerrain = up;
        this.rightTerrain = right;
        this.downTerrain = down;
        this.leftTerrain = left;
        this.xPosition = x;
        this.yPosition = y;
        this.material = m;
        this.typeCount = count;
        this.turn = turn1;
        this.areas = areas1;
    }
    public Material Material // This is your property
    {
        get { return this.material; }
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
    public List<Area> Areas
    {
        get { return this.areas; }
        set { this.areas = value; }
    }

}

   