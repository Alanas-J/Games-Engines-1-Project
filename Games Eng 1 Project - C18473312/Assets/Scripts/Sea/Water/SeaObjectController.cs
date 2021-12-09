using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaObjectController : MonoBehaviour {
    
    // User inputs =========================   
    public int tileWidth = 2048;
    public HeightMapResolution heightMapResolution;
    public float oceanFrameRate = 30;

    public enum HeightMapResolution{ x129, x256, x512, x1024, x2048} 

    // Inner Variables ================
    Vector2 fromCornerToCentre; // Used to get center of mesh.
    Terrain seaTerrain;
    int heightMapResolutionInt;
    float[,] heightMap;
    float terrainMaxHeight;
    
    // Taking viewerPosition from Land Generator to sync up position updates
    Vector2 viewerLastPostition;


    // ========================== Lifecycle methods =============================
    void Start(){
        // Centering the water and raising sea level
        fromCornerToCentre = new Vector2(-tileWidth/2, -tileWidth/2);  // Transform needed to centre the sea tile
        transform.position = new Vector3(fromCornerToCentre.x, SeaManager.instance.minWaveHeight, fromCornerToCentre.y);

        seaTerrain = GetComponent<Terrain>();
    }


    float timer = 0;
    void Update()
    {
        // Update throttler
        timer += Time.deltaTime*oceanFrameRate;
        if(timer > 1){
            timer = 0;

            CheckIfPositionNeedsUpdate();
            UpdateTerrainMesh();
        }  

    }

    // ===========================================================================

    // Code To steal position update from terrain gen, faster than tracking viewer on both.
    void CheckIfPositionNeedsUpdate(){
            // If the chunk generator's position is updated we update the sea's position.
        if(LandGenerator.viewerPositionLastUpdate != viewerLastPostition){
            viewerLastPostition = LandGenerator.viewerPositionLastUpdate;

            Vector2 translationVector2 = viewerLastPostition+ fromCornerToCentre;
            transform.position = new Vector3(translationVector2.x, SeaManager.instance.minWaveHeight, translationVector2.y);
        }
    }

    // Fetch and use height data from 
    void UpdateTerrainMesh(){
        // Heightmap value from enum.
        switch(heightMapResolution){
            case HeightMapResolution.x129:
                heightMapResolutionInt = 129;
                break;
            case HeightMapResolution.x256:
                heightMapResolutionInt = 256;
                break;
            case HeightMapResolution.x512:
                heightMapResolutionInt = 512;
                break;
            case HeightMapResolution.x1024:
                heightMapResolutionInt = 1024;
                break;     
            case HeightMapResolution.x2048:
                heightMapResolutionInt = 2048;
                break;            
        }    
        seaTerrain.terrainData.heightmapResolution = heightMapResolutionInt;

        // Calls to singleton sea manager to get heightmap data.
        heightMap = SeaManager.instance.GetNormalizedHeightMap(transform.position, tileWidth, heightMapResolutionInt);
        terrainMaxHeight = SeaManager.instance.maxWaveHeight;

        // Size of the terrain mesh
        seaTerrain.terrainData.size = new Vector3(tileWidth, terrainMaxHeight, tileWidth);
        
        // Update of HeightData
        seaTerrain.terrainData.SetHeights(0,0, heightMap);

    }

}
