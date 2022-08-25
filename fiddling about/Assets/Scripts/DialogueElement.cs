using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueElement
{
    public enum Characters{ Claire, November, John, NPC, none};
    public enum AvatarPos{ left, right, none};
    public Characters Character;
    public AvatarPos CharacterPosition;
    public Sprite CharacterPic;
    public string DialogueText;
    public float TextPlayBackSpeed;
    public AudioClip PlayBackSoundFile;
}