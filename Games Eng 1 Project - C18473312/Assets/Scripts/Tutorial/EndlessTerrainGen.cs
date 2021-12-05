using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrainGen : MonoBehaviour
{
    
    const float viewerMoveThresholdForChunkUpdate = 25f;
    const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;


    public LODInfo[] detailLevels;
    public static float renderDistance = 450;


    public Transform viewer;
    public Material mapMaterial;


    static MapGenerator mapGenerator;
    public static Vector2 viewerPosition;
    public static Vector2 viewerPositionPrevUpdate;

    int chunkSize;
    int chunksVisible;

    Dictionary<Vector2, TerrainChunk> terrainChunks = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> previousVisibleTerainChunks = new List<TerrainChunk>();


    // On start
    void Start(){
        mapGenerator = FindObjectOfType<MapGenerator>();
        chunkSize = MapGenerator.mapChunkSize-1;
        chunksVisible = Mathf.RoundToInt(renderDistance/chunkSize);

        renderDistance = detailLevels[(detailLevels.Length-1)].visibleDstThresh;


        UpdateVisibleChunks();
    }

    void Update(){
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);

        if((viewerPositionPrevUpdate-viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate){
            viewerPositionPrevUpdate = viewerPosition;
            UpdateVisibleChunks();
        }

        
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
                    terrainChunks.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, detailLevels, transform, mapMaterial));
                }
            }
        }
    }

    public class TerrainChunk{
        
        GameObject meshObject;
        Vector2 position;
        MeshRenderer meshRenderer;
        MeshFilter meshFilter;

        // will need to look into bounds
        Bounds bounds;

        LODInfo[] detailLevels;
        LODMesh[] lodMeshes;

        MapData mapData;
        bool mapDataReceived;
        int previousLODIndex = -1;

        public TerrainChunk(Vector2 coord, int size, LODInfo[] detailLevels, Transform parent, Material material){

            this.detailLevels = detailLevels;

            position = coord * size;
            bounds = new Bounds(position, Vector2.one*size);

            Vector3 positionV3 = new Vector3(position.x, 0, position.y);


            meshObject = new GameObject("Terrain Chunk");
            meshRenderer = meshObject.AddComponent<MeshRenderer>();;
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshRenderer.material = material;

            meshObject.transform.position = positionV3;
            meshObject.transform.parent = parent;

            
            SetVisible(false);

            lodMeshes = new LODMesh[detailLevels.Length];
            for(int i = 0; i<detailLevels.Length; i++){
                lodMeshes[i] = new LODMesh(detailLevels[i].lod, UpdateChunk);
            }

            mapGenerator.RequestMapData(position, OnMapDataReceived);
        }

        void OnMapDataReceived(MapData mapData){
            this.mapData = mapData;
            mapDataReceived = true;

            
            Texture2D texture = TextureGenerator.TextureFromColourMap(mapData.colourMap, MapGenerator.mapChunkSize, MapGenerator.mapChunkSize);
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
                }

                SetVisible(visible);
            }
        }

        public void SetVisible(bool visible){
            meshObject.SetActive(visible);
        }

        public bool isVisible(){
            return meshObject.activeSelf;
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
            mapGenerator.RequestMeshData(mapData, lod, OnMeshDataReceived);
        }
    }

    [System.Serializable]
    public struct LODInfo{
        public int lod;
        public float visibleDstThresh;
    }    
}
