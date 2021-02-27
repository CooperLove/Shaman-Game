using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Skills/Croc Head")]
public class CrocodileHead : Active
{
    public override void OnLearn()
    {
        Player player = Player.Instance;
        player.gameObject.AddComponent<CrocodileHeadPhysics>();
        
    }

    public override void OnUpgrade()
    {
        throw new System.NotImplementedException();
    }

    public override bool OnUse()
    {
        Player player = Player.Instance;
        player.GetComponent<CrocodileHeadPhysics>().Use();

        return true;
    }
}
