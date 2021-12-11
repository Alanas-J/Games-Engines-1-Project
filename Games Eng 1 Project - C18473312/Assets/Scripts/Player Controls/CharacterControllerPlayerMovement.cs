using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerPlayerMovement : MonoBehaviour
{

    // Input Variables
    public CharacterController characterController;
    public float speed = 12f;
    public float gravity = 9.8f;
    public Transform groundChecker;
    public LayerMask groundMask;
    
    // Inner logic variables
    Vector3 gravityVelocity;
    float groundCheckerRadius = .1f;


    bool onGround = true;

    // ===================== Life Cycle Methods =====================================================
    void Update(){


        float xMovement = Input.GetAxis("Horizontal");
        float zMovement = Input.GetAxis("Vertical");

        // transform.right and transform.forward respectively define normalized vectors of where and X and Z are pointing in the world. 
        // below logic outputs (xMovement,0,yMovement) adjusted by current rotation state of the player object.
        Vector3 movement = transform.right * xMovement + transform.forward * zMovement;


        // Character Controller is used to implement the movement.
        characterController.Move(movement*speed*Time.deltaTime);



        ApplyGravity(); // Used to pull charcter to ground;
    }



    void ApplyGravity(){
        onGround = Physics.CheckSphere(groundChecker.position, groundCheckerRadius, groundMask);


        if(onGround){
            gravityVelocity.y = 0; // A bit of velocity is kept to ensure player is grounded
        } 
        else {
            gravityVelocity.y -= gravity * (Time.deltaTime); // Gravity per second is added to velocity
        }
        characterController.Move(gravityVelocity*Time.deltaTime); // Velocity is acted towards per second.
        
        // This applies gravity/second^2 acceleration.


    }
}