using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    [Header("Reference to the Objects.")]
    [SerializeField, Description("Globals Object")]
    private Globals globals;
    [SerializeField, Description("Player Object")]
    private GameObject player;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (encounterQueue.Count < 5)
        {
            encounterQueue.Enqueue(getEncounter(EncounterType.Test));
        }

        if (globals.CurrentState == GameState.Game)
        {
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
                    Vector3 moving = encounter.transform.position;
                    moving.x += encounterMoveSpeed * Time.deltaTime;
                    encounter.transform.position = moving;
                }
                if (encounterQueue.Peek().GetComponent<BoxCollider2D>().IsTouching(player.GetComponent<BoxCollider2D>()))
                {
                    globals.CurrentState = GameState.Encounter;

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
                encounter = Instantiate(prefabList[typeToIndex(type)], new Vector3(this.transform.position.x, this.transform.position.y, -0.5f), Quaternion.identity);
                encounter.transform.SetParent(this.transform);
            }
            else
            {
                encounter.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -0.5f);
                encounter.SetActive(true);
            }
        }
        // if no droplets in pool, makes a new one
        else
        {
            encounter = Instantiate(prefabList[typeToIndex(type)], new Vector3(this.transform.position.x, this.transform.position.y, -0.5f), Quaternion.identity);
            encounter.transform.SetParent(this.transform);
        }

        // brings current droplet to the end of the droplet list
        encounter.transform.SetAsLastSibling();
        return encounter;
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
}
