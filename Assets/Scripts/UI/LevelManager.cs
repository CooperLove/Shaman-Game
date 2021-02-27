using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LevelManager : MonoBehaviour
{
    public TMP_Text levelText;
    public Slider slider;
    [SerializeField] private Healthbar[] hpbar = null;
    [SerializeField] private Manabar[] mpbar = null;
    // [SerializeField] private Staminabar[] spbar = null;
    [SerializeField] private Expbar expbar;
    [SerializeField] private static LevelManager instance;

    public Expbar Expbar { get => expbar; set => expbar = value; }
    public static LevelManager Instance { get => instance; set => instance = value; }

    LevelManager (){
        if (instance == null)
            instance = this;
    }
    private void Awake() {
        levelText.text = Player.Instance.playerInfo.Level+"";
        
        //GameObject instance = Instantiate(Resources.Load("Prefabs/Highlight", typeof(GameObject))) as GameObject;
        //Debug.Log(instance?.name+" "+instance.transform.parent+" "+instance.transform.position);
    }
   
    public void UpdatePlayerLevel (){
        Player.Instance.playerInfo.Level = (int) slider.value;
        levelText.text = Player.Instance.playerInfo.Level+"";
        
    }
    public void OnLevelUp (){
        var playerInfo = Player.Instance.playerInfo;
        
        foreach (var bar in hpbar){
            bar.OnLevelUp();
        }
        foreach (var bar in mpbar){
            bar.OnLevelUp();
        }
        // foreach (var bar in spbar){
        //     bar.maxSP = playerInfo.Max_SP;   bar.UpdateSP_Bar(); bar.UpdateSPText();
        // }
        Expbar.UpdateEXP_Bar();
        levelText.text = playerInfo.Level+"";

        playerInfo.NextLevelEXP *= 1.125f;
    }
}
