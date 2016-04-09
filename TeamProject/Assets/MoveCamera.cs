using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour
{

    //
    // VARIABLES
    //

    // rotation
    public float turnSpeed = 4.0f;

    // panning
    public float panSpeed = 4.0f;
    public float touchpanSpeed = 0.5f;

    // middle button zoom
    public float zoomSpeed = 4.0f; 

    // scroll zoom
    public float scrollSpeed = 40f;

    // pinch zoom
    public float pinchZoomSpeed = 0.5f;

    // edge move speed
    public float edgeSpeed = 2f;

    // movement
    public float Xmax = 40f;
    public float Xmin = -40f;
    public float Zmax = 40f;
    public float Zmin = -40f;

    // distance
    public float cameraDistanceMax = 90f;
    public float cameraDistanceMin = 30f;


    private float zoom;
    
    private Vector3 mouseOrigin;	// Position of cursor when mouse dragging starts

    private bool isPanning;		    // Is the camera being panned?
    private bool isRotating;	    // Is the camera being rotated?
    private bool isZooming;         // Is the camera zooming (middlebutton)?

    private bool dontUseTouch = true;   // Use touchscreen, or not

    //
    // START
    //

    void Start()
    {
        Debug.Log("DPI =" + Screen.dpi);
        
        //check if our current system info equals a desktop
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            //we are on a desktop device, so don't use touch
            dontUseTouch = true;
        }
        //if it isn't a desktop, lets see if our device is a handheld device aka a mobile device
        else if (SystemInfo.deviceType == DeviceType.Handheld && Input.multiTouchEnabled)
        {
            //we are on a mobile device, so lets use touch input
            dontUseTouch = false;
        }
    }

    //
    // UPDATE
    //
    void Update()
    {
        // Check current zoom
        zoom = Camera.main.transform.position.y * 0.01f;

        if (dontUseTouch)
        {
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

            // Edge moving
            if (!isZooming && !isRotating && !isPanning)
            {
                Vector3 tmp = (Camera.main.ScreenToViewportPoint(Input.mousePosition));
                if (tmp.x > 0.99)
                    transform.Translate(edgeSpeed * zoom, 0, 0);
                else
                if (tmp.x < 0.01)
                    transform.Translate(-edgeSpeed * zoom, 0, 0);
                if (tmp.y > 0.99)
                    transform.Translate(0, edgeSpeed * zoom, 0);
                else
                if (tmp.y < 0.01)
                    transform.Translate(0, -edgeSpeed * zoom, 0);
            }

            // Rotate camera along X and Y axis
            if (isRotating)
            {
                Vector2 prevPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
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

            // W A S D controls
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                transform.Translate(edgeSpeed * zoom, 0, 0);
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                transform.Translate(-edgeSpeed * zoom, 0, 0);
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                transform.Translate(0, edgeSpeed * zoom, 0);
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                transform.Translate(0, -edgeSpeed * zoom, 0);

            // middle button zoom
            if (isZooming)
            {
                Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

                Vector3 move = pos.y * zoomSpeed * transform.forward;
                transform.Translate(move, Space.World);
            }

            // ScrollMouse Zoom
            if (!isZooming)
            {
                transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * scrollSpeed);
            }
        }
        // Multitouch controls
        else
        {
            // Touchscreen panning
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
                transform.Translate(-touchDeltaPosition.x * touchpanSpeed * zoom / Screen.dpi * 300,
                                    -touchDeltaPosition.y * touchpanSpeed * zoom / Screen.dpi * 300, 0);
            }

            // Pinch Zoom
            if (Input.touchCount == 2)
            {
                // Store both touches.
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                // Find the position in the previous frame of each touch.
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // Find the magnitude of the vector (the distance) between the touches in each frame.
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // Find the difference in the distances between each frame.
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;


                transform.Translate(0, 0, -deltaMagnitudeDiff * pinchZoomSpeed);
            }
        }
       

        // limits
        transform.position = new Vector3(
        Mathf.Clamp(transform.position.x, Xmin, Xmax),
        Mathf.Clamp(transform.position.y, cameraDistanceMin, cameraDistanceMax),
        Mathf.Clamp(transform.position.z, Zmin, Zmax)
            );
    }
}
