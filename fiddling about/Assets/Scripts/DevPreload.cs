using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevPreload : MonoBehaviour 
{
    public bool thingsWorking = false;
    public bool tellTheUpdateScriptToDoThings = false;
    public int dummyTimer = 0;
    void Awake()
    {
        if(!thingsWorking)
        {
            tellTheUpdateScriptToDoThings = true;
        }
    }
    void Update()
    {
        if(tellTheUpdateScriptToDoThings)
        {
            GameObject check = GameObject.Find("GameManager");
            if (check==null)
            { 
                UnityEngine.SceneManagement.SceneManager.LoadScene("_preload"); 
            }
            else
            {
                thingsWorking = true;
                tellTheUpdateScriptToDoThings = false;
            }
        }
    }
}
