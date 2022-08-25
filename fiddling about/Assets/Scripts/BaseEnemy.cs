using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BaseEnemy : Characters
{
    public enum Rarity
    {
        Common,
        Rare
    }
    
    public Type EnemyType;
    public Rarity rarity;

    public List<Item> enemyAttackPool;
    public virtual Item chooseRandomAttack()
    {
        return null;
    }
    public Sprite enemySprite;
    public int xpGranted;
    public int goldGranted;
    public int baseMorale;
    public int currentMorale;
    public int encounterArea;
    public override Sprite GetSprite()
    {
        return enemySprite;
    }
}