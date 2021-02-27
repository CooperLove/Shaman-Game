using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CompanionSkillTreeComponent : MonoBehaviour
{
    //[SerializeField] private SpellRequirements skillTreeComponent;
    [SerializeField] private Spell spell  = null;
    [SerializeField] private Spell temporarySpell  = null;
    [SerializeField] TMP_Text text  = null;

    public Spell Spell { get => spell; set => spell = value; }
    public Spell TemporarySpell { get => temporarySpell; set => temporarySpell = value; }

    //public SpellRequirements SkillTreeComponent { get => skillTreeComponent; set => skillTreeComponent = value; }

    public void OnSkillUpgrade () => Spell?.OnSkillUpgrade(text);
    public void OnSkillUpgradeTemp () => TemporarySpell?.OnSkillUpgrade(text);
    
    
}