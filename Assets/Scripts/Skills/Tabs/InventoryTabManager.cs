using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTabManager : MonoBehaviour
{
    [SerializeField] public GameObject currentTab = null;
    public Tab tab;
    public Tab_AllItems allItemsTab = null;
    public Tab_Weapon weaponTab = null;
    public Tab_Gear gearTab = null;
    public Tab_Jewerly jewerlyTab = null;
    public Tab_Gem gemTab = null;
    public Tab_Consumable consumableTab = null;
    public Tab_Companion companionTab = null;
    public Tab_Enhance enhanceTab = null;
    public Tab_Quests questsTab = null;

    private static InventoryTabManager instance;

    public static InventoryTabManager Instance { get => instance; set => instance = value; }

    InventoryTabManager (){
        if (Instance == null)
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void Initialize (){
        allItemsTab   = Resources.FindObjectsOfTypeAll<Tab_AllItems>()[1];
        weaponTab     = Resources.FindObjectsOfTypeAll<Tab_Weapon>()[0];
        gearTab       = Resources.FindObjectsOfTypeAll<Tab_Gear>()[0];
        jewerlyTab    = Resources.FindObjectsOfTypeAll<Tab_Jewerly>()[0];
        gemTab        = Resources.FindObjectsOfTypeAll<Tab_Gem>()[0];
        consumableTab = Resources.FindObjectsOfTypeAll<Tab_Consumable>()[0];
        companionTab  = Resources.FindObjectsOfTypeAll<Tab_Companion>()[1];
        enhanceTab    = Resources.FindObjectsOfTypeAll<Tab_Enhance>()[0];
        questsTab     = Resources.FindObjectsOfTypeAll<Tab_Quests>()[1];
        questsTab.Initialize();
    }

    public void OpenTab (GameObject tab) {
        if (tab == null)
            return;
        
        CloseTab(currentTab);
        currentTab = tab;
        this.tab = currentTab.GetComponent<Tab>();
        this.tab.OpenAction();
    }

    public void CloseTab (GameObject tab) {
        if (tab == null)
            return;

        currentTab.GetComponent<Tab>().CloseAction();
    }

    public void SetCurrentTab (GameObject tab) => currentTab = tab; 
}
