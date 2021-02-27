using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CompanionUI :MonoBehaviour, IPointerClickHandler {
    [SerializeField] private GameObject pauseMenu = null;
    [SerializeField] private GameObject mainWindow = null;
    [SerializeField] private GameObject statsMenu = null;
    [SerializeField] private GameObject skillTree = null;
    [SerializeField] private GameObject evolutionTree = null;
    public GameObject characterMenuStatistics = null;
    public Scrollbar characterMenuStatisticsScrollbar = null;
    public GameObject characterMenuUI = null;
    public GameObject filters = null;
    [SerializeField] private Companion companion = null;
    [SerializeField] private List<TMP_Text> texts = new List<TMP_Text>();
    [SerializeField] private Animator animator  = null;
    public Button characterMenuButton = null;
    public Button allItems = null;
    public Button weapons = null;
    public Button gear = null;
    public Button jewerly = null;
    public Button gems = null;
    public Button consumables = null;
    public Button companions = null;
    public Button quests = null;
    [SerializeField] private bool isOpen = false;

    private static CompanionUI instance;

    public static CompanionUI Instance { get => instance; set => instance = value; }
    public GameObject StatsMenu { get => statsMenu; set => statsMenu = value; }
    public GameObject MainWindow { get => mainWindow; set => mainWindow = value; }
    public List<TMP_Text> Texts { get => texts; set => texts = value; }

    private CompanionUI (){
        if (instance == null)
            instance = this;
    }
    private void Awake() {
        CreateTextsList ();
        isOpen = true;
        UpdateUI();
        companion = Player.Instance.Companion;
    }
    private void Start() {
        
    }

    public void CreateTextsList(){
        Texts = new List<TMP_Text>();
        if (!statsMenu.transform.parent.gameObject.activeInHierarchy)
            return;
        statsMenu.SetActive(true);
        Texts.Add(GameObject.Find("CompanionUILevelText").GetComponent<TMP_Text>());
        Texts.Add(GameObject.Find("CompanionUIAP Text").GetComponent<TMP_Text>());
        Texts.Add(GameObject.Find("CompanionUIT1 Text").GetComponent<TMP_Text>());
        Texts.Add(GameObject.Find("CompanionUIT2 Text").GetComponent<TMP_Text>());
        Texts.Add(GameObject.Find("CompanionUIT3 Text").GetComponent<TMP_Text>());
        Texts.Add(GameObject.Find("CompanionUIIntelligence Text").GetComponent<TMP_Text>());
        Texts.Add(GameObject.Find("CompanionUIIntellect Text").GetComponent<TMP_Text>());
        Texts.Add(GameObject.Find("CompanionUIWisdom Text").GetComponent<TMP_Text>());
        Texts.Add(GameObject.Find("CompanionUISpecialty Text").GetComponent<TMP_Text>());
        Texts.Add(GameObject.Find("CompanionUIHealingText").GetComponent<TMP_Text>());
        Texts.Add(GameObject.Find("CompanionUIShieldingText").GetComponent<TMP_Text>());
        Texts.Add(GameObject.Find("CompanionUIBuffingText").GetComponent<TMP_Text>());
        Texts.Add(GameObject.Find("CompanionUIMP Text").GetComponent<TMP_Text>());
        statsMenu.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left){
            Debug.Log("Left click");
            isOpen = !isOpen;
            animator.SetBool("isOpen", isOpen);
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
            Debug.Log("Middle click");
        else if (eventData.button == PointerEventData.InputButton.Right){
            Debug.Log("Right click");
            OpenInventoryTab();
            UpdateUI();
            InputHandler.Instance.Pause();
        }
    }

    public void OpenInventoryTab () {
        //Debug.Log("All Items: "+allItems?.gameObject.name);
        allItems?.Select();
        Inventory.Instance?.HighlightTab(allItems);
        Inventory.Instance.Open();
        //CloseCharacterStatistics();
        CloseCharacterMenuUI();
        MainWindow?.SetActive(true);
        OpenFilters();
        Inventory.Instance.CloseQuests();
        Inventory.Instance?.InventorySize.ShowAllItems();
        CloseCompanionWindows();
    }
    public void OpenCharacterTab () {
        //Debug.Log("All Items: "+characterMenuButton?.gameObject.name);
        characterMenuButton?.Select();
        Inventory.Instance?.HighlightTab(characterMenuButton);
        CloseFilters();
        Inventory.Instance.InventorySize.CloseGearFilters();
        OpenCharacterStatistics();
        OpenCharacterMenuUI();
        Inventory.Instance.CloseQuests();
        MainWindow?.SetActive(true);
        CloseCompanionWindows();
    }
    public void OpenCompanionsTab () {
        //Debug.Log("All Items: "+companions?.gameObject.name);
        companions?.Select();
        Inventory.Instance?.HighlightTab(companions);
        //CloseFilters();
        Inventory.Instance.InventorySize.CloseGearFilters();
        //CloseCharacterStatistics();
        CloseCharacterMenuUI();
        Inventory.Instance.CloseQuests();
        MainWindow?.SetActive(true);
        Inventory.Instance?.InventorySize.ShowCompanions();
        Inventory.Instance.Open();
    }
    public void OpenQuestsTab () {
        //Debug.Log("All Items: "+quests?.gameObject.name);
        quests?.Select();
        Inventory.Instance?.HighlightTab(quests);
        CloseFilters();
        Inventory.Instance.InventorySize.CloseGearFilters();
        MainWindow?.SetActive(true);
        CloseCompanionWindows();
    }
    public void CloseInventory () => MainWindow.SetActive(false);
    public void OpenPauseMenu () => pauseMenu.SetActive(true);
    public void ClosePauseMenu () => pauseMenu.SetActive(false);
    public void OpenStatsMenu () => StatsMenu.SetActive(true);
    public void CloseStatsMenu () => StatsMenu.SetActive(false);
    public void OpenSkillTree () => skillTree.SetActive(true);
    public void CloseSkillTree () => skillTree.SetActive(false);
    public void OpenEvolutionTree () => evolutionTree.SetActive(true);
    public void CloseEvolutionTree () => evolutionTree.SetActive(false);
    public void OpenFilters () => filters.SetActive(true);
    public void OpenCharacterStatistics () {
        if (characterMenuStatisticsScrollbar != null)
            characterMenuStatisticsScrollbar.value = 1;

        characterMenuStatistics.SetActive(true);
    }
    public void CloseCharacterStatistics () => characterMenuStatistics.SetActive(false);
    public void OpenCharacterMenuUI () => characterMenuUI.SetActive(true);
    public void CloseCharacterMenuUI () => characterMenuUI.SetActive(false);
    public void CloseFilters () => filters.SetActive(false);
    public void UpdateCompanionStatus () {
        if (!Player.Instance.CompanionObj.CompName.Equals("")){
            companion = Player.Instance.Companion;
            Player.Instance.Companion?.ChangeCompanionStats(Player.Instance.CompanionObj);
        }
    } 

    public void IncreaseIntelligence () {
        companion = Player.Instance.Companion;
        if (companion?.AdditionalPoints > 0){
            companion.Intelligence += 1;
            companion.AdditionalPoints -= 1;
            companion.MaxMana += 2.5f;
            if (companion.CompName.Equals(Player.Instance.CompanionObj.CompName)){
                Player.Instance.CompanionObj.Intelligence = Player.Instance.Companion.Intelligence;
            }
        }
    }

    public void IncreaseIntellect () {
        companion = Player.Instance.Companion;
        if (companion?.AdditionalPoints > 0){
            companion.Intellect += 1;
            companion.AdditionalPoints -= 1;
            companion.ManaRegen += 0.05f;
            if (companion.CompName.Equals(Player.Instance.CompanionObj.CompName)){
                Player.Instance.CompanionObj.Intellect = Player.Instance.Companion.Intellect;
            }
        }
    }


    public void IncreaseWisdom () {
        companion = Player.Instance.Companion;
        if (companion?.AdditionalPoints > 0){
            companion.Wisdom += 1;
            companion.AdditionalPoints -= 1;
            companion.SpellEffectMultiplier += 0.005f;
            if (companion.CompName.Equals(Player.Instance.CompanionObj.CompName)){
                Player.Instance.CompanionObj.Wisdom = Player.Instance.Companion.Wisdom;
            }
        }
    }

    public void IncreaseHealing () {
        companion = Player.Instance.Companion;
        if (companion?.SpecializationPoints > 0){
            companion.Healing += 1;
            companion.SpecializationPoints -= 1;
            if (companion.CompName.Equals(Player.Instance.CompanionObj.CompName)){
                Player.Instance.CompanionObj.Healing = Player.Instance.Companion.Healing;
            }
        }
    }

    public void IncreaseShielding () {
        companion = Player.Instance.Companion;
        if (companion?.SpecializationPoints > 0){
            companion.Shielding += 1;
            companion.SpecializationPoints -= 1;
            if (companion.CompName.Equals(Player.Instance.CompanionObj.CompName)){
                Player.Instance.CompanionObj.Shielding = Player.Instance.Companion.Shielding;
            }
        }
    }

    public void IncreaseBuffing () {
        companion = Player.Instance.Companion;
        if (companion?.SpecializationPoints > 0){
            companion.Buffing += 1;
            companion.SpecializationPoints -= 1;
            if (companion.CompName.Equals(Player.Instance.CompanionObj.CompName)){
                Player.Instance.CompanionObj.Buffing = Player.Instance.Companion.Buffing;
            }
        }
    }

    public void UpdateUI(){
        Companion comp = Player.Instance.Companion;
        //Debug.Log(texts);
        if (texts == null || texts.Count == 0)
            CreateTextsList();
        Texts[0].text = ""+comp.Level;
        Texts[1].text = ""+comp.AdditionalPoints;

        Texts[2].text = ""+comp.CrystalT1;
        Texts[3].text = ""+comp.CrystalT2;
        Texts[4].text = ""+comp.CrystalT3;

        Texts[5].text = ""+comp.Intelligence;
        Texts[6].text = ""+comp.Intellect;
        Texts[7].text = ""+comp.Wisdom;

        Texts[8].text = ""+comp.SpecializationPoints;
        Texts[9].text = ""+comp.Healing;
        Texts[10].text = ""+comp.Shielding;
        Texts[11].text = ""+comp.Buffing;

        Texts[12].text = ""+comp.Mana+"/"+comp.MaxMana;
    }
    
    public void UpdateUI (Companion comp){
        //Debug.Log("Comp: "+comp.CompName+" "+texts.Count+" "+comp.Mana+"/"+comp.MaxMana);
        if (texts == null || texts.Count == 0)
            CreateTextsList();
        Texts[0].text = ""+comp.Level;
        Texts[1].text = ""+comp.AdditionalPoints;

        Texts[2].text = ""+comp.CrystalT1;
        Texts[3].text = ""+comp.CrystalT2;
        Texts[4].text = ""+comp.CrystalT3;

        Texts[5].text = ""+comp.Intelligence;
        Texts[6].text = ""+comp.Intellect;
        Texts[7].text = ""+comp.Wisdom;

        Texts[8].text = ""+comp.SpecializationPoints;
        Texts[9].text = ""+comp.Healing;
        Texts[10].text = ""+comp.Shielding;
        Texts[11].text = ""+comp.Buffing;

        Texts[12].text = ""+comp.Mana+"/"+comp.MaxMana;
    
    }

    public void CloseCompanionWindows (){
        //if (StatsMenu.activeInHierarchy){
            //CloseStatsMenu();
            CompanionSkillTree.Instance.Close();
            EvolutionTree.Instance.Close();
            CompanionDescriptionPanel.Instance.Close();
            QuestDescriptionPanel.Instance.Close();
        //}
    }

}
