using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Wolf/On Hit/Armor Shred")]
public class ArmorShred : OnHitEffect
{
    private List<KeyValuePair<Enemy, int>> enemies = new List<KeyValuePair<Enemy, int>>();
    [SerializeField] private float shredPercentage = 1f;


    public override void OnLearn()
    {
        // if (Learned)
        //     return;

        Debug.Log($"Aprendendo Armor Shred");

        BasicAttack_Handler.Instance.AddOnHitEffect ((Action<Character>) OnHit);
        AddOneTimeEffects();

        CurrentLevel = 1;

        Learned = true;
    }
    public override void OnHit(){}

    public override void OnHit(Character info)
    {
        if (info is Player){
            playerInfo.Armor = (playerInfo.Armor / 100f) * (100f - shredPercentage);
            return;
        }

        if (enemies.FirstOrDefault(x => x.Key.Equals(info)).Key == null)
            enemies.Add(new KeyValuePair<Enemy, int>((Enemy) info, 0));

        var size = enemies.Count;
        for (int i = size - 1; i >= 0 ; i--)
        {
            if (enemies[i].Key == null){
                enemies.RemoveAt(i);
                continue;
            }

            if (enemies[i].Value == 5)
                continue;

            enemies[i].Key.Armor = (enemies[i].Key.Armor / 100f) * (100f - shredPercentage);
            Debug.Log($"Reduzindo a armadura de {enemies[i].Key.name} para {enemies[i].Key.Armor}");
            enemies[i] = new KeyValuePair<Enemy, int>(enemies[i].Key, enemies[i].Value + 1);
        }
    }

    public override void OnUpgrade()
    {
        if (!CanUpgradeSkill())
            return;

        shredPercentage += 1;
    }

    protected override void AddOneTimeEffects()
    {
        playerInfo.BasicAttackDamagekMultiplier = (playerInfo.BasicAttackDamagekMultiplier / 100f) * 90f;
        Debug.Log($"Player now does {playerInfo.BasicAttackDamagekMultiplier * 100f}% damage on basic attacks");
    }
}
