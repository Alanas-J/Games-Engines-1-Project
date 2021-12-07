using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class MapGenerator : MonoBehaviour
{
    // Flick between generation output.
    public enum DrawMode{
        NoiseMap, ColourMap, Mesh
    }
    public DrawMode drawMode;

    public Noise.NormalizeMode normalizeMode;

    // Mesh size limiter for LOD changing, replaced map width and map height
    public const int mapChunkSize = 241;
    
    // Slider
    [Range(0,6)]
    public int previewLOD; // Divide no of vertices in plane. must be (w-1)%input = 0
    
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

    // ThreadQueue
    Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>(); 
    Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();


    public void DrawMapInEditor(){
        MapData mapData = GenerateMapData(Vector2.zero);

        MapDisplay display = FindObjectOfType<MapDisplay> ();
        if(drawMode == DrawMode.NoiseMap){
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
        } else if(drawMode == DrawMode.ColourMap){
            display.DrawTexture(TextureGenerator.TextureFromColourMap(mapData.colourMap, mapChunkSize, mapChunkSize));
        }
        else if(drawMode == DrawMode.Mesh){
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve, previewLOD), TextureGenerator.TextureFromColourMap(mapData.colourMap, mapChunkSize, mapChunkSize));
        }
    }


    // Multithreading code
    public void RequestMapData(Vector2 centre, Action<MapData> callback){
        ThreadStart threadStart = delegate{
            MapDataThread(centre, callback);
        };
        new Thread(threadStart).Start();
    }

    void MapDataThread(Vector2 centre, Action<MapData> callback){
        MapData mapData = GenerateMapData(centre);

        // Mutexes in C#
        lock(mapDataThreadInfoQueue){
            mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<MapData>(callback, mapData));
        }
    
    }

    public void RequestMeshData(MapData mapData, int lod, Action<MeshData> callback){
        ThreadStart threadStart = delegate{
            MeshDataThread(mapData, lod,  callback);
        };
        new Thread(threadStart).Start();
    }

    void MeshDataThread(MapData mapData,int lod, Action<MeshData> callback){
        MeshData meshData = MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve, lod);

        // Mutexes in C#
        lock(meshDataThreadInfoQueue){
            meshDataThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData>(callback, meshData));
        }
    
    }
    //=====================================

    void Update(){

        // If map thread buffer has data
        if(mapDataThreadInfoQueue.Count > 0){
            for(int i = 0; i < mapDataThreadInfoQueue.Count; i++){
                MapThreadInfo<MapData> threadInfo = mapDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }

        // If mesh thread buffer has data
        if(meshDataThreadInfoQueue.Count > 0){
            for(int i = 0; i < meshDataThreadInfoQueue.Count; i++){
                MapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
    }

    MapData GenerateMapData(Vector2 centre){
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, peristance, lacunarity, centre+offset, normalizeMode);

        // Colouring code
        Color[] colourMap = new Color[mapChunkSize*mapChunkSize];
        for(int y = 0; y < mapChunkSize; y++){
            for(int x = 0; x < mapChunkSize; x++){
                float currentHeight = noiseMap[x,y];

                for(int i = 0; i<regions.Length; i++){
                    
                    if(currentHeight >= regions[i].height){

                        // Using a 1d array as 2d
                        colourMap[y*mapChunkSize+ x] = regions[i].colour;
                        //break;
                    } 
                }
            }
        }

      
        return new MapData(noiseMap,colourMap);
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

    public struct MapThreadInfo<T>{
        public readonly Action<T> callback;
        public readonly T parameter;

        public MapThreadInfo(Action<T> callback, T parameter){
            this.callback = callback;
            this.parameter = parameter;
        }
    }
}


public struct MapData{
    public float[,] heightMap;
    public Color[] colourMap;

    public MapData(float[,] heightMap, Color[] colourMap){
        this.heightMap = heightMap;
        this.colourMap = colourMap;
    }
}