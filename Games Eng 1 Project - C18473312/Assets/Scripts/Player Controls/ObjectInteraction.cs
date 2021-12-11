using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInteraction : MonoBehaviour {


    // Input variables
    public float interractionDistance = 2f;
    public LayerMask interractAbleObjectMask;
    public Image interactImage;

    // Script scope variables
    Interactable interactableObject;

    // Lifecycle methods ====================================================================
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interractionDistance, interractAbleObjectMask)) {

            Debug.Log(hit.collider.name);

            // If collider collides with a object containing an Interractable component.
            if(hit.collider.GetComponent<Interactable>() != null){

                // Checking if an interractable object is already pointed at, and if a different one is pointed at now.
                if(interactableObject == null || interactableObject.id != hit.collider.GetComponent<Interactable>().id){

                    // If not the same or null, we store interractable as our newest context.
                    interactableObject = hit.collider.GetComponent<Interactable>();
                }
                if(interactableObject.interactionText!= null){
                    PlayerGUIText.AddString(interactableObject.interactionText);
                }

                // If E is pressed
                if(Input.GetKeyDown(KeyCode.E)){
                    
                    // Invoke interraction method.
                    interactableObject.onInteract.Invoke();
                }
            } 
        }

    }
}