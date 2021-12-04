using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int map_width;
    public int map_height;
    public float noise_scale;
    public bool updateOnChange;


    public void GenerateMap(){
        float[,] noise_map = Noise.GenerateNoiseMap(map_width,map_height, noise_scale);


        MapDisplay display = FindObjectOfType<MapDisplay> ();
        display.DrawNoiseMap(noise_map);
    }
}
