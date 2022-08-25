using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Town : MonoBehaviour
{
    public enum TownMainMenu
    {
        INN,
        ITEMSHOP,
        PARLIAMENT,
        BYPASS,
        DUNGEON,
        INACTIVE
    }
    public TownMainMenu currentTownMainMenuState;
    public enum TownStates
    {
        MainMenu,
        SubMenu1,
        SubMenu2,
        SubMenu3,
        SubMenu4
    }
    public TownStates currentTownState;
    public GameObject TownMenuReference;
    public GameObject SubMenuReference;
    public bool doesTheTownMainMenuNeedAnUpdate = false;
    public bool doesTheTownMainMenuEvenExist = false;
    public GameObject backgroundReference;
    public GameObject Manager;
    public Camera refcam;
    public float menuWidth = 0.4f;
    public float menuHeight = 0.6f;
    public int currentOption = 0;
    public int OptionMax = 5;
    public bool isThereATextBoxActive;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        Manager = GameObject.Find("GameManager");
        currentTownMainMenuState = TownMainMenu.INN;
        currentTownState = TownStates.MainMenu;
        refcam = Camera.main.GetComponent<Camera>();
        GameObject Menu = UIHassle();
        GameObject background1 = new GameObject();
        background1.transform.parent = Menu.transform;
        Image RefImage = background1.AddComponent<Image>();
        RefImage.sprite = Resources.Load<Sprite>("Sprites/bg1");
        background1.GetComponent<RectTransform>().sizeDelta = new Vector2(refcam.scaledPixelWidth, refcam.scaledPixelHeight);
        background1.GetComponent<RectTransform>().localPosition = new Vector3(0f,0f,0f);
        Menu.GetComponent<RectTransform>().localPosition = new Vector3(0f,0f,0f);
        backgroundReference = Menu;
        TownMenuReference = MakeTownMenu(Menu);
        MakeTownSign(Menu);
        doesTheTownMainMenuEvenExist = true;
    }



    // Update is called once per frame
    void Update()
    {
        switch(currentTownState)
        {
            case TownStates.MainMenu:
            if(doesTheTownMainMenuEvenExist)
            {
                MoveCursor();
                if(doesTheTownMainMenuNeedAnUpdate)
                {
                    Destroy(TownMenuReference);
                    TownMenuReference = MakeTownMenu(backgroundReference);
                    doesTheTownMainMenuNeedAnUpdate = false;
                }
            }
            else
            {
                currentOption = 0;
                currentTownMainMenuState = TownMainMenu.INN;
                TownMenuReference = MakeTownMenu(backgroundReference);
                doesTheTownMainMenuEvenExist = true;
                doesTheTownMainMenuNeedAnUpdate = false;
            }
            break;
            case TownStates.SubMenu1:
            break;
            case TownStates.SubMenu2:
            break;
            case TownStates.SubMenu3:
            break;
        }
        switch(currentTownMainMenuState)
        {
                case TownMainMenu.INN:
                break;
                case TownMainMenu.ITEMSHOP:
                break;
                case TownMainMenu.PARLIAMENT:
                break;
                case TownMainMenu.BYPASS:
                break;
                case TownMainMenu.DUNGEON:
                break;
                case TownMainMenu.INACTIVE:
                if(doesTheTownMainMenuEvenExist)
                {
                    Destroy(TownMenuReference);
                    doesTheTownMainMenuEvenExist = false;
                }
                break;
        }
        CheckInput();
    }

    private GameObject MakeTownMenu(GameObject parent)
    {
        GameObject Menu = UIHassle();
        GameObject background2 = new GameObject();
        background2.transform.parent = Menu.transform;
        Image RefImage2 = background2.AddComponent<Image>();
        RefImage2.sprite = Resources.Load<Sprite>("Sprites/menuborder");
        RefImage2.type = Image.Type.Sliced;
        background2.GetComponent<RectTransform>().sizeDelta = new Vector2(refcam.scaledPixelWidth * menuWidth, refcam.scaledPixelHeight * menuHeight);
        background2.GetComponent<RectTransform>().localPosition = new Vector3(refcam.scaledPixelWidth * -0.3f,0f,0f);
        for(int i = 0; i < 5; i++)
        {
            MakeMenuText(background2, i);
        }

        Menu.GetComponent<RectTransform>().localPosition = new Vector3(0f,0f,0f);

        return Menu;
    }

    private void MakeMenuText(GameObject parent, int i)
    {

        if(i == currentOption)
        {
            GameObject CursorObject = new GameObject();
            CursorObject.transform.parent = parent.transform;
            Image cursorImage = CursorObject.AddComponent<Image>();
            cursorImage.sprite = Resources.Load<Sprite>("Sprites/select");
            cursorImage.type = Image.Type.Sliced;
            CursorObject.GetComponent<RectTransform>().sizeDelta = new Vector2(refcam.scaledPixelWidth * menuWidth, refcam.scaledPixelHeight * (menuHeight / 6f));
            CursorObject.GetComponent<RectTransform>().localPosition = new Vector3(0f, refcam.scaledPixelHeight * (menuHeight /2.5f) - refcam.scaledPixelHeight * (menuHeight * (i%5) / 5f),0f);
            //make a selection highlight box and make the text color black
        }
        else
        {
            //don't do anything?
        }
        GameObject TextObject = new GameObject();
        TextObject.transform.parent = parent.transform;
        Text TextText = TextObject.AddComponent<Text>();
        TextText.fontSize = Convert.ToInt32(Math.Floor(refcam.scaledPixelHeight * 0.05f));
        TextText.font = Resources.Load<Font>("Fonts/OpenSans-Bold");
        TextText.alignment = TextAnchor.UpperLeft;
        switch(i)
        {
            case 0:
            TextText.text = "HOTEL";
            break;
            case 1:
            TextText.text = "SHOP";
            break;
            case 2:
            TextText.text = "PARLIAMENT";
            break;
            case 3:
            TextText.text = "OVERPASS";
            break;
            case 4:
            TextText.text = "DUNGEON";
            break;
        }
        if(i == currentOption)
        {
            TextText.color = Color.black;
        }
        else
        {
            TextText.color = Color.white;
        }

        TextObject.GetComponent<RectTransform>().sizeDelta = new Vector2(refcam.scaledPixelWidth * menuWidth, refcam.scaledPixelHeight * (menuHeight / 5f));
        TextObject.GetComponent<RectTransform>().localPosition = new Vector3(refcam.scaledPixelWidth * 0.01f, refcam.scaledPixelHeight * (menuHeight /2.8f) - refcam.scaledPixelHeight * (menuHeight * (i%5) / 5f),0f);
    }

    private GameObject MakeTownSign(GameObject parent)
    {
        GameObject CursorObject = new GameObject();
        CursorObject.transform.parent = parent.transform;
        Image cursorImage = CursorObject.AddComponent<Image>();
        cursorImage.sprite = Resources.Load<Sprite>("Sprites/menuborder");
        cursorImage.type = Image.Type.Sliced;
        GameObject TextObject = new GameObject();
        TextObject.transform.parent = CursorObject.transform;
        Text TextText = TextObject.AddComponent<Text>();
        TextText.fontSize = Convert.ToInt32(Math.Floor(refcam.scaledPixelHeight * 0.05f));
        TextText.font = Resources.Load<Font>("Fonts/OpenSans-Bold");
        TextText.alignment = TextAnchor.MiddleCenter;
        TextText.text = "TOWN";
        RectTransform rect = CursorObject.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(0f,refcam.scaledPixelHeight * 0.35f,0f);
        rect.sizeDelta = new Vector2(refcam.scaledPixelWidth * 0.15f, refcam.scaledPixelHeight * 0.1f);

        return CursorObject;
    }
    private GameObject UIHassle()
    {
        GameObject UIThing = new GameObject();
        UIThing.transform.parent = transform;
        Canvas refcanvas = UIThing.AddComponent<Canvas>();
        refcanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        UIThing.AddComponent<CanvasScaler>();
        UIThing.AddComponent<GraphicRaycaster>();

        return UIThing;
    }
    private void MoveCursor()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if(currentOption + 1 == OptionMax)
            {
                currentOption = 0;
            }
            else
            {
                currentOption++;
            }
            doesTheTownMainMenuNeedAnUpdate = true;
        }
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if(currentOption - 1 == -1)
            {
                currentOption = OptionMax-1;
            }
            else
            {
                currentOption--;
            }
            doesTheTownMainMenuNeedAnUpdate = true;
        }

    }
    private void CheckInput()
    {
        if(Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
        {
            if(currentTownState == TownStates.MainMenu)
            {
                switch(currentOption)
                {
                    //INN
                    case 0:
                        currentTownMainMenuState = TownMainMenu.INACTIVE;
                        currentTownState = TownStates.SubMenu1;
                    break;
                    //ITEM SHOP
                    case 1:
                        currentTownMainMenuState = TownMainMenu.INACTIVE;
                        currentTownState = TownStates.SubMenu2;
                    break;
                    //PARLIAMENT
                    case 2:
                        currentTownMainMenuState = TownMainMenu.INACTIVE;
                        currentTownState = TownStates.SubMenu3;
                    break;
                    //OVERPASS
                    case 3:
                        currentTownMainMenuState = TownMainMenu.INACTIVE;
                        currentTownState = TownStates.SubMenu4;
                    break;
                    //DUNGEON
                    case 4:
                    //change some states in the gamemanager.
                        Manager.GetComponent<GameStates>().setCurrentState(GameStates.GameState.ENTERTHEDUNGEON);
                        //SceneManager.LoadScene("test2");
                    break;
                }
            }
            if(currentTownState == TownStates.SubMenu1)
            {
                Debug.Log("It will cost 10 bucks to get some sleep.");
                Manager.GetComponent<GameStates>().Inn(10);
                currentTownState = TownStates.MainMenu;
            }
        }
        if(Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape))
        {
            if(currentTownState != TownStates.MainMenu)
            {
                currentTownState = TownStates.MainMenu;
            }
        }
    }
}
