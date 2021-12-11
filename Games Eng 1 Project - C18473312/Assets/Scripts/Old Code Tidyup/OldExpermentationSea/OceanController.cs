using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanController: MonoBehaviour
{

    // Singleton instance
    public static OceanController instance;

    // Wave Parametres
    public float amplitude = 1f;

    // Divides how much of the perlin map is used, more division (more zoom);
    public float wave_scale = 2f;
    
    // Used to offset perlin map render.
    public float offset_x = 0f;
    public float offset_y = 0f;

    // Speed of ofsetting to simulate waves.
    public float speed = 1f;


    private void Update(){
        offset_x += Time.deltaTime * speed;
        offset_y += Time.deltaTime * speed;
    }

    // For Given x value, will need to be changed for Perlin's X/Y axis
    public float GetOceanHeight(float x, float y){
        float x_coordinate = x/wave_scale + offset_x;
        float y_coordinate = y/wave_scale + offset_y;

        // Returns 0, 1 value.
        return Mathf.PerlinNoise(x_coordinate, y_coordinate);
    }


    // Singleton code
    private void Awake(){
        if(instance == null){
            instance = this;
        }
        else if(instance != this){
            Debug.Log("Wavemanager instance already running, destroying duplicate instance.");
            Destroy(this);
        }
    }
}    