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
    private float sailSize = 20; // How long extended sails are

    // ================= Lifecycle methods =================================
    void Start() {
        steeringWheel = GameObject.FindWithTag("Ship Steering Wheel"); // I tagged the steering wheel to access for rotation.
        sails = GameObject.FindWithTag("Ship Sails"); // same for sails

        Debug.Log(sails);
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
        sailState = Mathf.Clamp(sailState, 2f, sailSize);
        sails.transform.localScale = new Vector3(25f, sailState, 1); // rezise sails
        sails.transform.localPosition = new Vector3(-1, -sailState/2 + 30, 2.68f); // adjust placement of sails to scale.


        // Ship physics method will go here =========
    }

    // ===================================================================

    // Used to dictate if component is being used.
    public void SteerShip(bool steering) {
        this.enabled = steering;
        objectInterractionComponent.enabled = !steering;
        characterController.enabled = !steering;
        characterControllerScript.enabled = !steering;
    }




}
