using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour{

    // User configuration
    public float shipSpeed = 10f;
    public float shipRotationSpeed = 10f;

    public float sailControlSpeed = 10f;
    public float steeringSpeed = 1f;

    // Objects that need disabling for control.
    public ObjectInteraction objectInterractionComponent;
    public CharacterController characterController;
    public CharacterControllerPlayerMovement characterControllerScript;


    // Private variables =========
    private Rigidbody shipRigidbody; // Used to move ship.
    private GameObject steeringWheel; // will be rotated
    private GameObject sails; // will be scaled


    private float wheelRotation = 0;
    private float sailState = 0f;
    private float maxSailSize = 20f; // How long extended sails are
    private float minSailSize = 2f; // how long retracted sails are.

    // ================= Lifecycle methods =================================
    void Start() {
        steeringWheel = GameObject.FindWithTag("Ship Steering Wheel"); // I tagged the steering wheel to access for rotation.
        sails = GameObject.FindWithTag("Ship Sails"); // same for sails
       
        // Getting the ship's rigidbody.
        shipRigidbody = this.gameObject.GetComponent<Rigidbody>();
    }


    void Update() {
        // Prompt to ensure player knows how to exit steering.
        PlayerGUIText.AddString("Press E to exit steering.");
        if(Input.GetKeyDown(KeyCode.E)){
            SteerShip(false);
        }

        // Input
        sailState += Input.GetAxis("Vertical") * sailControlSpeed * Time.deltaTime;
        wheelRotation -= Input.GetAxis("Horizontal") * steeringSpeed * Time.deltaTime;



        // Set wheel state
        wheelRotation = Mathf.Clamp(wheelRotation, -450f, 450f);
        steeringWheel.transform.localRotation = Quaternion.Euler(0, 0, wheelRotation);

        // Set sail state
        sailState = Mathf.Clamp(sailState, minSailSize, maxSailSize);
        sails.transform.localScale = new Vector3(25f, sailState, 1); // rezise sails
        sails.transform.localPosition = new Vector3(-1, -sailState/2 + 30, 2.68f); // adjust placement of sails to scale.




        ApplyShipDriving();
        // Ship physics method will go here =========
    }

    // ===================================================================

    // Used to dictate if component is being steered.
    public void SteerShip(bool steering) {
        this.enabled = steering;
        objectInterractionComponent.enabled = !steering;
        characterController.enabled = !steering;
        characterControllerScript.enabled = !steering;
    }


    // Applies vehicle control forces to rigidbody;
    private void ApplyShipDriving() {

        // 1. Add forward force based on speed / sail ratio.

        // firstly we get the forward vector of the ship.
        Vector3 forwardForce = shipRigidbody.gameObject.transform.forward;

        // This multiplies the forward vector by ship speed / ammount of sails used.
        forwardForce = forwardForce * shipSpeed * Mathf.InverseLerp(minSailSize, maxSailSize, sailState);

        shipRigidbody.AddForce(forwardForce, ForceMode.VelocityChange); // Apply force


        // 2. Ship rotation.


        // if sail ratio 0 don't apply rotation, else apply rotation per wheel ratio
        //rigidBody.AddTorque(upPullStrength*-rigidBody.angularVelocity*waterAngularDrag *Time.fixedDeltaTime, ForceMode.VelocityChange);
    }






}
