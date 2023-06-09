using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Encounter : MonoBehaviour
{
    // Initialize in Encounter Prefab please!
    [SerializeField, Description("Type of Encounter.")]
    private EncounterType type;

    private Encounters encounters;
    private (string, List<(string, string)>, List<string>, List<Stats>) responseObject;

    // Used by the manager to tell the event when to move
    private bool send = false;
    public bool Send
    {
        get { return send; }
        set { send = value; }
    }

    // Holds the player's stats. Calculates encounter stat checks using these.
    private Stats playerStats;

    public EncounterType Type
    {
        get { return type; }
    }

    // OnEnable is called when the object becomes enabled and active. 
    void OnEnable()
    {
        send = false;   
    }

    public void setStats(Stats stats)
    {
        playerStats = stats;
        // Item 1 is the text displayed in the large text box in the play area
        // Item 2 is the list of actions + the intuition text. Right now there are only 4 menu buttons so please don't go above count = 4.
        // Item 3 is the list of results from the player's choices.
        // Item 4 is the effect on the player's stats. These also come in pairs and each pair corresponds to a menu button, so again, don't go above 4.
        // Need to refactor this code as it will be incredibly awkward to form encounters in the future using this style.
        switch (Type)
        {
            case EncounterType.Test:
                {
                    List<(string, string)> exit = new List<(string, string)>();
                    List<string> results = new List<string>();
                    results.Add("");
                    List<Stats> statChanges = new List<Stats>();
                    exit.Add(("Exit", "Insight would go here."));
                    statChanges.Add(new Stats(playerStats));
                    statChanges[0].HP -= 1;
                    responseObject = ($"This is a test encounter to test the Encounter System. Your stats are:\n" +
                        $"HP: {playerStats.HP}, Stamina: {playerStats.Stamina}, " +
                        $"Strength: {playerStats.Strength}, Knowledge: {playerStats.Knowledge}, Intuition: {playerStats.Intuition}, Luck: {playerStats.Luck}.\n" +
                        $"As a test, your HP will decrease by 1.",
                        exit, results, statChanges);
                    break;
                }
            case EncounterType.Attack:
                encounters = new AttackEncounters(playerStats);
                responseObject = (encounters.DescText, encounters.Choices, encounters.Results, encounters.StatChanges);
                break;
            case EncounterType.CityTown:
                encounters = new CityEncounters(playerStats);
                responseObject = (encounters.DescText, encounters.Choices, encounters.Results, encounters.StatChanges);
                break;

            default:
                {
                    List<(string, string)> exit = new List<(string, string)>();
                    exit.Add(("Exit", ""));
                    List<string> results = new List<string>();
                    results.Add("");
                    List<Stats> statChanges = new List<Stats>();
                    statChanges.Add(new Stats(playerStats));
                    responseObject = ("Something went wrong and this encounter was not set up properly.", exit, results, statChanges);
                }
                break;
        }
    }
    public void updateStats(Stats stats)
    {
        playerStats = stats;
        encounters.UpdateStats(playerStats);
        responseObject = (encounters.DescText, encounters.Choices, encounters.Results, encounters.StatChanges);
    }
    /// <summary>
    /// Returns the prompt and the corresponding actions.
    /// </summary>
    /// <returns>A tuple with the encounnter string and options.</returns>
    public (string, List<(string, string)>, List<string>, List<Stats>) encounterInfo()
    {
        return responseObject;
    }

}
