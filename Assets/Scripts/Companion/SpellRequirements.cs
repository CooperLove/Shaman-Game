using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu (menuName = "Companion/Requirements")]
public class SpellRequirements : ScriptableObject
{
    [SerializeField] private float cost; 
    [SerializeField] private float costIncrement; 
    [SerializeField] private int requiredPlayerLevel;
    [SerializeField] private RequiredSpecializations specializations;
    [SerializeField] private int currentLevel;
    [SerializeField] private int maxLevel;
    [SerializeField] private int requiredPlayerLevelIncrement;
    [SerializeField] private bool canBeUpgraded;

    public int RequiredPlayerLevel { get => requiredPlayerLevel; set => requiredPlayerLevel = value; }
    public RequiredSpecializations Specializations { get => specializations; set => specializations = value; }
    public int CurrentLevel { get => currentLevel; set => currentLevel = value; }
    public int MaxLevel { get => maxLevel; set => maxLevel = value; }
    public int RequiredPlayerLevelIncrement { get => requiredPlayerLevelIncrement; set => requiredPlayerLevelIncrement = value; }
    public bool CanBeUpgraded { get => canBeUpgraded; set => canBeUpgraded = value; }
    public float Cost { get => cost; set => cost = value; }
    public float CostIncrement { get => costIncrement; set => costIncrement = value; }

    public void OnSkillUpgrade (TMP_Text text, Spell spell){
        if (!canBeUpgraded || currentLevel >= maxLevel || Player.Instance.playerInfo.Level <= requiredPlayerLevel)
            return;

        //if (currentLevel == 0)
            //LearnedSpells.Instance.OnLearnSpell(spell);

        Companion c = Player.Instance.Companion;
        if (c?.SpecializationPoints > 0
            && c.Healing >= specializations.healing
            && c.Shielding >= specializations.sheilding
            && c.Buffing >= specializations.buffing )
        {
            Player.Instance.Companion.SpecializationPoints -= 1;
            requiredPlayerLevel += requiredPlayerLevelIncrement;
            cost += costIncrement;
            currentLevel += 1;
            text.text = currentLevel+"/"+maxLevel;
        }
    }
}

