using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour{

    // Added to any interractable object, contains the following params :
    public UnityEvent onInteract;
    public int id;
    public string interactionText;

    void Start(){
        id = Random.Range(0,99999); // random id if not assigned from editor.
    }

}
