using UnityEngine;

public class CameraPan1C : MonoBehaviour
{
    private Vector3 startpoint; // startpoint
    private Vector3 endpoint; // endpoint
    private bool panning = false;

    private float zoomSpeed = 15;

    // cam rotate
    private bool rotating = false;
    private int layerMask = 1 << 8;
    private Vector3 tmp;
    private Ray ray;
    private RaycastHit hit;
    private int camSpeed = 20;
    // mainloop
    void Update()
    {

        // starts here, right mousebutton is pressed down
        if (Input.GetMouseButtonDown(1))
        {
            // make ray
            Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
            startpoint = ray1.GetPoint(transform.position.y * 2); // Returns a point at distance units along the ray
            startpoint.y = 0; // fix z to 0
            panning = true;
        }

        // 
        if (panning)
        {
            Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
            endpoint = ray2.GetPoint(transform.position.y * 2); // Returns a point at distance units along the ray
            endpoint.y = 0; // fix z, somehow its not always 0?

            // panning
            transform.position += startpoint - endpoint;
        }


        if (Input.GetMouseButtonUp(1)) // release button, stop pan
        {
            panning = false;

            // reset midpoint
            ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out hit, 9999))
            {
                tmp = hit.point;
            }
        }


        // zoom
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        if (scrollWheel != 0)
        {
            //transform.position+=Vector3.forward*zoomSpeed*Time.deltaTime;
            transform.Translate((scrollWheel * Vector3.forward) * zoomSpeed, Space.Self);
        }

        // rotate cam
        // Rotation A/D
        if (Input.GetKey("a"))
        {
            if (!rotating)
            {
                ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                if (Physics.Raycast(ray, out hit, 9999))
                {
                    tmp = hit.point;
                    rotating = true;
                }
            }
            Camera.main.transform.RotateAround(tmp, -Vector3.up, 50 * Time.deltaTime);
        }

        if (Input.GetKeyUp("a"))
        {
            rotating = false;
        }

        if (Input.GetKeyUp("d"))
        {
            rotating = false;
        }

        if (Input.GetKey("d"))
        {
            if (!rotating)
            {
                ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                if (Physics.Raycast(ray, out hit, 9999))
                {
                    tmp = hit.point;
                    rotating = true;
                }
            }
            Camera.main.transform.RotateAround(tmp, Vector3.up, 50 * Time.deltaTime);
        }

    }
}