using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractScript : MonoBehaviour
{
    public List<GameObject> rootNodes;
    public Dictionary<int, GameObject> connected;
    public List<string> statNames;
    public List<int> statVals;

    private Dictionary<int, List<GameObject>> colliding;
    private List<GameObject> parts;

    void Start(){
        connected = new Dictionary<int, GameObject>();
        colliding = new Dictionary<int, List<GameObject>>();
        parts = new List<GameObject>();
        parts.Add(gameObject);
    }

    public bool OnChildTriggerEnter(Collider other, Collider child){
        int index = rootNodes.IndexOf(child.gameObject);
        if(!connected.ContainsKey(index) && other.gameObject.tag == "Connection" && !parts.Contains(other.transform.parent.gameObject)){
            if(!colliding.ContainsKey(index)) colliding[index] = new List<GameObject>();
            colliding[index].Add(other.gameObject);
            return true;
        }
        return false;
    }

    public void OnChildTriggerExit(Collider other, Collider child){
        int index = rootNodes.IndexOf(child.gameObject);
        if(colliding.ContainsKey(index) && colliding[index].Contains(other.gameObject)) colliding[index].Remove(other.gameObject);
    }

    //Connect colliding nodes, ignore if already connected
    public void InteractButtonPress(){
        for(int i = 0; i < rootNodes.Count; i++){
            GameObject other = GetNearestObject(i);
            if(connected.ContainsKey(i) || other == null) continue;
            FixedJoint f = gameObject.AddComponent<FixedJoint>() as FixedJoint;
            f.connectedBody = other.transform.parent.GetComponent<Rigidbody>();
            f.enableCollision = false;
            other.transform.parent.GetComponent<InteractScript>().ConnectTo(gameObject, other);
            ConnectTo(other.transform.parent.gameObject, rootNodes[i]);
            Grab(true);
        }
    }

    public void Grab(bool grabIsTrue){
        for(int i = 0; i < rootNodes.Count; i++){
            if(!connected.ContainsKey(i) && grabIsTrue){
                rootNodes[i].GetComponent<MeshRenderer>().enabled = true;
            }
            else{
                rootNodes[i].GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    //Labels node as connected
    public void ConnectTo(GameObject other, GameObject node = null){
        if(node != null) connected[rootNodes.IndexOf(node)] = other;
        List<GameObject> tempParts = other.GetComponent<InteractScript>().GetParts();
        List<GameObject> toAdd = new List<GameObject>();
        foreach(GameObject obj in tempParts){
            if(!parts.Contains(obj)){
                parts.Add(obj);
                toAdd.Add(obj);
            }
        }
        foreach(GameObject obj in toAdd){
            obj.GetComponent<InteractScript>().ConnectTo(gameObject);
        }
    }

    public List<GameObject> GetParts(){
        return parts;
    }

    //Returns stats of total item
    public Dictionary<string, int> GetStats(){
        Dictionary<string, int> temp = new Dictionary<string, int>();
        foreach(GameObject obj in parts){
            InteractScript inS = obj.GetComponent<InteractScript>();
            for(int i = 0; i < inS.statNames.Count; i++){
                if(!temp.ContainsKey(inS.statNames[i])) temp.Add(inS.statNames[i], 0);
                temp[inS.statNames[i]] += inS.statVals[i];
            }
        }
        return temp;
    }

    private GameObject GetNearestObject(int index, GameObject ignore = null){
        if(!colliding.ContainsKey(index)) return null;
        GameObject minObj = null;
        float minDist = Mathf.Infinity, dist = 0f;
        foreach(GameObject obj in colliding[index]){
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
