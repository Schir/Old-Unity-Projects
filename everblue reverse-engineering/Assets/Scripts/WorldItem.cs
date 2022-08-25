using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public Item item;
    public bool isCameraFocusedOnThis = false;
    public int id;
    public List<Item> possibleItems = new List<Item>{new Key1(), new Painting(), new Vase(), new Trophy(), new PackOfCigarettes(), new Ashtray()};
    // Start is called before the first frame update
    void Awake()
    {
        item = possibleItems[id];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    0: Key1
    1: Painting
    2: Vase
    3: Trophy
    4: PackOfCigarettes
    5: Ashtray
     */
}
