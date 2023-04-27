using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
public class Encounters
{
    protected string descText;
    protected List<(string, string)> choices;
    protected List<string> results;
    protected List<Stats> statChanges;
    protected int seed;
    protected float check;
    protected Random rand = new Random();

    public string DescText { get { return descText; } }
    public List<(string, string)> Choices { get { return choices; } }
    public int NumChoices { get { return choices.Count; } }
    public List<string> Results { get { return results; } }
    public List<Stats> StatChanges { get { return statChanges; } }

    protected Stats stats;

    public void UpdateStats(Stats playerStats)
    {
        stats = new Stats(playerStats);
        generateEncounter();
    }
    protected virtual void generateEncounter()
    {
    }
}
