using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildTriggerScript : MonoBehaviour
{
    private InteractScript interactionMethod;

    void Start(){
        interactionMethod = transform.parent.GetComponent<InteractScript>();
    }

    private void OnTriggerEnter(Collider other){
        interactionMethod.OnChildTriggerEnter(other, this.GetComponent<Collider>());

    }

    private void OnTriggerExit(Collider other){
        interactionMethod.OnChildTriggerExit(other, this.GetComponent<Collider>());
    }
}
