using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class splashTimer : MonoBehaviour
{
    public float timer;
    public float timerStart;
    public float timerMax;
    public GameObject Manager;
    // Start is called before the first frame update
    void Awake()
    {
        timerStart = Time.time;
        timerMax = 1f;
        Manager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        if((Time.time - timerStart) > timerMax)
        {
            Manager.GetComponent<GameStates>().setCurrentState(GameStates.GameState.TITLE);
        }
    }
}
