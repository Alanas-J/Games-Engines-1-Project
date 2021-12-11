using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class SeaChunkDataController : MonoBehaviour
{
    // Constant Variables
    public const int chunkSize = 257; // 241 vertices, making a 240 length piece.

    // Configurable Variables
    public float waveFrequency; // How much zoom into the noise map.
    
    public Vector2 offset; // Input offset.
    public Vector2 offsetSpeed; // Ammount waves change.

    public float waveHeightMultiplier; // amplitude

    // ================== Core Functionality Code =========================
    ChunkData GenerateChunkDataForPoint(Vector2 centre){
        float[,] noiseMap = SeaNoiseSource.GenerateNoiseMap(chunkSize, chunkSize,  waveFrequency, new Vector2(centre.x+offset.x, centre.y + offset.y));
        //float[,] noiseMap = Noise.GenerateNoiseMap(chunkSize, chunkSize, seed, noiseScale, octaves, peristance, lacunarity, centre+offset, Noise.NormalizeMode.Global);
        
        return new ChunkData(noiseMap);
    }


    // Will be used by floaters
    float GetHeightAtPoint(Vector2 centre){
        return waveHeightMultiplier*SeaNoiseSource.GetHeightAtPoint(waveFrequency, centre+offset);
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
        MeshData meshData = MeshDataGenerator.GenerateChunkMesh(mapData.heightMap, waveHeightMultiplier, lod);

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
        offset = offset + (offsetSpeed * Time.deltaTime);

        CheckThreadQueues();
    }

    // ================== Validations of Input code ============================
    void OnValidate() {
        if(waveFrequency <=0){
            waveFrequency = 0.0001f;
        }

    }
}