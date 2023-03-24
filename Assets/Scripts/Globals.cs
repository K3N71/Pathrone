using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

/// <summary>
/// Has variables that many classes and scripts uses.
/// Author: Ken Adachi-Bartholomay
/// Last edited: 3/23/23
/// </summary>
public class Globals : MonoBehaviour
{
    [Header("DO NOT CHANGE CURRENTSTATE DURING PLAY")]
    [SerializeField] // doing read only requires a whole work around that i don't feel like doing
    private GameState currentState = GameState.MainMenu;
    [SerializeField]
    private ScreenMode currentMode = ScreenMode.Landscape;

    [SerializeField, Description("If you need, do an If(Globals.Debug) and check this to print debug info.")]
    public bool Debug = true;

    public GameState CurrentState 
    { 
        get { return currentState; }
        set { currentState = value; }
    }
    public ScreenMode CurrentMode
    {
        get { return currentMode; }
        set { currentMode = value; }
    }
}

public enum GameState
{
    MainMenu,
    Game,
    Encounter,
    Pause
}
public enum ScreenMode
{
    Landscape,
    Portait
}

public enum EncounterType
{
    CityTown,
    Caravan,
    Attack
}