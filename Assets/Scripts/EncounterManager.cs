using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EncounterManager : MonoBehaviour
{
    [Header("Reference to the Objects.")]
    [SerializeField, Description("Globals Object")]
    private Globals globals;
    [SerializeField, Description("Player Object")]
    private GameObject player;
    [SerializeField, Description("Text Object")]
    private TextMeshProUGUI text;
    [SerializeField, Description("Stats Display")]
    private TextMeshProUGUI statsDisplay;
    [SerializeField, Description("Menu Object 1")]
    private GameObject menu1;
    [SerializeField, Description("Menu Object 2")]
    private GameObject menu2;
    [SerializeField, Description("Menu Object 3")]
    private GameObject menu3;
    [SerializeField, Description("Menu Object 4")]
    private GameObject menu4;
    private List<(string, string)> menuData = new List<(string, string)>();
    private List<Stats> statChanges = new List<Stats>();

    [Header("Encounter Variables.")]
    [SerializeField, Description("Frequency of Encounters.")]
    private float encounterRate = 12.0f;
    private float encounterTimer;
    [SerializeField, Description("Encounter move speed.")]
    private float encounterMoveSpeed = 3.0f;

    [SerializeField, Description("Encounter Queue")]
    private Queue<GameObject> encounterQueue = new Queue<GameObject>();

    [Header("Encounter Prefabs")]
    [SerializeField, Description("Encounter Prefabs")]
    private List<GameObject> prefabList = new List<GameObject>();

    private List<string> resultingText = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        encounterTimer = encounterRate;
        menu1.SetActive(false);
        menu2.SetActive(false);
        menu3.SetActive(false);
        menu4.SetActive(false);
        text.text = "Welcome to our game, Pathrone! When the colored cube hits your cube, you will enter an encounter.";
        
}

    // Update is called once per frame
    void Update()
    { 
        // Code that occurs during the game.
        if (globals.CurrentState == GameState.Game)
        {
            statsDisplay.text = $"Player Stats\n" +
                $"HP: {player.GetComponent<Player>().Stats.HP} / {player.GetComponent<Player>().Stats.Longevity * 4}\n" +
                $"Stamina: {player.GetComponent<Player>().Stats.Stamina} / {player.GetComponent<Player>().Stats.Longevity * 4}\n" +
                $"Strength: {player.GetComponent<Player>().Stats.Strength}\n" +
                $"Knowledge: {player.GetComponent<Player>().Stats.Knowledge}\n" +
                $"Intuition: {player.GetComponent<Player>().Stats.Intuition}\n" +
                $"Luck: {player.GetComponent<Player>().Stats.Luck}";

            // Make sure the game has a queue of 5 events to peek at. Need to fix pool code, is not working.
            if (encounterQueue.Count < 5  && player.GetComponent<Player>())
            {
                // Once other events are made, generate pseudorandomly
                switch (UnityEngine.Random.Range(0, 2))
                {
                    case 0:
                        encounterQueue.Enqueue(getEncounter(EncounterType.Attack));
                        if (globals.Debug) Debug.Log("adding attack encounter");
                        break;
                    case 1:
                        encounterQueue.Enqueue(getEncounter(EncounterType.CityTown));
                        if (globals.Debug) Debug.Log("adding city encounter");
                        break;
                }
            }

            // Every X amount of seconds, where X is encounterRate, send an encounter towards the player.
            encounterTimer -= Time.deltaTime;
            if (encounterTimer <= 0)
            {
                encounterTimer = encounterRate;
                encounterQueue.Peek().GetComponent<Encounter>().Send = true;
            }

            foreach (GameObject encounter in encounterQueue)
            {
                // Send encounters to the right, when they hit x = 10 (about halfway into the player) trigger the encounter.
                if (encounter.GetComponent<Encounter>().Send)
                {
                    Vector3 moving = encounter.transform.localPosition;
                    moving.x += encounterMoveSpeed * Time.deltaTime;
                    encounter.transform.localPosition = moving;
                }
                if (encounterQueue.Peek().transform.localPosition.x >= 10.0f)
                {
                    if (globals.Debug) Debug.Log("Hey it touched.");
                    globals.CurrentState = GameState.Encounter;
                    triggerEncounter(encounterQueue.Peek().GetComponent<Encounter>());
                }
            }
            
            if (player.GetComponent<Player>().Stats.HP <= 0)
            {
                globals.CurrentState = GameState.MainMenu;
            }
        }
    }

    /// <summary>
    /// Gets an encounter of a specified type. Copied from Up the Water Spout.
    /// </summary>
    /// <param name="type">The specified EncounterType</param>
    /// <returns>The encounter GameObject.</returns>
    private GameObject getEncounter(EncounterType type)
    {
        GameObject encounter;
        
        // check if pool has encounters of type type
        if (hasType(type))
        {
            // if yes, grabs first one
            // if it is already active, makes a new one.
            // If not, makes it active.
            encounter = this.transform.GetChild(getFirstOfType(type)).gameObject;
            if (encounter.activeInHierarchy)
            {
                encounter = Instantiate(prefabList[typeToIndex(type)], new Vector3(this.transform.position.x, this.transform.position.y, 93.7f), Quaternion.identity);
                encounter.transform.SetParent(this.transform);
            }
            else
            {
                encounter.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 93.7f);
                encounter.SetActive(true);
            }
        }
        // if no encounters in pool, makes a new one
        else
        {
            encounter = Instantiate(prefabList[typeToIndex(type)], new Vector3(this.transform.position.x, this.transform.position.y, 93.7f), Quaternion.identity);
            encounter.transform.SetParent(this.transform);
        }

        // brings encounter to the end of the encounter pool.
        encounter.transform.SetAsLastSibling();
        encounter.GetComponent<Encounter>().player = player.GetComponent<Player>();
        return encounter;
    }

    /// <summary>
    /// Sets the encounter stage and fills the canvas with information.
    /// </summary>
    /// <param name="encounter">Encounter to use</param>
    private void triggerEncounter(Encounter encounter)
    {
        // Clears any previous data
        menuData.Clear();
        resultingText.Clear();
        statChanges.Clear();

        // Get new data
        (string, List<(string, string)>, List<string>, List<Stats>) data = encounter.GetComponent<Encounter>().encounterInfo();

        text.text = data.Item1;
        menuData = data.Item2;
        resultingText = data.Item3;
        statChanges = data.Item4;
        menu1.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = menuData[0].Item1;
        menu1.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = menuData[0].Item2;
        menu1.SetActive(true);

        if (menuData.Count > 1)
        {
            menu2.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = menuData[1].Item1;
            menu2.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = menuData[1].Item2;
            menu2.SetActive(true);
        }
        if (menuData.Count > 2)
        {
            menu3.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = menuData[2].Item1;
            menu3.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = menuData[2].Item2;
            menu3.SetActive(true);
        }
        if (menuData.Count > 3)
        {
            menu4.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = menuData[3].Item1;
            menu4.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = menuData[3].Item2;
            menu4.SetActive(true);
        }
        
    }

    /// <summary>
    /// Checks if the pool has an encounter of a specified type
    /// </summary>
    /// <param name="type">The specified type</param>
    /// <returns>True if there is an event, false if not.</returns>
    private bool hasType(EncounterType type)
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).GetComponent<Encounter>().Type == type)
                return true;
        }
        return false;
    }
    /// <summary>
    /// To be used in conjunction with hasType(). Returns the index of the first child of a specified type.
    /// </summary>
    /// <param name="type">Type to search for.</param>
    /// <returns>The index of the first child of the specified type. Returns -1 if there is none of type.</returns>
    private int getFirstOfType(EncounterType type)
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).GetComponent<Encounter>().Type == type)
                return i;
        }
        return -1;
    }

    /// <summary>
    /// Returns the respective index of the prefab list based on the given type.
    /// </summary>
    /// <param name="type">Encounter type</param>
    /// <returns>Index of the specified type in the prefab list.</returns>
    private int typeToIndex(EncounterType type)
    {
        switch(type)
        {
            case EncounterType.Attack:
                return 1;
            case EncounterType.CityTown:
                return 2;
            default:
                return 0;
        }
    }
    /// <summary>
    /// Returns the respective type of encounter based on the given index.
    /// </summary>
    /// <param name="index">Index in prefab list</param>
    /// <returns>Encounter type.</returns>
    private EncounterType indexToType(int index)
    {
        switch (index)
        {
            case 1:
                return EncounterType.Attack;
            case 2:
                return EncounterType.CityTown;
            default:
                return EncounterType.Test;
        }
    }

    // OnClick events, each one corresponds to a specified action.
    // When clicked, affect the player's stats according to the encounter action.
    // After, hide all menu buttons and change the game state back to game.
    public void OnClick1()
    {
        player.GetComponent<Player>().Stats = statChanges[0];
        menu1.SetActive(false);
        menu2.SetActive(false);
        menu3.SetActive(false);
        menu4.SetActive(false);
        text.text = resultingText[0];
        globals.CurrentState = GameState.Game;
        encounterQueue.Dequeue().SetActive(false);
    }
    public void OnClick2()
    {
        player.GetComponent<Player>().Stats = statChanges[1];
        menu1.SetActive(false);
        menu2.SetActive(false);
        menu3.SetActive(false);
        menu4.SetActive(false);
        text.text = resultingText[1];
        globals.CurrentState = GameState.Game;
        encounterQueue.Dequeue().SetActive(false);
    }
    public void OnClick3()
    {
        player.GetComponent<Player>().Stats = statChanges[2];
        menu1.SetActive(false);
        menu2.SetActive(false);
        menu3.SetActive(false);
        menu4.SetActive(false);
        text.text = resultingText[2];
        globals.CurrentState = GameState.Game;
        encounterQueue.Dequeue().SetActive(false);
    }
    public void OnClick4()
    {
        player.GetComponent<Player>().Stats = statChanges[3];
        menu1.SetActive(false);
        menu2.SetActive(false);
        menu3.SetActive(false);
        menu4.SetActive(false);
        text.text = resultingText[3];
        globals.CurrentState = GameState.Game;
        encounterQueue.Dequeue().SetActive(false);
    }
}
