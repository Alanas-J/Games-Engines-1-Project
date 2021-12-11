using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{

    public enum NormalizeMode{
        Local,Global
    }
    

    public static float[,] GenerateNoiseMap(int map_width, int map_height, int seed, float scale, int octaves, float peristance, float lacunarity, Vector2 offset, NormalizeMode normalizeMode){
        
        // Noisemap Array
        float[,] noise_map = new float[map_width, map_height];

        // RNG Seed
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        
        float maxPossibleHeight = 0;
        float amplitude = 1;
        float frequency = 1;

        for(int i = 0 ; i < octaves; i++ ){
            float offsetX = prng.Next(-100000,100000) + offset.x;
            float offsetY = prng.Next(-100000,100000) - offset.y;

            octaveOffsets[i] = new Vector2(offsetX, offsetY);

            maxPossibleHeight += amplitude;
            amplitude *= peristance;
        }


        // clamping scale to avoid division by 0;
        if(scale <=0){
            scale = 0.0001f;
        }

        // Init of noise height bound ints
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = map_width/2;
        float halfHeight = map_height/2;


        for(int y = 0; y < map_height; y++){
            for(int x = 0; x < map_width; x++){

                frequency = 1;
                amplitude = 1;
                float noiseHeight = 0;

                // For each octave that has different frequency
                for(int i = 0; i < octaves; i++){
                    float sample_x = (x-halfWidth + octaveOffsets[i].x)/scale * frequency;
                    float sample_y = (y-halfHeight + octaveOffsets[i].y)/scale * frequency;

                    // Perlin noise value -1 - 1, so an octave can suptract from another.
                    float perlin_value = (Mathf.PerlinNoise(sample_x,sample_y) * 2) -1;
                    
                    // add the perlinvalue modified by the octaves amplitude.
                    noiseHeight += perlin_value * amplitude;

                    amplitude *= peristance;
                    frequency *= lacunarity;
                }

                // Getting the range of highest to lowest noise values.
                if(noiseHeight > maxNoiseHeight){
                    maxNoiseHeight = noiseHeight;
                } else if (noiseHeight < minNoiseHeight){
                    minNoiseHeight = noiseHeight;
                }


                noise_map[x , y] = noiseHeight;
            }
        }

        // Second nested loop to lerp noisemap into range of 0-1;
        for(int y = 0; y < map_height; y++){
            for(int x = 0; x < map_width; x++){
            

                if(NormalizeMode.Local == normalizeMode){
                    //works if only 1 chunk noise_map                   // local noise height rename later
                   noise_map[x, y]  = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noise_map[x,y]);
                } else{
                    float normalizedHeight = (noise_map[x,y] +1)/(2f *maxPossibleHeight/ 2f);
                    noise_map[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
                }
                
            }
        }    

        

        return noise_map;
    }
}
