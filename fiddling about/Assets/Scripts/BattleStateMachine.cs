using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStateMachine : MonoBehaviour {
	public List<EnemyStateMachine> Enemies;
	public List<GameObject> EnemyGameObjectList;
	public List<GameObject> HeroGameObjectList;
	public List<HeroStateMachine> Heroes;
	public List<EnemyHolder> EnemyHolderList;
	public GameObject Manager;
	public Enemy enemyStuff;
	public int currentFloor;
	public int xpGained = 0;

	//Kind of a generic implementation of an attack action.
	public enum PerformAction
	{
		
		WAITING, //WAITING is what this machine's going to be doing most of the time. When an actor needs to attack or something, they'll send a command to set the machine to TAKEACTION.
		TAKEACTION, //TAKEACTION is the state the machine's in when it's figuring out what attack's happening and who's getting hit.
		PERFORMACTION, //Once the machine knows who to attack and who to hit, perform the action. Change one of the other enums to the next state and go back to waiting until PerformAction's needed again.
		CHECKDEAD,// Do both sides still have members left?
		ENDOFTURN,
		DONE
	}
	public PerformAction currentPerformState;

	public List<TurnHandler> TurnList = new List<TurnHandler>(); //A list of the turns that need to be processed.

	public List<CharacterStats> HeroesInBattle = new List<CharacterStats>(); //A list of the characters on the hero's side.
	public List<BaseEnemy> EnemiesInBattle = new List<BaseEnemy>(); //A list of the enemies on the enemy's side.
	public List<Characters> CreaturesInCombat = new List<Characters>(); //list of all participants in the combat.
	public enum HeroGUI
	{
		ACTIVATE, //wake up the hero's GUI.
		WAITING,
		INPUT1, //input for the first set of menus (fight/magic/item/run). It might be interesting to have magic be unusable in combat.
		INPUT2,//select enemy?
		CHECKREMAINING,
		DONE
	}
	public HeroGUI HeroInput;
	public List<GameObject> CharactersToManage = new List<GameObject>();
	//private TurnHandler HeroChoice;

	public enum EnemyChoice
	{
		ACTIVATE,
		PROCESSATTACK,
		SENDSIGNAL,
		DONE
	}
	public EnemyChoice CurrentEnemyState;
	public enum BattleState
	{
		INITIALIZE,
		BATTLE,
		WIN,
		GETLOOT,
		PROCESSXP,
		LOSE,
		END
	}
	public BattleState currentBattleState;
	public GameObject enemyButton;
	public int currentPlayerToMove = 0;
	public int goldGained = 0;
	public GameObject WindowManager;
	public bool statsWindowEnabled = false;
	public bool areAllTheEnemiesDead = false;
	public bool areAllTheHeroesDead = false;
	public Camera refCamera;
	public bool isAWindowActive = false;
	public List<WorldItemInformation> infoList;
//-------------------------------------------------

	//============================================
	// INITIALIZATION OF STATES.
	//============================================
	void Start () {
		
	}



	void Awake()
	{
		enemyStuff = new Enemy();
		Manager = GameObject.Find("GameManager");
		EnemiesInBattle = new List<BaseEnemy>();
		refCamera = GameObject.FindGameObjectWithTag("BattleCamera").GetComponent<Camera>();
		Enemies = new List<EnemyStateMachine>();
		tag = "BattleManager";
		//We're adding the windowManager here and adding the BattleWindowMaker to it later because this is a complicated deck of cards whoops.
		WindowManager = new GameObject();
		WindowManager.name = "BattleWindowInfo";
		WindowManager.transform.parent = transform;
		statsWindowEnabled = true;
		HeroesInBattle = new List<CharacterStats>();
		infoList = new List<WorldItemInformation>();
		foreach(CharacterStats character in Manager.GetComponent<GameStates>().getPartyInfo())
		{
			if(character.isInParty)
			{
				HeroesInBattle.Add(character);
			}
		}
		Heroes = new List<HeroStateMachine>();


		EnemyGameObjectList = new List<GameObject>();
		HeroGameObjectList = new List<GameObject>();

		currentBattleState = BattleState.INITIALIZE;
		//initialize everything. This means
		//get the floor
		currentFloor = Manager.GetComponent<GameStates>().GetCurrentFloor();
		//initialize the pool of enemies. We're throwing this in its own method because it's going to be a massive pain. 
		//This should probably return a list<EnemyStateMachine>
		InitializeEnemyPool(currentFloor);
		InitializeHeroPool();
		
		foreach(GameObject thing in EnemyGameObjectList)
		{
			CharactersToManage.Add(thing);
		}
		foreach(GameObject thing in HeroGameObjectList)
		{
			CharactersToManage.Add(thing);
		}

		WindowManager.AddComponent<BattleWindowMaker>();


		//then,
		currentPerformState = PerformAction.TAKEACTION;
		currentBattleState = BattleState.BATTLE;
		HeroInput = HeroGUI.ACTIVATE;
		CurrentEnemyState = EnemyChoice.DONE;
	}

    // Update is called once per frame
    void Update () 
	{
		if(!isAWindowActive)
		{
		//The overall state of the battle.
		ProcessBattle();

		//Handles the actions.
		switch(currentPerformState)
		{
			case PerformAction.TAKEACTION:
			WindowManager.GetComponent<BattleWindowMaker>().doesTheStatsWindowNeedAnUpdate = true;
			//gathering the actions into a list. Sort by speed if you're feeling adventurous.
			break;
			case PerformAction.PERFORMACTION:
			//process actions in the turn list until the list's empty.
			//use a window maker to pop up windows explaining what's being done.
			Dialogue dlog = ScriptableObject.CreateInstance<Dialogue>();
			dlog.DialogItems = new List<DialogueElement>();
			for(int i=0; i < TurnList.Count; i++)
			{
				TurnList[i].Attack.SetInitiator(TurnList[i].Attacker);
				TurnList[i].Attack.SetTarget(TurnList[i].TargetOfAttack);
				DialogueElement d = new DialogueElement();
				d.Character = DialogueElement.Characters.none;
				d.CharacterPosition = DialogueElement.AvatarPos.none;
				string s = TurnList[i].Attacker.characterName + " uses " + TurnList[i].Attack.itemName + " on " + TurnList[i].TargetOfAttack.characterName + "! Oh no!";

				dlog.DialogItems.Add(d);
				TurnList[i].Attack.Effect();
				s += "\n" + TurnList[i].Attack.GetMessage();
				if(TurnList[i].TargetOfAttack.isDead)
				{
					s += "\n" + TurnList[i].TargetOfAttack.characterName + " is stone-cold fucking dead now.";
				}
				d.DialogueText = s;
				//WindowManager.GetComponent<BattleWindowMaker>().doesTheStatsWindowNeedAnUpdate = true;
				CheckIfEveryoneIsDeadYet();
			}
			TurnList.RemoveRange(0, TurnList.Count);
			GameObject go = new GameObject();
			go.transform.parent = transform;
			WorldItemInformation wiii = go.AddComponent<WorldItemInformation>();
			wiii.CallMakeWindow(dlog, this);
			isAWindowActive = true;


			//then,
			currentPerformState = PerformAction.ENDOFTURN;
			break;
			case PerformAction.ENDOFTURN:
			CheckIfEveryoneIsDeadYet();
			if(areAllTheEnemiesDead || areAllTheHeroesDead)
			{
				currentPerformState = PerformAction.DONE;
			}
			else
			{
				currentPerformState = PerformAction.WAITING;
			}
			break;
			case PerformAction.WAITING:
			//just waiting around.			
			break;
			case PerformAction.DONE:
			break;
		}


		//==================================
		//	WHAT'S THE PLAYER DOING?
		//================================
		HandleHero();
		
		//======================================
		//    WHAT'S THE ENEMY UP TO?
		//======================================
		HandleEnemy();
		}
		else
		{

		}
	}

   

    private void ProcessBattle()
    {
        switch(currentBattleState)
		{
			case BattleState.INITIALIZE:
				//load up all the menus and initialize them here.

				break;
			case BattleState.BATTLE:
				break;
			case BattleState.WIN:
				//pop up a message saying "You survived! I'm glad."
				currentBattleState = BattleState.GETLOOT;
				break;
			case BattleState.GETLOOT:
				//roll the enemy's drop table for items. Try to add them to the inventory.

				//then,
				foreach(BaseEnemy slain in EnemiesInBattle)
				{
					goldGained += slain.goldGranted;
				}
				Manager.GetComponent<GameStates>().gold += goldGained;
				Debug.Log("Gained " + goldGained + " dollars, or the regional equialent!");
				currentBattleState = BattleState.PROCESSXP;
				break;
			case BattleState.PROCESSXP:
				//process XP. If any of it is above the xp for the character to get to the next level, set the character's 'can level up' variable to true.
				foreach(BaseEnemy slain in EnemiesInBattle)
				{
					xpGained += slain.xpGranted;
				}
				foreach(CharacterStats character in HeroesInBattle)
				{
					if(!character.isDead)
					{
						character.experience += xpGained;
						Debug.Log(character.characterName + " gained: "+ xpGained + " experience!");
						if(character.experience >= 100)
						{
							character.canLevelUp = true;
						}
					}
				}
				//then,
				currentBattleState = BattleState.END;
				break;
			case BattleState.LOSE:
				//You lost, son!
				//do something. Maybe a game over?
				break;
			case BattleState.END:
				Manager.GetComponent<GameStates>().setCurrentState(GameStates.GameState.DUNGEON);
				break;
		}
    }



	private void HandleHero()
    {
        switch(HeroInput)
		{
			case HeroGUI.ACTIVATE:
			//reactivate the window for the battle icon.
			HeroInput = HeroGUI.INPUT1;
			break;
			case HeroGUI.INPUT1:

			//Let the hero state machine handle this, maybe.
			break;
			case HeroGUI.INPUT2:
			//deactivate the window with fight/item/run.

			break;
			case HeroGUI.CHECKREMAINING:
				if(currentPlayerToMove + 1 == HeroGameObjectList.Count)
				{
					HeroInput = HeroGUI.WAITING;
				}
				else
				{
					currentPlayerToMove++;
					HeroGameObjectList[currentPlayerToMove].GetComponent<HeroStateMachine>().MakeThisMachineActive(true);
					HeroInput = HeroGUI.ACTIVATE;
				}
				break;
			case HeroGUI.WAITING:
				//now that the heroes have taken their actions, it's time for the enemies to start taking theirs.
				CurrentEnemyState = EnemyChoice.ACTIVATE;
				HeroInput = HeroGUI.DONE;
				break;
			case HeroGUI.DONE:
			if(currentPerformState == PerformAction.WAITING)
			{
				currentPlayerToMove = 0;
				HeroGameObjectList[currentPlayerToMove].GetComponent<HeroStateMachine>().MakeThisMachineActive(true);
				HeroInput = HeroGUI.ACTIVATE;
				currentPerformState = PerformAction.TAKEACTION;
			}
			break;
		}
    }

	private void HandleEnemy()
	{
		switch(CurrentEnemyState)
		{
			case (EnemyChoice.ACTIVATE):
				CurrentEnemyState = EnemyChoice.PROCESSATTACK;
				break;
			case (EnemyChoice.PROCESSATTACK):
				//run through a loop of all the enemies in the enemy list, then switch the currentPerformState to PERFORMACTION
				foreach(EnemyStateMachine enemyThoughts in Enemies)
				{
					enemyThoughts.MakeThisMachineActive(true);
					enemyThoughts.currentState = EnemyStateMachine.TurnState.PROCESSING;
				}
				CurrentEnemyState = EnemyChoice.SENDSIGNAL;
				break;
			case (EnemyChoice.SENDSIGNAL):
			//now that the enemy has finished adding its turns to the list of turns, make the perform state start processing the turns in order.
			//It's probably a bit FF1 to do it that way. Need to add a handler for what to do if the target's dead. Just deal damage into the negatives? Maybe.
				currentPerformState = PerformAction.PERFORMACTION;
				//then move the enemy into doing nothing.
				CurrentEnemyState = EnemyChoice.DONE;
				break;
			case (EnemyChoice.DONE):
				break;
		}
	}
    //=========================================
    // MACHINES ARE ABOVE, METHODS ARE BELOW
    //=========================================
    public void SetCurrentHeroState(HeroGUI option)
	{
		HeroInput = option;
	}

	public void CheckIfEveryoneIsDeadYet()
	{
		
		for(int i = 0; i < EnemiesInBattle.Count; i++)
		{
			if(!EnemiesInBattle[i].isDead)
			{
				break;
			}
			else if(i == Enemies.Count-1 && Enemies[i].enemy.isDead)
			{
				currentBattleState = BattleState.WIN;
				currentPerformState = PerformAction.DONE;
				areAllTheEnemiesDead = true;
			}
		}
		for(int i = 0; i< HeroesInBattle.Count; i++)
		{
			if(!HeroesInBattle[i].isDead)
			{
				break;
			}
			else if(i == Heroes.Count-1 && HeroesInBattle[i].isDead)
			{
				currentBattleState = BattleState.LOSE;
				currentPerformState = PerformAction.DONE;
				areAllTheHeroesDead = true;
			}
		}
	}
	//=================================================
	//                  ENEMY INITIALIZATION
	//=================================================
	public List<EnemyStateMachine> InitializeEnemyPool(int currentFloor)
    {
		//implement all of this later.
		//this is probably a case for switches within switches.
		System.Random tempRnd = new System.Random();
		int positionOnScreen = 0;
		List<EnemyStateMachine> tempEnemy = new List<EnemyStateMachine>();
		List<EnemyHolder> tempEnemy1 = Enemy.setArea1Enemies();
		List<EnemyHolder> tempEnemy2 = new List<EnemyHolder>();
		List<BaseEnemy> tempEnemy3 = new List<BaseEnemy>();
        switch(currentFloor)
		{
			case 0:
				//make some kind of joke enemy since floor 0 is where the town is and anyone trying to initialize combat there is clearly cheating.
				break;
			case 1:
				
				int numberOfEnemies = tempRnd.Next(1, 4);

				for(int i=numberOfEnemies; i>0; i--)
				{
					int j = tempRnd.Next(0, tempEnemy1.Count);
					tempEnemy2.Add(tempEnemy1[j]);
					tempEnemy1.RemoveAt(j);	
				}
				foreach(EnemyHolder enemy in tempEnemy2)
				{
					GameObject enemyN = MakeEnemy(positionOnScreen, enemy);
					EnemyGameObjectList.Add(enemyN);
					tempEnemy3.Add(enemy.enemyInfo);
					positionOnScreen++;
				}
				EnemiesInBattle = tempEnemy3;
				EnemyHolderList = tempEnemy2;
				break;
			case 2:
				break;
			case 3:
				break;
			default:
				break;
		}
		return tempEnemy;
    }

    private GameObject MakeEnemy(int n, EnemyHolder enemy)
    {
        GameObject enemyN = new GameObject();
		enemyN.transform.parent = transform;
		enemyN.tag = "Enemy";
		enemyN.AddComponent<EnemyStateMachine>();
		EnemyStateMachine stateMachineTemp = enemyN.GetComponent<EnemyStateMachine>();
		stateMachineTemp.setEnemyInMachine(enemy.enemyInfo);
		enemyN.name = stateMachineTemp.enemy.characterName;
		stateMachineTemp.MakeThisMachineActive(false);
		stateMachineTemp.currentState = EnemyStateMachine.TurnState.WAITING;
		Enemies.Add(stateMachineTemp);
		GameObject enemySub1 = new GameObject();
		enemySub1.transform.parent = enemyN.transform;
		enemySub1.AddComponent<Canvas>();
		enemySub1.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
		enemySub1.AddComponent<CanvasScaler>();
        enemySub1.AddComponent<GraphicRaycaster>();

		GameObject enemyN2 = new GameObject();
		enemyN2.transform.parent = enemySub1.transform;
		Image EnemySprite = enemyN2.AddComponent<Image>();
		EnemySprite.sprite = enemy.enemyInfo.enemySprite;
		EnemySprite.type = Image.Type.Simple;
		EnemySprite.preserveAspect = true;
		RectTransform enemyRect = enemyN2.GetComponent<RectTransform>();
		enemyRect.sizeDelta = new Vector2(refCamera.scaledPixelWidth * 0.15f, refCamera.scaledPixelWidth * 0.15f); 
		switch(n)
		{
			case 0:
			enemyRect.localPosition = new Vector3(refCamera.scaledPixelWidth * 0f, refCamera.scaledPixelHeight * 0.2f, 0f);
			break;

			case 1:
			enemyRect.localPosition = new Vector3(refCamera.scaledPixelWidth * -0.16f, refCamera.scaledPixelHeight * 0.2f, 0f);
			break;

			case 2:
			enemyRect.localPosition = new Vector3(refCamera.scaledPixelWidth * 0.16f, refCamera.scaledPixelHeight * 0.2f, 0f);
			break;

			default:
			enemyRect.localPosition = new Vector3(refCamera.scaledPixelWidth * 0.32f, refCamera.scaledPixelHeight * 0.2f, 0f);
			break;
		}
		EnemyHolder.assignEnemyPositionForLaterReference(n, enemy.enemyInfo);
		return enemyN;
    }

    public List<HeroStateMachine> InitializeHeroPool()
	{
		List<HeroStateMachine> tempHeroPool = new List<HeroStateMachine>();
		int i = 0;
		//do some stuff
		foreach(CharacterStats character in HeroesInBattle)
		{
			GameObject heroN = new GameObject();
			heroN.transform.parent = transform;
			heroN.tag = "Player";
			HeroStateMachine machine = heroN.AddComponent<HeroStateMachine>();
			machine.SetCharacterInMachine(character);
			EnemyHolder.assignEnemyPositionForLaterReference(i, character);
			heroN.name = machine.Hero1.characterName;
			tempHeroPool.Add(machine);
			HeroGameObjectList.Add(heroN);
			i++;
		}


		tempHeroPool[0].MakeThisMachineActive(true);
		return tempHeroPool;
	}
	

	public void AddTurnToList(TurnHandler action)
	{
		TurnList.Add(action);
	}
	public GameObject GetBattleParticipantAtIndex(int i)
	{
		return CharactersToManage[i];
	}


}
