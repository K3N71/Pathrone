using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Encounter : MonoBehaviour
{
    [SerializeField, Description("Type of Encounter.")]
    private EncounterType type;
    private bool send = false;
    private List<int> playerStats = new List<int>();
    public bool Send
    {
        get { return send; }
        set { send = value; }
    }

    public EncounterType Type
    {
        get { return type; }
    }

    // OnEnable is called when the object becomes enabled and active. 
    void OnEnable()
    {
        send = false;
    }

    public void getStats(Player player)
    {
        playerStats.Add(player.Strength);
        playerStats.Add(player.Knowledge);
        playerStats.Add(player.Intuition);
        playerStats.Add(player.Luck);
        playerStats.Add(player.HP);
        playerStats.Add(player.Stamina);
    }
    
    /// <summary>
    /// Returns the prompt and the corresponding actions.
    /// </summary>
    /// <returns>A tuple with the encounnter string and options.</returns>
    public (string, List<string>, List<int>) encounterInfo()
    {
        (string, List<string>, List<int>) responseObject;
        List<string> actions = new List<string>();
        List<int> effects = new List<int>();
        switch (Type)
        {
            case EncounterType.Test:
                actions.Add("Exit");
                actions.Add("Insight would go here.");
                effects.Add(0);
                effects.Add(0);
                responseObject = ($"This is a test encounter to test the Encounter System. Your stats are:\n" +
                    $"HP: {playerStats[4]}, Stamina: {playerStats[5]}, " +
                    $"Strength: {playerStats[0]}, Knowledge: {playerStats[1]}, Intuition: {playerStats[2]}, Luck: {playerStats[3]}", 
                    actions, effects);
                return responseObject;
            default:
                actions.Add("Exit");
                actions.Add("");
                effects.Add(0);
                effects.Add(0);
                responseObject = ("Something went wrong and this encounter was not set up properly.", actions, effects);
                return responseObject;
        }
    }

}
