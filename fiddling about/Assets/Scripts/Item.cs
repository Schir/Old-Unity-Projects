using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//=============================
//ABSTRACT DESCRIPTION OF ITEM
//=============================

[System.Serializable]
public abstract class Item {
	public Characters target;
	public Characters initiator;
	public string itemName;
	public string message;
	public int id;
	public enum ItemType
	{
		WEAPON,
		CONSUMABLE,
		KEY,
		ARMOR,
		ATTACK
	}
	public enum EffectType
	{
		BUFF,
		DEBUFF,
		DAMAGE,
		HEAL,
		MAGIC,
		WORLD,
		KEY
	}
	public static ItemType thisItemsType;
	public static EffectType thisItemsEffect;
	//public GameObject getManager;
	public string description;
	abstract public void Effect();
	abstract public string GetName();
	public string GetMessage()
	{
		return message;
	}
	public GameObject grabTheManager()
	{
		return GameObject.Find("GameManager");
	}
	public void SetTarget(Characters targeted)
	{
		target = targeted;
	}
	public void SetInitiator(Characters initiate)
	{
		initiator = initiate;
	}

}
//===========================
//IMPLEMENTED ITEMS
//===========================

	//============================
	//CONSUMABLES
	//============================
[System.Serializable]
	class Potion : Item
	{
		public void Start()
		{
	
		}
		public override void Effect()
		{
			if(target.hpCurrent+ 10 > target.hpMax)
			{
				target.hpCurrent = target.hpMax;
			}
			else
			{
				target.hpCurrent += 10;
			}

		}	
		public Potion(Characters targget)
		{
			target = targget;
			//getManager = GameObject.Find("GameManager");
			itemName = "Potion";
			//GameObject getManager = grabTheManager();
			id = 1;
			thisItemsType = ItemType.CONSUMABLE;
			thisItemsEffect = EffectType.HEAL;
		}
		public Potion()
		{
			itemName = "Potion";
			//GameObject getManager = grabTheManager();
			id = 1;
			thisItemsType = ItemType.CONSUMABLE;
			description = "A curiously-concocted brew of many obscure plants. Restores 10 HP.";
			thisItemsEffect = EffectType.HEAL;
		}
		public void CallEffect()
		{
			Effect();
		}
		public override string GetName()
		{
			return "Potion";
		}
	}
[System.Serializable]
	class BiodesiaBloom : Item
	{
		public int getItemID()
		{
			return 3;
		}
		public override string GetName()
		{
			return "BioBloom";
		}
		public override void Effect()
		{
			//Yeah I've got no idea either.
		}
		public BiodesiaBloom()
		{
			//getManager = GameObject.Find("GameManager");
			itemName = "Biodesia Bloom";
			//GameObject getManager = grabTheManager();
			id = 3;
			thisItemsType = ItemType.CONSUMABLE;
			description = "Much like the passions of youth, you'll never forget the scent of Biodesia. We have no idea what this does.";
			thisItemsEffect = EffectType.MAGIC;
		}
	}


	//==========================
	//WEAPONS
	//==========================






	//==========================
	//ARMORS
	//==========================



	//==========================
	//KEY ITEMS
	//==========================



	//=======================
	//ATTACK
	//=======================
	[System.Serializable]
	class Attack : Item
	{
		public int attackersAttack;
		public int defendersDefense;

		public void Start()
		{

		}
		public override void Effect()
		{
			attackersAttack = initiator.curAtk;
			defendersDefense = target.curDef;
			int damageDealt = attackersAttack - defendersDefense;
			//Debug.Log(initiator.characterName + " dealt "+ damageDealt + " damage to " + target.characterName);
			message = initiator.characterName + " dealt "+ damageDealt + " damage to " + target.characterName;
			if(damageDealt < 0)
			{
				damageDealt = 0;
			}
			target.hpCurrent = target.hpCurrent - damageDealt;
			if(target.hpCurrent <= 0)
			{
				target.isDead = true;

			}
			
		}	
		public Attack()
		{
			//GameObject getManager = GameObject.Find("GameManager");
			itemName = "Attack";
			id = 2;
			thisItemsType = ItemType.ATTACK;
			description = "A basic attack.";
			thisItemsEffect = EffectType.DAMAGE;
		}
		public void CallEffect()
		{
			Effect();
		}
		public override string GetName()
		{
			return "Attack";
		}
		public int getItemID()
		{
			return 2;
		}
	}


	//=======================
	//MAGIC (?)
	//=======================

	//Tier 1: Uno

	//Tier 2: Unrama

	//Tier 3: Undyne