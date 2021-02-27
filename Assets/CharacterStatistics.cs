using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class CharacterStatistics : MonoBehaviour
{
    [SerializeField] private AddPoints add;

    [SerializeField] private Image hpFill = null;
    [SerializeField] private Image mpFill = null;
    [SerializeField] private Image spFill = null;
    [SerializeField] private Image expFill = null;

    [SerializeField] private List<TMP_Text> percentages = new List<TMP_Text>();

    private PlayerInfo playerInfo;
    // Start is called before the first frame update
    private void Awake() {
        Debug.Log("Awake cm");
        add = Resources.FindObjectsOfTypeAll<AddPoints>()[0]; 
        add.Initialize();

        playerInfo = Player.Instance.playerInfo;
        UpdateBars();
    }

    private void UpdateBars()
    {
        var percHp = playerInfo.Health / playerInfo.Max_HP;
        var percMp = playerInfo.Mana / playerInfo.Max_MP;
        var percSp = playerInfo.Stamina / playerInfo.Max_SP;
        var percExp = playerInfo.Experience / playerInfo.NextLevelEXP;

        hpFill.fillAmount = percHp;
        mpFill.fillAmount = percMp;
        spFill.fillAmount = percSp;
        expFill.fillAmount = percExp;

        percentages[0].text = $"{System.Math.Round(percHp  * 100f, 2)}%";
        percentages[1].text = $"{System.Math.Round(percMp  * 100f, 2)}%";
        percentages[2].text = $"{System.Math.Round(percSp  * 100f, 2)}%";
        percentages[3].text = $"{System.Math.Round(percExp * 100f, 2)}%";
    }
}
