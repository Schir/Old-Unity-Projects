using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorChecker : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 10f))
        {
            if(Input.GetMouseButtonDown(0))
            {
                switch(hit.collider.tag)
                {
                case "fish":
                Debug.Log("Buzz");
                break;
                default:
                Debug.Log("Fizz");
                break;
                }

            }
        }
    }
}
