using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectInteraction : MonoBehaviour {


    // Input variables
    public float interractionDistance = 2f;
    public LayerMask interractAbleObjectMask;

    // Script scope variables
    UnityEvent onInterract;

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

            if(hit.collider.GetComponent<Interactable>)
        }

    }
}
