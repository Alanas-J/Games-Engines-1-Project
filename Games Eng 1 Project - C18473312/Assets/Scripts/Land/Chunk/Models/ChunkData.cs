using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ChunkData {
    public float[,] heightMap;
    public Color[] colourMap;

    public MapData(float[,] heightMap, Color[] colourMap) {
        this.heightMap = heightMap;
        this.colourMap = colourMap;
    }
}