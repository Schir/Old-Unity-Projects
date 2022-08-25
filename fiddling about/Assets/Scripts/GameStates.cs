using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStates : MonoBehaviour {
	public bool outOfHouseTesting = true;
	public bool preloadHasHappened = false;
	public bool splashHasLoaded = false;
	public int currentFloor = 0;
	public int gold = 0;
	public WorldItemInformation currentwiiref;
	public CharacterStats character1Stats;
	public CharacterStats character2Stats;
	public List<CharacterStats> currentParty;
	public List<Item> currentInventory;
	public int maxBagSize = 32;
	public enum GameState
	{
		PRELOAD,
		SPLASH,
		TITLE,
		CUTSCENE,
		ENTERTHETOWN,
		TOWN,
		ENTERTHEDUNGEON,
		DUNGEON,
		BATTLE,
		LOADING,
		EXITTHEDUNGEON,
		OPTIONS

	}
	public bool systemMenuOn = false;
	public GameState currentGameState;
	public bool isAWindowActive = false;
	public bool areWeWaiting = false;
	public int positionInScene = 1;
	public GameObject SystemMenuReference;
	public bool systemMenuInitialized = false;

	//--------------------------------------
	//INITIALIZATION AND UPDATE
	//--------------------------------------
	void Start () {
		currentGameState = GameState.PRELOAD;
		character1Stats = new CharacterStats();
		character2Stats = new CharacterStats();
		currentInventory = new List<Item>();
		currentFloor = 0;
		gold = 0;
	}


    // Update is called once per frame
    void Update () {
	if(!systemMenuOn)
	{
		switch(currentGameState)
		{
			case GameState.PRELOAD:
				if(!preloadHasHappened)
				{
					doPreload();
				}
				break;
			case GameState.SPLASH:
				Splash();
				break;
			case GameState.TITLE:
				if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Title")
					{
						UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
					}
				break;
			case GameState.CUTSCENE:
			if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Cutscene1")
					{
						UnityEngine.SceneManagement.SceneManager.LoadScene("Cutscene1");
					}
					if(!areWeWaiting)
					{
						GameObject gj = new GameObject();
						WorldItemInformation wii = gj.AddComponent<WorldItemInformation>();
						wii.setDialogBoxInfo(0,positionInScene);
						wii.CallMakeWindow(this);
						isAWindowActive = wii.isThisOneActive;
						currentwiiref = wii;
						areWeWaiting = true;
					}
				if(!isAWindowActive)
				{
					if(positionInScene == 2)
					{
						currentGameState = GameState.ENTERTHETOWN;
					}
					else
					{
						positionInScene++;
						areWeWaiting = false;
					}
				}
				else
				{
					if(!currentwiiref.shouldMenuBeActive)
					{
						isAWindowActive = false;
					}
				}
				break;
			case GameState.ENTERTHETOWN:
				UnityEngine.SceneManagement.SceneManager.LoadScene("town");
				currentFloor = 0;
				currentGameState = GameState.TOWN;
				break;
			case GameState.TOWN:
				break;
			case GameState.ENTERTHEDUNGEON:
				//active players are added to the party in TitleCursorMover
				if(outOfHouseTesting)
				{
					UnityEngine.SceneManagement.SceneManager.LoadScene("test2OutOfHouse");
				}
				else
				{
					UnityEngine.SceneManagement.SceneManager.LoadScene("test2");
				}
				currentFloor = 1;
				currentGameState = GameState.DUNGEON;
				break;
			case GameState.DUNGEON:

				break;
			case GameState.BATTLE:
				break;
			case GameState.LOADING:
				break;
			case GameState.EXITTHEDUNGEON:
				currentFloor = 0;
				currentGameState = GameState.ENTERTHETOWN;
				break;
			case GameState.OPTIONS:
				break;

		}
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			systemMenuOn = true;
		}
	}
	else
	{
		//make a status menu.
		if(!systemMenuInitialized)
		{
			SystemMenuReference = new GameObject();
			SystemMenuReference.AddComponent<MainMenu>();
			systemMenuInitialized = true; 
		}
		if(!SystemMenuReference.GetComponent<MainMenu>().isThisStillOn)
		{
			systemMenuOn = false;
			systemMenuInitialized = false;
			Destroy(SystemMenuReference);
		}
	}
		debugSaveCheck();
	}

	//------------------------------------------------
	//GETTERS AND SETTERS
	//------------------------------------------------
	
    public void setCurrentState(GameState state)
    {
        currentGameState = state;
    }

	public GameState getCurrentState()
	{
		return currentGameState;
	}
    
	public void callSetCharacter1Stats()
	{
		setCharacter1Stats();
	}
	public void setCharacter1Stats()
	{
		character1Stats.hpMax = 10;
		character1Stats.hpCurrent = 10;
		character1Stats.mpMax = 0;
		character1Stats.mpCurrent = 0;
		character1Stats.attack = 10;
		character1Stats.curAtk = character1Stats.attack;
		character1Stats.defense = 10;
		character1Stats.curDef = character1Stats.defense;
		character1Stats.intelligence = 16;
		character1Stats.curInt = character1Stats.intelligence;
		character1Stats.characterName = "Claire";
		character1Stats.experience = 0;
		character1Stats.level = 1;
		character1Stats.canLevelUp =false;
		character1Stats.characterJob = CharacterStats.Job.Reporter;
		character1Stats.isInParty = true;
		character1Stats.isDead = false;
		character1Stats.isEnemy = false;
	}
	public void callSetCharacter2Stats()
	{
		setCharacter2Stats();
	}
	public void setCharacter2Stats()
	{
		character2Stats.hpMax = 10;
		character2Stats.hpCurrent = 10;
		character2Stats.mpMax = 0;
		character2Stats.mpCurrent = 0;
		character2Stats.attack = 10;
		character2Stats.curAtk = character2Stats.attack;
		character2Stats.defense = 10;
		character2Stats.curDef = character2Stats.defense;
		character2Stats.intelligence = 16;
		character2Stats.curInt = character2Stats.intelligence;
		character2Stats.characterName = "November";
		character2Stats.experience = 0;
		character2Stats.level = 1;
		character2Stats.canLevelUp =false;
		character2Stats.characterJob = CharacterStats.Job.FleshAndroid;
		character2Stats.isInParty = true;
		character2Stats.isDead = false;
		character2Stats.isEnemy = false;
	}
	public void updateCharacterInfo(CharacterStats input)
	{
		switch(input.characterName)
		{
			case "Claire":
				character1Stats = input;
				break;
			case "November":
				character2Stats = input;
				break;
		}
	}
	public void callUpdateCharacterInfo(CharacterStats input)
	{
		updateCharacterInfo(input);
	}
	public void callAddToParty()
	{
		addAllActivePlayersToParty();
	}

	private void addAllActivePlayersToParty()
    {
       if(character1Stats.isInParty && !currentParty.Contains(character1Stats))
	   {
		   currentParty.Add(character1Stats);
	   }
	   if(character2Stats.isInParty && !currentParty.Contains(character2Stats))
	   {
		   currentParty.Add(character2Stats);
	   }
    }
	public List<CharacterStats> callGetPartyInfo()
	{
		return getPartyInfo();
	}
	public List<CharacterStats> getPartyInfo()
	{
		return currentParty;
	}

	public List<Item> getBagContents()
	{
		return currentInventory;
	}
	
	//this one's mainly for dealing with battle.
	public void updateBagContents(List<Item> newBag)
	{
		currentInventory = newBag;
	}
	public void AddItemToBag(Item newItem)
	{
		if(currentInventory.Count > maxBagSize)
		{
			//give the player a message once I've implemented a dialog box system
		}
		else
		{
			currentInventory.Add(newItem);
		}
	}
	public int GetCurrentFloor()
	{
		return currentFloor;
	}
	public void SetCurrentFloor(int floor)
	{
		currentFloor = floor;
	}

	//======================
	//SAVE AND LOAD
	//======================

		//Note to self:
    //Now, I know you're going to forget this. Remember to add flags 
    //and other character info to these Save and Load methods when
    //you get around to adding them in. Don't forget!
	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file1 = File.Create(Application.persistentDataPath + "/player1Info.sav");

		PlayerData data = new PlayerData();
		data.Player1Data = character1Stats;
		data.Player2Data = character2Stats;
		data.gold = gold;
		data.currentScene = SceneManager.GetActiveScene().name;
		//data.cameraHolder = GameObject.Find("Main Camera");
		data.savedState = currentGameState;
		data.savedInventory = currentInventory;
				
		bf.Serialize(file1, data);
		file1.Close();
	}
	public void Load()
	{
		if(File.Exists(Application.persistentDataPath + "/player1Info.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file1 = File.Open(Application.persistentDataPath + "/player1Info.sav", FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize(file1);
			file1.Close();
			character1Stats = data.Player1Data;
			character2Stats = data.Player2Data;


			gold = data.gold;
			currentInventory = data.savedInventory;
			currentGameState = data.savedState;
			addAllActivePlayersToParty();
			SceneManager.LoadScene(data.currentScene);
		}
	}

	//=========================
	//MISCELLANIA
	//=========================

    private void Splash()
	{
		//do a timer then head to the title screen
		if(!splashHasLoaded)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene("Splash");
			splashHasLoaded = true;
		}
		//currentGameState = GameState.TITLE;
	}
	public void debugSaveCheck()
	{
		if(Input.GetKeyDown(KeyCode.End))
		{
			Save();
			Debug.Log("Your data has been saved!");
		}
	}
       private void doPreload()
    {
		//do a timer and then,
		preloadHasHappened = true;
        currentGameState = GameState.SPLASH;
    }

	public void Inn(int price)
	{
		if(gold - price >= 0)
		{
			foreach(CharacterStats character in currentParty)
			{
				character.hpCurrent = character.hpMax;
				if(character.canLevelUp)
				{
					LevelUp(character);
				}
			}
			gold -= price;
			Debug.Log("Thank you for staying with us. Checkout is at 11.");
		}
		else
		{
			Debug.Log("You can't afford to pay.");
		}
	}

    private void LevelUp(CharacterStats character)
    {
        character.hpMax++;
		character.hpCurrent = character.hpMax;
		character.level++;
		character.experience -= 100;
		if(character.experience >= 100)
		{
			LevelUp(character);
		}
		//do the rest later.

    }
}


//CLASS TO HOLD SAVE INFORMATION
[System.Serializable]
class PlayerData
{
	public CharacterStats Player1Data;
	public CharacterStats Player2Data;
	public int gold;
	public string currentScene;
	//public GameObject cameraHolder;
	public GameStates.GameState savedState;
	public List<Item> savedInventory;
}


