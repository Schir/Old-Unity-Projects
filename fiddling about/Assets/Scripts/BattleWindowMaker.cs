using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BattleWindowMaker : MonoBehaviour {
	public GameObject GetTheManager;
	public GameObject BattleWindowReference;
	public GameObject ItemSubMenuReference;
	public List<CharacterStats> ListOfCharacters;
	public List<string> TextToBeDisplayed;
	public RectTransform StatsWindow = new RectTransform();
	public Text StatTextInfo;
	string battleText;
	string battleTextCharacter;
	string battleTextHP;
	string battleTextMP;
	string battleTextAC;
	public Sprite background;
	public Sprite backgroundYellow;

	public GameObject getBattleWindow;
	public bool doesTheStatsWindowNeedAnUpdate = true;
	public bool doesTheItemSubMenuNeedAnUpdate = false;
	public bool isTheItemSubMenuActive = false;
	public bool shouldTheItemMenuStillBeActive = false;
	public bool haveWeInitiatedEverything = false;
	public List<Item> tempBag;
	public List<Item> tempBag2;
	public int currentItemIndex = 0;
	public int currentItemCursorPosition = 0;
	public int lastDisplayedItem = 7;
	public int firstDisplayedItem = 0;
	public int itemMax;
	public int dummyTimer = 0;
	public int timerMax = 40;
	public Camera refCamera;
	public BattleStateMachine bsm;
	// Use this for initialization
	void Start () {

	}
	void Awake()
	{
		GetTheManager = GameObject.Find("GameManager");
		getBattleWindow = GameObject.Find("BattleManager");
		bsm = getBattleWindow.GetComponent<BattleStateMachine>();
		background = Resources.Load<Sprite>("Sprites/menuborder");
		backgroundYellow = Resources.Load<Sprite>("Sprites/menuborder2");
		TextToBeDisplayed = new List<string>();
		refCamera = GameObject.FindGameObjectWithTag("BattleCamera").GetComponent<Camera>();
		ListOfCharacters = GetTheManager.GetComponent<GameStates>().currentParty;
		tempBag = GetTheManager.GetComponent<GameStates>().currentInventory;
		itemMax = GetTheManager.GetComponent<GameStates>().maxBagSize;
		tempBag2 = new List<Item>();
		//ChangeStatsWindow();
		BattleWindowReference = MakeStatsWindow();
		ItemSubMenuReference = new GameObject();
		ItemSubMenuReference.transform.parent = getBattleWindow.transform;
		shouldTheItemMenuStillBeActive = false;
	}

	// Update is called once per frame
	void Update () 
	{
			if(doesTheStatsWindowNeedAnUpdate)
			{
				Destroy(BattleWindowReference);
				//ChangeStatsWindow();
				BattleWindowReference = MakeStatsWindow();
				doesTheStatsWindowNeedAnUpdate = false;
			}
			if(isTheItemSubMenuActive && !shouldTheItemMenuStillBeActive)
			{
				ItemSubMenuReference.SetActive(false);
				isTheItemSubMenuActive = false;
			}
			if(!isTheItemSubMenuActive && shouldTheItemMenuStillBeActive)
			{
				ItemSubMenuReference.SetActive(true);
				isTheItemSubMenuActive = true;
			}
			if(doesTheItemSubMenuNeedAnUpdate)
			{
				Destroy(ItemSubMenuReference);
				ChangeItemSubWindow();
				ItemSubMenuReference = ItemSubMenu();
				doesTheItemSubMenuNeedAnUpdate = false;
			}
			//debug thing
			if(Input.GetKeyDown(KeyCode.Delete))
			{
				doesTheStatsWindowNeedAnUpdate = true;
			}
			//===================================
			// ITEM WINDOW EDGE HANDLING
			//===================================
			if(isTheItemSubMenuActive && shouldTheItemMenuStillBeActive)
			{
				//jesus christ dude, re-examine what this is doing
				if(Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetKeyDown(KeyCode.W)))
				{
					if(currentItemIndex - 2 < 0)
					{
						currentItemIndex = tempBag.Count-2;
						firstDisplayedItem = tempBag.Count-8;
						lastDisplayedItem = firstDisplayedItem + 7;
					}
					else
					{
						currentItemIndex -= 2;
						if(currentItemIndex < firstDisplayedItem)
						{
							firstDisplayedItem -=2;
							lastDisplayedItem -=2;
						}
					}
					doesTheItemSubMenuNeedAnUpdate = true;
				}
				if(Input.GetKeyDown(KeyCode.LeftArrow) || (Input.GetKeyDown(KeyCode.A)))
				{
					if(currentItemIndex - 1 < 0)
					{
						if(tempBag.Count % 2 == 0)
						{
							currentItemIndex = tempBag.Count-1;
							firstDisplayedItem = tempBag.Count-8;
							lastDisplayedItem = firstDisplayedItem + 7;
						}
						else
						{
							currentItemIndex = tempBag.Count - 1;
							firstDisplayedItem = tempBag.Count - 7;
						}
					}
					else
					{
						currentItemIndex -= 1;
						if(currentItemIndex < firstDisplayedItem)
						{
							firstDisplayedItem -=2;
							lastDisplayedItem -=2;
						}
					}
					doesTheItemSubMenuNeedAnUpdate = true;
				}
			if(Input.GetKeyDown(KeyCode.DownArrow) || (Input.GetKeyDown(KeyCode.S)))
			{
				if(currentItemIndex + 2 > tempBag.Count-1)
				{
					currentItemIndex = 0;
					firstDisplayedItem = 0;
					lastDisplayedItem = 7;
				}
				else
				{
					currentItemIndex += 2;
					if(currentItemIndex > lastDisplayedItem && lastDisplayedItem < tempBag.Count-1)
					{
						firstDisplayedItem +=2;
						lastDisplayedItem +=2;
					}
				}
				doesTheItemSubMenuNeedAnUpdate = true;
			}
			if(Input.GetKeyDown(KeyCode.RightArrow) || (Input.GetKeyDown(KeyCode.D)))
			{
				if(currentItemIndex + 1 > tempBag.Count-1)
				{
					currentItemIndex = 0;
					firstDisplayedItem = 0;
					lastDisplayedItem = 7;
				}
				else
				{
					currentItemIndex += 1;
					if(currentItemIndex > lastDisplayedItem && lastDisplayedItem < tempBag.Count-1)
					{
						firstDisplayedItem +=2;
						lastDisplayedItem +=2;
					}
				}
				doesTheItemSubMenuNeedAnUpdate = true;
			}
			if(lastDisplayedItem > tempBag.Count-1)
			{
				lastDisplayedItem = tempBag.Count-1;
			}
			if(Input.GetKeyDown(KeyCode.X))
			{
				shouldTheItemMenuStillBeActive = false;
			}
		}
	}
	//=============================
	// CHARACTER INFO WINDOW
	//=============================

	public GameObject MakeStatsWindow()
	{
	 	GameObject StatWindow;
		GameObject StatBackground;
        Canvas StatWindowCanvas;
        Image WindowBG;
        RectTransform rectTransform;

        // Canvas
        StatWindow = new GameObject();
        StatWindow.name = "StatsWindow";
		StatWindow.transform.parent = getBattleWindow.transform;
        StatWindow.AddComponent<Canvas>();

        StatWindowCanvas = StatWindow.GetComponent<Canvas>();
        StatWindowCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        StatWindow.AddComponent<CanvasScaler>();
        StatWindow.AddComponent<GraphicRaycaster>();

		// Image
		StatBackground = new GameObject();
		StatBackground.transform.parent = StatWindow.transform;
		StatBackground.name = "background";

		WindowBG = StatBackground.AddComponent<Image>();
		WindowBG.sprite = background;
		WindowBG.type = Image.Type.Sliced;

		// Text box
        rectTransform = WindowBG.GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(refCamera.scaledPixelWidth * 0.8f,refCamera.scaledPixelHeight * 0.3f);
		rectTransform.localPosition = new Vector3(0.0f, refCamera.scaledPixelHeight * -0.3f, 0.0f);


		List<GameObject> heroList = new List<GameObject>();
		heroList = bsm.HeroGameObjectList;

		for(int i=0; i<heroList.Count; i++)
		{
			string dummystring = "";
			HeroStateMachine dummyMachine = heroList[i].GetComponent<HeroStateMachine>();
			CharacterStats hero = dummyMachine.Hero1;
			if(dummyMachine.isThisMachineActive)
			{
				GameObject highlight = new GameObject();
				highlight.transform.parent = StatBackground.transform;
				highlight.name = "Highlight";
				
				Image highlightImage = highlight.AddComponent<Image>();
				highlightImage.sprite = backgroundYellow;
				highlightImage.type = Image.Type.Sliced;
				
				RectTransform rect = highlight.GetComponent<RectTransform>();
				rect.localPosition = new Vector3(0f, (refCamera.scaledPixelHeight * (-0.01f - (i * 0.06f))) + refCamera.scaledPixelHeight * 0.12f, 0f);
				rect.sizeDelta = new Vector2(refCamera.scaledPixelWidth * 0.8f, refCamera.scaledPixelHeight * 0.07f); 
			}

			StatTextMaker(dummyMachine, StatBackground, hero.characterName, "CharacterName", 0, i, dummyMachine.isThisMachineActive);
			dummystring = "HP" + hero.hpCurrent.ToString() + " / " + hero.hpMax.ToString();
			StatTextMaker(dummyMachine, StatBackground, dummystring, "HPText", 1, i, dummyMachine.isThisMachineActive);
			dummystring = "MP" + hero.mpCurrent.ToString() + " / " + hero.mpMax.ToString();
			StatTextMaker(dummyMachine, StatBackground, dummystring, "MPText", 2, i, dummyMachine.isThisMachineActive);
			dummystring = "AC " + hero.curDef.ToString();
			StatTextMaker(dummyMachine, StatBackground, dummystring, "ACText", 3, i, dummyMachine.isThisMachineActive);
			
		}

		return StatWindow;
	}
	GameObject StatTextMaker(HeroStateMachine machine, GameObject objectParent, string text, string name, int positionWidth, int positionHeight, bool isSelected)
	{
		GameObject TextObject = new GameObject();
        TextObject.transform.parent = objectParent.transform;
        TextObject.name = name;

		Text statText = TextObject.AddComponent<Text>();
        statText.font = (Font)Resources.Load("Fonts/OpenSans-Semibold");
        statText.text = text;
        statText.fontSize = Convert.ToInt32(Math.Floor(refCamera.scaledPixelHeight * 0.06f));
		battleTextHP = "";
		StatTextInfo = statText;
		if(isSelected)
		{
			statText.color = Color.black;
		}
		else
		{
			statText.color = Color.white;
		}
		float lineHeight = CalculateLineHeight(statText);

		RectTransform transfoorm = TextObject.GetComponent<RectTransform>();
		transfoorm.localPosition = new Vector3(refCamera.scaledPixelWidth * (0.2f * positionWidth), refCamera.scaledPixelHeight *(-0.01f - (0.06f * positionHeight)), 0f);
        transfoorm.sizeDelta = new Vector2(refCamera.scaledPixelWidth * 0.8f, refCamera.scaledPixelHeight * 0.3f);

		return TextObject;
	}
	//========================
	// ITEM SUB-MENU
	//========================
	public void ChangeItemSubWindow()
	{
		if(tempBag2.Count>0)
		{
			tempBag2 = new List<Item>();
		}
		foreach(Item item in tempBag)
		{
			tempBag2.Add(item);
		}
	}
	public GameObject ItemSubMenu()
	{
		GameObject SubMenu;
		GameObject SubBackground;
		Image SubBG2;
		Canvas SubMenuCanvas;
		RectTransform ItemWindow;
		
		SubMenu = new GameObject();
        SubMenu.name = "SubMenu";
		SubMenu.transform.parent = getBattleWindow.transform;
        SubMenu.AddComponent<Canvas>();

        SubMenuCanvas = SubMenu.GetComponent<Canvas>();
        SubMenuCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        SubMenu.AddComponent<CanvasScaler>();
        SubMenu.AddComponent<GraphicRaycaster>();

		// Image
		SubBackground = new GameObject();
		SubBackground.transform.parent = SubMenu.transform;
		SubBackground.name = "background";

		SubBG2 = SubBackground.AddComponent<Image>();
		SubBG2.sprite = backgroundYellow;
		SubBG2.type = Image.Type.Sliced;

		ItemWindow = SubBackground.GetComponent<RectTransform>();
		ItemWindow.localPosition = new Vector3(refCamera.scaledPixelWidth * -0.25f, refCamera.scaledPixelHeight * 0.1f, 0.0f);
		ItemWindow.sizeDelta = new Vector2(refCamera.scaledPixelWidth * 0.25f, refCamera.scaledPixelHeight * 0.5f);

		GameObject ItemWindowCursor = new GameObject();
		ItemWindowCursor.transform.parent = SubBackground.transform;
		ItemWindowCursor.name = "ItemCursor";
		Image CursorImage = ItemWindowCursor.AddComponent<Image>();
		CursorImage.sprite = background;
		CursorImage.type = Image.Type.Sliced;
		CursorImage.color = Color.black;
		RectTransform cursorRect = ItemWindowCursor.GetComponent<RectTransform>();
		//Throw this bit in a loop
		int i = 0;
		for(int j=firstDisplayedItem; j <= lastDisplayedItem;j++)
		{
			GameObject ItemWindowText = new GameObject();
			
        	ItemWindowText.transform.parent = SubBackground.transform;
        	ItemWindowText.name = tempBag2[j].GetName();

	        Text ItemText = ItemWindowText.AddComponent<Text>();
	        ItemText.font = (Font)Resources.Load("Fonts/OpenSans-Light");
	        ItemText.text = tempBag2[j].GetName();
	        ItemText.fontSize = 25;
			RectTransform tempRectTransform = ItemWindowText.GetComponent<RectTransform>();

				if(i%2==0)
				{
					tempRectTransform.localPosition = new Vector3(refCamera.scaledPixelWidth * 0.0f, refCamera.scaledPixelHeight*(0.0f+(i/2)*-0.1f), 0);
	        		tempRectTransform.sizeDelta = new Vector2(refCamera.scaledPixelWidth * 0.2f, refCamera.scaledPixelHeight * 0.4f);
				}
				else
				{
					tempRectTransform.localPosition = new Vector3(refCamera.scaledPixelWidth * 0.11f, refCamera.scaledPixelHeight*(0.0f+(i/2)*-0.1f), 0);
	        		tempRectTransform.sizeDelta = new Vector2(refCamera.scaledPixelWidth * 0.2f, refCamera.scaledPixelHeight * 0.4f);	
				}
			if(j == currentItemIndex)
			{
				cursorRect.localPosition = tempRectTransform.localPosition - new Vector3(refCamera.scaledPixelWidth * 0.05f, refCamera.scaledPixelHeight * -0.18f, 0f);
				cursorRect.sizeDelta = new Vector2(refCamera.scaledPixelWidth * 0.13f, refCamera.scaledPixelHeight * 0.08f);
				CursorAnimator animate = ItemWindowCursor.AddComponent<CursorAnimator>();
				animate.setAnimation(false);
				ItemText.color = Color.white;
			}
			else
			{
				ItemText.color = Color.black;
			}	
			i++;
		}
		//figure out everything that needs to go in here, and then

		//at the very end
		return SubMenu;
	}
	public Item SelectItem()
	{
		//This section's a mess. I need to rewrite it.

		if(tempBag2[currentItemIndex] != null)
		{
			Item tempItem = tempBag2[currentItemIndex];
			return tempItem;
		}
		else
		{
			return null;
		}
	}

	public void RemoveItemFromBag()
	{
		if(tempBag2[currentItemIndex] != null)
		{
			//tempBag2[currentItemIndex].Effect();
			tempBag2.RemoveAt(currentItemIndex);
			tempBag.RemoveAt(currentItemIndex);
			Destroy(ItemSubMenuReference);
			ItemSubMenuReference = new GameObject();
			ItemSubMenuReference.transform.parent = getBattleWindow.transform;
		}
	}
	public GameObject ItemTextHighlight(int i)
	{
		return null;
	}
	private float CalculateLineHeight(Text text)
	{
		Vector2 extents = text.cachedTextGenerator.rectExtents.size * 0.5f;
    	float lineHeight = text.cachedTextGeneratorForLayout.GetPreferredHeight("A", text.GetGenerationSettings(extents));
		return lineHeight;
	}
}
