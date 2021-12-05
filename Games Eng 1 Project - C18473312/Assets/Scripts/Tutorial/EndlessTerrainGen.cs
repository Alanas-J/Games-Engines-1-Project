using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrainGen : MonoBehaviour
{
    public const float renderDistance = 450;
    public Transform viewer;


    public static Vector2 viewerPosition;
    int chunkSize;
    int chunksVisible;

    Dictionary<Vector2, TerrainChunk> terrainChunks = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> previousVisibleTerainChunks = new List<TerrainChunk>();


    // On start
    void Start(){
        chunkSize = MapGenerator.mapChunkSize-1;
        chunksVisible = Mathf.RoundToInt(renderDistance/chunkSize);
    }

    void Update(){
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);
        UpdateVisibleChunks();
    }
    


    void UpdateVisibleChunks(){

        // Set all old chunks invisible so visibility
        for(int i = 0; i < previousVisibleTerainChunks.Count; i++){
            previousVisibleTerainChunks[i].SetVisible(false);
        }
        previousVisibleTerainChunks.Clear();


        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);


        for(int yOffset = -chunksVisible; yOffset <= chunksVisible; yOffset++){
            for(int xOffset = -chunksVisible; xOffset <= chunksVisible; xOffset++){
               Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                // check if chunk already added.
                if(terrainChunks.ContainsKey(viewedChunkCoord)){
                    terrainChunks[viewedChunkCoord].UpdateChunk();
                    if(terrainChunks[viewedChunkCoord].isVisible()){
                        previousVisibleTerainChunks.Add(terrainChunks[viewedChunkCoord]);
                    }
                } else{
                    terrainChunks.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, transform));
                }
            }
        }
    }

    public class TerrainChunk{
        
        GameObject meshObject;
        Vector2 position;

        // will need to look into bounds
        Bounds bounds;

        public TerrainChunk(Vector2 coord, int size, Transform parent){
            position = coord * size;
            bounds = new Bounds(position, Vector2.one*size);

            Vector3 positionV3 = new Vector3(position.x, 0, position.y);

            meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            meshObject.transform.position = positionV3;
            meshObject.transform.localScale = Vector3.one* size/10f;
            meshObject.transform.parent = parent;


            SetVisible(false);
        }


        public void UpdateChunk(){
            float viewerDistanceFromEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));

            bool isVisible = viewerDistanceFromEdge <= renderDistance;
            SetVisible(isVisible);

        }

        public void SetVisible(bool visible){
            meshObject.SetActive(visible);
        }

        public bool isVisible(){
            return meshObject.activeSelf;
        }


    }
}
