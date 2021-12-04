using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{

    public Renderer textureRenderer;

    public void DrawNoiseMap(float[,] noise_map){
        int width = noise_map.GetLength(0);
        int height = noise_map.GetLength(1);
    
        Texture2D texture = new Texture2D(width,height);


        Color[] colourMap = new Color[width * height];
        for(int y = 0; y<height; y++){
            for( int x = 0; x < width; x++){

                colourMap[y*width +x] = Color.Lerp(Color.black, Color.white, noise_map[x,y]);
            }
        }


        texture.SetPixels(colourMap);
        texture.Apply();

        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(width,1,height);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
