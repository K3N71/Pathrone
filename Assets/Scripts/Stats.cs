using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    int[] stats = new int[9];

    public Stats()
    {
        stats[0] = 3; // strength
        stats[1] = 3; // knowledge
        stats[2] = 3; // intuition
        stats[3] = 3; // luck
        stats[4] = 3; // longevity
        for (int i = 0; i < 10; i++)
        {
            int statToAllocate = Random.Range(0, 5);
            if (stats[statToAllocate] < 8)
                stats[statToAllocate]++;
            else
                i--;
        }
        stats[5] = stats[4] * 4;       // max HP
        stats[6] = stats[4] * 4;       // max stamina
        stats[7] = stats[5];           // current HP
        stats[8] = stats[6];           // current stamina
    }
    public Stats(int strength, int knowledge, int intuition, int luck, int longevity)
    {
        stats[0] = strength;      // strength
        stats[1] = knowledge;     // knowledge
        stats[2] = intuition;     // intuition
        stats[3] = luck;          // luck
        stats[4] = longevity;     // longevity
        stats[5] = stats[4] * 4;  // max HP
        stats[6] = stats[4] * 4;  // max stamina
        stats[7] = stats[5];      // current HP
        stats[8] = stats[6];      // current stamina
    }
    public Stats(Stats copy)
    {
        stats[0] = copy.Strength;   // strength
        stats[1] = copy.Knowledge;  // knowledge
        stats[2] = copy.Intuition;  // intuition
        stats[3] = copy.Luck;       // luck
        stats[4] = copy.Longevity ; // longevity
        stats[5] = stats[4] * 4;    // max HP
        stats[6] = stats[4] * 4;    // max stamina
        stats[7] = copy.HP;        // current HP
        stats[8] = copy.Stamina;        // current stamina
    }

    public int Strength
    {
        get { return stats[0]; }
        set { stats[0] = value; }
    }
    public int Knowledge
    {
        get { return stats[1]; }
        set { stats[1] = value; }
    }
    public int Intuition
    {
        get { return stats[2]; }
        set { stats[2] = value; }
    }
    public int Luck
    {
        get { return stats[3]; }
        set { stats[3] = value; }
    }
    public int Longevity
    {
        get { return stats[4]; }
        set
        {
            stats[4] = value;
            stats[5] = stats[4] * 4; // max HP
            stats[6] = stats[4] * 4; // max stamina
        }
    }
    public int HP
    {
        get { return stats[7]; }
        set
        { 
            stats[7] = value;
            if (stats[7] > stats[4] * 4) stats[7] = stats[4] * 4;
        }
    }
    public int Stamina
    {
        get { return stats[8]; }
        set 
        { 
            stats[8] = value;
            if (stats[8] > stats[4] * 4) stats[8] = stats[4] * 4;
        }
    }
}
