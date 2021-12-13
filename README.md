# Procedural Ocean Sailing Simulation

Name: Alanas Jakubauskas

Student Number: C18473312

Class Group: DT228/4  TU856/4

# Description of the project
This project features a ship you can sail accross an inifinite procedurally generated ocean terrain. 
Featuring wave physics that influence the ship and ship steering controls akin to Sea of Theives.

![Ship](./images/Ship.JPG) 

![Steering](./images/Steering.JPG) 

![Overhead view](./images/Overhead.JPG) 



# Instructions for use
Download the repo and open in Unity. Select the scene 'Game' and run in the Unity Editor.

# How it works

## Summary
This project uses perlin noise maps to create height maps which is used for Terrain Chunk generation and a simpler perlin noise map for wave simulation.
Wave physics uses the same noisemap as the waves to check the water level at a in-game coordinate.

The player controller is built using the Character Controller component and with the use of Raycasting, keybinds and GUI elements is able to interract with the ship.

Floatation physics are implemented by the use of a Rigidbody parent component, then empty GameObjects are given a floatation script that samples the Sea wave heightmap to calculate if they are submerged and apply forces accordingly on the parent Rigidbody.

## Land Generation
Land generation monobehaviour scripts are put into an Empty GameObject which then are used to create children Land Chunk objects, there are two Monobehaviour scripts present : 

<img src="./images/LandConfiguration.JPG" alt="Landconfig" width="200"/>

### Land Chunk Data Generator

This is used to configure the land generation characteristics. We have the following parametres:

- Noise Scale : Used to control the initial division of offset coordinates into a Perlin noisemap, this provides zoom.
- Octaves: How many layers of perlin noismapping are used.
- Persistance: What fraction of amplitude persists in each Octave interation.
- Lacunarity: The rate of frequence increase over Octaves.

This is used to allow the perlin noise feature have the initial octave focus on overall terrain heights with each following octave focusing on more granular details in the noisemap, emulating terrain.

- Seed: A seed can be given to the pseudorandom number generator which gives random offsets to each noisemap layer.
- Offset: An offset can be applied to pick an initial coordinate the generation begins from on the noisemap.
- Height Multiplier: By default the noise heightmap outputted in ranges 0-1, a multiplier is provided to amplify the height.
- Terain Layers: Used for texture generation, a serializable struct definition for a terrain layer is written into Land Chunk Data Generatorm, containing a name string, height string and Color parametre. Heights are 0-1 to parse colour values from the initial parametre accordingly if the vertice value is higher than the layer.

More detailed information on under the hood operation is commented into the sourcecode. This includes all data models, the refactoring of responsibility into multiple classes and the multithreading implementation.

### Land Generator
This script handles the rendering and creation of Land Chunk Objects. 

The parametres taken are : 
- Viewer: This is the definition of what Transform the world position is taken from to determine whats should be rendered.
- MapMaterials: This is the base material applied to all LandChunks created.
- RenderDistanceLodLevels: This is a serialized struct that defines render distance randes for different level of detail levels. 0-6 can be used. 0 provides 1 terrain mesh vertice per world coordinate, 1-6 provide a pertice of 1 vertice per (1-6)*2 world coordinates. The highest visible distance is used as the maximum render distance for Land Chunks.

This script instanciates Land Chunk class objects to a dictionary of Vector 2 coordinates, so it knows if a Chunk has been previously created and uses this dictionary with the real world coordinates to check which landchunks need creation or to be rendered.

Each chunk is created with the creation of a LandChunk class. This class stores all of the mesh data and textures it should have. A list of different LOD level meshes is kept here. If the land chunk is missing a LOD level mesh that the Land generator signalled to display, an async thread call is sent to the Land Chunk Data generator with a onReceive callback that updates the mesh once a mesh is calculated.

## Sea Generation
Sea Generation is a simpler implementation than the land generation, instead of creating objects, the object itself is a Terrain object. a different generation approach was taken as performance is more important for the Sea which requires to be constantly rendered with new vertices. 

It features two classes and the sea sound present in the game.

### Land Chunk Data Generator
<img src="./images/SeaConfig.JPG" alt="SeaConfig" width="200"/>

## Floatation Physics
<img src="./images/Floatation.JPG" alt="Floatation" width="200"/>

## Character Controls
### Land Chunk Data Generator
<img src="./images/Floatation.JPG" alt="Floatation" height="200"/>
### Land Chunk Data Generator
<img src="./images/CameraScripts.JPG" alt="CameraScripts" height="200"/>
### Land Chunk Data Generator
<img src="./images/ApplyShipVelocity.JPG" alt="ApplyShipVelocity" height="200"/>



## Ship Controls

<img src="./images/ShipController.JPG" alt="Ship Controller" width="200"/>



# List of classes/assets in the project and whether made yourself or modified or if its from a source, please give the reference

| Class/asset | Source |
|-----------|-----------|
| ChunkData.cs | Modified from [Sebastian Lague's Terrain Generation Tutorial Series EP 1-12](https://www.youtube.com/watch?v=wbpMiKiSKm8&list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3) |
| LODMesh.cs | Modified from [Sebastian Lague's Terrain Generation Tutorial Series EP 1-12](https://www.youtube.com/watch?v=wbpMiKiSKm8&list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3) |
| LODMeshThreshold.cs | Modified from [Sebastian Lague's Terrain Generation Tutorial Series EP 1-12](https://www.youtube.com/watch?v=wbpMiKiSKm8&list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3) |
| MeshData.cs | Modified from [Sebastian Lague's Terrain Generation Tutorial Series EP 1-12](https://www.youtube.com/watch?v=wbpMiKiSKm8&list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3) |
| MeshDataGenerator.cs | Modified from [Sebastian Lague's Terrain Generation Tutorial Series EP 1-12](https://www.youtube.com/watch?v=wbpMiKiSKm8&list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3) |
| TextureGenerator.cs | Modified from [Sebastian Lague's Terrain Generation Tutorial Series EP 1-12](https://www.youtube.com/watch?v=wbpMiKiSKm8&list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3) |
| LandNoiseSource.cs | Modified from [Sebastian Lague's Terrain Generation Tutorial Series EP 1-12](https://www.youtube.com/watch?v=wbpMiKiSKm8&list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3) |
| LandChunkDataGenerator.cs | Modified from [Sebastian Lague's Terrain Generation Tutorial Series EP 1-12](https://www.youtube.com/watch?v=wbpMiKiSKm8&list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3) |
| LandChunk.cs | Modified from [Sebastian Lague's Terrain Generation Tutorial Series EP 1-12](https://www.youtube.com/watch?v=wbpMiKiSKm8&list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3) |
| LandGenerator.cs | Modified from [Sebastian Lague's Terrain Generation Tutorial Series EP 1-12](https://www.youtube.com/watch?v=wbpMiKiSKm8&list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3) |
| Interractable.cs | Modified from [JTA Games's Interraction Feature Tutorial](https://www.youtube.com/watch?v=lZThP8KG1W0) |
| ObjectInterraction.cs | Modified from [JTA Games's Interraction Feature Tutorial](https://www.youtube.com/watch?v=lZThP8KG1W0) |
| LookingAround.cs | Modified from [Brackey's FIRST PERSON MOVEMENT in Unity Tutorial](https://www.youtube.com/watch?v=_QajrabyTJc) |
| CharacterControllerPlayerMovement.cs |  Modified from [Brackey's FIRST PERSON MOVEMENT in Unity Tutorial](https://www.youtube.com/watch?v=_QajrabyTJc) |
| RigidbodyPlayerMovement.cs | Modified from [Plai's Rigidbody FPS Controller Turotial EP 1-2](https://www.youtube.com/watch?v=LqnPeqoJRFY&list=PLRiqz5jhNfSo-Fjsx3vv2kvYbxUDMBZ0u) |
| PlayerGUIText.cs | Self written |
| SeaFloater.cs | Modified from [Tom Weiland's Ship Buoyancy Tutorial](https://www.youtube.com/watch?v=eL_zHQEju8s) |
| ShipController.cs | Self written |
| SeaManager.cs | Self written |
| SeaObjectController.cs | Self written |
| Basic Ship.prefab | Self built |
| CharController Player.prefab | Modified from [Brackey's FIRST PERSON MOVEMENT in Unity Tutorial](https://www.youtube.com/watch?v=_QajrabyTJc) |

# References

# What I am most proud of in the assignment

# Proposal submitted earlier can go here:

