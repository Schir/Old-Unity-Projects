using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Characters
{
    public string characterName; // The character's name.
    public enum Type
    {
        Grass,
        Fire,
        Water,
        Electric,
        Light,
        Dark,
        Default
    }
    public bool isEnemy;

    public bool isDead;

    public int hpMax;
    public int hpCurrent;

    public int mpMax;
    public int mpCurrent;

    public int attack;
    public int curAtk;
    public int defense;
    public int curDef;
    public int intelligence;
    public int curInt;
    public int positionInCombat;
    public List<Item> characterAttackPool;
    
    public virtual Sprite GetSprite()
    {
        return null;
    }
}
