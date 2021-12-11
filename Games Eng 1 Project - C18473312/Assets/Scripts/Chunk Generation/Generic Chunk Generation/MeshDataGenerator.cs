using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshDataGenerator {
    
    // Takes in a heightmap, amplitude multiplier, height curve and Lod divider
    public static MeshData GenerateChunkMesh(float[,] heightMap, float heightMultiplier, AnimationCurve _heightCurve, int levelOfDetail) {
        
        // ================= Parametres ======================
        
        AnimationCurve heightCurve = new AnimationCurve (_heightCurve.keys); // New Instance of animation curve is made for thread safety.

        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;
        // eg. 25 point square, 5 point width   centre = 3 (3-1)/-2 = -2 ,   3-2 point is leftmost. Z is same logic with addtion as its a positive translation to top.

        // LOD values are mapped to 1, 2, 4, 6, 8, 10, 12. Factors of 240.
        int verticeReductionRatio = levelOfDetail == 0 ? 1 : levelOfDetail * 2;

        int verticesPerLine = ((width-1)/verticeReductionRatio) + 1;
        //  eg. 5 point square is of 4 vertice length, divided by 2 its 2, to make a 2 length line +1 vertice is needed .
        // ====================================================


        MeshData meshData = new MeshData(verticesPerLine, verticesPerLine);
        int vertexIndex = 0;

        // For each vertice of mesh.
        for(int y = 0; y < height; y+= verticeReductionRatio){
            for(int x = 0; x < width; x+= verticeReductionRatio){

                // Vertices are added from the topleft corner, height is translated by the height curve.
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x,y])  * heightMultiplier, topLeftZ - y);
                
                meshData.uvs[vertexIndex] = new Vector2(x/(float) width, y/(float) height);

                // Ignoring leftmost and rightmost vertices as no space for triangles.
                if(x < width-1 && y < height - 1){

                    /* Two triangles in the shape of a square are made. In unity the face of a polygon is calculated clockwise.
                        In a square of points
                        AB
                        CD           First triangle is A, D, C
                                             Second is D, A, B

                    */
                    meshData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
                    meshData.AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        return meshData;
    }


    // Method overload without animation curve.
    public static MeshData GenerateChunkMesh(float[,] heightMap, float heightMultiplier, int levelOfDetail) {
        
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;
        // eg. 25 point square, 5 point width   centre = 3 (3-1)/-2 = -2 ,   3-2 point is leftmost. Z is same logic with addtion as its a positive translation to top.

        // LOD values are mapped to 1, 2, 4, 6, 8, 10, 12. Factors of 240.
        int verticeReductionRatio = levelOfDetail == 0 ? 1 : levelOfDetail * 2;

        int verticesPerLine = ((width-1)/verticeReductionRatio) + 1;
        //  eg. 5 point square is of 4 vertice length, divided by 2 its 2, to make a 2 length line +1 vertice is needed .
        // ====================================================

        MeshData meshData = new MeshData(verticesPerLine, verticesPerLine);
        int vertexIndex = 0;

        // For each vertice of mesh.
        for(int y = 0; y < height; y+= verticeReductionRatio){
            for(int x = 0; x < width; x+= verticeReductionRatio){

                // Vertices are added from the topleft corner, height is translated by the height curve.
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightMap[x,y]  * heightMultiplier, topLeftZ - y);
                
                meshData.uvs[vertexIndex] = new Vector2(x/(float) width, y/(float) height);

                // Ignoring leftmost and rightmost vertices as no space for triangles.
                if(x < width-1 && y < height - 1){

                    /* Two triangles in the shape of a square are made. In unity the face of a polygon is calculated clockwise.
                        In a square of points
                        AB
                        CD           First triangle is A, D, C
                                             Second is D, A, B

                    */
                    meshData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
                    meshData.AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        return meshData;
    }



}
