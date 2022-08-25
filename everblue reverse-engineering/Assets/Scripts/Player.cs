using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player
{
    public Item bodyEquip;
    public Item BagEquip;
    public Item FeetEquip;
    public Item TankEquip;
    public Backpack inventory;
    public int hpCurrent = 800;
    public int hpMax = 800;
    public int airMax = 200;
    public int airCurrent = 200;
    public int depthMax;
    public int depthOffset;
    public float currentDepth;
    public float maxSpeed;
    public int maxWeight;
    public int weightOverMax = 0;

    public Player()
    {
        bodyEquip = new Wetsuit();
        BagEquip = new Bag();
        FeetEquip = new Fins();
        TankEquip = new Tank();
        inventory = new Backpack();
        depthOffset = 5;
        //We're doing this as functions in the case that this project gets expanded.
        setMaxDepth();
        SetMaxSpeed();
        SetMaxWeight();
        depthOffset = 5;
        //currentDepth = depthOffset - transform.position.y;
    }
    void SetMaxSpeed()
    {
        maxSpeed = FeetEquip.velocity;
    }
    void SetMaxWeight()
    {
        maxWeight = BagEquip.maxCapacity;
    }
    void setMaxDepth()
    {
        depthMax = bodyEquip.maxDepth;
    }
    void setMaxAir()
    {
        airMax = TankEquip.airCapacity;
    }
    public void WeightHPReductionFunction()
    {
        if(inventory.weight > maxWeight)
        {
            weightOverMax = inventory.weight - maxWeight;
            hpCurrent -= (weightOverMax / 30);
        }
        checkHP();
    }
    public void ChangeTankEquip(Item e)
    {
            TankEquip = e;
            setMaxAir();
    }
    public void ChangeBagEquip(Item b)
    {
            BagEquip = b;
            SetMaxWeight();
    }
    public void ChangeBodyEquip(Item w)
    {
            bodyEquip = w;
            setMaxDepth();
    }
    public void ChangeFinEquip(Item f){
            FeetEquip = f;
            SetMaxSpeed();
        }
    public void ChangeEquipment(Item i)
    {
        switch(i.thisItemsType)
        {
            case Item.ItemType.Body:
            ChangeBodyEquip(i);
            break;
            case Item.ItemType.Back:
            ChangeTankEquip(i);
            break;
            case Item.ItemType.Legs:
            ChangeFinEquip(i);
            break;
            case Item.ItemType.Bag:
            ChangeBagEquip(i);
            break;
            default:
            break;
        }
    }
    //need to add a ChangeEquipment for gadgets. Do that later.

    public int AirReductionFunction()
    {
        int i = 1;
        //if(depthOffset - )
        return i;
    }
    void checkHP()
    {
        if(hpCurrent < 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("lose");
        }
    }
}
