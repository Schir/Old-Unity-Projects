using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//makes the system menu
public class MainMenu : MonoBehaviour
{
    public enum MainSubMenu
    {
        STATUS,
        ITEM,
        EQUIP,
        MAGIC,
        SAVE,
        MISC
    }
    public MainSubMenu currentMenuState;
    public enum BroaderMenuState
    {
        Main,
        Sub,
        Deactivated
    }
    public BroaderMenuState broaderState;
    public bool isThisStillOn = true;
    public List<string> MenuNames = new List<string>{"STATUS", "ITEM", "EQUIP", "MAGIC", "SAVE", "MISC"};
    public List<string> menuInfo = new List<string>{"Check out a character's status.", "Go and use an item.", "Make sure you're in a shape to stay safe.", "Use some weird magic.", "Save your game.", "Everything else goes here."};
    public int currentMenuIndex = 0;
    public static Dictionary<int, MainMenu.MainSubMenu> subMenuDictionary = new Dictionary<int, MainMenu.MainSubMenu>
    {
        {0, MainSubMenu.STATUS},
        {1, MainSubMenu.ITEM},
        {2, MainSubMenu.EQUIP},
        {3, MainSubMenu.MAGIC},
        {4, MainSubMenu.SAVE},
        {5, MainSubMenu.MISC} 
    };



    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake()
    {
        broaderState = BroaderMenuState.Main;
        currentMenuState = MainSubMenu.STATUS;
        isThisStillOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        switch(broaderState)
        {
            case BroaderMenuState.Main:
                CheckMainMenuInput();
                if(Input.GetButtonDown("Cancel"))
                {
                    broaderState = BroaderMenuState.Deactivated;
                }
            break;
            case BroaderMenuState.Sub:
                //check something;
                break;
            case BroaderMenuState.Deactivated:
                isThisStillOn = false;
                break;
        }

    }


    public bool MainMenuEdgeChecker(int index)
    {
        if(index >= MenuNames.Count)
        {
            return false;
        }
        if(index < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public void CheckMainMenuInput()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(MainMenuEdgeChecker(currentMenuIndex +1))
            {
                currentMenuIndex++;
            }
            else
            {
                currentMenuIndex = 0;
            }
        }
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            //do something.
        }
    }
}
