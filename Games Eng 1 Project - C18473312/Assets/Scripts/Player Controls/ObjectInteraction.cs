using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectInteraction : MonoBehaviour {


    // Input variables
    public float interractionDistance = 2f;
    public LayerMask interractAbleObjectMask;

    // Script scope variables
    UnityEvent onInteract;

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

                // Store the onInteract event from the intractable object.
                onInteract = hit.collider.GetComponent<Interactable>().onInteract;

                // If E is pressed
                if(Input.GetKeyDown(KeyCode.E)){
                    
                    // Invoke.
                    onInteract.Invoke();
                }
            }
        }

    }
}
