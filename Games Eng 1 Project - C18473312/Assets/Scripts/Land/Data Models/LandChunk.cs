using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandChunk{
        
        GameObject chunkObject;
        Vector2 position;
        MeshRenderer meshRenderer;
        MeshFilter meshFilter;

        // will need to look into bounds
        Bounds bounds;

        LODInfo[] detailLevels;
        LODMesh[] lodMeshes; // Used store all lod levels of meshes

        MapData mapData;
        bool mapDataReceived;
        int previousLODIndex = -1;

        public TerrainChunk(Vector2 coord, int size, LODInfo[] detailLevels, Transform parent, Material material){

            this.detailLevels = detailLevels;

            position = coord * size;
            bounds = new Bounds(position, Vector2.one*size);

            Vector3 positionV3 = new Vector3(position.x, 0, position.y);


            chunkObject = new GameObject("Land Chunk");
            meshRenderer = chunkObject.AddComponent<MeshRenderer>();;
            meshFilter = chunkObject.AddComponent<MeshFilter>();
            meshRenderer.material = material;

            chunkObject.transform.position = positionV3*scale;
            chunkObject.transform.parent = parent;
            chunkObject.transform.localScale = Vector3.one*scale;

            
            SetVisible(false);

            lodMeshes = new LODMesh[detailLevels.Length];
            for(int i = 0; i<detailLevels.Length; i++){
                lodMeshes[i] = new LODMesh(detailLevels[i].lod, UpdateChunk);
            }

            landChunkDataGenerator.RequestMapData(position, OnMapDataReceived);
        }

        void OnMapDataReceived(MapData mapData){
            this.mapData = mapData;
            mapDataReceived = true;

            
            Texture2D texture = TextureGenerator.TextureFromColourMap(mapData.colourMap, LandChunkDataGenerator.mapChunkSize, LandChunkDataGenerator.mapChunkSize);
            meshRenderer.material.mainTexture = texture;
            UpdateChunk();
        }

        /*
        void OnMeshDataReceived(MeshData meshData){
            meshFilter.mesh = meshData.CreateMesh();
        }
            */
        public void UpdateChunk(){
            if(mapDataReceived){

                float viewerDistanceFromEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));

                bool visible = viewerDistanceFromEdge <= renderDistance;

                if(visible){
                    int lodIndex = 0;

                    for(int i = 0; i < detailLevels.Length -1; i++){
                        if(viewerDistanceFromEdge > detailLevels[i].visibleDstThresh){
                            lodIndex = i+1;
                        } else{
                            break;
                        }
                    }

                    if(lodIndex != previousLODIndex){
                        LODMesh lodMesh = lodMeshes[lodIndex];

                        if(lodMesh.hasMesh){
                            meshFilter.mesh = lodMesh.mesh;
                            previousLODIndex = lodIndex;
                        } else if (!lodMesh.hasRequestedMesh){
                            lodMesh.RequestMesh(mapData);
                        }
                    }

                    previousVisibleTerainChunks.Add(this);
                }

                SetVisible(visible);
            }
        }

        public void SetVisible(bool visible){
            chunkObject.SetActive(visible);
        }

        public bool isVisible(){
            return chunkObject.activeSelf;
        }


    }

    
    class LODMesh{
        public Mesh mesh;
        public bool hasRequestedMesh;
        public bool hasMesh;
        int lod;
        System.Action updateCallBack;

        public LODMesh(int lod, System.Action updateCallBack){
            this.lod = lod;
            this.updateCallBack = updateCallBack;
        }

        void OnMeshDataReceived(MeshData meshData){
            mesh = meshData.CreateMesh();
            hasMesh = true;

            updateCallBack();
        }

        public void RequestMesh(MapData mapData){
            hasRequestedMesh = true;
            landChunkDataGenerator.RequestMeshData(mapData, lod, OnMeshDataReceived);
        }
    }
