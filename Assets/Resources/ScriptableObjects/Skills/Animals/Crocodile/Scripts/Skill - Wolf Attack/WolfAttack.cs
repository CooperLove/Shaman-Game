using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Wolf/WolfAttack")]
public class WolfAttack : Active
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void OnLearn()
    {
        if (Player.Instance.playerInfo.LearnedSkills.Contains(this))
            return;
            
        CurrentLevel = 1;
        Player.Instance.playerInfo.LearnedSkills.Add(this);
    }

    public override void OnUpgrade()
    {
        throw new System.NotImplementedException();
    }

    public override bool OnUse()
    {
         PlayerInfo pi = Player.Instance.playerInfo;
        Debug.Log($"Using WolfAttack");
        
        if (pi.Mana <= MpCost){
            GameAnnouncement.NotEnoughManaAnnouncement();
            return false;
        } 

        GameObject g = Resources.Load("Prefabs/Combat/Wolf/Wolf Attack") as GameObject;

        Debug.Log($"{g} => WolfAttack");

        if (g != null){
            GameObject wolfAttack = Instantiate(g, Player.Instance.transform.position, g.transform.rotation);
        }

        return true;
    }
}
