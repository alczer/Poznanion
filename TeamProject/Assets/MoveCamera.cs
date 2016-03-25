using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{

    //
    // VARIABLES
    //

    // rotation
    public float turnSpeed = 4.0f;		// Speed of camera turning when mouse moves in along an axis
    public float panSpeed = 4.0f;		// Speed of the camera when being panned

    // middle button zoom
    public float zoomSpeed = 4.0f;      // Speed of the camera going back and forth

    // scroll zoom
    public float scrollSpeed = 30f;

    // edge move speed
    public float edgeSpeed = 0.7f;

    // movement
    public float Xmax = 150f;
    public float Zmax = 150f;

    // distance
    public float cameraDistanceMax = 90f;
    public float cameraDistanceMin = 30f;



    private Vector3 mouseOrigin;	// Position of cursor when mouse dragging starts
    private bool isPanning;		    // Is the camera being panned?
    private bool isRotating;	    // Is the camera being rotated?
    private bool isZooming;         // Is the camera zooming?





    void Start()
    {
    }

    //
    // UPDATE
    //


    void Update()
    {
        // Edge moving
        {
            Vector3 tmp = (Camera.main.ScreenToViewportPoint(Input.mousePosition));
            if (tmp.x > 0.99)
                transform.Translate(edgeSpeed, 0, 0);
            else
            if (tmp.x < 0.01)
                transform.Translate(-edgeSpeed, 0, 0);
            if (tmp.y > 0.99)
                transform.Translate(0, edgeSpeed, 0);
            else
            if (tmp.y < 0.01)
                transform.Translate(0, -edgeSpeed, 0);
        }


        // Get the left mouse button + LeftCTRL
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl))
        {
            // Get mouse origin
            mouseOrigin = Input.mousePosition;
            isRotating = true;
        }

        // Get the right mouse button
        if (Input.GetMouseButtonDown(1))
        {
            // Get mouse origin
            mouseOrigin = Input.mousePosition;
            isPanning = true;
        }

        // Get the middle mouse button
        if (Input.GetMouseButtonDown(2))
        {
            // Get mouse origin
            mouseOrigin = Input.mousePosition;
            isZooming = true;
        }

        // Disable movements on button release
        if (!Input.GetMouseButton(0)) isRotating = false;
        if (!Input.GetMouseButton(1)) isPanning = false;
        if (!Input.GetMouseButton(2)) isZooming = false;

        // Rotate camera along X and Y axis
        if (isRotating)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

            transform.RotateAround(transform.position, transform.right, -pos.y * turnSpeed);
            transform.RotateAround(transform.position, Vector3.up, pos.x * turnSpeed);
        }

        // Move the camera on it's XY plane
        if (isPanning)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

            Vector3 move = new Vector3(-pos.x * panSpeed, -pos.y * panSpeed, 0);
            transform.Translate(move, Space.Self);
        }

        // middle button zoom
        if (isZooming)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

            Vector3 move = pos.y * zoomSpeed * transform.forward;
            transform.Translate(move, Space.World);
        }

        // ScrollMouse Zoom
        {
            transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * scrollSpeed);
        }

        // limits
        transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, -Xmax, Xmax),
                Mathf.Clamp(transform.position.y, cameraDistanceMin, cameraDistanceMax),
                Mathf.Clamp(transform.position.z, -Zmax, Zmax)
            );

    }
}
