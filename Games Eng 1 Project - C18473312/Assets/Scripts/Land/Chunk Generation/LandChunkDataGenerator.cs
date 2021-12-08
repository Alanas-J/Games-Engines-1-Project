using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

// Job of this script is to output Terrain Chunk Data (Heightmap and colour map)
public class LandChunkDataGenerator : MonoBehaviour
{
    // Constant Variables
    public const int chunkSize = 241; // 241 vertices, making a 240 length piece.

    // Configurable Variables
    public float noiseScale; // How much zoom into the noise map.
    public int octaves; // No of perlin noise maps used.
    [Range(0,1)] 
    public float peristance; // Strength of each upper noise map. // Suboctaves should not become bigger leading to range 0-1.
    public float lacunarity; // Frequency scale increase for each octave
    
    public int seed; // RNG Seed
    public Vector2 offset; // Input offset.

    public float heightMultiplier;
    public AnimationCurve meshHeightCurve; // Curve perlin input. Exactly like using colour curves for editing photos.

    public TerrainLayer[] terrainLayers; // Used to colour map


    // ================== Core Functionality Code =========================

    ChunkData GenerateChunkDataForPoint(Vector2 centre){
        float[,] noiseMap = LandNoiseSource.GenerateNoiseMap(chunkSize, chunkSize, seed, noiseScale, octaves, peristance, lacunarity, centre+offset);
        Color[] colourMap = new Color[chunkSize*chunkSize]; // Colours need to be stored in a 1D array;
        
        for(int y = 0; y < chunkSize; y++){
            for(int x = 0; x < chunkSize; x++){
                float currentHeight = noiseMap[x,y];

                // Cycle through all terrain height layers
                for(int i = 0; i < terrainLayers.Length; i++){
                    
                    if(currentHeight >= terrainLayers[i].height){
                        colourMap[y*chunkSize+ x] = terrainLayers[i].colour;
                    } 
                }
            }
        }

        // Output is a map of values to create a mesh and texture from.
        return new ChunkData(noiseMap,colourMap);
    }

    [System.Serializable] // Makes Struct Visible to unity
    public struct TerrainLayer {
        public string name;
        public float height;
        public Color colour;
    }

    // ================== Multithreading code ============================

    // Thread Queues
    Queue<DataCallbackPair<ChunkData>> chunkDataThreadOutputQueue = new Queue<DataCallbackPair<ChunkData>>(); 
    Queue<DataCallbackPair<MeshData>> meshDataThreadOutputQueue = new Queue<DataCallbackPair<MeshData>>();

    public struct DataCallbackPair<T>{
        public readonly Action<T> callback;
        public readonly T data;

        public DataCallbackPair(Action<T> callback, T data){
            this.callback = callback;
            this.data = data;
        }
    }


    // === Request Data on Thread Calls
    /*
        1. Requests are used to instanciate a thread.
        2. Threads complete work and add to queues.
        3. Main thread fetches data off of queue and sends in callback.
    */
    public void RequestChunkData(Vector2 centre, Action<ChunkData> callback){
        ThreadStart threadStart = delegate{
            ChunkDataRequestThread(centre, callback);
        };
        new Thread(threadStart).Start();
    }
    void ChunkDataRequestThread(Vector2 centre, Action<ChunkData> callback){
        ChunkData chunkData = GenerateChunkDataForPoint(centre);

        lock(chunkDataThreadOutputQueue){
            chunkDataThreadOutputQueue.Enqueue(new DataCallbackPair<ChunkData>(callback, chunkData));
        }
    }
    
    public void RequestMeshData(ChunkData mapData, int lod, Action<MeshData> callback){
        ThreadStart threadStart = delegate{
            MeshDataThread(mapData, lod,  callback);
        };
        new Thread(threadStart).Start();
    }
    void MeshDataThread(ChunkData mapData,int lod, Action<MeshData> callback){
        MeshData meshData = MeshGenerator.GenerateTerrainMesh(mapData.heightMap, heightMultiplier, meshHeightCurve, lod);

        lock(meshDataThreadOutputQueue){
            meshDataThreadOutputQueue.Enqueue(new DataCallbackPair<MeshData>(callback, meshData));
        }
    
    }

    // For the main thread to call
    void CheckThreadQueues(){
        if(chunkDataThreadOutputQueue.Count > 0){
            for(int i = 0; i < chunkDataThreadOutputQueue.Count; i++){
                DataCallbackPair<ChunkData> threadOutput = chunkDataThreadOutputQueue.Dequeue();
                threadOutput.callback(threadOutput.data);
            }
        }
        if(meshDataThreadOutputQueue.Count > 0){
            for(int i = 0; i < meshDataThreadOutputQueue.Count; i++){
                DataCallbackPair<MeshData> threadOutput = meshDataThreadOutputQueue.Dequeue();
                threadOutput.callback(threadOutput.data);
            }
        }

    }
    // ============================================================================================

    void Update(){
        CheckThreadQueues();
    }

    // ================== Validations of Input code ============================
    void OnValidate() {
        if(lacunarity < 1){
            lacunarity = 1;
        }
        if(octaves < 1){
            octaves = 1;
        }

        if(octaves < 0){
            octaves = 0;
        }
        if(noiseScale <=0){
            noiseScale = 0.0001f;
        }

    }
}
