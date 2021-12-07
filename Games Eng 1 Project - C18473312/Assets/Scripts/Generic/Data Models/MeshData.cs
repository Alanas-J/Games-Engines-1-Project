using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData {
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    int triangleIndex; // Used to keep track of all triangles

    public MeshData(int meshWidth, int meshHeight) {
        
        // No of vertices in mesh
        vertices = new Vector3[meshWidth * meshHeight];

        // Texture Coordinates
        uvs = new Vector2[meshWidth * meshHeight]; 

        // 2 Triangles of 3 points are made per square in mesh.
        triangles = new int[(meshWidth-1) * (meshHeight-1) *6];
    }

    public void AddTriangle(int a, int b, int c){
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }

    public Mesh CreateMesh(){
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}