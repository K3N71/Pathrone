using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Encounter : MonoBehaviour
{
    [SerializeField, Description("Type of Encounter.")]
    private EncounterType type;
    private bool send = false;
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

    // Update is called once per frame
    void Update()
    {
    }

}
