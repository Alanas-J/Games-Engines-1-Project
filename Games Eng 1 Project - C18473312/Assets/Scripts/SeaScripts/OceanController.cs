using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanController: MonoBehaviour
{

    // Singleton instance
    public static OceanController instance;

    // Wave Parametres
    public float amplitude = 1f;
    public float wave_scale = 2f;
    public float speed = 1f;
    public float offset_x = 0f;
    public float offset_y = 0f;

    private void Update(){
        offset_x += Time.deltaTime * speed;
        offset_y+= Time.deltaTime * speed;
    }

    // For Given x value, will need to be changed for Perlin's X/Y axis
    public float GetOceanHeight(float x, float y){
        float x_coordinate = (float)x/wave_scale + offset_x;
        float y_coordinate = (float)y/wave_scale + offset_y;
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