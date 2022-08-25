using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Senator
{
    static List<string> firstNames = new List<string>{
        "John",
        "George",
        "Jimmy",
        "Bill",
        "William",
        "Sarah",
        "Teresa",
        "Terry",
        "Chris",
        "Jake",
        "April"
    };
    static List<string> lastNames = new List<string>{
        "Brown",
        "Grey",
        "Green",
        "White",
        "Smith",
        "Baker",
        "Williams",
        "Carpenter",
        "Carter",
        "Schmidt",
        "Gunface",
        "Goatfucker",
        "May"
    };

    public string senatorName = "";
    public enum PoliticalAffiliation
    {
        Left,
        Right,
        Center
    }
    public PoliticalAffiliation senatorsAffiliation;
    public enum Belief
    {
        Extremist,
        Moderate,
        Opportunist
    }
    public Belief senatorsBelief;
    public static Senator CreateSenator(System.Random r)
    {
        Senator s = new Senator();
        s.senatorName = GenerateSenatorName(r);
        s.senatorsAffiliation = GenerateAffiliation(r.Next(0, 2));

        return s;
    }
    public static string GenerateSenatorName(System.Random r)
    {
        string tempName = "";
        tempName += firstNames[r.Next(0, firstNames.Count)];
        tempName += " ";
        tempName += lastNames[r.Next(0, lastNames.Count)];

        return tempName;
    }
    public static PoliticalAffiliation GenerateAffiliation(int i)
    {
        switch(i)
        {
            case 0:
                return PoliticalAffiliation.Center;
            case 1:
                return PoliticalAffiliation.Left;
            case 2:
                return PoliticalAffiliation.Right;
            default:
                return PoliticalAffiliation.Center;

        }

    }
}