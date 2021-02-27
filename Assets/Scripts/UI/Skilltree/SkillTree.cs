using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    public Gradient enabledPathColor = new Gradient();
    public Gradient disabledPathColor = new Gradient();
    private static Player player;
    private static SkillTree instance;

    public static SkillTree Instance { get => instance; set => instance = value; }

    SkillTree (){
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    private void Start() {
        player = Player.Instance;
       // EnableSkills();
    }

    public void EnableSkills (){
        Component[] components = GetComponentsInChildren(typeof(SkillTreeComponent), false);
        foreach (SkillTreeComponent item in components)
        {
            if (((Active)item.Skill) is Active && item.Skill != null && ((Active)item.Skill).LevelToLearn <= player.playerInfo.Level){
                item.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void Open () {
        GameStatus.isSkillTreeOpened = true;
        this.gameObject.SetActive(true);
    }
    public void Close () {
        this.gameObject.SetActive(false);
        GameStatus.isSkillTreeOpened = false;
    }

}
