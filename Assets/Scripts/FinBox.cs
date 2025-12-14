using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinBox : MonoBehaviour
{
    public Quest questScript;
    public TextMeshPro t;

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Grabbable"){
            if(CheckFinished(other.gameObject)){
                questScript.CreateQuest();
            }
        }
    }

    public bool CheckFinished(GameObject item){
        t.text = "";
        bool temp = true;
        Dictionary<string, int> quest = questScript.GetQuest();
        if(item == null) return false;
        Dictionary<string, int> stats = item.GetComponent<InteractScript>().GetStats();
        foreach(KeyValuePair<string, int> s in quest){
            if(!stats.ContainsKey(s.Key) || stats[s.Key] != s.Value) temp = false;
        }
        foreach(KeyValuePair<string, int> s in stats){
            if(stats.ContainsKey(s.Key)) t.text += s.Key + ": " + stats[s.Key] + "\n";
        }
        return temp;
    }
}
