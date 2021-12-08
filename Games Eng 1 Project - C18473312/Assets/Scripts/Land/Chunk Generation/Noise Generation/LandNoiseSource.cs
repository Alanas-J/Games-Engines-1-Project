using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used as the source of land terrain
public static class LandNoiseSource {

    static System.Random randomNoGenerator;
    
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset){
        float[,] noiseMap = new float[mapWidth, mapHeight];
        
         randomNoGenerator = new System.Random(seed);
        
        // Parametres to ensure correct noisemap.
        Vector2[] randomOctaveOffsets = GetRandomOffsets(octaves, offset); // PseudoRandom offsets to sample different parts of perlin.
        float maxPossibleHeight = GetPossibleHeightMax(octaves, persistance); // Getting height to normalize.
        
        // Vector given is in the centre of noisemap.
        float halfWidth = mapWidth/2;
        float halfHeight = mapHeight/2;


        //Height Sampling Loop
        for(int y = 0; y < mapHeight; y++){
            for(int x = 0; x < mapWidth; x++){

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for(int i = 0; i < octaves; i++){
                    float sample_x = (x-halfWidth + randomOctaveOffsets[i].x + offset.x)/scale * frequency;
                    float sample_y = (y+halfHeight + randomOctaveOffsets[i].y - offset.y)/scale * frequency;

                    // Perlin noise value -1 - 1, so an octave can suptract from another.
                    float perlin_value = (Mathf.PerlinNoise(sample_x,sample_y) * 2) -1;
                    
                    // add the perlinvalue modified by the octaves amplitude.
                    noiseHeight += perlin_value * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                noiseMap[x,y] = noiseHeight;
            }
        }
        // Loop to normalize values to capture most in height 0-1. Absolute 0 and -1 are rare so numbers are multiplied
        // Could have a more elegant solution
        for(int y = 0; y < mapHeight; y++){
            for(int x = 0; x < mapWidth; x++){

                // Number is pushed into positives and divided by 2*heightmax to get perfect normalization.
                // But max possible height is divided to spread more values into 0-1 range
                float normalizedHeight = (noiseMap[x,y] +1)/(2f *maxPossibleHeight/ 2f);
                    noiseMap[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue); // Clamp on 0 to ensure no negatives.
                }
        }    
        return noiseMap;
    }

    private static Vector2[] GetRandomOffsets(int octaves, Vector2 offset) {
        Vector2[] randomOctaveOffsets = new Vector2[octaves];

        for(int i = 0 ; i < octaves; i++ ){
            float offsetX = randomNoGenerator.Next(-100000,100000) + offset.x;
            float offsetY = randomNoGenerator.Next(-100000,100000) - offset.y;
            randomOctaveOffsets[i] = new Vector2(offsetX, offsetY);
        }
        return randomOctaveOffsets;
    }

    private static float GetPossibleHeightMax(int octaves, float persistance) {
        float maxPossibleHeight = 0;
        float amplitude = 1;

        for(int i = 0 ; i < octaves; i++ ){
            maxPossibleHeight += amplitude;
            amplitude *= persistance;        }
        return maxPossibleHeight;
    }
}    
