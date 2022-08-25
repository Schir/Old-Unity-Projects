using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WorldItemInformation : MonoBehaviour
{
    public Dialogue dialogBoxInfo;

    public bool isMenuActive = false;
    public bool shouldMenuBeActive = false;
    public Sprite backgroundGreen;
    public GameObject menuReference;
    public bool doesThisHaveAPortrait = false;
    Camera refcam;
    public bool doesThisOneHaveAnItem;
    public bool hasThisOneBeenTriggered = false;
    public int area;
    public int id;
    public Item ChestItem;
    public bool isThisTheExit;
    public int currentPositionInDialogue = 0;
    public Sprite currentTalkingHead;
    public string currentText;
    public bool doesTheMenuNeedAnUpdate;
    public CameraMover dungeoncamReference;
    public bool wasThisCalledFromTheDungeon = false;
    public bool isThisOneActive = false;
    public bool wasThisCalledFromTheManager = false;
    public BattleStateMachine battleMachineReference;
    public bool wasThisCalledFromBattle = false;
    public void Awake()
    {
        refcam = Camera.main;
        backgroundGreen = Resources.Load<Sprite>("Sprites/menuborder");
        dialogBoxInfo = Resources.Load<Dialogue>("Dialogue/Dialogue"+ area.ToString() + "-" + id.ToString());
    }
    public void setDialogBoxInfo(int areaNumber, int idNumber)
    {
        dialogBoxInfo = Resources.Load<Dialogue>("Dialogue/Dialogue"+ areaNumber.ToString() + "-" + idNumber.ToString());
    }
    public void CallMakeWindow()
    {
        shouldMenuBeActive = true;
        isThisOneActive = true;
        currentPositionInDialogue = 0;
    }
    public void CallMakeWindow(CameraMover thing)
    {
        dungeoncamReference = thing;
        wasThisCalledFromTheDungeon = true;
        shouldMenuBeActive = true;
        //currentPositionInDialogue = 0;
        if(dialogBoxInfo.DialogItems.Count > 0 && !hasThisOneBeenTriggered)
        {
            ProcessInformation(dialogBoxInfo, currentPositionInDialogue);
            menuReference = MakeWindow(currentPositionInDialogue);
        }
        else if(hasThisOneBeenTriggered)
        {
            menuReference.SetActive(true);
        }
        isMenuActive = true;
        float respect = 0f;
        while(respect < 0.1f)
        {
            respect += Time.deltaTime;
        }
        
        isThisOneActive = true;
    }
    public void CallMakeWindow(GameStates manager)
    {
        //dungeoncamReference = thing;
        wasThisCalledFromTheManager = true;
        shouldMenuBeActive = true;
        //currentPositionInDialogue = 0;
        if(dialogBoxInfo.DialogItems.Count > 0 )
        {
            ProcessInformation(dialogBoxInfo, currentPositionInDialogue);
            menuReference = MakeWindow(currentPositionInDialogue);
        }
        else if(hasThisOneBeenTriggered)
        {
            menuReference.SetActive(true);
        }
        isMenuActive = true;
        float respect = 0f;
        while(respect < 0.1f)
        {
            respect += Time.deltaTime;
        }
        
        isThisOneActive = true;
    }
    public void CallMakeWindow(Dialogue dlog, BattleStateMachine bsm)
    {
        dialogBoxInfo = dlog;
        battleMachineReference = bsm;
        wasThisCalledFromBattle = true;
        shouldMenuBeActive = true;
        currentPositionInDialogue = 0;
        if(dialogBoxInfo.DialogItems.Count > 0)
        {
            ProcessInformation(dialogBoxInfo, currentPositionInDialogue);
            menuReference = MakeWindow(currentPositionInDialogue);
        }
        else if(hasThisOneBeenTriggered)
        {
            menuReference.SetActive(true);
        }
        isMenuActive = true;
        
        float respect = 0f;
        while(respect < 0.1f)
        {
            respect += Time.deltaTime;
        }
        
        isThisOneActive = true;
    }
    public GameObject MakeWindow(int position)
    {
        GameObject SubMenu;
		GameObject SubBackground;
		Image SubBG2;
		Canvas SubMenuCanvas;
		RectTransform rectTransform;

		SubMenu = new GameObject();
        SubMenu.name = "SubMenu";
		SubMenu.transform.parent = transform;
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
		SubBG2.sprite = backgroundGreen;
		SubBG2.type = Image.Type.Sliced;

		rectTransform = SubBackground.GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(refcam.scaledPixelWidth * 0.8f, refcam.scaledPixelHeight * 0.3f);
		rectTransform.localPosition = new Vector3(0.0f, refcam.scaledPixelHeight * -0.3f, 0.0f);

        // Text
        GameObject ItemWindowText = new GameObject();
        ItemWindowText.transform.parent = SubBackground.transform;
        ItemWindowText.name = "text";

	    Text ItemText = ItemWindowText.AddComponent<Text>();
	    ItemText.font = (Font)Resources.Load("Fonts/Roboto-Medium");
	    ItemText.text = currentText;
        ItemText.fontSize = Convert.ToInt32(Math.Floor(refcam.scaledPixelHeight * 0.05f));
        RectTransform tempRectTransform = ItemWindowText.GetComponent<RectTransform>();
        if(doesThisHaveAPortrait)
        {
            tempRectTransform.sizeDelta = new Vector2(refcam.scaledPixelWidth * 0.6f, refcam.scaledPixelHeight * 0.3f);
            tempRectTransform.localPosition = new Vector3(refcam.scaledPixelWidth * 0.1f, 0f, 0f);
            GameObject WindowImager = new GameObject();
            WindowImager.transform.parent = SubBackground.transform;
            Image WindowImage = WindowImager.AddComponent<Image>();
            WindowImage.sprite = currentTalkingHead;
            WindowImage.preserveAspect = true;
            RectTransform rect = WindowImager.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(refcam.scaledPixelWidth * 0.18f, refcam.scaledPixelHeight * 0.29f);
            rect.localPosition = new Vector3(refcam.scaledPixelWidth * -0.3f, 0f, 0f);
        }
        else
        {
            tempRectTransform.sizeDelta = new Vector2(refcam.scaledPixelWidth * 0.79f, refcam.scaledPixelHeight * 0.3f);
            tempRectTransform.localPosition = new Vector3(0.01f,0f,0f);
        }
        ContentSizeFitter fit = ItemWindowText.AddComponent<ContentSizeFitter>();
        return SubMenu;
    }
    void Update()
    {
        if(isThisOneActive)
        {
            CheckInput();
            if(shouldMenuBeActive && !isMenuActive)
            {
                menuReference = MakeWindow(currentPositionInDialogue);
                isMenuActive = true;
            }
            if(!shouldMenuBeActive && isMenuActive)
            {
                menuReference.SetActive(false);
                //Destroy(menuReference);
                isMenuActive = false;
                if(wasThisCalledFromTheDungeon)
                {
                    dungeoncamReference.windowActive = false;
                }
                else if(wasThisCalledFromBattle)
                {
                    battleMachineReference.isAWindowActive = false;
                }
            }
            if(doesTheMenuNeedAnUpdate)
            {
                Destroy(menuReference);
                menuReference = MakeWindow(currentPositionInDialogue);
                doesTheMenuNeedAnUpdate = false;
            }
        }
    }

    private void CheckInput()
    {
        if(Input.GetButtonDown("Submit"))
        {
            if(currentPositionInDialogue + 1 == dialogBoxInfo.DialogItems.Count)
            {
                hasThisOneBeenTriggered = true;
                menuReference.SetActive(false);
                shouldMenuBeActive = false;
                isMenuActive = false;
                isThisOneActive = false;
                doesTheMenuNeedAnUpdate = false;
                if(wasThisCalledFromTheDungeon)
                {
                    dungeoncamReference.windowActive = false;
                }
                else if(wasThisCalledFromBattle)
                {
                    battleMachineReference.isAWindowActive = false;
                }

            }
            else
            {
                currentPositionInDialogue++;
                ProcessInformation(dialogBoxInfo,currentPositionInDialogue);
                doesTheMenuNeedAnUpdate = true;
            }
        }
    }
    public void ProcessInformation(Dialogue element, int position)
    {
        DialogueElement dialogueRef;
        if(element.DialogItems.Count > 0)
        {
            dialogueRef = element.DialogItems[position];
            if(dialogueRef.Character == DialogueElement.Characters.none)
            {
                doesThisHaveAPortrait = false;
            }
            else
            {
                doesThisHaveAPortrait = true;
            }
            switch(dialogueRef.Character)
            {
                case DialogueElement.Characters.none:
                break;
                case DialogueElement.Characters.Claire:
                currentTalkingHead = Resources.Load<Sprite>("Sprites/reporter");
                break;
                case DialogueElement.Characters.John:
                currentTalkingHead = Resources.Load<Sprite>("Sprites/busy");
                break;
                case DialogueElement.Characters.November:
                currentTalkingHead = Resources.Load<Sprite>("Sprites/robo");
                break;
                case DialogueElement.Characters.NPC:
                break;
            }
            currentText = dialogueRef.DialogueText;

        }
        else
        {
            shouldMenuBeActive = false;
        }
    }
}
