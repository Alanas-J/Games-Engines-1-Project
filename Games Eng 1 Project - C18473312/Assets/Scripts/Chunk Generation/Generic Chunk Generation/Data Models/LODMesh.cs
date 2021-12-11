using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    
public class LODMesh{
    public Mesh mesh;
    public bool hasRequestedMesh;
    public bool hasMesh;
    int lod;
    System.Action updateCallBack;
    LandChunkDataGenerator landChunkDataGenerator;

    public LODMesh(int lod, System.Action updateCallBack, LandChunkDataGenerator landChunkDataGenerator){
        this.lod = lod;
        this.updateCallBack = updateCallBack;
        this.landChunkDataGenerator = landChunkDataGenerator;
    }

    void OnMeshDataReceived(MeshData meshData){
        mesh = meshData.CreateMesh();
        hasMesh = true;
        updateCallBack();
    }

    public void RequestMesh(ChunkData landChunkData){
        hasRequestedMesh = true;
        landChunkDataGenerator.RequestMeshData(landChunkData, lod, OnMeshDataReceived);
    }
}
