using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public uint x = 0b_0001;
    List<Vector3> positions;
    Transform cursorPos;
    public Player playerRef;
    public Backpack reference;
    public GameObject currentObject;
    public GameObject textHolder1;
    public GameObject textHolder2;
    public GameObject ItemDescriptionHolder;
    public TextMesh ItemDescriptionRef;
    List<Vector3> rotations;
    List<Vector3> scales;
    float text2ref = 0.045f;
    Vector3 text2Offset;
    public int currentPos = 0;
    public int offsetItemMenuTop = 0; //This one is for the item menu.
    List<GameObject> bagText;
    public Font f;
    public GameObject EquipMenuHolder;
    public GameObject EquipMenu2;
    public uint equipMenuState = 0b_10000;
    List<GameObject> equipText;
    public bool needsUpdate = false;
    public float equipOffset1 = 0.04f;
    public TextMesh EquipDesc;
    public TextMesh EquipType;
    public TextMesh EquipWeight;
    List<GameObject> bagContents;
    List<Item> equipMenuItem;
    public int equipMenuOffset; // this one is for the equip menu.
    public int CurrentEquipNumber;
    public TextMesh BodyEquipText;
    public TextMesh BagEquipText;
    public TextMesh TankEquipText;
    public TextMesh FinsEquipText;
    public enum State
    {
        MenuTop,
        Item,
        Equip,
        Save,
        Load
    }
    public enum EquipState
    {

        EquipTop,
        ChangeBetweenTopAndSelecting,
        EquipSelecting,
        ChangeBetweenSelectingAndTop,
        EquipNotActive
    }
    public State currentState = State.MenuTop;
    public EquipState currentEquipState = EquipState.EquipNotActive;
    // Start is called before the first frame update
    void Awake()
    {
        //oh christ I probably need to comment this, huh.
        //okay so uh
        //bagText is a list that holds the stuff in the item menu. it's mainly here so that items can be more easily deleted when refreshing the screen
        bagText = new List<GameObject>();
        equipText = new List<GameObject>();
        bagContents = new List<GameObject>();
        //scales is a list of vectors that the cursor uses to change size 
        scales = new List<Vector3>{new Vector3(0.2f,0.2f,0f), new Vector3(0.1f, 0.1f, 0f)};
        //rotations is the same as scales, but for rotations
        rotations = new List<Vector3>{new Vector3(0f,0f, -90f), new Vector3(0f,0f, 90f)};
        //reference should load in the current state of the player's inventory. this is debug for now
        playerRef = new Player();
        reference = playerRef.inventory;
        //positions is a list of vectors to put the cursor in on the main menu
        positions = new List<Vector3>{new Vector3(0.1f, 0.11f, 0f), new Vector3(0.1f, 0.04f, 0f), 
                                      new Vector3(0.1f, -0.03f, 0f), new Vector3(0.1f, -0.1f, 0f)};
        //finding the cursor
        cursorPos = GameObject.Find("MenuCursor1").transform;
        //rotating and scaling it to be correct
        cursorPos.localRotation = Quaternion.Euler(rotations[0].x, rotations[0].y, rotations[0].z);
        cursorPos.localScale = scales[0];
        //setting this current object reference to the main menu's object
        currentObject = textHolder1;
        //this is here to deal with me being an idiot and moving the cursor all over the place while mocking things up
        //unparent it from whatever I've set it to and then parent it to the current object
        cursorPos.parent = null;
        cursorPos.parent = transform;
        //why is this here
        text2Offset = new Vector3(-0.045f, 0.07f, -0.1f);
        //deactivate unnecessary objects that need to be active on Awake() to find them, but that shouldn't be seen otherwise
        textHolder2.SetActive(false);
        ItemDescriptionHolder.SetActive(false);
        EquipMenuHolder.SetActive(false);
        //resetting the equipment's text. it works.
        RedrawEquipment();
    }

    // Update is called once per frame
    void Update()
    {
        //damn dog is this nintendo because that's a lot of switches
        switch(currentState)
        {
            //we're pretty much just switching between a handful of the states that the menu can be in.
            case State.MenuTop:
                //doing a couple of checks for input. using input.getkeyup for now, need to change this to an axis or a button at some point
                if(Input.GetKeyUp(KeyCode.UpArrow))
                {
                    //I don't want to write another enum for this, so I'm just using bitshifting for reasons that become clearer later
                    uint y = x >> 1;
                    if(y != 0b_0000)
                    {
                        x = y;
                    }
                }
                if(Input.GetKeyUp(KeyCode.DownArrow))
                {
                    uint y = x << 1;
                    if (y <= 0b_1000)
                    {
                        x = y;
                    }
                }
                //mainly this reason:
                switch(x)
                {
                    //it's easier to see (roughly) where the cursor should be. This switch is just moving the cursor's transform.
                    case 0b_0001:
                        cursorPos.localPosition = positions[0];
                        break;
                    case 0b_0010:
                        cursorPos.localPosition = positions[1];
                        break;
                    case 0b_0100:
                        cursorPos.localPosition = positions[2];
                        break;
                    case 0b_1000:
                        cursorPos.localPosition = positions[3];
                        break;
                }
                //if the user presses the confirm button, change the state.
                if(Input.GetKeyUp(KeyCode.Z))
                {
                    switch(x)
                    {
                    //writing a function to set the state to make this slightly less cluttered, and they're all doing pretty much the same thing.
                        case 0b_0001:
                        setTheCurrentState(State.Item);
                        break;
                    case 0b_0010:
                        setTheCurrentState(State.Equip);
                        break;
                    case 0b_0100:
                        setTheCurrentState(State.Save);
                        break;
                    case 0b_1000:
                        setTheCurrentState(State.Load);
                        break;
                    }
                }
                //and this is the end of the top menu
                break;

            //alright this one's a bit of a mess.
            case State.Item:
            //so there's an item menu, yeah?
            //well we've gotta do a bunch of edge checking.
                if(Input.GetKeyUp(KeyCode.UpArrow))
                {
                    //okay so if there's less than 20 items in the bag then we can just get lazy
                    if(reference.contents.Count < 20)
                    {
                        if(currentPos - 2 >= 0)
                        {
                                currentPos -= 2;
                        }
                    }
                    //otherwise we have to actually do work.
                    else 
                    {
                        //in the case of this one, where we're going up through the menu, we've gotta first see if we're not at the top of the menu
                        //because if we aren't then we don't have to think too hard about it.
                        if(currentPos + offsetItemMenuTop - 2 >= offsetItemMenuTop)
                        {
                            currentPos -=2;
                        }
                        //otherwise we actually have to change the offset and redraw things.
                        else if(currentPos + offsetItemMenuTop - 2 < offsetItemMenuTop && offsetItemMenuTop > 0)
                        {
                            offsetItemMenuTop -= 2;
                            //this function remakes the list from scratch. There's probably smarter ways of doing this but I'm dumb and this makes sense to me and also seems to work.
                            RemakeList();
                        }
                    }
                }
                //this if statement's pretty much just doing the same thing as the one above, but for going down through the menu.
                if(Input.GetKeyUp(KeyCode.DownArrow))
                {
                    if(reference.contents.Count <= 20)
                    {
                        if(currentPos + 2 < 20)
                        {
                            currentPos += 2;
                        }
                    }
                    else
                    {
                        //I'm not sure if I need this "check if the list's odd or even" int here, but I've had hell with weird offsets at the edges in the past. 
                        //basically I just don't want the thing to throw an IndexOutOfBoundsException and this is so that you can press down on the second-to-last item 
                        //it won't act weird.
                        int l = reference.contents.Count % 2;
                        if(currentPos + 2 < 20)
                        {
                            currentPos += 2;
                        }
                        //if we're at the bottom of the screen, there's no reason to move the cursor. just update the offset and redraw the list.
                        //also yeah it seems like the %2 variable does help to make sure the offset behavior works right. 
                        //if the list is even, then it adds zero. if the list is odd, it adds one.
                        //
                        else if(currentPos + 2 >= 20 && (currentPos + offsetItemMenuTop + 2 < reference.contents.Count + l))
                        {
                            offsetItemMenuTop += 2;
                            RemakeList();
                        }
                    }
                }
                //not bothering with edge checking on moving left/right through the list. it's weird but writing the earlier ones was enough of a pain. this is fine.
                if(Input.GetKeyUp(KeyCode.RightArrow))
                {
                    if(currentPos + 1 < 20)
                    {
                        currentPos += 1;
                    }
                }
                if(Input.GetKeyUp(KeyCode.LeftArrow))
                {
                    if(currentPos - 1 >= 0)
                    {
                        currentPos -= 1;
                    }
                }
                //then we update the menu.
                UpdateItemMenu();
                //lastly we check for the cancel button.
                if(Input.GetKeyUp(KeyCode.X))
                {
                    //if we do, destroy all the items on-screen. working backwards through the bag because it makes sense to me.
                    if(bagText.Count > 0)
                    {
                        for(int i = bagText.Count-1; i >= 0; i--)
                        {
                            Destroy(bagText[i]);
                        }
                    }
                    //and then hide the gameobject before returning the state to the top menu
                    ItemDescriptionHolder.SetActive(false);
                    setTheCurrentState(State.MenuTop);
                }
            break;

            case State.Equip:
            //realized that what I was doing in the item part of the switch was silly and makes it hard to remember that
            //you're looking at a thing in Update(), so I'm putting all the stuff that the equip menu's doing into a function.
            ManageEquipMenu();
            break;

            case State.Save:
            break;

            case State.Load:
            break;
        }
    }



    void LateUpdate()
    {
        
    }
    //chuck a bunch of things into the bag so that we can debug things more easily
    public void DebugAddItem()
    {
        reference.AddItemToBag(new PackOfCigarettes());
        reference.AddItemToBag(new Trophy());
        reference.AddItemToBag(new Painting());
        reference.AddItemToBag(new Fins());
        reference.AddItemToBag(new Wetsuit());
        reference.AddItemToBag(new Wetsuit2());
        reference.AddItemToBag(new Wetsuit3());
        reference.AddItemToBag(new Bag());
        reference.AddItemToBag(new Key1());
        reference.AddItemToBag(new Vase());
        reference.AddItemToBag(new Ashtray());
        reference.AddItemToBag(new Tank());
    }
    void RemakeList()
    {
        //remaking the list by deleting all the items on-screen and then redrawing it
        for(int i = bagText.Count-1; i >= 0; i--)
            {
                Destroy(bagText[i]);
            }
        MakeTheItemList();
    }
    //this is where we do the necessary transitions between states 
    void setTheCurrentState(State s)
    {
        
        switch(s)
        {
            case State.MenuTop:
            //so the main menu can be coming from pretty much any of the other states.
            //we're keeping a dummy gameObject to hold whichever one that is, and deactivating it.
            currentObject.SetActive(false);
            //then, setting it to be whichever gameObject we need it to be. here, it's TextHolder1.
            currentObject = textHolder1;
            //and setting that to be true.
            currentObject.SetActive(true);
            //and then parenting the cursor to it to make the transforms easier
            cursorPos.parent = null;
            cursorPos.parent = transform;
            cursorPos.localScale = scales[0];
            cursorPos.localRotation = Quaternion.Euler(0f, 0f, rotations[0].z);
            currentEquipState = EquipState.EquipNotActive;
            break;

            case State.Item:
            //pretty much the same as above.
            currentObject.SetActive(false);
            currentObject = textHolder2;
            currentObject.SetActive(true);
            cursorPos.parent = null;
            cursorPos.parent = currentObject.transform;
            cursorPos.localScale = scales[1];
            cursorPos.localRotation = Quaternion.Euler(0f, 0f, rotations[1].z);
            ItemDescriptionHolder.SetActive(true);
            MakeTheItemList();
            break;

            case State.Equip:
            currentObject.SetActive(false);
            currentObject = EquipMenuHolder;
            currentObject.SetActive(true);
            cursorPos.parent = null;
            cursorPos.parent = currentObject.transform;
            cursorPos.localScale = scales[1];
            cursorPos.localRotation = Quaternion.Euler(0f, 0f, rotations[1].z);
            needsUpdate = true;
            currentEquipState = EquipState.EquipTop;
            break;

            case State.Save:
            break;

            case State.Load:
            break;
        }
        currentState = s;
    }
    public void UpdateItemMenu()
    {
        //this is a dummy variable to get half of the current position. mainly this is because the menu's split into two
        int i = currentPos / 2;
        //if it's even, put it on the left side of the screen.
        if(currentPos%2 == 0)
        {
            //we're using magic numbers of vec2(-0.21f, 0.2f) because that's what looked good in the editor. 
            //text2ref is a float that looked like it did a reasonably good job of being the vertical difference between each of the elements of text. 
            cursorPos.localPosition = new Vector3(-0.21f, 0.2f - (i * text2ref), 0f); 
        }
        else
        {
            cursorPos.localPosition = new Vector3(0.02f, 0.2f - (i * text2ref), 0f); 
        }
        //this is the most wasteful for loop lol. i could cut the runtime by half if i just started it at the offset.
        for(int j = 0; j < bagText.Count; j++)
        {
            if(j == currentPos)
            {
                bagText[j].GetComponent<TextMesh>().color = Color.yellow;
            }
            else
            {
                bagText[j].GetComponent<TextMesh>().color = Color.white;
            }
        }
        //this one's updating the description text
        if(reference.contents.Count != 0)
        {
            //k's also a dummy variable. it's standing in for the actual position the cursor should be at.
            int k = currentPos + offsetItemMenuTop;
            //so if k's a number that's less than the amount of things in the bag (and k can't be less than zero because of all that edge checking we did earlier),
            //then k is a number that is within the indexes of the list
            if(k < reference.contents.Count)
            {
                //and if there's not an empty item at that point in the bag (there shouldn't be, this if statement is unnecessary)
                if(reference.contents[k] != null)
                {
                    //then I should've remembered to write a description for the item, and because I definitely did that,
                    //then we set the text in the TextMesh at the bottom of the screen to the item's description. 
                    ItemDescriptionRef.text = reference.contents[k].ItemDescription;
                }
            }
            //and if any of those things aren't true then there's no need to access the array because there's no item there. 
            else
            {
                //so we set the description to an empty string.
                ItemDescriptionRef.text = "";
            }
        }
        //and if there's no items in the bag then there's no description that we need to render.
        else
        {
            ItemDescriptionRef.text = "";
        }
    }
    void MakeTheItemList()
    {
        //oh god this one.
        //okay so we start by making a list to put things in
        List<GameObject> bagg = new List<GameObject>();
        if(reference.contents.Count > 0)
        {
            //if the inventory's nonzero, we make another list to hold things in while we work
            //wait why is this here
            bagText = new List<GameObject>();
            //and set an int to figure out our offsets
            int figureOutHowManyToDraw = 0;
            //if there's more than twenty things between the offset and the end of the list, we cap it to 20.
            if(reference.contents.Count - offsetItemMenuTop > 20)
            {
                figureOutHowManyToDraw = 20;
            }
            else
            {
                //otherwise we go to the end of the list.
                figureOutHowManyToDraw = reference.contents.Count - offsetItemMenuTop;
            }
            //now to make sure we're doing things right, we start at the offset, and then go until the offset plus the amount we need to draw.
            for(int j = offsetItemMenuTop; j< offsetItemMenuTop + figureOutHowManyToDraw;j++)
            {
                //and this is where things get tedious.
                //so we start by making a gameObject. I've named it text for reasons that are entirely unclear to me.
                GameObject thing = new GameObject("text");
                //I've also put it on the UI layer, to be safe.
                thing.layer = 5;
                //now I add a TextMesh to it because lmao fuck Unity's Canvas system with a rusty pike.
                //how do you guarantee that the UI looks the same at different aspect ratios? 
                //You don't!!!! 
                //(at least not without a lot of Camera.scaledPixelHeight and Camera.scaledPixelWidth calls and jesus fuck I'm not doing that again)
                //so we're going to put all this in 3D space, which has the knock-on effect of making every single vector we're ever going to deal with a million times simpler.  
                TextMesh t = thing.AddComponent<TextMesh>();
                //now when you add a TextMesh component Unity also adds a MeshRenderer to it, so we're referencing that.
                MeshRenderer meshh = thing.GetComponent<MeshRenderer>();
                //The display as I've set it up looks a bit fucked if items are more than ten characters long, so we're trimming them like it's a SNES game.
                if(reference.contents[j].ItemName.Length > 10)
                {
                    t.text = reference.contents[j].ItemName.Substring(0, 10);
                }
                else
                {
                    t.text = reference.contents[j].ItemName;
                }
                //child it to the item window
                thing.transform.parent = currentObject.transform;
                //arbitrary font size to make it look cleaner. this looked fine in the editor.
                t.fontSize = 103;
                //oh god, this.
                //okay so every kind of 2D asset in Unity is literally hell to work with.
                //you want to change the font on your text mesh to not be Arial, and you want to do that in code?
                //well, guess it's time to suffer because it's not enough to just change the font.  
                t.font = Resources.Load<Font>("Fonts/SourceSansPro-Bold");
                //no, you've gotta load the font as a material and attach that material to the MeshRenderer.
                //this is only mentioned, once, off-hand, as a heads-up, in the documentation for Fonts. not in the scripting reference, not on the TextMesh page, on the Font page.
                //like "also you've gotta change the main texture if you change the font if you're using a TextMesh"
                //figuring this out took two hours and I am Mad Online about it.
                meshh.material = Resources.Load<Material>("Fonts/SourceSansPro-bold");
                //anyway, this one's making the text not be a million pixels big. 
                t.characterSize = 0.003f;
                //and setting anchors and alignments to what seemed to work in the editor.
                t.anchor = TextAnchor.MiddleLeft;
                t.alignment = TextAlignment.Left;
                //set the transform where it should be.
                int l = (j-offsetItemMenuTop) / 2;
                if(j%2 == 0)
                {
                    thing.transform.localPosition = new Vector3(-0.165f, 0.2f - (l * text2ref), 0f);
                }
                else
                {
                    thing.transform.localPosition = new Vector3(0.03f, 0.2f - (l * text2ref), 0f);
                }
                //and set the local rotation to align with what it should be
                thing.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                //and then add it to the bag
                bagg.Add(thing);
            }
            //anyway we set the list<GameObject> that we declared up top to be the list we just made here. 
            bagText = bagg;
        }
    }

    //alright so let's go over the equipment menu manager. christ, what a pain, there's more to this than I like.
    private void ManageEquipMenu()
    {
        //alright, so, what seemed to make sense to me is to give the equipment menu its own state machine. There's five states, one for being in the left menu, one for selecting what to equip,
        //two for transitioning between those two, and one for the menu being inactive. 
        switch(currentEquipState)
        {
        case EquipState.EquipTop:
            //so first we check for various inputs and move between five options in the same manner as the main menu.
            if(Input.GetKeyUp(KeyCode.DownArrow))
            {
                uint temp = equipMenuState >> 1;
                if(temp != 0b_00000)
                {
                    equipMenuState = temp;
                    //and set a switch to true
                    needsUpdate = true;
                }
            }
            if(Input.GetKeyUp(KeyCode.UpArrow))
            {
                uint temp = equipMenuState << 1;
                if(temp <= 0b_10000)
                {
                    equipMenuState = temp;
                    needsUpdate = true;
                }
            }
            //cancel and confirm
            if(Input.GetKeyUp(KeyCode.X))
            {
                currentEquipState = EquipState.EquipNotActive;
                setTheCurrentState(State.MenuTop);
            }
            if(Input.GetKeyUp(KeyCode.Z))
            {
                currentEquipState = EquipState.ChangeBetweenTopAndSelecting;
            }
            ChangeListOfEquipment();
            break;
            
            //first transition
            case EquipState.ChangeBetweenTopAndSelecting:
                //reset the equipment menu
                CurrentEquipNumber =0;
                equipMenuOffset = 0;
                //move the cursor
                cursorPos.parent = null;
                cursorPos.parent = EquipMenu2.transform;
                currentEquipState = EquipState.EquipSelecting;
                //update
                needsUpdate = true;
            break;

            case EquipState.EquipSelecting:
                //cancel
                if(Input.GetKeyUp(KeyCode.X))
                {
                    currentEquipState = EquipState.ChangeBetweenSelectingAndTop;
                }
                //input and offset checking
                if(Input.GetKeyUp(KeyCode.UpArrow))
                {
                    if(CurrentEquipNumber - 1 >= 0)
                    {
                        CurrentEquipNumber -= 1;
                    }
                    else if((CurrentEquipNumber + equipMenuOffset -1 < equipMenuOffset) && (equipMenuOffset > 0))
                        {
                            equipMenuOffset -=1; 
                        }
                    needsUpdate = true;
                }
                //eight seems like a good number. the menu draws outside of the box otherwise.
                if(Input.GetKeyUp(KeyCode.DownArrow))
                {
                    if(CurrentEquipNumber + 1 < 8)
                    {
                        CurrentEquipNumber++;
                    }
                    else if((CurrentEquipNumber + 1 == 8) && (CurrentEquipNumber + equipMenuOffset + 1 < equipMenuItem.Count))
                    {
                        equipMenuOffset++;
                    }
                    needsUpdate = true;
                }
                //select equipment
                if(Input.GetKeyUp(KeyCode.Z))
                {
                    //if it's within bounds,
                    if(equipMenuOffset + CurrentEquipNumber < equipMenuItem.Count)
                    {
                        //and if the item isn't null (it won't be but i like being cautious here)
                        if(equipMenuItem[equipMenuOffset + CurrentEquipNumber] != null)
                        {
                            //change the player's equipment
                            playerRef.ChangeEquipment(equipMenuItem[equipMenuOffset+CurrentEquipNumber]);
                            //refresh the text 
                            RedrawEquipment();
                            //exit the state
                            currentEquipState = EquipState.ChangeBetweenSelectingAndTop;
                        }
                    }
                }
                //commented out an if(needsupdate). not sure if I need it here or not.
                //if(needsUpdate)
                //{
                    cursorPos.localPosition = new Vector3(-0.12f, 0.17f - (0.05f * CurrentEquipNumber), 0.1f);
                    ChangeListOfEquipment();
                //}
            break;
            //second change. mainly just changing what the cursor's parented to
            case EquipState.ChangeBetweenSelectingAndTop:
                cursorPos.parent = null;
                cursorPos.parent = EquipMenuHolder.transform;
                currentEquipState = EquipState.EquipTop;
            break;
            //doing nothing, as it should be
            case EquipState.EquipNotActive:
            break;
        }
    }
    //i know it says regenerate item menu but this is actually for the equipment menu
    //naming things is hard
    void RegenerateItemMenu(List<Item> list, string s)
    {
        //anyway, destroy all game objects in the equipment list
        if(equipText.Count > 0)
        {
            for(int i = equipText.Count-1; i >= 0; i--)
            {
                Destroy(equipText[i]);
            }
        }
        //make them again
        GenerateEquipmentMenu(list, s);
    }


    void GenerateEquipmentMenu(List<Item> list, string s)
    {
        //create a sprite icon to go along with the text
        Sprite icon;
        switch(s)
        {
            //based on what's passed to it, select a different icon
            case "body":
            icon = Resources.Load<Sprite>("Sprites/body");
            break;
            case "bag":
            icon = Resources.Load<Sprite>("Sprites/backpack");
            break;
            case "flippers":
            icon = Resources.Load<Sprite>("Sprites/flippers");
            break;
            case "tank":
            icon = Resources.Load<Sprite>("Sprites/tank");
            break;
            default:
            icon = Resources.Load<Sprite>("Sprites/body");
            break;
        }
        //get an int to figure out how many need to be made
        int howManyItemsToDraw = 0;
        //if there's more than eight items between the end of the list and the offset, say we draw eight
        if(list.Count - equipMenuOffset >= 8)
        {
            howManyItemsToDraw = 8;
        }
        else
        {
            //otherwise, draw that number
            howManyItemsToDraw = list.Count - equipMenuOffset;
        }
        //draw the items at the right offsets
        for(int i = equipMenuOffset; i < equipMenuOffset + howManyItemsToDraw; i++)
        {
            //do the hell of making and manipulating 2D items in code in Unity
            GameObject spriteObject = new GameObject();
            spriteObject.transform.parent = EquipMenu2.transform;
            spriteObject.AddComponent<SpriteRenderer>();
            spriteObject.GetComponent<SpriteRenderer>().sprite = icon;
            spriteObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            spriteObject.transform.localScale = new Vector3(0.00625f, 0.00625f, 0f);
            GameObject newText = new GameObject();
            newText.transform.parent = spriteObject.transform;
            newText.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            TextMesh t = newText.AddComponent<TextMesh>();
            t.font = Resources.Load<Font>("Fonts/SourceSansPro-Regular");
            MeshRenderer m = newText.GetComponent<MeshRenderer>();
            m.material = Resources.Load<Material>("Fonts/SourceSansPro-Regular");
            t.anchor = TextAnchor.UpperLeft;
            t.alignment = TextAlignment.Left;
            t.characterSize = 0.004f;
            t.fontSize = 80;
            t.text = list[i].ItemName;
            newText.transform.localScale = new Vector3(160f, 160f, 1f);
            newText.transform.localPosition = new Vector3(4.13f, 3.71f, 0f);
            spriteObject.transform.localPosition = new Vector3(-0.08f, 0.17f - ((i - equipMenuOffset) * 0.05f), 0.1f);
            //and throw them into lists for easy reference later
            equipText.Add(spriteObject);
            equipText.Add(newText);
            bagContents.Add(spriteObject);
        }
        //and set the reference list to be the list we're passing in here
        equipMenuItem = list;
    }

    //updates the equipment list that's shown based on cursor position
    void ChangeListOfEquipment()
    {
        //make a list of items based on what position the cursor's in on the left side.
        //this one's needed no matter which state the machine's in, so we're just calling it normally
        switch(equipMenuState)
            {
                //if it's on the body equip, pass in a body parameter, etc.
                case 0b_10000:
                List<Item> equipList = new List<Item>();
                foreach(Item i in reference.contents)
                {
                    if(i.thisItemsType == Item.ItemType.Body)
                    {
                        equipList.Add(i);
                    }
                }
                    //these if(needsUpdate) things are why we set needsUpdate to true up in the other function. these set it to false.
                    if(needsUpdate)
                    {
                        RegenerateItemMenu(equipList, "body");

                        needsUpdate = false;
                    }
                break;
                case 0b_01000:
                List<Item> equipList2 = new List<Item>();
                foreach(Item i in reference.contents)
                {
                    if(i.thisItemsType == Item.ItemType.Bag)
                    {
                        equipList2.Add(i);
                    }
                }
                    if(needsUpdate)
                    {
                        RegenerateItemMenu(equipList2, "bag");
                        needsUpdate = false;
                    }
                break;
                case 0b_00100:
                List<Item> equipList3 = new List<Item>();
                foreach(Item i in reference.contents)
                {
                    if(i.thisItemsType == Item.ItemType.Back)
                    {
                        equipList3.Add(i);
                    }
                }
                    if(needsUpdate)
                    {
                        RegenerateItemMenu(equipList3, "tank");

                        needsUpdate = false;
                    }
                break;
                case 0b_00010:
                List<Item> equipList4 = new List<Item>();
                foreach(Item i in reference.contents)
                {
                    if(i.thisItemsType == Item.ItemType.Legs)
                    {
                        equipList4.Add(i);
                    }
                }
                    if(needsUpdate)
                    {
                        RegenerateItemMenu(equipList4, "flippers");

                        needsUpdate = false;
                    }
                break;
                case 0b_00001:
                break;
            }
        //next, if we're in the top menu, update cursor positions and item text based on what's equipped
        if(currentEquipState == EquipState.EquipTop)
        {
            switch(equipMenuState)
            {
                case 0b_10000:
                cursorPos.localPosition = new Vector2(-.18f, .16f);
                EquipType.text = playerRef.bodyEquip.getItemType();
                EquipWeight.text = "Weight: " + playerRef.bodyEquip.weight + "g";
                EquipDesc.text = playerRef.bodyEquip.ItemDescription;
                break;
                case 0b_01000:
                cursorPos.localPosition = new Vector2(-.18f, .12f);
                EquipType.text = playerRef.BagEquip.getItemType();
                EquipWeight.text = "Weight: " + playerRef.BagEquip.weight + "g";
                EquipDesc.text = playerRef.BagEquip.ItemDescription;
                break;
                case 0b_00100:
                cursorPos.localPosition = new Vector2(-.18f, .08f);
                EquipType.text = playerRef.TankEquip.getItemType();
                EquipWeight.text = "Weight: " + playerRef.TankEquip.weight + "g";
                EquipDesc.text = playerRef.TankEquip.ItemDescription;
                break;
                case 0b_00010:
                cursorPos.localPosition = new Vector2(-.18f, .04f);
                EquipType.text = playerRef.FeetEquip.getItemType();
                EquipWeight.text = "Weight: " + playerRef.FeetEquip.weight + "g";
                EquipDesc.text = playerRef.FeetEquip.ItemDescription;
                break;
                case 0b_00001:
                cursorPos.localPosition = new Vector2(-.18f, 0f);
                break;
            }
        }
        //otherwise, change equipment info based on what index we're at in the selection
        else if (currentEquipState == EquipState.EquipSelecting)
        {
            if(CurrentEquipNumber + equipMenuOffset < equipMenuItem.Count)
            {
                Item dummy = equipMenuItem[CurrentEquipNumber + equipMenuOffset];
                EquipType.text =  dummy.getItemType();
                EquipWeight.text = "Weight: " + dummy.weight + "g";
                EquipDesc.text = dummy.ItemDescription;
            }
            //also if the list's empty, give it an empty string
            else
            {
                EquipType.text =  "";
                EquipWeight.text = "Weight: g";
                EquipDesc.text = "";
            }
        }
    }
    //just changes text on each of the equipped items.
    void RedrawEquipment()
    {
        BodyEquipText.text = playerRef.bodyEquip.ItemName;
        BagEquipText.text = playerRef.BagEquip.ItemName;
        TankEquipText.text = playerRef.TankEquip.ItemName;
        FinsEquipText.text = playerRef.FeetEquip.ItemName;
    }
}
