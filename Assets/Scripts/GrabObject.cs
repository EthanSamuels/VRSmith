using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabObject : MonoBehaviour
{
    public Material highlightMat;
    public XRNode interactSource;
    public float grabTolerance = 0.2f;

    private List<GameObject> colliding = new List<GameObject>();
    private GameObject holding = null;
    private InteractScript holdInteract = null;
    private GameObject closestObj = null;
    private Material closestMat = null;
    private float grabForce;
    private bool interact;

    void Update()
    {
        //Detect input at the start of each update
        InputDevice device = InputDevices.GetDeviceAtXRNode(interactSource);
        bool tempInteract = interact;
        device.TryGetFeatureValue(CommonUsages.primaryButton, out interact);
        float tempGrabForce = grabForce;
        device.TryGetFeatureValue(CommonUsages.grip, out grabForce);

        //Highlight nearest object, sets closestObj
        if(holding == null){
            GameObject minObj = GetNearestObject();
            if(minObj == null && closestObj != null){
                if(closestMat != null) closestObj.GetComponent<Renderer>().material = closestMat;
                closestObj = null;
            }
            else if(minObj != closestObj){
                if(closestObj != null) closestObj.GetComponent<Renderer>().material = closestMat;
                closestObj = minObj;
                Material temp = closestObj.GetComponent<Renderer>().material;
                if(temp == highlightMat) closestMat = null;
                else closestMat = closestObj.GetComponent<Renderer>().material;
                closestObj.GetComponent<Renderer>().material = highlightMat;
            }
        }

        //Grab and Release closestObject
        if(holding == null && tempGrabForce <= grabTolerance && grabForce > grabTolerance && closestObj != null){
            holding = closestObj;
            holdInteract = holding.GetComponent<InteractScript>();
            holdInteract.Grab(true);
            holding.transform.SetParent(this.transform);
            if(closestMat != null) holding.gameObject.GetComponent<Renderer>().material = closestMat;
            holding.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        else if(holding != null && (grabForce < grabTolerance || holding.transform.parent != this.transform)){
            holdInteract.Grab(false);
            holdInteract = null;
            if(holding.transform.parent == this.transform){
                holding.transform.SetParent(null);
                holding.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            }
            if(colliding.Contains(holding)) colliding.Remove(holding);
            holding = null;
        }

        //Interact when input changes to true
        if(holding != null && interact && !tempInteract){
            holdInteract.InteractButtonPress();
        }
    }

    private void OnTriggerEnter(Collider other){
        if(other.tag == "Grabbable" && other.gameObject != holding){
            colliding.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other){
        if(colliding.Contains(other.gameObject)){
            if(other.gameObject == closestObj){
                if(closestMat != null) closestObj.GetComponent<Renderer>().material = closestMat;
                closestObj = null;
            }
            colliding.Remove(other.gameObject);
        }
    }

    private GameObject GetNearestObject(GameObject ignore = null){
        GameObject minObj = null;
        float minDist = Mathf.Infinity, dist = 0f;
        foreach(GameObject obj in colliding){
            //Ignore if given parameter
            if(obj == ignore) continue;
            //Compare squared distance to avoid sqroot
            dist = (obj.transform.position - transform.position).sqrMagnitude;
            if(dist < minDist){
                minObj = obj;
                minDist = dist;
            }
        }
        return minObj;
    }
}
