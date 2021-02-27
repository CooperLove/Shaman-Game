using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossUIManager : MonoBehaviour
{
    [SerializeField] private Boss boss = null;
    [SerializeField] private TMP_Text bossName = null;
    [SerializeField] private Transform hpBar = null;
    public GameObject bossUI = null;

    private static BossUIManager instance = null;

    public static BossUIManager Instance { get => instance; set => instance = value; }
    public Boss Boss { get => boss; set => boss = value; }

    BossUIManager (){
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Initialize (){
        bossName.text = Boss.BossName;
        hpBar.localScale = Vector3.one;
        bossUI.SetActive(true);
    }
    public void UpdateHPBar (){
        float currHP = Boss.Hp/Boss.MaxHP;
        hpBar.localScale = new Vector3(currHP, 1, 1);
    }

    public void OnEndBattle (){
        bossUI.SetActive(false);
    }
}
