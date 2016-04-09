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

public struct tileEdges
{
    public terrainTypes terrain;
    public bool meeple;
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
    private tileEdges middle; //0
    private tileEdges upL; //1
    private tileEdges upM; //2
    private tileEdges upR; //3
    private tileEdges rightU; //4
    private tileEdges rightM; //5
    private tileEdges rightD; //6
    private tileEdges downR; //7
    private tileEdges downM; //8
    private tileEdges downL; //9
    private tileEdges leftD; //10
    private tileEdges leftM; //11
    private tileEdges leftU; //12
    private List<List<int>> grassAreas = new List<List<int>>();
    private List<List<int>> castleAreas = new List<List<int>>();
    private List<List<int>> roadAreas = new List<List<int>>();

    public void Init(terrainTypes up, terrainTypes right, terrainTypes down, terrainTypes left, float x, float y, Material m, int count, terrainTypes middle1,
         terrainTypes upL1, terrainTypes upM1, terrainTypes upR1, terrainTypes rightU1, terrainTypes rightM1, terrainTypes rightD1, terrainTypes downR1, terrainTypes downM1, terrainTypes downL1,
         terrainTypes leftD1, terrainTypes leftM1, terrainTypes leftU1, List<List<int>> grassAreas1, List<List<int>> castleAreas1, List<List<int>> roadAreas1)
    {
        this.upTerrain = up;
        this.rightTerrain = right;
        this.downTerrain = down;
        this.leftTerrain = left;
        this.xPosition = x;
        this.yPosition = y;
        this.material = m;
        this.typeCount = count;

        this.middle.terrain = middle1;
        this.middle.meeple = false;
        this.upL.terrain = upL1;
        this.upL.meeple = false;
        this.upM.terrain = upM1;
        this.upM.meeple = false;
        this.upR.terrain = upR1;
        this.upR.meeple = false;
        this.rightD.terrain = rightD1;
        this.rightD.meeple = false;
        this.rightM.terrain = rightM1;
        this.rightM.meeple = false;
        this.rightU.terrain = rightU1;
        this.rightU.meeple = false;
        this.downL.terrain = downL1;
        this.downL.meeple = false;
        this.downM.terrain = downM1;
        this.downM.meeple = false;
        this.downR.terrain = downR1;
        this.downR.meeple = false;
        this.leftD.terrain = leftD1;
        this.leftD.meeple = false;
        this.leftM.terrain = leftM1;
        this.leftM.meeple = false;
        this.leftU.terrain = leftU1;
        this.leftU.meeple = false;
        this.grassAreas = grassAreas1;
        this.roadAreas = roadAreas1;
        this.castleAreas = castleAreas1;


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

    public tileEdges Middle
    {
        get { return middle; }
        set { middle = value; }
    }

    public tileEdges UpL
    {
        get
        {
            return upL;
        }

        set
        {
            upL = value;
        }
    }

    public tileEdges UpM
    {
        get
        {
            return upM;
        }

        set
        {
            upM = value;
        }
    }

    public tileEdges UpR
    {
        get
        {
            return upR;
        }

        set
        {
            upR = value;
        }
    }

    public tileEdges RightU
    {
        get
        {
            return rightU;
        }

        set
        {
            rightU = value;
        }
    }

    public tileEdges RightM
    {
        get
        {
            return rightM;
        }

        set
        {
            rightM = value;
        }
    }

    public tileEdges RightD
    {
        get
        {
            return rightD;
        }

        set
        {
            rightD = value;
        }
    }

    public tileEdges DownR
    {
        get
        {
            return downR;
        }

        set
        {
            downR = value;
        }
    }

    public tileEdges DownM
    {
        get
        {
            return downM;
        }

        set
        {
            downM = value;
        }
    }

    public tileEdges DownL
    {
        get
        {
            return downL;
        }

        set
        {
            downL = value;
        }
    }

    public tileEdges LeftD
    {
        get
        {
            return leftD;
        }

        set
        {
            leftD = value;
        }
    }

    public tileEdges LeftM
    {
        get
        {
            return leftM;
        }

        set
        {
            leftM = value;
        }
    }

    public tileEdges LeftU
    {
        get
        {
            return leftU;
        }

        set
        {
            leftU = value;
        }
    }

    public List<List<int>> GrassAreas
    {
        get
        {
            return grassAreas;
        }

        set
        {
            grassAreas = value;
        }
    }

    public List<List<int>> CastleAreas
    {
        get
        {
            return castleAreas;
        }

        set
        {
            castleAreas = value;
        }
    }

    public List<List<int>> RoadAreas
    {
        get
        {
            return roadAreas;
        }

        set
        {
            roadAreas = value;
        }
    }
}
