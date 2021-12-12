using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyPlayerMovement : MonoBehaviour {

    [Header("Ground Movement")] // did not know about this earlier but useful.
    public float moveSpeed = 6f;
    public float movementMultiplier = 10;
    public float movementDrag = 6f;

    [Header("Air Movement")]
    public float jumpForce = 5f;
    public float jumpMovementMultiplier = 1;
    public float jumpDrag = 6f;
    
    [Header("Ground Checking Setup")]
    public float playerHeight = 2.8f;
    public float groundCheckRadius = 0.4f;
    [SerializeField] LayerMask groundLayer;


    // ===================
    float horizontalInput;
    float verticalInput;
    bool isGrounded;


    Vector3 moveDirection;

    Rigidbody playerRigidbody;

    // Lifecycle methods ================================================

    private void Start() {
        playerRigidbody = GetComponent<Rigidbody>();

        // This stops torque physics from applying to the rigid body.
        playerRigidbody.freezeRotation = true;
    }

    private void Update() {
        

        GetPlayerInput();
        ControlDrag();


        // Jumping logic =
        // Using a sphere for ground detection.
        isGrounded = Physics.CheckSphere(transform.position - new Vector3(0,playerHeight/2,0), groundCheckRadius, groundLayer);
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            Jump();
        }

    }

    // Called every time the physics system is called.
    private void FixedUpdate() {
        MovePlayer();
    }



    // Other Class Methods ===============================================================
    private void GetPlayerInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Maps input amplitude to a normalised transform vec3 for how axes.
        moveDirection = transform.forward *verticalInput + transform.right * horizontalInput;
    }

    void MovePlayer(){


        if(isGrounded) {
            // Normalise is applied to make 2 input movement have the same speed as 1;
            playerRigidbody.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else{
            playerRigidbody.AddForce(moveDirection.normalized * moveSpeed * jumpMovementMultiplier, ForceMode.Acceleration);
        }

        
    }

    // Control drag - Determines how grippy the player movement is.
    void ControlDrag(){

        if(isGrounded) {
            playerRigidbody.drag = movementDrag;
        }
        else{
            playerRigidbody.drag = jumpDrag;
        }
        
    }

    void Jump(){
        playerRigidbody.AddForce(transform.up* jumpForce, ForceMode.Impulse);
    }


}
