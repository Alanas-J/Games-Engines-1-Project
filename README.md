# Procedural Ocean Sailing Simulation

Name: Alanas Jakubauskas

Student Number: C18473312

Class Group: DT228/4  TU856/4

# Description of the project
This project features a ship you can sail accross an inifinite ocean terrain. 
Featuring wave physics that influence the ship and ship steering controls akin to Sea of Theives.


# Instructions for use
Download the repo and open in Unity. Select the scene 'Game' and run in the Unity Editor.

# How it works
This project uses perlin noise maps to create height maps which is used for Terrain Chunk generation and a simpler perlin noise map for wave simulation.
Wave physics uses the same noisemap as the waves to check the water level at a in-game coordinate.

The player controller is built using the Character Controller component and with the use of Raycasting, keybinds and GUI elements is able to interract with the ship.

Floatation physics are implemented by the use of a Rigidbody parent component, then empty GameObjects are given a floatation script that samples the Sea wave heightmap to calculate if they are submerged and apply forces accordingly on the parent Rigidbody.



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

## This is how to markdown text:

This is *emphasis*

This is a bulleted list

- Item
- Item

This is a numbered list

1. Item
1. Item

This is a [hyperlink](http://bryanduggan.org)

# Headings
## Headings
#### Headings
##### Headings

This is code:

```Java
public void render()
{
	ui.noFill();
	ui.stroke(255);
	ui.rect(x, y, width, height);
	ui.textAlign(PApplet.CENTER, PApplet.CENTER);
	ui.text(text, x + width * 0.5f, y + height * 0.5f);
}
```

So is this without specifying the language:

```
public void render()
{
	ui.noFill();
	ui.stroke(255);
	ui.rect(x, y, width, height);
	ui.textAlign(PApplet.CENTER, PApplet.CENTER);
	ui.text(text, x + width * 0.5f, y + height * 0.5f);
}
```

This is an image using a relative URL:

![An image](images/p8.png)

This is an image using an absolute URL:

![A different image](https://bryanduggandotorg.files.wordpress.com/2019/02/infinite-forms-00045.png?w=595&h=&zoom=2)

This is a youtube video:

[![YouTube](http://img.youtube.com/vi/J2kHSSFA4NU/0.jpg)](https://www.youtube.com/watch?v=J2kHSSFA4NU)

This is a table:

| Heading 1 | Heading 2 |
|-----------|-----------|
|Some stuff | Some more stuff in this column |
|Some stuff | Some more stuff in this column |
|Some stuff | Some more stuff in this column |
|Some stuff | Some more stuff in this column |

