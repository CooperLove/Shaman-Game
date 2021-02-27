using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Wolf/Freezing Slash")]
public class FreezingSlash : ChargeableSkill
{

    private FreezingSlashMechanic mechanic = null;
    public override void OnLearn()
    {
        Initialize();
     
        if (Player.Instance.playerInfo.LearnedSkills.Contains(this))
            return;
            
        CurrentLevel = 1;
        Player.Instance.playerInfo.LearnedSkills.Add(this);
    }

    public override void OnUpgrade()
    {
        
    }

    public override bool OnUse()
    {
        Debug.Log($"Begin charging");
        ChargeAmount = 0;
        Player player = Player.Instance;
        PlayerInfo pi = Player.Instance.playerInfo;

        if (pi.Mana < MpCost)
            return false;

        var charge = Resources.Load("Prefabs/Combat/Wolf/Freezing Slash - Charge") as GameObject;

        if (charge){
            var g = Instantiate(charge, player.transform.position, charge.transform.rotation) as GameObject;
            mechanic = g.GetComponent<FreezingSlashMechanic>();
        }

        return true;
    }

    public override bool OnCharging()
    {
        ChargeAmount += ChargingRate * Time.deltaTime;
        Debug.Log($"Charging");

        return true;
    }

    public override bool OnEndCharging()
    {
        Debug.Log($"OnEndCharging - Damage {Mathf.Lerp(minDamage, maxDamage, ChargeAmount/100f)}");
        ChargeAmount = 0;

        Player player = Player.Instance;

        var slash = Resources.Load("Prefabs/Combat/Wolf/Freezing Slash") as GameObject;
        var usageFX = Resources.Load("Prefabs/Combat/Wolf/Freezing Slash Usage Fx") as GameObject;

        if (slash){
            var g = Instantiate(slash, player.transform.position, slash.transform.rotation) as GameObject;
            g.transform.localScale = Player.Instance.IsFacingRight ? Vector3.one : new Vector3(-1, 1, 1);
        }

        if (usageFX){
            var g = Instantiate(usageFX, player.transform.position, usageFX.transform.rotation) as GameObject;
            g.transform.localScale = Player.Instance.IsFacingRight ? Vector3.one : new Vector3(-1, 1, 1);
            int size = g.transform.childCount;
            for (int i = 0; i < size; i++)
            {
                g.transform.GetChild(i).localScale = Player.Instance.IsFacingRight ? Vector3.one : new Vector3(-1, 1, 1);
            }
        }

        mechanic?.StopParticles();
        mechanic = null;

        return true;
    }

    
}
