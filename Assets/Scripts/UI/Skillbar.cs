using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Skillbar : MonoBehaviour
{
    public Player player;
    private static Skillbar instance;
    [SerializeField] private RectTransform learnedSkillsRect = null;
    //[SerializeField] private Transform skillbarBG = null;
    //[SerializeField] private Transform skillbar = null;
    [SerializeField] private Transform skills = null;
    [SerializeField] private float cellSize = 0;
    [SerializeField] private float cellSpace = 0;
    [SerializeField] private float topPadding = 0;
    [SerializeField] private float leftPadding = 0;
    [SerializeField] private int numSkills = 0;
    [SerializeField] private bool horizontal = true;

    public static Skillbar Instance { get => instance; set => instance = value; }

    // Start is called before the first frame update
    private void Awake() {
        player = Player.Instance;
    }
    void Start()
    {
        UpdateSkillbar();
    }

    // Update is called once per frame
    
    void Update()
    {
        //UpdateSkillbar();
        //UpdateCooldown();
        //UpdateLearnedSkillRect();
    }

    public void UpdateSkillbar (){
            
        if (skills.childCount > 4) {
            RectTransform skillRect = GetComponent<RectTransform>();
            // RectTransform bar = skillbar.GetComponent<RectTransform>();
            // RectTransform bg = skillbarBG.GetComponent<RectTransform>();
            int t = skills.childCount;
            skillRect.sizeDelta = new Vector2 (leftPadding + (t * cellSize + ((t-1) * cellSpace)), skillRect.sizeDelta.y);
            // bar.sizeDelta = new Vector2 (leftPadding + (skills.childCount * (cellSize + cellSpace)), bar.sizeDelta.y);
            // bg.sizeDelta = new Vector2 (leftPadding + (skills.childCount * (cellSize +cellSpace)), bg.sizeDelta.y);
        }
    }

    public void UpdateLearnedSkillRect (){
        int h = (learnedSkillsRect.childCount/numSkills);
        h = learnedSkillsRect.childCount%numSkills == 0 ? h : h+1;
        //Debug.Log($"{learnedSkillsRect.childCount%5} => {h}");
        learnedSkillsRect.sizeDelta = new Vector2 (learnedSkillsRect.sizeDelta.x, (h * cellSize) + (h * cellSpace) + topPadding);
    }
    public void UpdateCooldown (){
        
    }

    public void CreateSkill (Skill skill){

    }

    public void RotateSkillBar () {
        horizontal = !horizontal;
        int rot = horizontal ? 0 : 90;
        int posY = horizontal ? 65 : -65;
        float posX = horizontal ? 127.5f : -127.5f;
        int posY2 = horizontal ? -145 : 145;
        skills.localRotation = Quaternion.Euler(0,0,rot);
        skills.GetComponent<RectTransform>().localPosition = new Vector3(skills.localPosition.x, skills.localPosition.y+posY, 0);
        learnedSkillsRect.localRotation = Quaternion.Euler(0,0,-rot);
        learnedSkillsRect.localPosition = new Vector3 (learnedSkillsRect.localPosition.x+posX, learnedSkillsRect.localPosition.y+posY2, 0);
        learnedSkillsRect.GetComponent<GridLayoutGroup>().startAxis = horizontal ? GridLayoutGroup.Axis.Horizontal : GridLayoutGroup.Axis.Vertical;
        learnedSkillsRect.GetComponent<GridLayoutGroup>().startCorner = horizontal ? GridLayoutGroup.Corner.UpperLeft : GridLayoutGroup.Corner.LowerLeft;
        for (int i = 0; i < skills.childCount; i++)
        {
            skills.GetChild(i).localRotation = Quaternion.Euler(0,0,-rot);
        }
        for (int i = 0; i < learnedSkillsRect.childCount; i++)
        {
            learnedSkillsRect.transform.GetChild(i).localRotation = Quaternion.Euler(0,0,-rot);
        }
    }
}
