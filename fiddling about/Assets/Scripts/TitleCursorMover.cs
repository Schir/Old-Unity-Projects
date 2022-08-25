using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleCursorMover : MonoBehaviour {
	private float xRotation = 0f;
	private float deltaXRotation = 0f;
	private float timer;
	private float timerMax;
	private int choiceHolder = 0;
	public GameObject manager;

	private GameObject findTheText;
	public bool notBusted = true;
	private enum MenuChoices
	{
		Start,
		Load,
		Options
	}
	private MenuChoices currentSelection;
	private Vector3 zeroItOut = new Vector3(0f,0f,0f);


	//-----------------------------------------------------------
	// Use this for initialization
	void Start () {
		timer = 0f;
		timerMax = 2f;
		currentSelection = MenuChoices.Start;
	}
	
	// Update is called once per frame
	void Update () {
		if(notBusted)
		{
			AnimateCursor();
			CheckInput();
			ChoiceUpdate(choiceHolder);
		}
	}
	//------------------------------------------------------------
    void AnimateCursor()
	{
		deltaXRotation = Time.deltaTime * 360f / timerMax;
		timer += Time.deltaTime;
		if(xRotation >= 360f)
		{
			transform.rotation = Quaternion.Euler(zeroItOut);
		}
		else
		{
			transform.Rotate(new Vector3(deltaXRotation, 0f, 0f));
		}
		if(timer >= timerMax)
		{
			timer = 0f;
		}
	}
	void CheckInput()
	{
		if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
		{
			choiceHolder++;
			CheckOutOfBounds();
		}
		if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
		{
			choiceHolder--;
			CheckOutOfBounds();
		}
		if(Input.GetKeyDown(KeyCode.Z) ||Input.GetKeyDown(KeyCode.Return))
		{
			manager = GameObject.Find("GameManager");
			notBusted = false;
			switch(currentSelection)
			{
				case MenuChoices.Start:
				//calling some functions in the gamemanager singleton. There's probably a way to make it so I don't need a call method to call a method. Shrug.
				manager.GetComponent<GameStates>().callSetCharacter1Stats();								
				manager.GetComponent<GameStates>().callSetCharacter2Stats();
				manager.GetComponent<GameStates>().callAddToParty();
				List<Item> dummything = new List<Item>();
				dummything.Add(new Potion());
				dummything.Add(new BiodesiaBloom());
				dummything.Add(new Attack());
				System.Random rnd = new System.Random();
				for(int i = 0; i < 12; i++)
				{
					int next = rnd.Next(0, 3);
					switch(next)
					{
						case 0:
							manager.GetComponent<GameStates>().AddItemToBag(new Potion());
							break;
						case 1:
							manager.GetComponent<GameStates>().AddItemToBag(new Potion());
							break;
						case 2:
							manager.GetComponent<GameStates>().AddItemToBag(new BiodesiaBloom());
							break;
						case 3:
							manager.GetComponent<GameStates>().AddItemToBag(new Attack());
							break;
					}
				}
				UnityEngine.SceneManagement.SceneManager.LoadScene("Cutscene1");
				manager.GetComponent<GameStates>().setCurrentState(GameStates.GameState.CUTSCENE);
				break;
				case MenuChoices.Load:
				manager.GetComponent<GameStates>().Load();
				break;
				case MenuChoices.Options:
				manager.GetComponent<GameStates>().setCurrentState(GameStates.GameState.OPTIONS);
				break;

			}
		}
	}

    private void CheckOutOfBounds()
    {
       if(choiceHolder < 0)
	   {
		   choiceHolder = 2;
	   }
	   if (choiceHolder > 2)
	   {
		   choiceHolder = 0;
	   }
    }
	
    private void ChoiceUpdate(int selection)
    {
        switch(selection)
		{
			case 0:
			currentSelection = MenuChoices.Start;
			break;
			case 1:
			currentSelection = MenuChoices.Load;
			break;
			case 2:
			currentSelection = MenuChoices.Options;
			break;
		}
		AnimateTransition();
    }

    private void AnimateTransition()
    {
        switch(currentSelection)
		{
			case MenuChoices.Start:
			findTheText = GameObject.Find("StartText");
			break;
			case MenuChoices.Load:
			findTheText = GameObject.Find("LoadText");
			break;
			case MenuChoices.Options:
			findTheText = GameObject.Find("OptionText");
			break;
		}
		transform.position = findTheText.transform.position + (30 * Vector3.Normalize(Vector3.up)) + (120 * Vector3.Normalize(Vector3.left));
    }

	//use this in the way you used it in the HeroStateMachine
	//remember to credit Unity user Spankenstein for this.
	private float CalculateLineHeight(Text text)
	{
		Vector2 extents = text.cachedTextGenerator.rectExtents.size * 0.5f;
    	float lineHeight = text.cachedTextGeneratorForLayout.GetPreferredHeight("A", text.GetGenerationSettings(extents));
		return lineHeight;
	}
}
