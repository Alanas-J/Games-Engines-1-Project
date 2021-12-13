using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to make the state of a single chunk instance.
public class LandChunk {
        
        // Object it maps to
        GameObject chunkObject; 

        // Rendering Components
        MeshRenderer meshRenderer; 
        MeshFilter meshFilter;
        MeshCollider meshCollider;

        // The real position of the chunk.
        Vector2 position; 
        Bounds bounds; // Box boundary of the chunk

        LODThreshold[] detailLevelThresholds; // Holds all possible lod distance threshholds
        LODMesh[] lodMeshes; // Used to store all lod levels of meshes

        ChunkData chunkData; // Chunkdata of heightmap and colour map.
        bool chunkDataReceived = false; // Flag if chunk has heightmap received from async callback.
        int previousLODIndex = -1; // Used to see if the same lod mesh was used last update

        // The parent land generator
        LandGenerator landGenerator; // Will need to refactor if time, passing whole parent object means less required constructoe args

        // ================================== Constructor =========================================================
        public LandChunk(Vector2 chunkCoord, int size, float scale, LODThreshold[] detailLevelThresholds, Transform parentObject, LandGenerator landGenerator, LandChunkDataGenerator landChunkDataGenerator, Material material) {

            // For pulling data from the managing generator.
            this.landGenerator = landGenerator;

            // Create the game object
            chunkObject = new GameObject("Land Chunk");
            meshRenderer = chunkObject.AddComponent<MeshRenderer>();;
            meshFilter = chunkObject.AddComponent<MeshFilter>();
            meshCollider = chunkObject.AddComponent<MeshCollider>();
            chunkObject.layer = 7;

            meshRenderer.material = material;
            

            // Scale and postion
            position = chunkCoord * size; // position is scaled from chunk coordinates.
            bounds = new Bounds(position, Vector2.one*size); // Bounds on perimetre of chunk created.
            Vector3 positionV3 = new Vector3(position.x, 0, position.y); //3D coordinates
            chunkObject.transform.parent = parentObject; // Parent Object
            chunkObject.transform.position = positionV3*scale; //Adjust postion by scale if used.
            chunkObject.transform.localScale = Vector3.one*scale; // Adjust scale of object if used.
        
            SetVisible(false); //By default chunk is invisible.

            lodMeshes = new LODMesh[detailLevelThresholds.Length]; // Array to hold all chunk mesh versions.
            
            // Instanciation of all meshes
            for(int i = 0; i<detailLevelThresholds.Length; i++) {
                lodMeshes[i] = new LODMesh(detailLevelThresholds[i].lod, UpdateChunk, landChunkDataGenerator);
            }

            // Request initial data for current position with Callback.
            landChunkDataGenerator.RequestChunkData(position, OnChunkDataReceived);
        }
        // =====================================================================================

        // ======================= Async Callback ==============================================
        void OnChunkDataReceived(ChunkData chunkData){
            this.chunkData = chunkData;
            chunkDataReceived = true;

            // Generates the texture for the terrain from the colour array.
            Texture2D texture = TextureGenerator.TextureFromColourArray(chunkData.colourMap, LandChunkDataGenerator.chunkSize, LandChunkDataGenerator.chunkSize);
            meshRenderer.material.mainTexture = texture;
            UpdateChunk();
        }

        // =================== Chunk Update Logic ========================================
        public void UpdateChunk(){
            if(chunkDataReceived){

                // Check the postion of point to the chunk boundary.
                float viewerDistanceFromEdge = Mathf.Sqrt(bounds.SqrDistance(LandGenerator.viewerPosition));
                bool visible = viewerDistanceFromEdge <= LandGenerator.renderDistance;

                if(visible){
                    int lodIndex = 0;
                    
                    // Cycling through all lod thresholds from shortest.
                    for(int i = 0; i < landGenerator.renderDistanceLodLevels.Length -1; i++){
                        if(viewerDistanceFromEdge > landGenerator.renderDistanceLodLevels[i].visibleDstThresh){
                            lodIndex = i+1;
                        } else{
                            break;
                        }
                    }

                    // Check if mesh needs to be swapped
                    if(lodIndex != previousLODIndex) {
                        LODMesh lodMesh = lodMeshes[lodIndex];

                        // If has mesh use, if not, request.
                        if(lodMesh.hasMesh){
                            meshFilter.mesh = lodMesh.mesh;
                            previousLODIndex = lodIndex;
                            meshCollider.sharedMesh = lodMesh.mesh;
                        } else if (!lodMesh.hasRequestedMesh){
                            lodMesh.RequestMesh(chunkData);
                        }
                    }

                    // Add to land generators list of visible chunks.
                    LandGenerator.visibleTerrainChunks.Add(this);
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
