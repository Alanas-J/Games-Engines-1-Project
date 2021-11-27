using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatFloater : MonoBehaviour
{
    // The parent's rigitbody component to simulate.
    public Rigidbody rigidBody;
    // Depth before maximum upward pull.
    public float depthBeforeSubmerged = 1f;
    // Boyouncy value.
    public float displacementAmmount = 3f;
    // How resistant to force water is.
    public float waterDrag = .99f;
    // How resistant water is to rotation.
    public float waterAngularDrag = .5f;
    // Ammount of floaters present, more optimal code could likely be found
    public int floaterCount = 1;

    // Tools to offset floater if necessary
    public float offset_debug_x = 0;
    public float offset_debug_z = 0;


    // Update is called once per frame
    private void FixedUpdate(){

      //Applying gravity per floater on the parent ( MIGHT CHANGE THIS COMPLETELY).
      rigidBody.AddForceAtPosition(Physics.gravity/floaterCount, transform.position, ForceMode.Acceleration);

      // Fetch wave manager instance value
      float waveHeight = OceanController.instance.GetOceanHeight(transform.position.x + offset_debug_x , transform.position.z + offset_debug_z)*OceanController.instance.amplitude;

      if(transform.position.y < waveHeight){
        float displacementMultiplier = Mathf.Clamp01((waveHeight-transform.position.y)/depthBeforeSubmerged)*displacementAmmount;
        rigidBody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) *displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
        rigidBody.AddForce(displacementMultiplier*-rigidBody.velocity*waterDrag *Time.fixedDeltaTime, ForceMode.VelocityChange);
        rigidBody.AddTorque(displacementMultiplier*-rigidBody.angularVelocity*waterAngularDrag *Time.fixedDeltaTime, ForceMode.VelocityChange);
      }
    }
}
