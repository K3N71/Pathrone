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
    [SerializeField, Description("Menu Object 1")]
    private GameObject menu1;
    [SerializeField, Description("Menu Object 2")]
    private GameObject menu2;
    [SerializeField, Description("Menu Object 3")]
    private GameObject menu3;
    [SerializeField, Description("Menu Object 4")]
    private GameObject menu4;
    private List<string> menuData = new List<string>();
    private List<int> statChanges = new List<int>();

    [Header("Encounter Variables.")]
    [SerializeField, Description("Frequency of Encounters.")]
    private float encounterRate = 12.0f;
    private float encounterTimer;
    [SerializeField, Description("Encounter move speed.")]
    private float encounterMoveSpeed = 10.0f;

    [SerializeField, Description("Encounter Queue")]
    private Queue<GameObject> encounterQueue = new Queue<GameObject>();

    [Header("Encounter Prefabs")]
    [SerializeField, Description("Encounter Prefabs")]
    private List<GameObject> prefabList = new List<GameObject>();

    

    // Start is called before the first frame update
    void Start()
    {
        encounterTimer = encounterRate;
        menu1.SetActive(false);
        menu2.SetActive(false);
        menu3.SetActive(false);
        menu4.SetActive(false);
        text.text = "Welcome to our game, Pathrone! This is a proof of concept MVI. When the grey square hits your cube, you will enter an encounter.";
    }

    // Update is called once per frame
    void Update()
    { 
        if (globals.CurrentState == GameState.Game)
        {
            if (encounterQueue.Count < 5)
            {
                encounterQueue.Enqueue(getEncounter(EncounterType.Test));
            }
            encounterTimer -= Time.deltaTime;
            if (encounterTimer <= 0)
            {
                encounterTimer = encounterRate;
                encounterQueue.Peek().GetComponent<Encounter>().Send = true;
            }

            foreach (GameObject encounter in encounterQueue)
            {
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
            
        }
    }

    /// <summary>
    /// Gets an encounter of a specified type.
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
            // If not, makes it active and drops it from the top
            encounter = this.transform.GetChild(0).gameObject;
            if (encounter.activeInHierarchy)
            {
                encounter = Instantiate(prefabList[typeToIndex(type)], new Vector3(this.transform.position.x, this.transform.position.y, 0.0f), Quaternion.identity);
                encounter.transform.SetParent(this.transform);
            }
            else
            {
                encounter.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0.0f);
                encounter.SetActive(true);
            }
        }
        // if no encounters in pool, makes a new one
        else
        {
            encounter = Instantiate(prefabList[typeToIndex(type)], new Vector3(this.transform.position.x, this.transform.position.y, 0.0f), Quaternion.identity);
            encounter.transform.SetParent(this.transform);
        }

        // brings encounter to the end of the encounter list
        encounter.transform.SetAsLastSibling();
        encounter.GetComponent<Encounter>().getStats(player.GetComponent<Player>());
        return encounter;
    }

    private void triggerEncounter(Encounter encounter)
    {
        menuData.Clear();
        statChanges.Clear();
        encounter.GetComponent<Encounter>().getStats(player.GetComponent<Player>());
        (string, List<string>, List<int>) data = encounter.GetComponent<Encounter>().encounterInfo();
        if (data.Item2.Count != data.Item3.Count || data.Item2.Count % 2 != 0 || data.Item3.Count % 2 != 0)
        {
            text.text = "Something went wrong and an event was formatted improperly.";
            menu1.SetActive(true);
            menu1.transform.GetChild(0).GetComponent<Button>().GetComponent<TextMeshProUGUI>().text = "Exit";
            menu1.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        }
        else
        {
            text.text = data.Item1;
            menuData = data.Item2;
            statChanges = data.Item3;
            menu1.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = menuData[0];
            menu1.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = menuData[1];
            menu1.SetActive(true);

            if (menuData.Count > 2)
            {
                menu2.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = menuData[2];
                menu2.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = menuData[3];
                menu2.SetActive(true);
            }
            if (menuData.Count > 4)
            {
                menu3.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = menuData[4];
                menu3.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = menuData[5];
                menu3.SetActive(true);
            }
            if (menuData.Count > 6)
            {
                menu4.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = menuData[6];
                menu4.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = menuData[7];
                menu4.SetActive(true);
            }
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
    /// Returns the respective index of the prefab list based on the given type.
    /// </summary>
    /// <param name="type">Encounter type</param>
    /// <returns>Index of the specified type in the prefab list.</returns>
    private int typeToIndex(EncounterType type)
    {
        switch(type)
        {
            case EncounterType.Test:
                return 0;
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
            case 0:
                return EncounterType.Test;
            default:
                return EncounterType.Test;
        }
    }

    public void OnClick1()
    {
        player.GetComponent<Player>().HP += statChanges[0];
        player.GetComponent<Player>().Stamina += statChanges[1];
        menu1.SetActive(false);
        menu2.SetActive(false);
        menu3.SetActive(false);
        menu4.SetActive(false);
        globals.CurrentState = GameState.Game;
        encounterQueue.Dequeue().SetActive(false);
    }
    public void OnClick2()
    {
        player.GetComponent<Player>().HP += statChanges[2];
        player.GetComponent<Player>().Stamina += statChanges[3];
        menu1.SetActive(false);
        menu2.SetActive(false);
        menu3.SetActive(false);
        menu4.SetActive(false);
        globals.CurrentState = GameState.Game;
        encounterQueue.Dequeue().SetActive(false);
    }
    public void OnClick3()
    {
        player.GetComponent<Player>().HP += statChanges[4];
        player.GetComponent<Player>().Stamina += statChanges[5];
        menu1.SetActive(false);
        menu2.SetActive(false);
        menu3.SetActive(false);
        menu4.SetActive(false);
        globals.CurrentState = GameState.Game;
        encounterQueue.Dequeue().SetActive(false);
    }
    public void OnClick4()
    {
        player.GetComponent<Player>().HP += statChanges[6];
        player.GetComponent<Player>().Stamina += statChanges[7];
        menu1.SetActive(false);
        menu2.SetActive(false);
        menu3.SetActive(false);
        menu4.SetActive(false);
        globals.CurrentState = GameState.Game;
        encounterQueue.Dequeue().SetActive(false);
    }
}
