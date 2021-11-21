using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    // Singleton instance
    public static WaveManager instance;

    // Wave Parametres
    public float aplitude = 1f;
    public float length = 2f;
    public float speed = 1f;
    public float offset = 0f;

    private void Update(){
        offset += Time.deltaTime * speed;
    }

    // For Given x value, will need to be changed for Perlin's X/Y axis
    public float GetWaveHeight(float _x){
        return aplitude * Mathf.Sin(_x/length);
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
