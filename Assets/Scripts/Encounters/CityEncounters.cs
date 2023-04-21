using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class CityEncounters : Encounters
{
    public CityEncounters(Stats playerStats)
    {
        UpdateStats(playerStats);
        descText = "";
        choices = new List<(string, string)>();
        results = new List<string>();
        statChanges = new List<Stats>();

        switch (Random.Range(0, 6))
        {
            case 0:// Desert City
                {
                    descText = "You encounter a desert city.";
                    choices.Add(("Practice Fighting.", ""));
                    int chance = Random.Range(0, 100);
                    if (chance < 75)
                    {
                        int strengthGain = Random.Range(1, 3);
                        results.Add($"You practice your fighting technique and permanently gain +{strengthGain} strength.");
                        Stats strengthChange = new Stats(stats);
                        strengthChange.Strength += strengthGain;
                        statChanges.Add(strengthChange);
                    }
                    else
                    {
                        results.Add($"Your training bears no fruit and you end up injuring yourself. You take 3 damage.");
                        Stats hpChange = new Stats(stats);
                        hpChange.HP -= 3;
                        statChanges.Add(hpChange);
                    }

                    choices.Add(("Find a place to rest.", ""));
                    if (chance < 80)
                    {
                        results.Add("You find a nice inn. You restore 5 HP and stamina and are ready to continue your journey.");
                        Stats hpStaminaChange = new Stats(stats);
                        hpStaminaChange.HP += 5;
                        hpStaminaChange.Stamina += 5;
                        statChanges.Add(hpStaminaChange);
                    }
                    else
                    {
                        results.Add("Your search bears no fruit and you lose sleep in order to protect yourself and your belongings. You lose 3 Stamina.");
                        Stats staminaChange = new Stats(stats);
                        staminaChange.Stamina -= 3;
                        statChanges.Add(staminaChange);
                    }

                    choices.Add(("Study the local lands.", ""));
                    if (chance < 90)
                    {
                        int knowledgeGain = Random.Range(1, 3);
                        results.Add($"You make notes about your environment. Permanently gain +{knowledgeGain} Knowledge.");
                        Stats knowledgeChange = new Stats(stats);
                        knowledgeChange.Knowledge += knowledgeGain;
                        statChanges.Add(knowledgeChange);
                    }
                    else
                    {
                        results.Add($"You fail to learn anything new.");
                        Stats noChange = new Stats(stats);
                        statChanges.Add(noChange);
                    }
                }
                break;
            case 1:// Forest City
                {
                    descText = "You encounter a forest city.";
                    choices.Add(("Practice Fighting.", ""));
                    int chance = Random.Range(0, 100);
                    if (chance < 100)
                    {
                        int strengthGain = Random.Range(1, 3);
                        results.Add($"You practice your fighting technique and permanently gain +{strengthGain} strength.");
                        Stats strengthChange = new Stats(stats);
                        strengthChange.Strength += strengthGain;
                        statChanges.Add(strengthChange);
                    }
                    else
                    {
                        results.Add($"Your training bears no fruit and you end up injuring yourself. You take 3 damage.");
                        Stats hpChange = new Stats(stats);
                        hpChange.HP -= 3;
                        statChanges.Add(hpChange);
                    }

                    choices.Add(("Find a place to rest.", ""));
                    if (chance < 100)
                    {
                        results.Add("You find a nice inn. You restore 5 HP and stamina and are ready to continue your journey.");
                        Stats hpStaminaChange = new Stats(stats);
                        hpStaminaChange.HP += 5;
                        hpStaminaChange.Stamina += 5;
                        statChanges.Add(hpStaminaChange);
                    }
                    else
                    {
                        results.Add("Your search bears no fruit and you lose sleep in order to protect yourself and your belongings. You lose 3 Stamina.");
                        Stats staminaChange = new Stats(stats);
                        staminaChange.Stamina -= 3;
                        statChanges.Add(staminaChange);
                    }

                    choices.Add(("Study the local lands.", ""));
                    if (chance < 90)
                    {
                        int intuitionGain = Random.Range(1, 3);
                        results.Add($"You make notes about your environment. Permanently gain +{intuitionGain} Intuition.");
                        Stats intuitionChange = new Stats(stats);
                        intuitionChange.Knowledge += intuitionGain;
                        statChanges.Add(intuitionChange);
                    }
                    else
                    {
                        results.Add($"You fail to learn anything new.");
                        Stats noChange = new Stats(stats);
                        statChanges.Add(noChange);
                    }
                }
                break;
            case 2:// Mountain City
                {
                    descText = "You encounter a mountain city.";
                    choices.Add(("Practice Fighting.", ""));
                    int chance = Random.Range(0, 100);
                    if (chance < 80)
                    {
                        int strengthGain = Random.Range(1, 3);
                        results.Add($"You practice your fighting technique and permanently gain +{strengthGain} strength.");
                        Stats strengthChange = new Stats(stats);
                        strengthChange.Strength += strengthGain;
                        statChanges.Add(strengthChange);
                    }
                    else
                    {
                        results.Add($"Your training bears no fruit and you end up injuring yourself. You take 3 damage.");
                        Stats hpChange = new Stats(stats);
                        hpChange.HP -= 3;
                        statChanges.Add(hpChange);
                    }

                    choices.Add(("Find a place to rest.", ""));
                    if (chance < 75)
                    {
                        results.Add("You find a nice inn. You restore 5 HP and stamina and are ready to continue your journey.");
                        Stats hpStaminaChange = new Stats(stats);
                        hpStaminaChange.HP += 5;
                        hpStaminaChange.Stamina += 5;
                        statChanges.Add(hpStaminaChange);
                    }
                    else
                    {
                        results.Add("Your search bears no fruit and you lose sleep in order to protect yourself and your belongings. You lose 3 Stamina.");
                        Stats staminaChange = new Stats(stats);
                        staminaChange.Stamina -= 3;
                        statChanges.Add(staminaChange);
                    }

                    choices.Add(("Study the local lands.", ""));
                    if (chance < 90)
                    {
                        int knowledgeGain = Random.Range(1, 4);
                        results.Add($"You make notes about your environment. Permanently gain +{knowledgeGain} Knowledge.");
                        Stats knowledgeChange = new Stats(stats);
                        knowledgeChange.Knowledge += knowledgeGain;
                        statChanges.Add(knowledgeChange);
                    }
                    else
                    {
                        results.Add($"You fail to learn anything new.");
                        Stats noChange = new Stats(stats);
                        statChanges.Add(noChange);
                    }
                }
                break;
            case 3:// Desolate City
                {
                    descText = "You encounter a desolate city.";
                    choices.Add(("Practice Fighting.", ""));
                    int chance = Random.Range(0, 100);
                    if (chance < 25)
                    {
                        int strengthGain = Random.Range(1, 3);
                        results.Add($"You practice your fighting technique and permanently gain +{strengthGain} strength.");
                        Stats strengthChange = new Stats(stats);
                        strengthChange.Strength += strengthGain;
                        statChanges.Add(strengthChange);
                    }
                    else
                    {
                        results.Add($"Your training bears no fruit and you end up injuring yourself. You take 3 damage.");
                        Stats hpChange = new Stats(stats);
                        hpChange.HP -= 3;
                        statChanges.Add(hpChange);
                    }

                    choices.Add(("Find a place to rest.", ""));
                    if (chance < 50)
                    {
                        results.Add("You find an alright inn. You restore 5 HP and stamina and are ready to continue your journey.");
                        Stats hpStaminaChange = new Stats(stats);
                        hpStaminaChange.HP += 5;
                        hpStaminaChange.Stamina += 5;
                        statChanges.Add(hpStaminaChange);
                    }
                    else
                    {
                        results.Add("Your search bears no fruit and you lose sleep in order to protect yourself and your belongings. You lose 3 Stamina.");
                        Stats staminaChange = new Stats(stats);
                        staminaChange.Stamina -= 3;
                        statChanges.Add(staminaChange);
                    }

                    choices.Add(("Study the local lands.", ""));
                    if (chance < 90)
                    {
                        results.Add("You make notes about your environment. Permanently gain +1 Knowledge and Intuition.");
                        Stats knowledgeIntuitionChange = new Stats(stats);
                        knowledgeIntuitionChange.Knowledge++;
                        knowledgeIntuitionChange.Intuition++;
                        statChanges.Add(knowledgeIntuitionChange);
                    }
                    else
                    {
                        results.Add($"You fail to learn anything new.");
                        Stats noChange = new Stats(stats);
                        statChanges.Add(noChange);
                    }
                }
                break;
            case 4:// Lakeside City
                {
                    descText = "You encounter a lakeside city.";
                    choices.Add(("Practice Fighting.", ""));
                    int chance = Random.Range(0, 100);
                    if (chance < 90)
                    {
                        int strengthGain = Random.Range(1, 3);
                        results.Add($"You practice your fighting technique and permanently gain +{strengthGain} strength.");
                        Stats strengthChange = new Stats(stats);
                        strengthChange.Strength += strengthGain;
                        statChanges.Add(strengthChange);
                    }
                    else
                    {
                        results.Add($"Your training bears no fruit and you end up injuring yourself. You take 3 damage.");
                        Stats hpChange = new Stats(stats);
                        hpChange.HP -= 3;
                        statChanges.Add(hpChange);
                    }

                    choices.Add(("Find a place to rest.", ""));
                    if (chance < 100)
                    {
                        results.Add("You find a nice inn. You restore 5 HP and stamina and are ready to continue your journey.");
                        Stats hpStaminaChange = new Stats(stats);
                        hpStaminaChange.HP += 5;
                        hpStaminaChange.Stamina += 5;
                        statChanges.Add(hpStaminaChange);
                    }
                    else
                    {
                        results.Add("Your search bears no fruit and you lose sleep in order to protect yourself and your belongings. You lose 3 Stamina.");
                        Stats staminaChange = new Stats(stats);
                        staminaChange.Stamina -= 3;
                        statChanges.Add(staminaChange);
                    }

                    choices.Add(("Study the local lands.", ""));
                    if (chance < 90)
                    {
                        int knowledgeGain = Random.Range(1, 4);
                        results.Add($"You make notes about your environment. Permanently gain +{knowledgeGain} Knowledge.");
                        Stats knowledgeChange = new Stats(stats);
                        knowledgeChange.Knowledge += knowledgeGain;
                        statChanges.Add(knowledgeChange);
                    }
                    else
                    {
                        results.Add($"You fail to learn anything new.");
                        Stats noChange = new Stats(stats);
                        statChanges.Add(noChange);
                    }
                }
                break;
            case 5:// Oceanside City
                {
                    descText = "You encounter an oceanside city.";
                    choices.Add(("Practice Fighting.", ""));
                    int chance = Random.Range(0, 100);
                    if (chance < 100)
                    {
                        int strengthGain = Random.Range(1, 3);
                        results.Add($"You practice your fighting technique and permanently gain +{strengthGain} strength.");
                        Stats strengthChange = new Stats(stats);
                        strengthChange.Strength += strengthGain;
                        statChanges.Add(strengthChange);
                    }
                    else
                    {
                        results.Add($"Your training bears no fruit and you end up injuring yourself. You take 3 damage.");
                        Stats hpChange = new Stats(stats);
                        hpChange.HP -= 3;
                        statChanges.Add(hpChange);
                    }

                    choices.Add(("Find a place to rest.", ""));
                    if (chance < 100)
                    {
                        results.Add("You find a nice inn. You restore 5 HP and stamina and are ready to continue your journey.");
                        Stats hpStaminaChange = new Stats(stats);
                        hpStaminaChange.HP += 5;
                        hpStaminaChange.Stamina += 5;
                        statChanges.Add(hpStaminaChange);
                    }
                    else
                    {
                        results.Add("Your search bears no fruit and you lose sleep in order to protect yourself and your belongings. You lose 3 Stamina.");
                        Stats staminaChange = new Stats(stats);
                        staminaChange.Stamina -= 3;
                        statChanges.Add(staminaChange);
                    }

                    choices.Add(("Study the local lands.", ""));
                    if (chance < 90)
                    {
                        int intuitionGain = Random.Range(1, 3);
                        results.Add($"You make notes about your environment. Permanently gain +{intuitionGain} Intuition.");
                        Stats intuitionChange = new Stats(stats);
                        intuitionChange.Knowledge += intuitionGain;
                        statChanges.Add(intuitionChange);
                    }
                    else
                    {
                        results.Add($"You fail to learn anything new.");
                        Stats noChange = new Stats(stats);
                        statChanges.Add(noChange);
                    }
                }
                break;
            default:
                descText = "Something went wrong, check random range in city encounters class.";
                break;
        }
    }
}