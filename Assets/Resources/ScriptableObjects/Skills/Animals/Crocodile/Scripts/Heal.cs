using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Crocodile/Heal")]
public class Heal : Active
{

    [SerializeField] private float healAmount = 0;
    [SerializeField] private float healPerLevel = 0;

    public override void OnLearn()
    {
        
            
        Player player = Player.Instance;
        player.TryGetComponent(out HealMonoBehaviour heal);
        if (heal == null){
            player.gameObject.AddComponent(typeof(HealMonoBehaviour));
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

    public override void OnUpgrade() {
        if (CanUpgradeSkill()){
            healAmount += healPerLevel; // Aumenta a quantidade de cura ganha
        }
    } 
    
    public override bool OnUse()
    {
        PlayerInfo pi = Player.Instance.playerInfo;

        if (pi.Mana <= MpCost){
            GameAnnouncement.NotEnoughManaAnnouncement();
            return false;
        } 

        var player = Player.Instance;
        
        player.TryGetComponent(out HealMonoBehaviour heal);
        heal?.Heal();

        player.TryGetComponent(out PassiveCrocodile passive);
        var passiveEnergy = !passive ? 0 : passive.CurrentEnergy;
        
        pi.Health += healAmount + passiveEnergy; // Cura o player
        pi.Mana -= MpCost; // Reduz a MP do player
        //Debug.Log($"Used heal: {healAmount+passiveEnergy} = {healAmount}+{passiveEnergy}");
        passive.CurrentEnergy = 0;
        passive?.Use(0);

        return true;
    }

    

}
