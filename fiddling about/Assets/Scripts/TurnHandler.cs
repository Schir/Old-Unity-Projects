using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurnHandler
{
	public Characters Attacker; //which game object is doing the attack
	public Characters TargetOfAttack; // subject of the attack.
	public Item Attack;
	//what attack is performed?
}
