using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Ice Creeper")]
public class IceCreeper : Active
{
    public float energyPerUse;

    public override void OnLearn()
    {
        throw new System.NotImplementedException();
    }

    public override void OnUpgrade()
    {
        if (CanUpgradeSkill()){

        }
    }

    public override bool OnUse()
    {
        Player pi = Player.Instance;
        if (pi.playerInfo.Mana < MpCost)
            return false;;
            
        // Tenta pegar o componente da skill
        pi.TryGetComponent<IceCreeperPhysics>(out IceCreeperPhysics iceCreeper);
        // Caso não tenha, é adicionado.
        if (iceCreeper == null)
            pi.gameObject.AddComponent(typeof( IceCreeperPhysics));
        
        //Tenta pegar o componente da skill
        pi.TryGetComponent<IceCreeperPhysics>(out iceCreeper);
        // Seta a posição inicial dos fragmentos
        iceCreeper.shardsInitialPoint = pi.transform.position;
        // Inicia a coroutine da skill
        iceCreeper.StartCoroutine(iceCreeper.JadeShards());
        Debug.Log("Used Ice Creeper");

        pi.playerInfo.Mana -= MpCost;

        //Passiva
        Player player = Player.Instance;
        PassiveCrocodileSO passive = (PassiveCrocodileSO) player.GetComponent<CrocodileAttacks>().Passive;
        passive.Use(energyPerUse);

        return true;
    }

    
}
