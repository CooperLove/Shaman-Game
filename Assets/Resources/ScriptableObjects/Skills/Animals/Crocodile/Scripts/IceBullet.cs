using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Ice Bullets")]
public class IceBullet : Skill_OverTime
{
    public int damagePerProjectile;
    public int energyPerProjectile;

    public override void OnLearn()
    {
        CurrentLevel = 1;
        Player pi = Player.Instance;
        // Tenta pegar o componente da skill
        pi.TryGetComponent<IceBulletPhysics>(out IceBulletPhysics iceBullet);
        // Caso não tenha, é adicionado.
        if (iceBullet == null)
            pi.gameObject.AddComponent(typeof( IceBulletPhysics));
    }

    public override void OnUpgrade()
    {
        throw new System.NotImplementedException();
    }

    public override void Stop()
    {
        Player player = Player.Instance;
        player.GetComponent<IceBulletPhysics>().shouldSpawn = false;
    }

    public override bool OnUse()
    {
        Player pi = Player.Instance;
        if (pi.playerInfo.Mana < MpCost)
            return false;

        //Tenta pegar o componente da skill
        pi.TryGetComponent<IceBulletPhysics>(out IceBulletPhysics iceBullet);
        iceBullet.shouldSpawn = true;
        iceBullet.iceBullet = this;
        Debug.Log("Used Ice Bullet");

        Mark mark = pi.GetComponent<Mark>();
        mark.Gain(GainOfEnergyPerUse);

        pi.playerInfo.Mana -= MpCost;
        return true;
    }
}
