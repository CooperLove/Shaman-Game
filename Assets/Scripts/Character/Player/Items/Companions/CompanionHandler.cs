using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CompanionHandler : ItemHandler
{

    [SerializeField] private Companion companion  = null;

    public Companion Companion { get => companion; set => companion = value; }

    private void Start() {
        Sprite = transform.GetChild(0).GetComponent<Image>();
        ItemName = transform.GetChild(1).GetComponent<TMP_Text>();
        QuantityText = transform.GetChild(2).GetComponent<TMP_Text>();
        _UpdateUI(false);    
    }
    public override void OnSelect(BaseEventData eventData)
    {
        Player.Instance.Companion = Companion;
        Player.Instance.Companion.companionHandler = this;
        CompanionUI.Instance.UpdateUI(Companion);
        companion.NewItemIndicator(Inventory.Instance.CheckForNewItems(companion.MyType(), transform.GetSiblingIndex()));
        if (transform.childCount > 3)
            Destroy(transform.GetChild(3).gameObject);
        DeactivateSpellTransforms(true);
        Companion.UpdateSkillTreeUI();

        DescriptionPanel.Instance.Close();
        CompanionSkillTree.Instance.Close();
        EvolutionTree.Instance.Close();
        CompanionDescriptionPanel.Instance.Close();
        QuestDescriptionPanel.Instance.Close();
        CompanionUI.Instance.OpenStatsMenu();
    }
    public override void OnDeselect(BaseEventData eventData)
    {
        RemoveListerners();
    }

    public void ChangeCompanion (){
        Inventory.CompanionHandler?._UpdateUI(false);
        Inventory.CompanionHandler?.DeactivateSpellTransforms(false);
        Companion comp = Player.Instance.Companion;
        Companion compObj = Player.Instance.CompanionObj;

        //if (!compObj.CompName.Equals(""))
            //comp.ChangeCompanionStats(Player.Instance.CompanionObj);
        ((InitialCompanion)compObj).UnfillPaths();
        Companion.companionHandler = this;
        compObj.ChangeCompanionStats(Companion);
        comp = Companion;
        comp.RefillPaths();

        _UpdateUI(true);
        Inventory.CompanionHandler = this;
        LearnedSpells.Instance.OnSwitchCompanion();
    }

    private void DeactivateSpellTransforms (bool state){
        int total = transform.childCount;
        //Debug.Log(total);

        for (int i = 0; i < total; i++)
        {
            Spell child;
            transform.GetChild(i).TryGetComponent<Spell>(out child);
            //Debug.Log(child);
            if (child != null)
                child.gameObject.SetActive(state);
        }
    }

    public void AddListerners () => GetComponent<Button>().onClick.AddListener(ChangeCompanion);
    public void RemoveListerners () => GetComponent<Button>().onClick.RemoveListener(ChangeCompanion);
    public void UpdateUI () => CompanionUI.Instance.UpdateUI();

    public void _UpdateUI (bool isSelected){
        itemName.text = !isSelected ? ""+Companion.CompName : ""+Companion.CompName+" (Equipped)";
        quantityText.text = "1";
    }

    protected override void OnSelect()
    {
        return;
    }

    public override Item CreateNewItem()
    {
        throw new System.NotImplementedException();
    }
}
