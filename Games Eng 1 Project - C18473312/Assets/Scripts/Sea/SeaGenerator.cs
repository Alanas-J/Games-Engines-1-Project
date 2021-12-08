using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaGenerator : MonoBehaviour{
    
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
    

    // Render distance  
    public static float renderDistance;

    //Sea Level of detail factor
    public static int levelOfDetailDivider = 12;

    // ==================== Sea Generation Variables ===========================

    // User to keep a dictionary of all chunks generated ever, the key is coordinates.
    Dictionary<Vector2, SeaChunk> SeaChunks = new Dictionary<Vector2, SeaChunk>();
    // List of visible terain chunks so they can be validated if they should be currently shown.
    public static List<SeaChunk> visibleTerrainChunks = new List<SeaChunk>();

    static SeaChunkDataController seaChunkDataController; 
    int chunkLength;
    int chunkCoordinateRenderDistance;


    // ======================== Life Cycle Functions ========================================
    void Start() {
        // Fetch instance of the chunk data generator.
        seaChunkDataController = FindObjectOfType<SeaChunkDataController>();
        
        chunkLength = SeaChunkDataController.chunkSize-1;
        chunkCoordinateRenderDistance = Mathf.RoundToInt(renderDistance/chunkLength); // How many chunks are rendered in a direction from player.

        UpdateVisibleChunks();
    }

    void Update(){
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z) /scale ;

        /* Keeping this if I want to sync ocean visibility.
        // This logic is kept to keep chunk tiles synced with terrain.
        if((viewerPositionLastUpdate - viewerPosition).sqrMagnitude > sqrViewerMovementUpdateThreshold){
            viewerPositionLastUpdate = viewerPosition;
            UpdateVisibleChunks();
        }
        */
        UpdateVisibleChunks();
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

                // If chunk is loaded first time.
                if(!SeaChunks.ContainsKey(currentChunkCoordinate)){
                    SeaChunks.Add(currentChunkCoordinate, new SeaChunk(currentChunkCoordinate, chunkLength, scale, transform, this, seaChunkDataController, mapMaterial  ));
                } else{

                    SeaChunks[currentChunkCoordinate].UpdateChunk();
                }
            }
        }
    }
}