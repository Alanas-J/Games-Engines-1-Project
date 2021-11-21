using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody rigidBody;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmmount = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate(){

      // Fetch wave manager instance value
      float waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x);

      if(transform.position.y < waveHeight){
        float displacementMultiplier = Mathf.Clamp01((waveHeight-transform.position.y)/depthBeforeSubmerged)*displacementAmmount;
        rigidBody.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) *displacementMultiplier, 0f), ForceMode.Acceleration);
      }
    }
}
