using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaObjectController : MonoBehaviour {
    
    // User inputs =========================   
    public static int tileWidth = 2048;
    public HeightMapResoluion heightMapResoluion;
    public float oceanFrameRate = 30;

    public enum HeightMapResoluion{ x129, x256, x512, x1024, x2048} 

    // Inner Variables ================
    Vector2 fromCornerToCentre = new Vector2(-tileWidth/2, -tileWidth/2);  // Transform needed to centre the sea tile
    Terrain seaTerrain;
    int heightMapResoluionInt;
    float[,] heightMap;
    float terrainMaxHeight;
    
    // Taking viewerPosition from Land Generator to sync up position updates
    Vector2 viewerLastPostition;


    // ========================== Lifecycle methods =============================
    void Start(){
        // Centering the water
        transform.position = new Vector3(fromCornerToCentre.x, 0, fromCornerToCentre.y);

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
            transform.position = new Vector3(translationVector2.x, 0, translationVector2.y);
        }
    }

    // Fetch and use height data from 
    void UpdateTerrainMesh(){
        // Heightmap value from enum.
        switch(heightMapResoluion){
            case HeightMapResoluion.x129:
                heightMapResoluionInt = 129;
                break;
            case HeightMapResoluion.x256:
                heightMapResoluionInt = 256;
                break;
            case HeightMapResoluion.x512:
                heightMapResoluionInt = 512;
                break;
            case HeightMapResoluion.x1024:
                heightMapResoluionInt = 1024;
                break;     
            case HeightMapResoluion.x2048:
                heightMapResoluionInt = 2048;
                break;            
        }    
        seaTerrain.terrainData.heightMapResoluion = heightMapResoluionInt;

        // Calls to singleton sea manager to get heightmap data.
        heightMap = SeaManager.instance.GetSeaHeightMap(transform.position, tileWidth, heightMapResoluionInt);
        terrainMaxHeight = SeaManager.instance.waveHeight;

        // Size of the terrain mesh
        seaTerrain.terrainData.size = new Vector3(tileWidth, terrainMaxHeight, tileWidth);
        
        // Update of HeightData
        seaTerrain.terrainData.SetHeights(0,0, heightMap);

    }

}
