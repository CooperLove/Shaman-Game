using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Wolf/On Hit/Perfect Block Heal")]
public class PerfectBlockHeal : OnHitEffect
{
    [SerializeField] private float healPercentage = 50f;
    // private GameObject damageText = Resources.Load("Prefabs/Combat/DamageText") as GameObject;

    public override void OnLearn()
    {
        // if (Learned)
        //     return;

        Initialize();

        player.TryGetComponent(out BlockAttack blockAttack);

        if (!blockAttack)
            return;

        blockAttack.AddEffect((Action<int>)OnHit);

        AddOneTimeEffects();

        CurrentLevel = 1;
        Learned = true;
    }
    public void OnHit (int damage){
        var damageHealed = (int)((damage / 100f) * healPercentage);
        playerInfo.Health += damageHealed;

        var damageText = Resources.Load("Prefabs/Combat/DamageText") as GameObject;
        var text = Instantiate(damageText, player.transform.position, damageText.transform.rotation);

        if (!text)
            return;

        Debug.Log($"Block Heal {damage} => {damageHealed}");
        text.GetComponent<DamageText>().SetText($"+{damageHealed}", Color.green, 0.25f, 1);
    }

    public override void OnUpgrade()
    {
        if (!CanUpgradeSkill())
            return;

        healPercentage += 10f;
    }

    protected override void AddOneTimeEffects(){}
    public override void OnHit(){}
    public override void OnHit(Character info){}
}
