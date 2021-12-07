using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to parse a 1D colour array into a texture.
public static class TextureGenerator
{
    public static Texture2D TextureFromColourArray(Color[] colourArray, int width, int height){
        Texture2D texture = new Texture2D(width,height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colourArray);
        texture.Apply();
        return texture;

    }
}
