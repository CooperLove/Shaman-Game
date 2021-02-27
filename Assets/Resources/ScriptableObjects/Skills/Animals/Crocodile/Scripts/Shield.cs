using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Crocodile/Shield")]
public class Shield : Active
{
    public float duration;
    public float shieldAmount;

    public override void OnLearn()
    {
        Player player = Player.Instance;
        player.TryGetComponent(out ShieldMonoBehaviour shield);
        if (shield == null){
            Player.Instance.gameObject.AddComponent(typeof(ShieldMonoBehaviour));
        }

        player.TryGetComponent(out PassiveCrocodile passive);
        if (passive == null){
            player.gameObject.AddComponent(typeof(PassiveCrocodile));
        }

        if (Player.Instance.playerInfo.LearnedSkills.Contains(this))
            return;
            
        CurrentLevel = 1;
        Player.Instance.playerInfo.LearnedSkills.Add(this);
    }

    public override void OnUpgrade()
    {
        if (CanUpgradeSkill()){
            //shieldAmount += // Aumenta a quantidade de cura ganha
        }
    }

    public override bool OnUse()
    {
        PlayerInfo pi = Player.Instance.playerInfo;

        if (pi.Mana <= MpCost) 
            return false;

        Player player = Player.Instance;
        // Instancia o shield
        var shield = Resources.Load("Prefabs/Combat/Wolf/Wolf Shield") as GameObject;        

        if (shield){
            var g = Instantiate(shield, Vector3.zero, shield.transform.rotation);
            g.transform.SetParent(player.transform);
            g.transform.localPosition = Vector3.zero;
            // g.transform.localScale = Vector3.one;
            Object.Destroy(g, duration);
        }
        // Reduz a MP do player
        pi.Mana -= MpCost; 

        return true;
    }
}
