using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackEncounters : Encounters
{
    public AttackEncounters(Stats playerStats)
    {
        seed = rand.Next(5);
        check = (float)rand.NextDouble();
        UpdateStats(playerStats);
    }

    protected override void generateEncounter()
    {
        descText = "";
        choices = new List<(string, string)>();
        results = new List<string>();
        statChanges = new List<Stats>();
        switch (seed)
        {
            case 0:// strong enemy attacks
                {
                    descText = "Strong enemy attacks.";
                    choices.Add(("Push it aside.", ""));
                    int strengthCheck = 7 + Mathf.RoundToInt(8 * check);
                    if (stats.Strength > strengthCheck)
                    {
                        results.Add("You succeed in defeating the strong enemy, but it took some effort. -3 Stamina");
                        Stats staminaChange = new Stats(stats);
                        staminaChange.Stamina -= 3;
                        statChanges.Add(staminaChange);
                    }
                    else
                    {
                        results.Add($"You fail to push it aside and the enemy throws you a large distance. You take {strengthCheck - stats.Strength} damage.");
                        Stats hpChange = new Stats(stats);
                        hpChange.HP -= (strengthCheck - stats.Strength);
                        statChanges.Add(hpChange);
                    }

                    choices.Add(("Dodge the attack.", ""));
                    int intuitionLuckCheck = 1 + Mathf.RoundToInt(7 * check);
                    if (stats.Intuition + stats.Luck > intuitionLuckCheck)
                    {
                        results.Add("You dodge its attack and quickly escape.");
                        Stats noChange = new Stats(stats);
                        statChanges.Add(noChange);
                    }
                    else
                    {
                        results.Add($"You fail to dodge the attack and the enemy grazes you. You manage to flee afterwards, but you take 5 damage.");
                        Stats hpChange = new Stats(stats);
                        hpChange.HP -= 5;
                        statChanges.Add(hpChange);
                    }

                    choices.Add(("Try and find a weak spot.", ""));
                    int knowledgeIntuitionCheck = 1 + Mathf.RoundToInt(11 * check);
                    if (stats.Knowledge + stats.Intuition > knowledgeIntuitionCheck)
                    {
                        results.Add("You find its weakness and swiftly take it down. Exploiting the weakness permanently earns you +1 Knowledge and Intuition.");
                        Stats knowledgeIntuitionChange = new Stats(stats);
                        knowledgeIntuitionChange.Knowledge++;
                        knowledgeIntuitionChange.Intuition++;
                        statChanges.Add(knowledgeIntuitionChange);
                    }
                    else
                    {
                        results.Add($"You fail to find the enemy's weakness in time and get hit with a huge tackle. You take 10 damage.");
                        Stats hpChange = new Stats(stats);
                        hpChange.HP -= 10;
                        statChanges.Add(hpChange);
                    }
                }
                break;

            case 1: // smart enemy attacks
                {
                    descText = "Smart enemy attacks.";
                    choices.Add(("Push it aside.", ""));
                    int strengthCheck = 1 + Mathf.RoundToInt(7 * check);
                    if (stats.Strength > strengthCheck)
                    {
                        results.Add("You succeed in defeating the smart enemy, but it took some effort. -3 Stamina");
                        Stats staminaChange = new Stats(stats);
                        staminaChange.Stamina -= 3;
                        statChanges.Add(staminaChange);
                    }
                    else
                    {
                        results.Add($"You fail to push it aside and the enemy knocks you over. You take {strengthCheck - stats.Strength / 2} damage.");
                        Stats hpChange = new Stats(stats);
                        hpChange.HP -= (strengthCheck - stats.Strength / 2);
                        statChanges.Add(hpChange);
                    }

                    choices.Add(("Dodge the attack.", ""));
                    int intuitionLuckCheck = 10 + Mathf.RoundToInt(10 * check);
                    if (stats.Intuition + stats.Luck > intuitionLuckCheck)
                    {
                        results.Add("You dodge its attack and quickly escape.");
                        Stats noChange = new Stats(stats);
                        statChanges.Add(noChange);
                    }
                    else
                    {
                        results.Add($"Your enemy reads your dodge and exploits it. You manage to flee afterwards, but you take {5 + (intuitionLuckCheck - stats.Knowledge)} damage.");
                        Stats hpChange = new Stats(stats);
                        hpChange.HP -= 5 + (intuitionLuckCheck - stats.Knowledge);
                        statChanges.Add(hpChange);
                    }

                    choices.Add(("Try and find a weak spot.", ""));
                    int knowledgeIntuitionCheck = 6 + Mathf.RoundToInt(12 * check);
                    if (stats.Knowledge + stats.Intuition > knowledgeIntuitionCheck)
                    {
                        results.Add("You find its weakness and swiftly take it down. Exploiting the weakness permanently earns you +1 Knowledge and Intuition.");
                        Stats knowledgeIntuitionChange = new Stats(stats);
                        knowledgeIntuitionChange.Knowledge++;
                        knowledgeIntuitionChange.Intuition++;
                        statChanges.Add(knowledgeIntuitionChange);
                    }
                    else
                    {
                        results.Add($"You fail to find the enemy's weakness in time and hurt. You take 5 damage.");
                        Stats hpChange = new Stats(stats);
                        hpChange.HP -= 5;
                        statChanges.Add(hpChange);
                    }
                }
                break;
            case 2: // multiple enemies attacks
                {
                    descText = "Multiple enemies attack.";
                    choices.Add(("Outlast the onslaught.", ""));
                    int strengthCheck = 5 + Mathf.RoundToInt(10 * check);
                    if (stats.Strength > strengthCheck)
                    {
                        results.Add("You succeed in defending against the enemies, but it took some effort. -3 Stamina");
                        Stats staminaChange = new Stats(stats);
                        staminaChange.Stamina -= 3;
                        statChanges.Add(staminaChange);
                    }
                    else
                    {
                        results.Add($"You fail to survive the onslaught and end up fleeing with your life. You take {strengthCheck - stats.Strength} damage.");
                        Stats hpChange = new Stats(stats);
                        hpChange.HP -= (strengthCheck - stats.Strength);
                        statChanges.Add(hpChange);
                    }

                    choices.Add(("Roll out of the way.", ""));
                    int intuitionLuckCheck = 10 + Mathf.RoundToInt(8 * check);
                    if (stats.Intuition + stats.Luck > intuitionLuckCheck)
                    {
                        results.Add("You dodge the attack and quickly escape.");
                        Stats noChange = new Stats(stats);
                        statChanges.Add(noChange);
                    }
                    else
                    {
                        results.Add($"You bump into one of the enemies and take a blow. You end up escaping in the end, but you take {5 + (intuitionLuckCheck - stats.Intuition)} damage.");
                        Stats hpChange = new Stats(stats);
                        hpChange.HP -= 5 + (intuitionLuckCheck - stats.Intuition);
                        statChanges.Add(hpChange);
                    }

                    choices.Add(("Look around for cover.", ""));
                    int knowledgeIntuitionCheck = 3 + Mathf.RoundToInt(12 * check);
                    if (stats.Knowledge + stats.Intuition > knowledgeIntuitionCheck)
                    {
                        results.Add("You find cover and it greatly assists in your fight. Making the most out of your surroundings permanently earns you +1 Knowledge and Intuition.");
                        Stats knowledgeIntuitionChange = new Stats(stats);
                        knowledgeIntuitionChange.Knowledge++;
                        knowledgeIntuitionChange.Intuition++;
                        statChanges.Add(knowledgeIntuitionChange);
                    }
                    else
                    {
                        results.Add($"You fail to look for cover and take 5 damage.");
                        Stats hpChange = new Stats(stats);
                        hpChange.HP -= 5;
                        statChanges.Add(hpChange);
                    }
                }
                break;
            case 3: // disguised enemy attacks
                {
                    descText = "Disguised enemy approaches!";
                    choices.Add(("Take off their mask.", ""));
                    int strengthLuckCheck = 5 + Mathf.RoundToInt(10 * check);
                    if (stats.Strength + stats.Luck > strengthLuckCheck)
                    {
                        results.Add("You succeed in taking off the enemy's mask and they flee to protect their identity.");
                        Stats noChange = new Stats(stats);
                        statChanges.Add(noChange);
                    }
                    else
                    {
                        results.Add($"You fail to take off their mask and they retaliate against you for trying. You take {5 + (strengthLuckCheck - stats.Luck)} damage before fleeing.");
                        Stats hpChange = new Stats(stats);
                        hpChange.HP -= (5 + (strengthLuckCheck - stats.Luck));
                        statChanges.Add(hpChange);
                    }

                    choices.Add(("Just walk away.", ""));
                    int luckCheck = 1 + Mathf.RoundToInt(7 * check);
                    if (stats.Luck > luckCheck)
                    {
                        results.Add("They fail to recognize you and walk in the other direction.");
                        Stats noChange = new Stats(stats);
                        statChanges.Add(noChange);
                    }
                    else
                    {
                        results.Add("They stalk you before launching their suprise attack. You take 5 damage before fleeing the rest of their attack.");
                        Stats hpChange = new Stats(stats);
                        hpChange.HP -= 5;
                        statChanges.Add(hpChange);
                    }

                    choices.Add(("Challenge them to a duel.", ""));
                    int intuitionLuckCheck = 10 + Mathf.RoundToInt(10 * check);
                    if (stats.Intuition + stats.Luck > intuitionLuckCheck)
                    {
                        results.Add("You succeed in your challenge and make a fool out of the enemy. The amazing performance you displayed permanently earns you +1 Strength, Knowledge, and Luck.");
                        Stats strengthKnowledgeLuckChange = new Stats(stats);
                        strengthKnowledgeLuckChange.Strength++;
                        strengthKnowledgeLuckChange.Knowledge++;
                        strengthKnowledgeLuckChange.Luck++;
                        statChanges.Add(strengthKnowledgeLuckChange);
                    }
                    else
                    {
                        results.Add("The enemy throws a smoke bomb at you and flees from your challenge. You take 5 damage.");
                        Stats hpChange = new Stats(stats);
                        hpChange.HP -= 5;
                        statChanges.Add(hpChange);
                    }
                }
                break;
            case 4: // RPS
                {
                    descText = "A man approaches offering to play rock, paper, scissors.";
                    choices.Add(("Play Strong Rock.", ""));
                    choices.Add(("Play Smart Scissors.", ""));
                    choices.Add(("Play Sensing Paper.", ""));
                    switch (Mathf.RoundToInt(3 * check))
                    {
                        case 0: // rock win, scissor ties, paper loses
                            {
                                results.Add("Your rock decimates the man's scissors. Your victory permanently earns you +2 Strength.");
                                Stats strengthChange = new Stats(stats);
                                strengthChange.Strength += 2;
                                statChanges.Add(strengthChange);
                                results.Add("You pick scissors and tie with the man's scissors. You both walk away indifferent.");
                                Stats noChange = new Stats(stats);
                                statChanges.Add(noChange);
                                results.Add("Your paper gets ripped to shreads by the man's scissors. You take 5 damage.");
                                Stats hpChange = new Stats(stats);
                                hpChange.HP -= 5;
                                statChanges.Add(hpChange);
                            }
                            break;
                        case 1:// rock loses, scissor wins, paper ties
                            {
                                results.Add("Your rock gets suffocated by the man's paper. You take 5 damage.");
                                Stats hpChange = new Stats(stats);
                                hpChange.HP -= 5;
                                statChanges.Add(hpChange);
                                results.Add("Your scissors rip the man's paper to shreads. Your victory permanently earns you +2 Knowledge.");
                                Stats knowledgeChange = new Stats(stats);
                                knowledgeChange.Knowledge += 2;
                                statChanges.Add(knowledgeChange);
                                results.Add("You pick paper and tie with the man's paper. You both walk away indifferent.");
                                Stats noChange = new Stats(stats);
                                statChanges.Add(noChange);
                            }
                            break;
                        case 2:// rock ties, scissor loses, paper wins
                            {
                                results.Add("You pick rock and tie with the man's rock. You both walk away indifferent.");
                                Stats noChange = new Stats(stats);
                                statChanges.Add(noChange);
                                results.Add("Your scissors get decimated by the man's rock. You take 5 damage.");
                                Stats hpChange = new Stats(stats);
                                hpChange.HP -= 5;
                                statChanges.Add(hpChange);
                                results.Add("Your paper suffocates the man's rock. Your victory permanently earns you +2 Intuition.");
                                Stats intuitionChange = new Stats(stats);
                                intuitionChange.Intuition += 2;
                                statChanges.Add(intuitionChange);
                            }
                            break;
                    }

                    choices.Add(("Kick him in the face.", ""));
                    int strengthKnowledgeIntuitionLuckCheck = 20 + Mathf.RoundToInt(10 * check);
                    if (stats.Strength + stats.Knowledge + stats.Intuition + stats.Luck > strengthKnowledgeIntuitionLuckCheck)
                    {
                        results.Add("You succeed and mock the man for his foolish challenge. Your bravado permanently earns you +2 Strength, Knowledge, Intuition, and Luck.");
                        Stats strengthKnowledgeIntuitionLuckChange = new Stats(stats);
                        strengthKnowledgeIntuitionLuckChange.Strength += 2;
                        strengthKnowledgeIntuitionLuckChange.Knowledge += 2;
                        strengthKnowledgeIntuitionLuckChange.Intuition += 2;
                        strengthKnowledgeIntuitionLuckChange.Luck += 2;
                        statChanges.Add(strengthKnowledgeIntuitionLuckChange);
                    }
                    else
                    {
                        results.Add("Your kick gloriously misses and the man retaliates before he mysteriously disappears. You take 15 damage");
                        Stats hpChange = new Stats(stats);
                        hpChange.HP -= 15;
                        statChanges.Add(hpChange);
                    }
                }
                break;
            default:
                descText = "Something went wrong, check random range in attack encounters class.";
                break;
        }
    }
}
