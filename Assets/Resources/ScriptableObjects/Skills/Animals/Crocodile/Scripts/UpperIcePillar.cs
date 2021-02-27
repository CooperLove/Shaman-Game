using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Upper Ice Pillar")]
public class UpperIcePillar : Active
{
    public float energyPerUse;

    public override void OnLearn()
    {
        throw new System.NotImplementedException();
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
            
        // Tenta pegar o componente da skill
        pi.TryGetComponent<UpperIcePillarPhysics>(out UpperIcePillarPhysics iceCreeper);
        // Caso não tenha, é adicionado.
        if (iceCreeper == null)
            pi.gameObject.AddComponent(typeof( UpperIcePillarPhysics));
        
        //Tenta pegar o componente da skill
        pi.TryGetComponent<UpperIcePillarPhysics>(out iceCreeper);
        // Inicia a skill
        iceCreeper.IceCollumn();
        Debug.Log("Used Upper Ice Pillar");

        pi.playerInfo.Mana -= MpCost;

        //Passiva
        Player player = Player.Instance;
        PassiveCrocodileSO passive = (PassiveCrocodileSO) player.GetComponent<CrocodileAttacks>().Passive;
        passive.Use(energyPerUse);

        return true;
    }
}
