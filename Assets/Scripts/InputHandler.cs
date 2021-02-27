using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    // Update is called once per frame
    public static bool isGamePaused = false;
    public GameObject optionsMenu = null;
    public GameObject mainMenu = null;
    public GameObject pauseMenu = null;
    //[SerializeField] private GameObject mainWindow = null;

    [SerializeField] PlayerInfo pi;

    [SerializeField] public CompanionUI companionUI = null;

    public static InputHandler Instance { get; private set; }

    private InputHandler (){
        if (Instance == null)
            Instance = this;
    }
    private void Awake() {
        var g = Resources.Load<Gear>("GearTest");
        //Debug.Log(g.ItemName);
        if (Instance == null)
            Instance = this;
        pi = Player.Instance.playerInfo;
        //Debug.Log("Awake Input Handler "+companionUI.name);
        //Debug.Log("Awake Input Handler "+companionUI.MainWindow?.name);
        
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //Debug.Log("Esc pressed");
            OnPause ();
            pauseMenu.SetActive(isGamePaused);
        }

        if (GameStatus.IgnoreInputs())
            return;

        GetInput(pi.Skill01, KeyCode.Alpha1);
        GetInput(pi.Skill02, KeyCode.Alpha2);
        GetInput(pi.Skill03, KeyCode.Alpha3);
        GetInput(pi.Skill04, KeyCode.Alpha4);
    }

    private void GetInput (UseSkill use, KeyCode key){
        if (use){
            if (Input.GetKeyDown(key))
                use.Use();
            if (use.Skill is ChargeableSkill charge){
                if (Input.GetKey(key))
                    charge.OnCharging();
                if (Input.GetKeyUp(key))
                    charge.OnEndCharging();
            }
        }
    }

    private void OnPause (){
        if (isGamePaused)
            Resume();
        else
            Pause();
    }

    public void Pause (){
        Debug.Log("Game Paused");
        GameStatus.isUsingSomeMenu = true;
        Player.Instance.gameObject.layer = 2;
        isGamePaused = true;
        //companionUI.OpenPauseMenu();
        Time.timeScale = 0f;
    }
    public void Resume (){
        Debug.Log("Game Resumed");
        GameStatus.isUsingSomeMenu = false;
        Player.Instance.gameObject.layer = 8;
        isGamePaused = false;
        //companionUI.ClosePauseMenu();
        if (GameStatus.isSkillTreeOpened)
            SkillTree.Instance.Close();
        else
            companionUI.CloseInventory();
        Time.timeScale = 1f;
    }
}
