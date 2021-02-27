using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private static MainMenu instance;

    public static MainMenu Instance { get => instance; set => instance = value; }

    MainMenu (){
        if (instance == null)
            instance = this;
    }
    public GameObject mainMenu;
    public Inventory inventory;
    public InputHandler inputHandler;
    public RectTransform leftMenuPanel;
    private void Awake() {
        //leftMenuPanel = GameObject.Find("General Menus").GetComponent<RectTransform>();
        mainMenu = Resources.FindObjectsOfTypeAll<CharacterMenuUI>()[0].transform.parent.parent.gameObject;

        InitializeTabs();
        InitializeInventory();
        InitializeInputHandler();
        InitializeCompanionUI();
        //gameObject.SetActive(false);
    }

    public void Open () => mainMenu.SetActive(true);
    public void Close () => mainMenu.SetActive(false);
    public void Open (GameObject g) => g.SetActive(true);
    public void Close (GameObject g) => g.SetActive(false);

    public void SetMenuSize (int x, int y){
        leftMenuPanel.sizeDelta = new Vector2(x, y);
    }

    private void InitializeInputHandler (){
        inputHandler = Resources.FindObjectsOfTypeAll<InputHandler>()[0];
        //inputHandler = GameObject.Find("InputHandler").GetComponent<InputHandler>();
        inputHandler.companionUI = CompanionUI.Instance;
        if (inputHandler.companionUI.MainWindow == null){
            inputHandler.companionUI.MainWindow = mainMenu;
        }

        //inputHandler.companionUI?.MainWindow.SetActive(false);
    }

    private void InitializeInventory (){
        inventory.StackItems();
        Inventory.Instance.Initialize();
        Player.Instance.Inventory = Inventory.Instance;
    }

    private void InitializeTabs (){
        InventoryTabManager.Instance.Initialize();
    }

    

    private void InitializeCompanionUI (){
        CompanionUI.Instance.characterMenuUI = Resources.FindObjectsOfTypeAll<CharacterMenuUI>()[0].gameObject;
        CompanionUI.Instance.characterMenuStatistics = Resources.FindObjectsOfTypeAll<CharacterStatistics>()[0].gameObject;
        CompanionUI.Instance.filters = Resources.FindObjectsOfTypeAll<FilterManager>()[0].gameObject;
        //Debug.Log($"{CompanionUI.Instance.filters.name} {CompanionUI.Instance.characterMenuUI.name} {CompanionUI.Instance.characterMenuStatistics.name}");
        CompanionUI.Instance.characterMenuUI.SetActive(false);
        CompanionUI.Instance.filters.SetActive(false);
        //CompanionUI.Instance.characterMenuStatistics.SetActive(false);
    }
}
