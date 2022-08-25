using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy {

  public List<BaseEnemy> area1Enemies;
  public List<BaseEnemy> area2Enemies;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

  //modify this whenever you think up more enemies for an area.
  public static List<EnemyHolder> setArea1Enemies()
  {
    List<EnemyHolder> tempArea1 = new List<EnemyHolder>();
    tempArea1.Add(new Coins());
    tempArea1.Add(new Coins());
    tempArea1.Add(new Coins());
    tempArea1.Add(new Coins());
    tempArea1.Add(new Coins());    
    tempArea1.Add(new GambolingCrate());
    tempArea1.Add(new GambolingCrate());
    tempArea1.Add(new GambolingCrate());
    tempArea1.Add(new GambolingCrate());
    tempArea1.Add(new GambolingCrate());

    return tempArea1;
  }
}


public class EnemyHolder
{
  public BaseEnemy enemyInfo;

  public static Item chooseRandomAttack(BaseEnemy enemy)
    {
      System.Random rnd = new System.Random();
      int randomAttack = rnd.Next(0, enemy.enemyAttackPool.Count-1);

      return enemy.enemyAttackPool[randomAttack];
    }
    public static void assignEnemyPositionForLaterReference(int n, Characters enemy)
    {
      enemy.positionInCombat = n;
    }
  public string getName()
  {
    return enemyInfo.characterName;
  }    
}
//=========================
//           AREA 1
//=========================

[System.Serializable]
public class Coins : EnemyHolder
{
	/*public string enemyName = "Animate Coins";

  	public Type EnemyType = BaseEnemy.Type.Dark;
    public Rarity rarity = Rarity.Common;

    public float baseHP = 12;
    public float curHP = 12;

    public float baseMP = 0;
    public float curMP = 0;

    public float baseAtk = 5;
    public float curAtk = 5;
    public float baseDef = 5;
    public float curDef = 5;*/

    public void Start()
    {
    }
    public Coins()
    {
      enemyInfo = new BaseEnemy();
      enemyInfo.characterName = "Animate Coins";
      enemyInfo.isEnemy = true;
      enemyInfo.EnemyType = BaseEnemy.Type.Dark;
      enemyInfo.rarity = BaseEnemy.Rarity.Common;
      enemyInfo.hpMax = 12;
      enemyInfo.hpCurrent = 12;
      enemyInfo.mpMax = 0;
      enemyInfo.mpCurrent = 0;
      enemyInfo.attack = 11;
      enemyInfo.curAtk = 11;
      enemyInfo.defense = 5;
      enemyInfo.curDef = 5;
      enemyInfo.xpGranted = 10;
      enemyInfo.goldGranted = 40;
      enemyInfo.isDead = false;
      enemyInfo.baseMorale = 70;
      enemyInfo.currentMorale = 70;
      Attack attack = new Attack();
      enemyInfo.enemyAttackPool = new List<Item>();
      enemyInfo.enemySprite = Resources.Load<Sprite>("Sprites/animatecoinsbw");
      //enemyInfo.characterSprite = Resources.Load<Sprite>("Sprites/animatecoinsbw");
      enemyInfo.enemyAttackPool.Add(attack);
      enemyInfo.enemyAttackPool.Add(attack);
      enemyInfo.encounterArea = 1;

    }
}
[System.Serializable]
public class GambolingCrate : EnemyHolder
{
  public void Start()
  {

  }

    public GambolingCrate()
    {
      enemyInfo = new BaseEnemy();
      enemyInfo.characterName = "Gamboling Crate";
      enemyInfo.isEnemy = true;
      enemyInfo.EnemyType = BaseEnemy.Type.Dark;
      enemyInfo.rarity = BaseEnemy.Rarity.Common;
      enemyInfo.hpMax = 12;
      enemyInfo.hpCurrent = 12;
      enemyInfo.mpMax = 0;
      enemyInfo.mpCurrent = 0;
      enemyInfo.attack = 11;
      enemyInfo.curAtk = 11;
      enemyInfo.isDead = false;
      enemyInfo.defense = 5;
      enemyInfo.curDef = 5;
      enemyInfo.xpGranted = 10;
      enemyInfo.goldGranted = 10;
      enemyInfo.baseMorale = 70;
      enemyInfo.currentMorale = 70;
      enemyInfo.enemySprite = Resources.Load<Sprite>("Sprites/gambolingcratebw2");
      //enemyInfo.characterSprite = Resources.Load<Sprite>("Sprites/gambolingcratebw2");
      enemyInfo.enemyAttackPool = new List<Item>();
      enemyInfo.enemyAttackPool.Add(new Attack());
      enemyInfo.enemyAttackPool.Add(new Attack());
    }
    
}

