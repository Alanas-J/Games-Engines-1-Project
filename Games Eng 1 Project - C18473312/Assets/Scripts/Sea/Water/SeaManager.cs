using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaManager : MonoBehaviour{
    // Inputs =============================
    public Vector2 seaOffset;
    public Vector2 seaSpeed;
    public float waveWidth = 10; // Divides into the noisemap to zoom/crop/spread out waves.  

    // Used by other scripts to scale up the noisemap.
    public float maxWaveHeight = 10; // Multiplies noisemap.
    public float minWaveHeight = 0; // Added to noisemap.

    

    // Internal variables ==============================
    public static SeaManager instance; // Singleton instance

    // Lifecycle methods =================================
    void Update(){
        seaOffset = seaOffset + (seaSpeed * Time.deltaTime);
    }
    void Awake(){
        if(instance == null){
            instance = this;
        }
        else if(instance != this){
            Debug.Log("Sea manager instance already running, destroying duplicate instance.");
            Destroy(this);
        }
    }

    // Methods to fetch Sea State ========================

    // Implementation noisemap goes here. Perlin is my choice for ease.
    public float GetNormalizedHeightAtPoint(float x, float y){
        float x_coordinate = x/waveWidth + seaOffset.x;
        float y_coordinate = y/waveWidth + seaOffset.y;


        return Mathf.PerlinNoise(x_coordinate, y_coordinate);
    }

    // Heightmap generation for terrain genration.
    public float[,] GetNormalizedHeightMap(Vector3 position, int width, int heightMapResolution){

        float[,] heights = new float[heightMapResolution, heightMapResolution];

    
        for (int x = 0; x < heightMapResolution; x++) {
            for (int z = 0; z < heightMapResolution; z++){

                // Mapping resolution points to the terrain width.
                float mappedX =  x *(width/heightMapResolution);
                float mappedZ =  z *(width/heightMapResolution);

                // Translating to the positon.
                mappedX += position.x;
                mappedZ += position.z;

                // Assign in row + column format. As per terrain setheightmap docs.
                heights[z, x] = GetNormalizedHeightAtPoint(mappedX, mappedZ); // Now need to look into the ocean controller

            }
        }
        return heights;
    }


}
