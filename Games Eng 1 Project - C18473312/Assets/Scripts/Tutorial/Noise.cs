using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int map_width, int map_height, int seed, float scale, int octaves, float peristance, float lacunarity, Vector2 offset){
        
        // Noisemap Array
        float[,] noise_map = new float[map_width, map_height];

        // RNG Seed
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for(int i = 0 ; i < octaves; i++ ){
            float offsetX = prng.Next(-100000,100000) + offset.x;
            float offsetY = prng.Next(-100000,100000) + offset.y;

            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }


        // clamping scale to avoid division by 0;
        if(scale <=0){
            scale = 0.0001f;
        }

        // Init of noise height bound ints
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for(int y = 0; y < map_height; y++){
            for(int x = 0; x < map_width; x++){


                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                // For each octave that has different frequency
                for(int i = 0; i < octaves; i++){
                    float sample_x = x/scale * frequency + octaveOffsets[i].x;
                    float sample_y = y/scale * frequency + octaveOffsets[i].y;

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
            
            noise_map[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noise_map[x,y]);
            }
        }    

        

        return noise_map;
    }
}
