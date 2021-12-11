using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingAround : MonoBehaviour {

    // Input variables.
    public float mouseSensetivity = 1000;
    public Transform playerBody; // To apply rotations to the player body.

    // Inner logic variables.
    float xAxisDegrees = 0f;


    // Lifecycle methods =================================================================
    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
    }



    void Update() {
        // Framerate independent due to .deltaTime and multiplied by sensetivity to allow configuration.
        float mouseX = Input.GetAxis("Mouse X") * mouseSensetivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensetivity * Time.deltaTime;


        // Playerbody right left ========================================================

        // Vector3.up defines a 0,1,0 vector, this allows for mouse X to multiply into the Y vector to create, 0,mouseX,0.
        // One of the ways to use .Rotate is by providing a vector3 of euler angles such as this.
        playerBody.Rotate(Vector3.up * mouseX);



        // Camera up down ===============================================================

        // We're keeping track of rotation state and setting instead of above logic.
        // This is to allow the clamping of camera up down movement 
        xAxisDegrees -= mouseY;
        xAxisDegrees = Mathf.Clamp(xAxisDegrees, -90f, 90f);


                                    // Used to convert euler input to Quaternion.
        transform.localRotation = Quaternion.Euler(xAxisDegrees, 0f, 0f);
    }
}
