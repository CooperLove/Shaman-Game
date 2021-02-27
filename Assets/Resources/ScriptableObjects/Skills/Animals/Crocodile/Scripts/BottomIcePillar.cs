using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Bottom Ice Pillar")]
public class BottomIcePillar : Active
{
    public float energyPerUse;

    public override void OnLearn()
    {
        Player pi = Player.Instance;
        // Tenta pegar o componente da skill
        pi.TryGetComponent<BottomIcePillarPhysics>(out BottomIcePillarPhysics iceCreeper);
        // Caso não tenha, é adicionado.
        if (iceCreeper == null)
            pi.gameObject.AddComponent(typeof( BottomIcePillarPhysics));
    }

    public override void OnUpgrade()
    {
        throw new System.NotImplementedException();
    }

    public override bool OnUse()
    {
        Player pi = Player.Instance;
        if (pi.playerInfo.Mana < MpCost)
            return false;;
            
        Mark mark = pi.GetComponent<Mark>();
        mark.Gain(GainOfEnergyPerUse);
        //Tenta pegar o componente da skill
        pi.TryGetComponent<BottomIcePillarPhysics>(out BottomIcePillarPhysics iceCreeper);
        // Seta a posição inicial dos fragmentos
        iceCreeper.shardsInitialPoint = pi.transform.position;
        // Inicia a coroutine da skill
        iceCreeper.StartCoroutine(iceCreeper.IceShards());
        Debug.Log("Used Bottom Ice Pillar");

        pi.playerInfo.Mana -= MpCost;

        //Passiva
        Player player = Player.Instance;
        PassiveCrocodileSO passive = (PassiveCrocodileSO) player.GetComponent<CrocodileAttacks>().Passive;
        passive.Use(energyPerUse);

        return true;
    }

   
}
