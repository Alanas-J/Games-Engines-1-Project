using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used as the source of sea waves
public static class SeaNoiseSource {
    
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale, Vector2 offset){
        float[,] noiseMap = new float[mapWidth, mapHeight];
        
        // Vector given is in the centre of noisemap.
        float halfWidth = mapWidth/2;
        float halfHeight = mapHeight/2;


        //Height Sampling Loop
        for(int y = 0; y < mapHeight; y++){
            for(int x = 0; x < mapWidth; x++){

                // No Octaves since we want smooth noise.
                float sample_x = (x-halfWidth + offset.x)/scale;
                float sample_y = (y+halfHeight  - offset.y)/scale;

                

                noiseMap[y,x] = Mathf.PerlinNoise(sample_x,sample_y);
            }
        }
        return noiseMap;
    }

    // WILLL NEED TO BE TESTED TO SEE IF ITS WORKING
    public static float GetHeightAtPoint(float scale, Vector2 offset){
        return Mathf.PerlinNoise(offset.x/scale, offset.y/scale);
    } 


}    
