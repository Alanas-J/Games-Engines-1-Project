using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to make the state of a single chunk instance.
public class SeaChunk {
        
        // Object it maps to
        GameObject chunkObject; 

        // Rendering Components
        MeshRenderer meshRenderer; 
        MeshFilter meshFilter;

        // The real position of the chunk.
        Vector2 position; 
        Bounds bounds; // Box boundary of the chunk

        ChunkData chunkData; // Chunkdata of heightmap and colour map.
        bool chunkDataReceived = false; // Flag if chunk has heightmap received from async callback.

        // The parent Sea generator
        SeaGenerator seaGenerator; // Will need to refactor if time, passing whole parent object means less required constructoe args
        SeaChunkDataController seaChunkDataController; // Noise map controller
        // ================================== Constructor =========================================================
        public SeaChunk(Vector2 chunkCoord, int size, float scale, Transform parentObject, SeaGenerator seaGenerator, SeaChunkDataController seaChunkDataController, Material material) {

            // For pulling data from the managing generator.
            this.seaGenerator = seaGenerator;
            this.seaChunkDataController = seaChunkDataController;

            // Create the game object
            chunkObject = new GameObject("Sea Chunk");
            meshRenderer = chunkObject.AddComponent<MeshRenderer>();;
            meshFilter = chunkObject.AddComponent<MeshFilter>();
            meshRenderer.material = material;

            // Scale and postion
            position = chunkCoord * size; // position is scaled from chunk coordinates.
            bounds = new Bounds(position, Vector2.one*size); // Bounds on perimetre of chunk created.
            Vector3 positionV3 = new Vector3(position.x, 0, position.y); //3D coordinates
            chunkObject.transform.parent = parentObject; // Parent Object
            chunkObject.transform.position = positionV3*scale; //Adjust postion by scale if used.
            chunkObject.transform.localScale = Vector3.one*scale; // Adjust scale of object if used.
        
            SetVisible(false); //By default chunk is invisible.

            // Request initial data for current position with Callback.
            seaChunkDataController.RequestChunkData(position, OnChunkDataReceived);
        }
        // =====================================================================================

        // ======================= Async Callback ==============================================
        void OnChunkDataReceived(ChunkData chunkData){
            this.chunkData = chunkData;
            chunkDataReceived = true;

            // Using for now expermentation
            // Generates the texture for the terrain from the colour array. 
            Texture2D texture = TextureGenerator.TextureFromColourArray(chunkData.colourMap, SeaChunkDataController.chunkSize, SeaChunkDataController.chunkSize);
            meshRenderer.material.mainTexture = texture;
            UpdateChunk();
        }
        // ========================================
        void OnMeshDataReceived(MeshData meshData){
            meshFilter.mesh = meshData.CreateMesh();
        }


        // =================== Chunk Update Logic ========================================
        public void UpdateChunk(){
            if(chunkDataReceived){

                // Check the postion of point to the chunk boundary.
                float viewerDistanceFromEdge = Mathf.Sqrt(bounds.SqrDistance(SeaGenerator.viewerPosition));
                bool visible = viewerDistanceFromEdge <= SeaGenerator.renderDistance;

                if(visible){
                    // Async collection of mesh.( may swap if it looks unsyncronised)
                    seaChunkDataController.RequestMeshData(chunkData, SeaGenerator.levelOfDetailDivider, OnMeshDataReceived);

                    // Add to Sea generators list of visible chunks.
                    SeaGenerator.visibleTerrainChunks.Add(this);
                    SetVisible(visible);
                }
            }
        }

        public void SetVisible(bool visible){
            chunkObject.SetActive(visible);
        }
        public bool isVisible(){
            return chunkObject.activeSelf;
        }
    }
