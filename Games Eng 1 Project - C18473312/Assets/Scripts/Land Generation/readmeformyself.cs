


/*
static class Noise (The imnplementation of perlin imput for terrain gen), takes input and uses perlin accordingly.

 Monobehaviour : ChunkDataGenerator uses Noise to Create a heightmap and colour map. 
    - ChunkData is a model used here

    - Handles threadloading of get meshData and chunkdata

static classMeshGen, returns ChunkMesh from ChunkData
TextureGen, returns ChunkTexture from ChunkData

ChunkRenderer - Will Attach to an object which will own all chunks. Will take input of transform to follow.
Will keep track of all active chunks and toggle on render distance.


ChunkRenderInEditor - will need to figure out. Calls On Chunk Renderer Methods in Editor.


// ======= For water ======== WILL LOOK AT BELOW IN MORE DETAIL AS I GET TO IT


WaterTileRenderer will enable disable and create ocean tile objects -Mesh will need to be redrawn each second.

OceanTile Object will update mesh each render from component, OceanTile.

OceanTile Component will take data from OceanMeshGenerator and possibly Texture generator.

OceanMeshGenerator will take a logical truth of state from OceanNoise.

OceanNoise, will be attached to the parent ocean object, it is the source of truth of floatation physics too.





 // FIND OUT WHAT UVS ARE (mesh)

*/