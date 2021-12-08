using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve _heightCurve, int levelOfDetail){
        
        // Animation curve breaks with multi threading;
        AnimationCurve heightCurve = new AnimationCurve (_heightCurve.keys);

        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;


        // tutorial used a ternary for this, I could tooo.
        int meshSimplificationInc;
        if(levelOfDetail == 0){
            meshSimplificationInc = 1;
        }
        else{
            meshSimplificationInc = levelOfDetail * 2;
        }

        int verticesPerLine = (width-1)/meshSimplificationInc+1;
         

        MeshData meshData = new MeshData(verticesPerLine, verticesPerLine);
        int vertexIndex = 0;

        for(int y = 0; y < height; y+= meshSimplificationInc){
            for(int x = 0; x < width; x+= meshSimplificationInc){

                                                                // To Center the mesh
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x,y])  * heightMultiplier, topLeftZ - y);
                meshData.uvs[vertexIndex] = new Vector2(x/(float) width, y/(float) height);

                if(x < width-1 && y < height - 1){
                    // square out of triangles, ignoring right and bottom edges of array as they dont have enough points.
                    meshData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
                    meshData.AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        return meshData;
    }
}
