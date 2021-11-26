using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanMeshController : MonoBehaviour
{
    public int terrain_width = 256;
    public int terrain_height = 256;
    public float terrain_scale = 10f;

    void Start()
    {
        
        
    }

    void Update(){
        Terrain water = GetComponent<Terrain>();
        water.terrainData = GenerateOcean(water.terrainData);
    }

    TerrainData GenerateOcean(TerrainData data)
    {
        data.heightmapResolution = terrain_width + 1;

        data.size = new Vector3(terrain_width,  OceanController.instance.amplitude, terrain_height);  // The Vector3 takes in 3 integer values, but which values and in what order?
        data.SetHeights(0, 0, GetHeights());

        return data;
    }

    float[,] GetHeights()
    {
        float[,] heights = new float[terrain_width, terrain_height];
        for (int x = 0; x < terrain_width; x++) 
        {
            for (int y = 0; y < terrain_height; y++)
            {
                heights[x, y] = OceanController.instance.GetOceanHeight(x+terrain_width, y+terrain_height);
            }
        }
        return heights;
    }

}
