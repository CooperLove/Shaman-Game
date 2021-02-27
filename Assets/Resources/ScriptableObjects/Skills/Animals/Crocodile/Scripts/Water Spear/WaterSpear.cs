using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Wolf/Water Spear")]
public class WaterSpear : Active
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
        //Debug.Log($"Using Geyser");
        
        if (pi.Mana < MpCost){
            GameAnnouncement.NotEnoughManaAnnouncement();
            return false;
        } 

        var g = Resources.Load("Prefabs/Combat/Wolf/Water Spear") as GameObject;


        if (g){
            var geyser = Instantiate(g, Player.Instance.transform.position, g.transform.rotation);
        }

        return true;
    }
}
