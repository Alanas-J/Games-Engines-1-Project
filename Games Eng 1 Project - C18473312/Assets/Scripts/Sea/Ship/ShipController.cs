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
    public CharacterController playerCharacterController;
    public CharacterControllerPlayerMovement characterControllerMovementScript;


    // Private variables =========
    private Rigidbody shipRigidbody; // Used to move ship.
    private GameObject steeringWheel; // will be rotated
    private GameObject sails; // will be scaled


    private float wheelRotation = 0;
    private float wheelMaxRotation = 450f; // 360 and 90 degrees to each side.
    private float sailState = 0f;
    private float maxSailSize = 20f; // How long extended sails are
    private float minSailSize = 2f; // how long retracted sails are.

    private bool isSteering = false; // state of controller.
    private bool keyWasUp; // fix to ensure controller doesnt instantly exit steering.

    // ================= Lifecycle methods =================================
    void Start() {
        steeringWheel = GameObject.FindWithTag("Ship Steering Wheel"); // I tagged the steering wheel to access for rotation.
        sails = GameObject.FindWithTag("Ship Sails"); // same for sails
       
        // Getting the ship's rigidbody.
        shipRigidbody = this.gameObject.GetComponent<Rigidbody>();
    }


    void Update() {
        
        // if steering ship.
        if(isSteering){
            if(Input.GetKeyUp(KeyCode.E)) // fix to ensure player doesnt instantly exit.
                keyWasUp = true;

            // Prompt to ensure player knows how to exit steering.
            PlayerGUIText.AddString("Press E to exit steering.");
            if(Input.GetKeyDown(KeyCode.E) && keyWasUp){
                SteerShip(false);
            }

            // Input
            sailState += Input.GetAxis("Vertical") * sailControlSpeed * Time.deltaTime;
            wheelRotation -= Input.GetAxis("Horizontal") * steeringSpeed * Time.deltaTime;
        }

        // Set wheel state
        wheelRotation = Mathf.Clamp(wheelRotation, -wheelMaxRotation, wheelMaxRotation);
        steeringWheel.transform.localRotation = Quaternion.Euler(0, 0, wheelRotation);

        // Set sail state
        sailState = Mathf.Clamp(sailState, minSailSize, maxSailSize);
        sails.transform.localScale = new Vector3(25f, sailState, 1); // rezise sails
        sails.transform.localPosition = new Vector3(-1, -sailState/2 + 30, 2.68f); // adjust placement of sails to scale.


        ApplyShipDriving(); // Applies driving physics.
    }

    // ===================================================================

    // Used to dictate if component is being steered.
    public void SteerShip(bool steering) {

        isSteering = steering; // enable disable inputs.
        keyWasUp = false;

        // Disable if steering all character controlls.
        //playerCharacterController.enabled = !steering;
        characterControllerMovementScript.enabled = !steering;
        objectInterractionComponent.enabled = !steering;

    }


    // Applies vehicle control forces to rigidbody;
    private void ApplyShipDriving() {

        // 1. Add forward force based on speed / sail ratio.

        // firstly we get the forward vector of the ship.
        Vector3 forwardForce = shipRigidbody.gameObject.transform.forward;

        // This multiplies the forward vector by ship speed / ammount of sails used.
        forwardForce = forwardForce * shipSpeed * Mathf.InverseLerp(minSailSize, maxSailSize, sailState);

        shipRigidbody.AddForce(forwardForce*Time.deltaTime, ForceMode.Acceleration); // Apply force


        // 2. Ship rotation.

        // sails have to be applying force for rotation to work.
        if(sailState != 2){
            Vector3 rotationForce = shipRigidbody.gameObject.transform.up;

            rotationForce = rotationForce * shipRotationSpeed * (Mathf.InverseLerp(wheelMaxRotation, -wheelMaxRotation, wheelRotation)*2-1);
            // *2 -1 applied to inverse lerp to output a -1 to 1 value;
            shipRigidbody.AddTorque(rotationForce*Time.deltaTime, ForceMode.Acceleration); // Apply torque.
        }
        
    }






}
