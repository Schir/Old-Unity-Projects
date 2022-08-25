using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    public int weight;
    public string ItemName;
    public string ItemDescription;
    public enum MaterialType
    {
        Metal,
        Wood,
        Clay,
        Glass
    }
    public enum ItemType
    {
        Body,
        Back,
        Legs,
        Bag,
        Gadget,
        Consumable,
        Key,
        Other

    }
    public ItemType thisItemsType;
    public MaterialType thisItemsMaterial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual bool GetEquipmentStatus()
    {
        return false;
    }
    public string getItemType()
    {
        switch(thisItemsType)
        {
            case Item.ItemType.Back:
            return "Gear: Back";
            case Item.ItemType.Bag:
            return "Gear: Bag";
            case Item.ItemType.Body:
            return "Gear: Body";
            case Item.ItemType.Consumable:
            return "Consumable";
            case Item.ItemType.Gadget:
            return "Gear: Gadget";
            case Item.ItemType.Key:
            return "Key";
            case Item.ItemType.Legs:
            return "Gear: Legs";
            case Item.ItemType.Other:
            return "Other";
            default:
            return "Other";
        }
    }
    public int maxCapacity;
    public int maxDepth;
    public float velocity;
    public int airCapacity;
    public int reasonableDepth;
}
public abstract class Equipment : Item
{
    public override bool GetEquipmentStatus()
    {
        return true;
    } 
}
public class Wetsuit : Equipment
{
    public Wetsuit()
    {
        maxDepth = 15;
        thisItemsType = Item.ItemType.Body;
        thisItemsMaterial = Item.MaterialType.Metal;
        ItemName = "Wetsuit 1";
        ItemDescription = "A cheap wetsuit. It only works to a depth of 15 feet.";
        weight = 0;
    }
}
public class Wetsuit2 : Wetsuit
{
    public Wetsuit2()
    {
        maxDepth = 30;
        thisItemsType = Item.ItemType.Body;
        thisItemsMaterial = Item.MaterialType.Metal;
        ItemName = "Wetsuit 2";
        ItemDescription = "A less cheap wetsuit. It works to a depth of 30 feet.";
        weight = 20;
    }
}
public class Wetsuit3 : Wetsuit
{
    public Wetsuit3()
    {
        maxDepth = 60;
        thisItemsType = Item.ItemType.Body;
        thisItemsMaterial = Item.MaterialType.Metal;
        ItemName = "Drysuit";
        ItemDescription = "A wetsuit with better insulating properties. It's popular among hobbyists\nIt insulates to a depth of 60 feet.";
        weight = 40;
    }
}
public class Bag : Equipment
{
    public Bag()
    {
        maxCapacity = 600;

        thisItemsType = Item.ItemType.Bag;
        thisItemsMaterial = Item.MaterialType.Metal;
        ItemName = "Backpack 1";
        ItemDescription = "A cheap bag you got off Amazon.\n Holds up to 600 grams of items.";
        weight = 0;
        
    }
}
public class Fins : Equipment
{   
    public Fins()
    {
        velocity = 9f;
        
        thisItemsType = Item.ItemType.Legs;
        thisItemsMaterial = Item.MaterialType.Metal;
        ItemName = "Fins";
        ItemDescription = "The most basic fins money can buy.";
    }
}
public class Tank : Equipment
{
    public Tank()
    {
        airCapacity = 200;
        reasonableDepth = 10;
        
        thisItemsType = ItemType.Back;
        thisItemsMaterial = Item.MaterialType.Metal;
        ItemName = "Air Tank";
        ItemDescription = "AirCo's budget AquaLung. \nWorks to a depth of 10 feet.";
    }
}
public class Key1 : Item
{
    public Key1()
    {
        thisItemsType = Item.ItemType.Key;
        thisItemsMaterial = Item.MaterialType.Metal;
        ItemName = "Boat Key";
        ItemDescription = "A key to unlock a door in the boat.";
        weight = 10;
    }
}
public class Painting : Item
{
    public Painting()
    {
        thisItemsType = Item.ItemType.Other;
        thisItemsMaterial = Item.MaterialType.Wood;
        ItemName = "Painting";
        ItemDescription = "It's a painting.";
        weight = 400;
    }
}
public class Vase : Item
{
    public Vase()
    {
        thisItemsType = Item.ItemType.Other;
        thisItemsMaterial = Item.MaterialType.Clay;
        ItemName = "Cool Vase";
        ItemDescription = "Wow!! Cool vase!!";
        weight = 500;
    }
}
public class Trophy : Item
{
    public Trophy()
    {
        thisItemsMaterial = Item.MaterialType.Metal;
        thisItemsType = Item.ItemType.Other;
        ItemName = "Rad Trophy";
        ItemDescription = "This trophy has Marxist-Leninist leanings.";
        weight = 600;
    }
}
public class PackOfCigarettes : Item
{
    public PackOfCigarettes()
    {
        thisItemsMaterial = Item.MaterialType.Wood;
        thisItemsType = Item.ItemType.Other;
        ItemName = "Pack o' cigs";
        ItemDescription = "why would you even pick up a pack of\ncigarettes you found on the ocean floor.\nthey're all wet.";
        weight = 40;
    }
}
public class Ashtray : Item
{
    public Ashtray()
    {
        thisItemsMaterial = Item.MaterialType.Glass;
        thisItemsType = Item.ItemType.Other;
        ItemName = "Ashtray";
        ItemDescription = "It's an ashtray.";
        weight = 120;
    }
}