using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealingTier1 : Spell
{
    [SerializeField] private int healingValue = 0;
    [SerializeField] private int healPerLevel = 0;

    public override void Use ()
    {
        if (Player.Instance.Companion.Mana >= Cost){
            Player.Instance.Companion.Mana -= Cost;
            Player.Instance.playerInfo.Health += (healingValue + (CurrentLevel * healPerLevel));
            Debug.Log("Healed for "+healingValue+"+"+"("+CurrentLevel+"*"+healPerLevel+") = "+(healingValue + (CurrentLevel * healPerLevel)));
        }
    }
    

    public override string GetDescription(){
        string[] split = Description.Split('<','>');
        string[] values = {"<color=red><b>"+(healingValue + (CurrentLevel * healPerLevel))+"</b></color> ("+
                       "<color=green><b>"+healingValue+"</b></color>"+"+"+
                       "<color=yellow><b>"+CurrentLevel+"</b></color>"+"*"+
                       "<color=green><b>"+healPerLevel+"</b></color>"+") "};
        int j = 0;
        string newDescription = "";
        for (int i = 0; i < split.Length; i++)
        {
            if (j < values.Length && split[i].Length == 0){
                split[i] = values[j++];
            }
            
            newDescription += split[i];
            //Debug.Log(split[i]);
        }
        //Debug.Log(newDescription);
        //Description = newDescription;
        return newDescription;
    }

    public override void OnSkillUpgrade(TMP_Text text)
    {
        Companion c = Player.Instance.Companion;
        GameObject cSpell = c.Spells.Find(x => x.GetComponent<Spell>().Index == Index);
        Spell s = null;
        if (cSpell != null) 
            s = cSpell.GetComponent<Spell>();

        if (s != null && (!s.CanBeUpgraded || s.CurrentLevel >= s.MaxLevel || Player.Instance.playerInfo.Level <= s.RequiredPlayerLevel))
            return;

        if (currentLevel == 0 && cSpell == null){
            GameObject g = Instantiate(gameObject);
            if (c.CompName.Equals(Player.Instance.CompanionObj.CompName))
                LearnedSpells.Instance.OnLearnSpell(g.GetComponent<Spell>());
            g.transform.localPosition = Vector3.zero;
            g.transform.SetParent(c.companionHandler.gameObject.transform);
            c.Spells.Add(g);
        }

        cSpell = c.Spells.Find(x => x.GetComponent<Spell>().Index == Index);
        s = cSpell.GetComponent<Spell>();
        
        if (c?.SpellPoints > 0
            && c.Healing >= specializations.healing
            && c.Shielding >= specializations.sheilding
            && c.Buffing >= specializations.buffing )
        {
            c.SpellPoints -= 1;
            s.RequiredPlayerLevel += s.LevelIncrement;
            s.Cost += s.CostIncrement;
            s.CurrentLevel += 1;
            text.text = s.CurrentLevel+"/"+s.MaxLevel;
        }
    }

    public override Spell MakeCopy()
    {
        HealingTier1 s = new HealingTier1();
        s.SpellName = this.spellName;
        s.Description = this.description;
        s.CurrentLevel = this.currentLevel;
        s.MaxLevel = this.maxLevel;
        s.RequiredPlayerLevel = this.requiredPlayerLevel;
        s.LevelIncrement = this.levelIncrement;
        s.Cooldown = this.cooldown;
        s.Cost = this.cost;
        s.CostIncrement = this.costIncrement;
        s.Specializations = this.specializations;
        s.CanBeUpgraded = this.canBeUpgraded;
        Debug.Log(s.spellName);
        return s;
    }
}
