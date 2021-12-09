using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanMeshController : MonoBehaviour
{
    public int terrain_width = 256;
    public int terrain_depth = 256;
    public float terrain_scale = 10f;
    Terrain water;

    void Start()
    {
        water = GetComponent<Terrain>();
        
    }

    void Update(){
        water.terrainData = GenerateOcean(water.terrainData);
    }
 
    TerrainData GenerateOcean(TerrainData data)
    {
        // Will need to read into heightmapResolution
        data.heightmapResolution = 256; // Was set to width +1 originally

        // Size of the terrain, X axis, Y axis and Z axis
        data.size = new Vector3(terrain_width,  OceanController.instance.amplitude, terrain_depth);

        // Will need to check what all parametres do ( most likely culprit);
        data.SetHeights(0, 0, GetHeights());

        return data;
    }

    float[,] GetHeights()
    {
        // 2D array of heights to collect height data from the ocean controller.

        // An array is stored as row x column (IMPORTANT to solve the XY and array mismatch.)
        float[,] heights = new float[256, 256];

        // For X co-ord
        for (int x = 0; x < 256; x++) 
        {
            // For Z co-ord
            for (int z = 0; z < 256; z++)
            {
                // Assign in row + column format.
                heights[z, x] = OceanController.instance.GetOceanHeight(x, z); // Now need to look into the ocean controller

            }
        }
        return heights;
    }

}
