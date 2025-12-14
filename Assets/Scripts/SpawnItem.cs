using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class SpawnItem : MonoBehaviour
{
    public XRNode swapSource, spawnSource;
    public TextMeshPro t;
    public List<GameObject> partList;

    private bool spawn, swap;
    private float spawnTimer = 0f;
    private int partIndex = 0;
    private List<GameObject> spawnedObjects;

    void Start(){
        spawnedObjects = new List<GameObject>();
    }

    void Update(){
        InputDevice device = InputDevices.GetDeviceAtXRNode(spawnSource);
        bool tempSpawn = spawn;
        device.TryGetFeatureValue(CommonUsages.secondaryButton, out spawn);
        device = InputDevices.GetDeviceAtXRNode(swapSource);
        bool tempSwap = swap;
        device.TryGetFeatureValue(CommonUsages.secondaryButton, out swap);
        if(spawnTimer > 0) spawnTimer -= Time.deltaTime;
        else if(!tempSpawn && spawn){
            spawnTimer = 2f;
            GameObject spawnedObject = Instantiate(partList[partIndex]) as GameObject;
            spawnedObject.transform.position = new Vector3(0, 5, 0.7f);
            spawnedObjects.Add(spawnedObject);
        }
        if(!tempSwap && swap){
            partIndex++;
            if(partIndex >= partList.Count) partIndex = 0;
            t.text = partList[partIndex].name;
        }
    }
    
    public void ClearParts(){
        if(spawnedObjects == null) return;
        foreach(GameObject obj in spawnedObjects){
            Destroy(obj);
        }
    }
}
