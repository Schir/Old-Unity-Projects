using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CameraMover : MonoBehaviour
{
    public Player playerInfo;
    public bool hasTrophy = false;
    public float translateforward = 0f;
    public float translatesideways = 0f;
    Rigidbody rb;
    public float hRotation;
    public float vRotation;
    public Vector3 mPos;
    GameObject TextThing;
    Text textref;
    Text textref2;
    bool textActive = false;
    float timerMax = 1f;
    float timerCurrent = 0f;
    GameObject TextReference;
    GameObject UIRef;
    ParticleSystem ps;
    public bool isMenuOpen = false;
    bool changeexittext = false;
    // Start is called before the first frame update
    void Awake()
    {
        playerInfo = new Player();
        rb = this.GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        TextReference = MakeADefaultUIThing(0.3f, 0.15f, 0f, Camera.main.scaledPixelHeight * -0.4f);
        string s = "";
        playerInfo.WeightHPReductionFunction();
        s += "HP: " + playerInfo.hpCurrent.ToString() + " / " + playerInfo.hpMax.ToString() + " ";
        s += "Air: "+ playerInfo.airCurrent.ToString() + " / " + playerInfo.airMax.ToString() + " ";
        s += "Weight: " + playerInfo.inventory.weight.ToString() + " / " + playerInfo.BagEquip.maxCapacity.ToString();
        UIRef = MakeADefaultUIThing2(0.6f, 0.15f, Camera.main.scaledPixelWidth * -0.2f, Camera.main.scaledPixelHeight * 0.4f, s);       
        textActive = false;
        ps = GameObject.FindGameObjectWithTag("particles").GetComponent<ParticleSystem>();
        ps.Play();
    }


    // Update is called once per frame
    void Update()
    {
        if(timerCurrent >= timerMax)
        {
            UpdateUIStuff();
            timerCurrent = 0f;
        }
        else
        {
            timerCurrent += Time.deltaTime;
        }
        RaycastHit hit = new RaycastHit();
        mPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        translateforward = Input.GetAxis("Vertical") * playerInfo.maxSpeed; //* Time.deltaTime;
        translatesideways = Input.GetAxis("Horizontal") * playerInfo.maxSpeed; //* Time.deltaTime;
        hRotation = playerInfo.maxSpeed * Input.GetAxis("Mouse X");
        vRotation = playerInfo.maxSpeed * Input.GetAxis("Mouse Y");
        if(translateforward != 0f)
        {
            if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.Mouse0))
            {
                rb.AddForce(Vector3.up * translateforward);
            }
            else
            {
                rb.AddForce(transform.forward * translateforward);                
            }
        }
        if(translatesideways != 0f)
        {
            rb.AddForce(transform.right * translatesideways);
        }
        transform.Rotate(-vRotation,0f,0f, Space.Self);
        transform.Rotate(0f, hRotation, 0f, Space.World);
        

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
          CheckTag(hit);
        }
        else
        {
            textActive = false;
        }
        TextThing.SetActive(textActive);
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit,3f))
            {
                switch(hit.transform.tag)
                {
                    case "Item":
                        playerInfo.inventory.AddItemToBag(hit.collider.gameObject.GetComponent<WorldItem>().item);
                        if(hit.transform.GetComponent<WorldItem>().item.ItemName == "Rad Trophy")
                        {
                            hasTrophy = true;
                        }
                        Destroy(hit.transform.gameObject);
                        textActive = false;
                        break;
                    case "exit":
                        if(hasTrophy)
                        {
                          UnityEngine.SceneManagement.SceneManager.LoadScene("end");  
                        }
                        else
                        {
                            changeexittext = true;
                        }
                        break;
                    case "door":
                        OpenDoor(hit);
                        break;
                }
            }
        }

       /* 
        if(mPos.y > 0.9f)
        {
            transform.Rotate(playerInfo.maxSpeed * -1f,0f,0f, Space.Self);
        }

        else if(mPos.y < 0.1f)
        { 
            transform.Rotate(playerInfo.maxSpeed * 1f,0f,0f, Space.Self);
        }
        else
        {

        }
        if(mPos.x < 0.2f)
        {
            transform.Rotate(0f, playerInfo.maxSpeed * -1f, 0f, Space.World);
        }
        else if(mPos.x > 0.8f)        
        {
            transform.Rotate(0f, playerInfo.maxSpeed * 1f, 0f, Space.World);
        }
        else
        {

        }*/
    }
    float ClampAngle(float angle, float from, float to)
     {
         // accepts e.g. -80, 80
         if (angle < 0f) angle = 360 + angle;
         if (angle > 180f) return Mathf.Max(angle, 360+from);
         return Mathf.Min(angle, to);
     }

    private void OpenDoor(RaycastHit hit)
    {
        if(!hit.transform.GetComponent<DoorScript>().needsKey)
        {
            if(hit.transform.GetComponent<DoorScript>().isOpen)
            {
                hit.transform.parent.Rotate(new Vector3(0f, 90f,0f));
            }
            else
            {
                hit.transform.parent.Rotate(new Vector3(0f, -90f,0f));
            }
            hit.transform.GetComponent<DoorScript>().OpenDoor();
        }
        else
        {
            foreach(Item item in playerInfo.inventory.contents)
            {
                if(item.ItemName == "Boat Key")
                {
                    if(hit.transform.GetComponent<DoorScript>().isOpen)
                    {
                        hit.transform.parent.Rotate(new Vector3(0f, 90f,0f));
                    }
                    else
                    {
                        hit.transform.parent.Rotate(new Vector3(0f, -90f,0f));
                    }
                    hit.transform.GetComponent<DoorScript>().OpenDoor();
                    break;
                }
            }
        }
    }
    

    void CheckTag(RaycastHit hit)
    {
        string s = hit.transform.tag;
        if(hit.distance < 3f)
        {
            switch(s)
            {
                case "Item":
                    //Debug.Log("Found an item!");
                    ChangeText(hit.transform.gameObject.GetComponent<WorldItem>().item.ItemName +"\nPress E to pick up.");
                    textActive = true;
                    break;
                case "exit":
                    if(!changeexittext)
                    {
                        ChangeText("This is the exit.\nPress E to leave.");
                    }
                    else
                    {
                        if(hasTrophy)
                        {
                            changeexittext = false;
                        }
                        else
                        {
                            ChangeText("What, and leave without the trophy?");
                        }
                    }
                    textActive = true;
                    break;
                case "door":
                    if(hit.transform.GetComponent<DoorScript>().needsKey)
                    {
                        ChangeText("You need a key to open this door.\nPress E when you have a key.");
                    }
                    else
                    {
                        ChangeText("Door\nPress E to open.");
                    }
                    textActive = true;
                    break;
                default:
                textActive = false;
                break;
            }
        }
        else
        {
            textActive = false;
        }
        //Debug.Log("Yep this is still being called");
    }

    void ChangeText(string s)
    {
        textref.text = s;
    }
    GameObject MakeADefaultUIThing(float xWidth, float yHeight, float xPos, float yPos)
    {
        TextThing = new GameObject();
        TextThing.transform.parent = transform;
        TextThing.AddComponent<Canvas>();
        TextThing.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        TextThing.AddComponent<GraphicRaycaster>();
        TextThing.AddComponent<CanvasScaler>();
        GameObject textBG = new GameObject();
        textBG.transform.parent = TextThing.transform;
        Image bg = textBG.AddComponent<Image>();
        bg.sprite = Resources.Load<Sprite>("Sprites/black");
        GameObject RespectText = new GameObject();
        RespectText.transform.parent = textBG.transform;
        Text RespectTextt = RespectText.AddComponent<Text>();
        RespectTextt.font = Resources.Load<Font>("Fonts/SourceSansPro-Regular");
        RespectTextt.color = Color.white; 
        RespectTextt.alignment = TextAnchor.MiddleCenter;
        RespectTextt.text = "found an item";
        RespectTextt.fontSize = Convert.ToInt32(Math.Floor(Camera.main.scaledPixelHeight * 0.05f));
        textref = RespectTextt;
        RectTransform rect = textBG.GetComponent<RectTransform>();
        RectTransform rect2 = RespectText.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(xPos,yPos,0f);
        rect2.localPosition = new Vector3(0f,0f,0f);
        rect.sizeDelta = new Vector2(Camera.main.scaledPixelWidth * xWidth, Camera.main.scaledPixelHeight * yHeight);
        rect2.sizeDelta = rect.sizeDelta;
        return TextThing;
    }
    void UpdateUIStuff()
    {
        string s = "";
        playerInfo.WeightHPReductionFunction();
        s += "HP: " + playerInfo.hpCurrent.ToString() + " / " + playerInfo.hpMax.ToString() + " ";
        playerInfo.airCurrent -= playerInfo.AirReductionFunction();
        s += "Air: "+ playerInfo.airCurrent.ToString() + " / " + playerInfo.airMax.ToString() + " ";
        s += "Weight: " + playerInfo.inventory.weight.ToString() + " / " + playerInfo.BagEquip.maxCapacity.ToString() + "\n";
        s += "Depth: " + (playerInfo.depthOffset - transform.position.y).ToString() + " / " + playerInfo.depthMax.ToString();
        textref2.text = s;
    }
     GameObject MakeADefaultUIThing2(float xWidth, float yHeight, float xPos, float yPos, string s)
    {
        GameObject TextThingg = new GameObject();
        TextThingg.transform.parent = transform;
        TextThingg.AddComponent<Canvas>();
        TextThingg.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        TextThingg.AddComponent<GraphicRaycaster>();
        TextThingg.AddComponent<CanvasScaler>();
        GameObject textBG = new GameObject();
        textBG.transform.parent = TextThingg.transform;
        Image bg = textBG.AddComponent<Image>();
        bg.sprite = Resources.Load<Sprite>("Sprites/black");
        GameObject RespectText = new GameObject();
        RespectText.transform.parent = textBG.transform;
        Text RespectTextt = RespectText.AddComponent<Text>();
        RespectTextt.font = Resources.Load<Font>("Fonts/SourceSansPro-Regular");
        RespectTextt.color = Color.white; 
        RespectTextt.alignment = TextAnchor.UpperLeft;
        RespectTextt.text = s;
        RespectTextt.fontSize = Convert.ToInt32(Math.Floor(Camera.main.scaledPixelHeight * 0.05f));
        textref2 = RespectTextt;
        RectTransform rect = textBG.GetComponent<RectTransform>();
        RectTransform rect2 = RespectText.GetComponent<RectTransform>();
        rect.localPosition = new Vector3(xPos,yPos,0f);
        rect2.localPosition = new Vector3(0f,0f,0f);
        rect.sizeDelta = new Vector2(Camera.main.scaledPixelWidth * xWidth, Camera.main.scaledPixelHeight * yHeight);
        rect2.sizeDelta = rect.sizeDelta;
        return TextThingg;
    }
}
