using UnityEngine;
using System.Collections;
public enum terrainTypes
{
    grass,
    castle,
    grassRoad
};
public class Tile : MonoBehaviour {

    
    private terrainTypes upTerrain;
    private terrainTypes rightTerrain;
    private terrainTypes downTerrain;
    private terrainTypes leftTerrain;
    private Material material;
    private float xPosition;
    private float yPosition;

    public void Init(terrainTypes up, terrainTypes right, terrainTypes down, terrainTypes left, float x, float y, Material m)
    {
        this.upTerrain = up;
        this.rightTerrain = right;
        this.downTerrain = down;
        this.leftTerrain = left;
        this.xPosition = x;
        this.yPosition = y;
        this.material = m;


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
}
