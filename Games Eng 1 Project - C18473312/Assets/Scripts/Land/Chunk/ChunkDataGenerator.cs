using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

// Job of this script is to output Terrain Chunk Data (Heightmap and colour map)
public class ChunkDataGenerator : MonoBehaviour
{
    // Constant Variables
    public const int chunkSize = 241; // 241 vertices, making a 240 length piece.

    // Configurable Variables
    public float noiseScale; // How much zoom into the noise map.
    public int octaves; // No of perlin noise maps used.
    [Range(0,1)]
    public float peristance; // Strength of each upper noise map.
    public float lacunarity; // Frequency scale increase for each octave
    
    public int seed; // RNG Seed
    public Vector2 offset; // Input offset.

    public float heightMultiplier;
    public AnimationCurve meshHeightCurve; // Curve perlin input. Exactly like using colour curves for editing photos.

    public TerrainLayer[] terrainLayers; // Used to colour map


    // =====================================================================

    ChunkData GenerateChunkDataForPoint(Vector2 centre){
        float[,] noiseMap = Noise.GenerateNoiseMap(chunkSize, chunkSize, seed, noiseScale, octaves, peristance, lacunarity, centre+offset);
        Color[] colourMap = new Color[chunkSize*chunkSize]; // Colours need to be stored in a 1D array;
        
        for(int y = 0; y < chunkSize; y++){
            for(int x = 0; x < chunkSize; x++){
                float currentHeight = noiseMap[x,y];

                // Cycle through all terrain height layers
                for(int i = 0; i<layers.Length; i++){
                    
                    if(currentHeight >= layers[i].height){
                        colourMap[y*chunkSize+ x] = layers[i].colour;
                    } 
                }
            }
        }

        // Output is a map of values to create a mesh and texture from.
        return new ChunkData(noiseMap,colourMap);
    }

    [System.Serializable] // Makes Struct Visible to unity
    public struct TerrainType {
        public string name;
        public float height;
        public Color colour;
    }

    // ================== Multithreading code ============================

    // Thread Queues (Will need to come back to this.)
    //Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>(); 
    //Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();



    public struct MapThreadInfo<T>{
        public readonly Action<T> callback;
        public readonly T parameter;

        public MapThreadInfo(Action<T> callback, T parameter){
            this.callback = callback;
            this.parameter = parameter;
        }
    }









    
    public void DrawMapInEditor(){
        // Will Figure out later most likely different class
    }



    // Validations of input
    void OnValidate() {
        if(lacunarity < 1){
            lacunarity = 1;
        }
        if(octaves < 0){
            octaves = 0;
        }
    }
}
