using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStats : Characters {
	//fix all this to fit with stuff inherited from Characters. It'll make item-handling less of a pain.
	public int level; // Highest floor reached. Stats increase proportionally to level. Might separate this raw upgrade out from combat ability? 
	public Job characterJob;
	//max MP. I'm not very fond of MP-based systems. I might want to try looking into other magic systems like spell level or spells which drain stats in their casting. More research is required.
	//I wonder if I could mix an MP system with a prepared spell system. Higher level spells cost more MP to prepare, but are more powerful. 
	//Player can only prepare spells when the GameManager is in GameStates.TOWN. Could be interesting. I don't really like the way that Final Fantasy or Dark Spire implemented spell slots,
	// but there's some potential in the idea.
	//Man, what if the player had to prepare spells on a rest, and then had to literally spell out the name of the spell they prepared in order to cast it, like what Wizardry did?
	//Players would hate all the extra hassle of casting spells, but it'd be rad. 
	//public int intelligence; //Affects something. Not sure what yet.
	public int experience; // experience points. Obtained from battle and quests. Might consider looking into an XP for GP system. Might give paltry experience for non-quest related experiences. Unsure for the moment.
	public bool canLevelUp; // Might use this to make it so the player doesn't immediately level up when they have the XP to do so.
	// I'm really fond of the way that The Dark Spire handled it. You aren't really skilled if you can't make it back from your adventures alive. Don't let your party members die. 
	// I should really make it so that escape is guaranteed when an enemy could theoretically kill you in one shot.
	public bool isInParty;
	//public int thac0; // this is a curse I will reserve for later.
	public enum Job
	{
		Reporter, // Default class for one of the characters.
		Scholar, // Upgradeable class.
		Mercenary, //Upgradeable class. Good attack.
		Scout, // Upgradeable class. Good speed? Not entirely sure yet.
		FleshAndroid, // Default class for a different one of the characters.
		Businessman, //listen I gotta play to my base, alright
		ChaosNoble //Party all changes to this class at the end of the game. Broken and overpowered.
	}
}

/*
public struct Job
{
	public enum JobName
	{
		Reporter, // Default class for one of the characters.
		Scholar, // Upgradeable class.
		Mercenary, //Upgradeable class. Good attack.
		Scout, // Upgradeable class. Good speed? Not entirely sure yet.
		FleshAndroid, // Default class for a different one of the characters.
		ChaosNoble //Party all changes to this class at the end of the game. Broken and overpowered.
	}

}*/ //don't need a struct quite yet. Might return to this when the combat system's a bit more fleshed out.


//writing some stuff down for later.
public class Magic
{
	public List<Spell> SpellsKnown;
	public List<Spell> SpellsPrepared;

}

[System.Serializable]
public class Spell
{
	public string spellName;
	public int id;
	public int level;
	//a reference to some data holding all the spells maybe?
}