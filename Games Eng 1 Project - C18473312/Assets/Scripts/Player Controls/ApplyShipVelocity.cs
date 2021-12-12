using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyShipVelocity : MonoBehaviour {
    

    private CharacterController playerCharacterController;
    public Rigidbody shipRigidbody;

    private Transform playerLocalPos;


    void Start() {
       playerCharacterController = this.gameObject.GetComponent<CharacterController>();
        //shipRigidbody = this.gameObject.GetComponentInParent<Rigidbody>();

       // playerLocalPos = this.GameObject.transform;
        //Debug.Log(shipRigidbody);
    }

    // Update is called once per frame
    void Update(){
        playerCharacterController.Move(shipRigidbody.GetPointVelocity(this.gameObject.transform.position)*Time.deltaTime);

        //playerLocalPos = this.GameObject.transform;
        
        // playerRigidbody.velocity += shipRigidbody.velocity*Time.deltaTime;

        //playerRigidbody.AddForce(shipRigidbody.velocity*.1f, ForceMode.VelocityChange);
    }
}

