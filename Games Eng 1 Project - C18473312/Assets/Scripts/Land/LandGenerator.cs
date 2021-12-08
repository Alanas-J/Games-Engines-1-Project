using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandGenerator : MonoBehaviour {
    
    // ========================= Parametres =================================
    const float scale = 1f;

    // How far the player will need to move before chunks will be updated.
    const float viewerMovementUpdateThreshold = 25f;
    const float sqrViewerMovementUpdateThreshold = viewerMovementUpdateThreshold * viewerMovementUpdateThreshold;
    public static Vector2 viewerPosition;
    static Vector2 viewerPositionLastUpdate;

    // Inputs ===========
    public Transform viewer;
    public Material mapMaterial;

    // Last lod element defines the render distance.    
    public LODThreshold[] renderDistanceLodLevels;
    public static float renderDistance;

    // ==================== Land Generation Variables ===========================

    // User to keep a dictionary of all chunks generated ever, the key is coordinates.
    Dictionary<Vector2, LandChunk> LandChunks = new Dictionary<Vector2, LandChunk>();
    // List of visible terain chunks so they can be validated if they should be currently shown.
    public static List<LandChunk> visibleTerrainChunks = new List<LandChunk>();

    static LandChunkDataGenerator landChunkDataGenerator; 
    int chunkLength;
    int chunkCoordinateRenderDistance;


    // ======================== Life Cycle Functions ========================================
    void Start() {
        // Fetch instance of the chunk data generator.
        landChunkDataGenerator = FindObjectOfType<LandChunkDataGenerator>();
        
        // The last lod level is used as the render distance.
        renderDistance = renderDistanceLodLevels[(renderDistanceLodLevels.Length-1)].visibleDstThresh;

        chunkLength = LandChunkDataGenerator.chunkSize-1;
        chunkCoordinateRenderDistance = Mathf.RoundToInt(renderDistance/chunkLength); // How many chunks are rendered in a direction from player.

        UpdateVisibleChunks();
    }

    void Update(){
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z) /scale ;

        // Pythagoras to check if chunk update threshold is hit.
        if((viewerPositionLastUpdate - viewerPosition).sqrMagnitude > sqrViewerMovementUpdateThreshold){
            viewerPositionLastUpdate = viewerPosition;
            UpdateVisibleChunks();
        }
    }

    void UpdateVisibleChunks(){
        // All previous chunks are set invisible.
        for(int i = 0; i < visibleTerrainChunks.Count; i++){
            visibleTerrainChunks[i].SetVisible(false);
        }
        visibleTerrainChunks.Clear();


        // each chunkLength = 1 in chunk coordinates;
        int viewerChunkCoordinateX = Mathf.RoundToInt(viewerPosition.x / chunkLength);
        int viewerChunkCoordinateY = Mathf.RoundToInt(viewerPosition.y / chunkLength);

        // Loop from minus value to use distance as a 'radius'.
        for(int yOffset = -chunkCoordinateRenderDistance; yOffset <= chunkCoordinateRenderDistance; yOffset++){
            for(int xOffset = -chunkCoordinateRenderDistance; xOffset <= chunkCoordinateRenderDistance; xOffset++){

               Vector2 currentChunkCoordinate = new Vector2(viewerChunkCoordinateX + xOffset, viewerChunkCoordinateY + yOffset);

                // Check dictionary if chunk instanciated or not.
                if(LandChunks.ContainsKey(currentChunkCoordinate)){
                    LandChunks[currentChunkCoordinate].UpdateChunk();
                    
                } else{
                    LandChunks.Add(currentChunkCoordinate, new LandChunk(currentChunkCoordinate, chunkLength, scale, renderDistanceLodLevels, transform, this, landChunkDataGenerator, mapMaterial  ));
                }
            }
        }
    }
}