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

    // Used by the manager to tell the event when to move
    private bool send = false;
    public bool Send
    {
        get { return send; }
        set { send = value; }
    }

    // Holds the player's stats. Calculates encounter stat checks using these.
    private List<int> playerStats = new List<int>();
    

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
        //                                  Indexes:
        playerStats.Add(player.Strength);//     0
        playerStats.Add(player.Knowledge);//    1
        playerStats.Add(player.Intuition);//    2
        playerStats.Add(player.Luck);//         3
        playerStats.Add(player.HP);//           4
        playerStats.Add(player.Stamina);//      5
    }
    
    /// <summary>
    /// Returns the prompt and the corresponding actions.
    /// </summary>
    /// <returns>A tuple with the encounnter string and options.</returns>
    public (string, List<string>, List<int>) encounterInfo()
    {
        // Item 1 is the text displayed in the large text box in the play area
        // Item 2 is the list of actions + the intuition text. These come in pairs but there is a max of 4 menu buttons so please don't go above Count = 8.
        // Item 3 is the effect on the player's HP and Stamina. These also come in pairs and each pair corresponds to a menu button, so again, don't go above 8.
        // Need to refactor this code as it will be incredibly awkward to form encounters in the future using this style.
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
