using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpellHandler : MonoBehaviour
{
    public Spell[] spells;
    public TMP_Text[] spellText;
    private static SpellHandler instance;

    public static SpellHandler Instance { get => instance; set => instance = value; }

    private SpellHandler (){
        if (instance == null)
            instance = this;
    }
    public void ChangeCurrentLevel (int index, int currentLevel){
        if (index >= 0 && index < spells.Length && spells[index] != null){
            Debug.Log(spells[index].CurrentLevel+" => "+currentLevel+" "+Player.Instance.Companion.Spells.Find(x => x.GetComponent<Spell>().Index == index));
            spells[index].CurrentLevel = currentLevel;
        }
    }

    public void UIText () => Player.Instance.Companion.UpdateSkillTreeUI();

    public void ResetLevel (){
        int i = 0;
        List<GameObject> l = Player.Instance.Companion.Spells;
        foreach (Spell s in spells)
        {
            if (l.Find(x => x.GetComponent<Spell>().Index == s.Index) == null)
                s.CurrentLevel = 0;
                
            spellText[i++].text = s.CurrentLevel+"/"+s.MaxLevel;
        }
    }
}
