using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyShipVelocity : MonoBehaviour {
    
    public Transform groundChecker;
    public LayerMask shipMask;
    public LayerMask groundMask;


    // ==================================
    private CharacterController playerCharacterController;
    private Rigidbody shipRigidbody;
    private Transform playerLocalPos;
    private GameObject shipCurrentlyOn;


    // Raycast variables.
    private float groundCheckerDistance = 3f;
    bool onShip = false;
    RaycastHit hit;

    // ================== LifeCycle Methods ===========================
    void Start() {
       playerCharacterController = this.gameObject.GetComponent<CharacterController>();
     
    }

    // Update is called once per frame
    void Update(){

        
        // Check for ship
        if(Physics.Raycast(groundChecker.position, groundChecker.up*-1, out hit, groundCheckerDistance, shipMask, QueryTriggerInteraction.UseGlobal)){

            shipCurrentlyOn = hit.rigidbody.gameObject;
            shipRigidbody = hit.rigidbody;

            // Apply orientation
            if(this.gameObject.transform.parent != shipCurrentlyOn.transform ){

                this.gameObject.transform.parent = shipCurrentlyOn.transform;
                // Sets the X and Z rotation but keeps looking round rotation.
                this.gameObject.transform.localRotation = Quaternion.Euler(0, this.gameObject.transform.localRotation.eulerAngles.y ,0);
            }
            
            onShip = true;
        }
        
        // Check for land.
        else if(Physics.Raycast(groundChecker.position, groundChecker.up*-1, out hit, groundCheckerDistance, groundMask, QueryTriggerInteraction.UseGlobal)) {

            // Apply orientation
            if(this.gameObject.transform.parent != null ){
                this.gameObject.transform.parent = null;
                // Sets the X and Z rotation but keeps looking round rotation.
                this.gameObject.transform.rotation = Quaternion.Euler(0, this.gameObject.transform.rotation.eulerAngles.y ,0);
            }
            onShip = false;
        }
        
        // If on ship and instanciated properly, apply ship motion
        if(onShip && shipRigidbody != null && shipCurrentlyOn != null){
            // Movement
            playerCharacterController.Move(shipRigidbody.GetPointVelocity(this.gameObject.transform.position)*Time.deltaTime);
        }


        // WORKING CODE ->


    }
}

