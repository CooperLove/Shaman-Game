using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class SkillTreeComponent : MonoBehaviour
{
    [SerializeField] private UpgradableSkill skill;
    [SerializeField] private List<SkillTreeComponent> parents = new List<SkillTreeComponent>();
    [SerializeField] private List<SkillTreeComponent> children = new List<SkillTreeComponent>();
    [SerializeField] private int currentLevel = 0;
    [SerializeField] private int playerLevelToUnlockMe = 0;
    [SerializeField] private List<int> parentLevelToUnlockMe = new List<int>();
    [SerializeField] private Image border = null;
    public Vector3 startTangent, endTangent = new Vector3(-5, 15, 0);
    public int CurrentLevel { get => currentLevel; set => currentLevel = value; }
    public List<SkillTreeComponent> Parents { get => children; set => children = value; }
    public List<SkillTreeComponent> Children { get => parents; set => parents = value; }
    public UpgradableSkill Skill { get => skill; set => skill = value; }

    private void Awake() {
        border = transform.parent.parent.GetComponent<Image>();
    }
    private void Start() {
        
        currentLevel = skill.CurrentLevel;
        if (skill is Active)
            playerLevelToUnlockMe = ((Active)skill).LevelToLearn;
        
        if (skill is Passive){
            foreach (SkillTreeComponent parent in parents)
                parentLevelToUnlockMe.Add(1);

        }
        
        BuildPath();

    }

    private void BuildPath () {
        if (children.Count == 0)
            return;

        int length = transform.childCount;
        for (int i = 0; i < length; i++)
        {
            if (i >= children.Count)
                return;
            Transform child = transform.GetChild(i);
            LineRenderer lr = child.GetComponent<LineRenderer>();
            if (children[i] == null)
                Debug.Log($"----- {transform.parent.parent.name}");
            lr.colorGradient = children[i].GetComponent<Button>().interactable ? 
                                SkillTree.Instance.enabledPathColor : SkillTree.Instance.disabledPathColor;

            // lr.SetPosition(0, new Vector3(transform.position.x, transform.position.y, 0));
            RectTransform rect = children[i].transform.parent.parent.GetComponent<RectTransform>();
            RectTransform myRect = transform.parent.parent.GetComponent<RectTransform>();

            Vector3 pos = rect.anchoredPosition;
            Vector3 myPos = myRect.anchoredPosition;

            lr.SetPosition(0, new Vector3(0, 0, -5));
            lr.SetPosition(1, new Vector3(pos.x + (myPos.x < 0 ? Mathf.Abs(myPos.x) : -Mathf.Abs(myPos.x)),
                                          pos.y + (myPos.y < 0 ? Mathf.Abs(myPos.y) : -Mathf.Abs(myPos.y)), -5));
        }
    }

    public void OnSkillUpgrade (){
        //Debug.Log($"OnUpgrade {skill.GetType()} {skill.name}");
        if (skill is Passive ){
            PassiveUpgrade();
        }else {
            ActiveUpgrade();
        }
    }

    private bool CanBeUpgraded (){
        int i = 0;
        foreach (SkillTreeComponent parent in parents){
            if (parent.currentLevel < parentLevelToUnlockMe[i++]){
                return false;
            }
        }
        return true;
    }

    private void PassiveUpgrade (){
        PlayerInfo pi = Player.Instance.playerInfo;
        
        if (pi.PassiveSkillPoints <= 0 || skill.CurrentLevel >= skill.MaxLevel)
            return;

        currentLevel +=1;
        GetComponent<MultiplePassive>().OnLearn();

        int i = 0;
        foreach (SkillTreeComponent child in children){
            if (child.CanBeUpgraded()){
                foreach (SkillTreeComponent parent in child.parents)
                    parent.EnableMultiplePath(child);
                
                child.GetComponent<Button>().interactable = true;
                LineRenderer lr = transform.GetChild(i).GetComponent<LineRenderer>();
                lr.colorGradient = SkillTree.Instance.enabledPathColor;
            }
            i++;
        }
        border.color = Color.green;
        pi.PassiveSkillPoints -= 1;
        return;
    }

    private void EnableMultiplePath (SkillTreeComponent child){
        int index = children.IndexOf(child);
        Gradient disabledColor = SkillTree.Instance.disabledPathColor;
        Gradient pathColor = transform.GetChild(index).GetComponent<LineRenderer>().colorGradient;
        if (pathColor.Equals(disabledColor)){
            transform.GetChild(index).GetComponent<LineRenderer>().colorGradient = SkillTree.Instance.enabledPathColor;
        } 
    }

    private void ActiveUpgrade (){
        PlayerInfo pi = Player.Instance.playerInfo;

        if (skill == null || currentLevel >= skill.MaxLevel)
            return;
        
        if (currentLevel == 0){
            PlayerLearnedSkills.Instance.OnLearnSkill((Active)Skill);
        }

        if (pi.ActiveSkillPoints >= Skill.PointsPerLevel){
            ((Active)Skill).OnUpgrade();
            currentLevel++;
            // Update the button text dependent.Skill.LevelToLearn <= Player.Instance.playerInfo.Level &&
            
            foreach (SkillTreeComponent child in children)
            {
                if (child.CanBeUpgraded()){
                    child.GetComponent<Button>().interactable = true;
                    child.transform.GetChild(0).GetComponent<LineRenderer>().colorGradient = SkillTree.Instance.enabledPathColor;
                }
            }
        }
    }

    private void OnDrawGizmos() {
        #if UNITY_EDITOR
        Handles.Label(new Vector3(transform.position.x, transform.position.y, transform.position.z), 
                                transform.parent.parent.GetSiblingIndex().ToString());
        #endif

        foreach (SkillTreeComponent child in children){
            if (child == null)
                continue;
            Gizmos.color = child.GetComponent<Button>().interactable ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position, child.transform.position);
        }

        foreach (SkillTreeComponent parent in parents)
        {
            if (parent == null)
                continue;
        
        #if UNITY_EDITOR
            Handles.DrawBezier(transform.position, parent.transform.position,
                                transform.position + startTangent, 
                                transform.position + new Vector3(-5, 15, 0), 
                                Color.cyan, null, 2f);
        #endif
        }
    }
}
