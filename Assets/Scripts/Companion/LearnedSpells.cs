using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LearnedSpells : MonoBehaviour
{   
    [SerializeField] private GameObject spell  = null;
    [SerializeField] private Transform spellbar  = null;
    [SerializeField] private static LearnedSpells learnedSpells  = null;
    [SerializeField] private SwitchSpells switchSpells  = null;

    public GameObject Spell { get => spell; set => spell = value; }
    public static LearnedSpells Instance { get => learnedSpells; private set => learnedSpells = value; }
    public SwitchSpells SwitchSpells { get => switchSpells; set => switchSpells = value; }

    private void Awake() {
        switchSpells = new SwitchSpells();
    }
    private LearnedSpells(){
        if (learnedSpells == null)
            learnedSpells = this;
    }

    public void OnLearnSpell (Spell s){
        GameObject newSpell = Instantiate(spell, Vector3.zero, transform.rotation) as GameObject;
        //Spell sp = s.MakeCopy();
        //Gets a random color
        ColorBlock cb = newSpell.GetComponent<Button>().colors;
        cb.normalColor = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1);
        cb.selectedColor = cb.normalColor;
        newSpell.GetComponent<Button>().colors = cb;
        //Setup tooltip
        //newSpell.AddComponent<TooltipHandler>();
        //newSpell.GetComponent<TooltipComponent>().SkillTreeComponent = companion;
        //newSpell.GetComponent<TooltipHandler>().Spell = s;
        //Setup script to use spell
        newSpell.AddComponent<UseSpell>();
        //newSpell.GetComponent<UseSpell>().SkillTreeComponent = companion;
        newSpell.GetComponent<UseSpell>().Spell = s;

        newSpell.transform.SetParent(transform);
        newSpell.transform.localScale = Vector3.one;
        newSpell.transform.localPosition = Vector3.zero;

        newSpell.gameObject.name = s.SpellName;
        newSpell.gameObject.SetActive(true);

        //Player.Instance.Companion.Spells.Add(sp);
    }


    public void OnSwitchCompanion (){
        List<GameObject> l = Player.Instance.Companion.Spells;

        int t = transform.childCount;
        for (int i = t - 1; i >= 0; i--)
            Destroy (transform.GetChild(i).gameObject);

        foreach (GameObject spell in l)
            OnLearnSpell(spell.GetComponent<Spell>());

        t = spellbar.childCount;
        for (int i = t - 1; i >= 0; i--)
            Destroy (spellbar.GetChild(i).gameObject);
    }

    public void UseSpellHelper (GameObject g) => StartCoroutine (UseSpellHandler(g));
    public IEnumerator UseSpellHandler (GameObject g){
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = false;
        
        yield return new WaitForSeconds(0f);
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).GetComponent<BoxCollider2D>().enabled = true;
            
        g.GetComponent<UseSpell>().canUseSpell = true;
        gameObject.SetActive(false);
        g.GetComponent<BoxCollider2D>().enabled = true;
    }
    private void OnDestroy() {
        Debug.Log("Destroudsadosajdjsabdisah");
    }
}

[System.Serializable]
public class SwitchSpells {
    public GameObject spell01;
    public GameObject spell02;

    public void OnSwitchSpell (){
        if (spell01 != null && spell02 != null){
            Transform p = spell01.transform.parent;

            int spell01Index = spell01.transform.GetSiblingIndex();
            int spell02Index = spell02.transform.GetSiblingIndex();

            spell01.AddComponent<SwitchSpell>();
            spell01.GetComponent<SwitchSpell>().Spellbar = spell01.transform.parent;
            spell02.AddComponent<OpenLearnedSpellsMenu>();

            //Destroy unnecessary scripts
            GameObject.Destroy(spell01.GetComponent<OpenLearnedSpellsMenu>());
            GameObject.Destroy(spell02.GetComponent<SwitchSpell>());
            GameObject.Destroy(spell01.GetComponent<Spell_Input>());
            
            //Switch parents
            spell01.transform.SetParent(spell02.transform.parent);
            spell02.transform.SetParent(p);

            //Switch the position on their parent
            spell01.transform.SetSiblingIndex(spell02Index);
            spell02.transform.SetSiblingIndex(spell01Index);

            //Set key to use spell (Shift + (index + 49))
            spell02.AddComponent<Spell_Input>();
            //spell02.GetComponent<Spell_Input>().SetKey();
            spell02.GetComponent<Spell_Input>().SetKey(spell01Index + 49);

            spell01 = null;
            spell02 = null;
            
        }else if (spell02 != null){
            spell02.AddComponent<OpenLearnedSpellsMenu>();
            GameObject.Destroy(spell02.GetComponent<SwitchSpell>());

            //Set key to use spell (Shift + (index + 49))
            spell02.AddComponent<Spell_Input>();
            //spell02.GetComponent<Spell_Input>().SetKey();
            spell02.GetComponent<Spell_Input>().SetKey(spell02.transform.GetSiblingIndex() + 49);
    
            spell01 = null;
            spell02 = null;
        }
    }

}
