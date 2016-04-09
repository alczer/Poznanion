using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

    public MoveCamera moveCamera;
    public float maxX = 0;
    public float minX = 0;
    public float maxZ = 0;
    public float minZ = 0;
    public bool cameraCheck = false;
    public void UpdateCamera()
    {
        moveCamera.Xmax = 40 + maxX * 4;
        moveCamera.Xmin = -40 + minX * 4;
        moveCamera.Zmax = 40 - minZ * 4;
        moveCamera.Zmin = -40 - maxZ * 4;
    }

    public void CheckCamera(int[] x)
    {
        if (x[1] - 100 > maxX)
            maxX = x[1] - 100;
        if (x[1] - 100 < minX)
            minX = x[1] - 100;
        if (x[0] - 100 > maxZ)
            maxZ = x[0] - 100;
        if (x[0] - 100 < minZ)
            minZ = x[0] - 100;

        cameraCheck = true;
        //Debug.Log(x[0]);
        //Debug.Log(x[1]);
        //Debug.Log(maxX);
        //Debug.Log(minX);
        //Debug.Log(maxZ);
        //Debug.Log(minZ);
    }
}
