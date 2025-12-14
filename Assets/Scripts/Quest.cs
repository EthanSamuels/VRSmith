using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using System.Linq;

public class Quest : MonoBehaviour
{
    public TextMeshPro t, level, hint;
    public GameObject finBox;
    public SpawnItem spawnScript;
    public int difficulty = 1;

    private Dictionary<string, int> quest;

    void Start(){
        quest = new Dictionary<string, int>();
        quest["Reach"] = 1;
        quest["Weight"] = 1;
        level.text = "Level: 1";
        t.text = "Reach: 1\nWeight: 1";
    }

    //Difficulty decides max items used for generating quest, quest only has 2 values to allow for multiple solutions
    public void CreateQuest(){
        difficulty++;
        level.text = "Level: " + difficulty;
        if(difficulty > 3) hint.enabled = false;
        spawnScript.ClearParts();
        quest = new Dictionary<string, int>();
        int numNodes = 2;
        for(int i = 0; i < difficulty; i++){
            int r = Random.Range(0, spawnScript.partList.Count);
            if(r != 0) numNodes--;
            InteractScript inS = spawnScript.partList[r].GetComponent<InteractScript>();
            for(int j = 0; j < inS.statNames.Count; j++){
                if(!quest.ContainsKey(inS.statNames[j])) quest[inS.statNames[j]] = 0;
                quest[inS.statNames[j]] += inS.statVals[j];
            }
            if(numNodes <= 0) break;
        }
        while(quest.Count > 2){
            int r = Random.Range(0, quest.Count);
            quest.Remove(quest.Keys.ElementAt(r));
        }
        t.text = quest.Keys.ElementAt(0) + ": " + quest.Values.ElementAt(0);
        for(int i = 1; i < quest.Count; i++){
            t.text += "\n" + quest.Keys.ElementAt(i) + ": " + quest.Values.ElementAt(i);
        }
    }

    public Dictionary<string, int> GetQuest(){ return quest; }
}
