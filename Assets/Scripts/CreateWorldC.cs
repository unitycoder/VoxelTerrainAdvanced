using System;
using UnityEngine;

public class CreateWorldC : MonoBehaviour
{
    public Transform chunk;

    // BUG: Most values wont work too well.. also you might have to adjust shader values to match water height
    public int size = 16;     // number of cubes along a side = resolution (cannot put too high, 65k vertex limit per mesh)
    public float axisMin = 0; // chunk pivot 0,0,0 (dont touch)
    public float axisMax = 8;  // chunk width in world units
    private float multiplier; // internal

    // How Many Chunks (dont try too many.. they are all created at start)
    public int sizeX = 8;
    private int sizeY = 8; // not used yet
    public int sizeZ = 8;

    // instantiate all chunks
    void Awake()
    {
        multiplier = axisMax / size;
        int y = 0;
        int counter = 0;
        for (int z = 0; z < sizeZ; z++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                Transform clone;
                clone = Instantiate(chunk, transform.position + new Vector3(x, 0, z) * axisMax, transform.rotation) as Transform;
                clone.parent = transform;
                clone.name = "zChunk_" + clone.position.x + "_" + clone.position.y + "_" + clone.position.z;

                // set offset
                MarchingCubesC other = clone.gameObject.GetComponent<MarchingCubesC>();
                
                other.offset = new Vector3(x * (size - 1), y * (size - 1), z * (size - 1));
                //Debug.Log(other.offset + ", " + new Vector3(x, y, z));
                counter++;
            }
        }

        // get list of our neighbours
        foreach (Transform child in transform) // Downcast notice..uh..
        {
            MarchingCubesC childChunk = child.gameObject.GetComponent<MarchingCubesC>();

            Vector3 currentPos = child.position; ///multiplier;
            // scan forward
            String searchName = "zChunk_" + currentPos.x + "_" + currentPos.y + "_" + (currentPos.z + axisMax);
            GameObject neighbour = GameObject.Find(searchName);
            if (neighbour != null)
            {
                childChunk.neighbours[0] = neighbour.transform;
            }

            // scan forward-right
            searchName = "zChunk_" + (currentPos.x + axisMax) + "_" + currentPos.y + "_" + (currentPos.z + axisMax);
            neighbour = GameObject.Find(searchName);
            if (neighbour != null)
            {
                childChunk.neighbours[1] = neighbour.transform;
            }

            // scan right
            searchName = "zChunk_" + (currentPos.x + axisMax) + "_" + currentPos.y + "_" + currentPos.z;
            neighbour = GameObject.Find(searchName);
            if (neighbour != null)
            {
                childChunk.neighbours[2] = neighbour.transform;
            }

            // scan backward-right
            searchName = "zChunk_" + (currentPos.x + axisMax) + "_" + currentPos.y + "_" + (currentPos.z - axisMax);
            neighbour = GameObject.Find(searchName);
            if (neighbour != null)
            {
                childChunk.neighbours[3] = neighbour.transform;
            }

            // scan backward
            searchName = "zChunk_" + currentPos.x + "_" + currentPos.y + "_" + (currentPos.z - axisMax);
            neighbour = GameObject.Find(searchName);
            if (neighbour != null)
            {
                childChunk.neighbours[4] = neighbour.transform;
            }

            // scan backward-left
            searchName = "zChunk_" + (currentPos.x - axisMax) + "_" + currentPos.y + "_" + (currentPos.z - axisMax);
            neighbour = GameObject.Find(searchName);
            if (neighbour != null)
            {
                childChunk.neighbours[5] = neighbour.transform;
            }

            // scan left
            searchName = "zChunk_" + (currentPos.x - axisMax) + "_" + currentPos.y + "_" + currentPos.z;
            neighbour = GameObject.Find(searchName);
            if (neighbour != null)
            {
                childChunk.neighbours[6] = neighbour.transform;
            }

            // scan forward-left
            searchName = "zChunk_" + (currentPos.x - axisMax) + "_" + currentPos.y + "_" + (currentPos.z + axisMax);
            neighbour = GameObject.Find(searchName);
            if (neighbour != null)
            {
                childChunk.neighbours[7] = neighbour.transform;
            }

        }
    }
}
