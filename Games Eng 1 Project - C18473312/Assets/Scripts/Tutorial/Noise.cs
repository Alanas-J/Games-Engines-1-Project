using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int map_width, int map_height, float scale){

        float[,] noise_map = new float[map_width, map_height];

        // clamping scale to avoid division by 0;
        if(scale <=0){
            scale = 0.0001f;
        }


        for(int y = 0; y < map_height; y++){
            for(int x = 0; x < map_width; x++){

                float sample_x = x/scale;
                float sample_y = y/scale;

                float perlin_value = Mathf.PerlinNoise(sample_x,sample_y);
                noise_map[x , y] = perlin_value;
            }
        }


        return noise_map;
    }
}
