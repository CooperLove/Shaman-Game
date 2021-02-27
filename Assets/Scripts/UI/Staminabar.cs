using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Staminabar : MonoBehaviour
{
    public float sp, maxSP;
    public Slider slider = null;
    public TMP_Text spText = null;
    public RectTransform spRect = null;
    public Image spImage = null;
    
    private PlayerInfo pi = null;
    void Awake() {
        pi = Player.Instance.playerInfo;
        maxSP = pi.Max_SP;
        UpdateSP_Bar();
        UpdateSPText();
    }
    void Update() {
        sp = pi.Stamina;
        maxSP = pi.Max_SP;
        if (sp >= maxSP)
            return;
        RegenSP();
        UpdateSP_Bar();
        UpdateSPText();
    }
    public void RegenSP (){
        pi.Stamina += pi.SPRegen * Time.deltaTime;
        sp = pi.Stamina;
    }
    // Update is called once per frame
    public void UpdateSP_Bar (){
        sp = pi.Stamina;
        float currSP = sp/pi.Max_SP;

        if (spImage != null){
            spImage.fillAmount = currSP;
            return;
        }
        spRect.localScale = new Vector3 (currSP, 1, 1);
    }
    public void UpdateSPText (){
        if (spText == null)
            return;
        spText.text = (int) pi.Stamina+"/"+(int) pi.Max_SP;
    }
    public void LoseSP (){
        pi.Stamina -= 10;
        sp = pi.Stamina;
    }
}
