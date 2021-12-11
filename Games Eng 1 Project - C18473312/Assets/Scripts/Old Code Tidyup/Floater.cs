using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody rigidBody;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmmount = 3f;
    public float waterDrag = .99f;
    public float waterAngularDrag = .5f;
    public int floaterCount = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate(){

      //Applying gravity on the floaters
      rigidBody.AddForceAtPosition(Physics.gravity/floaterCount,transform.position, ForceMode.Acceleration);

      // Fetch wave manager instance value
      float waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x);

      if(transform.position.y < waveHeight){
        float displacementMultiplier = Mathf.Clamp01((waveHeight-transform.position.y)/depthBeforeSubmerged)*displacementAmmount;
        rigidBody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) *displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
        rigidBody.AddForce(displacementMultiplier*-rigidBody.velocity*waterDrag *Time.fixedDeltaTime, ForceMode.VelocityChange);
        rigidBody.AddTorque(displacementMultiplier*-rigidBody.angularVelocity*waterAngularDrag *Time.fixedDeltaTime, ForceMode.VelocityChange);
      }
    }
}
