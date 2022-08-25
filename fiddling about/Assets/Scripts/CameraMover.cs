using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//this moves the camera around in the dungeon.
public class CameraMover : MonoBehaviour {
	private RaycastHit forwardRay;
	public bool CameraDisabled;
	private float randomEncounter;
	public GameObject BattleObject;
	private GameObject minimap;
	private GameObject battleWindow;
	private int dummy;
	private int timermax;
	private bool doWeIgnoreItNow;
	private GameObject dungeonWindow;
	private GameObject dungeonCamera;
	public GameObject getTheManager;
	public bool randomEncounterHappening = false;
	public int numberOfSteps = 0;
	public bool windowActive = false;
	// Use this for initialization
	void Start () {

		CameraDisabled = false;
		dummy = 0;
		timermax = 5;
		doWeIgnoreItNow = false;
	}
	void Awake()
	{
		battleWindow = GameObject.Find("BattleWindow");
		//battleWindow = new GameObject();
		minimap = GameObject.Find("Minimap");
		DisableBattleWindow();
		dungeonWindow = GameObject.Find("DungeonWindow");
		dungeonCamera = GameObject.Find("DungeonCamera");
		getTheManager = GameObject.Find("GameManager");
		doWeIgnoreItNow = true;
	}

    // Update is called once per frame
    void Update () {
		if(!CameraDisabled)
		{
			if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
			{
				forwardRay = new RaycastHit();
				Physics.Raycast(dungeonCamera.transform.position, dungeonCamera.transform.forward, out forwardRay);
				if(forwardRay.distance > 0.6)
				{
					//Camera.main.transform.position = Camera.main.transform.position + Vector3.Normalize(Camera.main.transform.forward);
					dungeonCamera.transform.position = dungeonCamera.transform.position + Vector3.Normalize(dungeonCamera.transform.forward);
					numberOfSteps++;
					CheckRandomEncounter();
				}
			}
			if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
			{
				//Camera.main.transform.Rotate(new Vector3(0.0f, 90.0f, 0.0f));
				dungeonCamera.transform.Rotate(new Vector3(0.0f, 90.0f, 0.0f));
			}
			if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
			{
				//Camera.main.transform.Rotate(new Vector3(0.0f, -90.0f, 0.0f));
				dungeonCamera.transform.Rotate(new Vector3(0.0f, -90.0f, 0.0f));
			}
			if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
			{
				forwardRay = new RaycastHit();
				Physics.Raycast(dungeonCamera.transform.position, -dungeonCamera.transform.forward, out forwardRay);
				if(forwardRay.distance > 0.6)
				{
					dungeonCamera.transform.position = dungeonCamera.transform.position - Vector3.Normalize(dungeonCamera.transform.forward);
					numberOfSteps++;
					CheckRandomEncounter();
				}
			}
			if(Input.GetKeyDown(KeyCode.Q))
			{
				forwardRay = new RaycastHit();
				Physics.Raycast(dungeonCamera.transform.position, -dungeonCamera.transform.right, out forwardRay);
				if(forwardRay.distance > 0.6)
				{
					dungeonCamera.transform.position = dungeonCamera.transform.position - Vector3.Normalize(dungeonCamera.transform.right);
					numberOfSteps++;
					CheckRandomEncounter();
				}
			}
			if(Input.GetKeyDown(KeyCode.E))
			{
				forwardRay = new RaycastHit();
				Physics.Raycast(dungeonCamera.transform.position, dungeonCamera.transform.right, out forwardRay);
				if(forwardRay.distance > 0.6)
				{
					dungeonCamera.transform.position = dungeonCamera.transform.position + Vector3.Normalize(dungeonCamera.transform.right);
					numberOfSteps++;
					CheckRandomEncounter();
				}
			}
			if(Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Space))
			{
				forwardRay = new RaycastHit();
				if(Physics.Raycast(dungeonCamera.transform.position, dungeonCamera.transform.forward, out forwardRay))
				{
					if(forwardRay.transform.tag == "WorldItem" && forwardRay.distance <= 0.6)
					{
						if(forwardRay.transform.GetComponentInParent<WorldItemInformation>())
						{
							if(forwardRay.transform.GetComponentInParent<WorldItemInformation>().isThisTheExit)
							{
								getTheManager.GetComponent<GameStates>().setCurrentState(GameStates.GameState.EXITTHEDUNGEON);
							}
							else
							{
								forwardRay.transform.GetComponentInParent<WorldItemInformation>().CallMakeWindow(this);
								CameraDisabled = true;
								windowActive = true;
								//Debug.Log("Found an item right in front of you!!");
							}
						}
					}
					else
					{
						Debug.Log("Found something else!");
					}
				}
			}
		}
		if(randomEncounterHappening && (getTheManager.GetComponent<GameStates>().getCurrentState() == GameStates.GameState.DUNGEON))
		{
			endRandomEncounter();
		}
		if(!windowActive && !randomEncounterHappening && CameraDisabled)
		{
			CameraDisabled = false;
		}
	}
	void CheckRandomEncounter()
	{
		randomEncounter = UnityEngine.Random.Range(1.0f, 10.0f);
		if((randomEncounter > 9.0f && numberOfSteps >= 9) || numberOfSteps == 20)
		{
			getTheManager.GetComponent<GameStates>().setCurrentState(GameStates.GameState.BATTLE);
			EnableBattleWindow();
			DisableDungeonCamera();
			randomEncounterHappening = true;
		}

	}

    private void DisableDungeonCamera()
    {
		CameraDisabled = true;
        dungeonCamera.SetActive(false);
    }
	private void EnableDungeonWindow()
    {
        dungeonCamera.SetActive(true);
    }

    void endRandomEncounter()
	{
		DisableBattleWindow();
		Destroy(BattleObject);
		EnableDungeonWindow();
		getTheManager.GetComponent<GameStates>().setCurrentState(GameStates.GameState.DUNGEON);
		randomEncounterHappening = false;
		CameraDisabled = false;
		numberOfSteps = 0;
	}

    private void EnableBattleWindow()
    {
		BattleObject = new GameObject();
		BattleObject.transform.parent = battleWindow.transform;
		BattleObject.tag = "BattleManager";
		BattleStateMachine BSM = BattleObject.AddComponent<BattleStateMachine>() as BattleStateMachine;
		BattleObject.name = "BattleManager";
		battleWindow.SetActive(true);
    }
	private void DisableBattleWindow()
	{
		//battleWindow = GameObject.Find("BattleWindow");
		//Destroy(battleWindow.GetComponent<BattleStateMachine>());
		Destroy(BattleObject);
		battleWindow.SetActive(false);
	}
	//public GameObject CreateMenu() 
}
