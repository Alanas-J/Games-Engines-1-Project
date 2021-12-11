using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerPlayerMovement : MonoBehaviour
{

    // Input Variables
    public CharacterController characterController;

    public float speed = 12f;


    // Update is called once per frame
    void Update()
    {
        float xMovement = Input.GetAxis("Horizontal");
        float zMovement = Input.GetAxis("Vertical");

        // transform.right and transform.forward respectively define normalized vectors of where and X and Z are pointing in the world. 
        // below logic outputs (xMovement,0,yMovement) adjusted by current rotation state of the player object.
        Vector3 movement = transform.right * xMovement + transform.forward * zMovement;


        // Character Controller is used to implement the movement.
        characterController.Move(movement*speed*Time.deltaTime);
    }
}
