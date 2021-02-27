using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStatus
{
    private static bool ignoreCommands = false;
    public static bool isCutScenePlaying = false;
    public static bool isGamePaused = false;
    public static bool isOnTutorial = false;
    public static bool isOnChallenge = false;
    public static bool isInteractingWithNpc = false;
    public static bool isSkillTreeOpened = false;
    public static bool isUsingSomeMenu = false;
    private static bool isUsingSkill = false;
    private static bool isAttacking = false;
    private static MonoBehaviour calledIgnore = null;


    
    public static bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public static bool IsIgnoringCommands => ignoreCommands;

    public static void IgnoreCommands(bool status, MonoBehaviour g)
    {
        // Debug.Log($" {calledIgnore}- {g} is setting ignore commands to {status}");
        if (!calledIgnore)
        {
            calledIgnore = g;
            ignoreCommands = status;
            return;
        }

        if (ignoreCommands && !g.Equals(calledIgnore))
        {
           //Debug.Log($"{g} tried to set ignoreCommands but {calledIgnore} set it first");
            return;
        }

        ignoreCommands = status;
        if (!status) calledIgnore = null;
    }
    
    public static void UsingSkill (bool status) => isUsingSkill = status;

    public static bool IgnoreInputs()
        => isGamePaused || isOnChallenge || isOnTutorial || isInteractingWithNpc || isUsingSkill || isSkillTreeOpened
        || isUsingSomeMenu || ignoreCommands;
}
