using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBoxMover : MonoBehaviour
{
    public Yarn.Unity.DialogueRunner DialogueRun;
    public GameObject DialogueBox;
    public UnityEngine.UI.Text tex;
    public Dictionary<string, GameObject> NPCsInScene;
    public List<string> NPCnames;
    // Start is called before the first frame update
    void Awake()
    {
        NPCnames = new List<string>();
        NPCsInScene = new Dictionary<string, GameObject>();
        var allParticipants = new List<Yarn.Unity.Example.NPC> (FindObjectsOfType<Yarn.Unity.Example.NPC> ());
        foreach(Yarn.Unity.Example.NPC item in allParticipants)
        {
            if(item != null)
            {
                NPCsInScene.Add(item.characterName, item.gameObject);
                NPCnames.Add(item.characterName);
            }
        }
        //NPCsInScene.Add(null, GameObject.FindGameObjectWithTag("Player"));
        //DialogueRun = FindObjectOfType<Yarn.Unity.DialogueRunner>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(DialogueRun.isDialogueRunning)
        {
            string s = "";
            //if(NPCsInScene.ContainsKey(DialogueRun.currentNodeName))
            //{
            //    transform.position = NPCsInScene[DialogueRun.currentNodeName].transform.position + Vector3.up * 2f;
            //}
            foreach(string t in NPCnames)
            {
                if(tex.text.Contains(t + ":"))
                {
                    s = t;
                    break;
                }
            }
            if(NPCsInScene.ContainsKey(s))
            {
                transform.position = NPCsInScene[s].transform.position + Vector3.up * 2f;
            }
        }
    }
}
