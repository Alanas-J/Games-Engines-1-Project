using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerPlayerMovement : MonoBehaviour
{

    // Input Variables
    public CharacterController characterController;
    public float speed = 12f;
    public float jumpHeight = 10f;
    public float gravity = 9.8f;
    public Transform groundChecker;
    public LayerMask groundMask;
    
    // Inner logic variables
    Vector3 gravityVelocity;
    float groundCheckerRadius = .5f;


    bool onGround = true;

    // ===================== Life Cycle Methods =====================================================
    void Update(){
        // Ground check
        onGround = Physics.CheckSphere(groundChecker.position, groundCheckerRadius, groundMask);

        float xMovement = Input.GetAxisRaw("Horizontal");
        float zMovement = Input.GetAxisRaw("Vertical");

        // transform.right and transform.forward respectively define normalized vectors of where and X and Z are pointing in the world. 
        // below logic outputs (xMovement,0,yMovement) adjusted by current rotation state of the player object.
        Vector3 movement = transform.right * xMovement + transform.forward * zMovement;


        // Character Controller is used to implement the movement. // Normalized is set to ensure that the speed is the same in all vertors, instead of more to combined value vectors.
        characterController.Move(movement.normalized*speed*Time.deltaTime);

        // Jumping logic
        if(Input.GetButton("Jump") && onGround) {
            gravityVelocity.y = Mathf.Sqrt((jumpHeight * 2f * gravity));
            onGround = false;
            // velocity require to jump a certain height can be calculated as 
            // square root of -2(height)*gravity
        }

        ApplyGravity(); // Used to pull charcter to ground;
    }


    void ApplyGravity(){
        // Check if on ground and not jumping.
        if(onGround){
            gravityVelocity.y = 0.01f; // slight gravity pull always
        } 
        else {
            gravityVelocity.y -= gravity * (Time.deltaTime); // Gravity per second is added to velocity
        }
        characterController.Move(gravityVelocity*Time.deltaTime); // Velocity is acted towards per second.
        
        // This applies gravity/second^2 acceleration.


    }
}
