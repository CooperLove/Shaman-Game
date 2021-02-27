using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillTooltip : Tooltip
{
    [SerializeField] private TMP_Text skillName = null;
    [SerializeField] private TMP_Text cost = null;
    [SerializeField] private TMP_Text currentLevel = null;
    [SerializeField] private TMP_Text nextLevel = null;
    [SerializeField] private TMP_Text cooldown = null;
    [SerializeField] private TMP_Text energyGain = null;
    [SerializeField] private TMP_Text shortcut = null;
    public float fixedWidth = 550;

    public static SkillTooltip Instance { get; private set; }

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private SkillTooltip (){
        if (Instance == null)
            Instance = this;
    }
    
    public override void Show(string s)
    {
        descText.text = s;
    }
    
    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    public override void Resize(string s)
    {
        
        descText.text = s;
        var preferredAfter = descText.GetPreferredValues();

        var height =  preferredAfter.y + 165;
        
        rectTransform.sizeDelta = new Vector2(fixedWidth, height);
       //Debug.Log($"Texto {descText.text} \n Altura do texto {height} | {preferredAfter}");
    }

    public void Show (Active skill){
        this.skillName.text = skill.SkillName;
        this.cost.text = $"Custo em MP: {skill.MpCost}";
        this.cooldown.text = $"Tempo de recarga: {skill.Cooldown}s";
        this.currentLevel.text = $"Nível atual: {skill.CurrentLevel}/{skill.MaxLevel}";
        this.nextLevel.text = $"Próximo nível: {skill.LevelToLearn}";
        this.energyGain.text = $"Ganho de energia: {skill.GainOfEnergyPerUse}";
        //Show(desc);
        this.shortcut.text = $"Atalho [{shortcut}]";
        Resize(skill.Description);
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    private void Update()
    {
        FollowMouse();
    }
}
