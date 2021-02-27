using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Wolf/Cascade")]
public class Cascade : Active
{
    [SerializeField] private float damage = 0f;
    [SerializeField] private float damagePerLevel = 0f;
    public override void OnLearn()
    {
        if (Player.Instance.playerInfo.LearnedSkills.Contains(this))
            return;
            
        CurrentLevel = 1;
        Player.Instance.playerInfo.LearnedSkills.Add(this);
    }

    public override void OnUpgrade()
    {
        if (CanUpgradeSkill()){
            damage += damagePerLevel; // Aumenta a quantidade de cura ganha
        }
    }

    public override bool OnUse()
    {
        var pi = Player.Instance.playerInfo;
        //Debug.Log($"Using Cascade");
        
        if (pi.Mana <= MpCost){
            GameAnnouncement.NotEnoughManaAnnouncement();
            return false;
        } 
        
        GameObject g = Resources.Load("Prefabs/Combat/Wolf/Cascade") as GameObject;

        
        if (g != null){
            GameObject cascade = Instantiate(g, Player.Instance.transform.position, g.transform.rotation);
        }

        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
