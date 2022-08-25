using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class HeroStateMachine : CharacterStateMachine
{
    //=========================================
    // VARIABLES AFFECTING THE CIRCLE MINIGAME
    //=========================================
    private float currentCooldown = 0.0f;
    private float maxCooldown = 3f;
    private float zAngleChange;
    private float calcCooldown;
    private bool minigameStillHappening = false;
    private float[] location = new float[360];
    private float tempLocationChecker;
    private int currentArrayLocation = 0;
    private int numberOfHits = 0;
    public Image SecondHand;

    //======================================================
    //  VARIABLES AFFECTING THE DRAWING OF THE STATS MENU
    //======================================================
    private bool battleWindowEnabled = false;
    public Canvas AttackMenu;
    public CharacterStats Hero1;
    public RectTransform Cursor;
    private int menuNumber = 0;
    public GameObject Manager;
    public GameObject BattleManager;
    public GameObject WindowManager;
    BattleStateMachine battleMachineReference;
    public int currentTargetIndex = 0;
    public bool isThisMachineActive = false;
    public bool doWeNeedToRememberTheItemMenu = false;
    public bool doWeNeedToRememberAnItem = false;
    public GameObject currentTargetReference;
    public float lineHeight1;

    //============================================
    // ENUMS TO KEEP TRACK OF THE CURRENT STATE
    //============================================
    public enum MenuState
    {
        FIGHT,
        ITEM,
        RUN
    }
    public enum TurnState
    {
        PROCESSING,
        ADDTOLIST,
        WAITING,
        SELECTING,
        TARGETSELECTION,
        MINIGAME,
        ACTION,
        DEFENSEMINIGAME,
        ITEM,
        DEAD,
        RUN,
        ALMOSTDONE,
        DONE
    }
    public MenuState menuOptions;
    public TurnState currentState;
    public TurnHandler currentMove;

    //============================================================
    //   VARIABLES THAT NEED TO BE PHASED OUT AT SOME POINT
    //============================================================
    private GameObject FightHighlight;
	private GameObject ItemHighlight;
	private GameObject RunHighlight;
    private GameObject Target;
    private bool doWeActuallyNeedToUpdateTheSelection = false;
    private bool isSomeCharacterSelected = false;
    private bool shouldSomeCharacterBeSelected = false;
    public bool isTheMainMenuActive = false;
    public GameObject MainBattleMenu;
    public Camera refCamera;

//-------------------------------------------------------

    void Start()
    {

    }

    void Awake()
    {
        currentState = TurnState.PROCESSING;
        menuOptions = MenuState.FIGHT;
        Manager = GameObject.Find("GameManager");
        BattleManager = GameObject.FindGameObjectWithTag("BattleManager");
        battleMachineReference = BattleManager.GetComponent<BattleStateMachine>();
        WindowManager = GameObject.Find("BattleWindowInfo");
        currentMove = new TurnHandler();
        refCamera = GameObject.FindGameObjectWithTag("BattleCamera").GetComponent<Camera>();
        currentTargetReference = new GameObject();
        currentTargetReference.name = "Target";
        currentTargetReference.transform.parent = BattleManager.transform;
    }
    void Update()
    {
        switch(currentState)
        {
            case (TurnState.PROCESSING):
                if(isThisMachineActive)
                {
                    if(Hero1.isDead)
                    {
                        currentState = TurnState.DEAD;
                    }
                    else
                    {
                        MainBattleMenu = MakeMainBattleMenu();
                        isTheMainMenuActive = true;
                        currentState = TurnState.SELECTING;
                    }
                }
                break;
            
            case (TurnState.ADDTOLIST):

                currentState = TurnState.SELECTING;
                break;
            
            case (TurnState.WAITING):
                minigameStillHappening = true;
                currentState = TurnState.MINIGAME;
                break;
            case (TurnState.SELECTING):
                UpdateMainBattleMenu();
                break;
            case (TurnState.MINIGAME):
            //    UpdateAttackCircle();
                currentMove.Attack = new Attack();
                doWeActuallyNeedToUpdateTheSelection = true;
                //doWeNeedToRememberTheItemMenu = false;
                Destroy(MainBattleMenu);
                isTheMainMenuActive = false;
                currentState = TurnState.TARGETSELECTION;
                
                break;
            case (TurnState.TARGETSELECTION):
                isSomeCharacterSelected = true;
                battleMachineReference.SetCurrentHeroState(BattleStateMachine.HeroGUI.INPUT2);
                //do something with a list of all the possible targets.
                UpdateTargetSelection();
                if(Input.GetKeyDown(KeyCode.Z))
                {
                    //get the target's name, add that to the TurnHandler currentMove
                    
                    if(doWeNeedToRememberTheItemMenu)
                    {
                        WindowManager.GetComponent<BattleWindowMaker>().RemoveItemFromBag();
                        doWeNeedToRememberTheItemMenu = false;
                    }
                    else
                    {
                        currentMove.Attack = new Attack();
                    }
                    currentMove.Attacker = Hero1;
                    currentState = TurnState.ALMOSTDONE;
                    //WindowManager.GetComponent<BattleWindowMaker>().RemoveItemFromBag();
                }
                break;
            case (TurnState.ACTION):
                //for(int i=numberOfHits; i > 0; i--)
                //{
                    //do something with the battle state machine
                //}


                break;
            case (TurnState.ITEM):
                WindowManager.GetComponent<BattleWindowMaker>().isTheItemSubMenuActive = true;
                
                if(Input.GetKeyDown(KeyCode.Z))
                {
                    currentMove.Attack = WindowManager.GetComponent<BattleWindowMaker>().SelectItem();
                    doWeNeedToRememberTheItemMenu = true;
                    currentState = TurnState.TARGETSELECTION;
                    WindowManager.GetComponent<BattleWindowMaker>().shouldTheItemMenuStillBeActive = false;
    

                }
                else if(Input.GetKeyDown(KeyCode.X))
                {
                    WindowManager.GetComponent<BattleWindowMaker>().shouldTheItemMenuStillBeActive = false;
                    currentState = TurnState.SELECTING;
                }
                break;
            
            case (TurnState.DEFENSEMINIGAME):

                break;
            case (TurnState.DEAD):
                isThisMachineActive = false;
                battleMachineReference.SetCurrentHeroState(BattleStateMachine.HeroGUI.CHECKREMAINING);
                currentState = TurnState.DONE;
                break;
            case (TurnState.RUN):
                Manager.GetComponent<GameStates>().setCurrentState(GameStates.GameState.DUNGEON);
                currentState = TurnState.PROCESSING;
                menuNumber = 0;
                break;
            case (TurnState.ALMOSTDONE):
                battleMachineReference.GetComponent<BattleStateMachine>().AddTurnToList(currentMove);
                currentMove = new TurnHandler();
                if(isSomeCharacterSelected)
                {
                    Destroy(currentTargetReference);
                    isSomeCharacterSelected = false;
                }
                isThisMachineActive = false;
                battleMachineReference.SetCurrentHeroState(BattleStateMachine.HeroGUI.CHECKREMAINING);

                currentState = TurnState.DONE;
                break;
            case(TurnState.DONE):
                if(isThisMachineActive)
                {
                    currentState = TurnState.PROCESSING;
                }
                break;
        }

    }




    //===========================
    // ATTACK CIRCLE
    //===========================
    public void UpdateAttackCircle()
    {
        currentCooldown = currentCooldown + Time.deltaTime;
        calcCooldown = currentCooldown / maxCooldown;
        zAngleChange = Time.deltaTime * 360.0f / maxCooldown;
        if(calcCooldown >= 1 || minigameStillHappening == false)
        {
            minigameStillHappening = false;
            currentArrayLocation = 0;
            SecondHand.transform.Rotate(0.0f, 0.0f, -360.0f - (calcCooldown * -360.0f));

            //This needs to send the current state to a different place.
            currentState = TurnState.ACTION;

        }
        else
        {
            SecondHand.transform.Rotate(new Vector3(0.0f, 0.0f, -1f * zAngleChange));
            if(Input.GetKeyDown(KeyCode.Z))
            {
                tempLocationChecker = calcCooldown * -360.0f;
                location[currentArrayLocation] = tempLocationChecker;
                if((tempLocationChecker <= -68.0f && tempLocationChecker >= -91f) || (tempLocationChecker <= -132f && tempLocationChecker >= -160.8f ) || (tempLocationChecker <= -200f && tempLocationChecker >= -303.0f))
                {
                    numberOfHits++;
                    currentArrayLocation++;
                    Debug.Log("Hit!");
                    Debug.Log("Rotation:" + tempLocationChecker);
                    Debug.Log("Number of Hits: " + numberOfHits);
                }
                else
                {
                    minigameStillHappening = false;
                    Debug.Log("Miss!");
                    Debug.Log("Rotation: " + tempLocationChecker);
                    Debug.Log("Number of Hits: " + numberOfHits);
                }

            }
        }
    }
//==================================
// FIRST BATTLE MENU
//==================================
    private void UpdateMainBattleMenu()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            menuNumber--;
        }
        if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            menuNumber++;
        }
        if(menuNumber < 0)
        {
            menuNumber = 2;
        }
        if(menuNumber > 2)
        {
            menuNumber = 0;
        }
        switch(menuNumber)
        {
            case 0:
                menuOptions = MenuState.FIGHT;
                if(Input.GetKeyDown(KeyCode.Z))
                {
                    currentState = TurnState.WAITING;

                }
                break;
            case 1:
                menuOptions = MenuState.ITEM;
                if(Input.GetKeyDown(KeyCode.Z))
                {
                    currentState = TurnState.ITEM;
                    WindowManager.GetComponent<BattleWindowMaker>().doesTheItemSubMenuNeedAnUpdate = true;
                    WindowManager.GetComponent<BattleWindowMaker>().shouldTheItemMenuStillBeActive = true;
            
                }
                break;
            case 2:
                menuOptions = MenuState.RUN;
                if(Input.GetKeyDown(KeyCode.Z))
                {
                    currentState = TurnState.RUN;
                }
                break;
            default:
                break;
        }
        //implement the function below later
        MoveMainBattleMenuCursor(menuNumber);
    }
    
    private void MoveMainBattleMenuCursor(int optionNumber)
    {
        lineHeight1 = CalculateLineHeight(FightHighlight.GetComponent<Text>());
        switch(optionNumber)
        {
            case 0:
                Cursor.sizeDelta = new Vector2(refCamera.scaledPixelWidth * 0.17f, lineHeight1 * 1.1f);
                TextHighlightToggle(true, false, false);
                break;
            case 1:
                Cursor.sizeDelta = new Vector2(refCamera.scaledPixelWidth * 0.19f, lineHeight1 * 1.1f);
                TextHighlightToggle(false, true, false);
                break;
            case 2:
                Cursor.sizeDelta = new Vector2(refCamera.scaledPixelWidth * 0.16f, lineHeight1 * 1.1f);
                TextHighlightToggle(false,false, true);
               break;
            default:
                TextHighlightToggle(false,false,false);
                break;
        }
        Cursor.localPosition = new Vector3 (refCamera.scaledPixelWidth * -0.4f, refCamera.scaledPixelHeight * 0.24f,0f) - ((optionNumber%3) * new Vector3(0f, refCamera.scaledPixelHeight * 0.1f, 0f));
        //odds that this will break the second I move the window? 100%.
        //I'm shocked that it hasn't.
    }
	void TextHighlightToggle(bool fightOn, bool itemOn, bool runOn)
	{
		FightHighlight.SetActive(fightOn);
		ItemHighlight.SetActive(itemOn);
		RunHighlight.SetActive(runOn);
	}
    public GameObject MakeMainBattleMenu()
    {
        GameObject BattleMenu = new GameObject();
        BattleMenu.name = "BattleMenu";
	    BattleMenu.transform.parent = BattleManager.transform;
    	BattleMenu.AddComponent<Canvas>();
	    BattleMenu.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
	    BattleMenu.AddComponent<CanvasScaler>();
        BattleMenu.AddComponent<GraphicRaycaster>();

        GameObject Background = new GameObject();
        Background.transform.parent = BattleMenu.transform;
        Background.name = "MenuBackground";
        Image BackgroundImage = Background.AddComponent<Image>();
        BackgroundImage.preserveAspect = true;
        BackgroundImage.type = Image.Type.Filled;
        BackgroundImage.sprite = Resources.Load<Sprite>("Sprites/Menuborder4");
        RectTransform BackgroundRect = Background.GetComponent<RectTransform>();
        BackgroundRect.sizeDelta = new Vector2(refCamera.scaledPixelWidth * 0.3f, refCamera.scaledPixelWidth * 0.3f);
        BackgroundRect.localPosition = new Vector3(refCamera.scaledPixelWidth * -0.4f, refCamera.scaledPixelHeight * 0.12f,0f);

        for(int i = 0; i < 6; i++)
        {
            if(i == 3)
            {
                GameObject CursorImage = new GameObject();
                CursorImage.transform.parent = BattleMenu.transform;
                CursorImage.name = "MenuCursor";
	    	    Image cursorSprite = CursorImage.AddComponent<Image>();
                cursorSprite.preserveAspect = false;
                cursorSprite.type = Image.Type.Sliced;
                cursorSprite.sprite = Resources.Load<Sprite>("Sprites/menuborder");
                cursorSprite.color = Color.black;
                RectTransform cursorRect = CursorImage.GetComponent<RectTransform>();
                CursorImage.AddComponent<CursorAnimator>();
                Cursor = cursorRect;
            }
            GameObject textInfo = new GameObject();
            textInfo.transform.parent = BattleMenu.transform;
            Text writing = textInfo.AddComponent<Text>();

            if(i%3 == 0)
            {
                //writing.text = "Fight\n\n";
                writing.text = "Fight";
                writing.name = "fight";
            }
            else if(i%3 == 1)
            {
                //writing.text = "\nItem\n";
                writing.text = "Item";
                writing.name = "item";
            }
            else if(i%3 == 2)
            {
                //writing.text = "\n\nRun";
                writing.text = "Run";
                writing.name = "run";
            }    
            if(i/3 == 0)
            {
                writing.color = Color.black;
            }
            else if(i/3 == 1)
            {
                writing.color = new Color(0.8113208f, 0.4656844f, 0f, 1f);
                if(i%3 == 0)
                {
                    FightHighlight = textInfo;
                    writing.name = "FightHighlight";
                }
                else if(i%3 == 1)
                {
                    ItemHighlight = textInfo;
                    writing.name = "ItemHighlight";
                }
                else if(i%3 == 2)
                {
                    RunHighlight = textInfo;
                    writing.name = "RunHighlight";
                }
            }
            writing.font = Resources.Load<Font>("Fonts/Aganeﾌ 75 (Extra Bold)");
            //writing.resizeTextForBestFit = true;
            //writing.resizeTextMaxSize = 80;
            //writing.resizeTextMinSize = 12;
            writing.fontSize = Convert.ToInt32(Math.Floor(refCamera.scaledPixelHeight * 0.08f));
            writing.lineSpacing = 1.3f;
            writing.alignment = TextAnchor.UpperCenter;
            RectTransform TextRect = textInfo.GetComponent<RectTransform>();
            TextRect.sizeDelta = new Vector2(refCamera.scaledPixelWidth * 0.16f, refCamera.scaledPixelWidth * 0.16f);
            TextRect.localPosition = BackgroundRect.localPosition - ((i%3) * new Vector3(0f, refCamera.scaledPixelHeight * 0.1f, 0f));
        }


        return BattleMenu;
    }

    public void SetCharacterInMachine(CharacterStats character)
    {
        Hero1 = character;
        characterInMachine = character;
    }
    public void MakeThisMachineActive(bool state)
    {
        isThisMachineActive = state;
    }
//===============================
// ENEMY SELECTION MENU
//===============================
    private void UpdateTargetSelection()
    {
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if(currentTargetIndex-1 < 0)
            {
                currentTargetIndex = battleMachineReference.CharactersToManage.Count-1;
                doWeActuallyNeedToUpdateTheSelection = true;
            }
            else
            {
                currentTargetIndex--;
                doWeActuallyNeedToUpdateTheSelection = true;
            }
        }

        if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if(currentTargetIndex + 1 > battleMachineReference.CharactersToManage.Count-1)
            {
                currentTargetIndex = 0;
                doWeActuallyNeedToUpdateTheSelection = true;
            }
            else
            {
                currentTargetIndex++;
                doWeActuallyNeedToUpdateTheSelection = true;
            }
        }

        if(doWeActuallyNeedToUpdateTheSelection)
        {
           isSomeCharacterSelected = true;
           currentMove.TargetOfAttack = battleMachineReference.GetBattleParticipantAtIndex(currentTargetIndex).GetComponent<CharacterStateMachine>().characterInMachine;
           Destroy(currentTargetReference);

           currentTargetReference = DrawCurrentTarget(currentTargetIndex, currentMove.TargetOfAttack);
           doWeActuallyNeedToUpdateTheSelection = false;
        }
    }

    private GameObject DrawCurrentTarget(int currentTargetIndex, Characters target)
    {
            GameObject selectorSub1 = new GameObject();
		    selectorSub1.transform.parent = BattleManager.transform;
    		selectorSub1.AddComponent<Canvas>();
	    	selectorSub1.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
		    selectorSub1.AddComponent<CanvasScaler>();
            selectorSub1.AddComponent<GraphicRaycaster>();
        
        if(target.isEnemy)
        {   
            GameObject BackgroundImage = new GameObject();
            BackgroundImage.transform.parent = selectorSub1.transform;
            BackgroundImage.name = "SelectionBackground";
            Image BackgroundSprite = BackgroundImage.AddComponent<Image>();

            GameObject CursorImage = new GameObject();
            CursorImage.transform.parent = selectorSub1.transform;
            CursorImage.name = "SelectionCursor";
		    Image cursorSprite = CursorImage.AddComponent<Image>();
            cursorSprite.preserveAspect = true;
            cursorSprite.type = Image.Type.Simple;


		    GameObject targetSub1 = new GameObject();
            targetSub1.name = "target";
	    	targetSub1.transform.parent = selectorSub1.transform;
    		Image TargetSprite = targetSub1.AddComponent<Image>();
	    	TargetSprite.sprite = target.GetSprite();
    		TargetSprite.type = Image.Type.Simple;
	    	TargetSprite.preserveAspect = true;
    		RectTransform targetRect = targetSub1.GetComponent<RectTransform>();
            RectTransform cursorRect = CursorImage.GetComponent<RectTransform>();
            RectTransform bgRect = BackgroundImage.GetComponent<RectTransform>();
            
            targetRect.sizeDelta = new Vector2(refCamera.scaledPixelWidth * 0.15f, refCamera.scaledPixelWidth * 0.15f); 
            cursorRect.sizeDelta = new Vector2(refCamera.scaledPixelWidth * 0.08f, refCamera.scaledPixelWidth *0.08f);
            bgRect.sizeDelta = new Vector2(refCamera.scaledPixelWidth * 0.16f, refCamera.scaledPixelWidth * 0.25f);
            if(!target.isDead)
            {
                BackgroundSprite.sprite = Resources.Load<Sprite>("Sprites/menuborder5");
            }
            else
            {
                BackgroundSprite.sprite = Resources.Load<Sprite>("Sprites/menuborder6");
            }
            BackgroundSprite.type = Image.Type.Sliced;
            

            switch(target.positionInCombat)
            {
    			case 0:
	    		targetRect.localPosition = new Vector3(refCamera.scaledPixelWidth * 0f, refCamera.scaledPixelHeight * 0.2f, 0f);
                cursorSprite.sprite = Resources.Load<Sprite>("Sprites/enemycursor1");
		    	break;
			    case 1:
			    targetRect.localPosition = new Vector3(refCamera.scaledPixelWidth * -0.16f, refCamera.scaledPixelHeight * 0.2f, 0f);
                cursorSprite.sprite = Resources.Load<Sprite>("Sprites/enemycursor2");
	    		break;
    			case 2:
			    targetRect.localPosition = new Vector3(refCamera.scaledPixelWidth * 0.16f, refCamera.scaledPixelHeight * 0.2f, 0f);
                cursorSprite.sprite = Resources.Load<Sprite>("Sprites/enemycursor3");
			    break;
			    default:
			    targetRect.localPosition = new Vector3(refCamera.scaledPixelWidth * 0.32f, refCamera.scaledPixelHeight * 0.2f, 0f);
                cursorSprite.sprite = Resources.Load<Sprite>("Sprites/enemycursor4");
			    break;
            }
            cursorRect.localPosition = targetRect.localPosition + new Vector3(0f, refCamera.scaledPixelHeight * 0.16f, 0f);
            bgRect.localPosition = targetRect.localPosition + new Vector3(0f, refCamera.scaledPixelHeight * 0.04f);
        }
        else
        {
            GameObject CursorImage = new GameObject();
            CursorImage.transform.parent = selectorSub1.transform;
            CursorImage.name = "SelectionCursor";
		    Image cursorSprite = CursorImage.AddComponent<Image>();
            cursorSprite.preserveAspect = true;
            cursorSprite.type = Image.Type.Simple;
            RectTransform cursorRect = CursorImage.GetComponent<RectTransform>();
            cursorRect.sizeDelta = new Vector2(refCamera.scaledPixelWidth * 0.1f, refCamera.scaledPixelHeight * (0.3f/4.0f));
            float textHeight = CalculateLineHeight(WindowManager.GetComponent<BattleWindowMaker>().StatTextInfo);
            cursorSprite.sprite = Resources.Load<Sprite>("Sprites/menupoint");
            cursorRect.localPosition = new Vector3(refCamera.scaledPixelWidth * -0.45f, (refCamera.scaledPixelHeight * -0.19f) - (textHeight * target.positionInCombat * 1.0f), 0.0f);
        }

        return selectorSub1;   
    }
    	//remember to credit Unity user Spankenstein for this.
	private float CalculateLineHeight(Text text)
	{
		Vector2 extents = text.cachedTextGenerator.rectExtents.size * 0.5f;
    	float lineHeight = text.cachedTextGeneratorForLayout.GetPreferredHeight("A", text.GetGenerationSettings(extents));
		return lineHeight;
	}
}

