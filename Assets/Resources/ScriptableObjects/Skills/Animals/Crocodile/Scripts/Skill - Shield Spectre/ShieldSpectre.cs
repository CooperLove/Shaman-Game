using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Wolf/ShieldSpectre")]
public class ShieldSpectre : Active
{
    // Start is called before the first frame update
    [SerializeField] private int damage;
    public int Damage { get => damage; set => damage = value; }

    [SerializeField] private float slowPercentage;
    public float SlowPercentage { get => slowPercentage; set => slowPercentage = value; }
    
    [SerializeField] private float slowDuration;
    public float SlowDuration { get => slowDuration; set => slowDuration = value; }

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
        Debug.Log($"Using ShieldSpectre");
        
        if (pi.Mana <= MpCost){
            GameAnnouncement.NotEnoughManaAnnouncement();
            return false;
        } 

        GameObject g = Resources.Load("Prefabs/Combat/Wolf/Shield Spectre") as GameObject;

        Debug.Log($"{g} => ShieldSpectre");

        if (g != null){
            GameObject shieldSpectre = Instantiate(g, Player.Instance.transform.position, g.transform.rotation);
        }

        return true;
    }
}
