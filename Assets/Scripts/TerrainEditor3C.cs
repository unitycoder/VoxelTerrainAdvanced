using UnityEngine;

public class TerrainEditor3C : MonoBehaviour
{
    // these are read from worldroot
    private int size = 30;     // number of cubes along a side
    private float axisMin = 0; // mesh size
    private float axisMax = 15; //120;
    private float multiplier;
    public GUIText infoText;
    private float addStrength = 5; //1
    private float removeStrength = -3; //-1
    private int size2;

    private LineRenderer lineRenderer;
    public Transform worldRoot;
    private int rotateSpeed = 50; //50
    private Vector3 lastHit = new Vector3(0, 0, 0);
    private Vector3 border; // where this comes from?


    void Start()
    {
        multiplier = axisMax / size;
        size2 = size * size;
        border = new Vector3(size - 1, size - 1, size - 1);
        // get world root
        // read global world variables from there
        CreateWorldC worldSetupScript = worldRoot.gameObject.GetComponent<CreateWorldC>();
        size = worldSetupScript.size;
        axisMin = worldSetupScript.axisMin;
        axisMax = worldSetupScript.axisMax;
        multiplier = axisMax / size;
        //	axisRange = worldSetupScript.size;
        size2 = size * size;
        border = new Vector3(size - 1, size - 1, size - 1);

        lineRenderer = Camera.main.transform.GetComponent<LineRenderer>();
    }

    void Update()
    {

        // add block .. with mouseclick
        if (Input.GetMouseButton(0)) // button is held down
        {

            // TODO: layermask for ray, hit only terrain
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 9999))
            {
                lastHit = hit.point;

                // get object reference

                Transform chunk = hit.transform;

                Vector3 localhit = chunk.InverseTransformPoint(lastHit);

                int hx = (int)(localhit.x / multiplier);
                int hy = (int)(localhit.y / multiplier); // -0.2 is temporary fix for clicking top plane and getting too big values..
                int hz = (int)(localhit.z / multiplier);

                infoText.text = "Name:" + chunk.name + "  Hit:" + hx + "," + hy + "," + hz;

                // show laser
                lineRenderer.SetPosition(0, Camera.main.transform.position + new Vector3(0, -1, 0));
                lineRenderer.SetPosition(1, lastHit);


                MarchingCubesC editChunk = chunk.gameObject.GetComponent<MarchingCubesC>();
                MarchingCubesC targetChunk;

                float modifyPower = 0;

                if (Input.GetKey(KeyCode.LeftShift)) // shift pressed, remove
                {
                    modifyPower = removeStrength;
                }
                else
                { // add
                    modifyPower = addStrength;
                }

                editChunk.ModifyChunkAdd(hx + size * hy + size2 * hz, modifyPower);
                editChunk.UpdateChunk();


                // check diagonals and other neighbours (if we need to affect them)
                if (hx == border.x && hz == border.z) // modify to forward right
                {
                    if (editChunk.neighbours[1] != null)
                    {
                        targetChunk = editChunk.neighbours[1].gameObject.GetComponent<MarchingCubesC>();
                        targetChunk.ModifyChunkAdd(0 + size * hy + size2 * 0, modifyPower);
                        targetChunk.UpdateChunk();
                    }
                }

                if (hx == border.x && hz == 0) // modify to back right
                {
                    if (editChunk.neighbours[3] != null)
                    {
                        targetChunk = editChunk.neighbours[3].gameObject.GetComponent<MarchingCubesC>();
                        targetChunk.ModifyChunkAdd((int)(0 + size * hy + size2 * border.z), modifyPower);
                        targetChunk.UpdateChunk();
                    }
                }

                if (hx == 0 && hz == 0) // modify to back left 
                {
                    if (editChunk.neighbours[5] != null)
                    {
                        targetChunk = editChunk.neighbours[5].gameObject.GetComponent<MarchingCubesC>();
                        targetChunk.ModifyChunkAdd((int)(border.x + size * hy + size2 * border.z), modifyPower);
                        targetChunk.UpdateChunk();
                    }
                }

                if (hx == 0 && hz == border.z) // modify to forward left 
                {
                    if (editChunk.neighbours[7] != null)
                    {
                        targetChunk = editChunk.neighbours[7].gameObject.GetComponent<MarchingCubesC>();
                        targetChunk.ModifyChunkAdd((int)(border.x + size * hy + size2 * 0), modifyPower);

                        targetChunk.UpdateChunk();
                    }
                }





                if (hx == border.x) // modify to RIGHT
                {
                    if (editChunk.neighbours[2] != null)
                    {
                        targetChunk = editChunk.neighbours[2].gameObject.GetComponent<MarchingCubesC>();
                        targetChunk.ModifyChunkAdd((int)(0 + size * hy + size2 * hz), modifyPower);
                        targetChunk.UpdateChunk();
                    }
                }


                if (hx == 0) // modify to LEFT
                {
                    if (editChunk.neighbours[6] != null)
                    {
                        targetChunk = editChunk.neighbours[6].gameObject.GetComponent<MarchingCubesC>();
                        targetChunk.ModifyChunkAdd((int)(border.x + size * hy + size2 * hz), modifyPower);
                        targetChunk.UpdateChunk();
                    }
                }

                if (hz == border.z) // modify to forward
                {
                    if (editChunk.neighbours[0] != null)
                    {
                        targetChunk = editChunk.neighbours[0].gameObject.GetComponent<MarchingCubesC>();
                        targetChunk.ModifyChunkAdd((int)(hx + size * hy + size2 * 0), modifyPower);
                        targetChunk.UpdateChunk();
                    }

                }


                if (hz == 0) // modify to backward
                {
                    if (editChunk.neighbours[4] != null)
                    {
                        targetChunk = editChunk.neighbours[4].gameObject.GetComponent<MarchingCubesC>();
                        targetChunk.ModifyChunkAdd((int)(hx + size * hy + size2 * border.z), modifyPower);
                        targetChunk.UpdateChunk();
                    }
                }

            }
        }


        if (Input.GetMouseButtonUp(0)) // button released
        {
            lineRenderer.SetPosition(0, Camera.main.transform.position);
            lineRenderer.SetPosition(1, Camera.main.transform.position);
        }

        if (Input.GetKey("r")) // reset scene
        {
            Application.LoadLevel(0);
        }

    }
}