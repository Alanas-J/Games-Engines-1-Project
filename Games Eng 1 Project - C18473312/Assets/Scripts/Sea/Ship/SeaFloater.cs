using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaFloater : MonoBehaviour{


    // Input variables ============================================
    public Rigidbody rigidBody; // The rigidbody the floater adds its forces 2.

    public float depthBeforeMaximumUpPull = 1f;
    public float strengthOfUpPull = 3f; // In real life physics the weight of displacement.
    
    public float waterDrag = .99f; // How resistant to force water is.
    public float waterAngularDrag = .5f;  // How resistant water is to rotation.

    public int floaterCount = 1; // Ammount of floaters present, code to count floaters could be implemented


    // Fixed update is tied to each iteration of the physics engine, its smoother.
    private void FixedUpdate(){
    
        // Gravity is distributed to each floater, this is to balance the floatation forces at each point.
        rigidBody.AddForceAtPosition(Physics.gravity/floaterCount, transform.position, ForceMode.Acceleration);

        // Fetch normalized water height at point in noise map.
        float waterHeight = GetWaterHeight();


        // If floater is under water.
        if(transform.position.y < waterHeight) {

            // Checking how much the floater is submerged.
            float depthSubmergedToUpPullDepthRatio = waterHeight-transform.position.y/depthBeforeMaximumUpPull;

            // If submerged fully/ over maximum pull depth, limit strength. 
            float upPullStrength = depthSubmergedToUpPullDepthRatio > 1 ? 1*strengthOfUpPull : depthSubmergedToUpPullDepthRatio * strengthOfUpPull;


            // Adding up force. Force is added relative to gravity / by floater count * by up pull. At Current Position and acceleration is added
            rigidBody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y)/floaterCount*upPullStrength, 0f), transform.position, ForceMode.Acceleration);


            // Drag code
            rigidBody.AddForce(upPullStrength*-rigidBody.velocity*waterDrag *Time.fixedDeltaTime, ForceMode.Acceleration);
            rigidBody.AddTorque(upPullStrength*-rigidBody.angularVelocity*waterAngularDrag *Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }


    float GetWaterHeight(){
        // Fetch normalized wave height
        float waterHeight = SeaManager.instance.GetNormalizedHeightAtPoint(transform.position.x, transform.position.z);

        // Adjusting height to sea parametres. Min height + (normalized value * height multiplier).
        waterHeight = SeaManager.instance.minWaveHeight + (waterHeight * SeaManager.instance.maxWaveHeight);

        return waterHeight;       
    }
}
