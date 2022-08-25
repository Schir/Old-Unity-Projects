using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrudGenerator : MonoBehaviour
{
    int crudAmount = 20;
    GameObject canvas;
    Vector3 cent;
    public Vector3 minimum;
    public Vector3 maximum;
    int[] rotations = {0, 90, 180, 270};
    public int width;
    Sprite[] gunk;
    // Start is called before the first frame update
    void Awake()
    {
        RectTransform rect = GetComponent<RectTransform>();
        canvas = GameObject.Find("Canvas");
        cent = GetComponent<Image>().sprite.bounds.center;
        minimum = GetComponent<Image>().sprite.bounds.min;
        maximum = GetComponent<Image>().sprite.bounds.max;
        width = Mathf.FloorToInt(Mathf.Abs(minimum.x) + Mathf.Abs(maximum.x) * (Screen.width / 30));  
        Sprite[] junk = {Resources.Load<Sprite>("Sprites/gunk1"), Resources.Load<Sprite>("Sprites/gunk2"), Resources.Load<Sprite>("Sprites/gunk3")};
        gunk = junk;
        GenerateCrud();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateCrud()
    {
        for(int i = 0; i < crudAmount; i++)
        {
            GameObject crud = GenerateIndividualThingOfCrud();
            crud.transform.SetParent(canvas.transform);
            crud.GetComponent<RectTransform>().anchoredPosition = Random.insideUnitCircle * width;
            crud.GetComponent<RectTransform>().localEulerAngles = new Vector3(0,0, rotations[Random.Range(0, rotations.Length)]);
        }
    }

    GameObject GenerateIndividualThingOfCrud()
    {
        GameObject oneCrud = new GameObject();
        oneCrud.AddComponent<RectTransform>();
        oneCrud.AddComponent<CanvasRenderer>();
        oneCrud.AddComponent<Image>();
        oneCrud.GetComponent<Image>().sprite = gunk[Random.Range(0, gunk.Length)];
        oneCrud.AddComponent<BoxCollider2D>();
        oneCrud.GetComponent<BoxCollider2D>().isTrigger = true;
        oneCrud.AddComponent<Rigidbody2D>();
        oneCrud.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        oneCrud.GetComponent<Rigidbody2D>().simulated = true;
        oneCrud.tag = "crud";
        return oneCrud;
    }
    public int GetCrudAmount()
    {
        return crudAmount;
    }
}
