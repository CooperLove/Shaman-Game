using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Wolf/Ice Breath")]
public class IceBreath : Active
{
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
        var pi = Player.Instance.playerInfo;
        //Debug.Log($"Using ice breath");
        
        if (pi.Mana <= MpCost){
            GameAnnouncement.NotEnoughManaAnnouncement();
            return false;
        } 

        GameObject g = Resources.Load("Prefabs/Combat/Wolf/Ice Breath") as GameObject;


        if (g != null){
            GameObject iceBreath = Instantiate(g, Player.Instance.transform.position, g.transform.rotation);
        }

        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
