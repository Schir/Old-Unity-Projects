using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack
{
    public List<Item> contents = new List<Item>();
    public int weight = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    int RecalculateWeight()
    {
        int contentTemp = 0;
        if(contents.Count > 0)
        {
            for(int i = 0; i < contents.Count; i++)
            {
                contentTemp += contents[i].weight;
            }
        }
        return contentTemp;
    }
    public void AddItemToBag(Item item)
    {
        contents.Add(item);
        weight = RecalculateWeight();
    }
}
