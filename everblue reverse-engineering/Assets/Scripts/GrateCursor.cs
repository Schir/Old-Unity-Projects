using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrateCursor : MonoBehaviour
{
    public RectTransform rect;
    public int crudRemaining;
    public int startingCrud;
    UnityEngine.UI.Text remainingText;
    // Start is called before the first frame update
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        crudRemaining = GameObject.FindGameObjectWithTag("crudGenerator").GetComponent<CrudGenerator>().GetCrudAmount();
        startingCrud = crudRemaining;
        remainingText = GameObject.Find("remainingText").GetComponent<UnityEngine.UI.Text>();
        remainingText.text = "Crud Remaining: " + crudRemaining + "/" + startingCrud;
    }

    // Update is called once per frame
    void Update()
    {
        rect.position = Input.mousePosition;
    }
    void ChangeText()
    {
        remainingText.text = "Crud Remaining: " + crudRemaining + "/" + startingCrud;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "crud")
        {
          other.gameObject.SetActive(false);
          crudRemaining--;
          ChangeText();
        }    
    }


    public int GetRemainingCrud()
    {
        return crudRemaining;
    }
}
