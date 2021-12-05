using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // Flick between generation output.
    public enum DrawMode{
        NoiseMap, ColourMap, Mesh
    }
    public DrawMode drawMode;

    // Mesh size limiter for LOD changing, replaced map width and map height
    public const int mapChunkSize = 241;
    
    // Slider
    [Range(0,6)]
    public int levelOfDetail; // Divide no of vertices in plane. must be (w-1)%input = 0
    
    // How much perlin gets divided into (zoom)
    public float noiseScale;
    // Ammount of perlin noise maps used.
    public int octaves;

    // Slider
    [Range(0,1)]
    public float peristance;
    public float lacunarity;
    
    public int seed;
    public Vector2 offset;
    public float meshHeightMultiplier;

    // Curve perlin input. Exactly like using colour curves for editing photos.
    public AnimationCurve meshHeightCurve;

    public bool updateOnChange;

    public TerrainType[] regions;


    //public void()

    public void GenerateMap(){
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, peristance, lacunarity, offset);

        // Colouring code
        Color[] colourMap = new Color[mapChunkSize*mapChunkSize];
        for(int y = 0; y < mapChunkSize; y++){
            for(int x = 0; x < mapChunkSize; x++){
                float currentHeight = noiseMap[x,y];

                for(int i = 0; i<regions.Length; i++){
                    
                    if(currentHeight <= regions[i].height){

                        // Using a 1d array as 2d
                        colourMap[y*mapChunkSize+ x] = regions[i].colour;
                        break;
                    } 
                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay> ();
        if(drawMode == DrawMode.NoiseMap){
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        } else if(drawMode == DrawMode.ColourMap){
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
        }
        else if(drawMode == DrawMode.Mesh){
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
        }
        
    }

    // How to limit editor config values
    void OnValidate() {
        if(lacunarity < 1){
            lacunarity = 1;
        }
        if(octaves < 0){
            octaves = 0;
        }
    }
}

// Data type visble to unity.
[System.Serializable]
public struct TerrainType  {
    public string name;
    public float height;
    public Color colour;
}