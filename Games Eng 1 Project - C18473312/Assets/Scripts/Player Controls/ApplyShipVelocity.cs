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
            PlayerGUIText.AddString("On Ship");
            PlayerGUIText.AddString(hit.rigidbody.gameObject.name);

            shipCurrentlyOn = hit.rigidbody.gameObject;
            shipRigidbody = hit.rigidbody;

            // Apply orientation
            if(this.gameObject.transform.parent != shipCurrentlyOn.transform ){

                this.gameObject.transform.parent = shipCurrentlyOn.transform;
                this.gameObject.transform.localRotation = Quaternion.identity;
            }
            
            onShip = true;
        }
        
        // Check for land.
        else if(Physics.Raycast(groundChecker.position, groundChecker.up*-1, out hit, groundCheckerDistance, groundMask, QueryTriggerInteraction.UseGlobal)){
            PlayerGUIText.AddString("On Land");

            // Apply orientation
            if(this.gameObject.transform.parent != null ){
                this.gameObject.transform.parent = null;
                this.gameObject.transform.rotation = Quaternion.identity;
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

